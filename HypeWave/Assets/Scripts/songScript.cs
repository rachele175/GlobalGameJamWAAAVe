using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum noteColor { White, Yellow, Red, Green };

public class songScript : MonoBehaviour {


    public AudioSource mySong;
    public float BPMNoteScrollTime;


    int noteCount;
    public float timeStamp;


    public List<noteColor> notesList;

    public List<float> noteTimes;


	// Use this for initialization
	void Start () {
        noteCount = 0;
        timeStamp = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (mySong.isPlaying)
        {
            timeStamp += Time.deltaTime;
        }
        if (noteCount < notesList.Count)
        {
            if (timeStamp >= noteTimes[noteCount])
            {
                Debug.Log(timeStamp);
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("songDisplays"))
                {
                    g.GetComponent<songDisplayManager>().spawnNote(notesList[noteCount]);

                }
                noteCount++;
            }
        }
	}



}

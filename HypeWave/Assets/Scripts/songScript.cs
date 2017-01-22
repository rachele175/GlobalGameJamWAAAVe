using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum noteColor { White, Yellow, Red, Green };

public class songScript : MonoBehaviour {

    public float delayGoal;
    public float delayTime;

    public AudioSource mySong;
    public float BPMNoteScrollTime;


    int noteCount;
    public float timeStamp;


    private List<noteColor> notesList= new List<noteColor>();

    private List<float> noteTimes= new List<float>();


	// Use this for initialization
	void Start () {
        noteCount = 0;
        timeStamp = 0;

        StreamReader file = new StreamReader("FreeBirdRecord.txt");

        while (!file.EndOfStream)
        {
            Debug.Log("attempted");
            string line = file.ReadLine();
            // Do Something with the input.
            string[] nums = line.Split(' ');
            noteTimes.Add(float.Parse(nums[0]));
            notesList.Add((noteColor)int.Parse(nums[1]));
        }

        file.Close();

    }
	
	// Update is called once per frame
	void Update () {
        delayTime += Time.deltaTime;
        if (delayTime>= delayGoal && !mySong.isPlaying)
        {
            mySong.Play();
            Debug.Log("PLAYING");
        }


        timeStamp += Time.deltaTime;
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

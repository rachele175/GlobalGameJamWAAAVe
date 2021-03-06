﻿using System.Collections;
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


    public string fileName;

    private List<noteColor> notesList= new List<noteColor>();

    private List<float> noteTimes= new List<float>();

    public bool playSong = false;
    public bool playingGame = false;
	// Use this for initialization
	void Start () {
        noteCount = 0;
        timeStamp = 0;

        if (playSong)
        {
            StreamReader file = new StreamReader(fileName);

            while (!file.EndOfStream)
            {
                //Debug.Log("attempted");
                string line = file.ReadLine();
                // Do Something with the input.
                string[] nums = line.Split(' ');
                noteTimes.Add(float.Parse(nums[0]) - 3 - delayGoal);
                notesList.Add((noteColor)int.Parse(nums[1]));
            }

            file.Close();
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (playingGame)
        {
            delayTime += Time.deltaTime;
            if (delayTime >= delayGoal && !mySong.isPlaying)
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



}

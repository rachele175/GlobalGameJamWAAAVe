using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePrefab : MonoBehaviour {
    float noteSpeed = -.03f;
    noteColor myColor;
    songDisplayManager myManager;
    public string playerID;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position+=(new Vector3(noteSpeed, 0f, 0f));
	}

    public void setUp(noteColor g, songDisplayManager manage, string playerNum)
    {
        myColor = g;
        myManager = manage;
        playerID = playerNum;
    }

    public noteColor getMyColor()
    {
        return myColor;
    }
}

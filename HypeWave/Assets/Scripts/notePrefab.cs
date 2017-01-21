using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePrefab : MonoBehaviour {
    float noteSpeed = -.02f;
    noteColor myColor;
    songDisplayManager myManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(new Vector3(noteSpeed, 0f, 0f));
	}

    public void setUp(noteColor g, songDisplayManager manage)
    {
        myColor = g;
        myManager = manage;
    }

    public noteColor getMyColor()
    {
        return myColor;
    }
}

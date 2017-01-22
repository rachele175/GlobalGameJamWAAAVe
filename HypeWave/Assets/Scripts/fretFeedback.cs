using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fretFeedback : MonoBehaviour {

    public static byte noteAlpha=255;
    Color32 defaultCol = new Color32(255, 255, 255, noteAlpha);
    Color32 green = new Color32(0, 255, 12, noteAlpha);
    Color32 red = new Color32(231, 0, 0, noteAlpha);
    Color32 yellow = new Color32(230, 255, 0, noteAlpha);
    Color32 white = new Color32(129, 211, 255, noteAlpha);
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void press(noteColor c)
    {

        if (c == noteColor.White)
        {
            GetComponent<SpriteRenderer>().color = white;
        }
        else if (c == noteColor.Yellow)
        {
            GetComponent<SpriteRenderer>().color = yellow;
        }
        else if (c == noteColor.Red)
        {
            GetComponent<SpriteRenderer>().color = red;
        }
        else if (c == noteColor.Green)
        {
            GetComponent<SpriteRenderer>().color = green;
        }
    }

    public void release()
    {
        GetComponent<SpriteRenderer>().color = defaultCol;

    }
}

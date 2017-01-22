using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePrefab : MonoBehaviour {
    float noteSpeed = -.03f;
    noteColor myColor;
    songDisplayManager myManager;
    public string playerID;
    public SpriteRenderer noteSprite;
    static byte noteAlpha = 200;
    Color32 green= new Color32(0,255,12,noteAlpha);
    Color32 red = new Color32(231, 0, 0, noteAlpha);
    Color32 yellow = new Color32(230, 255, 0, noteAlpha);
    Color32 white = new Color32(129, 211, 255, noteAlpha);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition+=(new Vector3(noteSpeed, 0f, 0f));
	}

    public void setUp(noteColor g, songDisplayManager manage, string playerNum)
    {
        myColor = g;
        myManager = manage;
        playerID = playerNum;
        switch ((int)g) {
            case 0:
                noteSprite.color = white;
                break;
            case 1:
                noteSprite.color = yellow;
                break;
            case 2:
                noteSprite.color = red;
                break;
            case 3:
                noteSprite.color = green;
                break;
        }

    }

    public noteColor getMyColor()
    {
        return myColor;
    }

    public void setColor(noteColor g)
    {
        switch ((int)g)
        {
            case 0:
                noteSprite.color = white;
                break;
            case 1:
                noteSprite.color = yellow;
                break;
            case 2:
                noteSprite.color = red;
                break;
            case 3:
                noteSprite.color = green;
                break;
        }
    }
}

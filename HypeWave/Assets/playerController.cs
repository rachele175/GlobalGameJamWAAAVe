using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	Vector3 moveDirection;
	public string controllerNumber;
	public songDisplayManager myDisplay; //this is the fret board
    public songDisplayManager songDisplayManagerPrefab;

    public CrowdPlayer crowdPlayer; //this is the player object in the crowd

    public Renderer debugRenderer;

    void Start()
	{
		Debug.Log(controllerNumber);

        //myDisplay = Instantiate(songDisplayManagerPrefab);
        crowdPlayer.controller = this;
    }

	void Update()
	{
		//use the wave with rb
		if (Input.GetButtonDown(controllerNumber + "Wave")) {
			Debug.Log(controllerNumber + " pressed rb");
            //call wave function
            crowdPlayer.CreateWave();
		}
		//x = rhythm game
		if (Input.GetButtonDown(controllerNumber + "NoteBlue")) {
			Debug.Log(controllerNumber + " pressed x");
			//call rhythm game functions
			if (myDisplay) myDisplay.strikeNote(noteColor.Blue);
		}
		//y = rhythm game
		if (Input.GetButtonDown(controllerNumber + "NoteYellow")) {
			Debug.Log(controllerNumber + " pressed y");
            //call rhythm game functions
            if (myDisplay) myDisplay.strikeNote(noteColor.Yellow);
        }
        //a = rhythm game
        if (Input.GetButtonDown(controllerNumber + "NoteOrange")) {
			Debug.Log(controllerNumber + " pressed a");
            //call rhythm game functions
            if (myDisplay) myDisplay.strikeNote(noteColor.Orange);
        }
        //b = rhythm game
        if (Input.GetButtonDown(controllerNumber + "NoteRed")) {
			Debug.Log(controllerNumber + " pressed b");
            //call rhythm game functions
            if (myDisplay) myDisplay.strikeNote(noteColor.Red);
        }
    }

    public Vector2 GetInput()
    {
        return Vector3.right * Input.GetAxis(controllerNumber + "Horizontal") + Vector3.up * Input.GetAxis(controllerNumber + "Vertical");
    }


    public void PlayerDied()
    {
        // TODO
        debugRenderer.enabled = false;
    }

    public void PlayerRespawned()
    {
        // TODO
        debugRenderer.enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	Vector3 moveDirection;
	public string controllerNumber;
	private songDisplayManager myDisplay; //this is the fret board

    bool strummed;

    public songDisplayManager songDisplayManagerPrefab;


    public CrowdPlayer crowdPlayer; //this is the player object in the crowd

    public Renderer debugRenderer;

    bool whitePressed;
    bool redPressed;
    bool greenPressed;
    bool yellowPressed;

    void Start()
	{
		Debug.Log(controllerNumber);
        strummed = false;
        myDisplay = Instantiate(songDisplayManagerPrefab);
        myDisplay.assignPlayerID(controllerNumber);
        crowdPlayer.controller = this;

        myDisplay.noMoreHype += crowdPlayer.Die;
    }

	void Update()
	{
        myDisplay.transform.position = transform.position + Vector3.right * 2 + Vector3.forward * 2;
        crowdPlayer.speed = crowdPlayer.minSpeed + (crowdPlayer.maxSpeed - crowdPlayer.minSpeed) * Mathf.InverseLerp(myDisplay.minHype, myDisplay.maxHype, myDisplay.hypeNumber);

        //use the wave with rb
        if (Input.GetButtonDown(controllerNumber + "Wave")) {
			Debug.Log(controllerNumber + " pressed rb");
            //call wave function
            crowdPlayer.CreateWave();
        }

        //Use THE STRUM BAR with RB
        if(Input.GetAxis(controllerNumber + "Strum") >= .95)
        {
            if (!strummed)
            {
                Debug.Log(controllerNumber + " pressed STRUM");
                myDisplay.strumFeedback();
                if (whitePressed)
                {
                    myDisplay.strikeNote(noteColor.White);

                }
                if (redPressed)
                {
                    myDisplay.strikeNote(noteColor.Red);

                }
                if (yellowPressed)
                {
                    myDisplay.strikeNote(noteColor.Yellow);

                }
                if (greenPressed)
                {
                    myDisplay.strikeNote(noteColor.Green);

                }
                strummed = true;
            }
        }
        else
        {
            strummed = false;
        }

		//RB = rhythm game
		if (Input.GetButton(controllerNumber + "NoteWhite")) {
			Debug.Log(controllerNumber + " pressed RB");
			//call rhythm game functions
            myDisplay.pressFret(noteColor.White);
            whitePressed = true;

        }
        else
        {
            myDisplay.releaseFret(noteColor.White);
            whitePressed = false;

        }

        //Y = rhythm game
        if (Input.GetButton(controllerNumber + "NoteYellow")) {
			Debug.Log(controllerNumber + " pressed y");
            //call rhythm game functions
            yellowPressed = true;
            myDisplay.pressFret(noteColor.Yellow);
        }
        else
        {
            myDisplay.releaseFret(noteColor.Yellow);
            yellowPressed = false;

        }

        //A = rhythm game
        if (Input.GetButton(controllerNumber + "NoteGreen")) {
			Debug.Log(controllerNumber + " pressed A");
            //call rhythm game functions
            myDisplay.pressFret(noteColor.Green);
            greenPressed = true;

        }
        else
        {
            myDisplay.releaseFret(noteColor.Green);
            greenPressed = false;

        }

        //B = rhythm game
        if (Input.GetButton(controllerNumber + "NoteRed")) {
			Debug.Log(controllerNumber + " pressed B");
            //call rhythm game functions
            myDisplay.pressFret(noteColor.Red);
            redPressed = true;

        }
        else
        {
            myDisplay.releaseFret(noteColor.Red);
            redPressed = false;
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

    public songDisplayManager getManager()
    {
        return myDisplay;
    }
}

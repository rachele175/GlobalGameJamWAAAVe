using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	Vector3 moveDirection;
	public string controllerNumber;
	public songDisplayManager myDisplay; //this is the fret board

    bool strummed;

   // public songDisplayManager songDisplayManagerPrefab;


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
        //songDisplayManagerInstance = Instantiate(songDisplayManagerPrefab);
        //myDisplay = Instantiate(songDisplayManagerPrefab);
        crowdPlayer.controller = this;

    }

	void Update()
	{


		//use the wave with rb
		if (Input.GetButton("j" + controllerNumber + "Wave")) {
			Debug.Log("j" + controllerNumber + " pressed rb");
            //call wave function
            crowdPlayer.CreateWave();
        }

        //Use THE STRUM BAR
        if(Input.GetAxis("j" + controllerNumber + "Strum") >= .95)
        {
            if (!strummed)
            {
                Debug.Log("j" + controllerNumber + " pressed STRUM");
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

		//x = rhythm game

		if (Input.GetButton("j" + controllerNumber + "NoteWhite")) {
			Debug.Log("j" + controllerNumber + " pressed RB");
			//call rhythm game functions
            myDisplay.pressFret(noteColor.White);
            whitePressed = true;

        }
        else
        {
            myDisplay.releaseFret(noteColor.White);
            whitePressed = false;

        }

        //y = rhythm game
        if (Input.GetButton("j" + controllerNumber + "NoteYellow")) {
			Debug.Log("j" + controllerNumber + " pressed y");
            //call rhythm game functions
            yellowPressed = true;
            myDisplay.pressFret(noteColor.Yellow);
        }
        else
        {
            myDisplay.releaseFret(noteColor.Yellow);
            yellowPressed = false;

        }
        //a = rhythm game
        if (Input.GetButton("j" + controllerNumber + "NoteGreen")) {
			Debug.Log("j" + controllerNumber + " pressed A");
            //call rhythm game functions
            myDisplay.pressFret(noteColor.Green);
            greenPressed = true;

        }
        else
        {
            myDisplay.releaseFret(noteColor.Green);
            greenPressed = false;

        }
        //b = rhythm game
        if (Input.GetButton("j" + controllerNumber + "NoteRed")) {
			Debug.Log("j" + controllerNumber + " pressed B");
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
}

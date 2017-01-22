using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

	Vector3 moveDirection;
	public int controllerNumber;
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
		Debug.Log("j" + controllerNumber);
        strummed = false;
        myDisplay = Instantiate(songDisplayManagerPrefab);
        myDisplay.assignPlayerID("j" + controllerNumber);
        crowdPlayer.controller = this;

        myDisplay.noMoreHype += crowdPlayer.Die;

    }

	void Update()
	{
        /*
        Vector3 outward = (myScreenPosition - center).normalized;
        */

        float constDist = 5;

        Vector3 myScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Vector2 vector = myScreenPosition - center;
        vector.Normalize();

        float angle = Mathf.Atan2(vector.y, vector.x);

        float x = Mathf.Clamp(Mathf.Cos(angle) * Screen.width + Screen.width / 2, 0.0f, Screen.width);
        float y = Mathf.Clamp(Mathf.Sin(angle) * Screen.height + Screen.height / 2, 0.0f, Screen.height);

        Vector3 outward = (new Vector3(x,y,center.z) - center);
        Vector3 screenEdgePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, constDist) - outward/3.5f);// myScreenPosition + outward * 100);// transform.position + Vector3.right + Vector3.forward * 1.4f + Vector3.up;
        Vector3 closeFollowPos = Camera.main.ScreenToWorldPoint(new Vector3(myScreenPosition.x, myScreenPosition.y, constDist) + outward.normalized*100);// myScreenPosition + outward * 100);// transform.position + Vector3.right + Vector3.forward * 1.4f + Vector3.up;

        if(Vector3.Distance(closeFollowPos, center) > Vector3.Distance(screenEdgePos, center))
        {
            myDisplay.targetPosition = closeFollowPos;
        }
        else
        {
            myDisplay.targetPosition = closeFollowPos;
        }

        crowdPlayer.speed = crowdPlayer.minSpeed + (crowdPlayer.maxSpeed - crowdPlayer.minSpeed) * Mathf.InverseLerp(myDisplay.minHype, myDisplay.maxHype, myDisplay.hypeNumber);

        //use the wave with rb
        if (Input.GetButtonDown("j" + controllerNumber + "Wave")) {
			Debug.Log("j" + controllerNumber + " pressed rb");
            //call wave function
            crowdPlayer.CreateWave();
        }

        //Use THE STRUM BAR with RB
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

		//RB = rhythm game
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

        //Y = rhythm game
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

        //A = rhythm game
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

        //B = rhythm game
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
        return Vector3.right * Input.GetAxis("j" + controllerNumber + "Horizontal") + Vector3.up * Input.GetAxis("j" + controllerNumber + "Vertical");
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
        myDisplay.hypeNumber=0;
    }

    public songDisplayManager getManager()
    {
        return myDisplay;
    }
}

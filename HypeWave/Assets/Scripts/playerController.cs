using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class playerController : MonoBehaviour {
    List<float> strumTestTimes= new List<float>();
    List<int> noteTests = new List<int>();
    private string filename;
    public bool creatingSong;
	Vector3 moveDirection;
	public int controllerNumber;
	private songDisplayManager myDisplay; //this is the fret board

    bool strummed;

    public songDisplayManager songDisplayManagerPrefab;

	//visual shit
	public GameObject[] characters;
	GameObject myCharacter;

    public CrowdPlayer crowdPlayer; //this is the player object in the crowd


    //public Renderer debugRenderer;

    bool whitePressed;
    bool redPressed;
    bool greenPressed;
    bool yellowPressed;

    public Spotlight spotlightPrefab;
    private Spotlight spotlight;

    public bool isKOd;
    void Start()
	{
		Debug.Log("j" + controllerNumber);
        strummed = false;
        myDisplay = Instantiate(songDisplayManagerPrefab);
        myDisplay.assignPlayerID("j" + controllerNumber);
        crowdPlayer.controller = this;

		//instantiate the character visual
		myCharacter = Instantiate(characters[(controllerNumber-1)]);
		myCharacter.transform.SetParent(gameObject.transform);
		myCharacter.transform.localPosition = Vector3.zero; //do I need to do this part?
		myCharacter.transform.rotation = Quaternion.AngleAxis(40f, Vector3.right);

        spotlight = Instantiate(spotlightPrefab);
        spotlight.target = this.transform;

        if (!creatingSong)
        {
            myDisplay.noMoreHype += crowdPlayer.Die;
        }

        filename = FindObjectOfType<songScript>().fileName;

        SetDisplayPos(true);
    }

    private void SetDisplayPos(bool snap)
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

        Vector3 outward = (new Vector3(x, y, center.z) - center);
        Vector3 screenEdgePos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, constDist) - outward / 3.5f);// myScreenPosition + outward * 100);// transform.position + Vector3.right + Vector3.forward * 1.4f + Vector3.up;
        Vector3 closeFollowPos = Camera.main.ScreenToWorldPoint(new Vector3(myScreenPosition.x, myScreenPosition.y, constDist) + outward.normalized * 100);// myScreenPosition + outward * 100);// transform.position + Vector3.right + Vector3.forward * 1.4f + Vector3.up;

        if (Vector3.Distance(closeFollowPos, center) > Vector3.Distance(screenEdgePos, center))
        {
            if (snap)
            {
                myDisplay.transform.position = closeFollowPos;
            }
            else
            { 
                myDisplay.targetPosition = closeFollowPos;
            }
        }
        else
        {
            if (snap)
            {
                myDisplay.transform.position = closeFollowPos;
            }
            else
            {
                myDisplay.targetPosition = closeFollowPos;
            }
        }

    }

    void Update()
	{
        SetDisplayPos(false);

        Rect myDisplayScreenRect = new Rect(Camera.main.WorldToScreenPoint(myDisplay.targetPosition), new Vector2(100, 100));

        bool isOccluding = false;

        if (!isKOd)
        {
            foreach (playerController player in FindObjectsOfType<playerController>())
            {
                if (myDisplayScreenRect.Contains(player.transform.position))
                {
                    myDisplay.setTransparency(.8f);
                    isOccluding = true;
                    break;
                }
            }

            if (!isOccluding)
            {
                myDisplay.setTransparency(1f);
            }
        }
        crowdPlayer.speed = crowdPlayer.minSpeed + (crowdPlayer.maxSpeed - crowdPlayer.minSpeed) * Mathf.InverseLerp(myDisplay.minHype, myDisplay.maxHype, myDisplay.hypeNumber);

        //use the wave with rb
        if (Input.GetButtonDown("j" + controllerNumber + "Wave")) {
			Debug.Log("j" + controllerNumber + " pressed rb");
            //call wave function
            if (myDisplay.hypeNumber > 4)
            {
                if (crowdPlayer.CreateWave())
                {
                    myDisplay.hypeNumber -= 5;
                    //AkSoundEngine.PostEvent("Play_CrowdWaveTrigger", waveSound.gameObject);
                }
            }

        }

        //Use THE STRUM BAR with RT
        if(Input.GetAxis("j" + controllerNumber + "Strum") >= .95)
        {
            /*
            if (!strummed)
            {
                Debug.Log("j" + controllerNumber + " pressed STRUM");

                if (creatingSong)
                {
                    int note = 3;
                    if(whitePressed)
                    {
                        note = 0;
                    }
                    if (redPressed)
                    {
                        note = 2;
                    }
                    if (yellowPressed)
                    {
                        note = 1;
                    }
                    if (greenPressed)
                    {
                        note = 3;
                    }
                    if (note >= 0)
                    {
                        strumTestTimes.Add(Time.time);
                        noteTests.Add(note);
                    }
                }
                
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
            */
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
            if (!whitePressed)
            {
                myDisplay.strikeNote(noteColor.White);

                if (creatingSong)
                {
                    strumTestTimes.Add(Time.time);
                    noteTests.Add(0);
                }
            }
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
            if (!yellowPressed)
            {
                myDisplay.strikeNote(noteColor.Yellow);
                if (creatingSong)
                {
                    strumTestTimes.Add(Time.time);
                    noteTests.Add(1);
                }
            }
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
            if (!greenPressed)
            {
                myDisplay.strikeNote(noteColor.Green);
                if (creatingSong)
                {
                    strumTestTimes.Add(Time.time);
                    noteTests.Add(3);
                }
            }
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
            if (!redPressed)
            {
                myDisplay.strikeNote(noteColor.Red);
                if (creatingSong)
                {
                    strumTestTimes.Add(Time.time);
                    noteTests.Add(2);
                }
            }
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
        isKOd = true;
        myDisplay.isKOd = isKOd;
        myCharacter.GetComponent<SpriteRenderer>().enabled = false;
        myDisplay.dissappear();
        spotlight.gameObject.SetActive(false);
    }

    public void PlayerRespawned()
    {
        // TODO
        isKOd = false;
        myDisplay.isKOd = isKOd;
		myCharacter.GetComponent<SpriteRenderer>().enabled = true;
        myDisplay.hypeNumber=0;
        myDisplay.reAppear();
        spotlight.gameObject.SetActive(true);
    }

    public songDisplayManager getManager()
    {
        return myDisplay;
    }

    private void OnDestroy()
    {
        if (creatingSong)
        {
            var sr = File.CreateText(filename);
            for (int i = 0; i < strumTestTimes.Count; i++)
            {
                sr.WriteLine("" + strumTestTimes[i] + " " + noteTests[i]);
            }
            sr.Close();
        }
    }

    public void SoundPlayed()
    {
        Debug.Log("Sound played");
    }
}

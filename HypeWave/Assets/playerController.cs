using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {

	Vector3 moveDirection;
	public float speed;
	public string controllerNumber;

    public songDisplayManager songDisplayManagerPrefab;
    public songDisplayManager songDisplayManagerInstance; //this is the fret board

    public CrowdPlayer crowdPlayerPrefab;
    public CrowdPlayer crowdPlayerInstance; //this is the player object in the crowd

    void Start()
	{
		Debug.Log(controllerNumber);

        songDisplayManagerInstance = Instantiate(songDisplayManagerPrefab);

    }

	void Update()
	{
		moveDirection = new Vector3 (Input.GetAxis(controllerNumber + "Horizontal") * speed, 
			0f, Input.GetAxis(controllerNumber + "Vertical") * -speed);
		transform.Translate(moveDirection * Time.deltaTime);

		//use the wave with rb
		if (Input.GetButton(controllerNumber + "Wave")) {
			Debug.Log(controllerNumber + " pressed rb");
			//call wave function
		}
		//use mosh pit with lb
		if (Input.GetButton(controllerNumber + "MoshPit")) {
			Debug.Log(controllerNumber + " pressed lb");
			//call mosh pit function
		}
		//x = rhythm game
		if (Input.GetButton(controllerNumber + "NoteBlue")) {
			Debug.Log(controllerNumber + " pressed x");
			//call rhythm game functions
			//songDisplayManager.strikeNote(noteColor Red, Yellow, Blue, Orange);
		}
		//y = rhythm game
		if (Input.GetButton(controllerNumber + "NoteYellow")) {
			Debug.Log(controllerNumber + " pressed y");
			//call rhythm game functions
			//songDisplayManager.strikeNote(noteColor Red, Yellow, Blue, Orange);
		}
		//a = rhythm game
		if (Input.GetButton(controllerNumber + "NoteOrange")) {
			Debug.Log(controllerNumber + " pressed a");
			//call rhythm game functions
			//songDisplayManager.strikeNote(noteColor Red, Yellow, Blue, Orange);
		}
		//b = rhythm game
		if (Input.GetButton(controllerNumber + "NoteRed")) {
			Debug.Log(controllerNumber + " pressed b");
			//call rhythm game functions
			//songDisplayManager.strikeNote(noteColor Red, Yellow, Blue, Orange);
		}
	}
}

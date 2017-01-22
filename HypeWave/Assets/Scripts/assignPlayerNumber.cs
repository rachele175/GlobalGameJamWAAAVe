using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assignPlayerNumber : MonoBehaviour {

	player[] fourPlayers;

	void Start()
	{
		fourPlayers = FindObjectsOfType<player>();
		for (int i = 0; i < 4; i++)
		{
			Debug.Log("the manager just grabbed " + fourPlayers[i]);
			fourPlayers[i].controllerNumber = "j" + (1 + i).ToString();
			Debug.Log("the manager just set " + fourPlayers[i] + " to j" +(1+i).ToString());
		}
	}
}

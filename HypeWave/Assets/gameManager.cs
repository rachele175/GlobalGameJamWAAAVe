using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
    public List<playerController> players;
    public GameObject startScreen;
     

    public int gameState;

    // Use this for initialization
    void Start () {
        gameState = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (deathCount() > 2)
        {

        }
	}

    public int deathCount()
    {
        int i = 0;
        foreach(playerController p in players)
        {
            if (p.dead)
            {
                i++;
            }
        }
        return i;
    }

    public void incrementGameState()
    {
        gameState++;
    }
}

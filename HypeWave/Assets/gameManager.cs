using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour {
    public List<playerController> players;
    public GameObject startScreen;
    public songScript mySong;

    public int gameState;

    // Use this for initialization
    void Start () {
        gameState = 0;
	}
	
	// Update is called once per frame
	void Update () {
        switch (gameState)
        {
            case 0:
                startScreen.SetActive(true);
                break;
            case 1:
                startScreen.SetActive(false);
                mySong.playingGame=true;
                break;
            case 2:
                Application.LoadLevel(Application.loadedLevel);
                break;
        }
        if (deathCount() > 2)
        {
            incrementGameState();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteKillZone : MonoBehaviour {

    public songDisplayManager myMang;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "note")
        {
            if (myMang.getPlayerID() == collision.gameObject.GetComponent<notePrefab>().playerID)
            {
                if (!myMang.isKOd)
                {
                    if (myMang.hypeNumber > -3)
                    {
                        myMang.hypeNumber -= 1;
                    }
                    else
                    {
                        myMang.killPlayer();
                    }
                    myMang.breakCombo();
                }
                Destroy(collision.gameObject, 0.1f);
            }
        }
    }
}

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
            Destroy(collision.gameObject);
            myMang.hypeNumber -= 1;
            myMang.breakCombo();
        }
    }
}

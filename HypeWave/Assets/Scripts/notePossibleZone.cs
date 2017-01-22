using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePossibleZone : MonoBehaviour {
    public List<GameObject> strummableNotes;
    public songDisplayManager myMang;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "note")
        {
            if (myMang.getPlayerID() == collider.gameObject.GetComponent<notePrefab>().playerID)
            {
                strummableNotes.Add(collider.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "note" && strummableNotes.Contains(other.gameObject))
        {
            strummableNotes.Remove(other.gameObject);
        }
    }
}

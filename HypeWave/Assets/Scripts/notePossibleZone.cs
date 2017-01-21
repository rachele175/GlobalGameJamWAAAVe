using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePossibleZone : MonoBehaviour {
    public List<GameObject> strummableNotes;
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
            strummableNotes.Add(collider.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "note")
        {
            strummableNotes.Remove(other.gameObject);
        }
    }
}

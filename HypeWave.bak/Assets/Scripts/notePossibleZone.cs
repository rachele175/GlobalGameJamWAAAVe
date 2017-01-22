using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class notePossibleZone : MonoBehaviour {
    public List<GameObject> strummableNotes;
    public songDisplayManager myMang;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songDisplayManager : MonoBehaviour {

    public int playerNum;
    public int hypeNumber;
    public TextMesh comboTracker;

    int combo;
    int hypeRequiredForWave = 5;
    int maxHype;
    public GameObject noteContainer;
    public GameObject notePrefab;

    public Transform redSpot;
    public Transform blueSpot;
    public Transform orangeSpot;
    public Transform yellowSpot;

    public Material redMat;
    public Material blueMat;
    public Material orangeMat;
    public Material yellowMat;



    public GameObject killZone;

    public notePossibleZone myNoteZone;


    // Use this for initialization
    void Start () {
        maxHype = hypeRequiredForWave * 3;
	}
	
	// Update is called once per frame
	void Update () {
        comboTracker.text = "x" + combo;
	}

    public void spawnNote(noteColor g){
        GameObject currentNote = Instantiate(notePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), noteContainer.transform);
        notePrefab noteScript=currentNote.GetComponent<notePrefab>();
        noteScript.setUp(g, this);
        moveNote(noteScript, g);
    }

    public void strikeNote(noteColor noteCol)
    {
        foreach(GameObject g in myNoteZone.strummableNotes)
        {
            if (g.GetComponent<notePrefab>().getMyColor() == noteCol)
            {
                Destroy(g);
                hypeNumber += 1;
                combo += 1;
            }
        }
    }

    public void moveNote(notePrefab n, noteColor c)
    {
        if (c == noteColor.Red)
        {
            n.transform.localPosition = redSpot.transform.position;
            n.gameObject.GetComponentInChildren<Renderer>().material = redMat;
        }
        else if (c == noteColor.Blue)
        {
            n.transform.localPosition = blueSpot.transform.position;
            n.gameObject.GetComponentInChildren<Renderer>().material = blueMat;
        }
        else if (c == noteColor.Orange)
        {
            n.gameObject.GetComponentInChildren<Renderer>().material = orangeMat;
            n.transform.localPosition = orangeSpot.transform.position;
        }
        else if (c == noteColor.Yellow)
        {
            n.transform.localPosition = yellowSpot.transform.position;
            n.gameObject.GetComponentInChildren<Renderer>().material = yellowMat;
        }

    }

    public int returnCombo()
    {
        return combo;
    }

    public int returnHype()
    {
        return hypeNumber;
    }

    public void breakCombo()
    {
        combo = 0;
    }
}

using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class songDisplayManager : MonoBehaviour {

    public event Action noMoreHype;


    public int hypeNumber;
    public TextMesh comboTracker;

    public int minHype = -3;
    public int maxHype = 10;

    int combo;
    int hypeRequiredForWave = 5;
    int internalMaxHype;
    public GameObject noteContainer;
    public GameObject notePrefab;

    public Transform whiteSpot;
    public Transform yellowSpot;
    public Transform redSpot;
    public Transform greenSpot;

    public Material whiteMat;
    public Material yellowMat;
    public Material redMat;
    public Material greenMat;

    public fretFeedback whiteF;
    public fretFeedback yellowF;
    public fretFeedback redF;
    public fretFeedback greenF;

    public GameObject killZone;

    public notePossibleZone myNoteZone;
    public GameObject hypeBar;
    public GameObject evilHypeBar;

    public GameObject strumBar;
    Color original;

    string playerNum;
    // Use this for initialization
    void Start () {
        internalMaxHype = hypeRequiredForWave*3;
        original = strumBar.GetComponent<Renderer>().material.color;

    }
	
	// Update is called once per frame
	void Update () {
        comboTracker.text = "x" + combo;
        hypeRunner();
	}

    public void spawnNote(noteColor g){
        GameObject currentNote = Instantiate(notePrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), noteContainer.transform);
        notePrefab noteScript=currentNote.GetComponent<notePrefab>();
        noteScript.setUp(g, this, playerNum);
        moveNote(noteScript, g);
    }

    public void strikeNote(noteColor noteCol)
    {
        List<GameObject> deleteNotes=new List<GameObject>();
        foreach(GameObject g in myNoteZone.strummableNotes)
        {
            if (g.GetComponent<notePrefab>().getMyColor() == noteCol)
            {
                if (hypeNumber < maxHype)
                {
                    hypeNumber += 1;
                }
                combo += 1;
                deleteNotes.Add(g);

            }
        }
        if (deleteNotes.Count==0)
        {
            hypeNumber -= 1;
            breakCombo();
        }
        foreach(GameObject d in deleteNotes)
        {
            myNoteZone.strummableNotes.Remove(d);
            Destroy(d, 0.1f);
        }
        

    }

    public void moveNote(notePrefab n, noteColor c)
    {
        if (c == noteColor.White)
        {
            n.transform.localPosition = whiteSpot.transform.localPosition;
            n.gameObject.GetComponentInChildren<Renderer>().material = whiteMat;
        }
        else if (c == noteColor.Yellow)
        {
            n.transform.localPosition = yellowSpot.transform.localPosition;
            n.gameObject.GetComponentInChildren<Renderer>().material = yellowMat;
        }
        else if (c == noteColor.Red)
        {
            n.gameObject.GetComponentInChildren<Renderer>().material = redMat;
            n.transform.localPosition = redSpot.transform.localPosition;
        }
        else if (c == noteColor.Green)
        {
            n.transform.localPosition = greenSpot.transform.localPosition;
            n.gameObject.GetComponentInChildren<Renderer>().material = greenMat;
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

    public void pressFret(noteColor c)
    {
        if (c == noteColor.White)
        {
            whiteF.press();
        }
        else if (c == noteColor.Yellow)
        {
            yellowF.press();
        }
        else if (c == noteColor.Red)
        {
            redF.press();
        }
        else if (c == noteColor.Green)
        {
            greenF.press();
        }
    }

    public void releaseFret(noteColor c)
    {
        if (c == noteColor.White)
        {
            whiteF.release();
        }
        else if (c == noteColor.Yellow)
        {
            yellowF.release();
        }
        else if (c == noteColor.Red)
        {
            redF.release();
        }
        else if (c == noteColor.Green)
        {
            greenF.release();
        }
    }

    public void hypeRunner()
    {
        if (hypeNumber >= 0)
        {
            hypeBar.transform.localScale = new Vector3(((float)hypeNumber / internalMaxHype) * 4.0506f, hypeBar.transform.localScale.y, hypeBar.transform.localScale.z);
        }
        if (hypeNumber <= 0)
        {
            evilHypeBar.transform.localScale = new Vector3((Mathf.Abs((float)hypeNumber) / 3) * 4.0506f, hypeBar.transform.localScale.y, hypeBar.transform.localScale.z);

        }
    }



    public void strumFeedback()
    {
        strumBar.GetComponent<Renderer>().material.color = Color.white;
        StartCoroutine(undoFeeback());
    }

    public IEnumerator undoFeeback()
    {
        float time=0f;
        while (time < 0.25f)
        {
            strumBar.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, original, time / .25f);
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void killPlayer()
    {
        hypeNumber = 0;
        noMoreHype();
    }

    public void assignPlayerID(string playerID)
    {
        playerNum = playerID;
    }

    public string getPlayerID()
    {
        return playerNum;
    }
}

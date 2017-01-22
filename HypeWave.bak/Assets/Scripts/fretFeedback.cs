using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fretFeedback : MonoBehaviour {
    public Material originalMat;
    public Material colorMat;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void press()
    {
        GetComponent<Renderer>().material = colorMat;
    }

    public void release()
    {
        GetComponent<Renderer>().material = originalMat;

    }
}

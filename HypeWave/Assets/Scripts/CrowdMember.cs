using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
	//Animator animator;
    private Crowd crowd;
    private Vector2 crowdPos;
    public float hypeLevel;

    public GameObject[] visuals;

    private float pitEndsTime;

    private void Start()
    {
        crowd = Crowd.Instance;
        int i = UnityEngine.Random.Range(0,(visuals.Length-1));
        GameObject vis = Instantiate(visuals[i]);
        vis.transform.SetParent(gameObject.transform);
        vis.transform.localPosition = Vector3.zero;
        vis.transform.localScale = Vector3.one * .35f; 
        vis.transform.rotation = Quaternion.AngleAxis(40f, Vector3.right);
        crowd.crowdUpdate += UpdateState;
        crowd.pitStart += PitStarts;
    }


    // This fires every time we should change 
    // what we are doing based on the crowd field
    private void UpdateState()
    {
        if (Time.time > pitEndsTime)
        {
            // TODO
            transform.position = new Vector3(
                            transform.position.x,
                            crowd.GetMove(crowdPos.x, crowdPos.y).magnitude,
                            transform.position.z);
            Vector2 hype = crowd.GetHype(crowdPos.x, crowdPos.y); //magnitude of this vector is the hype
                                                                  //float of hype
            hypeLevel = hype.magnitude;
            //set the animator float that governs the blend tree of how hype each member is
            //if (animator) animator.SetFloat("HypeLevel", hypeLevel);
            GetComponent<Renderer>().material.color = new Color(hype.x, hype.y, 0);
        }
    }

    public void SetPosition(int x, int y)
    {
        crowdPos = new Vector2(x, y);
        transform.position = new Vector3(x, 0, y);
    }

    private void PitStarts(float x, float y, float radius, float duration)
    {
        if(Vector2.Distance(crowdPos, new Vector2(x,y)) < radius)
        {
            pitEndsTime = Time.time + duration;
            GetComponent<Renderer>().material.color = Color.white;
        }
    }
}

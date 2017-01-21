using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    private Crowd crowd;
    private Vector2 crowdPos;

    private float pitEndsTime;

    private void Start()
    {
        crowd = Crowd.Instance;
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
            Vector2 hype = crowd.GetHype(crowdPos.x, crowdPos.y);
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

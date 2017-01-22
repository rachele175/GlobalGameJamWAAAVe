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
    public float maxHype = 2;

    public GameObject[] visuals;

    public Animator animator;

    private Vector3 centerPosition;

    public float relocatePeriod = 0.1f;
    private float lastRelocate;
    private Vector3 relocateTarget;

    private float pitEndsTime;
    private GameObject vis;

    private float pitAnimateTime;
    private int currentPitSprite;
    public Sprite[] pitSprites;

    private void Start()
    {
        crowd = Crowd.Instance;
        int i = UnityEngine.Random.Range(0,(visuals.Length-1));
        vis = Instantiate(visuals[i]);
        vis.transform.SetParent(gameObject.transform);
        vis.transform.localPosition = Vector3.zero;
        vis.transform.localScale = Vector3.one * .35f; 
        vis.transform.rotation = Quaternion.AngleAxis(40f, Vector3.right);
        crowd.crowdUpdate += UpdateState;
        crowd.pitStart += PitStarts;
        animator = vis.GetComponent<Animator>();
    }


    // This fires every time we should change 
    // what we are doing based on the crowd field
    private void UpdateState()
    {
        if (Time.time > pitEndsTime)
        {
            // TODO
            /*
            transform.position = new Vector3(
                            transform.position.x,
                            crowd.GetMove(crowdPos.x, crowdPos.y).magnitude * 1.15f,
                            transform.position.z);
            */        
            Vector2 hype = crowd.GetHype(crowdPos.x, crowdPos.y); //magnitude of this vector is the hype
                                                                  //float of hype
            hypeLevel = hype.magnitude;
            //set the animator float that governs the blend tree of how hype each member is
            //if (animator) animator.SetFloat("HypeLevel", hypeLevel);
            //GetComponent<Renderer>().material.color = new Color(hype.x, hype.y, 0);
            vis.GetComponent<SpriteRenderer>().enabled = true;
            animator.enabled = true;
        }

        if(Time.time - pitAnimateTime > UnityEngine.Random.Range(0.25f, 0.4f))
        {
            pitAnimateTime = Time.time;

            currentPitSprite = (currentPitSprite + 1) % pitSprites.Length;
            vis.GetComponent<SpriteRenderer>().sprite = pitSprites[currentPitSprite];
        }

        float hypeLerp = Mathf.Sqrt(hypeLevel / maxHype);

        if (Time.time - lastRelocate > relocatePeriod * Mathf.Lerp(1, 0.1f, hypeLerp))
        {
            lastRelocate = Time.time;
            float deviation1 = 0;
                deviation1 += UnityEngine.Random.Range(0f,1f);
            float deviation2= 0;
            int n = 10;
            for(int i = 0; i < n; i ++)
            {
                deviation2 += UnityEngine.Random.Range(-1f,1f);
            }
            //deviation1 = deviation1 / n;
            deviation2 = deviation2 / n;

            relocateTarget = centerPosition + Mathf.Lerp(0.5f, 1f, hypeLerp) * (Quaternion.AngleAxis(360 * deviation1, Vector3.up) * Vector3.right * Mathf.Lerp(1, 1f, hypeLerp) + deviation2 * Vector3.right * Mathf.Lerp(1, 1f, hypeLerp));
            //relocateTarget = centerPosition + deviation1 * Vector3.right + deviation2 * Vector3.forward;

            
        }


        transform.position = Vector3.Lerp(transform.position, relocateTarget, Time.deltaTime / Mathf.Lerp(0.2f,0.02f, hypeLerp));
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, crowd.GetMove(crowdPos.x, crowdPos.y).magnitude * 2f * Mathf.Lerp(1, 1.5f, hypeLerp), Time.deltaTime/0.03f), transform.position.z);

    }

    public void SetPosition(int x, int y)
    {
        crowdPos = new Vector2(x, y);
        centerPosition = new Vector3(x, 0, y);
        transform.position = centerPosition;
    }

    private void PitStarts(float x, float y, float radius, float duration)
    {
        if(Vector2.Distance(crowdPos, new Vector2(x,y)) < radius)
        {
            pitEndsTime = Time.time + duration;
            //GetComponent<Renderer>().material.color = Color.white;
            animator.enabled = false;
            vis.GetComponent<SpriteRenderer>().sprite = pitSprites[0];

            if(UnityEngine.Random.Range(0f,1f) < 0.2f)
            {
                vis.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}

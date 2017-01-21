using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPlayer : MonoBehaviour
{
    public float waveSize = 0.2f;
    public float pushInfluence = 1f;
    public float speed = 0.5f;
    public float hypeAmount = 2;
    private Crowd crowd;

    public string inputNumber = "1";
    public bool debugInput = true;

    private float staunchitude;
    public float staunchitudeTime = 0.5f;

    private Vector2 lastMove;

    public int kernelSize = 2;

    public float magnitudeToStartPit = 1f; //if average mag is higher than this
    public float chaosToStartPit = 0.5f; //and average vector mag is less than this
    // then a pit is started

    private Vector2 crowdPosition;

    private void Start()
    {
        crowd = Crowd.Instance;
        crowd.crowdUpdate += CrowdUpdate;
    }

    private void Update()
    {
        crowdPosition = crowd.GetCrowdPosition(transform.position.x, transform.position.z);

        if (debugInput)
        {
            // move around based on input
            lastMove = Vector3.right * Input.GetAxis(inputNumber + "Horizontal") + Vector3.up * Input.GetAxis(inputNumber + "Vertical");
            transform.position += speed * lastMove.x * Vector3.right
                                + speed * lastMove.y * Vector3.forward;


            if (Input.GetButtonDown(inputNumber + "Wave"))
            {
                // start a wave
                crowd.AddMove(crowdPosition.x, crowdPosition.y, waveSize);
                // don't be affected by crowd movement right after a wave
                staunchitude = 1;
            }
        }

        // decrease staunchitude over time
        staunchitude = Mathf.Clamp(staunchitude - 1f / staunchitudeTime * Time.deltaTime, 0, 1);
    }

    private void CrowdUpdate()
    {
        // get pushed by the crowd
        Vector2 crowdPush = crowd.GetMove(crowdPosition.x, crowdPosition.y);
        transform.position += new Vector3(crowdPush.x, 0, crowdPush.y) * pushInfluence * (1 - staunchitude);

        // add hype from our movement
        crowd.AddHype(crowdPosition.x, crowdPosition.y, lastMove * hypeAmount);

        // analyze a kernel around the player
        float averageMag = 0;
        Vector2 averageVec = Vector2.zero;
        for(int dx = -kernelSize; dx <= kernelSize; dx ++)
        {
            for (int dy = -kernelSize; dy <= kernelSize; dy++)
            {
                averageMag += crowd.GetHype(crowdPosition.x + dx, crowdPosition.y + dy).magnitude;
                averageVec += crowd.GetHype(crowdPosition.x + dx, crowdPosition.y + dy);
            }
        }
        averageMag = averageMag / (kernelSize * kernelSize);
        averageVec = averageVec / (kernelSize * kernelSize);

        if(averageMag > 1 && averageVec.magnitude < chaosToStartPit)
        {
            //pit starts
            Vector2 hype = crowd.GetHype(crowdPosition.x, crowdPosition.y);
            crowd.AddMove(crowdPosition.x, crowdPosition.y, waveSize, bias:true, biasx:hype.x, biasy:hype.y);
        }
    }
}

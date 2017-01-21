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

    private void Start()
    {
        crowd = Crowd.Instance;
        crowd.crowdUpdate += CrowdUpdate;
    }

    private void Update()
    {
        if (debugInput)
        {
            lastMove = Vector3.right * Input.GetAxis(inputNumber + "Horizontal") + Vector3.up * Input.GetAxis(inputNumber + "Vertical");
            transform.position += speed * lastMove.x * Vector3.right
                                + speed * lastMove.y * Vector3.forward;


            if (Input.GetButtonDown(inputNumber + "Wave"))
            {
                crowd.AddMove(transform.position.x, transform.position.z, waveSize);
                staunchitude = 1;
            }
        }

        staunchitude = Mathf.Clamp(staunchitude - 1f / staunchitudeTime * Time.deltaTime, 0, 1);
    }

    private void CrowdUpdate()
    {
        Vector2 crowdPush = crowd.GetMove(transform.position.x, transform.position.z);
        transform.position += new Vector3(crowdPush.x, 0, crowdPush.y) * pushInfluence * (1 - staunchitude);

        crowd.AddHype(transform.position.x, transform.position.z, lastMove * hypeAmount);
    }
}

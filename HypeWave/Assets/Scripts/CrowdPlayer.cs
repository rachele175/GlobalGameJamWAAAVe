using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPlayer : MonoBehaviour
{
    public float pushAmount = 0.2f;
    public float speed = 0.5f;
    public float hypeAmount = 2;
    private Crowd crowd;

    public string inputNumber = "1";
    public bool debugInput = true;

    private void Start()
    {
        crowd = Crowd.Instance;
    }

    private void Update()
    {
        if (debugInput)
        {
            transform.position += speed * Vector3.right * Input.GetAxis(inputNumber + "Horizontal")
                                + speed * Vector3.forward * Input.GetAxis(inputNumber + "Vertical");

            crowd.AddHype(transform.position.x, transform.position.z, Vector2.right * Input.GetAxis(inputNumber + "Horizontal")
                                                                    + Vector2.up    * Input.GetAxis(inputNumber + "Vertical"));

            if (Input.GetButton(inputNumber + "Wave"))
            {
                crowd.AddMove(transform.position.x, transform.position.z, pushAmount);
            }
        }

        Vector2 crowdPush = crowd.GetMove(transform.position.x, transform.position.z);

        transform.position += new Vector3(crowdPush.x, 0, crowdPush.y) * pushAmount;
    }
}

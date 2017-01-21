using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float pushAmount = 0.2f;
    public float speed = 0.5f;
    public float hypeAmount = 2;
    public Crowd crowd;

    public string inputNumber = "1";
    public bool debugInput = true;

    private void Update()
    {
        if (debugInput)
        {
            transform.position += speed * Vector3.right * Input.GetAxis("Horizontal" + inputNumber)
                                + speed * Vector3.forward * Input.GetAxis("Vertical" + inputNumber);

            crowd.AddHype(transform.position.x, transform.position.z, Vector2.right * Input.GetAxis("Horizontal" + inputNumber)
                                                                    + Vector2.up * Input.GetAxis("Vertical" + inputNumber));

            if (Input.GetButton("Jump" + inputNumber))
            {
                crowd.AddMove(transform.position.x, transform.position.z, pushAmount);
            }
        }

        Vector2 crowdPush = crowd.GetMove(transform.position.x, transform.position.z);

        transform.position += new Vector3(crowdPush.x, 0, crowdPush.y) * pushAmount;
    }
}

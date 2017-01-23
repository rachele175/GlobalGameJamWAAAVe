using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdPlayer : MonoBehaviour
{
    public float waveSize = 0.2f;
    public float pushInfluence = 1f;
    public float maxSpeed = 0.12f;
    public float minSpeed = 0.05f;
    internal float speed;
    public float hypeAmount = 2;
    private Crowd crowd;

    [HideInInspector]
    public playerController controller;

    private float staunchitude;
    public float staunchitudeTime = 0.5f;

    private Vector2 lastMove;

    private bool invulnerableToPits;

    private bool dead;
    private float deathTime;
    public float respawnTime;

    public int kernelSize = 2;
    public float pitRadius = 2;
    public float pitDuration = 5f;

    public float pitCooldownTime = 1f;
    private float lastPitCreationTime;

    public float magnitudeToStartPit = 1f; //if average mag is higher than this
    public float chaosToStartPit = 0.5f; //and average vector mag is less than this
    // then a pit is started

    private Vector2 crowdPosition;

    internal Transform respawn;

    private void Start()
    {
        crowd = Crowd.Instance;
        crowd.crowdUpdate += CrowdUpdate;

        transform.position = respawn.transform.position;
    }

    public bool CreateWave()
    {
        if (!dead && !crowd.IsStage(crowdPosition.x, crowdPosition.y))
        {
            // start a wave
            crowd.AddMove(crowdPosition.x, crowdPosition.y, waveSize);
            // don't be affected by crowd movement right after a wave
            staunchitude = 1;

            return true;
        }
        return false;
    }

    private void Update()
    {
        if (!dead)
        {
            crowdPosition = crowd.GetCrowdPosition(transform.position.x, transform.position.z);

            if (true)
            {
                // move around based on input
                lastMove = controller.GetInput();
                transform.position += speed * lastMove.x * Vector3.right
                                    + speed * lastMove.y * Vector3.forward;

                // die in a mosh pit
                if (crowd.IsPit(crowdPosition.x, crowdPosition.y) && !invulnerableToPits && !crowd.IsStage(crowdPosition.x, crowdPosition.y))
                {
                    // death
                    Die();
                }

                // die if you fall off the edge
                if (crowdPosition.x < 1 || crowdPosition.x > crowd.fieldSize - 2 || crowdPosition.y < 1 || crowdPosition.y > crowd.fieldSize - 2)
                {
                    Die();
                }
            }

            // decrease staunchitude over time
            staunchitude = Mathf.Clamp(staunchitude - 1f / staunchitudeTime * Time.deltaTime, 0, 1);
        }
        else
        {
            if(Time.time - deathTime > respawnTime )
            {
                if (controller.lives > 0)
                {
                    //respawn
                    dead = false;
                    controller.PlayerRespawned();
                    transform.position = respawn.transform.position;
                }
                else
                {
                    controller.dead = true;
                }
            }
        }
    }

    private void CrowdUpdate()
    {
        if (!dead)
        {
            // get pushed by the crowd
            Vector2 crowdPush = crowd.GetMove(crowdPosition.x, crowdPosition.y);
            transform.position += new Vector3(crowdPush.x, 0, crowdPush.y) * pushInfluence * (1 - staunchitude);

            // add hype from our movement
            crowd.AddHype(crowdPosition.x, crowdPosition.y, lastMove * hypeAmount);

            // analyze a kernel around the player
            float averageMag = 0;
            Vector2 averageVec = Vector2.zero;
            for (int dx = -kernelSize; dx <= kernelSize; dx++)
            {
                for (int dy = -kernelSize; dy <= kernelSize; dy++)
                {
                    averageMag += crowd.GetHype(crowdPosition.x + dx, crowdPosition.y + dy).magnitude;
                    averageVec += crowd.GetHype(crowdPosition.x + dx, crowdPosition.y + dy);
                }
            }
            averageMag = averageMag / (kernelSize * kernelSize);
            averageVec = averageVec / (kernelSize * kernelSize);

            if (Time.time - lastPitCreationTime > pitCooldownTime && averageMag > 1 && averageVec.magnitude < chaosToStartPit)
            {
                //pit starts
                Debug.Log("starting pit");
                crowd.StartPit(crowdPosition.x, crowdPosition.y, pitRadius, pitDuration);

                Vector2 push = (pitRadius + 1) * crowd.GetHype(crowdPosition.x, crowdPosition.y).normalized;
                StartCoroutine(PushOut(push.x * Vector3.right + push.y * Vector3.forward));
                invulnerableToPits = true;

                lastPitCreationTime = Time.time;
                //Vector2 hype = crowd.GetHype(crowdPosition.x, crowdPosition.y);
                //crowd.AddMove(crowdPosition.x, crowdPosition.y, waveSize, bias:true, biasx:hype.x, biasy:hype.y);
            }
        }
    }

    public void Die()
    {
        deathTime = Time.time;
        dead = true;
        controller.PlayerDied();
    }

    private IEnumerator PushOut(Vector3 movement)
    {
        Vector3 originalPos = transform.position;

        float duration = 0.2f;

        float time = 0;
        while(time < duration)
        {
            transform.position = Vector3.Lerp(originalPos, originalPos + movement, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        invulnerableToPits = false;
    }
}

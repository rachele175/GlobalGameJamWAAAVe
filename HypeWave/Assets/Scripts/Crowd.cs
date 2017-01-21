using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public Transform crowdMemberPrefab;

    public int crowdSize = 20;
    private Transform[,] crowdVis;

    public int fieldSize = 20;
    public Vector2[,] moveField;
    public Vector2[,] hypeField;

    public float maxHype = 2;

    private void Start()
    {
        moveField = new Vector2[fieldSize, fieldSize];
        hypeField = new Vector2[fieldSize, fieldSize];

        crowdVis = new Transform[crowdSize, crowdSize];
        for (int x = 1; x < crowdSize - 1; x++)
        {
            for (int y = 1; y < crowdSize - 1; y++)
            {
                crowdVis[x, y] = Instantiate(crowdMemberPrefab);
                crowdVis[x, y].position = new Vector3(x, 0, y);
            }
        }
    }

    internal void AddHype(float x, float y, Vector2 hype)
    {
        if (x >= 1 && x <= fieldSize - 2 && y >= 1 && y <= fieldSize - 2)
        {
            int xi = Mathf.RoundToInt(x);
            int yi = Mathf.RoundToInt(y);

            hypeField[xi + 1, yi] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi));
            hypeField[xi - 1, yi] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi));
            hypeField[xi, yi - 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi, yi - 1));
            hypeField[xi, yi + 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi, yi + 1));
            hypeField[xi + 1, yi + 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi + 1));
            hypeField[xi - 1, yi - 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi - 1));
            hypeField[xi + 1, yi - 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi - 1));
            hypeField[xi - 1, yi + 1] = hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi + 1));
        }
    }

    // bilinear interpolation
    public Vector2 GetMove(float x, float y)
    {
        int x1 = Mathf.FloorToInt(x);
        int x2 = Mathf.CeilToInt(x);
        int y1 = Mathf.FloorToInt(y);
        int y2 = Mathf.CeilToInt(y);
        Vector2 horzLerp1 = Vector2.Lerp(moveField[x1, y1], moveField[x2, y1], x-x1);
        Vector2 horzLerp2 = Vector2.Lerp(moveField[x1, y2], moveField[x2, y2], x-x1);
        return Vector2.Lerp(horzLerp1, horzLerp2, y-y1);
    }

    public void AddMove(float x, float y, float moveAmount)
    {
        int xi = Mathf.RoundToInt(x);
        int yi = Mathf.RoundToInt(y);

        moveField[xi + 1, yi] = Vector2.right * moveAmount;
        moveField[xi - 1, yi] = Vector2.left * moveAmount;
        moveField[xi, yi - 1] = Vector2.down * moveAmount;
        moveField[xi, yi + 1] = Vector2.up * moveAmount;
        moveField[xi + 1, yi + 1] = (Vector2.right + Vector2.up).normalized * moveAmount;
        moveField[xi - 1, yi - 1] = (Vector2.left + Vector2.down) * moveAmount;
        moveField[xi + 1, yi - 1] = (Vector2.right + Vector2.down) * moveAmount;
        moveField[xi - 1, yi + 1] = (Vector2.left + Vector2.up) * moveAmount;
    }

    // how fast the move_wave decays in amplitude as it moves outward
    public float waveDecay = 0.8f;
    // rate at which hype decays
    public float hypeDecay = 0.8f;
    public float crowdUpdateInterval = 0.1f;
    private float lastCrowdUpdate;

    private void Update()
    {
        if(Time.time - lastCrowdUpdate > crowdUpdateInterval)
        {
            lastCrowdUpdate = Time.time;

            Vector2[,] newField = new Vector2[fieldSize, fieldSize];

            // iterate across the crowd field (except for the edges)
            for (int x = 1; x < fieldSize - 1; x++)
            {
                for (int y = 1; y < fieldSize - 1; y++)
                {
                    hypeField[x, y] = Vector2.ClampMagnitude(hypeField[x, y] * hypeDecay, maxHype);

                    if (x > 0 && x < fieldSize - 1 && y > 0 && y < fieldSize - 1)
                    {
                        // iterate across our 8 neighbors
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                // skip the center (us) - this check is not necessary but it makes it clearer to me
                                if (!(dx == 0 && dy == 0))
                                {
                                    // transmit our vector to them, but only in the direction of our vector
                                    float dotproduct = Vector2.Dot(moveField[x, y], new Vector2(dx, dy).normalized);
                                    if (dotproduct > 0)
                                    {
                                        Vector2 waveTransmission = moveField[x, y] * Mathf.InverseLerp(0, moveField[x, y].magnitude, dotproduct) * waveDecay;
                                        newField[x + dx, y + dy] += waveTransmission * (1 - Mathf.InverseLerp(0, maxHype, hypeField[x + dx, y + dy].magnitude));
                                        //newField[x + (int)Mathf.Sign(hypeField[x + dx, y + dy].x), y + (int)Mathf.Sign(hypeField[x + dx, y + dy].y)] += waveTransmission * Mathf.InverseLerp(0, maxHype, hypeField[x + dx, y + dy].magnitude);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            moveField = newField;

            for (int x = 1; x < crowdSize - 1; x++)
            {
                for (int y = 1; y < crowdSize - 1; y++)
                {
                    crowdVis[x, y].position = new Vector3(
                        crowdVis[x, y].position.x, 
                        GetMove(x, y).magnitude * 4, 
                        crowdVis[x, y].position.z);
                    crowdVis[x, y].GetComponent<Renderer>().material.color = new Color(hypeField[x, y].x, hypeField[x,y].y, 0);
                }
            }
        }
    }


}

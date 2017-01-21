﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    public static Crowd Instance;

    public CrowdMember crowdMemberPrefab;
    public int crowdSize = 20;
    private CrowdMember[,] crowdVis;
    public float transmissionSpeed = 0.8f;
    public float hypeTransmissionDecay = 0.1f;

    public int fieldSize = 20;
    public Vector2[,] moveField;
    public Vector2[,] hypeField;
    public float maxHype = 2;

    public event Action crowdUpdate;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        moveField = new Vector2[fieldSize, fieldSize];
        hypeField = new Vector2[fieldSize, fieldSize];

        crowdVis = new CrowdMember[crowdSize, crowdSize];
        for (int x = 1; x < crowdSize - 1; x++)
        {
            for (int y = 1; y < crowdSize - 1; y++)
            {
                crowdVis[x, y] = Instantiate(crowdMemberPrefab);
                crowdVis[x, y].SetPosition(x, y);
            }
        }
    }

    public void AddHype(float x, float y, Vector2 hype)
    {
        if (x >= 1 && x <= fieldSize - 2 && y >= 1 && y <= fieldSize - 2)
        {
            int xi = Mathf.RoundToInt(x);
            int yi = Mathf.RoundToInt(y);

            hypeField[xi + 1, yi] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi));
            hypeField[xi - 1, yi] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi));
            hypeField[xi, yi - 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi, yi - 1));
            hypeField[xi, yi + 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi, yi + 1));
            hypeField[xi + 1, yi + 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi + 1));
            hypeField[xi - 1, yi - 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi - 1));
            hypeField[xi + 1, yi - 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi + 1, yi - 1));
            hypeField[xi - 1, yi + 1] += hype / Vector2.Distance(new Vector2(xi, yi), new Vector2(xi - 1, yi + 1));
        }
    }

    public Vector2 GetHype(float x, float y)
    {
        if (x >= 1 && x <= fieldSize - 2 && y >= 1 && y <= fieldSize - 2)
        {
            int x1 = Mathf.FloorToInt(x);
            int x2 = Mathf.CeilToInt(x);
            int y1 = Mathf.FloorToInt(y);
            int y2 = Mathf.CeilToInt(y);
            Vector2 horzLerp1 = Vector2.Lerp(hypeField[x1, y1], hypeField[x2, y1], x - x1);
            Vector2 horzLerp2 = Vector2.Lerp(hypeField[x1, y2], hypeField[x2, y2], x - x1);
            return Vector2.Lerp(horzLerp1, horzLerp2, y - y1);
        }
        return Vector2.zero;
    }

    // bilinear interpolation
    public Vector2 GetMove(float x, float y)
    {
        if (x >= 1 && x <= fieldSize - 2 && y >= 1 && y <= fieldSize - 2)
        {
            int x1 = Mathf.FloorToInt(x);
            int x2 = Mathf.CeilToInt(x);
            int y1 = Mathf.FloorToInt(y);
            int y2 = Mathf.CeilToInt(y);
            Vector2 horzLerp1 = Vector2.Lerp(moveField[x1, y1], moveField[x2, y1], x - x1);
            Vector2 horzLerp2 = Vector2.Lerp(moveField[x1, y2], moveField[x2, y2], x - x1);
            return Vector2.Lerp(horzLerp1, horzLerp2, y - y1);
        }
        return Vector2.zero;
    }

    public void AddMove(float x, float y, float moveAmount)
    {
        if (x >= 1 && x <= fieldSize - 2 && y >= 1 && y <= fieldSize - 2)
        {
            int xi = Mathf.RoundToInt(x);
            int yi = Mathf.RoundToInt(y);

            moveField[xi + 1, yi] += Vector2.right * moveAmount;
            moveField[xi - 1, yi] += Vector2.left * moveAmount;
            moveField[xi, yi - 1] += Vector2.down * moveAmount;
            moveField[xi, yi + 1] += Vector2.up * moveAmount;
            moveField[xi + 1, yi + 1] += (Vector2.right + Vector2.up).normalized * moveAmount;
            moveField[xi - 1, yi - 1] += (Vector2.left + Vector2.down) * moveAmount;
            moveField[xi + 1, yi - 1] += (Vector2.right + Vector2.down) * moveAmount;
            moveField[xi - 1, yi + 1] += (Vector2.left + Vector2.up) * moveAmount;

        }
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
            Vector2[,] newHype = new Vector2[fieldSize, fieldSize];

            // iterate across the crowd field (except for the edges)
            for (int x = 1; x < fieldSize - 1; x++)
            {
                for (int y = 1; y < fieldSize - 1; y++)
                {
                    newHype[x, y] += Vector2.ClampMagnitude(hypeField[x, y] * hypeDecay, maxHype);

                    if (x > 0 && x < fieldSize - 1 && y > 0 && y < fieldSize - 1)
                    {
                        Vector2 waveTransmission = moveField[x, y] * waveDecay * transmissionSpeed;
                        newField[x, y] += moveField[x, y] * waveDecay * (1 - transmissionSpeed);

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
                                        Vector2 specWaveTransmission = waveTransmission * Mathf.Pow(Mathf.InverseLerp(0, moveField[x, y].magnitude, dotproduct), 2);
                                        newField[x + dx, y + dy] += specWaveTransmission * (1 - Mathf.InverseLerp(0, maxHype, hypeField[x + dx, y + dy].magnitude));
                                        newField[x + (int)Mathf.Sign(hypeField[x + dx, y + dy].x), y + (int)Mathf.Sign(hypeField[x + dx, y + dy].y)] += hypeField[x + dx, y + dy].normalized * specWaveTransmission.magnitude * Mathf.InverseLerp(0, maxHype, hypeField[x + dx, y + dy].magnitude);
                                    }

                                    float hypedotproduct = Vector2.Dot(hypeField[x, y], new Vector2(dx, dy).normalized);
                                    if (hypedotproduct > 0)
                                    {
                                        Vector2 hypeTransmission = hypeField[x, y] * hypeTransmissionDecay * Mathf.Pow(Mathf.InverseLerp(0, hypeField[x, y].magnitude, hypedotproduct), 1);
                                        newHype[x + dx, y + dy] += hypeTransmission;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            moveField = newField;
            hypeField = newHype;

            if (crowdUpdate != null)
            {
                crowdUpdate.Invoke();
            }
        }
    }


}

﻿using UnityEngine;
using System.Collections.Generic;

public class CloudBox : MonoBehaviour
{
    public Transform[] prefabs;

    public int count;

    public float sepDist;

    private Vector2 bottomLeft, topRight;
    private List<Transform> clouds;

    // Use this for initialization
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;

        bottomLeft = transform.position - (transform.localScale / 2);
        topRight = bottomLeft + (Vector2)transform.localScale;

        clouds = new List<Transform>();

        for(int i = 0; i < count; i++)
        {
            Vector3 pos;

            if(getPos(out pos))
            {
                Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0f, 0f));
                Transform t = Instantiate(prefabs[Random.Range(0, prefabs.Length)], pos, rot) as Transform;
                clouds.Add(t);
            }
        }
    }

    private bool getPos(out Vector3 pos)
    {
        pos = Vector3.zero;

        for (int i = 0; i < 5; i++)
        {
            pos.x = Random.Range(bottomLeft.x, topRight.x);
            pos.y = Random.Range(bottomLeft.y, topRight.y);

            if(isValidPos(pos))
            {
                return true;
            }
        }

        return false;
    }

    private bool isValidPos(Vector3 pos)
    {
        for(int i = 0; i < clouds.Count; i++)
        {
            if(Vector3.Distance(pos, clouds[i].position) < sepDist)
            {
                return false;
            }
        }

        return true;
    }
}

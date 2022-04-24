using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float after = 2f;

    private float _t;

    private void FixedUpdate()
    {
        _t += Time.deltaTime;
        if (_t >= after)
        {
            Destroy(gameObject);
        }
    }
}

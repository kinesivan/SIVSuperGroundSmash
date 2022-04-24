using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionPulse : MonoBehaviour
{
    public Material material;
    public float amplitude = 1f;
    public float frequency = 2f;
    public Color colorA;
    public Color colorB;

    private bool _enabled;
    private float _t;

    private void Awake()
    {
        material.SetColor("_EmissionColor", Color.black);
    }

    private void Update()
    {
        _t += Time.deltaTime;
        
        if (_enabled)
        {
            var val = Mathf.Abs(Mathf.Sin(_t * frequency) * amplitude);
            material.SetColor("_EmissionColor", Color.Lerp(colorA, colorB, val));
        }
    }

    public void StartPulse()
    {
        material.EnableKeyword("_EMISSION");
        _t = 0;
        _enabled = true;
    }

    public bool Playing()
    {
        return _enabled;
    }

    public void StopPulse()
    {
        _t = 0;
        material.SetColor("_EmissionColor", Color.black);
        _enabled = false;
    }
}

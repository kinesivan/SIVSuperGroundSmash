using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCycler : MonoBehaviour
{
    public Material material;
    public Texture2D[] textures;
    public float cycleDelay = 0.2f;

    private float _t;
    private int _texIndex;

    private void Update()
    {
        _t += Time.deltaTime;

        if (_t >= cycleDelay)
        {
            _t = 0;
            _texIndex++;
            if (_texIndex > textures.Length - 1)
            {
                _texIndex = 0;
            }
            material.SetTexture("_MainTex", textures[_texIndex]);
        }
    }
}

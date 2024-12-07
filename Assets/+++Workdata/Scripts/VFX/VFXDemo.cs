using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXDemo : MonoBehaviour
{
    [SerializeField] private VisualEffect linkedEffect;
    [SerializeField] private float particleScale = 1f;

    private float lastParticleScale = -1f;

    private void Update()
    {
        if (lastParticleScale != particleScale)
        {
            lastParticleScale = particleScale;
            
            linkedEffect.SetFloat("Particle Scale", particleScale);
        }
    }
}

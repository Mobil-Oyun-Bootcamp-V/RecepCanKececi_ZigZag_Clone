using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    ParticleSystem _colParticle;

    private void Start() 
    {   // On trigger this particle effect plays. Particle system stays under main scene.
        _colParticle = FindObjectOfType<ParticleSystem>();
    }
    private void Update() 
    {
        transform.Rotate(0, 1, 0);    
    }
    private void OnTriggerEnter(Collider other) 
    {   // Particles play a little above of collectable object.
        _colParticle.transform.position = transform.position + Vector3.up;
        _colParticle.Play();
        Destroy(gameObject);    
    }
}

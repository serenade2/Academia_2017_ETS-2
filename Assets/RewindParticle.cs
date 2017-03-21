using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindParticle : MonoBehaviour
{

    private ParticleSystem particleSystem;

    private float rewindStartTime;
    private float timeRewinded = 0;
    [HideInInspector]
    public bool isRewinding = false;
    private float beginEmissionTime;
    

    ParticleSystem.Particle[] particles;

    void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();

        particleSystem.Clear();
        particleSystem.Stop();
        particleSystem.randomSeed = 1; // or anything else
        particleSystem.Play();
        beginEmissionTime = Time.time; // now
    }

	// Update is called once per frame
	void Update ()
	{      
	    if (!isRewinding && !particleSystem.emission.enabled && Time.time >= beginEmissionTime )
	    {
            var emission = particleSystem.emission;
            emission.enabled = true;
        }
    }

    void InitializeIfNeeded()
    {
        if (particles == null || particles.Length < particleSystem.main.maxParticles)
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    public void StartRewind()
    {
        rewindStartTime = Time.time;

        isRewinding = true;

        // stop emission since we playback the particles already existing
        var emission = particleSystem.emission;
        emission.enabled = false;

        ToggleRewindEffect(true);
    }

    public void StopRewind()
    {
        float rewindDuration = Time.time - rewindStartTime;

        beginEmissionTime = Time.time;

        ToggleRewindEffect(false);
        isRewinding = false;
    }

    private void ToggleRewindEffect(bool turnOn)
    {
        InitializeIfNeeded();
        var main = particleSystem.main;
        main.gravityModifierMultiplier = -particleSystem.main.gravityModifierMultiplier;

        // GetParticles is allocation free because we reuse the particles buffer between updates
        int numParticlesAlive = particleSystem.GetParticles(particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].velocity = -particles[i].velocity;
        }

        // Apply the particle changes to the particle system
        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}

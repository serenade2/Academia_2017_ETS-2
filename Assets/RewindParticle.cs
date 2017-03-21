using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindParticle : MonoBehaviour
{
    public float rewindSpeedMultiplier = 2f;

    [HideInInspector]
    public bool shouldEmit = false;

    private ParticleSystem particleSystem;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;

    private float rewindStartTime;
    private float timeRewinded = 0;
    private float beginEmissionTime;

    private float initialSimulationSpeed;
    

    ParticleSystem.Particle[] particles;

    void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        main = particleSystem.main;  // get the variable its always complaining about...
        emission = particleSystem.emission;

        beginEmissionTime = Time.time; // begin now
        initialSimulationSpeed = main.simulationSpeed;
    }

	// Update is called once per frame
	void Update ()
	{      
	    if (shouldEmit && !particleSystem.emission.enabled && Time.time >= beginEmissionTime )
	    {
            emission.enabled = true;
        }
    }

    void InitializeIfNeeded()
    {
        if (particles == null || particles.Length < main.maxParticles)
            particles = new ParticleSystem.Particle[main.maxParticles];
    }

    public void StartRewind()
    {
        rewindStartTime = Time.time;

        shouldEmit = false;

        // stop emission since we playback the particles already existing
        emission.enabled = false;

        ChangeParticlesVelocity(new Vector3(0f, 0f, -main.startSpeedMultiplier * rewindSpeedMultiplier));
    }

    public void StopRewind()
    {
        timeRewinded = Time.time - rewindStartTime;
        beginEmissionTime = Time.time + timeRewinded;

        ChangeParticlesVelocity(new Vector3(0f, 0f, main.startSpeedMultiplier));
        shouldEmit = true;
    }

    public void Pause()
    {
        // stop emission since we're paused
        emission.enabled = false;
        shouldEmit = false;
        
        main.simulationSpeed = 0f; // freeze the simulation
    }

    public void UnPause()
    {
        main.simulationSpeed = initialSimulationSpeed;
        shouldEmit = true;
    }

    private void ChangeParticlesVelocity(Vector3 newVelocity)
    {
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the particles buffer between updates
        int numParticlesAlive = particleSystem.GetParticles(particles);
        
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].velocity = newVelocity;
            particles[i].remainingLifetime = particles[i].remainingLifetime*2f;
        }

        // Apply the particle changes to the particle system
        particleSystem.SetParticles(particles, numParticlesAlive);
    }
}

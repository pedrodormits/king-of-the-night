using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private List<ParticleSystem> _particles;
    [HideInInspector] public ParticleSystem CurrentParticle;
    [HideInInspector] public Dictionary<string, ParticleSystem> ParticlesDict = new();
    #endregion

    private void Awake() => DefineParticles();
    
    private void DefineParticles()
    {
        ParticlesDict.Add("Particle1", _particles[0]);
        ParticlesDict.Add("Particle2", _particles[1]);
        ParticlesDict.Add("Particle3", _particles[2]);
    }

    public void PlayParticle()
    {
        CurrentParticle.gameObject.SetActive(true);
        CurrentParticle.Play();
    }

    public void StopParticle() => StartCoroutine(StopParticleDelayed());

    private IEnumerator StopParticleDelayed()
    {
        CurrentParticle.Stop();
        yield return new WaitWhile(() => CurrentParticle.IsAlive(true));
        CurrentParticle.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    #region VARIABLES
    [HideInInspector] public Dictionary<string, ParticleSystem> ParticlesDict = new();
    [HideInInspector] public ParticleSystem CurrentParticle;
    [SerializeField] private List<ParticleSystem> _Particles;
    #endregion

    private void Awake() => DefineParticles();
    
    private void DefineParticles()
    {
        ParticlesDict.Add("Particle1", _Particles[0]);
        ParticlesDict.Add("Particle2", _Particles[1]);
        ParticlesDict.Add("Particle3", _Particles[2]);
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
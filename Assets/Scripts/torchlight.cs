using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchlight : MonoBehaviour
{
    public float torchLife = 10f; // How long the fuel in your torch is good for
    Light torchSource;
    public float baseTorchRange = 2f; //base range of the light source at 1 unit of fuel
    public float maxTorchRange = 20f; //Torch Range at full fuel
    public float torchMaxFuel = 20f; //Limit the max amount of torch fuel!?
    public float dimRate = 4f; //(Dim one unit per 2 seconds)
    ParticleSystem particles;
    AudioSource torchAudio;
    [Range(0f,1f)]
    public float startingTorchVolume = .5f;
    public float particleStartingSize = .1f;
    public float particleStartingEmissionRate = 10f;

    // Start is called before the first frame update
    void Start()
    {
        torchSource = GetComponent<Light>();
        particles = GetComponentInChildren<ParticleSystem>();
        torchAudio = GetComponent<AudioSource>();
    }

    public void FillTorch(float amountOfLight)
    {
        if (amountOfLight <= torchMaxFuel - torchLife)
        {
            torchLife += amountOfLight;
        }
        else torchLife = torchMaxFuel;
    }

    public float GetRadius()
    {
        return torchSource.range;
    }

    public float GetMaxRadius()
    {
        return maxTorchRange;
    }

    private void TorchDim()
    {
        var main = particles.main;
        var emission = particles.emission;
        if (torchLife > 0) torchLife = torchLife - Time.deltaTime/dimRate;
        float torchRadiusModifier = torchLife / torchMaxFuel * maxTorchRange;
        torchSource.range = torchRadiusModifier * baseTorchRange; //adjust the radius of the torch every frame.
        main.startSize = particleStartingSize - (1-torchLife/torchMaxFuel)*particleStartingSize;
        emission.rateOverTime = particleStartingEmissionRate - (1 - torchLife / torchMaxFuel) * particleStartingEmissionRate;
        torchAudio.volume = startingTorchVolume - (1 - torchLife / torchMaxFuel) * startingTorchVolume;
    }

    public void ReduceTorchLife(float torchLifeReduction)
    {
        if(torchLife > 0) torchLife -= torchLifeReduction;
    }

    public float GetTorchLife()
    {
        return torchLife;
    }

    public float GetMaxTorchLife()
    {
        return torchMaxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        TorchDim();
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManeuverCombo 
{
    [SerializeField]
    public List<Maneuver> maneuverChain;
    [SerializeField]
    public float currSpeed;
    [SerializeField]
    public float health;
    [SerializeField]
    public float energy;
    [SerializeField]
    public Section mapSection;
    // Start is called before the first frame update
    void Start()
    {

    }
    public ManeuverCombo(List<Maneuver> _maneuverChain, float _currSpeed, float _health, float _energy, Section _mapSection)
    {
        maneuverChain = _maneuverChain;
        currSpeed = _currSpeed;
        health = _health;
        energy = _energy;
        mapSection = _mapSection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

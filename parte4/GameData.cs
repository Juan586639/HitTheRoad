using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    [SerializeField]
    public int numberOfCombos;
    [SerializeField]
    public List<ManeuverCombo> maneuverCombos;
}

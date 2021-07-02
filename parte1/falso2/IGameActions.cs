using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameActions 
{
    // Start is called before the first frame update
    void OnAction(controlHub character);
    void OffAction(controlHub character);

}

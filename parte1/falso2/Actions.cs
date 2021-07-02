using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Up : IGameActions
{
    float valueToReturn = 0;

    public void OffAction(controlHub character)
    {
        valueToReturn -= Time.deltaTime * 2.944444f;
        if (valueToReturn < 0)
            valueToReturn = 0;
        character.Vertical = valueToReturn / 1.112f;
        Debug.Log(valueToReturn);
    }

    public void OnAction(controlHub character)
    {
        valueToReturn += Time.deltaTime*2.944444f;
        if (valueToReturn > 1)
            valueToReturn = 1;
        Debug.Log("upppppp");
        character.Vertical = valueToReturn / 1.112f;
    }

}
[Serializable]
public class Down : IGameActions
{
    float valueToReturn = 0;
    public void OffAction(controlHub character)
    {
        throw new NotImplementedException();
    }

    public void OnAction(controlHub character)
    {
        float valueToReturn = 0;
        Debug.Log("dooooooown");
    }

}
[Serializable]
public class Left : IGameActions
{
    float valueToReturn = 0;
    public void OffAction(controlHub character)
    {
        valueToReturn += Time.deltaTime * 2.944444f;
        if (valueToReturn > 0)
            valueToReturn = 0;
        character.Horizontal = valueToReturn;

    }

    public void OnAction(controlHub character)
    {
        valueToReturn -= Time.deltaTime * 2.944444f;
        if (valueToReturn < -1)
            valueToReturn = -1;
        //if (Input.GetKey(KeyCode.O))
        //{
        //    valueToReturn += 0.15858858f;
        //    if (valueToReturn > 1)
        //        valueToReturn = 1;
        //}
        //else
        //{
        //    valueToReturn -= 0.15858858f;
        //    if (valueToReturn < 0)
        //        valueToReturn = 0;
        //}
        Debug.Log("Left");
        character.Horizontal = valueToReturn;
    }
}
[Serializable]
public class Right : IGameActions
{
    float valueToReturn = 0;
    public void OffAction(controlHub character)
    {
        valueToReturn -= Time.deltaTime * 2.944444f;
        if (valueToReturn < 0)
            valueToReturn = 0;
        character.Horizontal = valueToReturn;
    }

    public void OnAction(controlHub character)
    {
        valueToReturn += Time.deltaTime * 2.944444f;
        if (valueToReturn > 1)
            valueToReturn = 1;
        //if (Input.GetKey(KeyCode.O))
        //{
        //    valueToReturn += 0.15858858f;
        //    if (valueToReturn > 1)
        //        valueToReturn = 1;
        //}
        //else
        //{
        //    valueToReturn -= 0.15858858f;
        //    if (valueToReturn < 0)
        //        valueToReturn = 0;
        //}
        Debug.Log("right");
        character.Horizontal = valueToReturn;
    }

}

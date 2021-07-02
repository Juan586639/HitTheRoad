using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Section 
{
    [SerializeField]
    public int sectionID;
    // Start is called before the first frame update
    void Start()
    {

    }
    public Section(int id)
    {
        sectionID = id;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

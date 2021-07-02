using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class SectionManager : Singleton<SectionManager>
{
    public List<MapSection> mapSections;
    public MapSectionEvent OnSectionEnded;
    public ManeuverManager maneuverManager;
    // Start is called before the first frame update
    void Start()
    {
        maneuverManager = FindObjectOfType<ManeuverManager>();
        OnSectionEnded.AddListener((MapSection ms) =>
        {
            CheckForActiveSection(ms);
        });
        mapSections = GetComponentsInChildren<MapSection>().ToList(); 
        mapSections.ForEach(ms => ms.manager = GetComponent<SectionManager>());
    }
    void CheckForActiveSection(MapSection currMapSection)
    {
        Debug.Log("checking");
        mapSections.ForEach(ms =>
        {
            if (ms.active && ms!= currMapSection)
            {
                ms.active = false;
            }
        });
        maneuverManager.currMapSection = currMapSection.section;
        maneuverManager.CreateManeuverCombo();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

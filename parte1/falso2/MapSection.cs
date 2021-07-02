using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSection : MonoBehaviour
{
    public int sectionId;
    public bool active;
    public SectionManager manager;
    public Section section;
    // Start is called before the first frame update
    void Start()
    {
        section = new Section(sectionId);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            active = true;
            Debug.Log(sectionId);
            manager.OnSectionEnded.Invoke(this);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringSideCharacters : MonoBehaviour
{
    public List<GameObject> people;
    public GameObject dialogBox;
    // Start is called before the first frame update
    private void Start()
    {
        people = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!people.Contains(other.GetComponent<GameObject>()) && other.transform.parent.name=="SideCharacters")
        {
            people.Add(other.GetComponent<GameObject>());
        }

        StoryIBT.blackboardTrigger[other.gameObject] = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (people.Contains(other.GetComponent<GameObject>()) && other.transform.parent.name == "SideCharacters")
        {
            people.Remove(other.GetComponent<GameObject>());
        }
        StoryIBT.blackboardTrigger[other.gameObject] = false;
    }
}

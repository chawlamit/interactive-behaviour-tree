using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerScript : MonoBehaviour
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
        if (other.transform.parent == null)
        {
            return;
        }
        if (!people.Contains(other.GetComponent<GameObject>()) && other.transform.parent.name == "SideCharacters")
        {
            people.Add(other.GetComponent<GameObject>());
            
        }

        if(transform.name=="Hero")
            StoryIBT.blackboardTrigger[other.gameObject] = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (people.Contains(other.GetComponent<GameObject>()) && other.transform.parent.name == "SideCharacters")
        {
            people.Remove(other.GetComponent<GameObject>());
        }
        if (transform.name == "Hero")
            StoryIBT.blackboardTrigger[other.gameObject] = false;
    }
}

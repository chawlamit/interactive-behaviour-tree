using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerScript : MonoBehaviour
{
    
    public GameObject dialogBox;
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null)
        {
            return;
        }
        Debug.Log("other: "+other.transform.name);
        Debug.Log("Contains:"+!StoryIBT.people.Contains(other.gameObject));
        Debug.Log("parent: "+ other.transform.parent.name);
        if (!StoryIBT.people.Contains(other.gameObject) && other.transform.parent.gameObject.CompareTag("Group"))
        {
            Debug.Log(("Added: "+other.gameObject));
            StoryIBT.people.Add(other.gameObject);
        }

        StoryIBT.blackboardTrigger[other.gameObject] = true;

    }

    private void OnTriggerExit(Collider other)
    {
        if (StoryIBT.people.Contains(other.gameObject) && other.transform.parent.name == "Groups" )
        {
            StoryIBT.people.Remove(other.gameObject);
        }
        
        StoryIBT.blackboardTrigger[other.gameObject] = false;
    }
}

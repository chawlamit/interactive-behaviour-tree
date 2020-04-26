using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggeringSideCharacters : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        StoryIBT.blackboardTrigger[other.gameObject] = true;
    }

    private void OnTriggerExit(Collider other)
    {
        StoryIBT.blackboardTrigger[other.gameObject] = false;
    }
}

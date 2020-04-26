using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerScript : MonoBehaviour
{
    public List<GameObject> people;
    public GameObject dialogBox;
    //public TextMesh text;
    // Start is called before the first frame update
    void Start()
    {

        people = new List<GameObject>();
        //text.SetActive(false);
        //text.GetComponent<TextMesh>().text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!people.Contains(other.GetComponent<GameObject>()))
        {
            people.Add(other.GetComponent<GameObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (people.Contains(other.GetComponent<GameObject>()))
        {
            people.Remove(other.GetComponent<GameObject>());
        }
    }
}

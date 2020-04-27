﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public class dialogBoxScript : MonoBehaviour
{
    public bool status;
    // Start is called before the first frame update
    void Start()
    {
        status = false;
        this.gameObject.SetActive(status);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void show(bool stat)
    {
        status = stat;
        this.gameObject.SetActive(stat);
    }

    public Node Node_show(bool stat)
    {
        return new Sequence( 
            new LeafInvoke(()=>status=stat),
            new LeafInvoke(()=>this.gameObject.SetActive(stat)));
    }
}

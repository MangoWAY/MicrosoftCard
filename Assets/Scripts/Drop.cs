using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Drop : MonoBehaviour{
    private GlobalManager m_gloManager;

    void Start()
    {
        m_gloManager = GameObject.Find("GlobalManager").GetComponent<GlobalManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="card")
        {
            m_gloManager.heapname = transform.name;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "card")
        {
            m_gloManager.heapname = "";
        }
    }

}

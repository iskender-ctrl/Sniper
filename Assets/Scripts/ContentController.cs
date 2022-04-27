using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System;
public class ContentController : MonoBehaviour
{

    public Sanal api;
    private void Start()
    {
        LoadContent();
    }
    public void LoadContent()
    {
        DestroyAllChildren();
        //StartCoroutine(api.Checks());
        StartCoroutine(api.GetDisplayBundleRoutine(OnContentLoaded, transform));
    }

    void OnContentLoaded(GameObject content)
    {
        //do something cool here
        Debug.Log("Loaded: " + content.name);
    }

    void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
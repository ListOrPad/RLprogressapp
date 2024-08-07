using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNDSound : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] soundObj = GameObject.FindGameObjectsWithTag("Sound");
        if(soundObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public GameObject dayPrefab;
    public GameObject content;

    private void Awake()
    {
        GameObject[] sessionManagerObj = GameObject.FindGameObjectsWithTag("SessionManager");
        if (sessionManagerObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        dayPrefab.AddComponent<Day>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

public class StartStop : MonoBehaviour
{
    public Sprite pauseSprite;
    public Sprite playSprite;
    private Timer timer;

    void Start()
    {
        
    }
    void Update()
    {
        if (timer.timerActive == true)
        {
            timer.timerActive = false;
        }
    }

}

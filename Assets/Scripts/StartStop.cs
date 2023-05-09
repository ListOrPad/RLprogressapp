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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.timerActive == true)
        {
            timer.timerActive = false;
        }
    }

}

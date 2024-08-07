using UnityEngine;
using UnityEngine.UI;

public class PausePlayButton : MonoBehaviour
{
    private void Awake()
    {
        GameObject GOtimer = GameObject.Find("Timer");
        Timer timer = GOtimer.GetComponent<Timer>();
        if (timer.pausePlayButton == null)
        {
            timer.pausePlayButton = GameObject.Find("Pause/Play Button").GetComponent<Button>();
        }
        this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { timer.StartPauseTimer(); });
    }
}

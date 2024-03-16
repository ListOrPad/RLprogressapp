using UnityEngine;

public class BackgroundService : MonoBehaviour
{
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaClass bridgeClass;

    private const string BridgeClassName = "com.myapplication.rlprogressapp.Bridge";
    private const string UnityDefaultJavaClassName = "com.unity3d.player.UnityPlayer";
    private const string BridgeReceiveActivityInstanceMethod = "ReceiveActivityInstance";
    private const string BridgeStartServiceMethod = "StartService";
    private const string BridgeStopServiceMethod = "StopService";

    private void Start()
    {
        SendActivityReference();
        StopService();
    }

    private void SendActivityReference()
    {
        Debug.Log("SendActivityReference called");
        unityClass = new AndroidJavaClass(UnityDefaultJavaClassName);
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        bridgeClass = new AndroidJavaClass(BridgeClassName);
        bridgeClass.CallStatic(BridgeReceiveActivityInstanceMethod, unityActivity);
    }

    public void StartService()
    {
        Debug.Log("StartService called");
        bridgeClass.CallStatic(BridgeStartServiceMethod);
    }

    public void StopService()
    {
        Debug.Log("StopService called");
        bridgeClass.CallStatic(BridgeStopServiceMethod);
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus called with: " + focus);
        if (!focus && Timer.timerActive)
        {
            SendActivityReference();
            StartService();
        }
        else
        {
            StopService();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && Timer.timerActive)
        {
            SendActivityReference();
            StartService();
        }
        else
        {
            StopService();
        }
    }

    private void OnApplicationQuit()
    {
        if (Timer.timerActive)
        {
            SendActivityReference();
            StartService();
        }
    }
}
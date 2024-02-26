using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundService : MonoBehaviour
{
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaClass customClass;

    private const string PackageName = "com.example.rlprogressservice";
    private const string UnityDefaultJavaClassName = "com.unity3d.player.UnityPlayer";
    private const string CustomClassReceiveActivityInstanceMethod = "ReceiveActivityInstance";
    private const string CustomClassStartServiceMethod = "StartService";
    private const string CustomClassStopServiceMethod = "StopService";
    private const string CustomClassPlaySoundMethod = "PlaySound";



    private void Awake()
    {
        SendActivityReference(PackageName);
        StopService();
        //StartService();
    }


    private void SendActivityReference(string packageName)
    {
        unityClass = new AndroidJavaClass(UnityDefaultJavaClassName);
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("myActivity");
        customClass = new AndroidJavaClass(packageName);
        customClass.CallStatic(CustomClassReceiveActivityInstanceMethod, unityActivity);
    }

    public void StartService()
    {
        customClass.CallStatic(CustomClassStartServiceMethod);
        PlaySound();
    }

    public void StopService()
    {
        customClass.CallStatic(CustomClassStopServiceMethod);
    }
    public void PlaySound()
    {
        customClass.CallStatic(CustomClassPlaySoundMethod);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && Timer.timerActive)
        {
            SendActivityReference(PackageName);
            StartService();
        }
        else
        {
            StopService();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause && Timer.timerActive)
        {
            SendActivityReference(PackageName);
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
            SendActivityReference(PackageName);
            StartService();
        }
    }

}
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundService : MonoBehaviour
{
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaClass customClass;
    private const string PackageName = "com.kdg.toast.plugin.Bridge";
    private const string UnityDefaultJavaClassName = "com.unity3d.player.UnityPlayer";
    private const string CustomClassReceiveActivityInstanceMethod = "ReceiveActivityInstance";
    private const string CustomClassStartServiceMethod = "StartService";
    private const string CustomClassStopServiceMethod = "StopService";
    private const string CustomClassPlaySoundMethod = "PlaySound";


    private void Awake()
    {
        StopService();
        //SendActivityReference(PackageName);
        //StartService();
    }


    private void SendActivityReference(string packageName)
    {
        unityClass = new AndroidJavaClass(UnityDefaultJavaClassName);
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
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
        if (focus)
        {
            StopService();
        }
        else
        {
            SendActivityReference(PackageName);
            StartService();
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
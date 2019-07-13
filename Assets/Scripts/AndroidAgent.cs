using UnityEngine;
using System;
using System.Collections;

#if UNITY_ANDROID
public class AndroidAgent : MonoBehaviour
{
    private AndroidJavaObject mActivity;

    void Awake()
    {
#if !UNITY_EDITOR
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            mActivity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            Debug.Log("mActivity:" + mActivity);
        }
#endif
    }

    public void Login(int type)
    {
        CallSdkApi("login", type);
    }

    public void Logout()
    {
        CallSdkApi("logout");
    }

    public void Pay()
    {
        CallSdkApi("pay");
    }

    public void CallSdkApi(string apiName, params object[] args)
    {
        if (mActivity != null)
        {
            Debug.Log("Call(\" " + apiName + "\")");
            mActivity.Call(apiName, args);
        }
        else
        {
            Debug.Log("Nont call(\"" + apiName + "\")");
        }
    }

    public ReturnType CallSdkApi<ReturnType>(string apiName, params object[] args)
    {
        if (mActivity != null)
        {
            Debug.Log("Call(\" " + apiName + "\")");
            return mActivity.Call<ReturnType>(apiName, args);
        }
        else
        {
            Debug.Log("Nont call(\"" + apiName + "\")");
            return default(ReturnType);
        }

    }
}
#endif
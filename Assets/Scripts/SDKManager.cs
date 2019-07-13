using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class SDKManager : SingletonMonoBehaviour<SDKManager>
{
#if UNITY_ANDROID
    AndroidAgent agent;
#endif

    public Action OnSDKLogin;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_ANDROID
        agent = gameObject.AddComponent<AndroidAgent>();
#endif
    }
	
	void Update ()
    {
	    if (Input.touchCount > 0)
        {
#if UNITY_ANDROID
            //agent.Login();
#endif
        }
	}

    public void Login(int type)
    {
#if UNITY_ANDROID
        agent.Login(type);
#endif
    }

    public void Pay()
    {
#if UNITY_ANDROID
        agent.Pay();
#endif
    }

    public void Logout()
    {
#if UNITY_ANDROID
        agent.Logout();
#endif
    }

    void login(JsonData jsonData)
    {
        Debug.LogError("$$$$$$$$$$$login$$$$$$$$$");
        if (jsonData.Keys.Contains("token"))
        {
            string strToken = (string)jsonData["token"];
            Debug.Log("token:" + strToken);

            RoleManager.instance.m_token = strToken;
        }

        if (jsonData.Keys.Contains("openId"))
        {
            string strOpenId = (string)jsonData["openId"];
            Debug.Log("openId:" + strOpenId);

            RoleManager.instance.m_openId = strOpenId;
        }

        if (jsonData.Keys.Contains("displayName"))
        {
            string strDisplayName = (string)jsonData["displayName"];
            Debug.Log("displayName:" + strDisplayName);

            RoleManager.instance.m_displayName = strDisplayName;
        }

        if (jsonData.Keys.Contains("email"))
        {
            string strEmail = (string)jsonData["email"];
            Debug.Log("email:" + strEmail);

            RoleManager.instance.m_email = strEmail;
        }

        if (OnSDKLogin != null)
        {
            OnSDKLogin();
        }
    }

    public void OnLoginResult(int code, JsonData jsonData)
    {
        Debug.Log("OnLoginResult :" + "error:" + code);

        switch (code)
        {
            case SDKConstants.SUCCESS:
                login(jsonData);
                break;
        }
    }

    public void Callback(string jsonstr)
    {
        Debug.Log(jsonstr);

        JsonData json = JsonMapper.ToObject(jsonstr);

        string callbackType = (string)json["callbackType"];

        int code = (int)json["code"];
        JsonData data = json["data"];

        switch (callbackType)
        {
            case "login":
                OnLoginResult(code, data);
                break;
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
    enum LoginType
    {
        GOOGLE      = 0,
        FACEBOOK    = 1,
        VISITOR     = 2,
    };

    public GameObject   m_loginBtn;
    public GameObject   m_payBtn;
    public GameObject   m_facebookBtn;
    public GameObject   m_logoutBtn;

    public UILabel      m_DisplayName;
    public UILabel      m_Email;
	
    void Awake()
    {
        UIEventListener.Get(m_loginBtn).onClick     += OnClickGoogleLogin;
        UIEventListener.Get(m_payBtn).onClick       += OnClickGooglePay;
        UIEventListener.Get(m_facebookBtn).onClick  += OnClickFacebookLogin;

        SDKManager.instance.OnSDKLogin += UpdateUI;
    }

	void Start ()
    {
        
	}

    void Destroy()
    {
        SDKManager.instance.OnSDKLogin -= UpdateUI;
    }

    public void OnClickGoogleLogin(GameObject go)
    {
        Debug.Log("OnClickGoogleLogin");
        SDKManager.instance.Login((int)LoginType.GOOGLE);
    }

    public void OnClickFacebookLogin(GameObject go)
    {
        Debug.Log("OnClickFacebookLogin");
        SDKManager.instance.Login((int)LoginType.FACEBOOK);
    }

    public void OnClickGooglePay(GameObject go)
    {
        Debug.Log("OnClickGooglePay");
        SDKManager.instance.Pay();
    }

    public void OnClickLogout(GameObject go)
    {
        Debug.Log("OnClickLogout");
        SDKManager.instance.Logout();
    }

    void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    m_DisplayName.text = "1111";
        //}
    }

    void UpdateUI()
    {
        Debug.Log(RoleManager.instance.m_displayName);
        if (m_DisplayName != null)
        {
            m_DisplayName.text = RoleManager.instance.m_displayName;
        }

        if (m_Email != null)
        {
            m_Email.text = RoleManager.instance.m_email;
        }
    }
}

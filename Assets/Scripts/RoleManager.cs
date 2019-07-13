using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleManager : SingletonMonoBehaviour<RoleManager>
{
    public string m_token;
    public string m_openId;
    public string m_displayName;
    public string m_email;

    protected override void Awake()
    {
        base.Awake();


    }

}

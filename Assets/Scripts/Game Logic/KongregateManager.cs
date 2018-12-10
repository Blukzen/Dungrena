using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongregateManager : Singleton<KongregateManager>
{
    // Use this for initialization
    void Start()
    {
        gameObject.name = "GameManager";
        Application.ExternalEval(@"if(typeof(kongregateUnitySupport) != 'undefined'){ kongregateUnitySupport.initAPI('GameManager', 'OnKongregateAPILoaded'); };");
    }

    public void OnKongregateAPILoaded(string userInfoString)
    {
        OnKongregateUserInfo(userInfoString);
    }

    public void OnKongregateUserInfo(string userInfoString)
    {
        var info = userInfoString.Split('|');
        var userId = System.Convert.ToInt32(info[0]);
        var username = info[1];
        var gameAuthToken = info[2];
        Debug.Log("Kongregate User Info: " + username + ", userId: " + userId);
    }
}

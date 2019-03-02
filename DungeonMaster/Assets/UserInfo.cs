using UnityEngine;
using System;

[Serializable]
public class UserInfo
{
    [SerializeField]
    public string id;
    [SerializeField]
    public string email;
    [SerializeField]
    public string password;

    public UserInfo(string id, string email, string password) {
        this.id = id;
        this.email = email;
        this.password = password;
    }
}

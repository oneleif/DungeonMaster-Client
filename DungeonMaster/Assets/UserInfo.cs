using UnityEngine;
using System;

[Serializable]
public class UserInfo
{
    //TODO: we can't send the id at first, and the JSONUtility needs to have [SerializeField]
    //So the only solutions I see are:
    //1. create 2 separate classes, one for post and one for getting the profile
    //2. allow unity to send an id at register but don't do anything with it
    int id = -1;
    [SerializeField]
    string email;
    [SerializeField]
    string password;

    public UserInfo(int id, string email, string password) {
        this.id = id;
        this.email = email;
        this.password = password;
    }
}

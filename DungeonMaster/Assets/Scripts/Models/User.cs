using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class User
{
    public string id;
    public string email;
    public string password;

    public User(string email, string password) {
        this.email = email;
        this.password = password;
    }
}

[Serializable]
public class UserList {
    public List<User> users;
}

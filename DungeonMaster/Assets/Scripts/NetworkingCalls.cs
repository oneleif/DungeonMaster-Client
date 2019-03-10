using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingCalls
{
    public static string GetJSONWithoutID(object o) {
        string json = JsonUtility.ToJson(o);
        return json.Replace("\"id\":\"\",", "");
    }
}

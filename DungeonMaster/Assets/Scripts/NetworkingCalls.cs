using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingCalls : MonoBehaviour
{
    IEnumerator GET(string url, string requestFunction, Type type) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        
        GlobalDebug.LogMessage("function: " + requestFunction + " waiting for request: " + www.url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed, url: " + www.url + " error: " + www.error + " body: " + System.Text.Encoding.UTF8.GetString(www.uploadHandler.data));
        } else {
            GlobalDebug.LogMessage("request succeeded, url: " + www.url + " responseCode: " + www.responseCode + " body: " + www.downloadHandler.text);
        }
        JsonUtility.FromJson<Type>(www.downloadHandler.text);
    }
    
    string GetJSONWithoutID(object o) {
        string json = JsonUtility.ToJson(o);
        return json.Replace("\"\"", "null");
    }

    UnityWebRequest CreatePostRequest(string url, byte[] formData, string route) {
        UnityWebRequest www = UnityWebRequest.Post(url, new WWWForm());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        return www;
    }
}

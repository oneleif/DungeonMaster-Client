using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using System;

public class DeveloperConsole : MonoBehaviour
{
    #region prefabs
    public GameObject headerText;
    public GameObject inputField;
    public GameObject deleteButton;
    public GameObject updateButton;
    public GameObject createButton;

    public Dropdown databaseSelector;

    public GameObject verticalLayoutGroup;
    public GameObject horizontalLayoutGroup;
    
    #endregion

    List<string> databases;
    public string currentTable;
    ItemList itemList;

    public string itemURL = "https://dungeonmaster-development.vapor.cloud/items";

    void Start()
    {
        databases = new List<string>();
        databases.Add("items");
        databases.Add("monsters");
        databaseSelector.ClearOptions();
        databaseSelector.AddOptions(databases);
        databaseSelector.onValueChanged.AddListener(delegate {
            DropdownValueChanged(databaseSelector);
        });
        databaseSelector.value = 0;
    }
    

    void DropdownValueChanged(Dropdown change) {
        currentTable = change.options[change.value].text;

        switch (change.value) {
            case 0:
                LoadItems();
                break;
        }
    }

    void LoadItems() {
        StartCoroutine(LOADITEMS());
    }

    IEnumerator LOADITEMS() {
        UnityWebRequest www = UnityWebRequest.Get(itemURL);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        GlobalDebug.LogMessage("function: LOADITEMS " + " waiting for request: " + www.url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed, url: " + www.url + " error: " + www.error);
        } else {
            GlobalDebug.LogMessage("request succeeded, url: " + www.url + " responseCode: " + www.responseCode + " body: " + www.downloadHandler.text);
            string json = www.downloadHandler.text;
            json = "{\"items\":" + json + "}";
            itemList = JsonUtility.FromJson<ItemList>(json);
        }
        ShowItems();
    }

    void ShowItems() {
        CreateHeader(typeof(Item));
        foreach (Item item in itemList.items) {
            GameObject horizontalGroup = Instantiate(horizontalLayoutGroup, verticalLayoutGroup.transform);
            FieldInfo[] fields = GetFields(item);
            foreach (FieldInfo field in fields) {
                GameObject input = Instantiate(inputField, horizontalGroup.transform);
                input.GetComponent<InputField>().text = field.GetValue(item).ToString();
            }
        }
    }

    void CreateHeader(Type type) {
        GameObject horizontalGroup = Instantiate(horizontalLayoutGroup, verticalLayoutGroup.transform);

        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (FieldInfo field in fields) {
            GameObject header = Instantiate(headerText, horizontalGroup.transform);
            header.GetComponent<Text>().text = field.Name;
        }
    }

    FieldInfo[] GetFields(object obj) {
        Type type = obj.GetType();
        return type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public void AddSelectedType() {
        StartCoroutine(ADDITEM());
    }

    IEnumerator ADDITEM() {
        Item item = new Item(null,"s", "s", "s", "s", 1, new Currency(0, 0, 0, 0, 0), 0);
        string json = GetJSONWithoutID(item);
        Debug.Log("JSON " + json);

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(formData);

        yield return StartCoroutine(WaitForRequest(www, "items"));
    }

    string GetJSONWithoutID(object o) {
        string json = JsonUtility.ToJson(o);
        return json.Replace("\"\"", "null");
    }

    IEnumerator WaitForRequest(UnityWebRequest www, string requestFunction) {

        GlobalDebug.LogMessage("function: " + requestFunction + " waiting for request: " + www.url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed, url: " + www.url + " error: " + www.error + " body: " + System.Text.Encoding.UTF8.GetString(www.uploadHandler.data));
        } else {
            GlobalDebug.LogMessage("request succeeded, url: " + www.url + " responseCode: " + www.responseCode + " body: " + www.downloadHandler.text);
        }
    }

    UnityWebRequest CreatePostRequest(byte[] formData) {
        UnityWebRequest www = UnityWebRequest.Post(itemURL, new WWWForm());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        return www;
    }
}

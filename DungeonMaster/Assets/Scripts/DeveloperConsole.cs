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

    public GameObject scrollViewContent;
    public GameObject horizontalLayoutGroup;
    #endregion

    List<string> databases;
    ItemList itemList;
    CharacterList characterList;

    public string baseURL = "https://dungeonmaster-development.vapor.cloud/";

    DataType currentDataType;

    public enum DataType {
        items, characters
    }

    void Start()
    {
        databases = new List<string>();
        databases.Add("items");
        databases.Add("characters");
        databaseSelector.ClearOptions();
        databaseSelector.AddOptions(databases);
        databaseSelector.onValueChanged.AddListener(delegate {
            DropdownValueChanged(databaseSelector);
        });
        DropdownValueChanged(databaseSelector);
    }
    
    void DropdownValueChanged(Dropdown change) {
        CleanScrollView();
        
        switch (change.value) {
            case 0:
                currentDataType = DataType.items;
                break;
            case 1:
                currentDataType = DataType.characters;
                break;
        }

        StartCoroutine(LoadTableRequest(baseURL + currentDataType));
    }

    void CleanScrollView() {
        foreach(Transform child in scrollViewContent.transform) {
            Destroy(child.gameObject);
        }
    }

    IEnumerator LoadTableRequest(string url) {
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed, url: " + www.url + " error: " + www.error);
        } else {
            GlobalDebug.LogMessage("request succeeded, url: " + www.url + " responseCode: " + www.responseCode + " body: " + www.downloadHandler.text);
            string json = www.downloadHandler.text;
            json = "{\"" + currentDataType.ToString() + "\":" + json + "}";
            SetList(json);
        }
    }

    void SetList(string json) {
        switch (currentDataType) {
            case DataType.items:
                itemList = JsonUtility.FromJson<ItemList>(json);
                ShowItems(typeof(Item), itemList.items);
                break;
            case DataType.characters:
                characterList = JsonUtility.FromJson<CharacterList>(json);
                ShowItems(typeof(Character), characterList.characters);
                break;
        }
    }

    void ShowItems(Type type, object[] objs) {
        CreateHeader(type);
        foreach (var obj in objs) {
            GameObject horizontalGroup = Instantiate(horizontalLayoutGroup, scrollViewContent.transform);
            FieldInfo[] fields = GetFields(obj);
            foreach (FieldInfo field in fields) {
                GameObject input = Instantiate(inputField, horizontalGroup.transform);
                input.GetComponent<InputField>().text = field.GetValue(obj).ToString();
            }
        }
    }

    void CreateHeader(Type type) {
        GameObject horizontalGroup = Instantiate(horizontalLayoutGroup, scrollViewContent.transform);

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

    public void CreateRecord() {
        CleanScrollView();
        StartCoroutine(CreateRecordRequest());
        DropdownValueChanged(databaseSelector);
    }

    IEnumerator CreateRecordRequest() {
        string json = GetJSONForType();

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(baseURL + currentDataType, formData);

        yield return StartCoroutine(WaitForRequest(www, currentDataType.ToString()));
    }

    string GetJSONForType() {
        string json = "";

        switch (currentDataType) {
            case DataType.items:
                Item item = Item.defaultItem;
                json = NetworkingCalls.GetJSONWithoutID(item);
                break;
            case DataType.characters:
                Character character = Character.defaultCharacter;
                json = NetworkingCalls.GetJSONWithoutID(character);
                break;
        }

        return json;
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

    UnityWebRequest CreatePostRequest(string url, byte[] formData) {
        UnityWebRequest www = UnityWebRequest.Post(url, new WWWForm());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        return www;
    }
}

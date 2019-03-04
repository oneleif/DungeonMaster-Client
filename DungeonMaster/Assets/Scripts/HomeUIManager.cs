using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    #region Public inspector references
    public GameObject homePanel;

    public GameObject signInPanel;
    public InputField emailSignInInput;
    public InputField passwordSignInInput;

    public GameObject registerPanel;
    public InputField emailRegisterInput;
    public InputField passwordRegisterInput;

    public GameObject tutorialPanel;

    public GameObject mainPanel;

    public GameObject adminPanel;
    #endregion

    //TODO: Extrapolate these out to a constants file or URL builder
    public string baseURL = "dungeonmaster-development.vapor.cloud";
    const string loginRoute = "login";
    const string registerRoute = "register";
    const string profileRoute = "profile";
    const string logoutRoute = "logout";
    const string URLPrefix = "https://";

    #region Private state variables
    User userInfo;

    public GameObject currentPanel;
    #endregion

    #region Mono Callbacks
    void Start()
    {
        CleanPanels();
        GoToHome();
    }

    private void Update() {
        if (Input.GetButtonDown("Console")) {
            currentPanel.SetActive(!currentPanel.activeInHierarchy);
            adminPanel.SetActive(!adminPanel.activeInHierarchy);
        }
    }
    #endregion

    #region Panel Controls
    public void CleanPanels() {
        homePanel.SetActive(false);
        signInPanel.SetActive(false);
        registerPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        mainPanel.SetActive(false);
        adminPanel.SetActive(false);
    }

    public void GoToHome() {
        CleanPanels();
        homePanel.SetActive(true);
        currentPanel = homePanel;
    }

    public void GoToSignIn() {
        CleanPanels();
        signInPanel.SetActive(true);
        currentPanel = signInPanel;
    }

    public void GoToRegister() {
        CleanPanels();
        registerPanel.SetActive(true);
        currentPanel = registerPanel;
    }

    public void GoToTutorials() {
        CleanPanels();
        tutorialPanel.SetActive(true);
        currentPanel = tutorialPanel;
    }

    public void GoToMain() {
        CleanPanels();
        mainPanel.SetActive(true);
        currentPanel = mainPanel;
    }

    #endregion

    #region Button Calls
    public void Register() {
        StartCoroutine(REGISTER());
    }

    public void LogIn() {
        StartCoroutine(LOGIN());
    }

    public void Logout() {
        StartCoroutine(LOGOUT());
    }
    #endregion

    #region Authentication Requests
    IEnumerator REGISTER() {
        userInfo = new User(emailRegisterInput.text, passwordRegisterInput.text);
        string json = GetJSONWithoutID(userInfo);
        
        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(formData, registerRoute);

        yield return StartCoroutine(WaitForRequest(www, registerRoute));
        GoToSignIn();
    }

    IEnumerator LOGIN() {
        userInfo = new User(emailSignInInput.text, passwordSignInInput.text);
        string json = GetJSONWithoutID(userInfo);

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(formData, loginRoute);

        yield return StartCoroutine(WaitForRequest(www, loginRoute));
        GoToMain();
        StartCoroutine(PROFILE());
    }

    IEnumerator PROFILE() {
        UnityWebRequest www = UnityWebRequest.Get(URLPrefix + baseURL + "/" + profileRoute);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return StartCoroutine(WaitForRequest(www, profileRoute));
        JsonUtility.FromJsonOverwrite(www.downloadHandler.text, userInfo);
    }

    IEnumerator LOGOUT() {
        UnityWebRequest www = UnityWebRequest.Get(URLPrefix + baseURL + "/" + logoutRoute);
        yield return StartCoroutine(WaitForRequest(www, logoutRoute));
        GoToHome();
    }
    #endregion

    #region Request Utilities
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

    UnityWebRequest CreatePostRequest(byte[] formData, string route) {
        UnityWebRequest www = UnityWebRequest.Post(URLPrefix + baseURL + "/" + route, new WWWForm());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        return www;
    }
    #endregion
}

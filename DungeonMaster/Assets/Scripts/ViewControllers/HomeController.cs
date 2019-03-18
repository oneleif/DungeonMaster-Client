using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HomeController : MonoBehaviour
{
    #region Public inspector references
    public GameObject homePanel;

    public GameObject signInPanel;
    public InputField emailSignInInput;
    public InputField passwordSignInInput;
    public Text signInErrorText;

    public GameObject registerPanel;
    public InputField emailRegisterInput;
    public InputField passwordRegisterInput;
    public Text registerErrorText;

    public GameObject tutorialPanel;

    public GameObject mainPanel;

    public GameObject adminPanel;
    #endregion

    //TODO: Extrapolate these out to a constants file or URL builder
    const string baseURL = "dungeonmaster-development.vapor.cloud";
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
        GoToPanel(homePanel);
    }

    public void GoToSignIn() {
        GoToPanel(signInPanel);
    }

    public void GoToRegister() {
        GoToPanel(registerPanel);
    }

    public void GoToTutorials() {
        GoToPanel(tutorialPanel);
    }

    public void GoToMain() {
        GoToPanel(mainPanel);
    }

    public void GoToPanel(GameObject go) {
        CleanPanels();
        go.SetActive(true);
        currentPanel = go;
    }

    #endregion

    #region Button Calls
    public void Register() {
        StartCoroutine(RegisterRequest());
    }

    public void LogIn() {
        StartCoroutine(LoginRequest());
    }

    public void Logout() {
        StartCoroutine(LogoutRequest());
    }
    #endregion

    #region Authentication Requests
    IEnumerator RegisterRequest() {
        userInfo = new User(emailRegisterInput.text, passwordRegisterInput.text);
        string json = NetworkingCalls.GetJSONWithoutID(userInfo);
        
        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(formData, registerRoute);

        bool failed = true;
        yield return StartCoroutine(WaitForRequest(www, registerRoute, value => failed = value));

        if (failed == false) {
            registerErrorText.text = "";
            GoToSignIn();
        } else {
            registerErrorText.text = "Register Failed";
        }
    }

    IEnumerator LoginRequest() {
        userInfo = new User(emailSignInInput.text, passwordSignInInput.text);
        string json = NetworkingCalls.GetJSONWithoutID(userInfo);

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest www = CreatePostRequest(formData, loginRoute);

        bool failed = true;
        yield return StartCoroutine(WaitForRequest(www, loginRoute, value => failed = value));

        if(failed == false) {
            signInErrorText.text = "";
            GoToMain();
            StartCoroutine(ProfileRequest());
        } else {
            signInErrorText.text = "Sign in failed";
        }
    }

    IEnumerator ProfileRequest() {
        UnityWebRequest www = UnityWebRequest.Get(URLPrefix + baseURL + "/" + profileRoute);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        bool failed = true;
        yield return StartCoroutine(WaitForRequest(www, profileRoute, value => failed = value));

        if (failed == false) {
            JsonUtility.FromJsonOverwrite(www.downloadHandler.text, userInfo);
        }
    }

    IEnumerator LogoutRequest() {
        UnityWebRequest www = UnityWebRequest.Get(URLPrefix + baseURL + "/" + logoutRoute);

        bool failed = true;
        yield return StartCoroutine(WaitForRequest(www, logoutRoute, value => failed = value));
        GoToHome();
    }
    #endregion

    #region Request Utilities
    

    IEnumerator WaitForRequest(UnityWebRequest www, string requestFunction, Action<bool> onFailed) {
        
        GlobalDebug.LogMessage("function: " + requestFunction + " waiting for request: " + www.url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed, url: " + www.url + " error: " + www.error + " body: " + System.Text.Encoding.UTF8.GetString(www.uploadHandler.data));
            onFailed(true);
        } else {
            GlobalDebug.LogMessage("request succeeded, url: " + www.url + " responseCode: " + www.responseCode + " response body: " + www.downloadHandler.text);
            onFailed(false);
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

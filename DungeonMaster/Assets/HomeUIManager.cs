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
    #endregion

    //TODO: Extrapolate these out to a constants file or URL builder
    const string baseURL = "dungeonmaster-development.vapor.cloud";
    const string loginRoute = "login";
    const string registerRoute = "register";
    const string profileRoute = "profile";
    const string logoutRoute = "logout";
    const string URLPrefix = "https://";

    #region Private state variables
    UserInfo userInfo;
    #endregion

    #region Mono Callbacks
    void Start()
    {
        CleanPanels();
        GoToHome();
    }
    #endregion

    #region Panel Controls
    public void CleanPanels() {
        homePanel.SetActive(false);
        signInPanel.SetActive(false);
        registerPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void GoToHome() {
        CleanPanels();
        homePanel.SetActive(true);
    }

    public void GoToSignIn() {
        CleanPanels();
        signInPanel.SetActive(true);
    }

    public void GoToRegister() {
        CleanPanels();
        registerPanel.SetActive(true);
    }

    public void GoToTutorials() {
        CleanPanels();
        tutorialPanel.SetActive(true);
    }

    public void GoToMain() {
        CleanPanels();
        mainPanel.SetActive(true);
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
        userInfo = new UserInfo(null, emailRegisterInput.text, passwordRegisterInput.text);
        byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(userInfo));
        UnityWebRequest www = CreatePostRequest(formData, registerRoute);

        yield return StartCoroutine(WaitForRequest(www, registerRoute));
        GoToSignIn();
    }

    IEnumerator LOGIN() {
        userInfo = new UserInfo(null, emailSignInInput.text, passwordSignInInput.text);
        byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(userInfo));
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
        Debug.Log(userInfo.id);
    }

    IEnumerator LOGOUT() {
        UnityWebRequest www = UnityWebRequest.Get(URLPrefix + baseURL + "/" + logoutRoute);
        yield return StartCoroutine(WaitForRequest(www, logoutRoute));
        GoToHome();
    }
    #endregion

    #region Request Utilities
    IEnumerator WaitForRequest(UnityWebRequest www, string requestFunction) {
        GlobalDebug.LogMessage("function: " + requestFunction + " waiting for request: " + www.url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            GlobalDebug.LogMessage("request failed: " + www.url + " " + www.error + " body: " + www.uploadHandler.data);
        } else {
            GlobalDebug.LogMessage("request succeeded: " + www.url + " responseCode: " + www.responseCode + " body: " + www.downloadHandler.text);
        }
    }

    UnityWebRequest CreatePostRequest(byte[] formData, string route) {
        UnityWebRequest www = UnityWebRequest.Post(URLPrefix + baseURL + "/" + route, new WWWForm());
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(formData);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json; charset=utf-8");
        return www;
    }

    byte[] GetUserInfoFormData() {
        userInfo = new UserInfo(null, emailRegisterInput.text, passwordRegisterInput.text);
        return System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(userInfo));
    }
    #endregion
}

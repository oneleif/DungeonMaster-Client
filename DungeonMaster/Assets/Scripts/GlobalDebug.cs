using UnityEngine;

public class GlobalDebug : MonoBehaviour
{
    [SerializeField]
    public static bool isInDebugMode = true;

    //Global debugger is useful to turn on/off general logging and focus in on one issue
    public static void LogMessage(string message) {
        if (isInDebugMode) {
            Debug.Log(message);
        }
    }
}

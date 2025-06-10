using UnityEngine;

public class OpenDevConsole : MonoBehaviour
{
    private void Start()
    {
        Debug.developerConsoleEnabled = true;
        Debug.LogError("Test error log message for the Dev Console.");
    }
}

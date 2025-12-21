using System;
using System.IO;
using UnityEngine;
using Input = UnityEngine.Input;

public class ScreenshotManager : MonoBehaviour
{
    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveScreenshot();
        }
#endif
    }
    
    private void SaveScreenshot()
    {
        // Create a timestamp string
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        // Create folder if it doesn't exist
        string folderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Full file path
        string filePath = Path.Combine(folderPath, $"screenshot_{timestamp}.png");

        // Take screenshot
        ScreenCapture.CaptureScreenshot(filePath);

        Debug.Log("Screenshot saved at: " + filePath);
    }
}

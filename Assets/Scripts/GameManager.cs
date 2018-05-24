using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static string guiLog = "Controls\n---------\nwasd - Movement\n1,2, 3 - Place Small, Med., Large Room" +
        "\nj - Place treasure\nk - Place enemy\np - Place Exit\nEsc - Save and close map\n----------\n";

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Quit"))
        {
            SaveAndCloseCurrentSession();
        }
	}

    private void OnGUI()
    {
        GUI.TextArea(new Rect(10, 10, Screen.width / 2 - 10, Screen.height - 10), guiLog, GUIStyle.none);
    }

    /// <summary>
    /// Saves the current map to a file and reloads the scene for a new level to be created
    /// </summary>
    void SaveAndCloseCurrentSession()
    {
        var currentDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        var filePath = Application.persistentDataPath + "/" + currentDateTime + "-level.dat";
        Level levelData = new Level(LevelManager.instance.GetLevel());
        File.WriteAllText(filePath, levelData.ToString());

        // Write debugging information for the location of the 
        LogToGui("Level written to: " + filePath);

        // Reload the scene for a fresh level generation session
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Writes a message to the application GUI
    /// </summary>
    /// <param name="message">The message to write to the GUI console</param>
    static void LogToGui(string message)
    {
        guiLog += (message + "\n");
    }
}

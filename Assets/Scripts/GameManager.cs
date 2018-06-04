using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    static string guiLog = "Controls\n---------\nwasd - Movement\n1,2, 3 - Place Small, Med., Large Room" +
        "\nj - Place treasure\nk - Place enemy\np - Place Exit\nu - Place key\ni - Place locked door\n" +
        "Esc - Save and close map\n----------\n";

    public static GameManager instance = null;

    public List<Step> demonstration;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        demonstration = new List<Step>();
        // Add the initial state with a null action
        State initialState = State.GetCurrentState();
        demonstration.Add(new Step(DiggerAction.None, initialState));
    }

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
        var levelFilePath = Application.persistentDataPath + "/" + currentDateTime + "-level.dat";
        var demoFilePath = Application.persistentDataPath + "/" + currentDateTime + "-demo.dat";
        var fullDemoFilePath = Application.persistentDataPath + "/" + currentDateTime + "-full-demo.dat";
        Level levelData = new Level(LevelManager.instance.GetLevel());
        var demoData = GetDemonstrationString();
        var fullDemoData = GetFullDemonstrationString();
        File.WriteAllText(levelFilePath, levelData.ToString());
        File.WriteAllText(demoFilePath, demoData);
        File.WriteAllText(fullDemoFilePath, fullDemoData);

        // Write debugging information for the location of the 
        LogToGui("Level written to: " + levelFilePath);
        LogToGui("Demo written to: " + demoFilePath);
        LogToGui("Full demo written to: " + fullDemoFilePath);

        // Reload the scene for a fresh level generation session
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Writes a message to the application GUI
    /// </summary>
    /// <param name="message">The message to write to the GUI console</param>
    public static void LogToGui(string message)
    {
        guiLog += (message + "\n");
    }

    /// <summary>
    /// Gets the string representation of the expert demonstration to be written to a file
    /// </summary>
    /// <returns></returns>
    private string GetDemonstrationString()
    {
        return string.Join("\n", demonstration.Select(n => n.ToString()).ToArray());
    }

    /// <summary>
    /// Gets the expanded string representation of the expert demonstration to be
    /// written to a file
    /// </summary>
    /// <returns></returns>
    private string GetFullDemonstrationString()
    {
        return string.Join("\n", demonstration.Select(n => n.GetFullStepString()).ToArray());
    }
}

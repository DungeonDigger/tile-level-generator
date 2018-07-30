using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoGameManager : MonoBehaviour {

    public List<TextAsset> levelFiles;
    public static DemoGameManager instance = null;

    private LevelManager levelManager;
    private int levelIndex = 0;
    [HideInInspector]
    public List<GameObject> enemies;
    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public bool doingSetup = false;
    [HideInInspector]
    public bool playerDied = false;

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(!instance.playerDied)
        {
            instance.levelIndex++;
            if (instance.levelIndex >= instance.levelFiles.Count)
                instance.levelIndex = 0;
        }
        else
        {
            instance.score = 0;
        }
        instance.InitGame();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();

        InitGame();
    }

    void InitGame()
    {
        GameObject.Find("LevelImage").SetActive(true);
        doingSetup = true;
        playerDied = false;

        levelManager.SetupScene(levelFiles[levelIndex]);
        var player = FindObjectOfType<Player>();
        player.transform.position = new Vector3(levelManager.GetLevel().GetLength(0) / 2 - 1,
            levelManager.GetLevel().GetLength(1) - 50f, 1);
        GetComponent<Camera>().transform.position = new Vector3(levelManager.GetLevel().GetLength(0) / 2 - 1,
            levelManager.GetLevel().GetLength(1) - 50, -10);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(doingSetup && Input.GetButtonDown("Sword"))
        {
            doingSetup = false;
            GameObject.Find("LevelImage").SetActive(false);
        }
        var player = GameObject.FindObjectOfType<Player>();
        GameObject.FindGameObjectWithTag("ScoreText").GetComponent<UnityEngine.UI.Text>().text =
            "Health: " + player.health + "\n" +
            "    : " + player.keyCount + "\n" +
            "Score: " + score;
        GameObject.FindGameObjectWithTag("LevelText").GetComponent<UnityEngine.UI.Text>().text =
            "Level " + (0 + levelIndex) + "\nPress Space to Continue";
    }
}

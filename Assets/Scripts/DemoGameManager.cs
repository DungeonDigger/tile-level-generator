using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGameManager : MonoBehaviour {

    public List<TextAsset> levelFiles;
    public static DemoGameManager instance = null;

    private LevelManager levelManager;
    private int levelIndex = 0;
    [HideInInspector]
    public List<GameObject> enemies;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        levelManager = GetComponent<LevelManager>();

        // Initialize the first level
        InitGame();
    }

    void InitGame()
    {
        levelManager.SetupScene(levelFiles[levelIndex]);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

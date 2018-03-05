using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static PackedMap playtest_map;
    public static bool endless_playtest = true;

    public static TempSceneRefs scene = new TempSceneRefs();

    private static GameManager instance;


    public static void GoToMenu()
    {
        instance.LoadScene(0);
    }


    public static void GoToEditor()
    {
        instance.LoadScene(1);
    }


    public static void GoToPlaytest(PackedMap _pmap = null)
    {
        playtest_map = _pmap;
        instance.LoadScene(2);
    }


    void Awake()
    {
        if (instance == null)
        {
            InitSingleton();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void InitSingleton()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += SceneLoaded;
    }


    void Update()
    {

    }


    void LoadScene(int _index)
    {
        AudioManager.StopAllSFX();
        SceneManager.LoadScene(_index);
    }


    // Find the GameState in the scene when its loaded.
    void SceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (instance != this)
            return;

        Time.timeScale = 1; // Always reset timeScale on scene load.
        GameState state = GameObject.FindObjectOfType<GameState>();

        if (state != null)
        {
            state.TriggerState();
        }
        else
        {
            Debug.LogWarning("No GameState found in scene.");
        }
    }

}

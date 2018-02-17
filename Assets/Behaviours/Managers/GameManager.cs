using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static PackedMap playtest_map { get; private set; }
    public static TempSceneRefs scene = new TempSceneRefs();

    private static GameManager instance;


    public static void StartPlaytest(PackedMap _pmap)
    {
        if (_pmap == null)
            return;

        playtest_map = _pmap;
        instance.LoadScene(1);
    }


    public static void ExitPlaytest()
    {
        instance.LoadScene(0);
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

        // Clicking Play in Editor does not call OnLevelWasLoaded. Thanks Unity.
#if UNITY_EDITOR
        OnLevelWasLoaded(0);
#endif
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    void LoadScene(int _index)
    {
        AudioManager.StopAllSFX();
        SceneManager.LoadScene(_index);
    }


    // Find the GameState in the scene when its loaded.
    void OnLevelWasLoaded(int _level)
    {
        if (instance != this)
            return;

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

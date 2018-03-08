using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float brightness { get { return (float)settings.brightness; } }

    public static PackedMap playtest_map;
    public static bool endless_playtest = true;

    public static TempSceneRefs scene = new TempSceneRefs();

    private static GameManager instance;
    private static GameSettings settings;


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


    public static void UpdateBrightness(float _brightness)
    {
        settings.brightness = Mathf.Clamp(_brightness, 0, 1);

        float b = (float)settings.brightness;
        RenderSettings.ambientLight = new Color(b, b, b);
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

        settings = FileIO.LoadSettings();
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
        UpdateBrightness((float)settings.brightness);

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


    void OnDestroy()
    {
        FileIO.SaveSettings(settings);
    }

}

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _PlayButton;
    [SerializeField] private string _LevelToLoad = "PedroScene";
    [SerializeField] private Button _OptionsButton;
    [SerializeField] private GameObject _OptionsScreen;
    [SerializeField] private Button _QuitButton;

    private void Start()
    {
        if (_PlayButton == null)
        {
            Debug.Log("Play Button is null");
        }
        
        if (_QuitButton == null)
        {
            Debug.Log("Level To Load is null");
        }
        
        if (_OptionsButton == null)
        {
            Debug.Log("Options Button is null");
        }
        
        if (_OptionsScreen == null)
        {
            Debug.Log("Options Screen is null");
        }
        
        if (_QuitButton == null)
        {
            Debug.Log("Quit Button is null");
        }
    }

    public void PlayGame() => SceneManager.LoadScene(_LevelToLoad);
    
    public void OpenOptions() => _OptionsScreen.SetActive(true);
    
    public void CloseOptions() => _OptionsScreen.SetActive(false);

    public void QuitGame()
    {
        // Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
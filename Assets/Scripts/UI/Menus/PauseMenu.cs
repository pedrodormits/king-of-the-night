using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _PauseScreen;
    [SerializeField] private GameObject _OptionsScreen;
    [SerializeField] private Player _Player;
    [SerializeField] private string _LevelToLoad; // MainMenuScene
    // [SerializeField] private GameObject _LoadingScreen;
    // [SerializeField] private GameObject _LoadingIcon;
    // [SerializeField] private TextMeshProUGUI _LoadingText;
    #endregion
    
    private void Start()
    {
        if (_PauseScreen == null)
        {
            Debug.Log("Pause Screen is null");
        }
        
        if (_OptionsScreen == null)
        {
            Debug.Log("Options Screen is null");
        }
        
        if (_Player == null)
        {
            Debug.Log("Player is null");
        }
        
        if (_LevelToLoad == null)
        {
            Debug.Log("Level To Load is null");
        }
        
        // if (_LoadingScreen == null)
        // {
        //     Debug.Log("Loading Screen is null");
        // }
        //
        // if (_LoadingIcon == null)
        // {
        //     Debug.Log("Loading Icon is null");
        // }
        //
        // if (_LoadingText == null)
        // {
        //     Debug.Log("Loading Text is null");
        // }
    }
    
    public void ResumeGame()
    {
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }
    
    public void OpenOptions() => _OptionsScreen.gameObject.SetActive(true);
    
    public void CloseOptions() => _OptionsScreen.SetActive(false);
    
    public void BackToMainMenu() => SceneManager.LoadScene(_LevelToLoad);
}
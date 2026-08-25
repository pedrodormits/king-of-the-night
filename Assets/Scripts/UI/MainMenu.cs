using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private string _levelToLoad;

    private void Start()
    {
        if (_playButton == null)
        {
            Debug.Log("Play Button is null");
        }
        
        if (_quitButton == null)
        {
            Debug.Log("Quit Button is null");
        }
        
        if (_quitButton == null)
        {
            Debug.Log("Level To Load is null");
        }
    }

    public void PlayGame() => SceneManager.LoadScene(_levelToLoad);
    
    public void QuitGame() => Application.Quit();
}
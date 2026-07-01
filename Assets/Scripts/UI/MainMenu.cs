using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private string _levelToLoad;
    
    public void PlayGame() => SceneManager.LoadScene(_levelToLoad);
    
    public void QuitGame() => Application.Quit();
}
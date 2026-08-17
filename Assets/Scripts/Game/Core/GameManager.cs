using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Canvas")]
    [SerializeField] private GameObject _GameOverScreen;
    [SerializeField] private GameObject _PauseScreen;
    [SerializeField] private string _LevelToLoad; // MainMenuScene

    [Header("Game State")]
    public bool GameIsOver {get; private set;}
    public bool GameIsPaused {get; private set;}
    [SerializeField] private PlayerInput _PlayerInput;
    [SerializeField] private Player _Player;
    
    private void Awake() => Instance = this;

    private void Start() => PrepareGame();

    private void Update() => PauseGame();
    
    private void PrepareGame()
    {
        _GameOverScreen.gameObject.SetActive(false);
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }
    
    private void PauseGame()
    {
        if (_PlayerInput.Pause)
        {
            GameIsPaused = !GameIsPaused;
            if (GameIsPaused)
            {
                _PauseScreen.gameObject.SetActive(true);
                _Player.enabled = false;
                Time.timeScale = 0;
            }
            else
            {
                _PauseScreen.gameObject.SetActive(false);
                _Player.enabled = true;
                Time.timeScale = 1;
            }
        }
    }

    public void GameOver()
    {
        GameIsOver = true;
        _GameOverScreen.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }
    
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    public void BackToMainMenu() => SceneManager.LoadScene(_LevelToLoad);
}
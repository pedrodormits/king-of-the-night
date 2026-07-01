using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("CANVAS")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private string _levelToLoad;

    [Header("GAME STATE")]
    public bool GameIsOver {get; private set;}
    public bool GameIsPaused {get; private set;}
    private PlayerInput _playerInput;

    private void Awake()
    {
        Instance = this;
        _playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Start()
    {
        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
    }

    private void Update() => PauseGame();

    private void PauseGame()
    {
        if (_playerInput.Pause)
        {
            GameIsPaused = !GameIsPaused;
            if (GameIsPaused)
            {
                _pauseScreen.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                _pauseScreen.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void GameOver()
    {
        GameIsOver = true;
        _gameOverScreen.gameObject.SetActive(true);
    }
    
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    public void BackToMainMenu() => SceneManager.LoadScene(_levelToLoad);
}
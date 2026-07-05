using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("CANVAS")]
    [SerializeField] private GameObject _GameOverScreen;
    [SerializeField] private GameObject _PauseScreen;
    [SerializeField] private string _LevelToLoad; // MainMenuScene

    [Header("GAME STATE")]
    public bool GameIsOver {get; private set;}
    public bool GameIsPaused {get; private set;}
    [SerializeField] private PlayerInput _PlayerInput;
    [SerializeField] private Player _Player;

    private void Awake() => Instance = this;

    private void Start() => PrepareGame();

    private void Update() => PauseGame();
    
    /// <summary>
    /// Prepares the game for active play by hiding UI screens, re-enabling the player,
    /// and restoring normal time progression.
    /// </summary>
    private void PrepareGame()
    {
        _GameOverScreen.gameObject.SetActive(false);
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }

    /// <summary>
    /// Toggles the pause state of the game when pause input is detected.
    /// When paused, it shows the pause screen, disables player control, and stops time.
    /// When resumed, it hides the pause screen, re-enables player control, and restores time.
    /// </summary>
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

    /// <summary>
    /// Ends the current game session by setting the game-over state and displaying the Game Over screen.
    /// </summary>
    public void GameOver()
    {
        GameIsOver = true;
        _GameOverScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Resumes gameplay by hiding the pause screen, re-enabling the player,
    /// and restoring normal time progression.
    /// </summary>
    public void ResumeGame()
    {
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }
    
    /// <summary>
    /// Restarts the current game by reloading the active scene.
    /// </summary>
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    
    /// <summary>
    /// Loads the main menu (or specified menu scene) by switching to the scene stored in _LevelToLoad.
    /// </summary>
    public void BackToMainMenu() => SceneManager.LoadScene(_LevelToLoad);
}
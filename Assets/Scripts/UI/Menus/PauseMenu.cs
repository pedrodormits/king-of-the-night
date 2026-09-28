using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject _PauseScreen;
    [SerializeField] private Player _Player;
    [SerializeField] private string _LevelToLoad; // MainMenuScene
    #endregion
    
    public void ResumeGame()
    {
        _PauseScreen.gameObject.SetActive(false);
        _Player.enabled = true;
        Time.timeScale = 1;
    }
    
    public void BackToMainMenu() => SceneManager.LoadScene(_LevelToLoad);
}
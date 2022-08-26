using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class StageController : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button exitButton;
    // Start is called before the first frame update
    private void Start()
    {
        mainMenuButton.onClick.AddListener(LoadMainMenu);
        exitButton.onClick.AddListener(Exit);
    }

    private void LoadMainMenu()
    {
        ResetPaused();
        SceneManage.Load(SceneManage.Scene.MainMenu);
    }
    
    private void Exit()
    {
        ResetPaused();
        SceneManage.Load(SceneManage.Scene.Quit);
    }
    
    private void ResetPaused()
    {
        Time.timeScale = 1f;
    }
}

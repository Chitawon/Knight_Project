using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button exitButton;
    void Start()
    {
        startButton.onClick.AddListener(LoadStartGame);
        controlButton.onClick.AddListener(LoadControlScene);
        creditButton.onClick.AddListener(LoadCredit);
        exitButton.onClick.AddListener(Exit);
    }

    private void LoadStartGame()
    {
        SceneManage.Load(SceneManage.Scene.SelectStage);
    }

    private void LoadControlScene()
    {
        SceneManage.Load(SceneManage.Scene.Control);
    }

    private void LoadCredit()
    {
        SceneManage.Load(SceneManage.Scene.Credit);
    }

    private void Exit()
    {
        SceneManage.Load(SceneManage.Scene.Quit);
    }
}

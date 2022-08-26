using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class SelectStageController : MonoBehaviour
{
    [SerializeField] private Button stage1Button;
    [SerializeField] private Button stage2Button;
    [SerializeField] private Button stage3Button;
    [SerializeField] private Button mainMenuButton;
    void Start()
    {
        stage1Button.onClick.AddListener(LoadStage1);
        stage2Button.onClick.AddListener(LoadStage2);
        stage3Button.onClick.AddListener(LoadStage3);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }
    
    private void LoadStage1()
    {
        SceneManage.Load(SceneManage.Scene.Map1);
    }
    
    private void LoadStage2()
    {
        SceneManage.Load(SceneManage.Scene.Map2);
    }
    
    private void LoadStage3()
    {
        SceneManage.Load(SceneManage.Scene.Map3);
    }
    
    private void LoadMainMenu()
    {
        SceneManage.Load(SceneManage.Scene.MainMenu);
    }
}

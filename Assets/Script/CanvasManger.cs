using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManger : MonoBehaviour
{
    public static bool GameIsPaused;
    [SerializeField] private GameObject pausedMenuUI;

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void Resume()
    {
        pausedMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Paused()
    {
        pausedMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}

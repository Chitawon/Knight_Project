using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public enum Scene
    {
        MainMenu,
        Control,
        Credit,
        SelectStage,
        Loading,
        Map1,
        Map2,
        Map3,
        Quit,
    }

    private static Action _onLoaderCallback;

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        int numberOfSceneManage = FindObjectsOfType<SceneManage>().Length;
        if (numberOfSceneManage > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void Load(Scene scene)
    {
        _onLoaderCallback = () =>
        {
            if (scene == Scene.Quit)
            {
                Application.Quit();
            }
            SceneManager.LoadScene(scene.ToString());
        };
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        if (_onLoaderCallback != null)
        {
            _onLoaderCallback();
            _onLoaderCallback = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    private bool _isFirstUpdate = true;
    
    // Update is called once per frame
    void Update()
    {
        if (_isFirstUpdate)
        {
            _isFirstUpdate = false;
            SceneManage.LoaderCallback();
        }
    }
}

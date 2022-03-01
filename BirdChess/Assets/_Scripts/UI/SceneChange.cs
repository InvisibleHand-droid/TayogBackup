using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void Btn_LoadScene(int buildIndex)
    {
        StartCoroutine(LoadingScene(buildIndex));
    }
    private IEnumerator LoadingScene(int buildIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(buildIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


}

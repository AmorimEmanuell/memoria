using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneInternal(sceneName));
    }

    private IEnumerator LoadSceneInternal(string sceneName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

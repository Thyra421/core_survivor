using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private static IEnumerator LoadSceneAsync(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone)
            yield return null;
    }

    public void LoadMenuAsync() {
        StartCoroutine(LoadSceneAsync("Menu"));
    }

    public void LoadGameAsync() {
        StartCoroutine(LoadSceneAsync("Game"));
    }
}

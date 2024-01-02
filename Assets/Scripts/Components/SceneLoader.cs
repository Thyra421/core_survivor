using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private GameObject loadingScreenPrefab;

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        LoadingScreenHUD loadingScreen = Instantiate(loadingScreenPrefab).GetComponent<LoadingScreenHUD>();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!asyncLoad.isDone) {
            loadingScreen.SetProgress(asyncLoad.progress);
            yield return null;
        }
    }

    public void LoadMenuAsync()
    {
        StartCoroutine(LoadSceneAsync("Menu"));
    }

    public void LoadDraftAsync()
    {
        StartCoroutine(LoadSceneAsync("Draft"));
    }

    public void LoadGameAsync()
    {
        StartCoroutine(LoadSceneAsync("Game"));
    }
}
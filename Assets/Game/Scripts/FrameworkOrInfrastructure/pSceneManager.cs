using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pSceneManager : MonoBehaviour
{
    public string scenesDefaultPath = "Assets/Game/Scenes/";

    public static pSceneManager instance;

    public System.Action OnStartLoadingScene;
    public System.Action OnNearlyDoneLoadingScene;
    public System.Action<float> OnDuringLoadingScene;

    public bool waitForInput = false;
    public void Awake()
    {
        var dups = FindObjectsOfType<pSceneManager>();
        foreach (var dup in dups)
            if (dup != this)
                Destroy(dup.gameObject);

        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadNewScene(string scenePath)
    {
        SceneManager.LoadScene(scenePath);   
    }
    public void LoadNewSceneAsync(string sceneName)
    {
        if (sceneName != "")
            StartCoroutine(LoadAsynchronously(sceneName));
    }
    public void LoadNewSceneWithDefaultPath(string sceneFileName)
    {
        LoadNewScene(scenesDefaultPath + sceneFileName);
    }
    public int GetSceneBuildIndexWithDefaultPath(string sceneFileName)
    {
        return GetSceneBuildIndex(scenesDefaultPath + sceneFileName);
    }
    public int GetSceneBuildIndex(string scenePath)
    {
        return SceneUtility.GetBuildIndexByScenePath(scenePath);
    }

    public SceneRef GetSceneRef(int sceneBuildIndex)
    {
        return SceneRef.FromIndex(sceneBuildIndex);
    }

    public SceneRef GetSceneRef(string sceneFullPath)
    {
        var idx = GetSceneBuildIndex(sceneFullPath);
        return GetSceneRef(idx);
    }

    public SceneRef GetSceneRefDefaultPath(string sceneFileName)
    {        
        return GetSceneRef(scenesDefaultPath + sceneFileName);
    }

    IEnumerator LoadAsynchronously(string sceneName)
    { // scene name is just the name of the current scene being loaded
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        OnStartLoadingScene?.Invoke();
        //mainCanvas.SetActive(false);
        //loadingMenu.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .95f);
            //loadingBar.value = progress;

            OnDuringLoadingScene?.Invoke(progress);

            //if (operation.progress >= 0.9f && waitForInput)
            //{
            //    loadPromptText.text = "Press " + userPromptKey.ToString().ToUpper() + " to continue";
            //    loadingBar.value = 1;

            //    if (Input.GetKeyDown(userPromptKey))
            //    {
            //        operation.allowSceneActivation = true;
            //    }
            //}
            if (operation.progress >= 0.9f && !waitForInput)
            {
                operation.allowSceneActivation = true;
                OnStartLoadingScene = null;
                OnDuringLoadingScene = null;
            }
            yield return null;
        }
    }
}

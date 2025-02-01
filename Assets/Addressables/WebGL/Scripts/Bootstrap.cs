using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Bootstrap : MonoBehaviour {
    [SerializeField] StartLoadingScreen _loadingScreen;
    [SerializeField] string _menuSceneName;
    
    SceneInstance nextScene;

    void Awake() {
        StartLoading().AttachExternalCancellation(destroyCancellationToken);
    }

    async UniTask StartLoading() {
        _loadingScreen.Show();

        try {
            await DownloadMenu();
            await DownloadSomeOtherAssets();
            
            _loadingScreen.ShowSimpleLog("Completing");

            await UniTask.WaitForSeconds(1.5f, cancellationToken: destroyCancellationToken);

            if (!nextScene.Scene.IsUnityNull()) 
                await nextScene.ActivateAsync();
        }
        catch (Exception e) {
            _loadingScreen.ShowSimpleLog("ERROR"); 
            Debug.LogError(e);
            throw;
        }
    }
    
    async UniTask DownloadMenu() {
        var sceneLoad = Addressables.LoadSceneAsync(_menuSceneName, activateOnLoad: false);
        _loadingScreen.CreateLog("Menu Scene", sceneLoad);

        await TrackProgress(sceneLoad);

        nextScene = sceneLoad.Result;
    }
    
    async UniTask DownloadSomeOtherAssets() {
        Action onComplete = new(() => { });
        _loadingScreen.CreateLog("Some Other Assets", ref onComplete);
        
        float time = 0;
        while (time < 3f) {
            time += Time.deltaTime;
            _loadingScreen.SetProcentage(Mathf.InverseLerp(0, 3, time));

            await UniTask.Yield(destroyCancellationToken);
        }

        await UniTask.Yield(destroyCancellationToken);

        if (!(time < 3f)) 
            onComplete.Invoke();
    }
    
    async UniTask TrackProgress(AsyncOperationHandle operation){
        while (!operation.IsDone) {
            _loadingScreen.SetProcentage(operation.PercentComplete);

            await UniTask.Yield(destroyCancellationToken);
        }
    }
}

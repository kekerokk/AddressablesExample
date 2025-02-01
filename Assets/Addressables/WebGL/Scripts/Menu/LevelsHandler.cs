using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour {
    [SerializeField] Button _toLevelsButton;
    [SerializeField] LevelPreview _levelUIPrefab;
    [SerializeField] RectTransform _container;

    [SerializeField] string _levelsDataAddress;
    [SerializeField] LevelsData _data;

    [SerializeField] List<RectTransform> _levels;

    const float MARGIN_X = 50f;
    bool _levelLoading;
    
    async UniTaskVoid Awake() {
        _toLevelsButton.interactable = false;
        await LoadData();
        _toLevelsButton.interactable = true;
        
        foreach (LevelData level in _data.levels) {
            LevelPreview levelUI = CreateLevelPreview(level.name);
            levelUI.OnClick += () => {
                if (!_levelLoading) 
                    LoadLevel(level.sceneAddressName).Forget();
            };
        }
    }

    LevelPreview CreateLevelPreview(string presentName) {
        LevelPreview levelPreview = Instantiate(_levelUIPrefab, _container);
        float previewWidth = levelPreview.rectTransform.rect.width;

        levelPreview.rectTransform.localPosition = _levels.Count > 0
            ? new Vector3(_levels[^1].localPosition.x + MARGIN_X + previewWidth, 0)
            : new Vector3(-_container.rect.width / 2f + MARGIN_X + previewWidth / 2f, 0);
        
        _levels.Add(levelPreview.rectTransform);
        levelPreview.SetLevelName(presentName);

        return levelPreview;
    }
    
    async UniTaskVoid LoadLevel(string keyname) {
        _levelLoading = true;

        try {
            await Addressables.LoadSceneAsync(keyname);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
        finally {
            await UniTask.WaitForSeconds(2f);
            _levelLoading = false;
        }
    }
    async UniTask LoadData() {
        _data = await Addressables.LoadAssetAsync<LevelsData>(_levelsDataAddress);
    }
}

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour {
    [SerializeField] RectTransform _mainRect;
    [SerializeField] Button _toLevels, _toMenu;
    [SerializeField] List<RectTransform> _panels;
    [SerializeField] Ease _slideType;

    float screenHeight => _mainRect.rect.height;
    float screenWidth => _mainRect.rect.width;

    // float marginSpace => screenWidth / 20f;
    int _slideValue = 0;
    
    void Awake() {
        SortPanels();
    }

    void SortPanels() {
        for (int i = 0; i < _panels.Count; i++) {
            RectTransform panel = _panels[i];
            panel.gameObject.SetActive(true);
            panel.localPosition = new Vector3(screenWidth * i, panel.localPosition.y, panel.localPosition.z);
        }
    }
    
    public void SlideAllPanels(int dir) {
        _slideValue += dir;
        for (int i = 0; i < _panels.Count; i++) {
            RectTransform panel = _panels[i];
            panel.DOLocalMove(new Vector3(screenWidth * (i + _slideValue), panel.localPosition.y, panel.localPosition.z), 0.7f).SetEase(_slideType);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class StartLoadingScreen : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] RectTransform _parent, _logPrefab, _logsParent;
    [SerializeField] Slider _progress;
    [SerializeField] float _maxDot = 5;
    List<RectTransform> _logs = new();

    [SerializeField] RectTransform _rectTransform;
    string _baseText = "Loading";
    int _dotCount = 0;
    Coroutine _textUpdater;

    float height => _rectTransform.rect.height;
    float width => _rectTransform.rect.width;
    const float RIGHT_MARGIN = 15, UP_MARGIN = 7;

    void OnValidate() {
        if (!_rectTransform) TryGetComponent(out _rectTransform);
    }

    public void CreateLog(string text, ref Action onComplete) {
        RectTransform log = Instantiate(_logPrefab, _parent);
        log.GetComponent<TextMeshProUGUI>().text = text;

        onComplete += () => {
            MoveLog(log);
        };
    }
    public void CreateLog(string text, AsyncOperationHandle operation) {
        RectTransform log = Instantiate(_logPrefab, _parent);
        log.GetComponent<TextMeshProUGUI>().text = text;

        operation.Completed += oper => {
            MoveLog(log);
        };
    }
    void MoveLog(RectTransform log) {
        log.parent = _logsParent;
        log.DOLocalMoveX(-log.rect.width / 2 - RIGHT_MARGIN, 0.5f);
        log.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Right;
        _logs.ForEach(x => x.DOLocalMoveY(log.rect.height / 2 + UP_MARGIN, 0.5f).SetRelative());
        _logs.Add(log);
    }

    IEnumerator UpdateText() {
        while (true) {
            _dotCount++;
            if (_dotCount > 5) _dotCount = 0;

            _text.text = _baseText;

            for (int i = 0; i < _dotCount; i++) {
                _text.text += ".";
            }

            yield return new WaitForSeconds(0.15f);
        }
    }

    public void Show() {
        _text.font = TMP_Settings.defaultFontAsset;
        gameObject.SetActive(true);
        _textUpdater = StartCoroutine(UpdateText());
    }
    public void Hide() {
        StopCoroutine(_textUpdater);
        gameObject.SetActive(false);
    }
    public void SetProcentage(float value) {
        _progress.value = value;
    }
    public void ShowSimpleLog(string log) {
        _baseText = log;
    }
}

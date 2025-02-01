using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPreview : MonoBehaviour, IPointerClickHandler {
    [SerializeField] TextMeshProUGUI _previewText;
    
    public event Action OnClick = delegate { };
    [field: SerializeField] public RectTransform rectTransform { get; private set; }

    public void SetLevelName(string name) {
        _previewText.text = name;
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        OnClick.Invoke();
    }
}

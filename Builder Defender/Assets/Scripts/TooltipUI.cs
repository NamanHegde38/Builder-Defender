using System;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour {
    
    public static TooltipUI Instance { get; private set; }
    
    [SerializeField] private RectTransform canvasRectTransform;
    
    private RectTransform _rectTransform;
    private TextMeshProUGUI _textMeshPro;
    private RectTransform _backgroundRectTransform;
    private TooltipTimer _tooltipTimer;

    private void Awake() {
        Instance = this;
        
        _rectTransform = GetComponent<RectTransform>();
        _textMeshPro = transform.Find("text").GetComponent<TextMeshProUGUI>();
        _backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();

        Hide();
    }

    private void Update() {
        HandleFollowMouse();
        
        if (_tooltipTimer != null) {
            _tooltipTimer.Timer -= Time.deltaTime;
            if (_tooltipTimer.Timer <= 0) {
                Hide();
            }
        }
    }

    private void HandleFollowMouse() {
        var anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if (anchoredPosition.x + _backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
            anchoredPosition.x = canvasRectTransform.rect.width - _backgroundRectTransform.rect.width;
        }
        
        if (anchoredPosition.y + _backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
            anchoredPosition.y = canvasRectTransform.rect.height - _backgroundRectTransform.rect.height;
        }
        
        _rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText) {
        _textMeshPro.SetText(tooltipText);
        _textMeshPro.ForceMeshUpdate();

        var textSize = _textMeshPro.GetRenderedValues(false);
        var padding = new Vector2(8, 8);
        _backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null) {
        _tooltipTimer = tooltipTimer;
        gameObject.SetActive(true);
        SetText(tooltipText);
        HandleFollowMouse();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public class TooltipTimer {
        public float Timer;
    }
}

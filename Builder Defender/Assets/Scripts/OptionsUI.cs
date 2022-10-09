using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI _soundVolumeText;
    private TextMeshProUGUI _musicVolumeText;
    
    private void Awake() {
        _soundVolumeText = transform.Find("soundVolumeText").GetComponent<TextMeshProUGUI>();
        _musicVolumeText = transform.Find("musicVolumeText").GetComponent<TextMeshProUGUI>();
        
        transform.Find("soundIncreaseBtn").GetComponent<Button>().onClick.AddListener((() => {
            soundManager.IncreaseVolume();
            UpdateText();
        }));
        transform.Find("soundDecreaseBtn").GetComponent<Button>().onClick.AddListener((() => {
            soundManager.DecreaseVolume();
            UpdateText();
        }));
        transform.Find("musicIncreaseBtn").GetComponent<Button>().onClick.AddListener((() => {
            musicManager.IncreaseVolume();
            UpdateText();
        }));
        transform.Find("musicDecreaseBtn").GetComponent<Button>().onClick.AddListener((() => {
            musicManager.DecreaseVolume();
            UpdateText();
        }));
        transform.Find("edgeScrollingToggle").GetComponent<Toggle>().onValueChanged.AddListener(((set) => {
            CameraHandler.Instance.SetEdgeScrolling(set);
        }));
        transform.Find("mainMenuBtn").GetComponent<Button>().onClick.AddListener((() => {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        }));
    }

    private void Start() {
        UpdateText();
        gameObject.SetActive(false);
        
        transform.Find("edgeScrollingToggle").GetComponent<Toggle>().SetIsOnWithoutNotify(CameraHandler.Instance.GetEdgeScrolling());
    }

    private void UpdateText() {
        _soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 10).ToString());
        _musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 10).ToString());
    }

    public void ToggleVisible() {
        gameObject.SetActive(!gameObject.activeSelf);

        Time.timeScale = gameObject.activeSelf ? 0f : 1f;
    }
}

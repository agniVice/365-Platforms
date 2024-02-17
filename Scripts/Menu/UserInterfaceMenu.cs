using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class UserInterfaceMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _scorePanel;

    [SerializeField] private CanvasGroup _mainBackground;
    [SerializeField] private CanvasGroup _secondBackground;

    [SerializeField] private TextMeshProUGUI _firstScore;
    [SerializeField] private TextMeshProUGUI _secondScore;
    [SerializeField] private TextMeshProUGUI _thirdScore;

    [SerializeField] private Slider _audioSlider;
    [SerializeField] private Slider _musicSlider;

    [SerializeField] private Image _vibrationButton;
    [SerializeField] private Sprite _vibrationEnabled;
    [SerializeField] private Sprite _vibrationDisabled;

    [SerializeField] private RectTransform _fadeIn;
    [SerializeField] private RectTransform _fadeOut;

    [SerializeField] private List<Transform> _transformsMenu = new List<Transform>();
    [SerializeField] private List<Transform> _transformsOption = new List<Transform>();
    [SerializeField] private List<Transform> _transformsScore = new List<Transform>();

    private void Start()
    {
        _fadeOut.DOLocalMoveY(-5262f, 0.5f).SetLink(_fadeOut.gameObject);

        OpenMenu();

        UpdateVibrationImage();
    }
    private void UpdateVibrationImage()
    {
        if (AudioVibrationManager.Instance.IsVibrationEnabled)
            _vibrationButton.sprite = _vibrationEnabled;
        else
            _vibrationButton.sprite = _vibrationDisabled;
    }
    public void OpenMenu()
    {
        //_menuPanel.transform.localScale = Vector3.zero;
        //_menuPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetLink(_menuPanel);
        _menuPanel.SetActive(true);
        _optionsPanel.SetActive(false);
        _scorePanel.SetActive(false);

        foreach (var item in _transformsMenu)
        {
            item.localScale = Vector3.zero;
            item.DOScale(1, Random.Range(0.15f, 0.3f)).SetLink(item.gameObject).SetEase(Ease.OutBack);
        }

        _mainBackground.DOFade(1, 0.2f).SetLink(_mainBackground.gameObject);
        _secondBackground.DOFade(0, 0.2f).SetLink(_secondBackground.gameObject);

        AudioVibrationManager.Instance.Save();
    }
    public void OpenOptions()
    {
        //_optionsPanel.transform.localScale = Vector3.zero;
        //_optionsPanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetLink(_optionsPanel);
        _menuPanel.SetActive(false);
        _optionsPanel.SetActive(true);

        foreach (var item in _transformsOption)
        {
            item.localScale = Vector3.zero;
            item.DOScale(1, Random.Range(0.15f, 0.3f)).SetLink(item.gameObject).SetEase(Ease.OutBack);
        }

        _mainBackground.DOFade(0, 0.2f).SetLink(_mainBackground.gameObject);
        _secondBackground.DOFade(1, 0.2f).SetLink(_secondBackground.gameObject);

        _audioSlider.value = AudioVibrationManager.Instance.Audio;
        _musicSlider.value = AudioVibrationManager.Instance.Music;
    }
    private void OpenScore()
    {
        //_scorePanel.transform.localScale = Vector3.zero;
        //_scorePanel.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).SetLink(_optionsPanel);

        _firstScore.text =  PlayerPrefs.GetInt("Score1", 0).ToString();
        _secondScore.text = PlayerPrefs.GetInt("Score2", 0).ToString();
        _thirdScore.text =  PlayerPrefs.GetInt("Score3", 0).ToString();

        _menuPanel.SetActive(false);
        _scorePanel.SetActive(true);

        foreach (var item in _transformsScore)
        {
            item.localScale = Vector3.zero;
            item.DOScale(1, Random.Range(0.15f, 0.3f)).SetLink(item.gameObject).SetEase(Ease.OutBack);
        }

        _mainBackground.DOFade(0, 0.2f).SetLink(_mainBackground.gameObject);
        _secondBackground.DOFade(1, 0.2f).SetLink(_secondBackground.gameObject);
    }
    public void ChangeAudio()
    {
        AudioVibrationManager.Instance.ChangeAudio(_audioSlider.value);
    }
    public void ChangeMusic()
    {
        AudioVibrationManager.Instance.ChangeMusic(_musicSlider.value);
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
    public void OnPlayButtonClicked()
    {
        _fadeIn.DOLocalMove(Vector2.zero, 0.5f).SetLink(_fadeIn.gameObject);
        Invoke("LoadGame", 0.52f);
    }
    private void LoadGame()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnSettingsButtonClicked()
    {
        OpenOptions();
    }
    public void OnReturnToMenuButtonCliked()
    {
        OpenMenu();
    }
    public void OnScoreButtonClicked()
    {
        OpenScore();
    }
    public void OnVibrationButtonClicked()
    {
        AudioVibrationManager.Instance.ToggleVibration();
        UpdateVibrationImage();
    }
}

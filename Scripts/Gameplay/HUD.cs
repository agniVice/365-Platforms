using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class HUD : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _infoPanel;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private RectTransform _fadeOut;
    [SerializeField] private Image _whiteFade;

    private bool _isInitialized;

    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void Initialize()
    {
        _fadeOut.DOLocalMoveY(2300, 0.5f).SetLink(_fadeOut.gameObject);

        GetComponent<Canvas>().worldCamera = Camera.main;

        Show();

        _isInitialized = true;

        _scoreText.text = "0";

        UpdateScore();
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Hide;
        GameState.Instance.GamePaused += Hide;
        GameState.Instance.GameUnpaused += Show;

        GameState.Instance.ScoreAdded += UpdateScore;
        GameState.Instance.LevelAdded += OnLevelAdded;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Hide;
        GameState.Instance.GamePaused -= Hide;
        GameState.Instance.GameUnpaused -= Show;

        GameState.Instance.ScoreAdded -= UpdateScore;
        GameState.Instance.LevelAdded -= OnLevelAdded;
    }
    private void UpdateScore()
    {
        _scoreText.text = PlayerScore.Instance.Score.ToString();
    }
    private void Show()
    {
        _panel.SetActive(true);
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    private void OnLevelAdded()
    {
        _whiteFade.DOFade(1f, 0.2f).SetLink(_whiteFade.gameObject);
        _whiteFade.DOFade(0f, 0.2f).SetLink(_whiteFade.gameObject).SetDelay(1f);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        _fadeOut.DOLocalMoveY(0, 0.5f).SetLink(_fadeOut.gameObject);
        Invoke("LoadMenu", 0.5f);
    }
    public void OnStartGameButtonClicked()
    {
        GameState.Instance.StartGame();
        _infoPanel.SetActive(false);
    }
    private void LoadMenu()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
    public void OnPauseButtonClicked()
    {
        GameState.Instance.PauseGame();
    }
}
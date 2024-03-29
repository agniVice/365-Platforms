using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public enum Languange
{
    English,
    Russian
}

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;
    public Languange CurrentLanguage;

    public  List<LocalizedString> LocalizedStrings = new List<LocalizedString>();

    [SerializeField] private List<string> _english;
    [SerializeField] private List<string> _russian;

    /* KEYS
     * 0 HIGHSCORE
     * 1 SETTINGS
     * 2 LOSE
     * 3 SCORE
     * 4 PAUSE
    */

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        Initialize();
    }
    private void Initialize()
    {
        CurrentLanguage = (Languange)PlayerPrefs.GetInt("Language", 0);
    }
    public void ToggleLanguange()
    {
        if (CurrentLanguage == Languange.English)
            CurrentLanguage = Languange.Russian;
        else if(CurrentLanguage == Languange.Russian)
            CurrentLanguage = Languange.English;

        PlayerPrefs.SetInt("Language", (int)CurrentLanguage);

        foreach (LocalizedString localizedString in LocalizedStrings)
        {
            if (localizedString != null)
                localizedString.LocalizeMe();
        }
    }
    public string GetTranslate(int keyIndex)
    {
        if (CurrentLanguage == Languange.English)
        {
            if (_english.Count > keyIndex)
                return _english[keyIndex];
            else
                return "Unknown";
        }
        else if(CurrentLanguage == Languange.Russian)
        {
            if (_russian.Count > keyIndex)
                return _russian[keyIndex];
            else
                return "Unknown";
        }
        return "Unknown";
    }
}

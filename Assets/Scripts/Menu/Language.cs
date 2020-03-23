using UnityEngine;

public class Language : MonoBehaviour
{
    [Header("Элементы меню")]
    [SerializeField] private TextTranslation[] texts;

    // Ссылки на компоненты
    private Languages languages;
    private TextTranslation textTranslation;

    private void Awake()
    {
        languages = Camera.main.GetComponent<Languages>();
        textTranslation = GetComponent<TextTranslation>();
    }

    private void Start()
    {
        SetupTranslation();
    }

    /// <summary>
    /// Установка перевода в зависимости от параметра языка
    /// </summary>
    private void SetupTranslation()
    {
        // Текущее значение параметра
        var language = PlayerPrefs.GetString("language");

        // Устанавливаем ключ перевода
        textTranslation.ChangeKey(language == "ru-RU" ? "language-rus" : "language-eng");
    }

    /// <summary>
    /// Переключение языка
    /// </summary>
    public void SwitchLanguage()
    {
        // Текущее значение параметра
        var language = PlayerPrefs.GetString("language");

        // Переключаем язык на другой
        PlayerPrefs.SetString("language", language == "ru-RU" ? "en-US" : "ru-RU");

        // Загружаем переводы из файла
        languages.LanguageSetting();

        SetupTranslation();

        // Обновляем переводы всех пунктов меню
        for (int i = 0; i < texts.Length; i++)
            texts[i].TranslateText();
    }
}
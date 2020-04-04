using UnityEngine;

public class Sound : MonoBehaviour
{
    // Активность звука в игре
    public static bool soundActivity;

    // Ссылки на компоненты
    private TextTranslation textTranslation;
    private Music backgroundMusic;

    private void Awake()
    {
        textTranslation = GetComponent<TextTranslation>();
    }

    private void Start()
    {
        // Находим объект фоновой музыки
        backgroundMusic = FindObjectOfType<Music>();

        // Текущее значение параметра
        soundActivity = PlayerPrefs.GetString("sounds") == "on" ? true : false;

        SetupTranslation();
    }

    /// <summary>
    /// Установка перевода в зависимости от сохраненного параметра
    /// </summary>
    private void SetupTranslation()
    {
        textTranslation.ChangeKey(soundActivity ? "sounds-on" : "sounds-off");
    }

    /// <summary>
    /// Переключение звука
    /// </summary>
    public void SwitchSound()
    {
        // Переключение звука на противоположное значение
        PlayerPrefs.SetString("sounds", soundActivity ? "off" : "on");

        // Обновляем текущее значение параметра
        soundActivity = PlayerPrefs.GetString("sounds") == "on" ? true : false;

        backgroundMusic.MusicSetting();

        // Обновляем перевод
        SetupTranslation();
    }
}
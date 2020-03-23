using UnityEngine;

public class Sound : MonoBehaviour
{
    // Ссылка на компонент перевода
    private TextTranslation textTranslation;

    private void Awake()
    {
        textTranslation = GetComponent<TextTranslation>();
    }

    private void Start()
    {
        SetupTranslation();
    }

    /// <summary>
    /// Установка перевода в зависимости от параметра звука
    /// </summary>
    private void SetupTranslation()
    {
        // Текущее значение параметра
        var sounds = PlayerPrefs.GetString("sounds");

        // Устанавливаем ключ перевода
        textTranslation.ChangeKey(sounds == "on" ? "sounds-on" : "sounds-off");
    }

    /// <summary>
    /// Переключение звука
    /// </summary>
    public void SwitchSound()
    {
        // Текущее значение параметра
        var sounds = PlayerPrefs.GetString("sounds");

        // Переключение звука на противоположное значение
        PlayerPrefs.SetString("sounds", sounds == "on" ? "off" : "on");

        // Обновляем перевод
        SetupTranslation();
    }
}
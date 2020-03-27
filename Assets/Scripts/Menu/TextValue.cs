using UnityEngine;

public class TextValue : TextTranslation
{
    [Header("Ключ сохранения")]
    [SerializeField] private string saveKey;

    protected override void Start()
    {
        TranslateText();
    }

    /// <summary>
    /// Вывод переведенного текста из словаря с добавлением сохраненного значения
    /// </summary>
    public override void TranslateText()
    {
        textComponent.text = Languages.translations[key] + " " + PlayerPrefs.GetInt(saveKey);
    }
}
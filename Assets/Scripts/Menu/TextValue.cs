using UnityEngine;

public class TextValue : TextTranslation
{
    [Header("Ключ сохранения")]
    [SerializeField] private string saveKey;

    protected override void Start()
    {
        // Выводим текст с необходимым значением
        textComponent.text = Languages.translations[key] + " " + PlayerPrefs.GetInt(saveKey);
    }
}
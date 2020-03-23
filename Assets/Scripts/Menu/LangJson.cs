using System;

[Serializable]
public class LangJson
{
    public Phrase[] language;
}

[Serializable]
public class Phrase
{
    // Ключ фразы
    public string Key;
    // Перевод фразы
    public string Value;
}
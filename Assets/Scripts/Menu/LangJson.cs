using System;

[Serializable]
public class LangJson
{
    public Phrase[] language;
}

[Serializable]
public class Phrase
{
    public string Key;
    public string Value;
}
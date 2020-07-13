using System;

namespace Cubra.Heplers
{
    [Serializable]
    public class LanguageHelper
    {
        public Phrase[] Phrase;
    }

    [Serializable]
    public class Phrase
    {
        public string Key;
        public string Value;
    }
}
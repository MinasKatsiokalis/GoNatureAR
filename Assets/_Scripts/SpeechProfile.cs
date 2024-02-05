using System;
using System.Collections;
using System.Collections.Generic;

namespace GoNatureAR
{
    [Serializable]
    public static class SpeechProfile
    {
        private static Dictionary<Keyword, string> Keywords = new Dictionary<Keyword, string>()
    {
        {Keyword.Julie , "Julie"},
        {Keyword.Continue , "Continue"},
        {Keyword.LetsGo , "Let's go"},
        {Keyword.Yes , "Yes"}
    };

        public static string GetKeywordToString(Keyword keyword)
        {
            return Keywords[keyword];
        }
    }

    [Serializable]
    public enum Keyword
    {
        Intro,
        Palm,
        Julie,
        Yes,
        Continue,
        LetsGo
    }
}

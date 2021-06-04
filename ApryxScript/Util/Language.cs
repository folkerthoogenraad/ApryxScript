using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Util
{
    public enum KeywordType
    {
        None,

        Function,
        Struct,
        Class,
        Var,
        Return,
    }

    public enum OperatorType
    {

    }

    public class Language
    {
        public static KeywordType GetKeywordType(string s)
        {
            switch (s)
            {
                case "function": return KeywordType.Function;
                case "return": return KeywordType.Function;
                default: return KeywordType.None;
            }
        }
    }
}

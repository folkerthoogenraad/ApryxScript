using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ApryxScript.Util
{
    public enum KeywordType
    {
        None,

        Native,
        Function,
        Struct,
        Class,
        Var,
        Return,
    }

    public enum OperatorType
    {
        None,

        Add,
        Subtract,
        Multiply,
        Divide,

        Equals,
        Arrow,
    }

    public class Language
    {
        public static Dictionary<string, KeywordType> Keywords;
        public static Dictionary<string, OperatorType> Operators;
        
        static Language()
        {
            Keywords = new Dictionary<string, KeywordType>();
            Operators = new Dictionary<string, OperatorType>();

            Keywords.Add("native", KeywordType.Native);
            Keywords.Add("function", KeywordType.Function);
            Keywords.Add("class", KeywordType.Class);
            Keywords.Add("struct", KeywordType.Struct);
            Keywords.Add("var", KeywordType.Var);

            Keywords.Add("return", KeywordType.Return);

            Operators.Add("+", OperatorType.Add);
            Operators.Add("-", OperatorType.Subtract);
            Operators.Add("*", OperatorType.Multiply);
            Operators.Add("/", OperatorType.Divide);

            Operators.Add("=", OperatorType.Equals);

            Operators.Add("=>", OperatorType.Arrow);
        }

        public static KeywordType GetKeywordType(string s)
        {
            if (Keywords.ContainsKey(s)) return Keywords[s];
            return KeywordType.None;
        }
        public static OperatorType GetOperatorType(string s)
        {
            if (Operators.ContainsKey(s)) return Operators[s];
            return OperatorType.None;
        }

        public static float GetFloatValue(string input)
        {
            return float.Parse(input, CultureInfo.InvariantCulture);
        }
        public static int GetIntValue(string input)
        {
            return int.Parse(input, CultureInfo.InvariantCulture);
        }
        public static double GetDoubleValue(string input)
        {
            return double.Parse(input, CultureInfo.InvariantCulture);
        }
    }
}

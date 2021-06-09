using ApryxScript.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Lexing
{
    public enum TokenType
    {
        Unknown, 

        Keyword,
        Identifier,
        Whitespace,

        BracketOpen,
        BracketClose,

        CurlyOpen,
        CurlyClose,

        Operator,

        Integer,
        Float,
        Double,
        String,

        Comma,
        Period,

        Colon,
        SemiColon,
    }

    public class Token
    {
        public int Line;
        public int StartIndex;
        public int EndIndex;

        public TokenType Type;

        public string Data;

        public Token(int line, int startIndex, int endIndex, TokenType type, string data)
        {
            Line = line;
            StartIndex = startIndex;
            EndIndex = endIndex;
            Type = type;
            Data = data;
        }

        public override string ToString()
        {
            return Type.ToString() + "(" + Data + ")";
        }

        public KeywordType KeywordType => Language.GetKeywordType(Data);
        public OperatorType OperatorType => Language.GetOperatorType(Data);
    }
}

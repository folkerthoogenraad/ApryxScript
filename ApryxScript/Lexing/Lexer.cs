using ApryxScript.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApryxScript.Lexing
{
    public class Lexer : IEnumerable<Token>
    {
        private LexReader Reader;

        public Lexer(Stream stream)
        {
            Reader = new LexReader(stream);
            Reader.Next();
        }

        public bool HasNext()
        {
            return Reader.HasCurrent();
        }

        public Token Next()
        {
            int startIndex = Reader.Index;
            char c = Reader.Current;

            if (IsAlphaNumeric(c))
            {
                StringBuilder builder = new StringBuilder();

                while (IsAlphaNumeric(Reader.Current))
                {
                    builder.Append(Reader.Current);
                    Reader.Next();
                }

                string data = builder.ToString();

                Token token = new Token(Reader.Line, startIndex, Reader.Index, IsKeyword(data) ? TokenType.Keyword : TokenType.Identifier, data);

                return token;
            }
            else if (IsWhitespace(c))
            {
                StringBuilder builder = new StringBuilder();

                while (IsWhitespace(Reader.Current))
                {
                    builder.Append(Reader.Current);
                    Reader.Next();
                }

                Token token = new Token(Reader.Line, startIndex, Reader.Index, TokenType.Whitespace, builder.ToString());

                return token;
            }
            else if (c == '"')
            {
                StringBuilder builder = new StringBuilder();

                // Consume the " 
                Reader.Next();

                while (Reader.Current != '"' && Reader.HasNext())
                {
                    builder.Append(Reader.Current);
                    Reader.Next();
                }

                // Consume the " 
                Reader.Next();

                Token token = new Token(Reader.Line, startIndex, Reader.Index, TokenType.String, builder.ToString());

                return token;
            }
            else if (IsNumeric(c))
            {
                StringBuilder builder = new StringBuilder();

                bool isFloatingPoint = false;

                while (IsNumeric(Reader.Current))
                {
                    builder.Append(Reader.Current);
                    Reader.Next();

                    if (Reader.Current == '.')
                    {
                        builder.Append('.');
                        Reader.Next();

                        isFloatingPoint = true;
                    }
                }

                TokenType type = TokenType.Integer;

                if (isFloatingPoint) type = TokenType.Double;

                if (Reader.Current == 'd')
                {
                    type = TokenType.Double;
                    Reader.Next();
                }
                if (Reader.Current == 'f')
                {
                    type = TokenType.Float;
                    Reader.Next();
                }

                Token token = new Token(Reader.Line, startIndex, Reader.Index, type, builder.ToString());

                return token;
            }
            else if (c == '(')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.BracketOpen, "(");
            }
            else if (c == ')')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.BracketClose, ")");
            }
            else if (c == '{')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.CurlyOpen, "{");
            }
            else if (c == '}')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.CurlyClose, "}");
            }
            else if (c == ',')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.CurlyClose, ",");
            }
            else if (c == '.')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.CurlyClose, ".");
            }
            else if (c == ':')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.Colon, ":");
            }
            else if (c == ';')
            {
                Reader.Next();

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.SemiColon, ";");
            }
            else if (c == '=')
            {
                Reader.Next();

                if(Reader.Current == '>')
                {
                    Reader.Next();
                    return new Token(Reader.Line, startIndex, Reader.Index, TokenType.Operator, "=>");
                }

                return new Token(Reader.Line, startIndex, Reader.Index, TokenType.Operator, "=");
            }
            else
            {
                Reader.Next();

                Token token = new Token(Reader.Line, startIndex, Reader.Index, TokenType.Unknown, "" + c);

                return token;
            }
        }

        public static bool IsAlphaNumeric(char c)
        {
            return c >= 'a' && c <= 'z'
                || c >= 'A' && c <= 'Z';
        }

        public static bool IsNumeric(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool IsWhitespace(char c)
        {
            return c == ' ' || c == '\n' || c == '\r' || c == '\t';
        }

        public static bool IsKeyword(string s)
        {
            return Language.GetKeywordType(s) != KeywordType.None;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            while (HasNext())
            {
                yield return Next();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

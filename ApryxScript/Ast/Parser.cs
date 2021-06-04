using ApryxScript.Lexing;
using ApryxScript.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Ast
{
    public class Parser
    {
        private int Index = 0;
        private List<Token> Tokens;

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public CompilationUnit ParseCompilationUnit()
        {
            CompilationUnit unit = new CompilationUnit();

            while (HasNext()) {
                unit.Statements.Add(ParseStatement());
            }

            return unit;
        }

        public Statement ParseStatement()
        {
            if (Current.Type == TokenType.Keyword)
            {
                KeywordType type = Language.GetKeywordType(Current.Data);

                switch (type)
                {
                    case KeywordType.Function: return ParseFunction();
                }
            }

            else if(Current.Type == TokenType.CurlyOpen)
            {
                return ParseBlockStatement();
            }

            // Unexpceted token
            return null;
        }

        public BlockStatement ParseBlockStatement()
        {
            if (Current.Type != TokenType.CurlyOpen) UnexpectedToken(Current, TokenType.CurlyOpen);
            Next();

            BlockStatement block = new BlockStatement();

            while(Current.Type != TokenType.CurlyClose && HasNext())
            {
                block.Statements.Add(ParseStatement());
            }

            if (Current.Type != TokenType.CurlyClose) UnexpectedToken(Current, TokenType.CurlyClose);
            Next();

            return block;
        }

        public FunctionStatement ParseFunction()
        {
            if (Current.Type != TokenType.Keyword) UnexpectedToken(Current, TokenType.Keyword);

            Next();

            FunctionStatement function = new FunctionStatement();

            function.Name = ParseTypeName();

            if (Current.Type != TokenType.BracketOpen) UnexpectedToken(Current, TokenType.BracketOpen);
            Next();

            // TODO parse the list in between :)

            if (Current.Type != TokenType.BracketClose) UnexpectedToken(Current, TokenType.BracketClose);
            Next();

            function.Body = ParseStatement();

            return function;
        }

        public TypeNameSyntax ParseTypeName()
        {
            if (Current.Type != TokenType.Identifier) UnexpectedToken(Current, TokenType.Identifier);

            TypeNameSyntax syntax = new TypeNameSyntax();

            syntax.Name = Current.Data;

            Next();

            return syntax;
        }


        public void UnexpectedToken(Token token)
        {
            throw new Exception("Unexpected token: " + token);
        }
        public void UnexpectedToken(Token token, TokenType expected)
        {
            throw new Exception("Unexpected token: " + token + ". Excpeted " + expected);
        }
        public void UnexpectedToken(Token token, TokenType expected, KeywordType keywordType)
        {
            throw new Exception("Unexpected token: " + token + ". Excpeted " + expected + " "+ keywordType);
        }

        public Token Next()
        {
            Index++;
            return Current;
        }

        public bool HasNext()
        {
            return Index <= Tokens.Count - 1;
        }

        public Token Current
        {
            get
            {
                if (Index >= Tokens.Count) return null;
                return Tokens[Index];
            }
        }
    }
}

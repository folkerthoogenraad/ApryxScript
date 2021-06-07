﻿using ApryxScript.Lexing;
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

        public Statement ParseStatement(bool native = false)
        {
            if (Current.Type == TokenType.Keyword)
            {
                KeywordType type = Language.GetKeywordType(Current.Data);

                switch (type)
                {
                    case KeywordType.Function: return ParseFunction(native);
                    case KeywordType.Native: Next(); return ParseStatement(true); // This is super ugly and hacky :)
                    case KeywordType.Return: return ParseReturnStatement();
                }
            }
            else if (Current.Type == TokenType.CurlyOpen)
            {
                return ParseBlockStatement();
            }
            else if (Current.Type == TokenType.SemiColon)
            {
                Next();
                return new EmptyStatement();
            }
            else
            {
                ExpressionStatement statement = new ExpressionStatement();

                statement.Expression = ParseExpression();
                return statement;
            }

            // Unexpceted token
            Next();
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

        public FunctionStatement ParseFunction(bool native)
        {
            if (Current.Type != TokenType.Keyword) UnexpectedToken(Current, TokenType.Keyword);

            Next();

            FunctionStatement function = new FunctionStatement();

            function.Name = ParseTypeName();

            if (Current.Type != TokenType.BracketOpen) UnexpectedToken(Current, TokenType.BracketOpen);
            Next();

            while(Current.Type != TokenType.BracketClose && HasNext())
            {
                // TODO parse parameter list?
                ParameterSyntax parameter = new ParameterSyntax();

                parameter.NameAndType = ParseNameAndType();

                function.Parameters.Add(parameter);

                if (Current.Type == TokenType.Comma) Next();
            }

            if (Current.Type != TokenType.BracketClose) UnexpectedToken(Current, TokenType.BracketClose);
            Next();

            if(Current.Type == TokenType.Operator && Current.OperatorType == OperatorType.Arrow)
            {
                Next();

                function.ReturnType = ParseTypeName();
            }

            function.Native = native;
            
            if (!function.Native)
            {
                function.Body = ParseStatement();
            }

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
        public NameAndTypeSyntax ParseNameAndType()
        {
            if (Current.Type != TokenType.Identifier) UnexpectedToken(Current, TokenType.Identifier);

            NameAndTypeSyntax syntax = new NameAndTypeSyntax();

            syntax.Name = Current.Data;
            Next();

            if (Current.Type != TokenType.Colon) UnexpectedToken(Current, TokenType.Colon);
            Next();

            syntax.Type = ParseTypeName();

            return syntax;
        }

        public ReturnStatement ParseReturnStatement()
        {
            if (Current.Type != TokenType.Keyword) UnexpectedToken(Current, TokenType.Keyword);
            Next();

            ReturnStatement statement = new ReturnStatement();

            statement.Expression = ParseExpression();

            return statement;
        }

        public Expression ParseExpression(int detail = 7)
        {
            if (detail >= 8)
            {
                return ParseExpression(7);
            }
            else if (detail > 2)
            {
                return ParseExpression(detail - 1);
            }

            else if(detail == 2)
            {
                return ParseExpression(detail - 1);
            }

            // Function invocation
            else if(detail == 1)
            {
                var expression = ParseExpression(detail - 1);

                if (Current.Type != TokenType.BracketOpen) return expression;
                Next();

                // Wrap the expression in function invocation
                InvocationExpression invoke = new InvocationExpression();

                invoke.Expression = expression;

                while(Current.Type != TokenType.BracketClose && HasNext())
                {
                    var exp = ParseExpression();

                    invoke.Arguments.Add(exp);

                    if (Current.Type == TokenType.Comma) Next();
                }

                if (Current.Type != TokenType.BracketClose) UnexpectedToken(Current, TokenType.BracketClose);
                Next();

                return invoke;
            }

            // Identifiers, Constants, Etc.
            else if(detail == 0)
            {
                Token t = Current;
                Next();

                if (t.Type == TokenType.String) return new StringConstantExpression(t.Data);
                if (t.Type == TokenType.Integer) return new IntegerConstantExpression(Language.GetIntValue(t.Data));
                if (t.Type == TokenType.Float) return new FloatConstantExpression(Language.GetFloatValue(t.Data));
                if (t.Type == TokenType.Double) return new DoubleConstantExpression(Language.GetDoubleValue(t.Data));
                if (t.Type == TokenType.Identifier) return new IdentifierExpression(t.Data);

                // We can't parse this...
                return null;
            }

            return null;
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

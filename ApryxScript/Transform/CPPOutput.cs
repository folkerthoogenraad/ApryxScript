﻿using ApryxScript.Ast;
using ApryxScript.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Transform
{
    public class CPPOutput
    {
        private StringBuilder Output;
        private int Tabs = 0;

        public CPPOutput()
        {
            Output = new StringBuilder();
        }

        public void Visit(SyntaxNode node)
        {
            Write("/*" + node.ToString() + "*/");
        }
        public void Write(SyntaxNode node)
        {
            if(node == null)
            {
                Write("/* null */");
                return;
            }
            Visit((dynamic)node);
        }

        public void Visit(CompilationUnit unit)
        {
            foreach (var f in unit.Statements)
            {
                Visit((dynamic)f);
            }
        }

        public void Visit(BlockStatement statement)
        {
            Write("{");
            IncreaseTabs();
            NewLine();

            for(int i = 0; i < statement.Statements.Count; i++)
            {
                bool last = i == (statement.Statements.Count - 1);
                var s = statement.Statements[i];

                Write(s);

                if (!last)
                {
                    NewLine();
                }
            }

            DecreaseTabs();
            NewLine();
            Write("}");
            NewLine();
        }
        public void Visit(ReturnStatement ret)
        {
            Write("return ");
            Write(ret.Expression);
            Write(";");
        }

        public void Visit(ParameterSyntax param)
        {
            Write(param.NameAndType);
        }
        public void Visit(FunctionSyntax function)
        {
            if(function.ReturnType != null)
            {
                Write(function.ReturnType);
            }
            else
            {
                Write("void");
            }
            Write(" ");
            Write(function.Name);
            Write("(");

            for(int i = 0; i < function.Parameters.Count; i++)
            {
                Write(function.Parameters[i]);
                if(i != function.Parameters.Count - 1)
                {
                    Write(",");
                }
            }

            Write(")");

            bool block = function.Body is BlockStatement && !function.Native;

            if (!block)
            {
                Write("{");
                IncreaseTabs();
                NewLine();
            }

            if (function.Native)
            {
                Write("// Implement the function here");
            }
            else
            {
                Write(function.Body);
            }

            if (!block)
            {
                DecreaseTabs();
                NewLine();
                Write("}");
                NewLine();
            }
        }
        public void Visit(TypeNameSyntax type)
        {
            Write(type.Name);
        }
        public void Visit(NameSyntax name)
        {
            Write(name.Name);
        }
        public void Visit(EmptyStatement name)
        {
            // Do nothgin :) its empty bro
            Write("/* empty */");
        }
        public void Visit(NameAndTypeSyntax nameAndType)
        {
            Write(nameAndType.Type);
            Write(" ");
            Write(nameAndType.Name);
        }
        public void Visit(ExpressionStatement expression)
        {
            Write(expression.Expression);
            Write(";");
        }


        // ================================================== //
        // Expressions
        // ================================================== //
        public void Visit(IntegerConstantExpression type)
        {
            Write("" + type.Value);
        }
        public void Visit(IdentifierExpression type)
        {
            Write(type.Name);
        }
        public void Visit(StringConstantExpression type)
        {
            Write("\"");
            Write(type.Value);
            Write("\"");
        }
        public void Visit(InvocationExpression invoke)
        {
            Write(invoke.Expression);

            Write("(");
            for (int i = 0; i < invoke.Arguments.Count; i++)
            {
                bool last = i == invoke.Arguments.Count - 1;

                Write(invoke.Arguments[i]);

                if (!last)
                {
                    Write(", ");
                }
            }
            Write(")");
        }

        public void Visit(MemberAccessExpression member)
        {
            Write(member.Expression);
            Write(".");
            Write(member.Member);
        }

        public void Visit(OperatorExpression op)
        {
            // Write("(");
            Write(op.LeftExpression);

            switch (op.Operator)
            {
                case OperatorType.Add: Write("+"); break;
                case OperatorType.Subtract: Write("-"); break;
                case OperatorType.Multiply: Write("*"); break;
                case OperatorType.Divide: Write("/"); break;
                default: Write("/* ? */"); break;
            }

            Write(op.RightExpression);
            // Write(")");
        }

        public void Visit(BracketedExpression brackets)
        {
            Write("(");
            Write(brackets.Expression);
            Write(")");
        }

        public string Result => Output.ToString();

        public void Write(string text)
        {
            Output.Append(text);
        }
        public void NewLine()
        {
            Write("\n");

            for (int i = 0; i < Tabs; i++)
                Write("  ");
        }

        public void IncreaseTabs()
        {
            Tabs++;
        }
        public void DecreaseTabs()
        {
            Tabs--;
        }
    }
}

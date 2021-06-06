using ApryxScript.Ast;
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

        }
        public void Write(SyntaxNode node)
        {
            Visit((dynamic)node);
        }

        public void Visit(CompilationUnit unit)
        {
            foreach (var f in unit.Statements)
            {
                Visit((dynamic)f);
            }
        }
        public void Visit(ParameterSyntax param)
        {
            Write(param.NameAndType);
        }
        public void Visit(FunctionStatement function)
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
            Write("{");
            IncreaseTabs();
            NewLine();

            if (function.Native)
            {
                Write("// Implement the function here");
            }
            else
            {
                Write(function.Body);
            }
            DecreaseTabs();
            NewLine();
            Write("}");
            NewLine();
        }
        public void Visit(TypeNameSyntax type)
        {
            Write(type.Name);
        }
        public void Visit(NameAndTypeSyntax nameAndType)
        {
            Write(nameAndType.Type);
            Write(" ");
            Write(nameAndType.Name);
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
                Write("\t");
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

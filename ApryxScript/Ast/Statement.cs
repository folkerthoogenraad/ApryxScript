using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Ast
{
    public class SyntaxNode
    {

    }

    public class Statement : SyntaxNode
    {

    }

    public class TypeNameSyntax : SyntaxNode
    {
        public string Name;
    }

    public class CompilationUnit : SyntaxNode
    {
        public List<Statement> Statements = new List<Statement>();
    }

    public class ClassStatement : Statement
    {

    }

    public class ParameterSyntax : SyntaxNode
    {

    }

    public class BlockStatement : Statement
    {
        public List<Statement> Statements = new List<Statement>();
    }

    public class FunctionStatement : Statement
    {
        public TypeNameSyntax Name;
        public List<ParameterSyntax> Parameters = new List<ParameterSyntax>();
        public SyntaxNode Body = null;
    }

}

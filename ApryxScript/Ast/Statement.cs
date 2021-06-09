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
    public class NameSyntax : SyntaxNode
    {
        public string Name;
    }

    public class NameAndTypeSyntax : SyntaxNode
    {
        public string Name;
        public TypeNameSyntax Type;
    }

    public class CompilationUnit : SyntaxNode
    {
        public List<Statement> Statements = new List<Statement>();
    }

    public class ClassStatement : Statement
    {

    }
    public class EmptyStatement : Statement
    {

    }

    public class ParameterSyntax : SyntaxNode
    {
        public NameAndTypeSyntax NameAndType;
    }
    public class ExpressionStatement : Statement
    {
        public Expression Expression;
    }
    public class ReturnStatement : Statement
    {
        public Expression Expression;
    }

    public class BlockStatement : Statement
    {
        public List<Statement> Statements = new List<Statement>();
    }

    public class FunctionStatement : Statement
    {
        public bool Native = false;

        public TypeNameSyntax Name;
        public TypeNameSyntax ReturnType;
        public List<ParameterSyntax> Parameters = new List<ParameterSyntax>();
        public SyntaxNode Body = null;
    }

}

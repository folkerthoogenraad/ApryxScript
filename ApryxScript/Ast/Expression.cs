using ApryxScript.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Ast
{
    public class Expression : SyntaxNode
    {
    }

    public class InvocationExpression : Expression
    {
        public Expression Expression;
        public List<Expression> Arguments = new List<Expression>();
    }

    public class OperatorExpression : Expression
    {
        public Expression LeftExpression;
        public OperatorType Operator;
        public Expression RightExpression;
    }

    public class BracketedExpression : Expression
    {
        public Expression Expression;
    }
    public class MemberAccessExpression : Expression
    {
        public Expression Expression;
        public NameSyntax Member;
    }

    public class IdentifierExpression : Expression
    {
        public string Name;

        public IdentifierExpression(string name)
        {
            Name = name;
        }
    }

    public class ConstantExpression<T> : Expression
    {
        public T Value;

        public ConstantExpression(T value)
        {
            Value = value;
        }
    }

    public class StringConstantExpression : ConstantExpression<string>
    {
        public StringConstantExpression(string value) : base(value) { }
    }

    public class IntegerConstantExpression : ConstantExpression<int>
    {
        public IntegerConstantExpression(int value) : base(value) { }
    }

    public class FloatConstantExpression : ConstantExpression<float>
    {
        public FloatConstantExpression(float value) : base(value) { }
    }

    public class DoubleConstantExpression : ConstantExpression<double>
    {
        public DoubleConstantExpression(double value) : base(value) { }
    }

}

using ApryxScript.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Typing
{
    public class Symbol
    {
        public SymbolScope EnclosedScope { get; set; } = null;
        public SymbolScope DefinesScope { get; set; } = null;
    }

    public class TypeSymbol : Symbol
    {

    }

    public class FunctionSymbol : TypeSymbol
    {
        public FunctionStatement Statement;
        public TypeSymbol ReturnType;
    }

    public class VariableSymbol : TypeSymbol
    {
        public TypeSymbol Type;
    }

    public class ParameterSymbol : VariableSymbol
    {
        public ParameterSyntax Parameter;
    }

    public class ExpressionSymbol : Symbol
    {
        public TypeSymbol Type;
        public FunctionSymbol Invocation; // Either explicit or implicit :)
    }


    public class ClassSymbol : TypeSymbol
    {

    }
}

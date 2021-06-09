using ApryxScript.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApryxScript.Typing
{
    public class Semantics
    {
        private Queue<SyntaxNode> List;

        public SymbolScope Root;

        public Semantics(CompilationUnit unit)
        {
            Root = new SymbolScope();
            List = new Queue<SyntaxNode>();

            List.Enqueue(unit);
        }

        public bool HasNext()
        {
            return List.Count > 0;
        }

        public void Next()
        {
            var node = List.Dequeue();

            if (node is FunctionSyntax) {
                var function = node as FunctionSyntax;
                var symbol = new FunctionSymbol();

                
            }
        }
    }
}

using ApryxScript.Ast;
using ApryxScript.Lexing;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ApryxScript
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Please supply first argument as a file to compile");
            }

            string filename = args[0];

            using (var stream = File.OpenRead(filename))
            {
                Lexer lexer = new Lexer(stream);

                var tokens = lexer.Where(token => token.Type != TokenType.Whitespace).ToList(); //

                Parser parser = new Parser(tokens);

                var unit = parser.ParseCompilationUnit();

                Console.WriteLine(unit);
            }

            return;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

    }
}

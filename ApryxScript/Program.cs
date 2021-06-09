using ApryxScript.Ast;
using ApryxScript.Lexing;
using ApryxScript.Transform;
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

            Stopwatch lexTime = new Stopwatch();
            Stopwatch parseTime = new Stopwatch();

            string filename = args[0];

            using (var stream = File.OpenRead(filename))
            {
                lexTime.Start();
                Lexer lexer = new Lexer(stream);

                var tokens = lexer.Where(token => token.Type != TokenType.Whitespace).ToList();
                
                lexTime.Stop();

                parseTime.Start();
                Parser parser = new Parser(tokens);

                var unit = parser.ParseCompilationUnit();
                parseTime.Stop();

                CPPOutput output = new CPPOutput();
                output.Visit(unit);

                Console.WriteLine(output.Result);

                Console.WriteLine();

                Console.WriteLine("Lexing took: " + lexTime.ElapsedMilliseconds + "ms");
                Console.WriteLine("Parsing took: " + parseTime.ElapsedMilliseconds + "ms");
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

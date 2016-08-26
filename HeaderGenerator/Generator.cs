using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaderGenerator
{
    public class Generator
    {
        private List<Token> tokens;
        Parser parser;

        public Generator(string dir, List<string> files)
        {
            tokens = Tokenize(dir, files);
            tokens.Add(new Token() { line = -1, value = "", type = TokenType.EOT });
            parser = new Parser(tokens);
        }

        private List<Token> Tokenize(string dir, List<string> files)
        {
            List<Token> t = new List<Token>();

            foreach (var f in files)
            {
                var filename = Path.Combine(dir, f);
                var l = new Lexer(filename);
                t.AddRange(l.tokens);
            }

            return t;
        }

        public void Dump(string headerfile, string sourcefile)
        {
            List<char> h = new List<char>();
            h.AddRange("#pragma once\n");

            List<char> cpp = new List<char>();
            cpp.AddRange("#include \"Header.h\"\n");

            parser.Dump(h, cpp);

            File.WriteAllText(headerfile, new string(h.ToArray()));
            File.WriteAllText(sourcefile, new string(cpp.ToArray()));
        }
    }
}

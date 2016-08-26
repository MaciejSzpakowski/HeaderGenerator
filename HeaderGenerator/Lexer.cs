using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace HeaderGenerator
{
    public enum TokenType
    {
        ClassProto, OpenBrace, CloseBrace, CloseBraceSemicolon, ClassField, MethodProto, MethodBody, PrivateKeyword, PublicKeyword, Include, ProtectedKeyword, EOT, InitializerList, NamespaceKeyword
    }

    public class Token
    {
        public string value;
        public TokenType type;
        public int line;
    }

    public class Lexer
    {
        public List<Token> tokens { get; }
        string filename;
        int index;
        string file;

        public Lexer(string f)
        {
            tokens = new List<Token>();
            filename = f;
            index = 0;
            Tokenize();
        }

        private void ConsumeWhiteSpace()
        {
            while (file[index] == ' ' || file[index] == '\n' || file[index] == '\r')
                index++;
        }

        private void Tokenize()
        {
            file = File.ReadAllText(filename);
            string id = "[_a-zA-Z0-9<>\\*]+( )*";
            string spc = "( )*";

            while (index < file.Length)
            {
                ///// DEBUG
                var substr = file.Substring(index);

                // skip spaces
                ConsumeWhiteSpace();

                if (file._IndexOf("class", index, "class".Length) == index)
                {
                    var match = new Regex($"\\Gclass {spc}{id}:?({spc},?{spc}(public|private|protected){spc}{id})*[\n\r ]*{{").Match(file, index);
                    var proto = match.Value.Pop(1);
                    proto = proto.Trim(' ', '\n', '\r');
                    tokens.Add(new Token() { type = TokenType.ClassProto, value = proto });

                    index += proto.Length;
                }
                else if (file._IndexOf("#include", index, "#include".Length) == index)
                {
                    var match = new Regex($"\\G#include {spc}(\"|<)[_a-zA-Z0-9\\.]+( )*(\"|>)").Match(file, index);
                    tokens.Add(new Token() { type = TokenType.Include, value = match.Value });

                    index += match.Value.Length;
                }
                else if (file._IndexOf("public:", index, "public:".Length) == index)
                {
                    tokens.Add(new Token() { type = TokenType.PublicKeyword, value = "public:" });

                    index += "public:".Length;
                }
                else if (file._IndexOf("private:", index, "private:".Length) == index)
                {
                    tokens.Add(new Token() { type = TokenType.PrivateKeyword, value = "private:" });

                    index += "private:".Length;
                }
                else if (file._IndexOf("protected:", index, "protected:".Length) == index)
                {
                    tokens.Add(new Token() { type = TokenType.ProtectedKeyword, value = "protected:" });

                    index += "protected:".Length;
                }
                else if (tokens.Count > 2 && (tokens[tokens.Count - 2].type == TokenType.MethodProto || tokens[tokens.Count - 2].type == TokenType.InitializerList)
                    && tokens[tokens.Count - 1].type == TokenType.OpenBrace)
                {
                    tokens.Add(FindMethodBody());
                    tokens.Add(new Token() { type = TokenType.CloseBrace, value = "}" });

                    index++;
                }
                else if (file[index] == '{')
                {
                    tokens.Add(new Token() { type = TokenType.OpenBrace, value = "{" });

                    index++;
                }
                else if (file._IndexOf("};", index, 2) == index)
                {
                    tokens.Add(new Token() { type = TokenType.CloseBraceSemicolon, value = "};" });

                    index += 2;
                }
                else if (file[index] == '}')
                {
                    tokens.Add(new Token() { type = TokenType.CloseBrace, value = "}" });

                    index++;
                }
                else if (file[index] == ':')
                {
                    index++;
                    while (file[index] == ' ' || file[index] == '\n' || file[index] == '\r')
                        index++;

                    var match = new Regex($"\\G[^{{]*").Match(file, index);
                    tokens.Add(new Token() { type = TokenType.InitializerList, value = match.Value });

                    index += match.Value.Length;
                }
                else if (file._IndexOf("namespace", index, "namespace".Length) == index)
                {
                    index += "namespace".Length;
                    ConsumeWhiteSpace();

                    var match = new Regex($"\\G{id}").Match(file, index);
                    tokens.Add(new Token() { type = TokenType.NamespaceKeyword, value = match.Value});

                    index += match.Value.Length;
                }
                else
                    tokens.Add(FindNextToken());
            }
        }

        private Token FindNextToken()
        {
            string id = "[_a-zA-Z0-9<>\\*:]+( )*";
            string spc = "( )*";

            var matchMethodProto = new Regex($"\\G({id} )?{id}\\((,?{spc}{id} {id})*\\)").Match(file, index);
            var matchField = new Regex($"\\G{id} {id};").Match(file, index);

            if (matchField.Value != "")
            {
                index += matchField.Value.Length;

                return new Token() { type = TokenType.ClassField, value = matchField.Value };
            }
            else if (matchMethodProto.Value != "")
            {
                index += matchMethodProto.Value.Length;

                return new Token() { type = TokenType.MethodProto, value = matchMethodProto.Value };
            }
            else
                throw new Exception("Unknown member " + index);
        }

        private Token FindMethodBody()
        {
            Token methodBody = new Token() { value = "", type = TokenType.MethodBody };

            int openBraceCount = 0;

            while (openBraceCount != 0 || file[index] != '}')
            {
                if (file[index] == '{')
                    openBraceCount++;
                else if (file[index] == '}')
                    openBraceCount--;

                methodBody.value += file[index];
                index++;
            }

            return methodBody;
        }
    }
}

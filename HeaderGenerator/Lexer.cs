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
        ClassStructProto, OpenBrace, CloseBrace, CloseBraceSemicolon, ClassStructField, MethodProto,
        MethodBody, PrivateKeyword, PublicKeyword, Include, ProtectedKeyword, EOT,
        InitializerList, NamespaceKeyword
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
            Match memberMatch = null;

            while (index < file.Length)
            {
                ///// DEBUG
                //var substr = file.Substring(index);
                //Token t;
                //try
                //{
                //    t = tokens.Last();
                //}
                //catch
                //{
                //}

                // skip spaces
                ConsumeWhiteSpace();

                if (tokens.Count > 2 && (tokens[tokens.Count - 2].type == TokenType.MethodProto || tokens[tokens.Count - 2].type == TokenType.InitializerList)
                    && tokens[tokens.Count - 1].type == TokenType.OpenBrace)
                {
                    tokens.Add(FindMethodBody());
                }
                else if (file._IndexOf("class", index, "class".Length) == index || file._IndexOf("struct", index, "struct".Length) == index)
                {
                    var match = new Regex("\\G((class)|(struct))[^\\{]*\\{")
                        .Match(file, index);
                    var proto = match.Value.Pop(1);
                    proto = proto.Trim(' ', '\n', '\r');
                    tokens.Add(new Token() { type = TokenType.ClassStructProto, value = proto });

                    index += proto.Length;
                }
                else if (file._IndexOf("#include", index, "#include".Length) == index)
                {
                    var match = new Regex("\\G#include ( )*(\"|<)[_a-zA-Z0-9\\.]+( )*(\"|>)").Match(file, index);
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
                    ConsumeWhiteSpace();

                    var match = new Regex($"\\G[^{{]*").Match(file, index);
                    tokens.Add(new Token() { type = TokenType.InitializerList, value = match.Value.Trim(' ', '\n', '\r') });

                    index += match.Value.Length;
                }
                else if (file._IndexOf("namespace", index, "namespace".Length) == index)
                {
                    index += "namespace".Length;
                    ConsumeWhiteSpace();

                    var match = new Regex($"\\G[_a-zA-Z0-9]+").Match(file, index);
                    tokens.Add(new Token() { type = TokenType.NamespaceKeyword, value = match.Value });

                    index += match.Value.Length;
                }
                // at the end check for method proto or field
                else if ((memberMatch = new Regex("([^\\);]*\\)[ \\n\\r]*:)|([^\\{;]+(\\{|;))").Match(file, index)).Value.Contains(";"))
                {
                    tokens.Add(new Token() { type = TokenType.ClassStructField, value = memberMatch.Value });

                    index += memberMatch.Value.Length;
                }
                else if (memberMatch.Value.Contains("{") || memberMatch.Value.Contains(":"))
                {
                    tokens.Add(new Token() { type = TokenType.MethodProto, value = memberMatch.Value.Pop(1).Trim(' ', '\n', '\r') });

                    index += memberMatch.Value.Length - 1;
                }
                else
                    throw new Exception("Unknown string " + index);
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace HeaderGenerator
{
    public partial class Parser
    {
        List<Token> tokens;
        List<IElement> globals;
        List<string> classes;
        List<Include> includes;
        int next;

        public Parser(List<Token> t)
        {
            globals = new List<IElement>();
            classes = new List<string>();
            tokens = t;
            includes = new List<Include>();
            next = 0;
            Parse();
        }

        private Token Next()
        {
            return tokens[next];
        }

        private void Match(TokenType token)
        {
            if (Next().type != token)
                throw new Exception("Tokens do not match");

            next++;
        }

        private void Parse()
        {
            _File();
        }

        private void _File()
        {
            globals.AddRange(Elements());
        }

        private List<IElement> Elements()
        {
            List<IElement> elements = new List<IElement>();

            while (Next().type != TokenType.EOT && Next().type != TokenType.CloseBrace)
            {
                var e = Element();

                if(e != null) // include will return null because its added to global include list
                    elements.Add(e);
            }

            return elements;
        }

        private Include Include()
        {
            Token t = Next();
            Match(TokenType.Include);
            Include inc = new Include();
            var match = Regex.Match(t.value, "<.*>");
            inc.header = match.Value.Substring(1);
            inc.header = inc.header.Remove(inc.header.Length - 1, 1);

            return inc;
        }

        private IElement Element()
        {
            if (Next().type == TokenType.ClassProto)
                return Class();
            else if (Next().type == TokenType.Include)
            {
                if (Next().value.Contains("<")) // for now parse only <> includes
                    includes.Insert(0, Include());
                else // still have to match "" includes
                    Match(TokenType.Include);

                return null;
            }
            else if (Next().type == TokenType.NamespaceKeyword)
            {
                var e = new Namespace() { name = Next().value };
                Match(TokenType.NamespaceKeyword);
                Match(TokenType.OpenBrace);
                e.elements = Elements();
                Match(TokenType.CloseBrace);

                return e;
            }
            else
                throw new Exception("Unknown global token");
        }

        private Class Class()
        {
            Token _proto = Next();
            Match(TokenType.ClassProto);
            var match = Regex.Match(_proto.value, "class( )+[_a-zA-Z0-9]+");
            string _name = match.Value.Remove(0,"class".Length).Trim(' ');
            classes.Add(_name);
            Class element = new Class() { proto = _proto.value, name = _name };

            ClassBody(element);

            return element;
        }

        private void ClassBody(Class element)
        {
            Match(TokenType.OpenBrace);

            while (Next().type != TokenType.CloseBraceSemicolon)
            {
                if (Next().type == TokenType.PrivateKeyword)
                {
                    Match(TokenType.PrivateKeyword);
                    element.privateMembers.AddRange(Members());
                }
                else if (Next().type == TokenType.PublicKeyword)
                {
                    Match(TokenType.PublicKeyword);
                    element.publicMembers.AddRange(Members());
                }
                else if (Next().type == TokenType.ProtectedKeyword)
                {
                    Match(TokenType.ProtectedKeyword);
                    element.protectedMembers.AddRange(Members());
                }
                else
                    throw new Exception("unknown token");
            }

            Match(TokenType.CloseBraceSemicolon);
        }

        private List<IElement> Members()
        {
            List<IElement> members = new List<IElement>();

            while (Next().type != TokenType.CloseBraceSemicolon 
                && Next().type != TokenType.PublicKeyword 
                && Next().type != TokenType.PrivateKeyword 
                && Next().type != TokenType.ProtectedKeyword)
                members.Add(Member());

            return members;
        }

        private IElement Member()
        {
            if (Next().type == TokenType.ClassField)
            {
                Field field = new Field() { proto = Next().value };
                Match(TokenType.ClassField);
                return field;
            }
            else if (Next().type == TokenType.MethodProto)
            {

                Method method = new Method() { proto = Next().value, initializerList = string.Empty };
                Match(TokenType.MethodProto);

                // optional initializer list
                if (Next().type == TokenType.InitializerList)
                {
                    method.initializerList = Next().value;
                    Match(TokenType.InitializerList);
                }

                Match(TokenType.OpenBrace);

                method.body = Next().value;
                Match(TokenType.MethodBody);

                Match(TokenType.CloseBrace);

                return method;
            }
            else
                throw new Exception("Unknown token");
        }

        public void Dump(List<char> h, List<char> cpp)
        {            

            foreach (var e in includes)
                e.Dump(h, cpp, "");

            foreach (var s in classes)
                h.AddRange("class " + s + ";\n");

            foreach(var e in globals)
                e.Dump(h, cpp, "");
                        
            
        }
    }
}

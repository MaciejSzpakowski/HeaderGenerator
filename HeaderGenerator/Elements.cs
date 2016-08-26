using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HeaderGenerator
{
    public interface IElement
    {
        void Dump(List<char> header, List<char> source, string _namespace);
    }

    public class Namespace : IElement
    {
        public string name;
        public List<IElement> elements;

        public Namespace()
        {
            elements = new List<IElement>();
        }

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
            h.AddRange($"\nnamespace {name}\n");
            h.AddRange("{\n");
            
            foreach (var e in elements)
                e.Dump(h, cpp, _namespace == "" ? name : _namespace + "::" + name);

            h.AddRange("}\n");
        }
    }

    public class Class : IElement
    {
        public string proto;
        public string name;
        public List<IElement> privateMembers;
        public List<IElement> publicMembers;
        public List<IElement> protectedMembers;

        public Class()
        {
            privateMembers = new List<IElement>();
            publicMembers = new List<IElement>();
            protectedMembers = new List<IElement>();
        }

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
            h.AddRange($"\n{proto}\n");
            h.AddRange("{\n");

            h.AddRange("private:\n");
            foreach (var f in privateMembers)
                f.Dump(h, cpp, _namespace == "" ? name : _namespace + "::" + name);

            h.AddRange("protected:\n");
            foreach (var f in protectedMembers)
                f.Dump(h, cpp, _namespace == "" ? name : _namespace + "::" + name);

            h.AddRange("public:\n");
            foreach (var f in publicMembers)
                f.Dump(h, cpp, _namespace == "" ? name : _namespace + "::" + name);

            h.AddRange("};\n");
        }
    }

    public class Include : IElement
    {
        public string header;

        public void Dump(List<char> h, List<char> cpp, string n)
        {
            h.AddRange("#include <" + header + ">\n");
        }
    }

    public class Struct : IElement
    {
        public string proto;
        public List<Method> methods = new List<Method>();
        public List<Field> fields = new List<Field>();

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
        }
    }

    public class Enum : IElement
    {
        public string proto;
        public List<string> fields = new List<string>();

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
        }
    }

    public class Method : IElement
    {
        public string proto;
        public string body;
        public string initializerList;
        public bool _virtual;
        public bool _override;
        public bool _pureVirtual;

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
            h.AddRange($"{proto};\n");

            // ctor
            if (Regex.IsMatch(proto, "^[_a-zA-Z0-9]+( )*\\("))
            {
                string namespaceProto = _namespace + "::" + proto;
                cpp.AddRange($"{namespaceProto}\n");
            }
            // other methods
            else
            {
                int spaceIndex = proto.IndexOf(' ');
                string namespaceProto = proto.Insert(spaceIndex + 1, _namespace + "::");
                cpp.AddRange($"{namespaceProto}\n");
            }

            if (initializerList != string.Empty)
                cpp.AddRange(":" + initializerList);

            cpp.AddRange("{\n");
            cpp.AddRange(body);
            cpp.AddRange("}\n\n");
        }
    }

    public class Field : IElement
    {
        public string proto;

        public void Dump(List<char> h, List<char> cpp, string _namespace)
        {
            h.AddRange($"{proto}\n");
        }
    }
}

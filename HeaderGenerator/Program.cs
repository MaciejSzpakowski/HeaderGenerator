using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeaderGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "C:/Users/Szpak/Documents/Visual Studio 2015/Projects/HeaderGenerator/Test";
            List<string> files = (new[] {
                "Source1.cpp",
                "Source2.cpp",
                "Source3.cpp",
                "Source4.cpp",
                "Source5.cpp"
            }).ToList();
            string h = Path.Combine(dir, "Header.h");
            string cpp = Path.Combine(dir, "Source.cpp");
            
            Generator generator = new Generator(dir, files);
            generator.Dump(h, cpp);
        }
    }
}

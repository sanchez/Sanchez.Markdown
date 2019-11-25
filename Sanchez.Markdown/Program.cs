using Sanchez.Markdown.Parser;
using System;
using System.IO;

namespace Sanchez.Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "test.md");
            var data = File.ReadAllText(fullFilePath);

            var res = MarkdownParser.ParseString(data);

            Console.WriteLine("Hello World!");
        }
    }
}

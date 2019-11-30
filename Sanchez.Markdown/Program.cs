using Sanchez.Markdown.Parser;
using Sanchez.Markdown.Renderers;
using Sanchez.Markdown.Renderers.Text;
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
            //var renderer = new TextRenderer();
            IRenderer<string> renderer = new TextRenderer();
            var result = renderer.Render(res);

            Console.WriteLine("Hello World!");
        }
    }
}

using Sanchez.Markdown.Parser;
using Sanchez.Markdown.Renderers;
using Sanchez.Markdown.Renderers.Text;
using System;
using System.IO;
using Microsoft.FSharp.Core;

namespace Sanchez.Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "test.md");
            var data = File.ReadAllText(fullFilePath);

            var metadata = MarkdownParser.ParseMetadata(data);
            var res = MarkdownParser.ParseString(data);
            //var renderer = new TextRenderer();
            IRenderer<string> renderer = new TextRenderer();

            var lookup = FuncConvert.ToFSharpFunc<string, FSharpFunc<string, string>>(action => FuncConvert.ToFSharpFunc<string, string>(args => "Hello"));
            var result = renderer.Render(res, lookup);

            Console.WriteLine("Hello World!");
        }
    }
}

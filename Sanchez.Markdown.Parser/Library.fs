namespace Sanchez.Markdown.Parser

open Sanchez.Markdown.Parser.Parsers.Parser

module MarkdownParser =

    let ParseString (document: string) =
        let lines = 
            document.Split [| '\n' |] 
            |> Array.toList
            |> List.map (fun x -> x.TrimEnd ())

        ParseLines lines

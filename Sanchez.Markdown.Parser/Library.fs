namespace Sanchez.Markdown.Parser

open Sanchez.Markdown.Parser.Parsers

module Parser =

    let ParseString (document: string) =
        let lines = 
            document.Split [| '\n' |] 
            |> Array.toList
            |> List.map (fun x -> x.TrimEnd ())

        BlockParser.ParseLines lines

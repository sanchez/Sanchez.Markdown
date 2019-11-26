namespace Sanchez.Markdown.Parser

open Sanchez.Markdown.Parser.Parsers.Parser
open Sanchez.Markdown.Symbols.Block

module MarkdownParser =

    let ParseString (document: string) =
        let lines = 
            document.Split [| '\n' |] 
            |> Array.toList
            |> List.map (fun x -> x.TrimEnd ())

        ParseLines lines
        |> DocumentSymbol
        |> Document

module Sanchez.Markdown.Parser

open Sanchez.Markdown.Parser.Parsers

let Parser (document: string) =
    let lines = 
        document.Split [| '\n' |] 
        |> Array.toList
        |> List.map (fun x -> x.TrimEnd ())

    let res = BlockParser.ParseLines lines
    "Hello World"

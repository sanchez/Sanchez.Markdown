module Sanchez.Markdown.Parser.Parsers.Inline.PlainText

open Sanchez.Markdown.Parser.Models

let CreatePlainText (chars: char list) =
    chars
    |> Array.ofList
    |> System.String.Concat
    |> Symbols.PlainTextSymbol
    |> Symbols.PlainText
    |> Some
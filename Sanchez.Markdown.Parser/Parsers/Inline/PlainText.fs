module Sanchez.Markdown.Parser.Parsers.Inline.PlainText

open Sanchez.Markdown.Symbols.Inline

let CreatePlainText (chars: char list) =
    chars
    |> Array.ofList
    |> System.String.Concat
    |> PlainTextSymbol
    |> PlainText
    |> Some
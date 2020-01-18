module Sanchez.Markdown.Parser.Parsers.Inline.CodeStatement

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Parsers.Inline.Utils
open Sanchez.Markdown.Symbols.Inline
open System

let Parse (characters: char list) (processor: InlineParserType) =
    if characters.Head = '`' then
        let (text, remainderChars) = SearchTillCharacter characters.Tail '`' []
        text
        |> String.Concat
        |> PlainTextSymbol
        |> CodeStatement
        |> (fun x -> (Some x, remainderChars))
    else (None, characters)
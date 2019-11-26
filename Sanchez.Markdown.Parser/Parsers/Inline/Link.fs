module Sanchez.Markdown.Parser.Parsers.Inline.Link

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Parsers.Inline.Utils
open Sanchez.Markdown.Symbols.Inline
open System

let Parse (characters: char list) (processor: InlineParserType) =
    if characters.Head = '[' then
        let (text, remainderChars) = SearchTillCharacter characters.Tail ']' []
        if remainderChars.Head = '(' then
            let (link, remainderRemainderChars) = SearchTillCharacter remainderChars.Tail ')' []
            let strLink = link |> String.Concat

            processor text []
            |> (fun x -> LinkSymbol (x, strLink))
            |> Link
            |> (fun x -> (Some x, remainderRemainderChars.Tail))
        else
            (None, characters)
    else
        (None, characters)
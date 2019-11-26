module Sanchez.Markdown.Parser.Parsers.Inline.Image

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Parsers.Inline.Utils
open Sanchez.Markdown.Symbols.Inline
open System

let Parse (characters: char list) (processor: InlineParserType) =
    if characters.Head = '!' && not characters.Tail.IsEmpty && characters.Tail.Head = '[' then
        let (text, remainderChars) = SearchTillCharacter characters.Tail.Tail ']' []
        if remainderChars.Head = '(' then
            let (link, remainderRemainderChars) = SearchTillCharacter remainderChars.Tail ')' []
            let strLink = link |> String.Concat

            (processor text [], strLink)
            |> LinkSymbol
            |> Image
            |> (fun x -> (Some x, remainderRemainderChars.Tail))
        else
            (None, characters)
    else
        (None, characters)

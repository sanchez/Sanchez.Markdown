module Sanchez.Markdown.Parser.Parsers.Inline.BoldItalics

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Symbols.Inline

let rec private ProcessBold (characters: char list) (startChar: char) (containingChars: char list) (processor: InlineParserType) =
    if characters.Length = 1 then
        (None, characters)
    else
        let currentChar = characters.Head
        let nextChar = characters.Tail.Head
        if nextChar = currentChar && currentChar = startChar then
            processor containingChars []
            |> SimpleInlineSymbol
            |> Bold
            |> (fun x -> (Some x, characters.Tail.Tail))
        else
            ProcessBold characters.Tail startChar (containingChars @ [characters.Head]) processor

let rec private ProcessItalics (characters: char list) (startChar: char) (containingChars: char list) (processor: InlineParserType) =
    if characters.Length = 0 then
        (None, characters)
    else
        let currentChar = characters.Head
        if currentChar = startChar then
            processor containingChars []
            |> SimpleInlineSymbol
            |> Italics
            |> (fun x -> (Some x, characters.Tail))
        else
            ProcessItalics characters.Tail startChar (containingChars @ [characters.Head]) processor

let Parse (characters: char list) (processor: InlineParserType) =
    let first = characters.Head
    if first = '*' || first = '_' then
        let second = characters.Tail.Head
        if second = first then
            ProcessBold characters.Tail.Tail first [] processor
        else
            ProcessItalics characters.Tail first [] processor
    else
        (None, characters)
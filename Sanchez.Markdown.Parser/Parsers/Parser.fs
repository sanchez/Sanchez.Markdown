module Sanchez.Markdown.Parser.Parsers.Parser

open Sanchez.Markdown.Parser.Models
open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Parsers.Block
open Sanchez.Markdown.Parser.Parsers.Inline
open Sanchez.Markdown.Parser.Parsers.Inline.PlainText

let private inlineParsers: List<char list -> InlineParserType -> Symbols.Inline option * char list> = [
    BoldItalics.Parse
]

let rec processInline (chars: char list) (emptyChars: char list) (processor: InlineParserType) =
    if chars.IsEmpty then [CreatePlainText emptyChars]
    else
        let parser =
            inlineParsers
            |> List.tryFind (fun x ->
                let (s, _) = x chars processor
                s.IsSome
                )

        match parser with
        | Some (x) ->
            let (s, remainderLines) = x chars processor
            if emptyChars.IsEmpty then
                s::processInline remainderLines [] processor
            else
                let plain = CreatePlainText emptyChars
                plain::s::processInline remainderLines [] processor
        | None ->
            processInline chars.Tail (emptyChars @ [chars.Head]) processor

let rec ParseInlines (chars: char list) (emptyChars: char list) =
    processInline chars emptyChars ParseInlines
    |> List.filter (fun x -> x.IsSome)
    |> List.map (fun x -> x.Value)

let private blockParsers: List<string list -> BlockParserType -> InlineParserType -> Symbols.Block option * string list> = [
    Heading.Parse
    UnOrderedList.Parse
    NewLine.Parse
    Blockquote.Parse

    Paragraph.Parse
]

let rec ParseLines (lines: string list) =
    if lines.IsEmpty then []
    else
        let result =
            blockParsers
            |> List.map (fun x -> x lines ParseLines ParseInlines)
            |> List.tryFind (fun x -> 
                let (s, _) = x
                s.IsSome)

        match result with
        | Some (x) ->
            let (s, remainderLines) = x
            s.Value::ParseLines remainderLines
        | None ->
            ParseLines lines.Tail
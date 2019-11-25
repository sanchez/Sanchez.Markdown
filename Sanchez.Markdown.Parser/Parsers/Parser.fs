module Sanchez.Markdown.Parser.Parsers.Parser

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Parsers.Block
open Sanchez.Markdown.Parser.Parsers.Inline
open Sanchez.Markdown.Parser.Parsers.Inline.PlainText
open Sanchez.Markdown.Symbols.Inline
open Sanchez.Markdown.Symbols.Block

let private inlineParsers: List<char list -> InlineParserType -> Inline option * char list> = [
    BoldItalics.Parse
]

let rec processInline (chars: char list) (emptyChars: char list) (processor: InlineParserType) =
    if chars.IsEmpty then [CreatePlainText emptyChars]
    else
        inlineParsers
        |> List.map (fun x -> x chars processor)
        |> List.tryFind (fst >> Option.isSome)
        |> Option.map (fun (s, remainderChars) -> 
            let plain = CreatePlainText emptyChars
            plain::s::processInline remainderChars [] processor)
        |> Option.defaultWith (fun () -> processInline chars.Tail (emptyChars @ [chars.Head]) processor)

let rec ParseInlines (chars: char list) (emptyChars: char list) =
    processInline chars emptyChars ParseInlines
    |> List.filter (fun x -> x.IsSome)
    |> List.map (fun x -> x.Value)

let private blockParsers: List<string list -> BlockParserType -> InlineParserType -> Block option * string list> = [
    Heading.Parse
    UnOrderedList.Parse
    NewLine.Parse
    Blockquote.Parse

    Paragraph.Parse
]

let rec ParseLines (lines: string list) =
    if lines.IsEmpty then []
    else
        blockParsers
        |> List.map (fun x -> x lines ParseLines ParseInlines)
        |> List.tryFind (fst >> Option.isSome)
        |> Option.map (fun (s, remainderLines) -> s.Value::ParseLines remainderLines)
        |> Option.defaultWith (fun () -> ParseLines lines.Tail)

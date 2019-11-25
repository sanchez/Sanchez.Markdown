module Sanchez.Markdown.Parser.Parsers.Block.Paragraph

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Symbols.Block

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    lines.Head
    |> Seq.toList
    |> inlineParser <| []
    |> SimpleSymbol
    |> Paragraph
    |> (fun x -> (Some x, lines.Tail))
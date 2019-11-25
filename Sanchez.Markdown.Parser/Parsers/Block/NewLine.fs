module Sanchez.Markdown.Parser.Parsers.Block.NewLine

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Symbols.Block

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let line = lines.Head
    if line.Length = 0 then
        ()
        |> BlankSymbol
        |> NewLine
        |> (fun x -> (Some x, lines.Tail))
    else
        (None, lines)

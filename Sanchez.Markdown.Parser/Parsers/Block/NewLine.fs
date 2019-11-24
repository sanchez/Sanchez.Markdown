module Sanchez.Markdown.Parser.Parsers.Block.NewLine

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Parser.Models

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let line = lines.Head
    if line.Length = 0 then
        let newLineSymbol = Symbols.NewLine (Symbols.BlankSymbol ())
        (Some newLineSymbol, lines.Tail)
    else
        (None, lines)

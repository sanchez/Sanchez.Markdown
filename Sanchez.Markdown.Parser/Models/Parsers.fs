namespace Sanchez.Markdown.Parser.Models.Parsers

open Sanchez.Markdown.Parser.Models

type InlineParserType = char list -> char list -> Symbols.Inline list
type BlockParserType = (string list) -> Symbols.Block list
namespace Sanchez.Markdown.Parser.Models.Parsers

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Symbols.Inline

type InlineParserType = char list -> char list -> Inline list
type BlockParserType = (string list) -> Block list
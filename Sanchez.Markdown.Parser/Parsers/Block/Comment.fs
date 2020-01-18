module Sanchez.Markdown.Parser.Parsers.Block.Comment

open Sanchez.Markdown.Parser.Models.Parsers
open System.Text.RegularExpressions
open Sanchez.Markdown.Symbols.Block

let private commentMatch = new Regex (@"^% ?(.*)$", RegexOptions.Compiled)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let matchResult = commentMatch.Match lines.Head
    if matchResult.Success then
        let comment = matchResult.Groups.[1].Value
        
        comment
        |> Comment
        |> (fun x -> (Some x, lines.Tail))
    else (None, lines)
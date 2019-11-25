module Sanchez.Markdown.Parser.Parsers.Block.Blockquote

open Sanchez.Markdown.Parser.Models.Parsers
open System.Text.RegularExpressions
open Sanchez.Markdown.Symbols.Block

let private blockquoteMatch = new Regex (@"^> (.*)$", RegexOptions.Compiled)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let matchResult = blockquoteMatch.Match lines.Head
    if matchResult.Success then
        matchResult.Groups.[1].Value
        |> Seq.toList
        |> inlineParser <| []
        |> SimpleSymbol
        |> BlockQuote
        |> (fun x -> (Some x, lines.Tail))
    else (None, lines)
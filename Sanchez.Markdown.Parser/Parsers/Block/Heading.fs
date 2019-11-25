module Sanchez.Markdown.Parser.Parsers.Block.Heading

open Sanchez.Markdown.Parser.Models.Parsers
open System.Text.RegularExpressions
open Sanchez.Markdown.Symbols.Block

let private headingMatch = new Regex (@"^(#{1,6})\ ?(.*)$", RegexOptions.Compiled)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let matchResult = headingMatch.Match lines.Head
    if matchResult.Success then
        let depth = matchResult.Groups.[1].Value.Length
        let title = matchResult.Groups.[2].Value

        title
        |> Seq.toList
        |> inlineParser <| []
        |> (fun x -> HeadingSymbol (depth, x))
        |> Heading
        |> (fun x -> (Some x, lines.Tail))
    else (None, lines)
module Sanchez.Markdown.Parser.Parsers.Block.CodeBlock

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Symbols.Block
open System.Text.RegularExpressions

let private codeBlockMatch = new Regex (@"^[`~]{3}$", RegexOptions.Compiled)

let rec FetchCodeLines (lines: string list) =
    if lines.IsEmpty then ([], [])
    else
        let matchResult = codeBlockMatch.Match lines.Head
        if matchResult.Success then ([], lines.Tail)
        else
            let (existingLines, remainder) = FetchCodeLines lines.Tail
            (lines.Head::existingLines, remainder)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let matchResult = codeBlockMatch.Match lines.Head
    if matchResult.Success then
        let (codeLines, remainder) = FetchCodeLines lines.Tail
        codeLines
        |> CodeBlockSymbol
        |> CodeBlock
        |> (fun x -> (Some x, remainder))
    else (None, lines)
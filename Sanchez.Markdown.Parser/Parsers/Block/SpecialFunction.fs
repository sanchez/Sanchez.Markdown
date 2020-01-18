module Sanchez.Markdown.Parser.Parsers.Block.SpecialFunction

open Sanchez.Markdown.Parser.Models.Parsers
open System.Text.RegularExpressions
open Sanchez.Markdown.Symbols.Block

let private specialMatch = new Regex (@"^!(\w+)\s(.*)$", RegexOptions.Compiled)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let matchResult = specialMatch.Match lines.Head
    if matchResult.Success then
        let func = matchResult.Groups.[1].Value
        let data = matchResult.Groups.[2].Value
        
        (func, data)
        |> SpecialFunction
        |> (fun x -> (Some x, lines.Tail))
    else (None, lines)
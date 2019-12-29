module Sanchez.Markdown.Parser.Parsers.Block.Metadata

open Sanchez.Markdown.Parser.Models.Parsers
open Sanchez.Markdown.Symbols.Block
open System.Text.RegularExpressions

let private metadataMatch = new Regex (@"^-{3}$", RegexOptions.Compiled)
let private metadataEntryMatch = new Regex (@"^(.*): ?(.*)$", RegexOptions.Compiled)

let rec FetchCodeLines (lines: string list) =
    if lines.IsEmpty then ([], [])
    else
        let matchResult = metadataMatch.Match lines.Head
        if matchResult.Success then ([], lines.Tail)
        else
            let (existingLines, remainder) = FetchCodeLines lines.Tail
            (lines.Head::existingLines, remainder)
            
let rec ParseToMap (lines: string list) =
    if lines.IsEmpty then Map.empty
    else
        let m = ParseToMap lines.Tail
        
        let entryResult = metadataEntryMatch.Match lines.Head
        if entryResult.Success then
            let key = entryResult.Groups.[1].Value
            let value = entryResult.Groups.[2].Value
            
            Map.add key value m
        else m
            
let Parse (lines: string list) =
    let matchResult = metadataMatch.Match lines.Head
    if matchResult.Success then
        let (metadataLines, remainder) = FetchCodeLines lines.Tail
        let metadata = ParseToMap metadataLines
        
        (metadata, remainder)
    else (Map.empty, lines)
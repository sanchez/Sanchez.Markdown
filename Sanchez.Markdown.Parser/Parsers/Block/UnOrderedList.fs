module Sanchez.Markdown.Parser.Parsers.Block.UnOrderedList

open Sanchez.Markdown.Parser.Models.Parsers
open System.Text.RegularExpressions
open Sanchez.Markdown.Symbols.Inline
open Sanchez.Markdown.Symbols.Block

let private unorderedMatch = new Regex (@"^(\s)*[-\.] (.*)$", RegexOptions.Compiled)

let rec private IterateUnordered (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let line = lines.Head
    let matchResult = unorderedMatch.Match line
    if matchResult.Success then
        let depth = matchResult.Groups.[1].Value.Length
        let title = matchResult.Groups.[2].Value
        let item =
            title
            |> Seq.toList
            |> inlineParser <| []
            |> (fun x -> (depth, x))
        let (items, remainderLines) = IterateUnordered lines.Tail blockParser inlineParser
        (item::items, remainderLines)
    else
        ([], lines)

let rec private SortItems (items: (int * Inline list) list) (currentDepth: int) =
    if items.Length = 0 then
        ([], items)
    else
        let (itemDepth, itemContent) = items.Head
        if (itemDepth > currentDepth) then
            let (children, remainderLines) = SortItems items itemDepth
            let group = 
                children
                |> GroupSymbol
                |> ListGroup

            let (siblings, remainderRemadinderLines) = SortItems remainderLines currentDepth
            (group::siblings, remainderRemadinderLines)
        elif itemDepth = currentDepth then
            let item = itemContent |> ListItem
            let (siblings, remainderLines) = SortItems items.Tail currentDepth
            (item::siblings, remainderLines)
        else
            ([], items)

let Parse (lines: string list) (blockParser: BlockParserType) (inlineParser: InlineParserType) =
    let (items, remainderLines) = IterateUnordered lines blockParser inlineParser
    if items.Length <> 0 then
        let (items, _) = SortItems items 0
        items
        |> GroupSymbol
        |> ListGroup
        |> UnorderedList
        |> Some
        |> (fun x -> (x, remainderLines))
    else
        (None, lines)

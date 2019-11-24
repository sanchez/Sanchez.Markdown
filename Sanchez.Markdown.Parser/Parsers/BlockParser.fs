namespace Sanchez.Markdown.Parser.Parsers

open Sanchez.Markdown.Parser.Models
open System.Text.RegularExpressions

module BlockParser =

    type ParseLinesType = (string list) -> Symbols.Block option list

    let private separateHeadOut (lines: string list) =
        (lines.Head, lines.Tail)

    let private ProcessNewLine (lines: string list) (processor: ParseLinesType) =
        let line = lines.Head
        if line.Length = 0 then
            let newLineSymbol = Symbols.NewLine (Symbols.BlankSymbol ())
            let (_, remainderLines) = separateHeadOut(lines)
            (Some newLineSymbol, remainderLines)
        else
            (None, lines)

    let headingMatch = new Regex (@"^(#{1,6})\ ?(.*)$", RegexOptions.Compiled)
    let private ProcessHeading (lines: string list) (processor: ParseLinesType) =
        let line = lines.Head
        let matchResult = headingMatch.Match line
        if matchResult.Success then
            let depth = matchResult.Groups.[1].Value.Length
            let title = matchResult.Groups.[2].Value
            let (_, remainderLines) = separateHeadOut lines

            title
            |> Seq.toList
            |> InlineParser.ParseInlines <| []
            |> (fun x -> Symbols.HeadingSymbol (depth, x))
            |> Symbols.Heading
            |> (fun x -> (Some x, remainderLines))
        else
            (None, lines)

    let unorderedMatch = new Regex (@"^(\s)*[-\.] (.*)$", RegexOptions.Compiled)
    let rec private IterateUnordered (lines: string list) =
        let line = lines.Head
        let matchResult = unorderedMatch.Match line
        if matchResult.Success then
            let depth = matchResult.Groups.[1].Value.Length
            let title = matchResult.Groups.[2].Value
            let item =
                title
                |> Seq.toList
                |> InlineParser.ParseInlines <| []
                |> (fun x -> (depth, x))
            let (items, remainderLines) = IterateUnordered lines.Tail
            (item::items, remainderLines)
        else
            ([], lines)

    let rec private SortItems (items: (int * Symbols.Inline list) list) (currentDepth: int) =
        if items.Length = 0 then
            ([], items)
        else
            let (itemDepth, itemContent) = items.Head
            if (itemDepth > currentDepth) then
                let (children, remainderLines) = SortItems items itemDepth
                let group = 
                    children
                    |> Symbols.GroupSymbol
                    |> Symbols.ListGroup

                let (siblings, remainderRemadinderLines) = SortItems remainderLines currentDepth
                (group::siblings, remainderRemadinderLines)
            elif itemDepth = currentDepth then
                let item = itemContent |> Symbols.ListItem
                let (siblings, remainderLines) = SortItems items.Tail currentDepth
                (item::siblings, remainderLines)
            else
                ([], items)


    let private ProcessUnOrdered (lines: string list) (processor: ParseLinesType) =
        let (items, remainderLines) = IterateUnordered lines
        if items.Length <> 0 then
            let (items, _) = SortItems items 0
            items
            |> Symbols.GroupSymbol
            |> Symbols.ListGroup
            |> Symbols.OrderedList
            |> Some
            |> (fun x -> (x, remainderLines))
        else
            (None, lines)


    let private ProcessParagraph (lines: List<string>) (processor: ParseLinesType) =
        let (content, remainderLines) = separateHeadOut(lines)
        content 
        |> Seq.toList
        |> InlineParser.ParseInlines <| []
        |> Symbols.SimpleSymbol
        |> Symbols.Paragraph
        |> (fun x -> (Some x, remainderLines))


    let private parsers: List<string list -> ParseLinesType -> Symbols.Block option * string list> = [
        ProcessHeading
        ProcessNewLine
        ProcessUnOrdered

        ProcessParagraph
    ]

    let rec ParseLines (lines: string list) =
        if lines.IsEmpty then []
        else
            let parser = 
                parsers
                |> List.tryFind (fun x -> 
                    let (s, remainderLines) = x lines ParseLines
                    s.IsSome
                    )

            match parser with
            | Some (x) ->
                let (s, remainderLines) = x lines ParseLines
                s::ParseLines(remainderLines)
            | None ->
                let (_, remainderLines) = separateHeadOut lines
                ParseLines(remainderLines)
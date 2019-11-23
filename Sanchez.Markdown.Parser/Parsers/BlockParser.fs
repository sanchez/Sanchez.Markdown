namespace Sanchez.Markdown.Parser.Parsers

open Sanchez.Markdown.Parser.Models
open System.Text.RegularExpressions

module BlockParser =

    let private separateHeadOut (lines: List<string>) =
        (lines.Head, lines.Tail)

    let private ProcessNewLine (lines: List<string>) =
        let line = lines.Head
        if line.Length = 0 then
            let newLineSymbol = Symbols.NewLine (Symbols.BlankSymbol ())
            let (_, remainderLines) = separateHeadOut(lines)
            (Some newLineSymbol, remainderLines)
        else
            (None, lines)

    let headingMatch = new Regex (@"^(#{1,6})\ ?(.*)$", RegexOptions.Compiled)
    let private ProcessHeading (lines: string list) =
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


    let private ProcessParagraph (lines: List<string>) =
        let (content, remainderLines) = separateHeadOut(lines)
        content 
        |> Seq.toList
        |> InlineParser.ParseInlines <| []
        |> Symbols.SimpleSymbol
        |> Symbols.Paragraph
        |> (fun x -> (Some x, remainderLines))


    let private parsers: List<List<string> -> Symbols.Block option * string list> = [
        ProcessHeading
        ProcessNewLine

        ProcessParagraph
    ]

    let rec ParseLines (lines: List<string>) =
        if lines.IsEmpty then []
        else
            let parser = 
                parsers
                |> List.tryFind (fun x -> 
                    let (s, remainderLines) = x lines
                    s.IsSome
                    )

            match parser with
            | Some (x) ->
                let (s, remainderLines) = x lines
                s::ParseLines(remainderLines)
            | None ->
                let (_, remainderLines) = separateHeadOut lines
                ParseLines(remainderLines)
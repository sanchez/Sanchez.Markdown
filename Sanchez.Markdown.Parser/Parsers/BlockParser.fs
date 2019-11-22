namespace Sanchez.Markdown.Parser.Parsers

open Sanchez.Markdown.Parser.Models

module BlockParser =

    let private separateHeadOut (lines: List<string>) =
        let head = lines.Head
        let remainderLines = lines.GetSlice (Some 1,  Some (lines.Length - 1))
        (head, remainderLines)

    let private ProcessNewLine (lines: List<string>) =
        let line = lines.Head
        if line.Length = 0 then
            let newLineSymbol = Symbols.NewLine (Symbols.BlankSymbol ())
            let (_, remainderLines) = separateHeadOut(lines)
            (Some newLineSymbol, remainderLines)
        else
            (None, lines)


    let private parsers: List<List<string> -> Symbols.Block option * string list> = [
        ProcessNewLine
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
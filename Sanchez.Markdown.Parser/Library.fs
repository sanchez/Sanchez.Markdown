module Sanchez.Markdown.Parser

let SeparateHeadOut (lines: List<string>) =
    let head = lines.Head
    let remainderLines = lines.GetSlice (Some 1,  Some (lines.Length - 2))
    (head, remainderLines)


let ProcessNewLine (lines: List<string>) =
    let line = lines.Head
    if line.Length = 0 then
        let newLineSymbol = Symbols.NewLine (Symbols.BlankSymbol ())
        let (_, remainderLines) = SeparateHeadOut(lines)
        (Some newLineSymbol, remainderLines)
    else
        (None, lines)


let parsers: List<List<string> -> Symbols.Block option * string list> = [
    ProcessNewLine
]

let rec onLine (lines: List<string>) =
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
            s::onLine(remainderLines)
        | None ->
            let (_, remainderLines) = SeparateHeadOut lines
            onLine(remainderLines)


let Parser (document: string) =
    let lines = 
        document.Split [| '\n' |] 
        |> Array.toList
        |> List.map (fun x -> x.TrimEnd ())

    let res = onLine lines
    "Hello World"

module InlineParser

open Sanchez.Markdown.Parser.Models

module InlineParser =

    let private separateHeadOut (characters: List<char>) =
        let head = characters.Head
        let remainderLines = characters.GetSlice (Some 1, Some (characters.Length - 1))
        (head, remainderLines)

    let private CreatePlainText (chars: List<char>) =
        chars 
        |> Array.ofList
        |> System.String.Concat
        |> Symbols.PlainTextSymbol
        |> Symbols.PlainText
        |> Some

    let private parsers: List<List<char> -> Symbols.Inline option * char list> = []

    let rec ParseInlines (chars: List<char>, emptyChars: List<char>) =
        if chars.IsEmpty then []
        else
            let parser =
                parsers
                |> List.tryFind (fun x ->
                    let (s, _) = x chars
                    s.IsSome
                    )

            match parser with
            | Some (x) ->
                let (s, remainderLines) = x chars
                if emptyChars.IsEmpty then
                    s::ParseInlines(remainderLines, [])
                else
                    let plain = CreatePlainText emptyChars
                    plain::s::ParseInlines(remainderLines, [])
            | None ->
                let (c, remainderLines) = separateHeadOut chars
                ParseInlines(remainderLines, emptyChars @ [c])

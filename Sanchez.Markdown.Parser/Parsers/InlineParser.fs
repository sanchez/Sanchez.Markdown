namespace Sanchez.Markdown.Parser.Parsers

open Sanchez.Markdown.Parser.Models

module InlineParser =

    let private separateHeadOut (characters: List<char>) =
        (characters.Head, characters.Tail)

    let private CreatePlainText (chars: List<char>) =
        chars 
        |> Array.ofList
        |> System.String.Concat
        |> Symbols.PlainTextSymbol
        |> Symbols.PlainText
        |> Some

    let private parsers: List<List<char> -> Symbols.Inline option * char list> = []

    let rec processInline (chars: List<char>) (emptyChars: List<char>) =
        if chars.IsEmpty then [CreatePlainText emptyChars]
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
                    s::processInline remainderLines []
                else
                    let plain = CreatePlainText emptyChars
                    plain::s::processInline remainderLines []
            | None ->
                let (c, remainderLines) = separateHeadOut chars
                processInline remainderLines (emptyChars @ [c])

    let ParseInlines (chars: char list) (emptyChars: char list) =
        processInline chars emptyChars
        |> List.filter (fun x -> x.IsSome)
        |> List.map (fun x -> x.Value)

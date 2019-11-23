namespace Sanchez.Markdown.Parser.Parsers

open Sanchez.Markdown.Parser.Models

module InlineParser =

    type InlineParserType = char list -> char list -> Symbols.Inline option list

    let private separateHeadOut (characters: List<char>) =
        (characters.Head, characters.Tail)

    let private CreatePlainText (chars: List<char>) =
        chars 
        |> Array.ofList
        |> System.String.Concat
        |> Symbols.PlainTextSymbol
        |> Symbols.PlainText
        |> Some

    let rec private ProcessBold (characters: char list) (startChar: char) (containingChars: char list) (processor: InlineParserType) =
        if characters.Length = 1 then
            (None, characters)
        else
            let currentChar = characters.Head
            let nextChar = characters.Tail.Head
            if nextChar = currentChar && currentChar = startChar then
                processor containingChars []
                |> List.filter (fun x -> x.IsSome)
                |> List.map (fun x -> x.Value)
                |> Symbols.SimpleInlineSymbol
                |> Symbols.Bold
                |> (fun x -> (Some x, characters.Tail.Tail))
            else
                ProcessBold characters.Tail startChar (containingChars @ [characters.Head]) processor

    let rec private ProcessItalics (characters: char list) (startChar: char) (containingChars: char list) (processor: InlineParserType) =
        if characters.Length = 0 then
            (None, characters)
        else 
            let currentChar = characters.Head
            if currentChar = startChar then
                processor containingChars []
                |> List.filter (fun x -> x.IsSome)
                |> List.map (fun x -> x.Value)
                |> Symbols.SimpleInlineSymbol
                |> Symbols.Italics
                |> (fun x -> (Some x, characters.Tail))
            else
                ProcessItalics characters.Tail startChar (containingChars @ [characters.Head]) processor

    let private ProcessBoldItalics (characters: char list) (processor: InlineParserType) =
        let first = characters.Head
        if first = '*' || first = '_' then
            let second = characters.Tail.Head
            if second = first then
                ProcessBold characters.Tail.Tail first [] processor
            else
                ProcessItalics characters.Tail first [] processor
        else
            (None, characters)

    let private parsers: List<char list -> InlineParserType -> Symbols.Inline option * char list> = [
        ProcessBoldItalics
    ]

    let rec processInline (chars: List<char>) (emptyChars: List<char>) =
        if chars.IsEmpty then [CreatePlainText emptyChars]
        else
            let parser =
                parsers
                |> List.tryFind (fun x ->
                    let (s, _) = x chars processInline
                    s.IsSome
                    )

            match parser with
            | Some (x) ->
                let (s, remainderLines) = x chars processInline
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

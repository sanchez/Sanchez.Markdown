module Symbols

type Inline =
    | Bold of SimpleInlineSymbol
    | Italics of SimpleInlineSymbol
    | PlainText of PlainTextSymbol

and SimpleInlineSymbol (content: List<Inline>) =
    member this.Content = content

and PlainTextSymbol (content: string) =
    member this.Content = content



type Block =
    | Document of SimpleSymbol
    | Heading of HeadingSymbol
    | NewLine of BlankSymbol
    | Paragraph of SimpleSymbol

and SimpleSymbol (content: List<Block>) =
    member this.Content = content

and HeadingSymbol (depth: int, title: List<Inline>) =
    member this.Depth = depth
    member this.Title = title

and NewLineSymbol () = class end

and BlankSymbol () = class end


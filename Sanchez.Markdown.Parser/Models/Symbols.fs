namespace Sanchez.Markdown.Parser.Models

module Symbols =

    type Inline =
        | Bold of SimpleInlineSymbol
        | Italics of SimpleInlineSymbol
        | PlainText of PlainTextSymbol

    and SimpleInlineSymbol (content: Inline list) =
        member this.Content = content

    and PlainTextSymbol (content: string) =
        member this.Content = content



    type Block =
        | Document of DocumentSymbol
        | Heading of HeadingSymbol
        | NewLine of BlankSymbol
        | Paragraph of SimpleSymbol

    and DocumentSymbol (content: Block list) =
        member this.Content = content

    and SimpleSymbol (content: Inline list) =
        member this.Content = content

    and HeadingSymbol (depth: int, title: Inline list) =
        member this.Depth = depth
        member this.Title = title

    and BlankSymbol () = class end

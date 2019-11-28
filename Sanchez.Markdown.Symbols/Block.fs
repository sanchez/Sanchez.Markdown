namespace Sanchez.Markdown.Symbols.Block

open Sanchez.Markdown.Symbols.Inline

type MarkupList =
    | ListItem of Inline list
    | ListGroup of GroupSymbol

and GroupSymbol (content: MarkupList list) =
    member this.Content = content

type Block =
    | Document of DocumentSymbol
    | Heading of HeadingSymbol
    | NewLine of BlankSymbol
    | Paragraph of SimpleSymbol
    | UnorderedList of MarkupList
    | BlockQuote of SimpleSymbol

and DocumentSymbol (content: Block list) =
    member this.Content = content

and SimpleSymbol (content: Inline list) =
    member this.Content = content

and HeadingSymbol (depth: int, title: Inline list) =
    member this.Depth = depth
    member this.Title = title

and BlankSymbol () = class end
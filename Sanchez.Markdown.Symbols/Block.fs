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
    | CodeBlock of CodeBlockSymbol
    | Comment of string
    | SpecialFunction of string * string

and DocumentSymbol (metadata: Map<string, string>, content: Block list) =
    member this.Metadata = metadata
    member this.Content = content
    
    member this.GetMetaField key =
        metadata |> Map.tryFind key
    
    member this.Title = this.GetMetaField "title" |> Option.defaultValue "Invalid Title"
    member this.Author = this.GetMetaField "author" |> Option.defaultValue "Invalid Author"
    member this.Date = this.GetMetaField "date" |> Option.defaultValue ""

and SimpleSymbol (content: Inline list) =
    member this.Content = content

and HeadingSymbol (depth: int, title: Inline list) =
    member this.Depth = depth
    member this.Title = title
    
and CodeBlockSymbol (content: string list) =
    member this.Content = content

and BlankSymbol () = class end
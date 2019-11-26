namespace Sanchez.Markdown.Symbols.Inline

type Inline =
    | Bold of SimpleInlineSymbol
    | Italics of SimpleInlineSymbol
    | PlainText of PlainTextSymbol
    | Link of LinkSymbol
    | Image of LinkSymbol

and SimpleInlineSymbol (content: Inline list) =
    member this.Content = content

and PlainTextSymbol (content: string) =
    member this.Content = content

and LinkSymbol (content: Inline list, link: string) =
    member this.Content = content
    member this.Link = link

type MarkupList =
    | ListItem of Inline list
    | ListGroup of GroupSymbol

and GroupSymbol (content: MarkupList list) =
    member this.Content = content
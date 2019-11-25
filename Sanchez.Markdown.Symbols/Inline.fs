namespace Sanchez.Markdown.Symbols.Inline

type Inline =
    | Bold of SimpleInlineSymbol
    | Italics of SimpleInlineSymbol
    | PlainText of PlainTextSymbol

and SimpleInlineSymbol (content: Inline list) =
    member this.Content = content

and PlainTextSymbol (content: string) =
    member this.Content = content

type MarkupList =
    | ListItem of Inline list
    | ListGroup of GroupSymbol

and GroupSymbol (content: MarkupList list) =
    member this.Content = content
namespace Sanchez.Markdown.Renderers

open Sanchez.Markdown.Symbols.Block

type IRenderer<'T> =
    abstract member Render: symbol: DocumentSymbol -> 'T

[<AbstractClass>]
type BaseRenderer<'T, 'U>() =
    abstract member CombineNodes: 'U list -> 'T

    abstract member RenderGroup: 'U list -> 'U
    abstract member RenderHeading: HeadingSymbol -> 'U
    abstract member RenderNewLine: BlankSymbol -> 'U
    abstract member RenderParagraph: SimpleSymbol -> 'U
    abstract member RenderUnorderedList: MarkupList -> 'U
    abstract member RenderBlockQuote: SimpleSymbol -> 'U

    member this.RenderBlock (symbol: Block) =
        match symbol with
        | Document d -> 
            d.Content 
            |> List.map this.RenderBlock
            |> this.RenderGroup
        | Heading s -> this.RenderHeading s
        | NewLine s -> this.RenderNewLine s
        | Paragraph s -> this.RenderParagraph s
        | UnorderedList s -> this.RenderUnorderedList s
        | BlockQuote s -> this.RenderBlockQuote s

    interface IRenderer<'T> with
        member this.Render symbol =
            symbol.Content
            |> List.map this.RenderBlock
            |> this.CombineNodes

namespace Sanchez.Markdown.Renderers.Text

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Symbols.Inline
open Sanchez.Markdown.Renderers

type TextRenderer() =
    let renderInlineGroup (inlineRenderer: InlineRenderer<string>) (nodes: string list) =
        nodes |> List.reduce (fun acc x -> acc + " " + x)

    let renderBold (inlineRenderer: InlineRenderer<string>) (b: SimpleInlineSymbol) =
        b.Content
        |> inlineRenderer
        |> (fun x -> "Bold(" + x + ")")

    let renderItalics (inlineRenderer: InlineRenderer<string>) (b: SimpleInlineSymbol) =
        b.Content
        |> inlineRenderer
        |> (fun x -> "Italics(" + x + ")")

    let renderPlainText (inlineRenderer: InlineRenderer<string>) (b: PlainTextSymbol) =
        b.Content

    let renderLink (inlineRenderer: InlineRenderer<string>) (b: LinkSymbol) =
        b.Content
        |> inlineRenderer
        |> (fun x -> "Link[" + b.Link + "] (" + x + ")")

    let renderImage (inlineRenderer: InlineRenderer<string>) (b: LinkSymbol) =
        b.Content
        |> inlineRenderer
        |> (fun x -> "Image[" + b.Link + "] (" + x + ")")

    let rec renderInline (symbol: Inline list) : string =
        symbol
        |> List.map (fun x -> 
            match x with
            | Bold s -> renderBold renderInline s
            | Italics s -> renderItalics renderInline s
            | PlainText s -> renderPlainText renderInline s
            | Link s -> renderLink renderInline s
            | Image s -> renderImage renderInline s
        )
        |> renderInlineGroup renderInline

    let renderGroup (blockRenderer: BlockRenderer<string>) nodes =
        nodes |> List.reduce (fun acc x -> acc + "\n" + x)

    let renderHeading (blockRenderer: BlockRenderer<string>) (symbol: HeadingSymbol) =
        "Heading " + symbol.Depth.ToString() + " (" + (renderInline symbol.Title) + ")"

    let renderNewLine (blockRenderer: BlockRenderer<string>) symbol =
        "NewLine"

    let renderParagraph (blockRenderer: BlockRenderer<string>) (symbol: SimpleSymbol) =
        "Paragraph(" + (renderInline symbol.Content) + ")"

    let renderUnorderedList (blockRenderer: BlockRenderer<string>) (symbol: MarkupList) =
        "Unordered List"

    let renderBlockQuote (blockRenderer: BlockRenderer<string>) (symbol: SimpleSymbol) =
        "BlockQuote(" + (renderInline symbol.Content) + ")"

    let rec renderBlock symbol : string =
        match symbol with
        | Document d ->
            d.Content
            |> List.map renderBlock
            |> renderGroup renderBlock
        | Heading s -> renderHeading renderBlock s
        | NewLine s -> renderNewLine renderBlock s
        | Paragraph s -> renderParagraph renderBlock s
        | UnorderedList s -> renderUnorderedList renderBlock s
        | BlockQuote s -> renderBlockQuote renderBlock s

    interface IRenderer<string> with
        member this.Render symbol =
            symbol.Content
            |> List.map (fun x -> renderBlock x)
            |> List.reduce (fun acc x -> acc + "\n" + x)

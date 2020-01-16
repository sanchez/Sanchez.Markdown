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
        
    let renderCodeStatement (inlineRenderer: InlineRenderer<string>) (b: PlainTextSymbol) =
        "Code(" + b.Content + ")"

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
            | CodeStatement s -> renderCodeStatement renderInline s
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
        
    let renderCodeBlock (blockRenderer: BlockRenderer<string>) (symbol: CodeBlockSymbol) =
        "Code Block(" + (List.reduce (+) symbol.Content) + ")"
        
    let renderComment (blockRenderer: BlockRenderer<string>) (symbol: string) =
        ""
        
    let renderSpecialFunction (blockRenderer: BlockRenderer<string>) (symbol: string * string) (specialLookup: string -> string -> string) =
        specialLookup (fst symbol) (snd symbol)

    let rec renderBlock (specialLookup: string -> string -> string) symbol =
        match symbol with
        | Document d ->
            d.Content
            |> List.map (renderBlock specialLookup)
            |> renderGroup (renderBlock specialLookup)
        | Heading s -> renderHeading (renderBlock specialLookup) s
        | NewLine s -> renderNewLine (renderBlock specialLookup) s
        | Paragraph s -> renderParagraph (renderBlock specialLookup) s
        | UnorderedList s -> renderUnorderedList (renderBlock specialLookup) s
        | BlockQuote s -> renderBlockQuote (renderBlock specialLookup) s
        | CodeBlock s -> renderCodeBlock (renderBlock specialLookup) s
        | Comment s -> renderComment (renderBlock specialLookup) s
        | SpecialFunction (action, args) -> renderSpecialFunction (renderBlock specialLookup) (action, args) specialLookup

    interface IRenderer<string> with
        member this.Render symbol specialLookup =
            symbol.Content
            |> List.map (fun x -> renderBlock specialLookup x)
            |> List.reduce (fun acc x -> acc + "\n" + x)

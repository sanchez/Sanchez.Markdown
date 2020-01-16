namespace Sanchez.Markdown.Renderers.Blazor

open System
open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Symbols.Inline
open Sanchez.Markdown.Renderers
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.Rendering

type BlazorRenderer() =
    let renderInlineGroup (inlineRenderer: InlineRenderer<RenderFragment>) (nodes: RenderFragment list) =
        new RenderFragment(fun builder ->
            nodes
            |> List.iteri (fun i (node: RenderFragment) -> builder.AddContent(i, node)))
        
    let renderBold (inlineRenderer: InlineRenderer<RenderFragment>) (b: SimpleInlineSymbol) =
        new RenderFragment(fun builder ->
            b.Content
            |> inlineRenderer
            |> (fun content ->
                builder.OpenElement(0, "b")
                builder.AddContent(1, content)
                builder.CloseElement()))
        
    let renderItalics (inlineRenderer: InlineRenderer<RenderFragment>) (b: SimpleInlineSymbol) =
        new RenderFragment(fun builder ->
            b.Content
            |> inlineRenderer
            |> (fun content ->
                builder.OpenElement(0, "i")
                builder.AddContent(1, content)
                builder.CloseElement()))
        
    let renderPlainText (inlineRenderer: InlineRenderer<RenderFragment>) (b: PlainTextSymbol) =
        new RenderFragment(fun builder ->
            builder.OpenElement(0, "text")
            builder.AddContent(1, b.Content)
            builder.CloseElement())
        
    let renderLink (inlineRenderer: InlineRenderer<RenderFragment>) (b: LinkSymbol) =
        new RenderFragment(fun builder ->
            b.Content
            |> inlineRenderer
            |> (fun content ->
                builder.OpenElement(0, "i")
                builder.AddAttribute(1, "href", b.Link)
                builder.AddContent(2, content)
                builder.CloseElement()))
        
    let renderImage (inlineRenderer: InlineRenderer<RenderFragment>) (b: LinkSymbol) =
        new RenderFragment(fun builder ->
            b.Content
            |> inlineRenderer
            |> (fun content ->
                builder.OpenElement(0, "img")
                builder.AddAttribute(1, "href", b.Link)
                builder.AddContent(2, content)
                builder.CloseElement()))
        
    let renderCodeStatement (inlineRenderer: InlineRenderer<RenderFragment>) (b: PlainTextSymbol) =
        new RenderFragment(fun builder ->
            builder.OpenElement(0, "code")
            builder.AddContent(1, b.Content)
            builder.CloseElement())
        
    let rec renderInline (symbol: Inline list) =
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
        
    let renderGroup (blockRenderer: BlockRenderer<RenderFragment>) nodes =
        new RenderFragment(fun builder ->
            nodes
            |> List.iteri (fun i (node: RenderFragment) -> builder.AddContent(i, node)))
        
    let renderHeading (blockRenderer: BlockRenderer<RenderFragment>) (symbol: HeadingSymbol) =
        new RenderFragment(fun builder ->
            symbol.Title
            |> renderInline
            |> (fun content ->
                builder.OpenElement(0, (sprintf "h%d" symbol.Depth))
                builder.AddContent(1, content)
                builder.CloseElement()))
        
    let renderNewLine (blockRenderer: BlockRenderer<RenderFragment>) symbol =
        new RenderFragment(fun builder ->
            builder.AddMarkupContent(0, "<br />"))
        
    let renderParagraph (blockRenderer: BlockRenderer<RenderFragment>) (symbol: SimpleSymbol) =
        new RenderFragment(fun builder ->
            builder.OpenElement(0, "p")
            builder.AddContent(1, renderInline symbol.Content)
            builder.CloseElement())
        
    let renderUnorderedList (blockRenderer: BlockRenderer<RenderFragment>) (symbol: MarkupList) =
        null
        
    let renderBlockQuote (blockRenderer: BlockRenderer<RenderFragment>) (symbol: SimpleSymbol) =
        new RenderFragment(fun builder ->
            builder.OpenElement(0, "blockquote")
            builder.AddContent(1, renderInline symbol.Content)
            builder.CloseElement())
        
    let renderCodeBlock (blockRenderer: BlockRenderer<RenderFragment>) (symbol: CodeBlockSymbol) =
        new RenderFragment(fun builder ->
            builder.OpenElement(0, "code")
            symbol.Content
            |> List.map (fun x -> x + "\n")
            |> List.reduce (+)
            |> (fun x -> builder.AddContent(1, x))
            builder.CloseElement())
        
    let renderComment (blockRenderer: BlockRenderer<RenderFragment>) (symbol: string) =
        new RenderFragment(fun builder ->
            0 |> ignore)
        
    let renderSpecialFunction (blockRenderer: BlockRenderer<RenderFragment>) (symbol: string * string) (specialLookup: string -> string -> RenderFragment) =
        let special = specialLookup (fst symbol) (snd symbol)
        if special = null then
            new RenderFragment(fun builder ->
                builder.OpenElement(0, "div")
                fst symbol
                |> sprintf "Missing Symbol Render for: %s"
                |> (fun x -> builder.AddContent(1, x))
                builder.CloseElement())
        else special
        
    let rec renderBlock specialLookup symbol =
        let nextRender = renderBlock specialLookup
        
        match symbol with
        | Document d ->
            d.Content
            |> List.map nextRender
            |> renderGroup nextRender
        | Heading s -> renderHeading nextRender s
        | NewLine s -> renderNewLine nextRender s
        | Paragraph s -> renderParagraph nextRender s
        | UnorderedList s -> renderUnorderedList nextRender s
        | BlockQuote s -> renderBlockQuote nextRender s
        | CodeBlock s -> renderCodeBlock nextRender s
        | Comment s -> renderComment nextRender s
        | SpecialFunction (action, args) -> renderSpecialFunction nextRender (action, args) specialLookup
    
    interface IRenderer<RenderFragment> with
        member this.Render symbol specialLookup =
            let nextRender = renderBlock specialLookup
            
            symbol.Content
            |> List.map nextRender
            |> renderGroup nextRender
module IServices

type IParser =
    abstract member ParseInlineContent: string -> List<Symbols.Inline>
    abstract member Parse: string -> Symbols.Block

type IRenderer<'T> =
    abstract member Render: Symbols.Block -> 'T
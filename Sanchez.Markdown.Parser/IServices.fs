module IServices

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Symbols.Inline

type IParser =
    abstract member ParseInlineContent: string -> List<Inline>
    abstract member Parse: string -> Block

type IRenderer<'T> =
    abstract member Render: Block -> 'T
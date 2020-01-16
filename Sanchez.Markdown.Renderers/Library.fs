namespace Sanchez.Markdown.Renderers

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Symbols.Inline

type BlockRenderer<'T> = Block -> 'T
type InlineRenderer<'T> = Inline list -> 'T

type IRenderer<'T> =
    abstract member Render: symbol: DocumentSymbol -> specialLookup: (string -> string -> 'T) -> 'T

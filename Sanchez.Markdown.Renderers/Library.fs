namespace Sanchez.Markdown.Renderers

open Sanchez.Markdown.Symbols.Block

type IRenderer<'T> =
    abstract member Render: symbol: DocumentSymbol -> 'T

[<AbstractClass>]
type BaseRenderer<'T, 'U> =
    abstract member CombineNodes: 'U list -> 'T
    abstract member ProcessLine: Block -> 'U

    interface IRenderer<'T> with
        member this.Render symbol =
            symbol
            |> Document
            |> this.ProcessLine
            |> (fun x -> [x])
            |> this.CombineNodes

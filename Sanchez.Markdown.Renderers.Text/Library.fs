namespace Sanchez.Markdown.Renderers.Text

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Renderers

type TextRenderer =
    inherit BaseRenderer<string, string>

    override this.CombineNodes nodes = "Hello"

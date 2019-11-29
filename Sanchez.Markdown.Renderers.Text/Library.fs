namespace Sanchez.Markdown.Renderers.Text

open Sanchez.Markdown.Symbols.Block
open Sanchez.Markdown.Renderers

type TextRenderer() =
    inherit BaseRenderer<string, string>()

    override this.CombineNodes nodes = 
        nodes |> List.reduce (fun acc x -> x + "\n" + acc)

    override this.RenderGroup nodes =
        nodes |> List.reduce (fun acc x -> x + "\n" + acc)
    override this.RenderHeading s = "Heading"
    override this.RenderNewLine s = "NewLine"
    override this.RenderParagraph s = "Paragraph"
    override this.RenderUnorderedList s = "UnorderedList"
    override this.RenderBlockQuote s = "BlockQuote"

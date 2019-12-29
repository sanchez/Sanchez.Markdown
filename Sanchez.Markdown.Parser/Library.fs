namespace Sanchez.Markdown.Parser

open Sanchez.Markdown.Parser.Parsers.Block
open Sanchez.Markdown.Parser.Parsers.Parser
open Sanchez.Markdown.Symbols.Block

module MarkdownParser =
    
    let ParseMetadata (document: string) =
        document.Split [| '\n' |]
        |> Array.toList
        |> List.map (fun x -> x.TrimEnd ())
        |> Metadata.Parse
        |> fst

    let ParseString (document: string) =
        let lines = 
            document.Split [| '\n' |] 
            |> Array.toList
            |> List.map (fun x -> x.TrimEnd ())
            
        let (metadata, documentBeginning) = Metadata.Parse lines
        let content = ParseLines documentBeginning
            
        DocumentSymbol (metadata, content)

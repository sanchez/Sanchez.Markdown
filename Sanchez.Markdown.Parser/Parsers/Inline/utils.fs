module Sanchez.Markdown.Parser.Parsers.Inline.Utils

let rec SearchTillCharacter (characters: char list) searchChar currentList =
    if characters.IsEmpty then
        ([], [])
    elif (characters.Head = searchChar) then
        (currentList, characters.Tail)
    else
        SearchTillCharacter characters.Tail searchChar (currentList @ [characters.Head])

let SeparateCharacterHead (characters: char list) =
    if characters.IsEmpty then
        (None, [])
    else
        (Some characters.Head, characters.Tail)
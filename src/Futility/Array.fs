
module Futility.Array

let inline head (a : 'a array) =
  a.[0]

let inline skip n a =
  Array.sub a n (a.Length - n)

let inline tail a =
  match a with
  | [||] -> [||]
  | [| e |] -> [||]
  | a -> a |> skip 1

// count


module Futility.List

let inline count f l =
  l |> List.filter f |> List.length

let inline mapKeep f l =
  let map x = x, f x
  List.map map l

let inline skip n l =
  l
  |> Seq.skip n 
  |> List.ofSeq // todo: make faster

let inline get n l =
  List.nth l n

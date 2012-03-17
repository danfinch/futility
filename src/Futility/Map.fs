
module Futility.Map

let inline mapKeys (f : 'a -> 'b -> 'a) (m : Map<'a, 'b>) =
  let arr = System.Array.CreateInstance (typeof<'a * 'b>, m.Count)
  let i = ref 0
  for p in m do
    arr.SetValue ((f p.Key p.Value, p.Value), !i)
    i := !i + 1
  arr :> obj :?> ('a * 'b) seq |> Map.ofSeq

let inline merge n o =
  let o =
    o |> Map.toSeq |> Seq.filter 
      (fun (k, v) -> n |> Map.containsKey k |> not)
  Map.toSeq n
  |> Seq.append o
  |> Map.ofSeq

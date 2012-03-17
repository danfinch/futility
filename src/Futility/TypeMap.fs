
namespace Futility

open System

/// Mutable, thread-safe, keyed-by-type container.
type TypeMap () =
  let dict = Dict<Type, obj> ()
  member self.Has t =
    lock dict
    <| fun _ ->
      dict.ContainsKey t
  member self.Set (t, o) =
    lock dict
    <| fun _ ->
      if dict.ContainsKey t then
        dict.[t] <- o
      else
        dict.Add (t, o)
  member self.Get t =
    lock dict
    <| fun _ ->
      if dict.ContainsKey t then
        Some dict.[t]
      else
        None
  interface Collections.IEnumerable with
    member self.GetEnumerator () =
      dict.GetEnumerator () :> Collections.IEnumerator
  interface seq<Pair<Type, obj>> with
    member self.GetEnumerator () =
      (dict :> seq<Pair<Type, obj>>).GetEnumerator ()

type TypeMap with
  member self.Has<'a> () = self.Has typeof<'a>
  member self.Get<'a> () = match self.Get typeof<'a> with None -> None | Some x -> Some (x :?> 'a)
  member self.Set<'a> (o : 'a) = self.Set (typeof<'a>, o)


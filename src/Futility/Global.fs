
[<AutoOpen>]
module Futility.Global

open System

// Utility functions

let inline dispose (d : #IDisposable) = d.Dispose ()
let inline isNull (a : obj) = Object.ReferenceEquals (a, null)
let inline notNull (a : obj) = not (Object.ReferenceEquals (a, null))
let inline lockf x f =
  let r = ref None
  let inner i =
    lock x (fun _ -> r := f i |> Some)
    r.Value.Value
  inner

// Operators

let inline (!) x = (^a : (member Value : ^b) x)
let inline (==) (a : obj) (b : obj) = Object.ReferenceEquals (a, b)
let inline ( |? ) x y = match x with Some x -> x | _ -> y
let inline ( |?? ) x y = match x with Some x' -> x | _ -> y
let inline ( !* ) (f : ('a * 'b) -> 'c) = fun a b -> f (b, a)

// Abbreviations

type Dict<'k, 'v> = System.Collections.Generic.Dictionary<'k, 'v>
type IDict<'k, 'v> = System.Collections.Generic.IDictionary<'k, 'v>
type IResizeArray<'a> = System.Collections.Generic.IList<'a>
type Pair<'k, 'v> = System.Collections.Generic.KeyValuePair<'k, 'v>
type oseq = System.Collections.IEnumerable
type Agent<'a> = MailboxProcessor<'a>

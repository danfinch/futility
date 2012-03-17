
[<AutoOpen>]
module Futility.Reflection

open System
open System.Reflection
open Microsoft.FSharp.Reflection

module Type =
  let inline getMethod (name : string) (t : Type) =
    t.GetMethod name
  let inline getMethods (t : Type) =
    t.GetMethods ()
    |> List.ofArray
  let inline getProperty (name : string) (t : Type) =
    t.GetProperty name
  let inline getProperties (t : Type) =
    t.GetProperties ()
    |> List.ofArray
  let inline createInstance (args : obj list) (t : Type) =
    Activator.CreateInstance (t, args |> List.toArray)
  let inline get (o : #obj) =
    o.GetType ()
  let inline makeGeneric (ets : Type list) (t : Type) =
    t.MakeGenericType (ets |> List.toArray)

module Enum =
  let inline ofString t s =
    Enum.Parse (t, s)

module Method =
  let inline isStatic (m : #MethodBase) =
    m.IsStatic
  let inline makeGeneric ts (m : MethodInfo) =
    m.MakeGenericMethod (ts |> Array.ofList)
  let inline invoke (o : obj) args (m : #MethodBase) =
    m.Invoke (o, args |> Array.ofList)
  let inline invokeStatic args (m : #MethodBase) =
    m.Invoke (null, args)
  let inline getParameters (m : #MethodBase) =
    m.GetParameters ()
    |> List.ofArray

module Property =
  let inline getValue (x : obj) (p : PropertyInfo) =
    p.GetValue (x, null)

module Attribute =
  let inline pick<'a when 'a :> Attribute> (ap : ICustomAttributeProvider) : 'a =
    let t = typeof<'a>
    ap.GetCustomAttributes (t, false)
    |> Array.head
    :?> 'a

  let inline tryPick<'a when 'a :> Attribute> (ap : ICustomAttributeProvider) : 'a option =
    let t = typeof<'a>
    match ap.GetCustomAttributes (t, false) with
    | [| |] -> None
    | x -> Some (x.[0] :?> 'a)

module Tuple =
  let inline createInstance (t : Type) (es : obj array) =
    FSharpValue.MakeTuple (es, t)

module FSharpUnion =
  let inline createInstance (c : UnionCaseInfo) (es : obj array) =
    FSharpValue.MakeUnion (c, es)

module FSharpRecord =
  let inline make (t : Type) (es : obj list) =
    FSharpValue.MakeRecord (t, es |> List.toArray)
  let inline getFields (t : Type) =
    FSharpType.GetRecordFields (t)
    |> List.ofArray

// Partial patterns

let inline (|OptionType|_|) (t : Type) =
  if t.IsGenericType && t.GetGenericTypeDefinition () = typedefof<option<_>> then
    Some (t.GetGenericArguments().[0])
  else
    None
let inline (|ListType|_|) (t : Type) =
  if t.IsGenericType && t.GetGenericTypeDefinition () = typedefof<List<_>> then
    Some (t.GetGenericArguments().[0])
  else
    None
let inline (|MapType|_|) (t : Type) =
  if t.IsGenericType && t.GetGenericTypeDefinition () = typedefof<Map<_,_>> then
    Some ((t.GetGenericArguments().[0]), (t.GetGenericArguments().[1]))
  else None
let inline (|DictType|_|) (t : Type) =
  if t.IsGenericType then
    let gt = t.GetGenericTypeDefinition ()
    if gt = typedefof<System.Collections.Generic.IDictionary<_,_>> || gt = typedefof<System.Collections.Generic.Dictionary<_,_>> then
      Some ((t.GetGenericArguments().[0]), (t.GetGenericArguments().[1]))
    else
      None
  else
    None
let inline (|PairType|_|) (t : Type) =
  if t.IsGenericType && t.GetGenericTypeDefinition () = typedefof<Pair<_, _>> then
    Some ((t.GetGenericArguments().[0]), (t.GetGenericArguments().[1]))
  else
    None
let inline (|ResizeArrayType|_|) (t : Type) =
  if t.IsGenericType then
    let gt = t.GetGenericTypeDefinition ()
    if gt = typedefof<Collections.Generic.IList<_>> || gt = typedefof<Collections.Generic.List<_>> then
      Some (t.GetGenericArguments().[0])
    else
      None
  else
    None
let inline (|SeqType|_|) (t : Type) =
  if t.IsGenericType && t.GetGenericTypeDefinition () = typedefof<seq<_>> then
    Some (t.GetGenericArguments().[0])
  else None

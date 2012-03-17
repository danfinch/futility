
module Futility.Seq

let inline cons (x : 'a) (xs : #seq<'a>) =
  seq { yield x
        yield! xs }

let inline tail (n : int) (s : #seq<_>) =
  Seq.skip 1 s

let inline tryFirst f (xs : #seq<'a>) =
  use e = xs.GetEnumerator()
  let mutable res = None
  while (Option.isNone res && e.MoveNext()) do
    res <- f e.Current
  res

let inline last (xs : #seq<_>) =
  use e = xs.GetEnumerator ()
  let last = ref None
  let brk = e.MoveNext () |> not |> ref
  while not !brk do
    last := Some e.Current
    if not <| e.MoveNext () then
      brk := true
  last.Value.Value

let inline tryTake (n : int) (s : #seq<_>) =
  if n < 1 then failwith "n < 1"
  if isNull s then
    Seq.empty
  else
    let e = s.GetEnumerator ()
    let i = ref 0
    seq {
        while e.MoveNext () && !i < n do
            i := !i + 1
            yield e.Current
        e.Dispose ()
    }

let inline trySkip (n : int) (s : #seq<_>) =
  let n = if n < 0 then 0 else n
  if isNull s then
    Seq.empty
  else
    let e = s.GetEnumerator ()
    let i = ref 0
    seq {
      while e.MoveNext () do
        i := !i + 1
        if !i > n then
          yield e.Current
      e.Dispose ()
    }

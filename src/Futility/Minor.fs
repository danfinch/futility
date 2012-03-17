
namespace Futility

open System

module Decimal =
  let inline ofBits (bits : int array) = Decimal bits
  let inline tryParse s =
    let v = ref (decimal 0)
    if Decimal.TryParse (s, v) then
      Some !v
    else None

module DateTime =
  let inline ofTicks (ticks : int64) = DateTime ticks

module TimeSpan =
  let inline ofTicks (ticks : int64) = TimeSpan ticks

module Uri =
  let inline ofString (s : string) = Uri s

module Dict =
  let inline get (k : 'k) (d : Dict<'k, 'v>) =
    if d.ContainsKey k then
      Some d.[k]
    else
      None

module Int32 =
  let inline tryParse s =
    let v = ref 0
    if Int32.TryParse (s, v) then
      Some !v
    else None

module Int64 =
  let inline tryParse s =
    let v = ref 0L
    if Int64.TryParse (s, v) then
      Some !v
    else None

module Single =
  let inline tryParse s =
    let v = ref 0.f
    if Single.TryParse (s, v) then
      Some !v
    else None

module Double =
  let inline tryParse s =
    let v = ref 0.0
    if Double.TryParse (s, v) then
      Some !v
    else None

module Byte =
  let inline tryParse s =
    let v = ref 0uy
    if Byte.TryParse (s, v) then
      Some !v
    else None

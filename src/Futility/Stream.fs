
module Futility.Stream

open System
open System.IO

let inline readAllText (s : Stream) =
  let sr = new StreamReader(s)
  sr.ReadToEnd()
  
let inline writeText (t : string) (s : Stream) = // todo: no writer
  let sw = new StreamWriter(s)
  sw.Write t
  sw.Flush ()


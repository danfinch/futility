
[<AutoOpen>]
module Futility.StreamExtensions

open System
open System.IO
open System.Text

type System.IO.Stream with
  member inline self.AsyncReadByte () =
    async {
      let! bytes = self.AsyncRead 1
      return bytes.[0]
    }
  member inline self.AsyncReadChar () = // todo: encoding
    async {
      let! bs = self.AsyncRead 2
      return BitConverter.ToChar (bs, 0)
    }
  member inline self.AsyncReadInt16 () =
    async {
      let! bs = self.AsyncRead 2
      return BitConverter.ToInt16 (bs, 0)
    }
  member inline self.AsyncReadUInt16 () =
    async {
      let! bs = self.AsyncRead 2
      return BitConverter.ToUInt16 (bs, 0)
    }
  member inline self.AsyncReadInt32 () =
    async {
      let! bs = self.AsyncRead 4
      return BitConverter.ToInt32 (bs, 0)
    }
  member inline self.AsyncReadUInt32 () =
    async {
      let! bs = self.AsyncRead 4
      return BitConverter.ToUInt32 (bs, 0)
    }
  member inline self.AsyncReadInt64 () =
    async {
      let! bs = self.AsyncRead 8
      return BitConverter.ToInt64 (bs, 0)
    }
  member inline self.AsyncReadUInt64 () =
    async {
      let! bs = self.AsyncRead 8
      return BitConverter.ToUInt64 (bs, 0)
    }
  member inline self.AsyncReadSingle () =
    async {
      let! bs = self.AsyncRead 4
      return BitConverter.ToSingle (bs, 0)
    }
  member inline self.AsyncReadDouble () =
    async {
      let! bs = self.AsyncRead 8
      return BitConverter.ToDouble (bs, 0)
    }
  member inline self.AsyncReadDecimal () =
    async {
      let! bs = self.AsyncRead 16
      let is = [ for n in [0 .. 3] -> BitConverter.ToInt32 (bs, n * 4) ] |> Array.ofList
      return Decimal is
    }
  member inline self.AsyncReadGuid () =
    async {
      let! bs = self.AsyncRead 16
      return Guid bs
    }
  member inline self.AsyncReadDateTime () =
    async {
      let! bs = self.AsyncRead 16
      return DateTime (BitConverter.ToInt64 (bs, 0))
    }
  member inline self.AsyncReadTimeSpan () =
    async {
      let! bs = self.AsyncRead 16
      return TimeSpan (BitConverter.ToInt64 (bs, 0))
    }
  /// Reads Int32 byte length (or -1 for null), then string, using default encoding.
  member inline self.AsyncReadString () =
    async {
      let! len = self.AsyncReadInt32 ()
      match len with
      | -1 -> return null
      | _ ->
        let! bytes = self.AsyncRead len
        return bytes |> Text.Encoding.Default.GetString
    }
  /// Reads a string of the specified byte length using default encoding.
  member inline self.AsyncReadString (len : int) =
    async {
      let! bytes = self.AsyncRead len
      return bytes |> Text.Encoding.Default.GetString
    }
  /// Reads a string of the specified byte length using specified encoding.
  member inline self.AsyncReadString (len : int) (encoding : Encoding) =
    async {
      let! bytes = self.AsyncRead len
      return bytes |> encoding.GetString
    }
  member inline self.AsyncWrite (x : bool) =
    async {
      do! self.AsyncWrite ([|(if x then 1uy else 0uy)|], 0, 1)
    }
  member inline self.AsyncWrite (x : byte) =
    async {
      do! self.AsyncWrite ([|x|], 0, 1)
    }
  member inline self.AsyncWrite (x : sbyte) =
    async {
      do! self.AsyncWrite ([|(byte x)|], 0, 1)
    }
  member inline self.AsyncWrite (x : char) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 2)
    }
  member inline self.AsyncWrite (x : int16) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 2)
    }
  member inline self.AsyncWrite (x : uint16) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 2)
    }
  member inline self.AsyncWrite (x : int) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 4)
    }
  member inline self.AsyncWrite (x : uint32) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 4)
    }
  member inline self.AsyncWrite (x : int64) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 8)
    }
  member inline self.AsyncWrite (x : uint64) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 8)
    }
  member inline self.AsyncWrite (x : single) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 4)
    }
  member inline self.AsyncWrite (x : double) =
    async {
      let y = BitConverter.GetBytes x
      do! self.AsyncWrite (y, 0, 8)
    }
  member inline self.AsyncWrite (x : decimal) =
    async {
      let y = Decimal.GetBits x
      let z = Array.create 16 0uy
      Buffer.BlockCopy (y, 0, z, 0, 16)
      do! self.AsyncWrite (z, 0, 16)
    }
  member inline self.AsyncWrite (x : DateTime) =
    async {
      do! self.AsyncWrite x.Ticks
    }
  member inline self.AsyncWrite (x : TimeSpan) =
    async {
      do! self.AsyncWrite x.Ticks
    }
  member inline self.AsyncWrite (x : Guid) =
    async {
      do! self.AsyncWrite (x.ToByteArray (), 0, 16)
    }
  /// Writes Int32 length (or -1 for null), then string, using default encoding.
  member inline self.AsyncWriteWithLength (x : string) =
    async {
      match x with
      | null -> do! self.AsyncWrite -1
      | _ ->
        let strBuf = x |> Text.Encoding.Default.GetBytes
        let lenBuf = strBuf.Length |> BitConverter.GetBytes
        let allBuf = Array.create (strBuf.Length + 4) 0uy
        Buffer.BlockCopy (lenBuf, 0, allBuf, 0, 4)
        Buffer.BlockCopy (strBuf, 0, allBuf, 4, strBuf.Length)
        do! self.AsyncWrite allBuf
    }
  /// Writes string using default encoding.
  member inline self.AsyncWrite (x : string) =
    async {
      match x with
      | null -> do! self.AsyncWrite -1
      | _ ->
        let strBuf = x |> Text.Encoding.Default.GetBytes
        let lenBuf = strBuf.Length |> BitConverter.GetBytes
        let allBuf = Array.create (strBuf.Length + 4) 0uy
        Buffer.BlockCopy (lenBuf, 0, allBuf, 0, 4)
        Buffer.BlockCopy (strBuf, 0, allBuf, 4, strBuf.Length)
        do! self.AsyncWrite allBuf
    }

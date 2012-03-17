
module Futility.String

// todo: null guard?

open System

/// Returns a copy of the given string in uppercase using invariant casing rules.
let inline upper (s : string) =
  s.ToUpperInvariant ()
  
/// Returns a copy of the given string in lowercase using invariant casing rules.
let inline lower (s : string) =
  s.ToLowerInvariant ()

/// Indicates whether a given string ends with the other.
let inline endsWith (endsWith : string) (source : string) =
  source.EndsWith endsWith
  
/// Indicates whether a given string begins with the other.
let inline startsWith (startsWith : string) (source : string) =
  source.StartsWith startsWith

/// Returns a substring occurring after the given number of characters.
let inline skip (n : int) (source : string) =
  source.Substring (n)

/// Returns a substring of the leftmost given number of characters.
let inline left (n : int) (source : string) =
  source.Substring (0, n)

/// Returns a substring beginning at the specified index.
let inline sub (start : int) (n : int) (source : string) =
  source.Substring (start, n)

/// Returns a substring using inclusive boundaries.
let inline range (startIndex : int) (endIndex : int) (source : string) =
  source.Substring (startIndex, endIndex - startIndex)

/// Returns a substring of the rightmost given number of characters.
let inline right (n : int) (source : string) =
  source.Substring (source.Length - n, n)

/// Returns a string with leading and trailing whitespace removed.
let inline trim (s : string) =
  s.Trim ()

let inline trimAny (cs : char list) (s : string) =
  s.Trim (cs |> Array.ofSeq)

let inline trimLeftAny (cs : char list) (s : string) =
  s.TrimStart (cs |> Array.ofSeq)

let inline trimRightAny (cs : char list) (s : string) =
  s.TrimEnd (cs |> Array.ofSeq)

/// Returns a string list which contains substrings delimited by any of the given characters, with empty entries removed.
let inline split (cs : char list) (s : string) =
  s.Split (cs |> List.toArray, StringSplitOptions.RemoveEmptyEntries)
  |> List.ofArray

/// Returns the character at the specified index.
let inline get (s : string) i =
  s.[i]

/// Replaces 'sub' with 'repl' in 'source'.
let inline replace (sub : string) (repl : string) (source : string) =
  source.Replace (sub, repl)

/// Creates a string of the specified length filled with the specified character.
let inline create (size : int) (c : char) =
  new String (Array.create size c)

/// Reports the first occurrence of a string within a string.
let inline indexOf (x : string) (source : string) =
  source.IndexOf x

/// Reports the last occurrence of a string within a string.
let inline lastIndexOf (x : string) (source : string) =
  source.LastIndexOf x

/// Appends a string to another.
let inline append (x : string) (source : string) =
  source + x

/// Prepends a string to another.
let inline prepend (x : string) (source : string) =
  x + source

/// Concatenates the elements of a sequence, using the specified separator character.
let inline join (separator : string) (source : string seq) =
  String.Join (separator, source)

let inline fromBase64 (source : string) =
  Convert.FromBase64String (source)

/// Creates a System.Security.SecureString from a string.
let secure s =
  let ss = new System.Security.SecureString()
  for c in s do
    ss.AppendChar c
  ss


// todo:
// insert
// length
// indexOfAny
// lastIndexOfAny
// padLeft
// padRight
// trimLeft
// trimRight
// isEmpty
// isNullOrEmpty
// isNullOrWhitespace

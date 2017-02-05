Option Strict On
Imports CommandLineParse
Imports System.io
Imports System.Text.RegularExpressions
Module Module1

   Sub Main()
      Dim args As String
      Dim parser As CommandLineParser
      Dim sErr As String
      Dim anEntry As CommandLineEntry
      Dim i As Integer
      parser = New CommandLineParser()
      Dim unmatched As CommandLineParse.UnmatchedTokens

      Do
         SetupCommandLineEntries(parser)
         Console.WriteLine("Enter a command line and press Enter to test it, or type ""exit"" and press Enter to quit.")
         Console.Write("Command Line: > ")
         args = Console.ReadLine
         If args.ToLower = "exit" Then
            End
         End If
         parser.CommandLine = args
         If parser.Parse() Then
            Console.WriteLine("Successful parse")
            Console.WriteLine("")
         Else
            Console.WriteLine("Parse failed")
            For Each sErr In parser.Errors
               Console.WriteLine("Reason: " & sErr)
            Next
            Console.WriteLine("")
         End If

         ' did the command line contain unmatched tokens?
         unmatched = parser.UnmatchedTokens
         If unmatched.Count > 0 Then
            Console.WriteLine("Some tokens were not matched")
            Dim aBadToken As String
            For Each aBadToken In unmatched
               Console.WriteLine("Unmatched token: " & aBadToken)
            Next
         End If
         Console.WriteLine("")
         i = 0
         For Each anEntry In parser.Entries
            If anEntry.HasValue Then
               i += 1
            End If
            Console.WriteLine(anEntry.ToString)
            Console.WriteLine(String.Empty)
         Next
      Loop
   End Sub
   Sub SetupCommandLineEntries(ByVal parser _
      As CommandLineParser)

      Dim anEntry As CommandLineEntry
      parser.Errors.Clear()
      parser.Entries.Clear()
      ' create a flag type entry that accepts a -f (file) 
      ' flag, (meaning the next parameter is a file 
      ' name), and is required 
      anEntry = parser.CreateEntry _
         (CommandLineParse.CommandTypeEnum.Flag, "f")
      anEntry.Required = True
      parser.Entries.Add(anEntry)

      ' store the new Entry in a local reference
      ' for use with the next CommandLineEntry's 
      ' MustFollow property.
      Dim fileEntry As CommandLineEntry
      fileEntry = anEntry

      ' now create am ExistingFile type entry that must
      ' follow the -f flag.
      anEntry = parser.CreateEntry _
      (CommandTypeEnum.ExistingFile)
      anEntry.MustFollowEntry = fileEntry
      anEntry.Required = True
      parser.Entries.Add(anEntry)

   End Sub
End Module

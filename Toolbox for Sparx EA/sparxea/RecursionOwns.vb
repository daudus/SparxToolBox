Public Module RecursionOwns

    Public Const SidePatternEnd As Byte = 0
    Public Const SidePatternStart As Byte = 1
    Public Const SidePatternMiddle As Byte = 2

    Public Const PatternSeparator = "_"
    Public Const PatternDelimiter = "."


    Public Sub DoRecursionOwns(ByRef package As EA.Package, wherePattern As Byte)

    End Sub

    Function _do(ByRef parent As EA.Element) As Boolean

        Return True
    End Function

    Function GetPattern(name As String) As Byte()
        Dim s As String
        Dim sPatternSplitted() As String
        s = name.Split(PatternSeparator)(1)
        sPatternSplitted = s.Split(PatternDelimiter)

        Dim nPattern(sPatternSplitted.Length) As Byte
        For i = 0 To sPatternSplitted.Length - 1
            nPattern(i) = CByte(sPatternSplitted(i))
        Next i
        Return nPattern
    End Function
End Module

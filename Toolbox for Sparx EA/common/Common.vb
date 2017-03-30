Module Common
    Dim key As ConsoleKeyInfo
    Dim STANDALONE As Boolean = True
    Public ReadOnly lLOG As log4net.ILog = log4net.LogManager.GetLogger("main")

    Public Sub InitLog()
    End Sub
    Public Sub Oldlog(msg As String, wait As Boolean)
        If STANDALONE Then
            Console.WriteLine(msg)
            lLOG.Debug(msg)
            If wait Then
                Console.WriteLine("press any key to continue")
                key = Console.ReadKey()
            End If
        Else
            If wait Then
                'Session.Prompt(msg, 0)
            Else
                'Session.Output(msg)
            End If
        End If
    End Sub
    Public Sub PopulateMessageArray(ByRef listMsg As ArrayList, level As Core.Level)
        Dim msg As String

        For Each msg In listMsg
            Select Case level
                Case Core.Level.Debug
                    lLOG.Debug(msg)
                Case Core.Level.Info
                    lLOG.Info(msg)
                Case Core.Level.Error
                    lLOG.Error(msg)
                Case Core.Level.Fatal
                    lLOG.Fatal(msg)
                Case Else
                    lLOG.Error("populateMessageArray is called with unknown Level of logging: " + level.Name)
                    lLOG.Debug(msg)
            End Select
        Next msg
    End Sub
    Public Function AreArraysEqual(Of T)(ByVal a As T(), ByVal b() As T) As Boolean

        'IF 2 NULL REFERENCES WERE PASSED IN, THEN RETURN TRUE, YOU MAY WANT TO RETURN FALSE
        If a Is Nothing AndAlso b Is Nothing Then Return True

        'CHECK THAT THERE IS NOT 1 NULL REFERENCE ARRAY
        If a Is Nothing Or b Is Nothing Then Return False

        'AT THIS POINT NEITHER ARRAY IS NULL
        'IF LENGTHS DON'T MATCH, THEY ARE NOT EQUAL
        If a.Length <> b.Length Then Return False

        'LOOP ARRAYS TO COMPARE CONTENTS
        For i As Integer = 0 To a.GetUpperBound(0)
            'RETURN FALSE AS SOON AS THERE IS NO MATCH
            If Not a(i).Equals(b(i)) Then Return False
        Next

        'IF WE GOT HERE, THE ARRAYS ARE EQUAL
        Return True

    End Function
End Module

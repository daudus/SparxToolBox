Module Common
    Dim key As ConsoleKeyInfo
    Dim STANDALONE As Boolean = True
    Public ReadOnly lLOG As log4net.ILog = log4net.LogManager.GetLogger("main")

    Public Sub initLog()
    End Sub
    Public Sub oldlog(msg As String, wait As Boolean)
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
    Public Sub populateMessageArray(ByRef listMsg As ArrayList, level As Core.Level)
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
End Module

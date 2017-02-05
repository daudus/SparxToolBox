Module Common
    Dim key As ConsoleKeyInfo
    Dim STANDALONE As Boolean = True
    Public ReadOnly lLOG As log4net.ILog = log4net.LogManager.GetLogger("GeneralLogger")


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
End Module

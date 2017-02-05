Imports CommandLineParse

Public Class AppConfig
    Private Const cParameterSwitch = "-"
    Private Const cOperationSwitch = "o"
    Private pOperation As String = ""

    Sub New(ByRef sArgs As String())
        If sArgs.Length = 0 Then                      'If there are no arguments
            lLOG.Info("<-no arguments passed->")      'Just output Hello World
        Else                                          'We have some arguments 
            For Each s As String In sArgs
                s = s.ToLower
                s.Re
                If s.Equals((cParameterSwitch + cOperationSwitch).ToLower) Then
                Else
                End If
            Next


            Dim j As Integer = 0

            While j < sArgs.Length             'So with each argument
                lLOG.Info("param: " & sArgs(j) + " and its value: " + sArgs(j + 1))       'Print out each item
                j = j + 2                       'Increment to the next argument
            End While
        End If
    End Sub
    Sub New(ByVal OperationSwitch As String, ByVal Operation As String)
        pOperationSwitch = OperationSwitch
        pOperation = Operation
    End Sub

    Public Property OperationSwitch() As String
        Get
            Return pOperationSwitch
        End Get
        Set(ByVal value As String)
            pOperationSwitch = value
        End Set
    End Property

    Public Property Operation() As String
        Get
            Return pOperation
        End Get
        Set(ByVal value As String)
            pOperation = value
        End Set
    End Property
End Class

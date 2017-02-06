'filled by command line
'example syntax: -switch=argument
Public Class AppConfig
    Private Const cParameterSwitch = "-"
    Private Const cParameterSeparator = "="
    Private Const cOperationSwitch = "op"
    Private Const cOperationCleanImport = "cleanimport"
    Private Const cOperationImport = "import"


    Private pOperationCleanImport As Boolean = False
    Private pOperationImport As Boolean = False

    Sub New(ByRef sArgs As String())

        Me.New(False, False)
        Dim exception As Boolean = False
        Dim message As String = ""
        Dim op As String

        If sArgs.Length = 0 Then
            lLOG.Fatal("No arguments passed in command line")
        Else
            'We have some arguments 
            dumpArguments(sArgs)
            For Each s As String In sArgs
                s = s.ToLower
                If s.StartsWith((cParameterSwitch + cOperationSwitch + cParameterSeparator).ToLower) Then
                    op = s.Remove(0, (cParameterSwitch + cOperationSwitch + cParameterSeparator).Length)
                    Select Case op
                        Case cOperationCleanImport.ToLower
                            pOperationCleanImport = True
                        Case cOperationImport.ToLower
                            pOperationImport = True
                        Case Else
                            'nothing
                    End Select
                End If
            Next
        End If
        message = validate()
        If Not IsNothing(message) Then
            lLOG.Fatal(message)
            Throw New Exception(message)
        End If
    End Sub
    Sub New(ByVal OperationCleanImport As Boolean, ByVal OperationImport As Boolean)
        pOperationCleanImport = OperationCleanImport
        pOperationImport = OperationImport
    End Sub

    Public ReadOnly Property OperationCleanImport() As String
        Get
            Return pOperationCleanImport
        End Get
    End Property

    Public ReadOnly Property OperationImport() As String
        Get
            Return pOperationImport
        End Get
    End Property

    Private Function validate() As String
        Dim message As String = Nothing
        If Not (pOperationCleanImport Or pOperationImport) Then
            message = "No valid arguments passed in command line"
        ElseIf (pOperationCleanImport And pOperationImport) Then
            message = "Incompatible arguments passed"
        Else
            If pOperationCleanImport Then
                lLOG.Info("System will perform " + cOperationCleanImport)
            ElseIf pOperationImport Then
                lLOG.Info("System will perform " + cOperationImport)
            End If
        End If
            validate = message
    End Function
    Private Sub dumpArguments(ByRef sArgs As String())
        Dim j As Integer = 0
        While j < sArgs.Length             'So with each argument
            lLOG.Info("param: " & sArgs(j))       'Print out each item
            j = j + 1                       'Increment to the next argument
        End While
    End Sub
End Class

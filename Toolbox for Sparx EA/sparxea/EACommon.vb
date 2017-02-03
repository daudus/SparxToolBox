Module EACommon
    Function getApp() As Object
        Dim EAapp As Object = Nothing
        lLOG.Info("Getting the Sparx EA application instance")
        Try
            EAapp = GetObject(, "EA.App")
        Catch e As Exception
            lLOG.Debug(e.Message)
        End Try
        If IsNothing(EAapp) Then
            EAapp = CreateObject("EA.App")
            lLOG.Info("New Sparx EA application instance was created")
            EAapp.Repository.OpenFile(My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile)
            lLOG.Info("Repository loaded: " & My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile)
        Else
            lLOG.Info("Running Sparx EA application instance will be used")
            lLOG.Info("Repository already opened: " & EAapp.Repository.ConnectionString)
        End If
        EAapp.Visible = True
        If String.Compare(EAapp.Repository.ConnectionString, My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile) <> 0 Then
            lLOG.Fatal("Wrong Repository Detected. Opened <" & EAapp.Repository.ConnectionString & "> but expected <" & My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile & "> ")
            close(EAapp, EAapp.Repository, True)
        End If
        Return EAapp
    End Function

    Function getRepository(ByRef EAapp As Object) As EA.Repository
        Return EAapp.Repository
    End Function

    Sub close(ByRef EAapp As Object, ByRef Repository As EA.Repository, close As Boolean)
        lLOG.Info("System is being to be closed")
        If close Then
            lLOG.Info("Sparx EA Repository as well as Sparx EA Application is being to be closed")
            Repository.Exit()
            Repository = Nothing
            EAapp = Nothing
        Else
            lLOG.Info("Sparx EA Repository is still running!")
        End If
    End Sub

    '**
    ' Generates And returns a Globally Unique Identifier in string form
    '
    ' @return A string representing a New globally unique identifier
    '/
    Function GUIDGenerateGUID() As String
        Dim typeLib = CreateObject("Scriptlet.TypeLib")

        ' GUID returned from typeLib has 2 unprintable characters at the end which stuff up string
        ' manipulation later on
        Return Left(typeLib.Guid(), 38)

    End Function
End Module

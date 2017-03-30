Module EACommon
    Function GetApp() As Object
        Dim EAapp As EA.App = Nothing
        lLOG.Info("Getting the Sparx EA application instance")
        Try
            EAapp = GetObject(, EAConstants.activeXEA)
        Catch e As Exception
            lLOG.Info("No Sparx EA instance is running ...")
            lLOG.Debug(e.Message)
        End Try
        Try
            If IsNothing(EAapp) Then
                lLOG.Info("Try to create new Sparx EA instance ...")
                EAapp = CreateObject(EAConstants.activeXEA)
                lLOG.Info("New Sparx EA application instance was created")
                EAapp.Repository.OpenFile(My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile)
                lLOG.Info("Repository loaded: " & My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile)
            Else
                lLOG.Info("Running Sparx EA application instance will be used")
                lLOG.Info("Repository already opened: " & EAapp.Repository.ConnectionString)
            End If
            EAapp.Visible = True
            If String.Compare(UCase(EAapp.Repository.ConnectionString), UCase(My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile)) <> 0 Then
                lLOG.Fatal("Wrong Repository Detected. Opened <" & EAapp.Repository.ConnectionString & "> but expected <" & My.Settings.SparxEATargetRepostoryDirectory & My.Settings.SparxEATargetRepostoryFile & "> ")
                close(EAapp, EAapp.Repository, True)
            End If
        Catch ee As Exception
            lLOG.Fatal("something wrong during creating the Sparx EA instance ... " + ee.Message)
        End Try
        Return EAapp
    End Function

    Function GetRepository(ByRef EAapp As Object) As EA.Repository
        Return EAapp.Repository
    End Function

    Function GetModel(ByRef repository As EA.Repository) As Object
        Dim found As Boolean = False
        Dim model As Object = Nothing
        Dim idx As Integer = 0

        lLOG.Info("Finding model in EA Repository: " + My.Settings.SparxEATargetRepostoryModelArchiImported)
        While (Not found) And (idx < repository.Models.Count)
            model = repository.Models.GetAt(idx)
            lLOG.Debug("EA model: " + model.Name)
            If model.Name.Equals(My.Settings.SparxEATargetRepostoryModelArchiImported) Then
                found = True
            End If
            idx = idx + 1
        End While
        If idx = 0 Then
            lLOG.Error("No Model found in Sparx EA repository.")
        End If
        If Not found Then
            model = Nothing
            lLOG.Error("Model " + My.Settings.SparxEATargetRepostoryModelArchiImported + " not found.")
            lLOG.Info("Try to crete model " + My.Settings.SparxEATargetRepostoryModelArchiImported)
            model = repository.Models.AddNew(My.Settings.SparxEATargetRepostoryModelArchiImported, "")
            If Not model.Update() Then
                lLOG.Fatal("Model " + My.Settings.SparxEATargetRepostoryModelArchiImported + " not created due to: " & model.GetLastError())
                model = Nothing
            Else
                repository.Models.Refresh()
                lLOG.Info("Model " + My.Settings.SparxEATargetRepostoryModelArchiImported + " successfuly created")
            End If
        End If
        Return model
    End Function

    Sub Close(ByRef EAapp As Object, ByRef Repository As EA.Repository, close As Boolean)
        lLOG.Info("System is being to be closed")
        If close Then
            lLOG.Info("Sparx EA Repository as well as Sparx EA Application is being to be closed")
            Repository.Exit()
            Repository = Nothing
            EAapp = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        Else
            If Not IsNothing(EAapp) Then
                lLOG.Info("Sparx EA Repository is still running!")
            End If
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
    Function GetPackageFromModel(ByRef model As Object) As EA.Package
        Dim found As Boolean = False
        Dim package As EA.Package = Nothing
        Dim idx As Integer = 0

        lLOG.Info("Finding package in EA Repository: " + My.Settings.SparxEATargetRepostoryPackageArchiImported)
        While (Not found) And (idx < model.Packages.Count)
            package = model.Packages.GetAt(idx)
            lLOG.Debug("Package: " + package.Name)

            If package.Name.Equals(My.Settings.SparxEATargetRepostoryPackageArchiImported) Then
                found = True
            Else
                idx = idx + 1
                package = getPackage(My.Settings.SparxEATargetRepostoryPackageArchiImported, package)
                If Not IsNothing(package) Then
                    found = True
                End If
            End If
        End While
        If Not found Then
            package = Nothing
            lLOG.Error("Package " + My.Settings.SparxEATargetRepostoryPackageArchiImported + " not found.")
            lLOG.Info("Try to crete package " + My.Settings.SparxEATargetRepostoryPackageArchiImported)
            package = model.Packages.AddNew(My.Settings.SparxEATargetRepostoryPackageArchiImported, "UNKNOWN")
            If Not package.Update() Then
                lLOG.Fatal("Package " + My.Settings.SparxEATargetRepostoryPackageArchiImported + " not created due to: " & package.GetLastError())
                package = Nothing
            Else
                model.Packages.Refresh()
                lLOG.Info("Package " + My.Settings.SparxEATargetRepostoryPackageArchiImported + " successfuly created")
            End If
        End If
        Return package
    End Function

    Function GetPackage(ByRef name As String, ByVal contextPackage As EA.Package) As EA.Package
        Dim package As EA.Package = Nothing
        Dim found As Boolean = False
        Dim idx As Integer = 0

        While (Not found) And (idx < contextPackage.Packages.Count)
            package = contextPackage.Packages.GetAt(idx)
            lLOG.Debug("Package: " + package.Name)
            If package.Name.Equals(name) Then
                found = True
            Else
                idx = idx + 1
                package = GetPackage(name, package)
                If Not IsNothing(package) Then
                    found = True
                End If
            End If
        End While
        If Not found Then package = Nothing
        Return package
    End Function
End Module

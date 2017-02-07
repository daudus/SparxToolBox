'TODO: ISSUE with inconsistency of exports to CSV format from ARCHI. Lots of "EOL"s in "free text" fields (e.g. documentation) cause exceptional behavioral. 
'Files need to be normalized ... solution is simple: choose "Strip Newline Characters"
'Analysis:
'  22 0a    {",LF} is problem. Has to be replaced by {"}
'  0a 22    {LF,"} is problem. Has to be replaced by {"}. Warning: {CR,LF,"} is valid and OK!
'  0a inside/between "" is problem
'  0d 0a inside/between "" is problem
'  0d 0a    {CR,LF} outside "" is OK.
'
Option Explicit On

<Assembly: log4net.Config.XmlConfigurator(ConfigFile:="log4net.xml", Watch:=True)>

Module Main
    Dim EAapp As Object
    Dim Model As Object
    Dim Repository As EA.Repository
    Dim Package As EA.Package

    Dim mappedElementsFileARCHI As New Hashtable
    Dim mappedPropertiesFileARCHI As New Hashtable
    Dim mappedRelationsFileARCHI As New Hashtable
    Dim appConfig As AppConfig

    Sub Main(ByVal sArgs As String())
        Try
            'inits app by command line parameters
            initApp(sArgs)
        Catch ex As Exception
            lLOG.Fatal("Fatal Error occured. Can not continue due to: " + ex.Message)
            closeApp()
            Exit Sub
        End Try
        'gets the Sparx EA application reference
        EAapp = getApp()
        ' ... and the proper repository
        Repository = getRepository(EAapp)
        If IsNothing(Repository) Then
            lLOG.Fatal("Sparx EA has to have opened any repository")
            closeApp()
            Exit Sub
        End If
        ' ... and the proper model
        Model = getModel(Repository)
        If IsNothing(Model) Then
            lLOG.Fatal("Sparx EA has to have model with gien name: " + My.Settings.SparxEATargetRepostoryModelArchiImported)
            closeApp()
            Exit Sub
        End If
        ' ... and the proper package
        Package = getPackageFromModel(Model)
        If IsNothing(Package) Then
            lLOG.Fatal("Sparx EA has to have repository with gien name: " + My.Settings.SparxEATargetRepostoryPackageArchiImported)
            closeApp()
            Exit Sub
        End If

        'read and map properties from ARCHI export
        mappedPropertiesFileARCHI = loadPropertiesFileARCHI()
        'read and map relations from ARCHI export
        mappedRelationsFileARCHI = loadRelationsFileARCHI()
        'read and map elements from ARCHI export
        'has to be last; after properties and relations!
        mappedElementsFileARCHI = loadElementsFileARCHI()
        createElementsInEA(Package, mappedElementsFileARCHI, mappedPropertiesFileARCHI)
        createRelationsInEA(Repository, mappedRelationsFileARCHI, mappedElementsFileARCHI, mappedPropertiesFileARCHI)

        'finishing the system
        closeApp()
    End Sub
    Sub createRelationsInEA(ByRef repository As EA.Repository, ByRef archiRelations As Hashtable, ByRef archiElements As Hashtable, ByRef archiProperties As Hashtable)
        Dim connector As EA.Connector
        Dim client As EA.Element
        Dim supplier As EA.Element
        Dim relationArchi As ArchiRelation
        Dim sourceArchi, targetArchi As ArchiElement
        Dim spin As ConsoleSpiner
        Dim listMsg As New ArrayList()

        Dim key As String
        Dim keys As Collections.ICollection
        Dim stereotype, type As String

        lLOG.Info("createRelationsInEA started")
        keys = archiRelations.Keys
        spin = New ConsoleSpiner(keys.Count, 1)
        For Each key In keys
            spin.Turn()
            client = Nothing
            supplier = Nothing
            relationArchi = archiRelations(key)
            sourceArchi = archiElements(relationArchi.Source)
            targetArchi = archiElements(relationArchi.Target)
            If IsNothing(sourceArchi) Then
                listMsg.Add("For relation " + relationArchi.ID + "there is no source element " + relationArchi.Source + " in import files")
            Else
                supplier = repository.GetElementByID(sourceArchi.ElementIDEA)
                If IsNothing(targetArchi) Then
                    listMsg.Add("For relation " + relationArchi.ID + "there is no target element " + relationArchi.Target + " in import files")
                Else
                    client = repository.GetElementByID(archiElements(relationArchi.Target).ElementIDEA)
                    stereotype = EAConstants.typeArchi2StereotypeEA(relationArchi.Type.Substring(0, Len(relationArchi.Type) - Len(ArchiConstants.RelationSuffix)))
                    type = EAConstants.stereotype2type(stereotype)

                    connector = supplier.Connectors.AddNew(relationArchi.Name, type)
                    With connector
                        .SupplierID = client.ElementID
                        .Stereotype = EAConstants.metatypeArchimatePrefix & stereotype
                        .Notes = relationArchi.Documentation
                        .Direction = EAConstants.connectorDirectionSourceDestination
                        'TODO: TaggedValues
                        'TODO: store Archi IDs into EA
                        'TODO: store EA IDs into Archi
                        .Update()
                    End With
                End If
            End If
        Next key
        spin.Finish()
        If Not IsNothing(listMsg) Then populateMessageArray(listMsg, Core.Level.Error)
        lLOG.Info("createRelationsInEA finished")
    End Sub
    Sub createElementsInEA(ByRef package As EA.Package, ByRef archiElements As Hashtable, ByRef archiProperties As Hashtable)
        Dim elementEA As EA.Element
        Dim taggedValue As EA.TaggedValue
        Dim elementArchi As ArchiElement
        Dim elementProperty As ArchiProperty
        Dim listMsg As New ArrayList()
        Dim spin As ConsoleSpiner

        Dim stereotype As String
        Dim type As String
        Dim key As String
        Dim keys As Collections.ICollection
        Dim properties As ArrayList

        lLOG.Info("createElementsInEA started")
        keys = archiElements.Keys
        spin = New ConsoleSpiner(keys.Count, 1)
        For Each key In keys
            'TODO: listMsg.Add(some error message) if necessary
            spin.Turn()
            elementArchi = archiElements(key)
            stereotype = EAConstants.typeArchi2StereotypeEA(elementArchi.Type)
            If elementArchi.Type.Equals(ArchiConstants.typeModel) Then
                'nothing. Model root
                'TODO: work with such model in EA also?
            Else
                type = EAConstants.stereotype2type(stereotype)
                elementEA = package.Elements.AddNew(elementArchi.Name, type)
                With elementEA
                    '.FQStereotype = EAConstants.metatypeArchimatePrefix & stereotype
                    .Stereotype = EAConstants.metatypeArchimatePrefix & stereotype
                    .Author = My.Settings.Author
                    .Notes = elementArchi.Documentation
                    '.Profile Metatype = EAConstants.metatypeArchimatePrefix & elementArchi.Type
                    properties = archiProperties(elementArchi.ID)
                    If Not IsNothing(properties) Then
                        For Each elementProperty In properties
                            taggedValue = .TaggedValues.AddNew(elementProperty.Key, elementProperty.Value)
                            taggedValue.Update()
                        Next
                    End If
                    'add reference to ARCHI model
                    taggedValue = .TaggedValues.AddNew(EAConstants.taggedValueArchiID, elementArchi.ID)
                    taggedValue.Update()
                    .TaggedValues.Refresh()
                    .Update()
                    'store EA identifiers into elementArchi
                    elementArchi.GUIDEA = .ElementGUID
                    elementArchi.ElementIDEA = .ElementID
                End With
            End If
        Next key
        spin.Finish()
        If Not IsNothing(listMsg) Then populateMessageArray(listMsg, Core.Level.Error)
        lLOG.Info("Package is being refreshed")
        package.Elements.Refresh()
        lLOG.Info("Package is refreshed")
        lLOG.Info("Package is being updated. It will take a while ...")
        package.Update()
        lLOG.Info("Package is updated")
        lLOG.Info("createElementsInEA finished")
    End Sub
    Sub initApp(ByRef sArgs As String())
        appConfig = New AppConfig(sArgs)
    End Sub
    Sub closeApp()
        mappedElementsFileARCHI.Clear()
        mappedElementsFileARCHI = Nothing
        mappedPropertiesFileARCHI.Clear()
        mappedPropertiesFileARCHI = Nothing
        mappedRelationsFileARCHI.Clear()
        mappedRelationsFileARCHI = Nothing
        close(EAapp, Repository, False)
        Console.WriteLine("PRESS ANY KEY TO EXIT")
        Console.ReadKey()
    End Sub
End Module

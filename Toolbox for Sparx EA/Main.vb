﻿'TODO: ISSUE with inconsistency of exports to CSV format from ARCHI. Lots of "EOL"s in "free text" fields (e.g. documentation) cause exceptional behavioral. 
'Files need to be normalized ... solution is simple: choose "Strip Newline Characters"
'Analysis:
'  22 0a    {",LF} is problem. Has to be replaced by {"}
'  0a 22    {LF,"} is problem. Has to be replaced by {"}. Warning: {CR,LF,"} is valid and OK!
'  0a inside/between "" is problem
'  0d 0a inside/between "" is problem
'  0d 0a    {CR,LF} outside "" is OK.
'TODO: property for property. Where stored EA.xref in ARCHI? In EA there is possibility into TagValue documentation ......example code is in early versions of Main.Create?????
'TODO: support oneway update ARCHI2SPARXEA
'TODO: help about parameters in command line
'TODO: reflect package structure in EA. Currently onlz one package is assumend
'TODO: support diagrams
'TODO: support twoway update ARCHI<->SPARXEA
'TODO: composition needs to be fixed  - SourceIsAggregate=2. Direction = Unspecified. Still actual?
'TODO: create Archi test model with all fatures of Archimate3 and test me with it
Option Explicit On

<Assembly: log4net.Config.XmlConfigurator(ConfigFile:="log4net.xml", Watch:=True)>

Module Main
    Dim EAapp As Object
    Dim Model As Object
    Dim Repository As EA.Repository
    Dim Package As EA.Package

    Dim TimeStampD As DateTime
    Dim TimeStampS As String
    Dim mappedElementsFileARCHI As New Hashtable 'TODO: use dictionary. Dim peopleHashtable As New Dictionary(Of String, Person)
    Dim columsMappedElementsFileARCHI As String()
    Dim mappedPropertiesFileARCHI As New Hashtable
    Dim columsMappedPropertiesFileARCHI As String()
    Dim mappedRelationsFileARCHI As New Hashtable
    Dim columsMappedrelationsFileARCHI As String()
    Dim appConfig As AppConfig

    Sub Main(ByVal sArgs As String())
        TimeStampD = DateTime.Now
        TimeStampS = TimeStampD.ToString("o")
        Try
            'inits app by command line parameters
            InitApp(sArgs)
        Catch ex As Exception
            lLOG.Fatal("Fatal Error occured. Can not continue due to: " + ex.Message)
            CloseApp()
            Exit Sub
        End Try
        'gets the Sparx EA application reference
        EAapp = GetApp()
        If IsNothing(EAapp) Then
            lLOG.Fatal("Sparx EA cannot start")
            CloseApp()
            Exit Sub
        End If
        ' ... and the proper repository
        Repository = GetRepository(EAapp)
        If IsNothing(Repository) Then
            lLOG.Fatal("Sparx EA has to have opened any repository")
            CloseApp()
            Exit Sub
        End If
        'Call ConnectorTest(Repository)
        ' ... and the proper model
        Model = GetModel(Repository)
        If IsNothing(Model) Then
            lLOG.Fatal("Sparx EA has to have model with given name: " + My.Settings.SparxEATargetRepostoryModelArchiImported)
            CloseApp()
            Exit Sub
        End If
        ' ... and the proper package
        Package = GetPackageFromModel(Model)
        If IsNothing(Package) Then
            lLOG.Fatal("Sparx EA has to have repository with given name: " + My.Settings.SparxEATargetRepostoryPackageArchiImported)
            CloseApp()
            Exit Sub
        End If

        'read and map properties from ARCHI export
        mappedPropertiesFileARCHI = LoadPropertiesFileARCHI()
        'read and map relations from ARCHI export
        mappedRelationsFileARCHI = LoadRelationsFileARCHI()

        'read and map elements from ARCHI export
        'has to be last; after properties and relations!
        mappedElementsFileARCHI = LoadElementsFileARCHI()
        columsMappedElementsFileARCHI = ArchiElement.GetFieldNamesCSV
        columsMappedPropertiesFileARCHI = ArchiProperty.GetFieldNamesCSV
        columsMappedrelationsFileARCHI = ArchiRelation.GetFieldNamesCSV
        CreateElementsInEA(Package, mappedElementsFileARCHI, mappedPropertiesFileARCHI)
        CreateRelationsInEA(Repository, mappedRelationsFileARCHI, mappedElementsFileARCHI, mappedPropertiesFileARCHI)
        CreateDiagram(Repository, Package, "This is testing name at " & TimeStampS, "This is testing notes at " & TimeStampS)
        SaveElementsFileARCHI(columsMappedElementsFileARCHI, mappedElementsFileARCHI)
        SaveRelationsFileARCHI(columsMappedrelationsFileARCHI, mappedRelationsFileARCHI)
        SavePropertiesFileARCHI(columsMappedPropertiesFileARCHI, mappedPropertiesFileARCHI)
        'TODO: saveRelationshipsFileARCHI
        'TODO: savePropertiesFileARCHI
        'finishing the system
        CloseApp()
    End Sub
    Sub CreateRelationsInEA(ByRef repository As EA.Repository, ByRef archiRelations As Hashtable, ByRef archiElements As Hashtable, ByRef archiProperties As Hashtable)
        Dim connectorEA As EA.Connector
        Dim client As EA.Element
        Dim supplier As EA.Element
        Dim relationArchi As ArchiRelation
        Dim sourceArchi, targetArchi As ArchiElement
        Dim spin As ConsoleSpiner
        Dim listMsgError As New ArrayList()
        Dim listMsgDebug As New ArrayList()
        Dim msg As String

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
                listMsgError.Add("For relation " + relationArchi.ID + "there is no source element " + relationArchi.Source + " in import files")
            Else
                supplier = repository.GetElementByID(sourceArchi.FK2)
                If IsNothing(targetArchi) Then
                    listMsgError.Add("For relation " + relationArchi.ID + "there is no target element " + relationArchi.Target + " in import files")
                Else
                    client = repository.GetElementByID(archiElements(relationArchi.Target).FK2)
                    stereotype = EAConstants.typeArchi2StereotypeEA(relationArchi.Type.Substring(0, Len(relationArchi.Type) - Len(ArchiConstants.RelationSuffix)))
                    type = EAConstants.stereotype2type(stereotype)

                    connectorEA = supplier.Connectors.AddNew(relationArchi.Name, type)
                    connectorEA.SupplierID = client.ElementID
                    If Not connectorEA.Update() Then
                        listMsgError.Add("Connector with ARCHI ID " + relationArchi.ID + " not created: " + connectorEA.GetLastError)
                    Else
                        With connectorEA
                            'TODO: for Archimate_Composition.    
                            '.SourceIsAggregate=2
                            If stereotype = (EAConstants.stereotypeArchimatePrefix & Archimate3.typeRelationComposition) Then
                                Dim i As Integer
                                i = .CustomProperties.Count
                                i = .Properties.Count
                                .Direction = EAConstants.connectorDirectionUnspecified
                            Else
                                .Direction = EAConstants.connectorDirectionSourceDestination
                            End If
                            .Stereotype = EAConstants.metatypeArchimatePrefix & stereotype
                            .Notes = relationArchi.Documentation
                            'add Tagged Values
                            msg = _addTaggedValuesConnector(connectorEA, relationArchi, archiProperties)
                            If Not IsNothing(msg) Then listMsgDebug.Add(msg)
                            'store EA identifiers into elementArchi
                            relationArchi.FK = .ConnectorGUID
                            relationArchi.FK2 = .ConnectorID
                            .Update()
                            .TaggedValues.Refresh()
                        End With
                        client.Connectors.Refresh()
                    End If
                End If
            End If
        Next key
        spin.Finish()
        If Not IsNothing(listMsgDebug) Then PopulateMessageArray(listMsgDebug, Core.Level.Debug)
        If Not IsNothing(listMsgError) Then PopulateMessageArray(listMsgError, Core.Level.Error)
        lLOG.Info("createRelationsInEA finished")
    End Sub
    Sub CreateElementsInEA(ByRef package As EA.Package, ByRef archiElements As Hashtable, ByRef archiProperties As Hashtable)
        Dim elementEA As EA.Element
        Dim elementArchi As ArchiElement
        Dim listDebugMsg As New ArrayList()
        Dim msg As String
        Dim spin As ConsoleSpiner

        Dim stereotype As String
        Dim type As String
        Dim key As String
        Dim keys As Collections.ICollection
        'Dim properties As ArrayList

        lLOG.Info("createElementsInEA started")
        keys = archiElements.Keys
        spin = New ConsoleSpiner(keys.Count, 1)
        For Each key In keys
            spin.Turn()
            elementArchi = archiElements(key)
            stereotype = EAConstants.typeArchi2StereotypeEA(elementArchi.Type)
            If elementArchi.Type.Equals(ArchiConstants.typeModel) Then
                'nothing. Model root
                'TODO: work with such model in EA also? Maybe create/reuse such model also in EA?
            Else
                type = EAConstants.stereotype2type(stereotype)
                elementEA = package.Elements.AddNew(elementArchi.Name, type)
                With elementEA
                    .Stereotype = EAConstants.metatypeArchimatePrefix & stereotype 'prefix is necessary to ensure, that Profile Archimate3 is used!
                    .Author = My.Settings.Author
                    .Notes = elementArchi.Documentation
                    'add Tagged Values
                    msg = _addTaggedValues(elementEA, elementArchi, archiProperties)
                    If Not IsNothing(msg) Then listDebugMsg.Add(msg)
                    'store EA identifiers into elementArchi
                    elementArchi.FK = .ElementGUID
                    elementArchi.FK2 = .ElementID
                    .TaggedValues.Refresh()
                    .Update()
                End With
            End If
        Next key
        spin.Finish()
        If Not IsNothing(listDebugMsg) Then PopulateMessageArray(listDebugMsg, Core.Level.Debug)
        lLOG.Info("Package is being refreshed")
        package.Elements.Refresh()
        lLOG.Info("Package is refreshed")
        lLOG.Info("Package is being updated. It will take a while ...")
        package.Update()
        lLOG.Info("Package is updated")
        lLOG.Info("createElementsInEA finished")
    End Sub
    'TODO: Tagged Values for Connector should be treated separatelly because some error in EA automation interface
    'Type(TagValue) for Connector is Object and not EA.TaggedValue
    Function _addTaggedValuesConnector(ByRef connectorEA As EA.Connector, ByRef relationArchi As ArchiRelation, ByRef archiProperties As Hashtable) As String
        Dim properties As ArrayList
        Dim taggedValue As Object 'Not EA.TaggedValue because some error in EA automation interface
        Dim connectorProperty As ArchiProperty
        Dim archiPropertyArray As ArrayList

        Dim msg As String = Nothing

        properties = archiProperties(relationArchi.ID)
        If Not IsNothing(properties) Then
            For Each connectorProperty In properties
                taggedValue = connectorEA.TaggedValues.AddNew(connectorProperty.Key, connectorProperty.Value)
                If Not taggedValue.Update() Then
                    'TODO: should be in string array and returned as ususally. But currently function returns only one string
                    'in case of reach the log console output will be slightly corrupted. Nothing else.
                    lLOG.Error("Tagged Value with Archi ID " + connectorProperty.ID + " not created: " + (taggedValue.GetLastError))
                Else
                    If Not taggedValue.Update() Then
                        'TODO: should be in string array and returned as ususally. But currently function returns only one string
                        'in case of reach the log console output will be slightly corrupted. Nothing else.
                        lLOG.Error("Tagged Value with Archi ID " + connectorProperty.ID + " not created: " + (taggedValue.GetLastError))
                    End If
                    'xreference
                    connectorProperty.FK = taggedValue.TagGUID
                    connectorProperty.FK2 = taggedValue.TagID
                End If
            Next connectorProperty
        Else
            msg = "Element does not have any property. So, no Tag_Value was created for archi element: " + relationArchi.ID + ":" + relationArchi.Type + ":" + relationArchi.Name
        End If
        'add reference to ARCHI model
        taggedValue = connectorEA.TaggedValues.AddNew(ArchiConstants.taggedValueArchiID, relationArchi.ID)
        If Not taggedValue.Update() Then
            lLOG.Error("xreference tagged value for archi connector: " + relationArchi.ToString + " in Sparx EA not created due to: " + taggedValue.GetLastError)
        End If
        'addd reference to EA model. create corresponding property in archiProperties for future sync
        connectorProperty = New ArchiProperty("", relationArchi.ID, EAConstants.taggedValueEAID, connectorEA.ConnectorGUID, ArchiConstants.PropertyType.propertyTypeForeignKey)
        archiPropertyArray = archiProperties.Item(relationArchi.ID)
        archiPropertyArray.Add(connectorProperty)
        Return msg
    End Function

    Function _addTaggedValues(ByRef elementEA As Object, ByRef elementArchi As Object, ByRef archiProperties As Hashtable) As String
        Dim properties As ArrayList
        Dim taggedValue As EA.TaggedValue
        Dim elementProperty As ArchiProperty
        Dim archiPropertyArray As ArrayList

        Dim msg As String = Nothing

        properties = archiProperties(elementArchi.ID)
        If Not IsNothing(properties) Then
            For Each elementProperty In properties
                taggedValue = elementEA.TaggedValues.AddNew(elementProperty.Key, elementProperty.Value)
                If Not taggedValue.Update() Then
                    'TODO: should be in string array and returned as ususally. But currently function returns only one string
                    'in case of reach the log console output will be slightly corrupted. Nothing else.
                    lLOG.Error("Tagged Value with Archi ID " + elementProperty.ID + " not created: " + (taggedValue.GetLastError))
                Else
                    If Not taggedValue.Update() Then
                        'TODO: should be in string array and returned as ususally. But currently function returns only one string
                        'in case of reach the log console output will be slightly corrupted. Nothing else.
                        lLOG.Error("Tagged Value with Archi ID " + elementProperty.ID + " not created: " + (taggedValue.GetLastError))
                        'store Sparx EA IDs into Archi property
                    End If
                    'xreference
                    elementProperty.FK = taggedValue.PropertyGUID
                    elementProperty.FK2 = taggedValue.PropertyID
                End If
            Next
        Else
            msg = "Element does not have any property. So, no Tag_Value was created for archi element: " + elementArchi.ID + ":" + elementArchi.Type + ":" + elementArchi.Name
        End If
        'add reference to ARCHI model
        taggedValue = elementEA.TaggedValues.AddNew(ArchiConstants.taggedValueArchiID, elementArchi.ID)
        If Not taggedValue.Update() Then
            lLOG.Error("xreference tagged value for archi element: " + elementArchi.ToString + " in Sparx EA not created due to: " + taggedValue.GetLastError)
        End If
        'addd reference to EA model. create corresponding property in archiProperties for future sync
        elementProperty = New ArchiProperty("", elementArchi.ID, EAConstants.taggedValueEAID, elementEA.ElementGUID, ArchiConstants.PropertyType.propertyTypeForeignKey)
        archiPropertyArray = archiProperties.Item(elementArchi.ID)
        archiPropertyArray.Add(elementProperty)
        Return msg
    End Function
    Sub InitApp(ByRef sArgs As String())
        appConfig = New AppConfig(sArgs)
    End Sub
    Sub CloseApp()
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

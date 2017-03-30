Module ArchiCommon
    Private ignoreRow As Boolean = True 'import file from Archi contains names of attributes as first row. I do not need them ...
    Private objFSO, objFile As Object
    Private strLine As String


    Function LoadElementsFileARCHI() As Hashtable
        Dim elementsArrayArchi As String()
        Dim archiElmt As ArchiElement
        Dim mappedElementsFileARCHI As New Hashtable

        ignoreRow = True
        lLOG.Info("loadElementsFileARCHI started")
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileElements, 1)
        'the first line contains names of columns
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            elementsArrayArchi = Split(Replace(strLine, ArchiConstants.csvQualifier, ""), ArchiConstants.csvDelimiter)
            If ignoreRow Then
                ignoreRow = False
                If Not AreArraysEqual(Of String)(elementsArrayArchi, ArchiElement.GetFieldNamesCSV) Then
                    Throw New ToolBoxException("Expected input format: " & ArchiElement.GetFieldNamesCSV.ToString() & " differs from input file format: " & elementsArrayArchi.ToString())
                    'lLOG.Fatal("Expected input format: " & ArchiElement.GetFieldNamesCSV.ToString() & " differs from input file format: " & elementsArrayArchi.ToString())
                End If
            Else
                If elementsArrayArchi(1) = ArchiConstants.typeModel Then
                    'do nothing
                Else
                    archiElmt = New ArchiElement(elementsArrayArchi(0), elementsArrayArchi(1), elementsArrayArchi(2), elementsArrayArchi(3))
                    mappedElementsFileARCHI.Add(elementsArrayArchi(0), archiElmt)
                End If
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedElementsFileARCHI.Count & " Elements have been read")

        'return
        LoadElementsFileARCHI = mappedElementsFileARCHI
        lLOG.Info("loadElementsFileARCHI finished")
    End Function

    Function LoadPropertiesFileARCHI() As Hashtable
        Dim propertiesArrayArchi As String()
        Dim ArchiPrpt As ArchiProperty
        Dim mappedPropertiesFileARCHI As New Hashtable
        Dim archiPropertyArray As ArrayList
        Dim i = 0

        lLOG.Info("loadPropertiesFileARCHI started")
        ignoreRow = True
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileProperties, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            propertiesArrayArchi = Split(Replace(strLine, ArchiConstants.csvQualifier, ""), ArchiConstants.csvDelimiter)
            'column names
            If ignoreRow Then
                ignoreRow = False
                If Not AreArraysEqual(Of String)(propertiesArrayArchi, ArchiProperty.GetFieldNamesCSV) Then
                    Throw New ToolBoxException("Expected input format: " & ArchiProperty.GetFieldNamesCSV.ToString() & " differs from input file format: " & propertiesArrayArchi.ToString())
                    'lLOG.Fatal("Expected input format: " & ArchiProperty.GetFieldNamesCSV.ToString() & " differs from input file format: " & propertiesArrayArchi.ToString())
                End If
            Else
                i = i + 1
                ' In ARCHI there is no ID for property. Only reference to owner/item
                ArchiPrpt = New ArchiProperty("", propertiesArrayArchi(0), propertiesArrayArchi(1), propertiesArrayArchi(2), ArchiConstants.PropertyType.propertyTypeOriginal)
                'in case of multiple properties per element
                If mappedPropertiesFileARCHI.ContainsKey(ArchiPrpt.Item) Then
                    'get array of properties for certain element - it is ID of property ...
                    archiPropertyArray = mappedPropertiesFileARCHI.Item(ArchiPrpt.Item)
                Else
                    'no array of properties found
                    archiPropertyArray = New ArrayList()
                    mappedPropertiesFileARCHI.Add(propertiesArrayArchi(0), archiPropertyArray)
                End If
                'add next property
                archiPropertyArray.Add(ArchiPrpt)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedPropertiesFileARCHI.Count & " Elements have Properties and they have been read. And also " & i & " Properties have been read")
        LoadPropertiesFileARCHI = mappedPropertiesFileARCHI
        lLOG.Info("loadPropertiesFileARCHI finished")
    End Function

    Function LoadRelationsFileARCHI() As Hashtable
        Dim relationsArrayArchi As String()
        Dim archiRlt As ArchiRelation
        Dim mappedRelationsFileARCHI As New Hashtable

        lLOG.Info("loadRelationsFileARCHI started")
        ignoreRow = True
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileRelations, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            relationsArrayArchi = Split(Replace(strLine, ArchiConstants.csvQualifier, ""), ArchiConstants.csvDelimiter)
            If ignoreRow Then
                ignoreRow = False
                If Not AreArraysEqual(Of String)(relationsArrayArchi, ArchiRelation.GetFieldNamesCSV) Then
                    'lLOG.Fatal("Expected input format: " & ArchiRelation.GetFieldNamesCSV.ToString() & " differs from input file format: " & relationsArrayArchi.ToString())
                    Throw New ToolBoxException("Expected input format: " & ArchiRelation.GetFieldNamesCSV.ToString() & " differs from input file format: " & relationsArrayArchi.ToString())
                End If
            Else
                archiRlt = New ArchiRelation(relationsArrayArchi(0), relationsArrayArchi(1), relationsArrayArchi(2), relationsArrayArchi(3), relationsArrayArchi(4), relationsArrayArchi(5))
                mappedRelationsFileARCHI.Add(relationsArrayArchi(0), archiRlt)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedRelationsFileARCHI.Count & " Relations have been read")
        LoadRelationsFileARCHI = mappedRelationsFileARCHI
        lLOG.Info("loadRelationsFileARCHI finished")
    End Function
    'TODO: include somehow --into properties ??? -- IDs from EA also!
    'TODO: Ignore add record for model. Is it bad approach?
    Function SaveElementsFileARCHI(ByRef columsMappedElementsFileARCHI As String(), ByRef archiElements As Hashtable) As String
        Dim msg As String = Nothing
        Dim key As String
        Dim spin As ConsoleSpiner
        Dim keys As ICollection
        Dim elementArchi As ArchiElement
        Dim stLine As Text.StringBuilder

        'TODO: try catch
        Dim objWriter As IO.StreamWriter = IO.File.CreateText(My.Settings.ArchiImportDirectory & My.Settings.ArchiExportFilePrefix & My.Settings.ArchiImportFileElements)

        lLOG.Info("saveElementsFileARCHI started")
        keys = archiElements.Keys
        spin = New ConsoleSpiner(keys.Count, 1)
        'columns names
        stLine = _mappedCSVRow(columsMappedElementsFileARCHI)
        objWriter.Write(stLine.ToString)
        objWriter.Write(Environment.NewLine)
        For Each key In keys
            spin.Turn()
            elementArchi = archiElements(key)
            stLine.Clear()
            'TODO:the same order as in input parameter in this function?
            stLine = _mappedCSVRow(elementArchi.toStringArray)
            objWriter.Write(stLine.ToString)
            'TODO: If value contains comma in the value then you have to perform this opertions
            'Dim append = If(_Msg.Contains(","), String.Format("""{0}""", _Msg), _Msg)
            'stLine = String.Format("{0}{1},", stLine, append)
            objWriter.Write(Environment.NewLine)
        Next key
        objWriter.Close()
        stLine = Nothing
        spin.Finish()
        lLOG.Info("saveElementsFileARCHI finished")
        Return msg
    End Function
    Function SaveRelationsFileARCHI(ByRef columsMappedRelationsFileARCHI As String(), ByRef archiRelations As Hashtable)
        lLOG.Info("saveRelationsFileARCHI started")
        lLOG.Info("saveRelationsFileARCHI finished")
    End Function
    Function SavePropertiesFileARCHI(ByRef columsMappedPropertiesFileARCHI As String(), ByRef archiProperties As Hashtable)
        lLOG.Info("savePropertiesFileARCHI started")
        lLOG.Info("savePropertiesFileARCHI finished")
    End Function
    Private Function _mappedCSVRowArray(columns As String()) As String()
        Dim stl(columns.Length) As String
        Dim idx As Byte = 0

        For Each s As String In columns
            stl(idx) = (ArchiConstants.csvQualifier) & (s) & (ArchiConstants.csvQualifier)
            idx = idx + 1
        Next s
        Return stl
    End Function

    Private Function _mappedCSVRow(columns As String()) As Text.StringBuilder
        Dim stl As New Text.StringBuilder
        Dim idx As Byte = 1

        For Each s As String In columns
            stl.Append(ArchiConstants.csvQualifier).Append(s).Append(ArchiConstants.csvQualifier)
            If idx < columns.Length Then stl.Append(ArchiConstants.csvDelimiter)
            idx = idx + 1
        Next s
        Return stl
    End Function
End Module

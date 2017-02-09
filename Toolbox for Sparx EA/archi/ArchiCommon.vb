Module ArchiCommon
    Private ignoreRow As Boolean = True 'import file from Archi contains names of attributes. I do not need them ...
    Private objFSO, objFile As Object
    Private strLine As String


    Function loadElementsFileARCHI() As Hashtable
        Dim elementsArrayArchi As String()
        Dim archiElement As ArchiElement
        Dim mappedElementsFileARCHI As New Hashtable

        lLOG.Info("loadElementsFileARCHI started")
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileElements, 1)
        'the first line contains names of columns
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            elementsArrayArchi = Split(Replace(strLine, """", ""), ",")
            archiElement = New ArchiElement(elementsArrayArchi(0), elementsArrayArchi(1), elementsArrayArchi(2), elementsArrayArchi(3))
            mappedElementsFileARCHI.Add(elementsArrayArchi(0), archiElement)
        Loop
        objFile.Close
        lLOG.Info(mappedElementsFileARCHI.Count & " Elements have been read")

        'return
        loadElementsFileARCHI = mappedElementsFileARCHI
        lLOG.Info("loadElementsFileARCHI finished")
    End Function

    Function loadPropertiesFileARCHI() As Hashtable
        Dim propertiesArrayArchi As String()
        Dim archiProperty As ArchiProperty
        Dim mappedPropertiesFileARCHI As New Hashtable
        Dim archiPropertyArray As ArrayList
        Dim i = 0

        lLOG.Info("loadPropertiesFileARCHI started")
        ignoreRow = True
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileProperties, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                i = i + 1
                propertiesArrayArchi = Split(Replace(strLine, """", ""), ",")
                ArchiProperty = New ArchiProperty(propertiesArrayArchi(0), propertiesArrayArchi(1), propertiesArrayArchi(2))
                'in case of multiple properties per element
                If mappedPropertiesFileARCHI.ContainsKey(ArchiProperty.ID) Then
                    'get array of properties for certain element - it is ID of property ...
                    archiPropertyArray = mappedPropertiesFileARCHI.Item(ArchiProperty.ID)
                Else
                    'no array of properties found
                    archiPropertyArray = New ArrayList()
                    mappedPropertiesFileARCHI.Add(propertiesArrayArchi(0), archiPropertyArray)
                End If
                'add next property
                archiPropertyArray.Add(ArchiProperty)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedPropertiesFileARCHI.Count & " Elements have Properties and they have been read. And also " & i & " Properties have been read")
        loadPropertiesFileARCHI = mappedPropertiesFileARCHI
        lLOG.Info("loadPropertiesFileARCHI finished")
    End Function

    Function loadRelationsFileARCHI() As Hashtable
        Dim relationsArrayArchi As String()
        Dim archiRelation As ArchiRelation
        Dim mappedRelationsFileARCHI As New Hashtable

        lLOG.Info("loadRelationsFileARCHI started")
        ignoreRow = True
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileRelations, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                relationsArrayArchi = Split(Replace(strLine, """", ""), ",")
                ArchiRelation = New ArchiRelation(relationsArrayArchi(0), relationsArrayArchi(1), relationsArrayArchi(2), relationsArrayArchi(3), relationsArrayArchi(4), relationsArrayArchi(5))
                mappedRelationsFileARCHI.Add(relationsArrayArchi(0), ArchiRelation)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedRelationsFileARCHI.Count & " Relations have been read")
        loadRelationsFileARCHI = mappedRelationsFileARCHI
        lLOG.Info("loadRelationsFileARCHI finished")
    End Function
    Function saveElementsFileARCHI(ByRef columsMappedElementsFileARCHI As String(), ByRef archiElements As Hashtable) As String
        Dim msg As String = Nothing
        Dim key As String
        Dim spin As ConsoleSpiner
        Dim keys As ICollection
        Dim elementArchi As ArchiElement
        Dim stLine As Text.StringBuilder

        'TODO: try catch
        'TODO: not constant file path and name
        Dim objWriter As IO.StreamWriter = IO.File.CreateText("c:\Users\david.skarka\Documents\Priv\MTU\test.csv")

        lLOG.Info("saveElementsFileARCHI started")
        keys = archiElements.Keys
        spin = New ConsoleSpiner(keys.Count, 1)
        'columns names
        stLine = _mappedScvRow(columsMappedElementsFileARCHI)
        objWriter.Write(stLine.ToString)
        objWriter.Write(Environment.NewLine)
        For Each key In keys
            spin.Turn()
            elementArchi = archiElements(key)
            stLine.Clear()
            stLine = _mappedScvRow(elementArchi.toStringArray)
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

    Private Function _mappedScvRow(columns As String()) As Text.StringBuilder
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

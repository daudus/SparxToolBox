Module ArchiCommon
    Private ignoreRow As Boolean = True 'import file from Archi contains names of attributes. I do not need them ...
    Private objFSO, objFile As Object
    Private strLine As String


    Function loadElementsFileARCHI() As Hashtable
        Dim elementsArrayArchi As String()
        Dim archiElement As ArchiElement
        Dim mappedElementsFileARCHI As New Hashtable

        ignoreRow = True
        If IsNothing(objFSO) Then objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileElements, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                elementsArrayArchi = Split(Replace(strLine, """", ""), ",")
                ArchiElement = New ArchiElement(elementsArrayArchi(0), elementsArrayArchi(1), elementsArrayArchi(2), elementsArrayArchi(3))
                mappedElementsFileARCHI.Add(elementsArrayArchi(0), ArchiElement)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedElementsFileARCHI.Count & " Elements have been read")

        'return
        loadElementsFileARCHI = mappedElementsFileARCHI
    End Function

    Function loadPropertiesFileARCHI() As Hashtable
        Dim propertiesArrayArchi As String()
        Dim archiProperty As ArchiProperty
        Dim mappedPropertiesFileARCHI As New Hashtable
        Dim archiPropertyArray As ArrayList
        Dim i = 0

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
    End Function

    Function loadRelationsFileARCHI() As Hashtable
        Dim relationsArrayArchi As String()
        Dim archiRelation As ArchiRelation
        Dim mappedRelationsFileARCHI As New Hashtable

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
    End Function
End Module

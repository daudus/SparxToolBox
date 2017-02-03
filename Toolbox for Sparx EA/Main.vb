Option Explicit On

Imports EA
Imports System.Collections.Specialized

<Assembly: log4net.Config.XmlConfigurator(ConfigFile:="log4net.xml", Watch:=True)>

Module Main
    Dim Repository As EA.Repository
    Dim EAapp, objFSO, objFile As Object
    Dim mappedElementsFileARCHI As New Hashtable
    Dim mappedPropertiesFileARCHI As New Hashtable
    Dim mappedRelationsFileARCHI As New Hashtable

    Sub Main(ByVal sArgs As String())
        Dim strLine As String
        Dim elementsArrayArchi As String()
        Dim propertiesArrayArchi As String()
        Dim relationsArrayArchi As String()
        Dim archiElement As ArchiElement
        Dim archiRelation As ArchiRelation
        Dim archiProperty As ArchiProperty
        Dim archiPropertyArray As ArrayList
        Dim ignoreRow As Boolean = True 'import file from Archi contains names of attributes. I do not need them ...
        Dim i As Integer

        initApp(sArgs)

        EAapp = getApp()
        Repository = getRepository(EAapp)
        If IsNothing(Repository) Then
            lLOG.Fatal("Sparx EA has to have opened any repository")
            Exit Sub
        End If
        'read and map elements from ARCHI export
        ignoreRow = True
        objFSO = CreateObject("Scripting.FileSystemObject")
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileElements, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                elementsArrayArchi = Split(Replace(strLine, """", ""), ",")
                archiElement = New ArchiElement(elementsArrayArchi(0), elementsArrayArchi(1), elementsArrayArchi(2), elementsArrayArchi(3))
                mappedElementsFileARCHI.Add(elementsArrayArchi(0), archiElement)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedElementsFileARCHI.Count & " Elements have been read")

        'read and map properties from ARCHI export
        ignoreRow = True
        i = 0
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileProperties, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                i = i + 1
                propertiesArrayArchi = Split(Replace(strLine, """", ""), ",")
                archiProperty = New ArchiProperty(propertiesArrayArchi(0), propertiesArrayArchi(1), propertiesArrayArchi(2))
                'in case of multiple properties per element
                If mappedPropertiesFileARCHI.ContainsKey(archiProperty.ID) Then
                    'get array of properties for certain element - it is ID of property ...
                    archiPropertyArray = mappedPropertiesFileARCHI.Item(archiProperty.ID)
                Else
                    'no array of properties found
                    archiPropertyArray = New ArrayList()
                    mappedPropertiesFileARCHI.Add(propertiesArrayArchi(0), archiPropertyArray)
                End If
                'add next property
                archiPropertyArray.Add(archiProperty)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedPropertiesFileARCHI.Count & " Elements have Properties and they have been read. And also " & i & " Properties have been read")
        'read and map relations from ARCHI export
        ignoreRow = True
        objFile = objFSO.OpenTextFile(My.Settings.ArchiImportDirectory & My.Settings.ArchiImportFileRelations, 1)
        Do While Not objFile.AtEndOfStream
            strLine = objFile.readline
            If ignoreRow Then
                ignoreRow = False
            Else
                relationsArrayArchi = Split(Replace(strLine, """", ""), ",")
                archiRelation = New ArchiRelation(relationsArrayArchi(0), relationsArrayArchi(1), relationsArrayArchi(2), relationsArrayArchi(3), relationsArrayArchi(4), relationsArrayArchi(5))
                mappedRelationsFileARCHI.Add(relationsArrayArchi(0), archiRelation)
            End If
        Loop
        objFile.Close
        lLOG.Info(mappedRelationsFileARCHI.Count & " Relations have been read")
        close(EAapp, Repository, False)
    End Sub

    Sub initApp(ByRef sArgs As String())
        If sArgs.Length = 0 Then                'If there are no arguments
            lLOG.Info("<-no arguments passed->")      'Just output Hello World
        Else                                    'We have some arguments 
            Dim j As Integer = 0

            While j < sArgs.Length             'So with each argument
                lLOG.Info("param: " & sArgs(j) + " and its value: " + sArgs(j + 1))       'Print out each item
                j = j + 2                       'Increment to the next argument
            End While
        End If
    End Sub
End Module

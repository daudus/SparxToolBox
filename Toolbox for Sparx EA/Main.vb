Option Explicit On

Imports EA
Imports System.Collections.Specialized

<Assembly: log4net.Config.XmlConfigurator(ConfigFile:="log4net.xml", Watch:=True)>

Module Main
    Dim Repository As EA.Repository
    Dim EAapp As Object
    Dim mappedElementsFileARCHI As New Hashtable
    Dim mappedPropertiesFileARCHI As New Hashtable
    Dim mappedRelationsFileARCHI As New Hashtable

    Sub Main(ByVal sArgs As String())

        'inits app by command line parameters
        initApp(sArgs)
        'gets the Sparx EA application reference
        EAapp = getApp()
        ' ... and the proper repository
        Repository = getRepository(EAapp)
        If IsNothing(Repository) Then
            lLOG.Fatal("Sparx EA has to have opened any repository")
            Exit Sub
        End If

        'read and map elements from ARCHI export
        mappedElementsFileARCHI = loadElementsFileARCHI()
        'read and map properties from ARCHI export
        mappedPropertiesFileARCHI = loadPropertiesFileARCHI()
        'read and map relations from ARCHI export
        mappedRelationsFileARCHI = loadRelationsFileARCHI()

        'finishing the system
        closeApp()
    End Sub

    Sub initApp(ByRef sArgs As String())
    End Sub
    Sub closeApp()
        mappedElementsFileARCHI.Clear()
        mappedElementsFileARCHI = Nothing
        mappedPropertiesFileARCHI.Clear()
        mappedPropertiesFileARCHI = Nothing
        mappedRelationsFileARCHI.Clear()
        mappedRelationsFileARCHI = Nothing
        close(EAapp, Repository, False)
    End Sub
End Module

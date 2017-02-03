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

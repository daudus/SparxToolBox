Public Class ArchiElement
    Inherits Element
    Private Shared ReadOnly ArchiElementCSVFieldNames As String() = {"ID", "Type", "Name", "Documentation"}

    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String)
        MyBase.New(ID, Type, Name, Documentation)
    End Sub

    Public Overrides Function ToStringArrayCSV() As String()
        Dim s() As String = {ID, Type, Name, Documentation}
        Return s
    End Function
    Public Shared Shadows Function GetFieldNamesCSV() As String()
        Return ArchiElementCSVFieldNames
    End Function
End Class


Public Class ArchiRelation
    Inherits Relation

    Private Shared ReadOnly ArchiRelationCSVFieldNames As String() = {"ID", "Type", "Name", "Documentation", "Source", "Target"}

    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String, ByVal Source As String, ByVal Target As String)
        MyBase.New(ID, Type, Name, Documentation, Source, Target)
    End Sub

    Public Overrides Function ToStringArrayCSV() As String()
        Dim s() As String = {ID, Type, Name, Documentation, Source, Target}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNamesCSV() As String()
        Return ArchiRelationCSVFieldNames
    End Function
End Class

Public Class ArchiProperty
    Inherits Tag

    Private Shared ReadOnly ArchiTagCSVFieldNames As String() = {"ID", "Key", "Value"}

    Sub New(ByVal ID As String, ByVal Item As String, ByVal Key As String, ByVal Value As String, ByVal SystType As String)
        MyBase.New(ID, Item, Key, Value, SystType)
    End Sub

    Public Overrides Function ToStringArrayCSV() As String()
        Dim s() As String = {ID, Key, Value}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNamesCSV() As String()
        Return ArchiTagCSVFieldNames
    End Function
End Class

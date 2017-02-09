Public Class ArchiElement
    Private pID As String
    Private pType As String
    Private pName As String
    Private pDocumentation As String
    Private pGUIDEA As String
    Private pElementIDEA As Integer

    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String)
        pID = ID
        pType = Type
        pName = Name
        pDocumentation = Documentation
    End Sub

    Public Property ID() As String
        Get
            Return pID
        End Get
        Set(ByVal value As String)
            pID = value
        End Set
    End Property

    Public Property Type() As String
        Get
            Return pType
        End Get
        Set(ByVal value As String)
            pType = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Property Documentation() As String
        Get
            Return pDocumentation
        End Get
        Set(ByVal value As String)
            pDocumentation = value
        End Set
    End Property

    Public Property GUIDEA() As String
        Get
            Return pGUIDEA
        End Get
        Set(ByVal value As String)
            pGUIDEA = value
        End Set
    End Property

    Public Property ElementIDEA() As String
        Get
            Return pElementIDEA
        End Get
        Set(ByVal value As String)
            pElementIDEA = value
        End Set
    End Property
    Public Function toStringArray() As String()
        Dim s() As String = {ID, Type, Name, Documentation}
        Return s
    End Function
End Class
Public Class ArchiRelation
    Private pID As String
    Private pType As String
    Private pName As String
    Private pDocumentation As String
    Private pSource As String
    Private pTarget As String
    Private pGUIDEA As String
    Private pRelationIDEA As Integer

    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String, ByVal Source As String, ByVal Target As String)
        pID = ID
        pType = Type
        pName = Name
        pDocumentation = Documentation
        pSource = Source
        pTarget = Target
    End Sub

    Public Property ID() As String
        Get
            Return pID
        End Get
        Set(ByVal value As String)
            pID = value
        End Set
    End Property

    Public Property Type() As String
        Get
            Return pType
        End Get
        Set(ByVal value As String)
            pType = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Property Documentation() As String
        Get
            Return pDocumentation
        End Get
        Set(ByVal value As String)
            pDocumentation = value
        End Set
    End Property

    Public Property Source() As String
        Get
            Return pSource
        End Get
        Set(ByVal value As String)
            pSource = value
        End Set
    End Property

    Public Property Target() As String
        Get
            Return pTarget
        End Get
        Set(ByVal value As String)
            pTarget = value
        End Set
    End Property
    Public Property GUIDEA() As String
        Get
            Return pGUIDEA
        End Get
        Set(ByVal value As String)
            pGUIDEA = value
        End Set
    End Property
    Public Property RelationIDEA() As String
        Get
            Return pRelationIDEA
        End Get
        Set(ByVal value As String)
            pRelationIDEA = value
        End Set
    End Property
End Class
Public Class ArchiProperty
    Private pID As String
    Private pKey As String
    Private pValue As String
    Private pGUIDEA As String
    Private pTagValueIDEA As Integer


    Sub New(ByVal ID As String, ByVal Key As String, ByVal Value As String)
        pID = ID
        pKey = Key
        pValue = Value
    End Sub

    Public Property ID() As String
        Get
            Return pID
        End Get
        Set(ByVal value As String)
            pID = value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return pKey
        End Get
        Set(ByVal value As String)
            pKey = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return pValue
        End Get
        Set(ByVal value As String)
            pValue = value
        End Set
    End Property

    Public Property GUIDEA() As String
        Get
            Return pGUIDEA
        End Get
        Set(ByVal value As String)
            pGUIDEA = value
        End Set
    End Property

    Public Property TagValueIDEA() As String
        Get
            Return pTagValueIDEA
        End Get
        Set(ByVal value As String)
            pTagValueIDEA = value
        End Set
    End Property
End Class

Public Class ArchiElement
    Private pID As String
    Private pType As String
    Private pName As String
    Private pDocumentation As String

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

End Class
Public Class ArchiRelation
    Private pID As String
    Private pType As String
    Private pName As String
    Private pDocumentation As String
    Private pSource As String
    Private pTarget As String

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
End Class
Public Class ArchiProperty
    Private pID As String
    Private pKey As String
    Private pValue As String

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
End Class

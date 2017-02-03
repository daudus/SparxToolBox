Class Gap
    Private pId As String
    Private pName As String
    Private pDescription As String
    Private pImpact As String
    Private pImpactedConcepts As HashSet(Of Concept)

    Sub New(ByVal Id As String, ByVal FullName As String, ByVal GapDescription As String)
        Dim firstColonPositionIndex
        If Len(FullName) > 0 Then
            pDescription = GapDescription
            pImpactedConcepts = New HashSet(Of Concept)
            firstColonPositionIndex = InStr(FullName, ":")
            If firstColonPositionIndex = 0 Then
                Call log("Gap <" + FullName + "> does not have impact or character "":"" is missing ", False)
                pName = FullName
            Else
                pImpact = Mid(FullName, 1, firstColonPositionIndex - 1)
                pName = Mid(FullName, firstColonPositionIndex + 1, Len(FullName))
            End If
        Else
            log("Gap does not have name ", False)
        End If
        pId = Id
    End Sub
    Public Property Id() As String
        Get
            Return pId
        End Get
        Set(ByVal value As String)
            pId = value
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
    Public Property Description() As String
        Get
            Return pDescription
        End Get
        Set(ByVal value As String)
            pDescription = value
        End Set
    End Property
    Public Property Impact() As String
        Get
            Return pImpact
        End Get
        Set(ByVal value As String)
            pImpact = value
        End Set
    End Property
    Public Property ImpactedConcepts() As HashSet(Of Concept)
        Get
            Return pImpactedConcepts
        End Get
        Set(ByVal value As HashSet(Of Concept))
            pImpactedConcepts = value
        End Set
    End Property
End Class

Class Concept
    Private pConceptDescription As String
    Private pConceptName As String
    Private pConceptType As String 'Service, Function, Interface

    Sub New(ByVal ConceptName As String, ByVal ConceptType As String, ByVal ConceptDescription As String, ByVal application As String)
        pConceptDescription = ConceptDescription
        pConceptName = ConceptName
        pConceptType = ConceptType
        pApplication = application
    End Sub
    Private pApplication As String
    Public Property Application() As String
        Get
            Return pApplication
        End Get
        Set(ByVal value As String)
            pApplication = value
        End Set
    End Property
    Public Property ConceptName()
        Get
            Return pConceptName
        End Get
        Set(ByVal value)
            pConceptName = value
        End Set
    End Property
    Public Property ConceptType()
        Get
            Return pConceptType
        End Get
        Set(ByVal value)
            pConceptType = value
        End Set
    End Property
    Public Property ConceptDescription()
        Get
            Return pConceptDescription
        End Get
        Set(ByVal value)
            pConceptDescription = value
        End Set
    End Property
End Class
Imports System.Runtime.Serialization

Public Class ConsoleSpiner
    Private pCounter As Integer
    Private pPercent100 As Long
    Private pStep As Decimal
    Private pMode As Byte = 0 ' 0...pseudographic, 1...number

    Public Sub New(percent100 As Long, mode As Byte)
        Me.New(percent100)
        pMode = System.Math.Abs(mode Mod 2)
    End Sub

    Public Sub New(percent100 As Long)
        Me.New()
        pPercent100 = percent100
        pStep = 100 / percent100
    End Sub

    Public Sub New()
        pCounter = 0
    End Sub
    Private Sub _turnNumber()
        Console.Write("{0,3:##0}%", pCounter * pStep)
        Console.Write(New String(ChrW(8), 4)) 'Backspace x 4  
    End Sub
    Private Sub _turnPseudoGraphic()
        Select Case (pCounter Mod 4)
            Case 0 : Console.Write(“/”)
            Case 1 : Console.Write(“-“)
            Case 2 : Console.Write(“\\”)
            Case 3 : Console.Write(“-“)
        End Select
        'Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop) generates exception in case of out of window
        Console.Write(New String(ChrW(8), 1))
    End Sub

    Public Sub Turn()
        pCounter = pCounter + 1
        Select Case pMode
            Case 0 'pseudographic
                _turnPseudoGraphic()
            Case 1 'number
                _turnNumber()
            Case Else 'combination
                _turnNumber()
        End Select
    End Sub
    Public Sub Finish()
        Console.WriteLine()
    End Sub
End Class

Public Class ToolBoxException
    Inherits Exception
    '
    ' Summary:
    '     Initializes a new instance of the ToolBoxException class.
    Public Sub New()
        MyBase.New()
    End Sub
    '
    ' Summary:
    '     Initializes a new instance of the ToolBoxException class with a specified
    '     error message.
    '
    ' Parameters:
    '   message:
    '     The message that describes the error.
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
    '
    ' Summary:
    '     Initializes a new instance of the ToolBoxException class with a specified
    '     error message and a reference to the inner exception that is the cause of this
    '     exception.
    '
    ' Parameters:
    '   message:
    '     The error message that explains the reason for the exception.
    '
    '   innerException:
    '     The exception that is the cause of the current exception. If the innerException
    '     parameter is not a null reference (Nothing in Visual Basic), the current exception
    '     is raised in a catch block that handles the inner exception.
    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub
    '
    ' Summary:
    '     Initializes a new instance of the ToolBoxException class with serialized
    '     data.
    '
    ' Parameters:
    '   info:
    '     The object that holds the serialized object data.
    '
    '   context:
    '     The contextual information about the source or destination.
    Protected Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class

Public MustInherit Class Modellable
    Implements IComparable

    '
    'Names and order for ToString method and default ToStringAray method
    Private Shared ReadOnly ModellableFieldNames As String() = {"ID", "FK", "FK2"}

    ReadOnly Property ID As String

    '
    'Reference to ID in other model. Helper property for Xreference
    Property FK As String
    Property FK2 As String

    Sub New(ByVal ID As String)
        Me.ID = ID
    End Sub

    'returns -1, 0, or 1 if the current object should be ordered less than, equal to, or greater than another object
    Public Overridable Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
        Dim other As Element = DirectCast(obj, Element)
        Return Me.ID.CompareTo(other.ID)
    End Function

    Public Overrides Function ToString() As String
        Return String.Format("[{0}, {1}, {2}]", ID, FK, FK2)
    End Function

    Public Overridable Function ToStringArray() As String()
        Dim s() As String = {ID, FK, FK2}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNames() As String()
        Return ModellableFieldNames
    End Function

    'For CSV representation
    Public MustOverride Function ToStringArrayCSV() As String()

    Public Shared Shadows Function GetFieldNamesCSV() As String()
        Throw New NotImplementedException
    End Function

End Class

Public MustInherit Class Element
    Inherits Modellable

    'Names and order for ToString method and default ToStringAray method
    Private Shared ReadOnly ElementFieldNames As String() = {"ID", "Type", "Name", "Documentation", "FK", "FK2"}

    ReadOnly Property Type As String
    ReadOnly Property Name As String
    ReadOnly Property Documentation As String


    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String)
        MyBase.New(ID)
        Me.Type = Type
        Me.Name = Name
        Me.Documentation = Documentation
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("[{0}, {1}, {2}, {3}, {4}, {5}]", ID, Type, Name, Documentation, FK, FK2)
    End Function

    Public Overrides Function ToStringArray() As String()
        Dim s() As String = {ID, Type, Name, Documentation, FK, FK2}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNames() As String()
        Return ElementFieldNames
    End Function

End Class

Public MustInherit Class Relation
    Inherits Element

    'Names and order for ToString method and default ToStringAray method
    Private Shared ReadOnly RelationFieldNames As String() = {"ID", "Type", "Name", "Documentation", "Source", "Target", "FK", "FK2"}

    ReadOnly Property Source As String
    ReadOnly Property Target As String

    Sub New(ByVal ID As String, ByVal Type As String, ByVal Name As String, ByVal Documentation As String, ByVal Source As String, ByVal Target As String)
        MyBase.New(ID, Type, Name, Documentation)
        Me.Source = Source
        Me.Target = Target
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("[{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}]", ID, Type, Name, Documentation, Source, Target, FK, FK2)
    End Function

    Public Overrides Function ToStringArray() As String()
        Dim s() As String = {ID, Type, Name, Documentation, Source, Target, FK, FK2}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNames() As String()
        Return RelationFieldNames
    End Function
End Class

Public MustInherit Class Tag
    Inherits Modellable

    'Names and order for ToString method and default ToStringAray method
    Private Shared ReadOnly TagFieldNames As String() = {"ID", "Item", "Key", "Value", "SystType", "FK", "FK2"}

    '
    'Referes to remote object which belongs this Tag.  "That object has this Tag"
    ReadOnly Property Item As String
    ReadOnly Property Key As String
    ReadOnly Property Value As String

    '
    'Serves as indicator whether property serves as hlder for Foregn Key 
    ReadOnly Property SystType As String


    Sub New(ByVal ID As String, ByVal Item As String, ByVal Key As String, ByVal Value As String, ByVal SystType As String)
        MyBase.New(ID)
        Me.Item = Item
        Me.SystType = SystType
        Me.Key = Key
        Me.Value = Value
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("[{0}, {1}, {2}, {3}, {4}, {5}, {6}]", ID, Item, Key, Value, SystType, FK, FK2)
    End Function

    Public Overrides Function ToStringArray() As String()
        Dim s() As String = {ID, Item, Key, Value, SystType, FK, FK2}
        Return s
    End Function

    Public Shared Shadows Function GetFieldNames() As String()
        Return TagFieldNames
    End Function

End Class


'represents memory representation of the model.
Public MustInherit Class ModelMEM(Of tElementMEM, tRelationMEM, tTagMEM)

    ReadOnly Property Elements As Dictionary(Of String, tElementMEM)
    ReadOnly Property Relations As Dictionary(Of String, tRelationMEM)
    ReadOnly Property Tags As Dictionary(Of String, tTagMEM)

    Sub New()
        Elements = New Dictionary(Of String, tElementMEM)
        Relations = New Dictionary(Of String, tRelationMEM)
        Tags = New Dictionary(Of String, tTagMEM)
    End Sub

    Overridable Sub Clear()
        Elements.Clear()
        Relations.Clear()
        Tags.Clear()
    End Sub

    'type - to distinguish between various types of exports - CSV, XML, ....
    Public MustOverride Function ImportFromFiles(directory As String, files() As String, type As String)

    'type - to distinguish between various types of exports - CSV, XML, ....
    Public MustOverride Function ExportToFiles(directory As String, files() As String, type As String)

End Class
'represents memory representation of the model.
'is able to manipulate with COM model  - i.e. trough API with runnig native applicaiton maintaining the model
'
Public MustInherit Class ModelCOM(Of tApplicationCOM, tRepositoryCOM, tElementMEM, tRelationMEM, tTagMEM)
    Inherits ModelMEM(Of tElementMEM, tRelationMEM, tTagMEM)

    ReadOnly Property AppCOM As tApplicationCOM 'should be cleared ....
    ReadOnly Property RepositoryCOM As tRepositoryCOM

    Sub New()
        MyBase.New()
    End Sub

    Overrides Sub Clear()
        MyBase.Clear()
    End Sub

    'supports various type of repositories
    'repository - some identifier, e.g. filename
    'params  - various parameters necessary to open the repository
    'Initiates class-global varibles appCOM and repositoryCOM
    Public MustOverride Sub GetApplicationCOM(repository As String, params As Dictionary(Of String, String))
End Class


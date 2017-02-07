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
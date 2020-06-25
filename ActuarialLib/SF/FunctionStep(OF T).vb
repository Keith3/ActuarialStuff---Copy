Public Class FunctionStep(Of T)

    Public ReadOnly Property Period As Range(Of Double)
    Public ReadOnly Property Value As T

    Private Sub New()
    End Sub

    Public Sub New(ByVal period As Range(Of Double),
                   ByVal value As T)

        If period Is Nothing Then
            Throw New ArgumentNullException(NameOf(period))
        ElseIf value Is Nothing Then
            Throw New ArgumentOutOfRangeException(NameOf(value))
        Else
            _Period = period
            _Value = value
        End If
    End Sub

    Public Sub New(ByVal periodBegin As Double,
                   ByVal periodEnd As Double,
                   ByVal value As T)

        Me.New(New Range(Of Double)(periodBegin, periodEnd), value)
    End Sub

    Public Function XIsInRange(ByVal atValue As Double) As Boolean
        Return _Period.IsInRange(atValue)
    End Function

    Public Overrides Function ToString() As String
        Return "Period: " & Period.ToString & "  Value: " & _Value.ToString
    End Function

End Class
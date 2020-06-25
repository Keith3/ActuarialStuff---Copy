Public Class Range(Of T As IComparable(Of T))

    Public ReadOnly Property MinValue As T
    Public ReadOnly Property MaxValue As T

    Private Sub New()
    End Sub

    Public Sub New(ByVal value1 As T, ByVal value2 As T)
        If value1.CompareTo(value2) <= 0 Then
            _MinValue = value1
            _MaxValue = value2
        Else   'swap to insure that _MinValue is <= _MaxValue
            _MinValue = value2
            _MaxValue = value1
        End If
    End Sub

    Public Function IsInRange(ByVal atValue As T) As Boolean
        Return atValue.CompareTo(_MinValue) >= 0 AndAlso atValue.CompareTo(_MaxValue) < 0
    End Function

    Public Overrides Function ToString() As String
        Return (_MinValue.ToString & "," & _MaxValue.ToString).AddParens
    End Function

    Public Function Overlap(ByVal other As Range(Of T)) As Range(Of T)
        If other Is Nothing Then Return Nothing

        Dim MaxMin As T = MinValue
        If other.MinValue.CompareTo(MinValue) >= 0 Then MaxMin = other.MinValue

        Dim MinMax As T = MaxValue
        If other.MaxValue.CompareTo(MaxValue) < 0 Then MinMax = other.MaxValue

        If MinMax.CompareTo(MaxMin) > 0 Then
            Return New Range(Of T)(MaxMin, MinMax)
        Else
            Return Nothing
        End If
    End Function

End Class
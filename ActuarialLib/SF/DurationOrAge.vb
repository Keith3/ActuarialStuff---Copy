Imports System.Globalization

Public NotInheritable Class DurationOrAge

    Private ReadOnly _time As Double
    Private ReadOnly _isDuration As Boolean

    Private Sub New()
    End Sub

    Public Sub New(ByVal time As Double,
                   ByVal isDuration As Boolean)

        _time = time
        _isDuration = isDuration
    End Sub

    Public Sub New(ByVal time As Double)

        _time = time
        _isDuration = True
    End Sub

    Public Function IsDuration() As Boolean
        Return _isDuration
    End Function

    Public Function Duration(ByVal issueAge As Integer) As Double
        Return If(_isDuration, _time, _time - issueAge)
    End Function

    Public Function Value() As Double
        Return _time
    End Function

    Public Overrides Function ToString() As String
        Return If(_isDuration, "Duration ", "Age ") & _time.ToString(CultureInfo.CurrentCulture)
    End Function

End Class
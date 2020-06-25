Imports System.Globalization.CultureInfo
Imports System.Text

Public Class ActuarialArray

    Private xyValues As SortedList(Of Integer, SortedList(Of Double, Double))
    Private yAxis As SortedList(Of Double, Integer)
    Private zValues As SortedList(Of Double, Double)

    Public Property XTitle As String = "Issue Age"
    Public Property YTitle As String = "Duration"
    Public Property ZTitle As String = "Attained Age"
    Public Property XAxisFormat As String = "N0"
    Public Property YAxisFormat As String = "N2"
    Public Property ZAxisFormat As String = "N0"
    Public Property ValuesFormat As String = "N5"

    Public Sub AddXYValue(ByVal x As Integer,
                          ByVal y As Double,
                          ByVal value As Double)

        If x >= 0 AndAlso y >= 0 Then
            If xyValues Is Nothing Then
                xyValues = New SortedList(Of Integer, SortedList(Of Double, Double))
                yAxis = New SortedList(Of Double, Integer)
            End If
            If Not xyValues.ContainsKey(x) Then
                Dim row As New SortedList(Of Double, Double)
                row(y) = value
                xyValues.Add(x, row)
                yAxis(y) = Math.Max(y.ToString(YAxisFormat, CurrentCulture).Length, value.ToString(ValuesFormat, CurrentCulture).Length) + 1
            ElseIf Not xyValues(x).ContainsKey(y) Then
                xyValues(x).Add(y, value)
                yAxis(y) = Math.Max(y.ToString(YAxisFormat, CurrentCulture).Length, value.ToString(ValuesFormat, CurrentCulture).Length) + 1
            Else
                xyValues(x)(y) = value
                yAxis(y) = Math.Max(y.ToString(YAxisFormat, CurrentCulture).Length, yAxis(y))
            End If
        End If
    End Sub

    Public Sub AddZValue(ByVal z As Double,
                         ByVal value As Double)

        If z >= 0 Then
            If zValues Is Nothing Then
                zValues = New SortedList(Of Double, Double)
            End If
            If Not zValues.ContainsKey(z) Then
                zValues(z) = value
            End If
        End If
    End Sub

    Public ReadOnly Property Values(ByVal x As Integer) As List(Of KeyValuePair(Of Double, Double))
        Get
            If xyValues IsNot Nothing AndAlso xyValues.ContainsKey(x) Then
                Dim newList As List(Of KeyValuePair(Of Double, Double)) = xyValues(x).ToList
                If zValues IsNot Nothing Then
                    Dim lastAttainedAge As Double = xyValues.Last.Key + x
                    For Each v In zValues
                        If v.Key > lastAttainedAge Then newList.Add(v)
                    Next
                End If
                Return newList
            ElseIf zValues IsNot Nothing Then
                Dim newList As New List(Of KeyValuePair(Of Double, Double))
                For Each v In zValues
                    If v.Key > x Then newList.Add(v)
                Next
                Return newList
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public Sub Trim()
        If zValues IsNot Nothing Then zValues.TrimExcess()

        If xyValues IsNot Nothing Then
            xyValues.TrimExcess()
            For Each row In xyValues
                row.Value.TrimExcess()
            Next
        End If
    End Sub

    Public Overrides Function ToString() As String

        Dim sb As New StringBuilder("")

        If xyValues IsNot Nothing Then

            'Horizontal y axis title
            Dim xLabelWidth As Integer = xyValues.Last.Key.ToString(XAxisFormat, CurrentCulture).Length
            sb.Append(New String(" "c, xLabelWidth + 3)).Append(YTitle).Append(vbCrLf)

            'Column Headings
            Dim yAxisWidth As Integer = 0
            sb.Append(New String(" "c, xLabelWidth + 3))
            For Each col In yAxis
                yAxisWidth += col.Value
                sb.Append(col.Key.ToString(YAxisFormat, CurrentCulture).PadLeft(col.Value))
            Next
            sb.Append(vbCrLf)

            'Row of ----------
            sb.Append(New String(" "c, xLabelWidth + 3))
            sb.Append(New String("-"c, yAxisWidth))
            sb.Append(vbCrLf)

            'Values
            Dim i As Integer = 0
            For Each row In xyValues
                If i < XTitle.Length Then
                    sb.Append(XTitle.Substring(i, 1))
                Else
                    sb.Append(" ")
                End If
                sb.Append(" ")
                sb.Append(row.Key.ToString(XAxisFormat, CurrentCulture).PadLeft(xLabelWidth)).Append("|")

                Dim value As Double
                For Each col In yAxis
                    If row.Value.ContainsKey(col.Key) Then
                        value = row.Value(col.Key)
                        sb.Append(value.ToString(ValuesFormat, CurrentCulture).PadLeft(col.Value))
                    Else
                        sb.Append(New String(" "c, col.Value))
                    End If
                Next
                i += 1
                sb.Append(vbCrLf)
            Next
            If zValues IsNot Nothing Then
                sb.Append(vbCrLf)
                sb.Append(vbCrLf)
            End If
        End If

        If zValues IsNot Nothing Then

            Dim zAxisKeyWidth As Integer = 0   ' xAxisMaxIndex.ToString.Length
            Dim zAxisValueWidth As Integer = 0

            For Each row In zValues       ' i = 0 To xAxisMaxIndex
                zAxisKeyWidth = Math.Max(row.Key.ToString(ZAxisFormat, CurrentCulture).Length, zAxisKeyWidth)
                zAxisValueWidth = Math.Max(row.Value.ToString(ValuesFormat, CurrentCulture).Length, zAxisValueWidth)
            Next
            zAxisValueWidth += 1

            'Values
            Dim i As Integer = 0
            For Each row In zValues      ' i = 0 To xAxisMaxIndex
                If i < ZTitle.Length Then
                    sb.Append(ZTitle.Substring(i, 1))
                Else
                    sb.Append(" ")
                End If
                sb.Append(" ")
                sb.Append(row.Key.ToString(ZAxisFormat, CurrentCulture).PadLeft(zAxisKeyWidth)).Append("|")
                sb.Append(row.Value.ToString(ValuesFormat, CurrentCulture).PadLeft(zAxisValueWidth)).Append(vbCrLf)
                i += 1
            Next

        End If

        Return sb.ToString
    End Function

End Class
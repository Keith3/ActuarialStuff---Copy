Imports System.Globalization

Public Class JaggedStringStringDoubleArray
    Inherits JaggedKeyedArray(Of String, String, Double)

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String,
                              ByVal format As String,
                              ByVal displayZero As Boolean,
                              ByVal displayNaN As Boolean) As String

        If xAxisName Is Nothing Then Throw New ArgumentNullException(NameOf(xAxisName))
        If yAxisName Is Nothing Then Throw New ArgumentNullException(NameOf(yAxisName))

        Dim sb As New Text.StringBuilder("")
        Dim xAxisLabels As IEnumerable(Of String)

        If IsOneDimensional() Then

            xAxisLabels = Keys2("")
            Dim xAxisLabelWidth As Integer = StringEnumWidth(xAxisLabels)
            Dim xAxisStringValues() As String = DisplayValues(Items(""), format, displayZero, displayNaN)
            Dim xAxisValueWidth As Integer = StringEnumWidth(xAxisStringValues)

            'Show table rows when both Axis Name and Axis Values exist
            For i = 0 To Math.Min(xAxisName.Length, xAxisLabels.Count) - 1
                sb.Append(xAxisName.Substring(i, 1))
                sb.Append(xAxisLabels(i).PadLeft(xAxisLabelWidth + 1)).Append("|")
                sb.Append(xAxisStringValues(i).PadLeft(xAxisValueWidth + 1)).Append(vbCrLf)
            Next

            'Value
            For i = Math.Min(xAxisName.Length, xAxisLabels.Count) To xAxisLabels.Count - 1
                If i <= xAxisName.Length - 1 Then
                    sb.Append(xAxisName.Substring(i, 1))
                Else
                    sb.Append(" ")
                End If
                sb.Append(xAxisLabels(i).PadLeft(xAxisLabelWidth + 1)).Append("|")
                sb.Append(xAxisStringValues(i).PadLeft(xAxisValueWidth + 1)).Append(vbCrLf)
            Next

        Else

            xAxisLabels = Keys1()
            Dim xAxisLabelWidth As Integer = StringEnumWidth(xAxisLabels)

            'Calculate Column Widths
            Dim yAxisWidth As New Dictionary(Of String, Integer)
            Dim yAxisLabels As IEnumerable(Of String) = Keys2(xAxisLabels(0))

            For Each col In yAxisLabels
                yAxisWidth(col) = col.Length
                For Each label In xAxisLabels
                    yAxisWidth(col) = Math.Max(DisplayValue(Item(label, col), format, displayZero, displayNaN).Length, yAxisWidth(col))
                Next
            Next

            'Horizontal y axis name
            sb.Append(Space(xAxisLabelWidth + 3)).Append(yAxisName).Append(vbCrLf)

            'Column Headings
            Dim TotalColWidth As Integer = 0
            sb.Append(Space(xAxisLabelWidth + 3))

            For Each col In yAxisLabels
                TotalColWidth += yAxisWidth(col) + 1
                If yAxisWidth.ContainsKey(col) Then
                    sb.Append(col.ToString(CultureInfo.CurrentCulture).PadLeft(yAxisWidth(col) + 1))
                Else
                    sb.Append(Space(yAxisWidth(col) + 1))
                End If
            Next
            sb.Append(vbCrLf)

            'Row of ----------
            sb.Append(Space(xAxisLabelWidth + 3)).Append(New String("-"c, TotalColWidth)).Append(vbCrLf)

            'Values
            For i = 0 To Keys1.Count - 1
                If i < xAxisName.Length AndAlso i < xAxisLabels.Count Then
                    sb.Append(xAxisName.Substring(i, 1))
                Else
                    sb.Append(" ")
                End If

                sb.Append(xAxisLabels(i).PadLeft(xAxisLabelWidth + 1)).Append("|")

                For Each col In yAxisLabels
                    sb.Append(DisplayValue(Item(xAxisLabels(i), col), format, displayZero, displayNaN).PadLeft(yAxisWidth(col) + 1))
                Next
                sb.Append(vbCrLf)
            Next
        End If

        Return sb.ToString

    End Function

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String,
                              ByVal format As String) As String

        Return TextTable(xAxisName, yAxisName, format, True, False)
    End Function

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String) As String

        Return TextTable(xAxisName, yAxisName, "G", True, False)
    End Function

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String,
                              ByVal displayZero As Boolean,
                              ByVal displayNaN As Boolean) As String

        Return TextTable(xAxisName, yAxisName, "G", displayZero, displayNaN)
    End Function

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String,
                              ByVal displayZero As Boolean) As String

        Return TextTable(xAxisName, yAxisName, "G", displayZero, False)
    End Function

    Public Function TextTable(ByVal xAxisName As String,
                              ByVal yAxisName As String,
                              ByVal format As String,
                              ByVal displayZero As Boolean) As String

        Return TextTable(xAxisName, yAxisName, format, displayZero, False)
    End Function

    Private Shared Function StringEnumWidth(ByVal strEnum As IEnumerable(Of String)) As Integer
        Dim width As Integer = 0

        For Each s In strEnum
            width = Math.Max(s.Length, width)
        Next

        Return width
    End Function

    Private Shared Function DisplayValue(ByVal value As Double,
                                         ByVal format As String,
                                         ByVal displayZero As Boolean,
                                         ByVal displayNaN As Boolean) As String

        If value = 0 AndAlso Not displayZero Then
            Return ""
        ElseIf Double.IsNaN(value) AndAlso Not displayNaN Then
            Return ""
        Else
            Return value.ToString(format, CultureInfo.CurrentCulture)
        End If
    End Function

    Private Shared Function DisplayValues(ByVal values As IEnumerable(Of Double),
                                          ByVal format As String,
                                          ByVal displayZero As Boolean,
                                          ByVal displayNaN As Boolean) As String()

        Dim valueStrings(values.Count - 1) As String
        For v = 0 To values.Count - 1
            valueStrings(v) = DisplayValue(values(v), format, displayZero, displayNaN)
        Next
        Return valueStrings
    End Function

End Class
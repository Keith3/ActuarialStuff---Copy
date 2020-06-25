Imports System.Text
Imports System.Collections.ObjectModel
Imports System.Globalization

Public Class SoaTable

    Public ReadOnly Property XmlText As String   ' XElement

    Public ReadOnly Property TableName As String = ""
    Public ReadOnly Property TableIdentity As Integer = -1
    Public ReadOnly Property ProviderDomain As String = ""
    Public ReadOnly Property ProviderName As String = ""
    Public ReadOnly Property ContentType As String = ""
    Public ReadOnly Property ContentTypeCode As Integer = 0
    Public ReadOnly Property TableReference As String = ""
    Public ReadOnly Property TableDescription As String = ""
    Public ReadOnly Property Comments As String = ""
    Public ReadOnly Property Keywords As New Collection(Of String)
    Public ReadOnly Property Warnings As New Collection(Of String)
    Public ReadOnly Property ContentTypeKey As String = ""
    Public ReadOnly Property ProviderKey As String = ""
    Public ReadOnly Property StructureKey As String = ""
    Public ReadOnly Property NationKey As String = ""

    Public ReadOnly Property SubTables As New Collection(Of SoaSubTable)

    Private Sub New()
    End Sub

    Public Sub New(ByVal xml As XElement)

        If xml Is Nothing Then Throw New ArgumentNullException(NameOf(xml))

        _XmlText = xml.ToString

        Dim xe As XElement
        Dim xa As XAttribute
        Dim xes As IEnumerable(Of XElement)

        With xml.Element("ContentClassification")

            xe = .Element("TableName")
            '_TableName = ""

            If xe Is Nothing Then
                _Warnings.Add("No Table Name")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Empty Table Name")
            Else
                _TableName = xe.Value.Trim

                If _TableName.Contains("  ") Then
                    _Warnings.Add("Consecutive Spaces in Table Name")
                End If

                If _TableName.ToLower(CultureInfo.CurrentCulture).Contains("table") Then
                    _Warnings.Add("Redundant 'Table' in Table Name")
                End If

                For Each c In _TableName.ToCharArray
                    If Char.IsWhiteSpace(c) AndAlso Asc(c) <> 32 Then
                        _Warnings.Add("Whitespace Other Than Asc(32) in Table Name")
                        Exit For
                    End If
                Next
            End If

            xe = .Element("TableIdentity")
            '_TableIdentity = -1

            If xe Is Nothing Then
                _Warnings.Add("Empty Table Identity (-1 Assumed")
            ElseIf Not Integer.TryParse(xe.Value, _TableIdentity) Then
                _Warnings.Add("Non-Integral TableIdentity (-1 Assumed")
            End If

            xe = .Element("ContentType")
            '_ContentType = ""
            '_ContentTypeCode = 0

            If xe Is Nothing Then
                _Warnings.Add("No Content Type Description")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Content Type Is Empty")
                'ElseIf ContentType = "CSO / CET" OrElse ContentType = "CSO/CET" Then
                '    AddWarning("Inconsistent Content Type Format: CSO / CET or CSO/CET")
            Else
                _ContentType = If(xe.Value, "").Trim

                xa = xe.Attribute("tc")
                If xa Is Nothing Then
                    _Warnings.Add("Empty Content Type ID")
                ElseIf Not Integer.TryParse(xa.Value, _ContentTypeCode) Then
                    _Warnings.Add("Non-Integral Content Type ID")
                End If
            End If

            xe = .Element("ProviderDomain")
            '_ProviderDomain = ""

            If xe Is Nothing Then
                _Warnings.Add("No Provider Domain")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Empty Provider Domain")
            Else
                _ProviderDomain = xe.Value.Trim
            End If

            xe = .Element("ProviderName")
            '_ProviderName = ""

            If xe Is Nothing Then
                _Warnings.Add("No Provider Name")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Empty Provider Name")
            Else
                _ProviderName = xe.Value.Trim
            End If

            xe = .Element("TableReference")
            '_TableReference = ""

            If xe Is Nothing Then
                _Warnings.Add("No Table Reference")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Empty Table Reference")
            Else
                _TableReference = xe.Value.Trim
            End If

            xe = .Element("TableDescription")
            '_TableDescription = If(xe Is Nothing, "", If(xe.Value, "").Trim)

            If xe Is Nothing Then
                _Warnings.Add("No Table Description")
            ElseIf xe.Value.Length = 0 Then
                _Warnings.Add("Empty Table Description")
            Else
                _TableDescription = xe.Value.Trim
            End If

            xes = .Elements("KeyWord")

            For Each k In .Elements("KeyWord")
                Dim key As String = k.Value.Trim

                If key.Length = 0 Then
                    _Warnings.Add("Empty Keyword")
                ElseIf _Keywords.Contains(k.Value.Trim) Then
                    _Warnings.Add("Duplicate Keyword Not Added")
                Else
                    _Keywords.Add(key)
                End If
            Next

            xe = .Element("Comments")
            _Comments = If(xe Is Nothing, "", If(xe.Value, "").Trim)
        End With

        Dim subTab As SoaSubTable

        For Each XTable As XElement In xml.Elements("Table")

            subTab = New SoaSubTable(XTable)






            SubTables.Add(subTab)

            _StructureKey &= subTab.StructureKey

        Next

        _ContentTypeKey = ContentTypeCode.ToString(CultureInfo.CurrentCulture).PadLeft(2, "0"c) & " " & ContentType
        _ProviderKey = ProviderName & ProviderDomain.AddParens
        _NationKey = SubTables(0).NationId.ToString(CultureInfo.CurrentCulture).PadLeft(4, "0"c) & " " & SubTables(0).Nation

        GenerateTableWarnings()

    End Sub

    Private Sub GenerateTableWarnings()

        For Each subTab As SoaSubTable In SubTables

            'Dim axisDefs() As SoaTableAxisDefinition = subTab.AxisDefinitions.ToArray

            For axisDefIndex = 0 To subTab.AxisDefinitions.Count - 1

                Dim axisDef As SoaTableAxisDefinition = subTab.AxisDefinitions(axisDefIndex)

                If axisDef.ScaleIncrement <= 0 Then
                    _Warnings.Add("AxisDef " & axisDefIndex.ToString(CultureInfo.CurrentCulture) & " Scale Increment <= 0")
                ElseIf axisDef.MaxScaleValue < axisDef.MinScaleValue Then
                    _Warnings.Add("AxisDef " & axisDefIndex.ToString(CultureInfo.CurrentCulture) & " MaxScaleValue < MinScaleValue")
                    'ElseIf keys(axisDefIndex).Count <> ((axisDef.MaxScaleValue - axisDef.MinScaleValue + axisDef.ScaleIncrement) \ axisDef.ScaleIncrement) Then
                    '    AddWarning("AxisDef " & axisDefIndex.ToString(CultureInfo.CurrentCulture) & " Count Doesn't Match Axis Key Count")
                    'Else
                    '    For j = 0 To keys(axisDefIndex).Count - 1
                    '        If keys(axisDefIndex)(j).Trim <> (axisDef.MinScaleValue + j * axisDef.ScaleIncrement).ToString(CultureInfo.CurrentCulture) Then
                    '            AddWarning("AxisDef Doesn't Match Axis Labels")
                    '            Exit For
                    '        End If
                    '    Next
                End If

            Next

        Next

    End Sub

    Public Function DisplayDescription() As String
        Dim sb As New StringBuilder("")

        sb.Append(TableIdentity.ToString("00000 ", CultureInfo.CurrentCulture))
        sb.Append(TableName).Append(vbCrLf).Append(vbCrLf)
        sb.Append("Content Type: ").Append(ContentTypeKey.Trim).Append(vbCrLf)
        sb.Append("Nation:       ").Append(NationKey.Trim).Append(vbCrLf)
        sb.Append("Key Words:    ").Append(MyJoin(", ", _Keywords).AddBraces).Append(vbCrLf)
        sb.Append("Provider:     ").Append(ProviderKey).Append(vbCrLf).Append(vbCrLf)
        sb.Append("Description:  ").Append(TableDescription).Append(vbCrLf).Append(vbCrLf)
        sb.Append("Reference:    ").Append(TableReference).Append(vbCrLf).Append(vbCrLf)
        sb.Append("Comments:     ").Append(Comments).Append(vbCrLf)

        Return sb.ToString
    End Function

    Public Function DisplayStructure() As String

        Dim sb As New StringBuilder(ToString)

        sb.Append(vbCrLf)
        sb.Append(vbCrLf)
        sb.Append("Table Structure: ").Append(_StructureKey)
        sb.Append(vbCrLf)

        For t = 0 To SubTables.Count - 1

            Dim currTab As SoaSubTable = SubTables(t)

            sb.Append(vbCrLf)
            sb.Append("Sub-Table: ").Append(t.ToString(CultureInfo.CurrentCulture)).Append(vbCrLf)
            sb.Append("           ").Append(currTab.Nation).Append(vbCrLf)
            sb.Append("           ").Append(currTab.TableDescription).Append(vbCrLf)
            sb.Append("           Scaling Factor ").Append(currTab.ScalingFactor.ToString(CultureInfo.CurrentCulture)).Append(vbCrLf)
            sb.Append("           Data Type ").Append(currTab.DataType).Append(vbCrLf).Append(vbCrLf)

            Dim axisDefCount As Integer = currTab.AxisDefinitions.Count
            Dim axisDef(axisDefCount - 1) As List(Of String)
            Dim axisDesc(axisDefCount - 1) As String

            For a = 0 To axisDefCount - 1
                Dim currAxis As SoaTableAxisDefinition = currTab.AxisDefinitions(a)

                axisDesc(a) = currAxis.AxisName & "/" & currAxis.ScaleType
                axisDef(a) = New List(Of String)

                'If currAxis.ScaleIncrement > 0 Then
                For i = currAxis.MinScaleValue To currAxis.MaxScaleValue Step Math.Max(currAxis.ScaleIncrement, 1)
                        axisDef(a).Add(i.ToString(CultureInfo.CurrentCulture))
                    Next
                'End If
            Next

            sb.Append(vbCrLf)

            Dim tableAxis() As String
            tableAxis = currTab.TableValues.Keys1

            If currTab.TableValues.IsOneDimensional Then
                tableAxis = currTab.TableValues.Keys2("")
                For i = 0 To tableAxis.Count - 1
                    tableAxis(i) = tableAxis(i).AddQuotes
                Next
            Else
                For i = 0 To tableAxis.Count - 1
                    Dim tableAxis2() As String = currTab.TableValues.Keys2(tableAxis(i))
                    For k = 0 To tableAxis2.Count - 1
                        tableAxis2(k) = tableAxis2(k).AddQuotes
                    Next
                    tableAxis(i) = tableAxis(i).AddQuotes.PadLeft(5) & " by " & MyJoin(", ", tableAxis2).AddBraces
                Next
            End If

            Dim col1Width As Integer = Math.Max(9, Math.Max(axisDesc(0).Length, currTab.AxisDefinitions(0).Range.Length))
            Dim col2Width As Integer = If(axisDefCount > 1, Math.Max(9, Math.Max(axisDesc(1).Length, currTab.AxisDefinitions(1).Range.Length)), 0)

            sb.Append(axisDesc(0).PadLeft(col1Width)).Append(If(axisDefCount > 1, axisDesc(1).PadLeft(col2Width + 2), "")).Append(vbCrLf)
            sb.Append(currTab.AxisDefinitions(0).Range.PadLeft(col1Width)).Append(If(axisDefCount > 1, currTab.AxisDefinitions(1).Range.PadLeft(col2Width + 2), "")).Append(vbCrLf)
            sb.Append("AxisDef 0".PadLeft(col1Width)).Append(If(axisDefCount > 1, "AxisDef 1".PadLeft(col2Width + 2), "")).Append("  Table Axis").Append(vbCrLf)
            sb.Append(New String("-"c, col1Width)).Append(If(axisDefCount > 1, "  " & (New String("-"c, col2Width)).PadLeft(col2Width), "")).Append("  ----------").Append(vbCrLf)

            Dim maxCount As Integer = Math.Max(axisDef(0).Count, tableAxis.Count)
            If axisDefCount = 2 Then maxCount = Math.Max(maxCount, axisDef(1).Count)

            For i = 0 To maxCount - 1
                If i < axisDef(0).Count Then
                    sb.Append(axisDef(0)(i).PadLeft(col1Width))
                Else
                    sb.Append(Space(col1Width))
                End If

                If axisDefCount > 1 Then
                    If i < axisDef(1).Count Then
                        sb.Append(axisDef(1)(i).PadLeft(col2Width + 2))
                    Else
                        sb.Append(Space(col2Width + 2))
                    End If
                End If

                If i < tableAxis.Count Then
                    sb.Append("  ").Append(tableAxis(i))
                End If

                sb.Append(vbCrLf)
            Next
        Next

        Return sb.ToString

    End Function

    Public Function DisplayValues() As String

        Dim sb As New StringBuilder(ToString)
        sb.Append(vbCrLf)

        For t = 0 To SubTables.Count - 1
            sb.Append(vbCrLf)
            sb.Append("Sub-Table: ").Append(t.ToString(CultureInfo.CurrentCulture)).Append(vbCrLf)
            sb.Append("           ").Append(SubTables(t).Nation).Append(vbCrLf)
            sb.Append("           ").Append(SubTables(t).TableDescription).Append(vbCrLf)
            sb.Append("           Scaling Factor ").Append(SubTables(t).ScalingFactor.ToString(CultureInfo.CurrentCulture)).Append(vbCrLf)
            sb.Append("           Data Type ").Append(SubTables(t).DataType).Append(vbCrLf)

            For i = 0 To SubTables(t).AxisDefinitions.Count - 1
                sb.Append("           Axis: ").Append(i).Append("  ")
                Dim AxisType As String = (SubTables(t).AxisDefinitions(i).AxisName & "/" & SubTables(t).AxisDefinitions(i).ScaleType)
                sb.Append("    ").Append(AxisType)
                sb.Append("    ").Append(SubTables(t).AxisDefinitions(i).Range)
                sb.Append(vbCrLf)
            Next
            sb.Append(vbCrLf)

            Dim xAxisName As String = SubTables(t).AxisDefinitions(0).AxisName
            Dim yAxisName As String = If(SubTables(t).AxisDefinitions.Count > 1, SubTables(t).AxisDefinitions(1).AxisName, "")
            sb.Append(SubTables(t).TableValues.TextTable(xAxisName, yAxisName))
        Next

        Return sb.ToString
    End Function

    Public Function DisplayWarnings() As String
        Return If(Warnings.Count > 0, MyJoin(vbCrLf, Warnings), "")
    End Function

    Public Overrides Function ToString() As String
        Return TableIdentity.ToString("00000", CultureInfo.CurrentCulture) & " " & TableName
    End Function

    Public Shared Function ComparerByTableName() As IComparer(Of SoaTable)
        Return New CompareByName()
    End Function

    Public Shared Function ComparerByTableId() As IComparer(Of SoaTable)
        Return New CompareByID()
    End Function

    Private Class CompareByName
        Implements IComparer(Of SoaTable)

        Public Function Compare(table1 As SoaTable,
                                table2 As SoaTable) As Integer Implements IComparer(Of SoaTable).Compare

            If table1 Is Nothing Then
                Throw New ArgumentNullException(NameOf(table1))
            ElseIf table2 Is Nothing Then
                Throw New ArgumentNullException(NameOf(table2))
            Else
                Return String.Compare(table1.TableName, table2.TableName, StringComparison.Ordinal)
            End If
        End Function
    End Class

    Private Class CompareByID
        Implements IComparer(Of SoaTable)

        Public Function Compare(table1 As SoaTable,
                                table2 As SoaTable) As Integer Implements IComparer(Of SoaTable).Compare

            If table1 Is Nothing Then
                Throw New ArgumentNullException(NameOf(table1))
            ElseIf table2 Is Nothing Then
                Throw New ArgumentNullException(NameOf(table2))
            Else
                Return table1.TableIdentity.CompareTo(table2.TableIdentity)
            End If
        End Function
    End Class

End Class
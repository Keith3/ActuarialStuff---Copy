Imports System.Collections.ObjectModel

Public Class SoaSubTable

    Public ReadOnly Property TableDescription As String = ""
    Public ReadOnly Property Nation As String = ""
    Public ReadOnly Property NationId As Integer = 0
    Public ReadOnly Property DataType As String = ""
    Public ReadOnly Property DataTypeId As Integer
    Public ReadOnly Property ScalingFactor As Integer
    Public ReadOnly Property StructureKey As String = ""
    Public ReadOnly Property TableValues As New JaggedStringStringDoubleArray
    Public ReadOnly Property Warnings As New Collection(Of String)

    Public ReadOnly Property AxisDefinitions As New Collection(Of SoaTableAxisDefinition)

    Private Sub New()
    End Sub

    Public Sub New(ByVal table As XElement)

        If table Is Nothing Then Throw New ArgumentNullException(NameOf(table))

        Dim xe As XElement
        Dim xa As XAttribute

        With table.Element("MetaData")

            xe = .Element("Nation")
            If xe IsNot Nothing Then
                _Nation = If(xe.Value, "").Trim
                _Nation = If(Nation = "United States of America", "United States", Nation)

                xa = xe.Attribute("tc")
                If xa Is Nothing Then
                    _Warnings.Add("Invalid Nation ID")
                ElseIf Not Integer.TryParse(xa.Value, _NationId) Then
                End If
                _NationId = -1
            Else
                _Nation = ""
                _NationId = -1
            End If
            If _NationId < 1 Then
            ElseIf _NationId = 1 AndAlso _Nation <> "United States" Then
                _Warnings.Add("Nation and Nation ID Mismatch")
            ElseIf _NationId = 2 AndAlso _Nation <> "Canada" Then
                _Warnings.Add("Nation and Nation ID Mismatch")
            ElseIf _NationId = 44 AndAlso _Nation <> "United Kingdom" Then
                _Warnings.Add("Nation and Nation ID Mismatch")
            End If
            If _Nation.Length = 0 Then
                _Warnings.Add("Empty Nation Name")
            End If

            xe = .Element("TableDescription")
            _TableDescription = If(xe Is Nothing, "", If(xe.Value, "").Trim)

            xe = .Element("DataType")
            If xe IsNot Nothing Then
                _DataType = If(xe.Value, "").Trim
                xa = xe.Attribute("tc")
                If xa Is Nothing OrElse Not Integer.TryParse(xa.Value, _DataTypeId) Then
                    _DataTypeId = -1
                End If
            Else
                _DataType = ""
                _DataTypeId = -1
            End If

            If _DataType.Length = 0 Then
                _Warnings.Add("Empty Data Type")
            End If

            If _DataTypeId < 0 Then
                _Warnings.Add("Negative Data Type ID")
            End If

            If .Element("ScalingFactor") Is Nothing OrElse Not Integer.TryParse(.Element("ScalingFactor").Value, _ScalingFactor) Then
                _ScalingFactor = -1
            End If

            If _ScalingFactor < 0 Then
                _Warnings.Add("Scaling Factor < 0")
            End If

            Dim key As String = ""
            For Each xDef In .Elements("AxisDef")
                Dim struc As New SoaTableAxisDefinition(xDef)
                AxisDefinitions.Add(struc)
                key &= struc.AxisName.Substring(0, 1)
            Next
            _StructureKey = If(key.Length > 1, key.AddParens, key)

        End With

        With table.Element("Values")

            Dim entryValue As Double
            Dim label1 As String
            Dim label2 As String

            For Each axis1 In .Elements("Axis")
                If axis1.HasAttributes Then     '2 dimensional

                    label1 = axis1.Attribute("t").Value

                    For Each axis2 In axis1.Elements("Axis")
                        For Each entry In axis2.Elements("Y")
                            label2 = entry.Attribute("t").Value

                            If Double.TryParse(entry.Value, entryValue) Then
                                _TableValues(label1, label2) = entryValue
                            Else
                                _TableValues(label1, label2) = Double.NaN
                            End If
                        Next
                    Next

                Else

                    For Each entry In axis1.Elements("Y")
                        label2 = entry.Attribute("t").Value
                        If Double.TryParse(entry.Value, entryValue) Then
                            _TableValues.Item("", label2) = entryValue
                        Else
                            _TableValues.Item("", label2) = Double.NaN
                        End If
                    Next

                End If
            Next

            If CheckForNonIntegralAxisKey(TableValues) Then
                _Warnings.Add("Non-Integral Axis Key")
            End If

            If AxisDefinitions.Count <> KeysCount(TableValues) Then
                _Warnings.Add("AxisDef Count and Axis Count Mismatch")
            End If

        End With

    End Sub

    Private Function CheckForNonIntegralAxisKey(ByVal tableValues As JaggedStringStringDoubleArray) As Boolean
        Dim result As Integer

        If tableValues.Keys1.Count = 1 AndAlso tableValues.Keys1.First.Length = 0 Then

            For Each k2 In tableValues.Keys2("")
                If Not Integer.TryParse(k2, result) Then
                    Return True
                End If
            Next

        Else

            For Each k1 In tableValues.Keys1()
                If Not Integer.TryParse(k1, result) Then
                    Return True
                End If
                For Each k2 In tableValues.Keys2(k1)
                    If Not Integer.TryParse(k2, result) Then
                        Return True
                    End If
                Next
            Next
        End If

        Return False
    End Function

    Private Function KeysCount(ByVal tableValues As JaggedStringStringDoubleArray) As Integer
        If tableValues.Keys1.Count = 1 AndAlso tableValues.Keys1.First.Length = 0 Then
            Return 1
        Else
            Return 2
        End If
    End Function

End Class


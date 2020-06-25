Imports System.Collections.ObjectModel
Imports System.Globalization

Public Class SoaTableAxisDefinition

    Public Property AxisId As String
    Public Property ScaleType As String = ""
    Public Property ScaleTypeId As Integer
    Public Property MinScaleValue As Integer
    Public Property MaxScaleValue As Integer
    Public Property ScaleIncrement As Integer
    Public ReadOnly Property Labels As New Collection(Of String)
    Public ReadOnly Property Warnings As New Collection(Of String)

    Private Sub New()
    End Sub

    Public Sub New(ByVal axisDefinition As XElement)
        If axisDefinition Is Nothing Then Throw New ArgumentNullException(NameOf(axisDefinition))

        Dim xe As XElement
        Dim xa As XAttribute

        xe = axisDefinition.Element("AxisName")
        AxisName = If(xe Is Nothing, "", If(xe.Value, ""))

        xa = axisDefinition.Attribute("id")
        AxisId = If(xa Is Nothing, "", If(xa.Value, ""))

        xe = axisDefinition.Element("ScaleType")
        If xe IsNot Nothing Then
            ScaleType = If(xe.Value, "")
            xa = xe.Attribute("tc")
            If xa Is Nothing OrElse Not Integer.TryParse(xa.Value, ScaleTypeId) Then ScaleTypeId = 0
        Else
            ScaleType = ""
            ScaleTypeId = 0
        End If

        xe = axisDefinition.Element("MinScaleValue")
        If xe Is Nothing OrElse Not Integer.TryParse(xe.Value, MinScaleValue) Then MinScaleValue = -1

        xe = axisDefinition.Element("MaxScaleValue")
        If xe Is Nothing OrElse Not Integer.TryParse(xe.Value, MaxScaleValue) Then MaxScaleValue = -1

        xe = axisDefinition.Element("Increment")
        If xe Is Nothing OrElse Not Integer.TryParse(xe.Value, ScaleIncrement) Then
            ScaleIncrement = 1
        End If

    End Sub

    Private _AxisName As String = ""
    Public Property AxisName As String
        Get
            Return _AxisName
        End Get
        Private Set(value As String)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            _AxisName = If(String.Compare(value, "Duration", True, CultureInfo.CurrentCulture) = 0, "Dur", value)
        End Set
    End Property

    Public Function Range() As String
        Return _MinScaleValue & "-" & _MaxScaleValue & "," & _ScaleIncrement
    End Function

    Public Sub AddAxisLabel(ByVal label As String)
        If label IsNot Nothing Then
            _Labels.Add(label)
        Else
            _Labels.Add("")
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return AxisName
    End Function

End Class
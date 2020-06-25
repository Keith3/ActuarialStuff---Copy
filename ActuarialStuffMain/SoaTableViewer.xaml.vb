Imports ActuarialLib

Public Class SoaTableViewer

    Private _Table As SoaTable

    Delegate Sub DisplayResults(ByVal table As SoaTable)
    Private dr As DisplayResults = AddressOf DisplayDescription         'Will be the tab that has focus                      

    Public Property Table As SoaTable
        Get
            Return _Table
        End Get
        Set(value As SoaTable)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))

            _Table = value
            dr.Invoke(_Table)       'Invoke the display of tab that has focus

            'textBoxDescription.Text = _Table.DisplayDescription
            'textBoxStructure.Text = _Table.DisplayStructure
            'textBoxValues.Text = _Table.DisplayValues
            'ecXML.Text = _Table.XmlText    'Need to update on tables because gotfocus is not firing for XML tabitem
            'textBoxWarnings.Text = MyJoin(vbCrLf, _Table.Warnings)

            If _Table.Warnings.Count = 0 Then
                tabItemWarnings.Foreground = New SolidColorBrush(Colors.Black)
            Else
                tabItemWarnings.Foreground = New SolidColorBrush(Colors.Red)
            End If
        End Set
    End Property

    'Update the table that has received focus
    Private Sub TabItem_GotFocus(sender As Object,
                                 e As RoutedEventArgs) Handles tabItemDescription.GotFocus,
                                                               tabItemStructure.GotFocus,
                                                               tabItemValues.GotFocus,
                                                               tabItemXML.GotFocus,
                                                               tabItemWarnings.GotFocus

        'Update the tab that has just received focus
        Select Case CType(sender, TabItem).Name
            Case "tabItemDescription" : dr = AddressOf DisplayDescription
            Case "tabItemStructure" : dr = AddressOf DisplayStructure
            Case "tabItemValues" : dr = AddressOf DisplayValues
            Case "tabItemXML" : dr = AddressOf DisplayXml
            Case "tabItemWarnings" : dr = AddressOf DisplayWarnings
            Case Else : Exit Sub
        End Select

        dr.Invoke(_Table)
    End Sub

    Private Sub DisplayDescription(ByVal table As SoaTable)
        textBoxDescription.Text = _Table.DisplayDescription
    End Sub

    Private Sub DisplayStructure(ByVal table As SoaTable)
        textBoxStructure.Text = _Table.DisplayStructure
    End Sub

    Private Sub DisplayValues(ByVal table As SoaTable)
        textBoxValues.Text = _Table.DisplayValues
    End Sub

    Private Sub DisplayXml(ByVal table As SoaTable)
        ecXML.Text = _Table.XmlText
    End Sub

    Private Sub DisplayWarnings(ByVal table As SoaTable)
        textBoxWarnings.Text = MyJoin(vbCrLf, _Table.Warnings)
        'If _Table.Warnings.Count = 0 Then
        '    tabItemWarnings.Foreground = New SolidColorBrush(Colors.Black)
        'Else
        '    tabItemWarnings.Foreground = New SolidColorBrush(Colors.Red)
        'End If
    End Sub

End Class
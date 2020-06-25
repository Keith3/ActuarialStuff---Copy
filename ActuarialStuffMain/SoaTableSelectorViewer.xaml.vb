Imports ActuarialLib

Public Class SoaTableSelectorViewer

    Private _FilteredSoaTablesRef As SoaTableCollection

    Public Sub ConnectSoaTables(ByRef filteredSoaTablesRef As SoaTableCollection)
        _FilteredSoaTablesRef = filteredSoaTablesRef
    End Sub

    'Call when changes to table list
    Public Sub ReloadTableList()
        listBoxSoaTables.ItemsSource = Nothing

        If _FilteredSoaTablesRef IsNot Nothing Then

            listBoxSoaTables.ItemsSource = _FilteredSoaTablesRef.ToArray
            TableListHeader.Text = "SOA Tables   #" & listBoxSoaTables.Items.Count
            If _FilteredSoaTablesRef.Any Then
                listBoxSoaTables.SelectedIndex = 0
            End If
        Else
            TableListHeader.Text = "SOA Tables   #0"
        End If
    End Sub

    Public Property SetColumn0Width() As Double
        Get
            Return ViewColumn0Width.Width.Value
        End Get
        Set(value As Double)
            ViewColumn0Width.Width = New GridLength(value, GridUnitType.Pixel)
        End Set
    End Property

    Private Sub MyListBox1_SelectedValueChanged(sender As Object,
                                                e As EventArgs) Handles listBoxSoaTables.SelectionChanged
        If listBoxSoaTables.SelectedItem IsNot Nothing Then
            tableViewer.Table = CType(listBoxSoaTables.SelectedItem, SoaTable)
        End If
    End Sub

End Class
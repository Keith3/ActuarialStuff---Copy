Imports ActuarialLib

Public Class SoaTableSelectorComparer

    Private _CurrentSoaTablesRef As SoaTableCollection

    Public Sub ConnectSoaTables(ByRef currentTablesRef As SoaTableCollection)
        _CurrentSoaTablesRef = currentTablesRef
    End Sub

    Public Sub ReloadTableList()
        listBoxSoaTables1.ItemsSource = Nothing
        listBoxSoaTables2.ItemsSource = Nothing

        If _CurrentSoaTablesRef IsNot Nothing Then

            listBoxSoaTables1.ItemsSource = _CurrentSoaTablesRef.ToArray
            listBoxSoaTables2.ItemsSource = _CurrentSoaTablesRef.ToArray

            tbTitle.Text = "SOA Tables   #" & _CurrentSoaTablesRef.Count

            If _CurrentSoaTablesRef.Any Then
                listBoxSoaTables1.SelectedIndex = 0
                listBoxSoaTables2.SelectedIndex = 0
            End If
        Else
            tbTitle.Text = "SOA Tables   #0"
        End If
    End Sub

    Public Property SetColumn0Width() As Double
        Get
            Return CompareColumn0Width.Width.Value
        End Get
        Set(value As Double)
            CompareColumn0Width.Width = New GridLength(value, GridUnitType.Pixel)
        End Set
    End Property

    Private Sub MyListBox1_SelectedValueChanged(sender As Object,
                                                e As EventArgs) Handles listBoxSoaTables1.SelectionChanged

        If listBoxSoaTables1.SelectedItem IsNot Nothing Then
            tableComparer.Table1 = CType(listBoxSoaTables1.SelectedItem, SoaTable)
        End If
    End Sub

    Private Sub MyListBox2_SelectedValueChanged(sender As Object,
                                                e As EventArgs) Handles listBoxSoaTables2.SelectionChanged

        If listBoxSoaTables2.SelectedItem IsNot Nothing Then
            tableComparer.Table2 = CType(listBoxSoaTables2.SelectedItem, SoaTable)
        End If
    End Sub

End Class
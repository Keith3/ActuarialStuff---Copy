Imports ActuarialLib

Public Class SoaTableFilter

    Private _AllSoaTablesRef As SoaTableCollection
    Private _FilteredSoaTablesRef As SoaTableCollection
    Private _SuspendFilter As Boolean = False

    Public Event TableFilterChanged As EventHandler

    Public Sub ConnectSoaTables(ByRef allSoaTablesRef As SoaTableCollection,
                                ByRef currentSoaTablesRef As SoaTableCollection)

        If allSoaTablesRef Is Nothing Then
            Throw New ArgumentNullException(NameOf(allSoaTablesRef))
        End If

        If currentSoaTablesRef Is Nothing Then
            Throw New ArgumentNullException(NameOf(currentSoaTablesRef))
        End If

        _AllSoaTablesRef = allSoaTablesRef
        _FilteredSoaTablesRef = currentSoaTablesRef
    End Sub

    'Refresh table listboxes after RegenerateCategory or category selection changes
    Public Sub ReloadTableList()

        lbFilteredTables.ItemsSource = Nothing

        If _FilteredSoaTablesRef IsNot Nothing Then
            lbFilteredTables.ItemsSource = _FilteredSoaTablesRef.ToArray
            tbTitle.Text = "Filtered SOA Tables   #" & lbFilteredTables.Items.Count
            If _FilteredSoaTablesRef.Any Then
                lbFilteredTables.SelectedIndex = 0
            End If
        Else
            tbTitle.Text = "Filtered SOA Tables   #0"
        End If
    End Sub

    'Regenerate Categories when table count changes
    Public Sub GenerateCategoryLists()

        clbContentType.ClearAll()
        clbKeyword.ClearAll()
        clbNation.ClearAll()
        clbStructure.ClearAll()
        clbProvider.ClearAll()
        clbWarning.ClearAll()

        For Each t In _AllSoaTablesRef
            clbContentType.AddCategory(t.ContentTypeKey)
            clbNation.AddCategory(t.NationKey)
            clbStructure.AddCategory(t.StructureKey)
            clbProvider.AddCategory(t.ProviderKey)

            If t.Warnings.Count > 0 Then
                For Each warning In t.Warnings
                    clbWarning.AddCategory(warning)
                Next
            Else
                clbWarning.AddCategory("None")
            End If

            If t.Keywords.Count > 0 Then
                For Each word In t.Keywords
                    clbKeyword.AddCategory(word)
                Next
            Else
                clbKeyword.AddCategory("None")
            End If
        Next

        clbContentType.LoadCategoryListBox()
        clbKeyword.LoadCategoryListBox()
        clbNation.LoadCategoryListBox()
        clbStructure.LoadCategoryListBox()
        clbProvider.LoadCategoryListBox()
        clbWarning.LoadCategoryListBox()

        clbContentType.Label = "Content Types " & clbContentType.Count
        clbKeyword.Label = "KeyWords " & clbKeyword.Count
        clbNation.Label = "Nations " & clbNation.Count
        clbStructure.Label = "Structures " & clbStructure.Count
        clbProvider.Label = "Providers " & clbProvider.Count
        clbWarning.Label = "Warnings " & clbWarning.Count

        _SuspendFilter = True

        clbContentType.SelectAll()
        clbKeyword.SelectAll()
        clbNation.SelectAll()
        clbStructure.SelectAll()
        clbProvider.SelectAll()
        clbWarning.SelectAll()

        _SuspendFilter = False
        RaiseEvent TableFilterChanged(Me, Nothing)
    End Sub

    Private Sub SelectedCategoriesChanged(ByVal sender As Object,
                                          ByVal e As EventArgs) Handles clbContentType.SelectedCategoriesChanged,
                                                                        clbKeyword.SelectedCategoriesChanged,
                                                                        clbNation.SelectedCategoriesChanged,
                                                                        clbProvider.SelectedCategoriesChanged,
                                                                        clbStructure.SelectedCategoriesChanged,
                                                                        clbWarning.SelectedCategoriesChanged

        If Not _SuspendFilter Then
            RaiseEvent TableFilterChanged(sender, e)
        End If
    End Sub

    Private Shared Function TableHasKeyWord(ByVal table As SoaTable,
                                            ByVal keyWords As IList(Of String)) As Boolean

        If table Is Nothing Then Throw New ArgumentNullException(NameOf(table))
        If keyWords Is Nothing Then Throw New ArgumentNullException(NameOf(keyWords))

        If table.Keywords.Count = 0 Then
            'tables do not have a None category, it is generated in categories when there are no other
            Return keyWords.Contains("None")
            'Return False
        Else
            For Each k1 In table.Keywords
                If keyWords.Contains(k1) Then Return True
            Next
        End If

        Return False
    End Function

    Private Shared Function TableHasCheckedWarning(ByVal table As SoaTable,
                                                   ByVal warnings As IList(Of String)) As Boolean

        If table Is Nothing Then Throw New ArgumentNullException(NameOf(table))
        If warnings Is Nothing Then Throw New ArgumentNullException(NameOf(warnings))

        If table.Warnings.Count = 0 Then
            Return warnings.Contains("None")
            'Return False
        Else
            For Each w1 In table.Warnings
                If warnings.Contains(w1) Then Return True
            Next
        End If

        Return False
    End Function

    Private Function SomethingChecked() As Boolean
        If clbContentType.CheckedCategories IsNot Nothing AndAlso clbContentType.CheckedCategories.Count > 0 Then
            If clbKeyword.CheckedCategories IsNot Nothing AndAlso clbKeyword.CheckedCategories.Count > 0 Then
                If clbStructure.CheckedCategories IsNot Nothing AndAlso clbStructure.CheckedCategories.Count > 0 Then
                    If clbNation.CheckedCategories IsNot Nothing AndAlso clbNation.CheckedCategories.Count > 0 Then
                        If clbProvider.CheckedCategories IsNot Nothing AndAlso clbProvider.CheckedCategories.Count > 0 Then
                            If clbWarning.CheckedCategories IsNot Nothing AndAlso clbWarning.CheckedCategories.Count > 0 Then

                                Return True

                            End If
                        End If
                    End If
                End If
            End If
        End If

        Return False
    End Function

    Public Sub FilterTables()

        If _AllSoaTablesRef Is Nothing OrElse Not _AllSoaTablesRef.Any Then Exit Sub

        _FilteredSoaTablesRef.Clear()

        If SomethingChecked() Then

            For Each t As SoaTable In _AllSoaTablesRef

                If clbContentType.CheckedCategories.Contains(t.ContentTypeKey) Then
                    If clbStructure.CheckedCategories.Contains(t.StructureKey) Then
                        If clbNation.CheckedCategories.Contains(t.NationKey) Then
                            If clbProvider.CheckedCategories.Contains(t.ProviderKey) Then
                                If TableHasKeyWord(t, clbKeyword.CheckedCategories) Then
                                    If TableHasCheckedWarning(t, clbWarning.CheckedCategories) Then

                                        _FilteredSoaTablesRef.Add(t)

                                    End If
                                End If
                            End If
                        End If
                    End If
                End If

            Next

        End If

    End Sub

End Class
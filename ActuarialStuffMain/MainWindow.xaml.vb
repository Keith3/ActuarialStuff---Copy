Imports ActuarialLib
Imports System.Globalization

Class MainWindow

    Public ReadOnly Property AllSoaTables As New SoaTableCollection
    Public ReadOnly Property FilteredSoaTables As New SoaTableCollection

    Private Sub MainWindow1_Initialized(sender As Object, e As EventArgs) Handles MainWindow1.Initialized

        'Restore window geometry
        Me.Height = Math.Max(My.Settings.WindowHeight, 50)
        Me.Width = Math.Max(My.Settings.WindowWidth, 50)
        Me.Left = Math.Max(My.Settings.WindowLeft, 50)
        Me.Top = Math.Max(My.Settings.WindowTop, 50)

        'Restore control geometry
        SoaTableSelectorViewer1.SetColumn0Width = Math.Max(My.Settings.ViewColumn0Width, 50)
        SoaTableSelectorComparer1.SetColumn0Width = Math.Max(My.Settings.CompareColumn0Width, 50)

        SoaTableFilter1.FilterColumn0Width.Width = New GridLength(Math.Max(My.Settings.FilterColumn0Width, 50), GridUnitType.Pixel)
        SoaTableFilter1.FilterColumn2Width.Width = New GridLength(Math.Max(My.Settings.FilterColumn2Width, 50), GridUnitType.Pixel)
        SoaTableFilter1.FilterCol0Row0Height.Height = New GridLength(Math.Max(My.Settings.FilterCol0Row0Height, 50), GridUnitType.Pixel)
        SoaTableFilter1.FilterCol0Row2Height.Height = New GridLength(Math.Max(My.Settings.FilterCol0Row2Height, 50), GridUnitType.Pixel)
        SoaTableFilter1.FilterCol2Row0Height.Height = New GridLength(Math.Max(My.Settings.FilterCol2Row0Height, 50), GridUnitType.Pixel)
        SoaTableFilter1.FilterCol2Row2Height.Height = New GridLength(Math.Max(My.Settings.FilterCol2Row2Height, 50), GridUnitType.Pixel)

        'Pass table references to controls
        SoaTableSelectorViewer1.ConnectSoaTables(FilteredSoaTables)
        SoaTableSelectorComparer1.ConnectSoaTables(FilteredSoaTables)
        SoaTableFilter1.ConnectSoaTables(AllSoaTables, FilteredSoaTables)

        SetWindowView("mnuTableView")

    End Sub

    Private Sub MainWindow1_Loaded(sender As Object,
                                   e As RoutedEventArgs) Handles MainWindow1.Loaded,
                                                                 mnuFileLoadSoaTables.Click

        Dim LoadDialog As New TableLoadDialog

        If LoadDialog.ShowDialog Then
            Cursor = Cursors.Wait
            Dim LoadStart = Timer

            AllSoaTables.LoadSelectedFiles(LoadDialog.TablesToLoad)

            FilterNewTables()

            Debug.Print("Table load time " & (Timer - LoadStart).ToString(CultureInfo.CurrentCulture) & " seconds")
            Cursor = Nothing
        End If

    End Sub

    Private Sub Window_Closing(sender As Object,
                               e As ComponentModel.CancelEventArgs) Handles MainWindow1.Closing

        My.Settings.WindowLeft = Me.Left
        My.Settings.WindowTop = Me.Top
        My.Settings.WindowWidth = Me.Width
        My.Settings.WindowHeight = Me.Height

        My.Settings.ViewColumn0Width = SoaTableSelectorViewer1.ViewColumn0Width.Width.Value
        My.Settings.CompareColumn0Width = SoaTableSelectorComparer1.CompareColumn0Width.Width.Value

        My.Settings.FilterColumn0Width = SoaTableFilter1.FilterColumn0Width.Width.Value
        My.Settings.FilterColumn2Width = SoaTableFilter1.FilterColumn2Width.Width.Value
        My.Settings.FilterCol0Row0Height = SoaTableFilter1.FilterCol0Row0Height.Height.Value
        My.Settings.FilterCol0Row2Height = SoaTableFilter1.FilterCol0Row2Height.Height.Value
        My.Settings.FilterCol2Row0Height = SoaTableFilter1.FilterCol2Row0Height.Height.Value
        My.Settings.FilterCol2Row2Height = SoaTableFilter1.FilterCol2Row2Height.Height.Value

        My.Settings.Save()

        AddHandler SoaTableFilter1.TableFilterChanged, AddressOf TableFilterChanged
    End Sub

    Private Sub TableFilterChanged(sender As Object,
                                   e As EventArgs)
        RefilterTables()
    End Sub

    Public Sub FilterNewTables()
        RemoveHandler SoaTableFilter1.TableFilterChanged, AddressOf TableFilterChanged
        SoaTableFilter1.GenerateCategoryLists()
        RefilterTables()
        AddHandler SoaTableFilter1.TableFilterChanged, AddressOf TableFilterChanged
    End Sub

    Public Sub RefilterTables()

        SoaTableFilter1.FilterTables()
        ReloadTableList()
    End Sub

    Private Sub ReloadTableList()
        SoaTableFilter1.ReloadTableList()
        SoaTableSelectorViewer1.ReloadTableList()
        SoaTableSelectorComparer1.ReloadTableList()
    End Sub

    Private Sub MnuFileClearAllTables_Click(sender As Object,
                                            e As RoutedEventArgs) Handles mnuFileClearAllTables.Click

        AllSoaTables.Clear()
        FilteredSoaTables.Clear()
        SoaTableFilter1.GenerateCategoryLists()
        ReloadTableList()
    End Sub

    Private Sub MenuFileCloseActuarialStuff_Click(sender As Object,
                                                  e As RoutedEventArgs) Handles mnuFileCloseActuarialStuff.Click

        Close()
    End Sub

    Private Sub MenuTableView_Click(sender As Object,
                                    e As RoutedEventArgs) Handles mnuTableView.Click,
                                                                  mnuTableFilter.Click,
                                                                  mnuTableCompare.Click

        Dim x As MenuItem = TryCast(sender, MenuItem)
        SetWindowView(x.Name)
    End Sub

    Private Sub SetWindowView(ByVal window As String)
        Select Case window
            Case "mnuTableView"
                SoaTableSelectorViewer1.Visibility = Visibility.Visible
                SoaTableFilter1.Visibility = Visibility.Hidden
                SoaTableSelectorComparer1.Visibility = Visibility.Hidden

            Case "mnuTableFilter"
                SoaTableSelectorViewer1.Visibility = Visibility.Hidden
                SoaTableFilter1.Visibility = Visibility.Visible
                SoaTableSelectorComparer1.Visibility = Visibility.Hidden

            Case "mnuTableCompare"
                SoaTableSelectorViewer1.Visibility = Visibility.Hidden
                SoaTableFilter1.Visibility = Visibility.Hidden
                SoaTableSelectorComparer1.Visibility = Visibility.Visible

            Case Else
        End Select

        mnuTableView.IsEnabled = (SoaTableSelectorViewer1.Visibility <> Visibility.Visible)
        mnuTableFilter.IsEnabled = (SoaTableFilter1.Visibility <> Visibility.Visible)
        mnuTableCompare.IsEnabled = (SoaTableSelectorComparer1.Visibility <> Visibility.Visible)
    End Sub

    Private Sub MenuTableSortByName_Click(sender As Object,
                                          e As RoutedEventArgs) Handles mnuTableSortByName.Click

        FilteredSoaTables.SortBy("Name")

        mnuTableSortByName.IsEnabled = (FilteredSoaTables.CurrentSortOrder <> "Name")
        mnuTableSortByID.IsEnabled = (FilteredSoaTables.CurrentSortOrder <> "ID")

        SoaTableSelectorViewer1.ReloadTableList()
        SoaTableFilter1.ReloadTableList()
        SoaTableSelectorComparer1.ReloadTableList()

    End Sub

    Private Sub MenuTableSortByID_Click(sender As Object,
                                        e As RoutedEventArgs) Handles mnuTableSortByID.Click

        FilteredSoaTables.SortBy("ID")

        mnuTableSortByName.IsEnabled = (FilteredSoaTables.CurrentSortOrder <> "Name")
        mnuTableSortByID.IsEnabled = (FilteredSoaTables.CurrentSortOrder <> "ID")

        SoaTableSelectorViewer1.ReloadTableList()
        SoaTableFilter1.ReloadTableList()
        SoaTableSelectorComparer1.ReloadTableList()

    End Sub

    Private Sub MnuHelpContents_Click(sender As Object,
                                      e As RoutedEventArgs) Handles mnuHelpContents.Click

    End Sub

    Private Sub MnuHelpAbout_Click(sender As Object,
                                   e As RoutedEventArgs) Handles mnuHelpAbout.Click

    End Sub

End Class
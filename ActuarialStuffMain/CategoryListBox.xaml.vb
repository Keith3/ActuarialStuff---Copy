Imports System.ComponentModel
Imports System.Globalization

Public Class CategoryListBox

    Private ReadOnly Property Categories As New List(Of CountedCategoryItem)

    Event SelectedCategoriesChanged As EventHandler

    Public Sub AddCategory(ByVal tryCategory As String)

        If tryCategory IsNot Nothing AndAlso tryCategory.Length > 0 Then
            Dim tryItem As New CountedCategoryItem(tryCategory)
            Dim ndx As Integer = Categories.IndexOf(tryItem)

            If ndx >= 0 Then
                Categories(ndx).Increment()
            Else
                Categories.Add(tryItem)
            End If
        End If

    End Sub

    Public Sub ClearAll()
        clbList.Items.Clear()
        Categories.Clear()
        RaiseEvent SelectedCategoriesChanged(Me, Nothing)
    End Sub

    Public Sub SelectAll()
        cbSelectionToggle.IsChecked = True
        clbList.SelectAll()
        RaiseEvent SelectedCategoriesChanged(Me, Nothing)
    End Sub

    Public Sub UnselectAll()
        cbSelectionToggle.IsChecked = False
        clbList.UnselectAll()
        RaiseEvent SelectedCategoriesChanged(Me, Nothing)
    End Sub

    Private Sub CbSelectionToggle_Changed(sender As Object,
                                          e As RoutedEventArgs) Handles cbSelectionToggle.Checked,
                                                                        cbSelectionToggle.Unchecked
        If clbList.Items.Count > 0 Then
            If cbSelectionToggle.IsChecked Then
                clbList.SelectAll()
            Else
                clbList.UnselectAll()
            End If
        End If

    End Sub

    Private Sub ClbList_SelectionChanged(sender As Object,
                                         e As SelectionChangedEventArgs) Handles clbList.SelectionChanged

        RaiseEvent SelectedCategoriesChanged(sender, e)
    End Sub

    Public Sub LoadCategoryListBox()
        Categories.Sort()

        clbList.Items.Clear()
        For Each cat In Categories
            clbList.Items.Add(cat)
        Next

        clbList.SelectAll()
    End Sub

    Public Function Count() As Integer
        Return Categories.Count
    End Function

    <Browsable(True), Category("My Properties")> Public Property Label As String
        Set(value As String)
            tbText.Text = value
        End Set
        Get
            Return tbText.Text
        End Get
    End Property

    Public ReadOnly Property CheckedCategories() As IList(Of String)
        Get
            Dim items As New List(Of String)

            For Each item As CountedCategoryItem In clbList.SelectedItems
                items.Add(item.Category)
            Next

            Return items
        End Get
    End Property

    Private Class CountedCategoryItem

        Implements IEquatable(Of CountedCategoryItem)
        Implements IComparable(Of CountedCategoryItem)

        Public ReadOnly Property Category As String
        Public ReadOnly Property Count As Integer

        Private Sub New()
        End Sub

        Public Sub New(ByVal catName As String)
            _Category = catName
            _Count = 1
        End Sub

        Public Sub Increment()
            _Count += 1
        End Sub

        Public Overrides Function ToString() As String
            Return _Category & " #" & _Count.ToString(CultureInfo.CurrentCulture)
        End Function

        Public Overloads Function Equals(other As CountedCategoryItem) As Boolean Implements IEquatable(Of CountedCategoryItem).Equals
            Return (other IsNot Nothing) AndAlso (_Category = other.Category)
        End Function

        Public Overloads Shared Operator =(ByVal item1 As CountedCategoryItem,
                                           ByVal item2 As CountedCategoryItem) As Boolean

            Return (item1 IsNot Nothing) AndAlso (item2 IsNot Nothing) AndAlso (item1.Category = item2.Category)
        End Operator

        Public Overloads Shared Operator <>(ByVal item1 As CountedCategoryItem,
                                            ByVal item2 As CountedCategoryItem) As Boolean

            Return (item1 IsNot Nothing) AndAlso (item2 IsNot Nothing) AndAlso (item1.Category <> item2.Category)
        End Operator

        Public Function CompareTo(other As CountedCategoryItem) As Integer Implements IComparable(Of CountedCategoryItem).CompareTo
            If other Is Nothing Then
                Throw New ArgumentNullException(NameOf(other))
            Else
                Return _Category.CompareTo(other.Category)
            End If
        End Function

    End Class

End Class
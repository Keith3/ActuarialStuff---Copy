Public Class CategoryList

    Private ReadOnly Property Categories As SortedList(Of String, Integer)

    Public Sub AddCategory(ByVal tryCategory As String)

        If tryCategory IsNot Nothing AndAlso tryCategory.Length > 0 Then
            Try
                Categories.Item(tryCategory) = Categories.Item(tryCategory) + 1
            Catch ex As KeyNotFoundException
                Categories.Item(tryCategory) = 1
            End Try
        End If

    End Sub

    Public Sub Clear()
        Categories.Clear()
    End Sub

    Public ReadOnly Property CategoryCount As Integer
        Get
            Return Categories.Count
        End Get
    End Property

    Public ReadOnly Property ItemCount As Integer
        Get
            Dim total As Integer = 0
            For Each cat In Categories
                total += cat.Value
            Next
            Return total
        End Get
    End Property

End Class
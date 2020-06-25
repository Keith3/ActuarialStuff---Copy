Public Class SortedCountedSet(Of T As {IComparable(Of T), IEquatable(Of T)})

    Implements IEnumerable(Of SortableCountableItem(Of T))

    Private _MySet As New SortedSet(Of SortableCountableItem(Of T))

    Public Sub Add(ByVal TryItem As T)
        If TryItem IsNot Nothing Then
            Dim AddItem As New SortableCountableItem(Of T)(TryItem)
            For Each i In From item1 In _MySet Where item1.Equals1(AddItem)
                i.IncrementCount()
                Exit Sub
            Next
            _MySet.Add(AddItem)
        End If
    End Sub

    Default Public ReadOnly Property ItemValue(ByVal index As Integer) As T
        Get
            Return _MySet(index).Value
        End Get
    End Property

    Public ReadOnly Property ItemCount(ByVal index As Integer) As Integer
        Get
            Return _MySet(index).Count
        End Get
    End Property

    Public ReadOnly Property Count As Integer
        Get
            Return _MySet.Count
        End Get
    End Property

    Public Sub IncrementItem(ByVal TryItem As T)
        Dim i As Integer = _MySet.ToList.IndexOf(New SortableCountableItem(Of T)(TryItem))
        _MySet(i).IncrementCount()
    End Sub

    Public Sub ClearSet()
        _MySet.Clear()
    End Sub

    Public Sub ClearCounts()
        For Each i In _MySet
            i.ClearCount()
        Next
    End Sub

    Public Function ToValueArray() As T()
        Dim newArray(_MySet.Count - 1) As T

        For i = 0 To _MySet.Count - 1
            newArray(i) = _MySet(i).Value
        Next

        Return newArray
    End Function

    Public Function ToStringArray() As String()
        Dim newArray(_MySet.Count - 1) As String

        For i = 0 To _MySet.Count - 1
            newArray(i) = _MySet(i).Value.ToString & " #" & _MySet(i).Count.ToString
        Next

        Return newArray
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of SortableCountableItem(Of T)) Implements IEnumerable(Of SortableCountableItem(Of T)).GetEnumerator
        For Each i In _MySet
            Yield i
        Next
    End Function

    Public Iterator Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Yield GetEnumerator()
    End Function

End Class
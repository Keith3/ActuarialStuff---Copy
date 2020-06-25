Public Class SortableCountableItem(Of T As {IComparable(Of T), IEquatable(Of T)})

	Implements IComparable(Of SortableCountableItem(Of T))
	Implements IEquatable(Of SortableCountableItem(Of T))

	Private _Item As T
	Private _Count As Integer = 1

	Private Sub New()
	End Sub

	Public Sub New(ByVal newItem As T)
		_Item = newItem
	End Sub

	Public ReadOnly Property Count As Integer
		Get
			Return _Count
		End Get
	End Property

    Public ReadOnly Property Value As T
        Get
            Return _Item
        End Get
    End Property

    Public Sub IncrementCount()
        _Count += 1
    End Sub

    Public Sub ClearCount()
		_Count = 0
	End Sub

	Public Overrides Function ToString() As String
		Return _Item.ToString & " #" & _Count.ToString
	End Function

	Public Function Equals1(other As SortableCountableItem(Of T)) As Boolean Implements IEquatable(Of SortableCountableItem(Of T)).Equals
		Return _Item.Equals(other.Value)
	End Function

	Public Function CompareTo(other As SortableCountableItem(Of T)) As Integer Implements IComparable(Of SortableCountableItem(Of T)).CompareTo
		Return _Item.CompareTo(other.Value)
	End Function

End Class
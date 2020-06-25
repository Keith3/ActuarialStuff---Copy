Public Class JaggedKeyedArray(Of TKey1 As IComparable,
                                 TKey2 As IComparable,
                                 TValue)

    Private ReadOnly dictKKT As New Dictionary(Of TKey1, Dictionary(Of TKey2, TValue))

    Public Function IsOneDimensional() As Boolean
        Return dictKKT.Keys.Count = 1 AndAlso dictKKT.Keys.First.ToString.Length = 0
    End Function

    Public Overloads Sub Add(ByVal key1 As TKey1,
                             ByVal key2 As TKey2,
                             ByVal value As TValue)

        If key1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(key1))
        ElseIf key2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(key2))
        ElseIf value Is Nothing Then
            Throw New ArgumentNullException(NameOf(value))
        Else
            dictKKT.Add(key1, New Dictionary(Of TKey2, TValue) From {{key2, value}})
        End If
    End Sub

    Default Public Overloads Property Item(ByVal key1 As TKey1,
                                           ByVal key2 As TKey2) As TValue
        Get
            If key1 Is Nothing Then
                Throw New ArgumentNullException(NameOf(key1))
            ElseIf key2 Is Nothing Then
                Throw New ArgumentNullException(NameOf(key2))
            ElseIf Not dictKKT.ContainsKey(key1) Then
                Throw New ArgumentOutOfRangeException(NameOf(key1))
            ElseIf Not dictKKT(key1).ContainsKey(key2) Then
                Throw New ArgumentOutOfRangeException(NameOf(key2))
            Else
                Return dictKKT(key1)(key2)
            End If
        End Get
        Set(value As TValue)
            If key1 Is Nothing Then
                Throw New ArgumentNullException(NameOf(key1))
            ElseIf key2 Is Nothing Then
                Throw New ArgumentNullException(NameOf(key2))
            ElseIf value Is Nothing Then
                Throw New ArgumentNullException(NameOf(value))
            ElseIf dictKKT.ContainsKey(key1) Then
                dictKKT(key1)(key2) = value
            Else
                Dim dictKT As New Dictionary(Of TKey2, TValue)
                dictKT(key2) = value
                dictKKT(key1) = dictKT
            End If
        End Set
    End Property

    Public Function Items(ByVal key1 As TKey1) As TValue()
        Return dictKKT(key1).Values.ToArray
    End Function

    Public Function Keys1() As TKey1()
        Return dictKKT.Keys.ToArray
    End Function

    Public Function Keys2(ByVal key1 As TKey1) As TKey2()
        If key1 Is Nothing Then Throw New ArgumentNullException(NameOf(key1))
        Return dictKKT(key1).Keys.ToArray
    End Function

    Public Overloads Sub Clear()
        For Each k In dictKKT.Keys
            dictKKT(k).Clear()
            dictKKT.Clear()
        Next
    End Sub

End Class
Public Class SFSynchronizedEnumerator(Of T)

    'Synchronized enumeration thru multiple StepFunctions

    Private ReadOnly StepFunctionList As New List(Of StepFunction(Of T))
    Private EnumeratorList As List(Of IEnumerator(Of FunctionStep(Of T)))

    'Public Property ForwardEnumeration As Boolean = True
    'Public Property IssueAge As Integer = 0
    Public ReadOnly Property CurrentOverlap As Range(Of Double)

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub Add(ByVal stepFunction As StepFunction(Of T))
        If stepFunction IsNot Nothing AndAlso stepFunction.Count > 0 Then
            StepFunctionList.Add(stepFunction)
        End If
    End Sub

    Public Function GetEnumerators(ByVal issueAge As Integer, ByVal forwardEnumeration As Boolean) As Boolean

        _CurrentOverlap = New Range(Of Double)(0, Double.MaxValue)

        EnumeratorList = New List(Of IEnumerator(Of FunctionStep(Of T)))

        'GetEnumerators and set on first item
        For i = 0 To StepFunctionList.Count - 1
            StepFunctionList(i).ForwardEnumeration = forwardEnumeration
            StepFunctionList(i).IssueAge = issueAge

            EnumeratorList.Add(StepFunctionList(i).GetEnumerator)
            'Abandon if positioning on first item fails
            If Not EnumeratorList(i).MoveNext Then Return False
            _CurrentOverlap = _CurrentOverlap.Overlap(EnumeratorList(i).Current.Period)
            'StepFunctionList(i).Current = EnumeratorList(i).Current
        Next

        Return True

    End Function

    'Public Function MoveNext() As Boolean

    '    If _ForwardEnumeration Then

    '        For i = 0 To StepFunctionList.Count - 1
    '            If EnumeratorList(i).Current.Period.MaxValue = _Overlap.MaxValue Then
    '                If Not EnumeratorList(i).MoveNext() Then Return False
    '            End If
    '            'StepFunctionList(i).Current = EnumeratorList(i).Current
    '        Next

    '        _Overlap = New Range(Of Double)(0, Double.MaxValue)

    '        For i = 0 To StepFunctionList.Count - 1
    '            _Overlap = _Overlap.Overlap(EnumeratorList(i).Current.Period)
    '        Next

    '    Else

    '        For Each sf In StepFunctionList
    '            If sf.Enumerator.Current.Period.MinValue = _Overlap.MinValue Then
    '                If Not sf.Enumerator.MoveNext() Then Return False
    '            End If
    '        Next

    '        _Overlap = New Range(Of Double)(0, Double.MaxValue)

    '        For Each sf In StepFunctionList
    '            _Overlap = _Overlap.Overlap(sf.Enumerator.Current.Period)
    '        Next

    '    End If

    '    Return True

    'End Function

End Class
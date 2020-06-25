Public Class StepFunction(Of T)
    Implements IEnumerable(Of FunctionStep(Of T))

    Private Structure DataPoint
        Public DurationOrAge As Double
        Public IsDuration As Boolean
        Public Content As T
    End Structure

    Private ReadOnly dataPoints As New List(Of DataPoint)

    Public Property ForwardEnumeration As Boolean = True
    Public Property IssueAge As Integer = 0

    Public Sub Add(ByVal contentValue As T,
                   ByVal durationOrAge As Double,
                   ByVal isDuration As Boolean)

        If contentValue IsNot Nothing AndAlso durationOrAge >= 0 Then
            Dim dp As DataPoint

            dp.DurationOrAge = durationOrAge
            dp.IsDuration = isDuration
            dp.Content = contentValue
            dataPoints.Add(dp)
        End If
    End Sub

    Public Sub Add(ByVal contentValue As T,
                   ByVal toDuration As Double)

        Add(contentValue, toDuration, True)
    End Sub

    Public Function Count() As Integer
        Return dataPoints.Count
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of FunctionStep(Of T)) Implements IEnumerable(Of FunctionStep(Of T)).GetEnumerator

        Dim currDur As Double
        Dim prevDur As Double = 0

        If _ForwardEnumeration Then

            For index As Integer = 0 To Count() - 1
                With dataPoints(index)
                    currDur = If(.IsDuration, .DurationOrAge, .DurationOrAge - IssueAge)
                    If currDur >= prevDur Then
                        Dim s As New FunctionStep(Of T)(prevDur, currDur, .Content)
                        Yield s
                        prevDur = currDur
                    End If
                End With
            Next

        Else      'Use stack to reverse order of items

            Dim steps As New Stack(Of FunctionStep(Of T))(Count() - 1)

            For index As Integer = 0 To Count() - 1
                With dataPoints(index)
                    currDur = If(.IsDuration, .DurationOrAge, .DurationOrAge - IssueAge)
                    If currDur >= prevDur Then
                        steps.Push(New FunctionStep(Of T)(prevDur, currDur, .Content))
                        prevDur = currDur
                    End If
                End With
            Next

            For Each s In steps
                Yield s
            Next

        End If
    End Function

    Private Iterator Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Yield GetEnumerator()
    End Function

End Class
Public Class MortalityTable

    Public ReadOnly Property Name As String
    Public ReadOnly Property Id As Integer
    Public ReadOnly Property IssueAgeRange As Range(Of Integer)
    Private ReadOnly qs()() As Double

    Public Sub New(ByVal table As SoaTable)
        If table Is Nothing Then Exit Sub

        'Dim maxDuration As Integer
        'Dim subTable0 As SoaTableSubTable = table.SubTables(0)

        '_Name = table.TableName
        '_Id = table.TableIdentity
        'Dim xUpperBound As Integer = subTable0.Values.GetUpperBound(0)
        'Dim minIssueAge = subTable0.AxisDef(0).MinScaleValue
        '_IssueAgeRange = New Range(Of Integer)(minIssueAge, xUpperBound + minIssueAge)
        'ReDim qs(xUpperBound)

        'Select Case table.StructureKey
        '    Case "A"
        '        For i = 0 To xUpperBound
        '            maxDuration = xUpperBound - i
        '            ReDim qs(i)(maxDuration)
        '            For d = 0 To maxDuration
        '                qs(i)(d) = subTable0.Values(d, 0)
        '            Next
        '        Next
        '    Case "(AD)A"
        '        Dim maxSelectDuration As Integer = subTable0.Values.GetUpperBound(1)
        '        Dim subTable1 As SoaTableSubTable = table.SubTables(1)
        '        Dim maxAttainedAge As Integer = subTable1.Values.GetUpperBound(0) + subTable1.AxisDef(0).MinScaleValue

        '        For i = 0 To xUpperBound
        '            maxDuration = xUpperBound - i
        '            ReDim qs(i)(maxDuration)
        '            For d = 0 To maxSelectDuration
        '                qs(i)(d) = subTable0.Values(i, d)
        '            Next
        '            For d = maxSelectDuration + 1 To maxDuration
        '                qs(i)(d) = subTable1.Values(d, 0)
        '            Next
        '        Next
        '    Case Else
        'End Select
    End Sub

    'Public Function DeathRates(ByVal issueAge As Integer) As StepFunction
    '    If Not _IssueAgeRange.IsInRange(issueAge) Then Return Nothing
    '    Return qs(issueAge - _IssueAgeRange.MinValue)
    'End Function

    'Public Shared Function MFq(ByVal qx As Double,
    '                           ByVal sFixed As Double) As MFSummation

    '    Dim x As New MFSummation
    '    x.AddProduct(1)
    '    x.SubtractProduct(MFp(qx, sFixed))
    '    Return x
    'End Function

    'Public Function SurvivalRates(ByVal issueAge As Integer) As StepFunction
    '    Dim values() As Double = DeathRates(issueAge)
    '    If values Is Nothing Then Return Nothing
    '    Dim stepOut As New StepFunction()
    '    Dim px As Double
    '    For dur = 0 To values.Count - 1
    '        px = 1 - values(dur)
    '        stepOut.Add(New MFExponential(px, 1), dur + 1)
    '    Next
    '    Return stepOut
    'End Function

    'Public Shared Function SurvivalProbabilityConstantForce(ByVal qx As Double,
    '                                                        ByVal sFixed As Double) As MFExponential

    '    Dim px As Double = 1 - qx
    '    Return New MFExponential(px, px ^ -(sFixed))
    'End Function

    'Public Function ForceOfMortality(ByVal issueAge As Integer) As Double()
    '    Dim values() As Double = q(issueAge - _MinIssueAge)
    '    If values IsNot Nothing Then
    '        For d = 0 To values.Count - 1
    '            values(d) = -Math.Log(1 - values(d))
    '        Next
    '        Return values
    '    Else
    '        Return Nothing
    '    End If
    'End Function

    Public Function LifeExpectancy(ByVal issueAge As Integer) As Double?
        If Not _IssueAgeRange.IsInRange(issueAge) Then Return Nothing

        Dim qx() As Double = qs(issueAge - _IssueAgeRange.MinValue)
        If qx Is Nothing Then Return Nothing

        Dim expect As Double = 0

        For d = qx.Count - 1 To 0 Step -1
            expect = (expect + 1) * (1 - qx(d))
        Next

        Return expect
    End Function

    'Public Function LifeExpectancyComplete(ByVal issueAge As Integer) As Double?
    '    If Not _IssueAgeRange.IsInRange(issueAge) Then Return Nothing

    '    Dim qx() As Double = qs(issueAge - _IssueAgeRange.MinValue)
    '    If qx Is Nothing Then Return Nothing

    '    Dim expect As Double = 0

    '    For d = qx.Count - 1 To 0 Step -1
    '        expect = expect * (1 - qx(d)) + SurvivalProbabilityConstantForce(qx(d), 0).DefiniteIntegral(0, 1)
    '    Next

    '    Return expect
    'End Function

End Class
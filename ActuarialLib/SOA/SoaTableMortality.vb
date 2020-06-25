'Public Class SoaTableMortality
'    Inherits SoaTableDecrement

'    Private saveIssueAge As Integer = -1
'    Private saveq() As Double

'    Public Sub New(ByVal xmlTable As XElement)
'        MyBase.New(xmlTable)
'    End Sub

'    Public Function q(ByVal issueAge As Integer) As Double()

'        If issueAge >= 0 Then
'            If issueAge <> saveIssueAge Then
'                saveIssueAge = -1
'                Select Case StructureKey
'                    Case "A"
'                        Dim initAge As Integer = issueAge - SubTables(0).AxisInfo(0).MinScaleValue
'                        Dim maxAge As Integer = SubTables(0).Values.GetUpperBound(0)
'                        ReDim saveq(maxAge - initAge)
'                        For a = initAge To maxAge
'                            saveq(a - initAge) = CType(SubTables(0).Values(a, 0), Double)
'                        Next
'                    Case "(AD)A"
'                        Dim maxDur As Integer = SubTables(0).Values.GetUpperBound(1)
'                        Dim minAge As Integer = issueAge + maxDur - SubTables(1).AxisInfo(0).MinScaleValue
'                        Dim maxAge As Integer = SubTables(1).Values.GetUpperBound(0)
'                        ReDim saveq(maxDur + maxAge - minAge + 1)
'                        For d = 0 To maxDur
'                            saveq(d) = CType(SubTables(0).Values(issueAge, d), Double)
'                        Next
'                        For a = minAge To maxAge
'                            saveq(a + maxDur - minAge + 1) = CType(SubTables(1).Values(a, 0), Double)
'                        Next
'                    Case Else
'                End Select
'                saveIssueAge = issueAge
'            End If
'            Return saveq
'        Else
'            Throw New Exception("Invalid Issue Age " & issueAge.ToString)
'        End If
'    End Function

'    ''' <summary>
'    ''' tQxs: x = issueAge + completedYears and s = fromDuration.
'    ''' Probability of death between age x + s and x + t where variable
'    ''' t ranges from s to 1.
'    ''' </summary>
'    ''' <param name="issueAge">Age at time of issue.</param>
'    ''' <param name="completedYears">Number of completed years since underwriting.</param>
'    ''' <param name="fromDuration">Partial year since last anniversry.</param>
'    ''' <returns>A step containing the math function f(t) returning q.</returns>
'    Public Function q(ByVal issueAge As Integer,
'                      ByVal completedYears As Integer,
'                      ByVal fromDuration As Double) As SFStep

'        Dim tQxs As New MFSummation()
'        tQxs.AddProduct(1, New MFPolynomial(1), New MFExponential(1, 1))
'        tQxs.SubtractProduct(-1, New MFPolynomial(1), CType(p(issueAge, completedYears, fromDuration).Y, MFExponential))
'        Return New SFStep(fromDuration, 1, tQxs)
'    End Function


'    ''' <summary>
'    ''' tPxs: x = issueAge + completedYears and s = fromDuration.
'    ''' Probability of surviving from age x + s to x + t where variable
'    ''' t ranges from s to 1.
'    ''' </summary>
'    ''' <param name="issueAge">Age at time of issue.</param>
'    ''' <param name="completedYears">Number of completed years since underwriting.</param>
'    ''' <param name="fromDuration">Partial year since last anniversry.</param>
'    ''' <returns>A step containing the math function f(t) returning p.</returns>
'    Public Function p(ByVal issueAge As Integer,
'                      ByVal completedYears As Integer,
'                      ByVal fromDuration As Double) As SFStep

'        Dim px As Double = 1 - q(issueAge)(completedYears)
'        Return New SFStep(fromDuration, 1, New MFExponential(px, px ^ -fromDuration))
'    End Function

'    ''' <summary>
'    ''' FOMxs: x = issueAge + completedYears.
'    ''' </summary>
'    ''' <param name="issueAge">Age at time of issue.</param>
'    ''' <param name="completedYears">Number of completed years since underwriting.</param>
'    ''' <returns>Force of mortality at x + s where variable s ranges from 0 to 1.</returns>
'    Public Function FOM(ByVal issueAge As Integer,
'                        ByVal completedYears As Integer) As SFStep

'        Dim px As Double = 1 - q(issueAge)(completedYears)
'        Return New SFStep(0, 1, New MFPolynomial(-Math.Log(px)))
'    End Function

'    ''' <summary>
'    ''' sPxFOMxs: x = issueAge + completedYears.
'    ''' </summary>
'    ''' <param name="issueAge">Age at time of issue.</param>
'    ''' <param name="completedYears">Number of completed years since underwriting.</param>
'    ''' <returns>Force of mortality at x + s times sPx where variable s ranges from 0 to 1.</returns>
'    Public Function PxFOM(ByVal issueAge As Integer,
'                          ByVal completedYears As Integer) As SFStep

'        Dim px As Double = 1 - q(issueAge)(completedYears)
'        Return New SFStep(0, 1, New MFExponential(px, -Math.Log(px)))
'    End Function

'    'Public Function e(ByVal issueAge As Integer, ByVal completedYears As Integer) As StepFunction
'    'End Function

'    'Public Function eComplete(ByVal issueAge As Integer, ByVal duration As Integer) As StepFunction
'    'End Function

'End Class
'Module PV

'Public Function PVDB(ByVal state As IssueState,
'                     ByVal distribution As DistributionOfDecrements,
'                     ByVal v As StepFunctionOfTime(Of Double),
'                     ByVal qDeathDecrement As SoaTableDecrement,
'                     ByVal qDeathSurvival As SoaTableDecrement,
'                     ByVal qLapse As SoaTableDecrement,
'                     ByVal deathBenefit As StepFunctionOfTime(Of MathFunctionOfDuration),
'                     ByVal endowments As StepFunctionOfTime(Of Double)) _
'                     As StepFunctionOfDuration(Of Double)

'		Dim Overlap As DurationSpan
'		Dim vFofDur As StepFunctionOfDuration(Of Double) = v.GetFunctionOfDuration(state)
'		Dim qDeathDecrementFofDur As StepFunctionOfDuration(Of Decrement) = qDeathDecrement.Decrements(state, distribution)
'		Dim qDeathSurvivalFofDur As StepFunctionOfDuration(Of Decrement) = qDeathSurvival.Decrements(state, distribution)
'		Dim qLapseFofDur As StepFunctionOfDuration(Of Decrement) = qLapse.Decrements(state, distribution)
'		Dim deathBenefitFofDur As StepFunctionOfDuration(Of MathFunctionOfDuration) = deathBenefit.GetFunctionOfDuration(state)
'		Dim endowmentsFofDur As StepFunctionOfDuration(Of Double) = endowments.GetFunctionOfDuration(state)

'		vFofDur.ResetEnd()
'		qDeathDecrementFofDur.ResetEnd()
'		qDeathSurvivalFofDur.ResetEnd()
'		qLapseFofDur.ResetEnd()
'		deathBenefitFofDur.ResetEnd()
'		endowmentsFofDur.ResetEnd()

'		'Position All @ last DB
'		deathBenefitFofDur.MoveNext()
'		Dim MaxDur As Duration = deathBenefitFofDur.DurationSpan.Duration2

'		vFofDur.MoveTo(MaxDur)
'		qDeathDecrementFofDur.MoveTo(MaxDur)
'		qDeathSurvivalFofDur.MoveTo(MaxDur)
'		qLapseFofDur.MoveTo(MaxDur)
'		endowmentsFofDur.MoveTo(MaxDur)

'		Dim CurrentDB As MathFunctionOfDuration = deathBenefitFofDur.CurrentValue
'		Dim vCurrent As Double = vFofDur.CurrentPoint.Value
'		Dim PV As Double = 0
'		Dim PVOut As New StepFunctionOfDuration(Of Double)

'		Do
'			'Calc Overlap
'			Overlap = vFofDur.DurationSpan
'			Overlap = DurationSpan.Overlap(Overlap, qDeathDecrementFofDur.DurationSpan)
'			Overlap = DurationSpan.Overlap(Overlap, qDeathSurvivalFofDur.DurationSpan)
'			Overlap = DurationSpan.Overlap(Overlap, qLapseFofDur.DurationSpan)
'			Overlap = DurationSpan.Overlap(Overlap, deathBenefitFofDur.DurationSpan)
'			Overlap = DurationSpan.Overlap(Overlap, endowmentsFofDur.DurationSpan)

'			'Calc PV
'			Dim OverDur1 As Double = Overlap.Duration1.Value
'			Dim vSpan As Double = vCurrent ^ Overlap.Span
'			Dim voft As New MathFunctionOfDurationExponential(vCurrent, vCurrent ^ OverDur1)
'			Dim poft As MathFunctionOfDuration = qDeathSurvivalFofDur.CurrentValue.p(OverDur1)
'			Dim forceoft As MathFunctionOfDuration = qDeathDecrementFofDur.CurrentValue.p(OverDur1)

'			If endowmentsFofDur.DurationSpan.Duration2 = Overlap.Duration2 Then
'				PV += endowmentsFofDur.CurrentValue
'			End If
'			PV *= vSpan * poft.ValueAt(Overlap.Duration1.Value)
'			PV += CurrentDB.Times(voft).Times(poft).Times(forceoft).DefiniteIntegral(OverDur1, Overlap.Duration2.Value)

'			'Get Next Slice
'			If vFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				vFofDur.MoveNext()
'				vCurrent = vFofDur.CurrentPoint.Value
'			End If

'			If qDeathDecrementFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				qDeathDecrementFofDur.MoveNext()
'			End If

'			If qDeathSurvivalFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				qDeathSurvivalFofDur.MoveNext()
'			End If

'			If qLapseFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				qLapseFofDur.MoveNext()
'			End If

'			If deathBenefitFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				deathBenefitFofDur.MoveNext()
'				CurrentDB = deathBenefitFofDur.CurrentValue
'			End If

'			If endowmentsFofDur.DurationSpan.Duration1 = Overlap.Duration1 Then
'				endowmentsFofDur.MoveNext()
'			End If

'		Loop Until Overlap.Duration1.Value = 0

'		Return PVOut
'End Function

'End Module
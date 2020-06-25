'Public Class UWClass

'    Public ReadOnly Property Name As String
'    Public ReadOnly Property Abbreviation As String
'    Public ReadOnly Property IssueAgeRange As Range(Of Integer)
'    Public ReadOnly Property NonforfeitureBasis As ActuarialBasis
'    Public ReadOnly Property SAPBasis As ActuarialBasis
'    Public ReadOnly Property TaxBasis As ActuarialBasis
'    Public ReadOnly Property GAAPBasis As ActuarialBasis
'    Public ReadOnly Property PremiumBasis As ActuarialBasis
'    Public ReadOnly Property ExperienceBasis As ActuarialBasis

'    Public Sub New(ByVal name As String,
'                   ByVal abbreviation As String,
'                   ByVal issueAgeRange As Range(Of Integer),
'                   ByVal nonforfeitureBasis As ActuarialBasis,
'                   ByVal SAPBasis As ActuarialBasis,
'                   ByVal TaxBasis As ActuarialBasis,
'                   ByVal GAAPBasis As ActuarialBasis,
'                   ByVal premiumBasis As ActuarialBasis,
'                   ByVal experienceBasis As ActuarialBasis)

'        _Name = name
'        _Abbreviation = abbreviation
'        _IssueAgeRange = issueAgeRange
'        _NonforfeitureBasis = nonforfeitureBasis
'        _SAPBasis = SAPBasis
'        _TaxBasis = TaxBasis
'        _GAAPBasis = GAAPBasis
'        _PremiumBasis = premiumBasis
'        _ExperienceBasis = experienceBasis
'    End Sub

'End Class

'Public Class ActuarialBasis

'    Public ReadOnly Property SurvivalMortality As SoaTableMortality
'    Public ReadOnly Property TriggerMortality As SoaTableMortality
'    Public ReadOnly Property LapseRate As SoaTable
'    Public ReadOnly Property InterestRate As StepFunction
'    'Expenses

'    Public Sub New(ByVal survivalMortality As SoaTableMortality,
'                   ByVal triggerMortality As SoaTableMortality,
'                   ByVal lapseRate As SoaTable,
'                   ByVal interestRate As StepFunction)

'        _SurvivalMortality = survivalMortality
'        If triggerMortality Is Nothing Then
'            _TriggerMortality = survivalMortality
'        Else
'            _TriggerMortality = triggerMortality
'        End If
'        _LapseRate = lapseRate
'        _InterestRate = interestRate
'    End Sub

'    Public Sub New(ByVal survivalMortality As SoaTableMortality,
'                   ByVal interestRate As StepFunction)

'        Me.New(survivalMortality, survivalMortality, Nothing, interestRate)
'    End Sub

'End Class
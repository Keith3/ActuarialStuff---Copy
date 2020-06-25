'Public Class InsurancePlanTraditionalLife

'    Public Shared ReadOnly UWClasses() As UWClass = {New UWClass("Male Standard",
'                                                                 "MS",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing),
'                                                     New UWClass("Female Standard",
'                                                                 "FS",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing),
'                                                     New UWClass("Male Non-Smoker",
'                                                                 "MNS",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing),
'                                                     New UWClass("Female Non-Smoker",
'                                                                 "FNS",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing),
'                                                     New UWClass("Male Preferred",
'                                                                 "MP",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing),
'                                                     New UWClass("Female Preferred",
'                                                                 "FP",
'                                                                 New Range(Of Integer)(0, 64),
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing,
'                                                                 Nothing)}
'    'Public Enum PremiumMode
'    '    Annual = 0
'    '    SemiAnnual
'    '    Quarterly
'    '    Monthly
'    '    ABC
'    'End Enum

'    'Public Shared ReadOnly ModeAdjustmenFactors() As Double = {1.0#, 0.52#, 0.26#, 0.09#, 1.0# / 12.0#}

'    'Public ReadOnly Property PlanKey As Long

'    Public Sub New()
'        '_PlanKey=
'    End Sub

'    Public Function IssuePolicy(ByVal deathBenefitUnit As StepFunction,
'                                ByVal endowment As Boolean,
'                                ByVal uwClass As UWClass,
'                                ByVal insured As InsuredPerson,
'                                ByVal units As Double,
'                                ByVal issueDate As Date) As InsurancePolicyTraditionalLife

'        'Validate here.  Throw any exceptions here rather than in Sub New.

'        Dim issueAge As Integer = CInt(((issueDate.Year - insured.BirthDate.Year) * 372& + (issueDate.Month - insured.BirthDate.Month) * 31& + issueDate.Day - insured.BirthDate.Day + 186) \ 372&)
'        'Dim issue As New IssueInfo(issueDate, issueAge)
'        Dim premium As New StepFunction()

'        'calculate premium rates

'        Return New InsurancePolicyTraditionalLife(deathBenefitUnit, endowment, uwClass, insured, issueDate, issueAge, units)
'    End Function

'    Public Class InsurancePolicyTraditionalLife

'        Public ReadOnly Property DeathBenefitUnit As StepFunction
'        Public ReadOnly Property Endowment As Boolean
'        Public ReadOnly Property UWClass As UWClass
'        Public ReadOnly Property Insured As InsuredPerson
'        Public ReadOnly Property IssueDate As Date
'        Public ReadOnly Property IssueAge As Integer
'        Public ReadOnly Property Units As Double

'        Private Sub New()
'        End Sub

'        Public Sub New(ByVal deathBenefitUnit As StepFunction,
'                       ByVal endowment As Boolean,
'                       ByVal uwClass As UWClass,
'                       ByVal insured As InsuredPerson,
'                       ByVal issueDate As Date,
'                       ByVal issueAge As Integer,
'                       ByVal units As Double)

'            _DeathBenefitUnit = deathBenefitUnit
'            _Endowment = endowment
'            _UWClass = uwClass
'            _Insured = insured
'            _IssueDate = issueDate
'            _IssueAge = issueAge
'            _Units = units
'        End Sub

'        'Public Shared Function PVFB() As StepFunction

'        'End Function

'        'Public Shared Function PVFP() As StepFunction

'        'End Function

'        'Public Function CashValue() As StepFunction

'        'End Function
'        'Public Function SAPReserve() As StepFunction

'        'End Function
'        'Public Function TaxReserve() As StepFunction

'        'End Function
'        'Public Function GAAPReserve() As StepFunction

'        'End Function
'        'Public Function AssetShare() As StepFunction

'        'End Function
'        'Public Function AnnualPremium() As Double

'        'End Function
'    End Class
'End Class
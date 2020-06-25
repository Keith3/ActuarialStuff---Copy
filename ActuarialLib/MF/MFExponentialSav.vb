Public Class MFExponential
    Inherits MFBase

    'a*b^x          

    Public ReadOnly Property Multiple As Double
    Public ReadOnly Property Base As Double

    Private Sub New()
    End Sub

    Public Sub New(ByVal base As Double,
                   ByVal multiple As Double)

        _Base = base
        _Multiple = multiple
        If base < 0 Then Throw New ArgumentOutOfRangeException("base")
    End Sub

    Public Overloads Shared Function XIsValid(xValue As Double) As Boolean
        Return True
    End Function

    Public Overloads Shared Function XRangeIsValid(xRange As Range(Of Double)) As Boolean
        Return True
    End Function

    Public Overrides Function Evaluate(ByVal xValue As Double) As Double
        If XIsValid(xValue) Then
            Return _Multiple * _Base ^ xValue
        Else
            Throw New ArgumentOutOfRangeException("xValue")
        End If
    End Function

    Public Overrides Function IndefiniteDerivative() As MFBase
        '(d)/(dx)(a b^x) = a b^x log(b)
        Return New MFExponential(_Base, _Multiple * Math.Log(_Base))
    End Function

    Public Overrides Function IndefiniteIntegral() As MFBase
        'integral a b^x dx = (a*b^x)/(log(b))+constant
        Return New MFExponential(_Base, _Multiple / (Math.Log(_Base)))
    End Function

    Public Overrides Function ToString() As String
        If _Base = Math.E Then
            Return _Multiple.ToString & "e^x"
        ElseIf _Base = 1 Then
            Return _Multiple.ToString
        ElseIf _Base = 0 Then
            Return "0"
        ElseIf _Multiple = 1 Then
            Return _Base.ToString & "^x"
        ElseIf _Multiple = 0 Then
            Return "0"
        Else
            Return _Multiple.ToString & "*" & _Base.ToString & "^x"
        End If
    End Function

    Public Shared Widening Operator CType(ByVal exponential1 As MFExponential) As MFProduct
        Return New MFProduct(1, New MFPolynomial(1), exponential1)
    End Operator

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential1, exponential2)
    End Operator

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFExponential

        Return Multiply(exponential1, constant2)
    End Operator

    Public Shared Operator *(ByVal constant1 As Double,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential2, constant1)
    End Operator

    Public Shared Operator /(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Divide(exponential1, exponential2)
    End Operator

    Public Shared Operator /(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFExponential

        Return Divide(exponential1, constant2)
    End Operator

    Public Shared Operator /(ByVal constant1 As Double,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Divide(constant1, exponential2)
    End Operator

    Public Shared Operator -(ByVal exponential1 As MFExponential) As MFExponential
        Return Negate(exponential1)
    End Operator

End Class
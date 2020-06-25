Imports System.Globalization

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
        If base < 0 Then Throw New ArgumentOutOfRangeException(NameOf(base))
    End Sub

    Public Sub New(ByVal base As Double)
        Me.New(base, 1.0)
    End Sub

    Public Overrides Function Evaluate(ByVal atValue As Double) As Double
        If AtValueIsValid(atValue) Then
            Return _Multiple * _Base ^ atValue
        Else
            Throw New ArgumentOutOfRangeException(NameOf(atValue))
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

    Public Overrides Function Negate() As MFBase
        Return New MFExponential(Base, -Multiple)
    End Function

    Public Overrides Function Clone() As MFBase
        Return New MFExponential(_Base, _Multiple)
    End Function

    Public Overrides Function ToString() As String
        If _Base = Math.E Then
            Return _Multiple.ToString(CultureInfo.CurrentCulture) & "e^x"
        ElseIf _Base = 1 Then
            Return _Multiple.ToString(CultureInfo.CurrentCulture)
        ElseIf _Base = 0 Then
            Return "0"
        ElseIf _Multiple = 1 Then
            Return _Base.ToString(CultureInfo.CurrentCulture) & "^x"
        ElseIf _Multiple = 0 Then
            Return "0"
        Else
            Return (_Multiple.ToString(CultureInfo.CurrentCulture) & " * " & _Base.ToString(CultureInfo.CurrentCulture) & "^x")
        End If
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        ElseIf Me Is obj Then
            Return True
        Else
            Dim exp As MFExponential = TryCast(obj, MFExponential)
            If exp Is Nothing Then
                Return False
            Else
                Return Base = exp.Base AndAlso Multiple = exp.Multiple
            End If
        End If
    End Function

    Public Overloads Shared Operator =(ByVal exponential1 As MFExponential,
                                       ByVal exponential2 As MFExponential) As Boolean

        If exponential1 Is Nothing Then
            Return False
        ElseIf exponential2 Is Nothing Then
            Return False
        ElseIf exponential1 Is exponential2 Then
            Return True
        Else
            Return exponential1.Base = exponential2.Base AndAlso exponential1.Multiple = exponential2.Multiple
        End If
    End Operator

    Public Overloads Shared Operator <>(ByVal exponential1 As MFExponential,
                                        ByVal exponential2 As MFExponential) As Boolean

        Return Not (exponential1 = exponential2)
    End Operator

    Public Shared Function Add(ByVal exponential1 As MFExponential,
                               ByVal exponential2 As MFExponential) As MFGeneral

        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf exponential1.Base = exponential2.Base Then
            Return MFGeneral.Add(New MFGeneral(), New MFExponential(exponential1.Base, exponential1.Multiple + exponential2.Multiple))
        Else
            Return MFGeneral.Add(MFGeneral.Add(New MFGeneral(), exponential1), exponential2)
        End If
    End Function

    Public Shared Operator +(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(exponential1, exponential2)
    End Operator

    Public Shared Function Add(ByVal exponential1 As MFExponential,
                               ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return MFPolynomial.Add(polynomial2, exponential1)
    End Function

    Public Shared Operator +(ByVal exponential1 As MFExponential,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return MFPolynomial.Add(polynomial2, exponential1)
    End Operator

    Public Shared Function Add(ByVal exponential1 As MFExponential,
                               ByVal constant2 As Double) As MFGeneral

        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        Else
            Return New MFGeneral(exponential1, New MFPolynomial(constant2))
        End If
    End Function

    Public Shared Operator +(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFGeneral

        Return Add(exponential1, constant2)
    End Operator

    Public Shared Function Add(ByVal constant1 As Double,
                               ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(exponential2, constant1)
    End Function

    Public Shared Operator +(ByVal constant1 As Double,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(exponential2, constant1)
    End Operator

    Public Overloads Shared Function Negate(ByVal exponential1 As MFExponential) As MFExponential
        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        Else
            Return New MFExponential(exponential1.Base, -exponential1.Multiple)
        End If
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential) As MFExponential
        Return Negate(exponential1)
    End Operator

    Public Shared Function Subtract(ByVal exponential1 As MFExponential,
                                    ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(exponential1, Negate(exponential2))
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(exponential1, Negate(exponential2))
    End Operator

    Public Shared Function Subtract(ByVal exponential1 As MFExponential,
                                    ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return MFPolynomial.Add(-polynomial2, exponential1)
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return MFPolynomial.Add(-polynomial2, exponential1)
    End Operator

    Public Shared Function Subtract(ByVal exponential1 As MFExponential,
                                    ByVal constant2 As Double) As MFGeneral

        Return Add(exponential1, -constant2)
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFGeneral

        Return Add(exponential1, -constant2)
    End Operator

    Public Shared Function Subtract(ByVal constant2 As Double,
                                    ByVal exponential1 As MFExponential) As MFGeneral

        Return Add(constant2, -exponential1)
    End Function

    Public Shared Operator -(ByVal constant2 As Double,
                             ByVal exponential1 As MFExponential) As MFGeneral

        Return Add(constant2, -exponential1)
    End Operator

    Public Shared Function Multiply(ByVal exponential1 As MFExponential,
                                    ByVal exponential2 As MFExponential) As MFExponential

        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            Return New MFExponential(exponential1.Base * exponential2.Base, exponential1.Multiple * exponential2.Multiple)
        End If
    End Function

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential1, exponential2)
    End Operator

    Public Shared Function Multiply(ByVal exponential1 As MFExponential,
                                    ByVal constant2 As Double) As MFExponential

        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        Else
            Return New MFExponential(exponential1.Base, exponential1.Multiple * constant2)
        End If
    End Function

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFExponential

        Return Multiply(exponential1, constant2)
    End Operator

    Public Shared Function Multiply(ByVal constant1 As Double,
                                    ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential2, constant1)
    End Function

    Public Shared Operator *(ByVal constant1 As Double,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential2, constant1)
    End Operator

    Public Shared Function Multiply(ByVal exponential1 As MFExponential,
                                    ByVal polynomial2 As MFPolynomial) As MFProduct

        Return MFPolynomial.Multiply(polynomial2, exponential1)
    End Function

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal polynomial2 As MFPolynomial) As MFProduct

        Return Multiply(exponential1, polynomial2)
    End Operator

    Public Function Invert() As MFExponential
        Return New MFExponential(1.0# / Base, 1.0# / Multiple)
    End Function

    Public Shared Function Invert(ByVal exponential1 As MFExponential) As MFExponential
        If exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        Else
            Return exponential1.Invert
        End If
    End Function

    Public Shared Function Divide(ByVal exponential1 As MFExponential,
                                  ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential1, Invert(exponential2))
    End Function

    Public Shared Operator /(ByVal exponential1 As MFExponential,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(exponential1, Invert(exponential2))
    End Operator

    Public Shared Function Divide(ByVal exponential1 As MFExponential,
                                  ByVal constant2 As Double) As MFExponential

        Return Multiply(exponential1, 1.0# / constant2)
    End Function

    Public Shared Operator /(ByVal exponential1 As MFExponential,
                             ByVal constant2 As Double) As MFExponential

        Return Multiply(exponential1, 1.0# / constant2)
    End Operator

    Public Shared Function Divide(ByVal constant1 As Double,
                                  ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(Invert(exponential2), constant1)
    End Function

    Public Shared Operator /(ByVal constant1 As Double,
                             ByVal exponential2 As MFExponential) As MFExponential

        Return Multiply(Invert(exponential2), constant1)
    End Operator

End Class
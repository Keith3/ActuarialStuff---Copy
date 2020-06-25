Imports System.Globalization

Public Class MFPolynomial
    Inherits MFBase

    Private ReadOnly _coefficients() As Double

    Public ReadOnly Property Degree As Integer
        Get
            Return _coefficients.Count - 1
        End Get
    End Property

    Private Sub New()
    End Sub

    Public Sub New(ByVal ParamArray coefficient() As Double)
        If coefficient IsNot Nothing Then
            Dim degree As Integer
            For i = coefficient.Count - 1 To 0 Step -1
                If coefficient(i) <> 0.0 Then
                    degree = i
                    Exit For
                End If
            Next
            ReDim _coefficients(degree)
            For j = 0 To degree
                _coefficients(j) = coefficient(j)
            Next
        Else
            Throw New ArgumentNullException(NameOf(coefficient))
        End If
    End Sub

    Public Function Coefficient(ByVal index As Integer) As Double
        Return _coefficients(index)
    End Function

    'Public Function Coefficients() As Double()
    '    Dim newPolArray(_coefficients.Count - 1) As Double

    '    For Index = 0 To _coefficients.Count - 1
    '        newPolArray(Index) = _coefficients(Index)
    '    Next

    '    Return newPolArray
    'End Function

    Public Overrides Function Evaluate(ByVal atValue As Double) As Double
        If AtValueIsValid(atValue) Then
            Dim value As Double = _coefficients(Degree)

            For Index = Degree - 1 To 0 Step -1
                value *= atValue
                value += _coefficients(Index)
            Next

            Return value
        Else
            Throw New ArgumentOutOfRangeException(NameOf(atValue))
        End If
    End Function

    Public Overrides Function IndefiniteIntegral() As MFBase
        Dim coeffArray(Degree + 1) As Double

        For i = 0 To Degree
            coeffArray(i + 1) = _coefficients(i) / (i + 1)
        Next

        Return New MFPolynomial(coeffArray)
    End Function

    Public Overrides Function IndefiniteDerivative() As MFBase
        If Degree > 0 Then
            Dim coeffArray(Degree - 1) As Double

            For Index = 0 To Degree - 1
                coeffArray(Index) = _coefficients(Index + 1) * (Index + 1)
            Next

            Return New MFPolynomial(coeffArray)
        Else
            Return New MFPolynomial(0)
        End If
    End Function

    Public Overrides Function Clone() As MFBase
        Return New MFPolynomial(_coefficients)
    End Function

    Public Overrides Function Negate() As MFBase
        Dim newPolArray(Degree) As Double

        For Index = 0 To Degree
            newPolArray(Index) = -Coefficient(Index)
        Next

        Return New MFPolynomial(newPolArray)
    End Function

    Public Overrides Function ToString() As String
        Dim disp As String = ""

        If _coefficients(0) <> 0.0 Then disp = _coefficients(0).ToString(CultureInfo.CurrentCulture)

        If Degree >= 1 Then
            If _coefficients(1) <> 0.0 Then
                If _coefficients(1) > 0 Then
                    If disp.Length > 0 Then disp &= " + "
                    If _coefficients(1) <> 1.0 Then disp &= _coefficients(1).ToString(CultureInfo.CurrentCulture)
                Else
                    If disp.Length > 0 Then disp &= " - "
                    If _coefficients(1) <> -1.0 Then disp &= (-_coefficients(1)).ToString(CultureInfo.CurrentCulture)
                End If
                disp &= "x"
            End If
        End If

        For Index = 2 To Degree
            If _coefficients(Index) <> 0.0 Then
                If _coefficients(Index) > 0 Then
                    If disp.Length > 0 Then disp &= " + "
                    If _coefficients(Index) <> 1.0 Then disp &= _coefficients(Index).ToString(CultureInfo.CurrentCulture)
                Else
                    If disp.Length > 0 Then disp &= " - "
                    If _coefficients(Index) <> -1.0 Then disp &= (-_coefficients(Index)).ToString(CultureInfo.CurrentCulture)
                End If
                disp &= "x^" & Index.ToString(CultureInfo.CurrentCulture)
            End If
        Next

        Return If(Degree > 0, disp.AddParens, disp)
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        ElseIf Me Is obj Then
            Return True
        Else
            Dim pol As MFPolynomial = TryCast(obj, MFPolynomial)
            If pol Is Nothing Then
                Return False
            Else
                For i = 0 To pol.Degree - 1
                    If pol.Coefficient(i) <> pol.Coefficient(i) Then
                        Return False
                    End If
                Next
                Return True
            End If
        End If
    End Function

    Public Overloads Shared Operator =(ByVal polynomial1 As MFPolynomial,
                                       ByVal polynomial2 As MFPolynomial) As Boolean

        If polynomial1 Is Nothing Then
            Return False
        ElseIf polynomial2 Is Nothing Then
            Return False
        ElseIf polynomial1 Is polynomial2 Then
            Return True
        ElseIf polynomial1.Degree = polynomial2.Degree Then
            For i = 0 To polynomial1.Degree - 1
                If polynomial1.Coefficient(i) <> polynomial2.Coefficient(i) Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Operator

    Public Overloads Shared Operator <>(ByVal polynomial1 As MFPolynomial,
                                        ByVal polynomial2 As MFPolynomial) As Boolean

        Return Not polynomial1 = polynomial2
    End Operator

    Public Shared Function Add(ByVal polynomial1 As MFPolynomial,
                               ByVal polynomial2 As MFPolynomial) As MFPolynomial

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        Else
            Dim newPolArray(Math.Max(polynomial1.Degree, polynomial2.Degree)) As Double

            For Index = 0 To polynomial1.Degree
                newPolArray(Index) = polynomial1.Coefficient(Index)
            Next

            For Index = 0 To polynomial2.Degree
                newPolArray(Index) += polynomial2.Coefficient(Index)
            Next

            Return New MFPolynomial(newPolArray)
        End If
    End Function

    Public Shared Operator +(ByVal polynomial1 As MFPolynomial,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(polynomial1, polynomial2)
    End Operator

    Public Shared Function Add(ByVal polynomial1 As MFPolynomial,
                               ByVal exponential2 As MFExponential) As MFGeneral

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf exponential2.Base = 1 Then
            Return New MFGeneral(Add(polynomial1, exponential2.Multiple))
        Else
            Return New MFGeneral(polynomial1, exponential2)
        End If
    End Function

    Public Shared Operator +(ByVal polynomial1 As MFPolynomial,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(polynomial1, exponential2)
    End Operator

    Public Shared Function Add(ByVal polynomial1 As MFPolynomial,
                               ByVal constant2 As Double) As MFPolynomial

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        Else
            Dim coeff(polynomial1._coefficients.Count - 1) As Double

            For i = 0 To polynomial1._coefficients.Count - 1
                coeff(i) = polynomial1.Coefficient(i)
            Next

            coeff(0) += constant2

            Return New MFPolynomial(coeff)
        End If
    End Function

    Public Shared Operator +(ByVal polynomial1 As MFPolynomial,
                             ByVal constant2 As Double) As MFPolynomial

        Return Add(polynomial1, constant2)
    End Operator

    Public Shared Function Add(ByVal constant1 As Double,
                               ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(polynomial2, constant1)
    End Function

    Public Shared Operator +(ByVal constant1 As Double,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(polynomial2, constant1)
    End Operator

    Public Overloads Shared Function Negate(ByVal polynomial1 As MFPolynomial) As MFPolynomial

        If polynomial1 Is Nothing Then Return Nothing
        Return CType(polynomial1.Negate, MFPolynomial)
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial) As MFPolynomial
        Return Negate(polynomial1)
    End Operator

    Public Shared Function Subtract(ByVal polynomial1 As MFPolynomial,
                                    ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(polynomial1, Negate(polynomial2))
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(polynomial1, Negate(polynomial2))
    End Operator

    Public Shared Function Subtract(ByVal polynomial1 As MFPolynomial,
                                    ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(polynomial1, MFExponential.Negate(exponential2))
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(polynomial1, MFExponential.Negate(exponential2))
    End Operator

    Public Shared Function Subtract(ByVal polynomial1 As MFPolynomial,
                                    ByVal constant2 As Double) As MFPolynomial

        Return Add(polynomial1, -constant2)
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial,
                             ByVal constant2 As Double) As MFPolynomial

        Return Add(polynomial1, -constant2)
    End Operator

    Public Shared Function Subtract(ByVal constant1 As Double,
                                    ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(Negate(polynomial2), constant1)
    End Function

    Public Shared Operator -(ByVal constant1 As Double,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Add(Negate(polynomial2), constant1)
    End Operator

    Public Shared Function Multiply(ByVal polynomial1 As MFPolynomial,
                                    ByVal polynomial2 As MFPolynomial) As MFPolynomial

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        Else
            Dim newPolArray(polynomial1.Degree + polynomial2.Degree) As Double

            For i = 0 To polynomial1.Degree
                For j = 0 To polynomial2.Degree
                    newPolArray(i + j) += polynomial1.Coefficient(i) * polynomial2.Coefficient(j)
                Next
            Next

            Return New MFPolynomial(newPolArray)
        End If
    End Function

    Public Shared Operator *(ByVal polynomial1 As MFPolynomial,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Multiply(polynomial1, polynomial2)
    End Operator

    Public Shared Function Multiply(ByVal polynomial1 As MFPolynomial,
                                    ByVal exponential2 As MFExponential) As MFProduct

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            Return New MFProduct(exponential2, polynomial1)
        End If
    End Function

    Public Shared Operator *(ByVal polynomial1 As MFPolynomial,
                             ByVal exponential2 As MFExponential) As MFProduct

        Return Multiply(polynomial1, exponential2)
    End Operator

    Public Shared Function Multiply(ByVal polynomial1 As MFPolynomial,
                                    ByVal constant2 As Double) As MFPolynomial

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        Else
            Dim newPolArray(polynomial1._coefficients.Count - 1) As Double

            For i = 0 To polynomial1.Degree
                newPolArray(i) = polynomial1._coefficients(i) * constant2
            Next

            Return New MFPolynomial(newPolArray)
        End If
    End Function

    Public Shared Operator *(ByVal polynomial1 As MFPolynomial,
                             ByVal constant2 As Double) As MFPolynomial

        Return Multiply(polynomial1, constant2)
    End Operator

    Public Shared Function Multiply(ByVal constant1 As Double,
                                    ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Multiply(polynomial2, constant1)
    End Function

    Public Shared Operator *(ByVal constant1 As Double,
                             ByVal polynomial2 As MFPolynomial) As MFPolynomial

        Return Multiply(constant1, polynomial2)
    End Operator

    Public Shared Function Divide(ByVal polynomial1 As MFPolynomial,
                                  ByVal constant2 As Double) As MFPolynomial

        Return Multiply(polynomial1, 1.0# / constant2)
    End Function

    Public Shared Operator /(ByVal polynomial1 As MFPolynomial,
                             ByVal constant2 As Double) As MFPolynomial

        Return Multiply(polynomial1, 1.0# / constant2)
    End Operator

    Public Shared Function Divide(ByVal polynomial1 As MFPolynomial,
                                  ByVal exponential2 As MFExponential) As MFProduct


        Return Multiply(polynomial1, exponential2.Invert)
    End Function

    Public Shared Operator /(ByVal polynomial1 As MFPolynomial,
                             ByVal exponential2 As MFExponential) As MFProduct

        Return Multiply(polynomial1, exponential2.Invert)
    End Operator

End Class
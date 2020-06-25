Public Class MFPolynomial
    Inherits MFBase

    Private _coefficients() As Double
    Private _degree As Integer

    Private Sub New()
    End Sub

    Public Sub New(ByVal ParamArray coefficient() As Double)
        If coefficient IsNot Nothing Then
            _degree = coefficient.Count - 1
            For i = coefficient.Count - 1 To 0 Step -1
                If coefficient(i) <> 0.0 Then
                    _degree = i
                    Exit For
                End If
            Next
            ReDim _coefficients(_degree)
            For j = 0 To _degree
                _coefficients(j) = coefficient(j)
            Next
        Else
            ReDim _coefficients(0)
            _coefficients(0) = 0
            _degree = 0
        End If
    End Sub

    Public ReadOnly Property Coefficient(ByVal index As Integer) As Double
        Get
            Return _coefficients(index)
        End Get
    End Property

    Public Function Coefficients() As Double()
        Dim c(_coefficients.Count - 1) As Double
        For i = 0 To _coefficients.Count - 1
            c(i) = _coefficients(i)
        Next
        Return c
    End Function

    Public ReadOnly Property Degree As Integer
        Get
            Return _degree
        End Get
    End Property

    Public Overloads Shared Function XIsValid(xValue As Double) As Boolean
        Return True
    End Function

    Public Overloads Shared Function XRangeIsValid(xRange As Range(Of Double)) As Boolean
        Return True
    End Function

    Public Overrides Function Evaluate(ByVal xValue As Double) As Double
        If XIsValid(xValue) Then
            Dim value As Double = _coefficients(Degree)

            For Index = Degree - 1 To 0 Step -1
                value *= xValue
                value += _coefficients(Index)
            Next

            Return value
        Else
            Throw New ArgumentOutOfRangeException("xValue")
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

    Public Overrides Function ToString() As String
        Dim disp As String = ""

        If _coefficients(0) <> 0.0 Then disp = _coefficients(0).ToString

        If Degree >= 1 AndAlso _coefficients(1) <> 0.0 Then
            If _coefficients(1) > 0 Then
                If disp.Length > 0 Then disp &= "+"
                If _coefficients(1) <> 1.0 Then disp &= _coefficients(1).ToString
            Else
                If disp.Length > 0 Then disp &= "-"
                If _coefficients(1) <> 1.0 Then disp &= (-_coefficients(1)).ToString
            End If
            disp &= "x"
        End If

        For Index = 2 To Degree
            If _coefficients(Index) <> 0.0 Then
                If _coefficients(Index) > 0 Then
                    If disp.Length > 0 Then disp &= "+"
                    If _coefficients(Index) <> 1.0 Then disp &= _coefficients(Index).ToString
                Else
                    If disp.Length > 0 Then disp &= "-"
                    If _coefficients(Index) <> -1.0 Then disp &= (-_coefficients(Index)).ToString
                End If
                disp &= "x^" & Index.ToString
            End If
        Next

        Return disp
    End Function

    Public Shared Widening Operator CType(ByVal poly1 As MFPolynomial) As MFProduct
        Return New MFProduct(1, poly1, New MFExponential(1, 1))
    End Operator

    Public Shared Operator +(ByVal poly1 As MFPolynomial,
                             ByVal poly2 As MFPolynomial) As MFPolynomial

        Return Add(poly1, poly2)
    End Operator

    Public Shared Operator +(ByVal poly1 As MFPolynomial,
                             ByVal const2 As Double) As MFPolynomial

        Return Add(poly1, const2)
    End Operator

    Public Shared Operator +(ByVal const1 As Double,
                             ByVal poly2 As MFPolynomial) As MFPolynomial

        Return Add(poly2, const1)
    End Operator

    Public Shared Operator -(ByVal poly1 As MFPolynomial,
                             ByVal poly2 As MFPolynomial) As MFPolynomial

        Return Subtract(poly1, poly2)
    End Operator

    Public Shared Operator -(ByVal poly1 As MFPolynomial,
                             ByVal const2 As Double) As MFPolynomial

        Return Subtract(poly1, const2)
    End Operator

    Public Shared Operator -(ByVal poly1 As MFPolynomial) As MFPolynomial
        Return Negate(poly1)
    End Operator

    Public Shared Operator *(ByVal poly1 As MFPolynomial,
                             ByVal poly2 As MFPolynomial) As MFPolynomial

        Return Multiply(poly1, poly2)
    End Operator

    Public Shared Operator *(ByVal poly1 As MFPolynomial,
                             ByVal const2 As Double) As MFPolynomial

        Return Multiply(poly1, const2)
    End Operator

    Public Shared Operator *(ByVal const1 As Double,
                             ByVal poly2 As MFPolynomial) As MFPolynomial

        Return Multiply(poly2, const1)
    End Operator

    Public Shared Operator /(ByVal poly1 As MFPolynomial,
                             ByVal const2 As Double) As MFPolynomial

        Return Multiply(poly1, 1 / const2)
    End Operator

    'Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
    '    If obj Is Nothing OrElse Me.GetType IsNot obj.GetType Then Return False
    '    Return Me = CType(obj, MFPolynomial)
    'End Function

    'Public Shared Operator =(ByVal poly1 As MFPolynomial,
    '                         ByVal poly2 As MFPolynomial) As Boolean

    '    If poly1 Is Nothing OrElse poly2 Is Nothing Then Return Nothing

    '    If poly1.Degree = poly2.Degree Then
    '        For i = 0 To poly1.Degree - 1
    '            If poly1.Coefficients(i) <> poly2.Coefficients(i) Then Return False
    '        Next
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Operator

    'Public Shared Operator <>(ByVal poly1 As MFPolynomial,
    '                          ByVal poly2 As MFPolynomial) As Boolean

    '    If poly1 Is Nothing OrElse poly2 Is Nothing Then
    '        Return Nothing
    '    Else
    '        Return Not (poly1 = poly2)
    '    End If
    'End Operator

End Class
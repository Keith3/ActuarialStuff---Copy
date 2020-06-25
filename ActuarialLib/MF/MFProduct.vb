Public Class MFProduct
    Inherits MFBase

    'Expression in the form of (Polynomial * Exponential)

    Public ReadOnly Property Polynomial As MFPolynomial
    Public ReadOnly Property Exponential As MFExponential

    Private Sub New()
    End Sub

    Public Sub New(ByVal polynomial1 As MFPolynomial,
                   ByVal exponential2 As MFExponential)

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            _Polynomial = MFPolynomial.Multiply(polynomial1, exponential2.Multiple)
            _Exponential = New MFExponential(exponential2.Base)
        End If
    End Sub

    Public Sub New(ByVal exponential1 As MFExponential,
                   ByVal polynomial2 As MFPolynomial)

        If polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        ElseIf exponential1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential1))
        Else
            _Polynomial = MFPolynomial.Multiply(polynomial2, exponential1.Multiple)
            _Exponential = New MFExponential(exponential1.Base)
        End If
    End Sub

    Public Overloads Shared Function XIsValid(valueX As Double) As Boolean
        Return MFPolynomial.AtValueIsValid(valueX) AndAlso MFExponential.AtValueIsValid(valueX)
    End Function

    Public Overrides Function Evaluate(atValue As Double) As Double
        If XIsValid(atValue) Then
            Return _Polynomial.Evaluate(atValue) * _Exponential.Evaluate(atValue)
        Else
            Throw New ArgumentOutOfRangeException(NameOf(atValue))
        End If
    End Function

    Public Overrides Function IndefiniteIntegral() As MFBase

        Dim pIn As MFPolynomial = CType(_Polynomial.Clone, MFPolynomial)
        Dim degree = pIn.Degree
        Dim lnb As Double = Math.Log(_Exponential.Base) * If(degree Mod 2 = 0, 1, -1)
        Dim lnbn As Double = lnb
        Dim pOut As MFPolynomial = pIn / lnb

        pIn = CType(pIn.IndefiniteDerivative, MFPolynomial)
        For d = degree - 1 To 0 Step -1
            lnbn *= -lnb
            pOut += pIn / lnbn
        Next

        Return New MFProduct(_Exponential, pOut)
    End Function

    Public Overrides Function IndefiniteDerivative() As MFBase
        Dim result As New MFGeneral(New MFProduct(CType(Exponential.IndefiniteDerivative, MFExponential), Polynomial),
                                    New MFProduct(Exponential, CType(Polynomial.IndefiniteDerivative, MFPolynomial)))

        Return result
    End Function

    Public Overrides Function Negate() As MFBase
        Return New MFProduct(CType(Exponential.Negate, MFExponential), Polynomial)
    End Function

    Public Overrides Function Clone() As MFBase
        Return New MFProduct(CType(_Exponential.Clone, MFExponential), CType(_Polynomial.Clone, MFPolynomial))
    End Function

    Public Overrides Function ToString() As String
        Dim x As String = ""

        If _Polynomial IsNot Nothing Then
            x &= _Polynomial.ToString
        End If

        If _Exponential IsNot Nothing Then
            If x.Length > 0 Then x &= " * "
            x &= _Exponential.ToString
        End If

        Return x
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        ElseIf Me Is obj Then
            Return True
        Else
            Dim prod As MFProduct = TryCast(obj, MFProduct)
            If prod Is Nothing Then
                Return False
            Else
                Return Polynomial = prod.Polynomial AndAlso Exponential = prod.Exponential
            End If
        End If
    End Function

    Public Overloads Shared Operator =(ByVal product1 As MFProduct,
                                       ByVal product2 As MFProduct) As Boolean

        If product1 Is Nothing Then
            Return False
        ElseIf product2 Is Nothing Then
            Return False
        ElseIf product1 Is product2 Then
            Return True
        Else
            Return product1.Polynomial = product2.Polynomial AndAlso product1.Exponential = product2.Exponential
        End If
    End Operator

    Public Overloads Shared Operator <>(ByVal product1 As MFProduct,
                                        ByVal product2 As MFProduct) As Boolean

        Return Not (product1 = product2)
    End Operator

    Public Shared Function Add(ByVal product1 As MFProduct,
                               ByVal product2 As MFProduct) As MFGeneral


        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        ElseIf product1.Exponential.Base = product2.Exponential.Base Then
            Return New MFGeneral(New MFProduct(New MFExponential(product1.Exponential.Base), product1.Polynomial * product1.Exponential.Multiple + product2.Polynomial * product2.Exponential.Multiple))
        Else
            Return New MFGeneral(product1, product2)
        End If
    End Function

    Public Shared Operator +(ByVal product1 As MFProduct,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(product1, product2)
    End Operator

    Public Shared Function Add(ByVal product1 As MFProduct,
                               ByVal polynomial2 As MFPolynomial) As MFGeneral

        If polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        ElseIf product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        Else
            Return New MFGeneral(product1, polynomial2)
        End If
    End Function

    Public Shared Operator +(ByVal product1 As MFProduct,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Add(product1, polynomial2)
    End Operator


    Public Shared Function Add(ByVal polynomial1 As MFPolynomial,
                               ByVal product2 As MFProduct) As MFGeneral

        Return Add(product2, polynomial1)
    End Function

    Public Shared Operator +(ByVal polynomial1 As MFPolynomial,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(product2, polynomial1)
    End Operator

    Public Shared Function Add(ByVal product1 As MFProduct,
                               ByVal exponential2 As MFExponential) As MFGeneral

        If exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf product1.Exponential.Base = exponential2.Base Then
            Return New MFGeneral(New MFProduct(New MFExponential(exponential2.Base), product1.Polynomial * product1.Exponential.Multiple + exponential2.Multiple))
        Else
            Return New MFGeneral(product1, exponential2)
        End If
    End Function

    Public Shared Operator +(ByVal product1 As MFProduct,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(product1, exponential2)
    End Operator

    Public Shared Function Add(ByVal exponential1 As MFExponential,
                               ByVal product2 As MFProduct) As MFGeneral

        Return Add(product2, exponential1)
    End Function

    Public Shared Operator +(ByVal exponential1 As MFExponential,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(product2, exponential1)
    End Operator

    Public Shared Function Add(ByVal product1 As MFProduct,
                               ByVal constant2 As Double) As MFGeneral

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        Else
            Return New MFGeneral(product1, New MFPolynomial(constant2))
        End If
    End Function

    Public Shared Operator +(ByVal product1 As MFProduct,
                             ByVal constant2 As Double) As MFGeneral

        Return Add(product1, constant2)
    End Operator

    Public Shared Function Add(ByVal constant1 As Double,
                               ByVal product2 As MFProduct) As MFGeneral

        Return Add(product2, constant1)
    End Function

    Public Shared Operator +(ByVal constant1 As Double,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(constant1, product2)
    End Operator

    Public Overloads Shared Function Negate(ByVal product1 As MFProduct) As MFProduct
        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        Else
            Return CType(product1.Negate, MFProduct)
        End If
    End Function

    Public Shared Operator -(ByVal product1 As MFProduct) As MFProduct
        Return Negate(product1)
    End Operator

    Public Shared Function Subtract(ByVal product1 As MFProduct,
                                    ByVal product2 As MFProduct) As MFGeneral

        Return Add(product1, -product2)
    End Function

    Public Shared Operator -(ByVal product1 As MFProduct,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(product1, -product2)
    End Operator

    Public Shared Function Subtract(ByVal product1 As MFProduct,
                                    ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Add(product1, -polynomial2)
    End Function

    Public Shared Operator -(ByVal product1 As MFProduct,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Add(product1, -polynomial2)
    End Operator

    Public Shared Function Subtract(ByVal polynomial1 As MFPolynomial,
                                    ByVal product2 As MFProduct) As MFGeneral

        If polynomial1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial1))
        ElseIf product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        Else
            Return New MFGeneral(polynomial1, -product2)
        End If
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Subtract(polynomial1, product2)
    End Operator

    Public Shared Function Subtract(ByVal product1 As MFProduct,
                                    ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(product1, -exponential2)
    End Function

    Public Shared Operator -(ByVal product1 As MFProduct,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(product1, -exponential2)
    End Operator

    Public Shared Function Subtract(ByVal exponential1 As MFExponential,
                                    ByVal product2 As MFProduct) As MFGeneral

        Return Add(exponential1, -product2)
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(exponential1, -product2)
    End Operator

    Public Shared Function Subtract(ByVal product1 As MFProduct,
                                    ByVal constant2 As Double) As MFGeneral

        Return Add(product1, -constant2)
    End Function

    Public Shared Operator -(ByVal product1 As MFProduct,
                             ByVal constant2 As Double) As MFGeneral

        Return Add(product1, -constant2)
    End Operator

    Public Shared Function Subtract(ByVal constant2 As Double,
                                    ByVal product1 As MFProduct) As MFGeneral

        Return Add(constant2, -product1)
    End Function

    Public Shared Operator -(ByVal constant2 As Double,
                             ByVal product1 As MFProduct) As MFGeneral

        Return Add(constant2, -product1)
    End Operator

    Public Shared Function Multiply(ByVal product1 As MFProduct,
                                    ByVal product2 As MFProduct) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        Else
            Return New MFProduct(MFExponential.Multiply(product1.Exponential, product2.Exponential),
                                 MFPolynomial.Multiply(product1.Polynomial, product2.Polynomial))
        End If
    End Function

    Public Shared Operator *(ByVal product1 As MFProduct,
                             ByVal product2 As MFProduct) As MFProduct

        Return Multiply(product1, product2)
    End Operator

    Public Shared Function Multiply(ByVal product1 As MFProduct,
                                    ByVal constant2 As Double) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        Else
            Return New MFProduct(MFExponential.Multiply(constant2, product1.Exponential), product1.Polynomial)
        End If
    End Function

    Public Shared Operator *(ByVal product1 As MFProduct,
                             ByVal constant2 As Double) As MFProduct

        Return Multiply(product1, constant2)
    End Operator

    Public Shared Function Multiply(ByVal constant1 As Double,
                                    ByVal product2 As MFProduct) As MFProduct

        If product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        Else
            Return Multiply(product2, constant1)
        End If
    End Function

    Public Shared Operator *(ByVal constant1 As Double,
                             ByVal product2 As MFProduct) As MFProduct

        Return Multiply(constant1, product2)
    End Operator

    Public Shared Function Multiply(ByVal product1 As MFProduct,
                                    ByVal polynomial2 As MFPolynomial) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        Else
            Return New MFProduct(product1.Exponential, MFPolynomial.Multiply(product1.Polynomial, polynomial2))
        End If
    End Function

    Public Shared Operator *(ByVal product1 As MFProduct,
                             ByVal polynomial2 As MFPolynomial) As MFProduct

        Return Multiply(product1, polynomial2)
    End Operator

    Public Shared Function Multiply(ByVal polynomial1 As MFPolynomial,
                                    ByVal product2 As MFProduct) As MFProduct

        Return Multiply(product2, polynomial1)
    End Function

    Public Shared Operator *(ByVal polynomial1 As MFPolynomial,
                             ByVal product2 As MFProduct) As MFProduct

        Return Multiply(polynomial1, product2)
    End Operator

    Public Shared Function Multiply(ByVal product1 As MFProduct,
                                    ByVal exponential2 As MFExponential) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            Return New MFProduct(MFExponential.Multiply(product1.Exponential, exponential2), product1.Polynomial)
        End If
    End Function

    Public Shared Operator *(ByVal product1 As MFProduct,
                             ByVal exponential2 As MFExponential) As MFProduct

        Return Multiply(product1, exponential2)
    End Operator

    Public Shared Function Multiply(ByVal exponential1 As MFExponential,
                                    ByVal product2 As MFProduct) As MFProduct

        Return Multiply(product2, exponential1)
    End Function

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal product2 As MFProduct) As MFProduct

        Return Multiply(product2, exponential1)
    End Operator

    Public Shared Function Divide(ByVal product1 As MFProduct,
                                  ByVal constant2 As Double) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        Else
            Return New MFProduct(MFExponential.Divide(product1.Exponential, constant2), product1.Polynomial)
        End If
    End Function

    Public Shared Operator /(ByVal product1 As MFProduct,
                             ByVal constant2 As Double) As MFProduct

        Return Divide(product1, constant2)
    End Operator

    Public Shared Function Divide(ByVal product1 As MFProduct,
                                  ByVal exponential2 As MFExponential) As MFProduct

        If product1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product1))
        ElseIf exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            Return New MFProduct(MFExponential.Divide(product1.Exponential, exponential2), product1.Polynomial)
        End If
    End Function

    Public Shared Operator /(ByVal product1 As MFProduct,
                             ByVal exponential2 As MFExponential) As MFProduct

        Return Divide(product1, exponential2)
    End Operator

End Class
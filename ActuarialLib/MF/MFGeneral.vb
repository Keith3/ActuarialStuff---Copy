Public Class MFGeneral
    Inherits MFBase

    'Expression in the form of MFProduct + MFProduct + ... + MFProduct

    Implements IEnumerable(Of MFBase)

    Private ReadOnly Products As New List(Of MFProduct)
    Private ReadOnly Exponentials As New List(Of MFExponential)
    Private Polynomial As New MFPolynomial(0)

    Private Sub New()
    End Sub

    Public Sub New(ByVal ParamArray terms() As MFBase)
        If terms Is Nothing Then
            Throw New ArgumentNullException(NameOf(terms))
        Else
            For Each t In terms
                If t Is Nothing Then
                    Throw New ArgumentNullException(NameOf(terms))
                Else
                    AddTerm(t)
                End If
            Next
        End If
    End Sub

    Private Sub AddTerm(ByVal term As MFBase)
        If term Is Nothing Then
            Throw New ArgumentOutOfRangeException(NameOf(term))
        Else
            Dim poly As MFPolynomial = TryCast(term, MFPolynomial)
            If poly IsNot Nothing Then
                Polynomial = MFPolynomial.Add(Polynomial, poly)
            Else
                Dim exp As MFExponential = TryCast(term, MFExponential)
                If exp IsNot Nothing Then
                    Dim e As Integer
                    Do While e < Exponentials.Count
                        If exp.Base = Exponentials(e).Base Then
                            Exponentials(e) = New MFExponential(exp.Base, exp.Multiple + Exponentials(e).Multiple)
                            Exit Sub
                        End If
                        e += 1
                    Loop

                    Dim p As Integer
                    Do While p < Products.Count
                        If exp.Base = Products(p).Exponential.Base Then
                            Products(p) = New MFProduct(MFPolynomial.Add(Products(p).Polynomial, exp.Multiple), New MFExponential(Products(p).Exponential.Base))
                            Exit Sub
                        End If
                        p += 1
                    Loop

                    Exponentials.Add(New MFExponential(exp.Base, exp.Multiple))
                Else
                    Dim prod As MFProduct = TryCast(term, MFProduct)
                    If prod IsNot Nothing Then
                        Dim p As Integer
                        Do While p < Products.Count
                            If prod.Exponential.Base = Products(p).Exponential.Base Then
                                Products(p) = New MFProduct(MFPolynomial.Add(Products(p).Polynomial, prod.Polynomial), New MFExponential(Products(p).Exponential.Base))
                                Exit Sub
                            End If
                            p += 1
                        Loop

                        Products.Add(CType(prod.Clone, MFProduct))
                    Else
                        Dim gen As MFGeneral = TryCast(term, MFGeneral)
                        If gen IsNot Nothing Then
                            For Each b In gen
                                AddTerm(b)
                            Next
                        Else
                            Throw New ArgumentOutOfRangeException(NameOf(term))
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Overloads Shared Function AtValueIsValid(atValue As Double) As Boolean
        Return MFPolynomial.AtValueIsValid(atValue) AndAlso MFExponential.AtValueIsValid(atValue)
    End Function

    Public Overrides Function Evaluate(atValue As Double) As Double
        Dim value As Double = Polynomial.Evaluate(atValue)

        For Each p In Products
            value += p.Evaluate(atValue)
        Next

        For Each e In Exponentials
            value += e.Evaluate(atValue)
        Next

        Return value
    End Function

    Public Overrides Function IndefiniteIntegral() As MFBase
        Dim pIntegral As New List(Of MFBase)

        For Each p In Products
            pIntegral.Add(p.IndefiniteIntegral)
        Next

        For Each e In Exponentials
            pIntegral.Add(e.IndefiniteIntegral)
        Next

        pIntegral.Add(Polynomial.IndefiniteIntegral)

        Return New MFGeneral(pIntegral.ToArray)
    End Function

    Public Overrides Function IndefiniteDerivative() As MFBase
        Dim pDerivative As New List(Of MFBase)

        For Each p In Products
            pDerivative.Add(p.IndefiniteDerivative)
        Next

        For Each e In Exponentials
            pDerivative.Add(e.IndefiniteDerivative)
        Next

        pDerivative.Add(Polynomial.IndefiniteDerivative)

        Return New MFGeneral(pDerivative.ToArray)
    End Function

    Public Overrides Function Negate() As MFBase
        Dim array As New List(Of MFBase)

        For Each p In Products
            array.Add(MFProduct.Negate(p))
        Next

        For Each e In Exponentials
            array.Add(MFExponential.Negate(e))
        Next

        array.Add(MFPolynomial.Negate(Polynomial))

        Return New MFGeneral(array.ToArray)
    End Function

    Public Overrides Function Clone() As MFBase
        Dim array As New List(Of MFBase)

        For Each p In Products
            array.Add(p.Clone)
        Next

        For Each e In Exponentials
            array.Add(e.Clone)
        Next

        array.Add(Polynomial.Clone)

        Return New MFGeneral(array.ToArray)
    End Function

    Public Overrides Function ToString() As String
        Dim x As String = ""

        For Each p In Products
            If p.ToString.Length > 0 Then
                If x.Length > 0 Then
                    x &= " + "
                End If
                x &= p.ToString
            End If
        Next

        For Each e In Exponentials
            If e.ToString.Length > 0 Then
                If x.Length > 0 Then
                    x &= " + "
                End If
                x &= e.ToString
            End If
        Next

        If Polynomial.ToString.Length > 0 Then
            If x.Length > 0 Then
                x &= " + "
            End If
            x &= Polynomial.ToString
        End If

        Return x
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        ElseIf Me Is obj Then
            Return True
        Else
            Dim gen As MFGeneral = TryCast(obj, MFGeneral)
            If gen Is Nothing Then
                Return False
            ElseIf Products.Count <> gen.Products.Count Then
                Return False
            ElseIf Exponentials.Count <> gen.Exponentials.Count Then
                Return False
            ElseIf Not Polynomial.Equals(gen.Polynomial) Then
                Return False
            Else
                For p = 0 To Products.Count - 1
                    If Not Products(p).Equals(gen.Products(p)) Then
                        Return False
                    End If
                Next

                For e = 0 To Exponentials.Count - 1
                    If Not Exponentials(e).Equals(gen.Exponentials(e)) Then
                        Return False
                    End If
                Next

                Return True
            End If
        End If
    End Function

    Public Overloads Shared Operator =(ByVal general1 As MFGeneral,
                                       ByVal general2 As MFGeneral) As Boolean

        If general1 Is Nothing Then
            Return False
        ElseIf general2 Is Nothing Then
            Return False
        ElseIf general1 Is general2 Then
            Return True
        ElseIf general1.Products.Count <> general2.Products.Count Then
            Return False
        ElseIf general1.Exponentials.Count <> general2.Exponentials.Count Then
            Return False
        ElseIf general1.Polynomial <> general2.Polynomial Then
            Return False
        Else
            For p = 0 To general1.Products.Count - 1
                If general1.Products(p) <> general2.Products(p) Then
                    Return False
                End If
            Next

            For e = 0 To general1.Exponentials.Count - 1
                If general1.Exponentials(e) <> general2.Exponentials(e) Then
                    Return False
                End If
            Next

            Return True
        End If
    End Operator

    Public Overloads Shared Operator <>(ByVal general1 As MFGeneral,
                                        ByVal general2 As MFGeneral) As Boolean

        Return Not general1 = general2
    End Operator

    Public Shared Function Add(ByVal general1 As MFGeneral,
                               ByVal general2 As MFGeneral) As MFGeneral

        If general2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, general2)
        End If
    End Function

    Public Shared Operator +(ByVal general1 As MFGeneral,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general1, general2)
    End Operator

    Public Shared Function Add(ByVal general1 As MFGeneral,
                               ByVal polynomial2 As MFPolynomial) As MFGeneral

        If polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, polynomial2)
        End If
    End Function

    Public Shared Operator +(ByVal general1 As MFGeneral,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Add(polynomial2, general1)
    End Operator

    Public Shared Function Add(ByVal polynomial1 As MFPolynomial,
                               ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, polynomial1)
    End Function

    Public Shared Operator +(ByVal polynomial1 As MFPolynomial,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, polynomial1)
    End Operator

    Public Shared Function Add(ByVal general1 As MFGeneral,
                               ByVal constant2 As Double) As MFGeneral

        If general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, New MFPolynomial(constant2))
        End If
    End Function

    Public Shared Operator +(ByVal general1 As MFGeneral,
                             ByVal constant2 As Double) As MFGeneral

        Return Add(general1, constant2)
    End Operator

    Public Shared Function Add(ByVal constant1 As Double,
                               ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, constant1)
    End Function

    Public Shared Operator +(ByVal constant1 As Double,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(constant1, general2)
    End Operator

    Public Shared Function Add(ByVal general1 As MFGeneral,
                               ByVal exponential2 As MFExponential) As MFGeneral

        If exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, exponential2)
        End If
    End Function

    Public Shared Operator +(ByVal general1 As MFGeneral,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Add(general1, exponential2)
    End Operator

    Public Shared Function Add(ByVal exponential1 As MFExponential,
                               ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, exponential1)
    End Function

    Public Shared Operator +(ByVal exponential1 As MFExponential,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, exponential1)
    End Operator

    Public Shared Function Add(ByVal general1 As MFGeneral,
                               ByVal product2 As MFProduct) As MFGeneral

        If product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, product2)
        End If
    End Function

    Public Shared Operator +(ByVal general1 As MFGeneral,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Add(general1, product2)
    End Operator

    Public Shared Function Add(ByVal product1 As MFProduct,
                               ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, product1)
    End Function

    Public Shared Operator +(ByVal product1 As MFProduct,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(general2, product1)
    End Operator

    Public Overloads Shared Function Negate(ByVal general1 As MFGeneral) As MFGeneral
        If general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return CType(general1.Negate, MFGeneral)
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral) As MFGeneral
        Return Negate(general1)
    End Operator

    Public Shared Function Subtract(ByVal general1 As MFGeneral,
                                    ByVal polynomial2 As MFPolynomial) As MFGeneral

        If general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        ElseIf polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        Else
            Return Add(MFPolynomial.Negate(polynomial2), general1)
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Subtract(general1, polynomial2)
    End Operator

    Public Shared Function Subtract(ByVal polynomial1 As MFPolynomial,
                                    ByVal general2 As MFGeneral) As MFGeneral

        Return Add(polynomial1, -general2)
    End Function

    Public Shared Operator -(ByVal polynomial1 As MFPolynomial,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Subtract(polynomial1, general2)
    End Operator

    Public Shared Function Subtract(ByVal general1 As MFGeneral,
                                    ByVal exponential2 As MFExponential) As MFGeneral

        If exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        Else
            Return Add(MFExponential.Negate(exponential2), general1)
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Subtract(general1, exponential2)
    End Operator

    Public Shared Function Subtract(ByVal exponential1 As MFExponential,
                                    ByVal general2 As MFGeneral) As MFGeneral

        Return Add(exponential1, -general2)
    End Function

    Public Shared Operator -(ByVal exponential1 As MFExponential,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Add(exponential1, -general2)
    End Operator

    Public Shared Function Subtract(ByVal general1 As MFGeneral,
                                    ByVal product2 As MFProduct) As MFGeneral

        If product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        Else
            Return Add(MFProduct.Negate(product2), general1)
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Subtract(general1, product2)
    End Operator

    Public Shared Function Subtract(ByVal general1 As MFGeneral,
                                    ByVal general2 As MFGeneral) As MFGeneral

        If general2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general2))
        Else
            Return Add(general1, CType(general2.Negate, MFGeneral))
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Subtract(general1, general2)
    End Operator

    Public Shared Function Subtract(ByVal general1 As MFGeneral,
                                    ByVal constant2 As Double) As MFGeneral

        If general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return New MFGeneral(general1, New MFPolynomial(-constant2))
        End If
    End Function

    Public Shared Operator -(ByVal general1 As MFGeneral,
                             ByVal constant2 As Double) As MFGeneral

        Return Subtract(general1, constant2)
    End Operator

    Public Shared Function Subtract(ByVal constant1 As Double,
                                    ByVal general2 As MFGeneral) As MFGeneral

        If general2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general2))
        Else
            Return New MFGeneral(New MFGeneral(New MFPolynomial(constant1), -general2))
        End If
    End Function

    Public Shared Operator -(ByVal constant1 As Double,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Subtract(constant1, general2)
    End Operator

    Public Shared Function Multiply(ByVal general1 As MFGeneral,
                                    ByVal polynomial2 As MFPolynomial) As MFGeneral

        If polynomial2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(polynomial2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else

            Dim genOut As New MFGeneral

            For Each i In general1

                Dim poly As MFPolynomial = TryCast(i, MFPolynomial)

                If poly IsNot Nothing Then
                    genOut = Add(genOut, MFPolynomial.Multiply(polynomial2, poly))
                Else
                    Dim exp As MFExponential = TryCast(i, MFExponential)

                    If exp IsNot Nothing Then
                        genOut = Add(genOut, MFPolynomial.Multiply(polynomial2, exp))
                    Else
                        Dim prod As MFProduct = TryCast(i, MFProduct)

                        If prod IsNot Nothing Then
                            genOut = Add(genOut, MFProduct.Multiply(polynomial2, prod))
                        Else
                            Dim gen As MFGeneral = TryCast(i, MFGeneral)

                            If gen IsNot Nothing Then
                                genOut = Add(genOut, Multiply(polynomial2, gen))
                            Else
                                Throw New ArgumentOutOfRangeException(NameOf(polynomial2))
                            End If
                        End If
                    End If
                End If
            Next

            Return genOut

        End If
    End Function

    Public Shared Operator *(ByVal general1 As MFGeneral,
                             ByVal polynomial2 As MFPolynomial) As MFGeneral

        Return Multiply(general1, polynomial2)
    End Operator

    Public Shared Function Multiply(ByVal polynomial2 As MFPolynomial,
                                    ByVal general1 As MFGeneral) As MFGeneral

        Return Multiply(general1, polynomial2)
    End Function

    Public Shared Operator *(ByVal polynomial1 As MFPolynomial,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(polynomial1, general2)
    End Operator

    Public Shared Function Multiply(ByVal general1 As MFGeneral,
                                    ByVal constant2 As Double) As MFGeneral

        If general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Dim genOut As New MFGeneral

            For Each i In general1
                Dim poly As MFPolynomial = TryCast(i, MFPolynomial)

                If poly IsNot Nothing Then
                    genOut = Add(genOut, MFPolynomial.Multiply(poly, constant2))
                Else

                    Dim exp As MFExponential = TryCast(i, MFExponential)

                    If exp IsNot Nothing Then
                        genOut = Add(genOut, MFExponential.Multiply(exp, constant2))
                    Else
                        Dim prod As MFProduct = TryCast(i, MFProduct)

                        If prod IsNot Nothing Then
                            genOut = Add(genOut, MFProduct.Multiply(prod, constant2))
                        Else
                            Dim gen As MFGeneral = TryCast(i, MFGeneral)

                            If gen IsNot Nothing Then
                                genOut = Add(genOut, Multiply(constant2, gen))
                            Else
                                Throw New ArgumentOutOfRangeException(NameOf(general1))
                            End If
                        End If
                    End If
                End If
            Next

            Return genOut
        End If
    End Function

    Public Shared Operator *(ByVal general1 As MFGeneral,
                             ByVal constant2 As Double) As MFGeneral

        Return Multiply(constant2, general1)
    End Operator

    Public Shared Function Multiply(ByVal constant2 As Double,
                                    ByVal general1 As MFGeneral) As MFGeneral

        Return Multiply(general1, constant2)
    End Function

    Public Shared Operator *(ByVal constant2 As Double,
                             ByVal general1 As MFGeneral) As MFGeneral

        Return Multiply(general1, constant2)
    End Operator

    Public Shared Function Multiply(ByVal general1 As MFGeneral,
                                    ByVal exponential2 As MFExponential) As MFGeneral

        If exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else

            Dim genOut As New MFGeneral

            For Each i In general1

                Dim poly As MFPolynomial = TryCast(i, MFPolynomial)

                If poly IsNot Nothing Then
                    genOut = Add(genOut, MFExponential.Multiply(exponential2, poly))
                Else

                    Dim exp As MFExponential = TryCast(i, MFExponential)

                    If exp IsNot Nothing Then
                        genOut = Add(genOut, MFExponential.Multiply(exponential2, exp))
                    Else
                        Dim prod As MFProduct = TryCast(i, MFProduct)

                        If prod IsNot Nothing Then
                            genOut = Add(genOut, MFProduct.Multiply(exponential2, prod))
                        Else
                            Dim gen As MFGeneral = TryCast(i, MFGeneral)

                            If gen IsNot Nothing Then
                                genOut = Add(genOut, Multiply(exponential2, gen))
                            Else
                                Throw New ArgumentOutOfRangeException(NameOf(general1))
                            End If
                        End If
                    End If
                End If
            Next

            Return genOut

        End If
    End Function

    Public Shared Operator *(ByVal general1 As MFGeneral,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Multiply(general1, exponential2)
    End Operator

    Public Shared Function Multiply(ByVal exponential1 As MFExponential,
                                    ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(general2, exponential1)
    End Function

    Public Shared Operator *(ByVal exponential1 As MFExponential,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(exponential1, general2)
    End Operator

    Public Shared Function Multiply(ByVal general1 As MFGeneral,
                                    ByVal product2 As MFProduct) As MFGeneral

        If product2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(product2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else

            Dim genOut As New MFGeneral

            For Each i In general1

                Dim poly As MFPolynomial = TryCast(i, MFPolynomial)

                If poly IsNot Nothing Then
                    genOut = Add(genOut, MFProduct.Multiply(product2, poly))
                Else

                    Dim exp As MFExponential = TryCast(i, MFExponential)

                    If exp IsNot Nothing Then
                        genOut = Add(genOut, MFProduct.Multiply(product2, exp))
                    Else
                        Dim prod As MFProduct = TryCast(i, MFProduct)

                        If prod IsNot Nothing Then
                            genOut = Add(genOut, MFProduct.Multiply(product2, prod))
                        Else
                            Dim gen As MFGeneral = TryCast(i, MFGeneral)

                            If gen IsNot Nothing Then
                                genOut = Add(genOut, MFGeneral.Multiply(product2, gen))
                            Else
                                Throw New ArgumentOutOfRangeException(NameOf(product2))
                            End If
                        End If
                    End If
                End If
            Next

            Return genOut

        End If
    End Function

    Public Shared Operator *(ByVal general1 As MFGeneral,
                             ByVal product2 As MFProduct) As MFGeneral

        Return Multiply(general1, product2)
    End Operator

    Public Shared Function Multiply(ByVal product1 As MFProduct,
                                    ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(general2, product1)
    End Function

    Public Shared Operator *(ByVal product1 As MFProduct,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(product1, general2)
    End Operator

    Public Shared Function Multiply(ByVal general1 As MFGeneral,
                                    ByVal general2 As MFGeneral) As MFGeneral

        If general2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else

            Dim genOut As New MFGeneral

            For Each i In general1

                Dim poly As MFPolynomial = TryCast(i, MFPolynomial)

                If poly IsNot Nothing Then
                    genOut = Add(genOut, Multiply(general2, poly))
                Else

                    Dim exp As MFExponential = TryCast(i, MFExponential)

                    If exp IsNot Nothing Then
                        genOut = Add(genOut, Multiply(general2, exp))
                    Else
                        Dim prod As MFProduct = TryCast(i, MFProduct)

                        If prod IsNot Nothing Then
                            genOut = Add(genOut, Multiply(general2, prod))
                        Else
                            Dim gen As MFGeneral = TryCast(i, MFGeneral)

                            If gen IsNot Nothing Then
                                genOut = Add(genOut, Multiply(general2, gen))
                            Else
                                Throw New ArgumentOutOfRangeException(NameOf(general2))
                            End If
                        End If
                    End If
                End If
            Next

            Return genOut

        End If
    End Function

    Public Shared Operator *(ByVal general1 As MFGeneral,
                             ByVal general2 As MFGeneral) As MFGeneral

        Return Multiply(general1, general2)
    End Operator

    Public Shared Function Divide(ByVal general1 As MFGeneral,
                                  ByVal exponential2 As MFExponential) As MFGeneral

        If exponential2 Is Nothing Then
            Throw New ArgumentNullException(NameOf(exponential2))
        ElseIf general1 Is Nothing Then
            Throw New ArgumentNullException(NameOf(general1))
        Else
            Return Multiply(general1, MFExponential.Invert(exponential2))
        End If
    End Function

    Public Shared Operator /(ByVal general1 As MFGeneral,
                             ByVal exponential2 As MFExponential) As MFGeneral

        Return Divide(general1, exponential2)
    End Operator

    Public Shared Function Divide(ByVal general1 As MFGeneral,
                                  ByVal constant2 As Double) As MFGeneral

        Return Multiply(general1, 1.0# / constant2)
    End Function

    Public Shared Operator /(ByVal general1 As MFGeneral,
                             ByVal constant2 As Double) As MFGeneral

        Return Divide(general1, constant2)
    End Operator

    Private Iterator Function GetEnumerator() As IEnumerator(Of MFBase) Implements IEnumerable(Of MFBase).GetEnumerator
        For Each p In Products
            Yield p
        Next
        For Each e In Exponentials
            Yield e
        Next
        Yield Polynomial
    End Function

    Private Iterator Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Yield GetEnumerator()
    End Function

End Class
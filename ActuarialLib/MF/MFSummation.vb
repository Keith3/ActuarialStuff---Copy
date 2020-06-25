Imports ActuarialLib

Public Class MFSummationCollection
    Inherits MFBase

    'Expression in the form of MFProduct + MFProduct + ... + MFProduct

    Implements IEnumerable(Of MFProduct)

    Private ReadOnly _summation As New List(Of MFProduct)

    Default Public ReadOnly Property Item(ByVal index As Integer) As MFProduct
        Get
            Return _summation(index)
        End Get
    End Property

    Public Overloads Shared Function XIsValid(xValue As Double) As Boolean
        Return MFProduct.XIsValid(xValue)
    End Function

    'Public Overloads Shared Function XRangeIsValid(xRange As Range(Of Double)) As Boolean
    '    Return MFProduct.XRangeIsValid(xRange)
    'End Function

    Public Overrides Function Evaluate(xValue As Double) As Double
        Dim value As Double = 0
        For Each p In _summation
            value += p.Evaluate(xValue)
        Next
        Return value
    End Function

    Public Overrides Function IndefiniteIntegral() As MFBase
        Dim s As New MFSummationCollection
        Dim pIntegral As MFProduct
        For Each p In _summation
            pIntegral = CType(p.IndefiniteIntegral, MFProduct)
            s.AddProduct(pIntegral.Polynomial, pIntegral.Exponential)
        Next
        Return s
    End Function

    Public Overrides Function IndefiniteDerivative() As MFBase
        Dim s As New MFSummationCollection
        Dim pDerivative As MFProduct
        For Each p In _summation
            pDerivative = CType(p.IndefiniteDerivative, MFProduct)
            s.AddProduct(pDerivative.Polynomial, pDerivative.Exponential)
        Next
        Return s
    End Function

    Public Overrides Function ToString() As String
        Dim x As String = ""
        For i = 0 To _summation.Count - 1
            If _summation(i).ToString.Length > 0 Then
                If x.Length > 0 Then
                    x &= " + "
                End If
                x &= _summation(i).ToString
            End If
        Next
        Return x
    End Function

    Public Sub AddProduct(ByVal constant As Double)
        'Add the new element to the summation
        If constant <> 0 Then
            _summation.Add(New MFProduct(New MFPolynomial(constant), New MFExponential(1, 1)))
        End If
    End Sub

    Public Sub AddProduct(ByVal polynomial As MFPolynomial)
        _summation.Add(New MFProduct(polynomial, New MFExponential(1, 1)))
    End Sub

    Public Sub AddProduct(ByVal exponential As MFExponential)
        _summation.Add(New MFProduct(New MFPolynomial(1), exponential))
    End Sub

    Public Sub AddProduct(ByVal polynomial As MFPolynomial, ByVal exponential As MFExponential)
        _summation.Add(New MFProduct(polynomial, exponential))
    End Sub

    Public Sub AddProduct(ByVal product As MFProduct)
        _summation.Add(product)
    End Sub

    Public Sub AddSummation(ByVal summation As MFSummationCollection)
        If summation IsNot Nothing Then
            For Each p In summation
                _summation.Add(p)
            Next
        End If
    End Sub

    'Public Sub AddProduct(ByVal constant As Double,
    '                      ByVal polynomial As MFPolynomial,
    '                      ByVal exponential As MFExponential)

    '    'Add the new element to the summation
    '    If constant <> 0 Then
    '        _summation.Add(New MFProduct(constant, polynomial, exponential))
    '    End If
    'End Sub

    Public Sub SubtractProduct(ByVal constant As Double)

        'Add the negated element to the summation
        If constant <> 0 Then
            _summation.Add(New MFProduct(New MFPolynomial(-constant), New MFExponential(1, 1)))
        End If
    End Sub

    Public Sub SubtractProduct(ByVal polynomial As MFPolynomial)
        _summation.Add(New MFProduct(-polynomial, New MFExponential(1, 1)))
    End Sub

    Public Sub SubtractProduct(ByVal exponential As MFExponential)
        _summation.Add(New MFProduct(New MFPolynomial(-1), exponential))
    End Sub

    'Public Sub SubtractProduct(ByVal constant As Double,
    '                           ByVal polynomial As MFPolynomial,
    '                           ByVal exponential As MFExponential)

    '    'Add the negated element to the summation
    '    If constant <> 0 Then
    '        _summation.Add(New MFProduct(-constant, polynomial, exponential))
    '    End If
    'End Sub

    Public Sub SubtractProduct(ByVal summation As MFSummationCollection)
        If summation IsNot Nothing Then
            For Each p In summation
                _summation.Add(p)
            Next
        End If
    End Sub

    Public Sub MultiplyProduct(ByVal constant As Double)
        For Each f In _summation
            f = New MFProduct(f.Polynomial * constant, f.Exponential)
        Next
    End Sub

    Public Sub MultiplyProduct(ByVal polynomial As MFPolynomial)
        For Each f In _summation
            f = New MFProduct(f.Polynomial * polynomial, f.Exponential)
        Next
    End Sub

    Public Sub MultiplyProduct(ByVal exponential As MFExponential)
        For Each f In _summation
            f = New MFProduct(f.Polynomial, f.Exponential * exponential)
        Next
    End Sub

    Public Sub MultiplyProduct(ByVal polynomial As MFPolynomial,
                               ByVal exponential As MFExponential)

        'Multiply each element of the summation by the appropriate function
        For Each f In _summation
            f = New MFProduct(f.Polynomial * polynomial, f.Exponential * exponential)
        Next
    End Sub

    Public Sub DivideProduct(ByVal constant As Double)
        For Each f In _summation
            f = New MFProduct(f.Polynomial / constant, f.Exponential)
        Next
    End Sub

    Public Sub DivideProduct(ByVal exponential As MFExponential)
        For Each f In _summation
            f = New MFProduct(f.Polynomial, f.Exponential / exponential)
        Next
    End Sub

    'Public Sub DivideProduct(ByVal constant As Double,
    '                         ByVal exponential As MFExponential)

    '    'Multiply each element of the summation by the inverse of the appropriate function
    '    For Each f In _summation
    '        f.Divide(exponential)
    '        f.Divide(constant)
    '    Next
    'End Sub

    Public Function GetEnumerator() As IEnumerator(Of MFProduct) Implements IEnumerable(Of MFProduct).GetEnumerator
        Return _summation.GetEnumerator
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return _summation.GetEnumerator
    End Function

    Public Overrides Function Negate() As MFBase
        Throw New NotImplementedException()
    End Function

    Public Overrides Function Clone() As MFBase
        Throw New NotImplementedException()
    End Function
End Class
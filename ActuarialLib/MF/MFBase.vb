Public MustInherit Class MFBase

    Public MustOverride Function Evaluate(ByVal atValue As Double) As Double
    Public MustOverride Function IndefiniteIntegral() As MFBase
    Public MustOverride Function IndefiniteDerivative() As MFBase
    Public MustOverride Function Negate() As MFBase
    Public MustOverride Function Clone() As MFBase

    Public Shared Function AtValueIsValid(ByVal atValue As Double) As Boolean
        Return True
    End Function

    Public Overridable Function DefiniteIntegral(ByVal fromX As Double,
                                                 ByVal toX As Double) As Double

        If Not AtValueIsValid(fromX) Then
            Throw New ArgumentOutOfRangeException(NameOf(fromX))
        ElseIf Not AtValueIsValid(toX) Then
            Throw New ArgumentOutOfRangeException(NameOf(toX))
        Else
            If fromX = toX Then
                Return 0
            Else
                Dim integral As MFBase = IndefiniteIntegral()
                Return integral.Evaluate(toX) - integral.Evaluate(fromX)
            End If
        End If
    End Function

    Public Overridable Function DefiniteIntegral(ByVal overRange As Range(Of Double)) As Double
        If overRange IsNot Nothing Then
            Return DefiniteIntegral(overRange.MaxValue, overRange.MinValue)
        Else
            Throw New ArgumentNullException(NameOf(overRange))
        End If
    End Function

    Public Overridable Function DefiniteDerivative(ByVal atX As Double) As Double
        If AtValueIsValid(atX) Then
            Return IndefiniteDerivative.Evaluate(atX)
        Else
            Throw New ArgumentOutOfRangeException(NameOf(atX))
        End If
    End Function

    Private Sub MathAssertions()

        Debug.Assert((5 + New MFPolynomial(3, -5, 7)).ToString = "(8 - 5x + 7x^2)")
        Debug.Assert((5 - New MFPolynomial(3, -5, 7)).ToString = "(2 + 5x - 7x^2)")
        Debug.Assert((5 * New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2)")

        Debug.Assert((5 + New MFExponential(1.03, 5)).ToString = "5 * 1.03^x + 5")
        Debug.Assert((5 - New MFExponential(1.03, 5)).ToString = "-5 * 1.03^x + 5")
        Debug.Assert((5 * New MFExponential(1.03, 5)).ToString = "25 * 1.03^x")
        Debug.Assert((5 / New MFExponential(1.03, 5)).ToString = "0.970873786407767^x")

        Debug.Assert((5 + New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(15 - 25x + 35x^2) * 1.03^x + 5")
        Debug.Assert((5 - New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(-15 + 25x - 35x^2) * 1.03^x + 5")
        Debug.Assert((5 * New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(75 - 125x + 175x^2) * 1.03^x")

        Debug.Assert((5 + (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)) + (New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + 5")
        Debug.Assert((5 - (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)) + (New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(-15 + 25x - 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + 5")
        Debug.Assert((5 * (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)) + (New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(75 - 125x + 175x^2) * 1.03^x + (-7 - 91x) * 1.05^x")

        Debug.Assert(MFPolynomial.Negate(New MFPolynomial(3, -5, 7)).ToString = "(-3 + 5x - 7x^2)")

        Debug.Assert((New MFPolynomial(3, -5, 7) + New MFPolynomial(1, 13)).ToString = "(4 + 8x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - New MFPolynomial(1, 13)).ToString = "(2 - 18x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFPolynomial(1, 13)).ToString = "(3 + 34x - 58x^2 + 91x^3)")

        Debug.Assert((New MFPolynomial(3, -5, 7) + 5).ToString = "(8 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - 5).ToString = "(-2 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * 5).ToString = "(15 - 25x + 35x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) / 5).ToString = "(0.6 - x + 1.4x^2)")

        Debug.Assert((New MFPolynomial(3, -5, 7) + New MFExponential(1.03, 5)).ToString = "5 * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - New MFExponential(1.03, 5)).ToString = "-5 * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(15 - 25x + 35x^2) * 1.03^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) / New MFExponential(1.03, 5)).ToString = "(0.6 - x + 1.4x^2) * 0.970873786407767^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) + New MFExponential(1.03, 5) * New MFPolynomial(1, 13)).ToString = "(5 + 65x) * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - New MFExponential(1.03, 5) * New MFPolynomial(1, 13)).ToString = "(-5 - 65x) * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) * New MFPolynomial(1, 13)).ToString = "(15 + 170x - 290x^2 + 455x^3) * 1.03^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) + (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(-15 + 25x - 35x^2) * 1.03^x + (7 + 91x) * 1.05^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(45 - 150x + 335x^2 - 350x^3 + 245x^4) * 1.03^x + (-21 - 238x + 406x^2 - 637x^3) * 1.05^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) + (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, -7))).ToString = "(8 - 116x + 35x^2) * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) - (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, -7))).ToString = "(-8 + 116x - 35x^2) * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, -7))).ToString = "(24 - 388x + 741x^2 - 987x^3 + 245x^4) * 1.03^x")

        Debug.Assert((-New MFExponential(1.03, 5)).ToString = "-5 * 1.03^x")

        Debug.Assert(New MFExponential(1.03, 5).Invert.ToString = "0.2 * 0.970873786407767^x")

        Debug.Assert((New MFExponential(1.03, 5) + New MFExponential(1.05, -7)).ToString = "5 * 1.03^x + -7 * 1.05^x")
        Debug.Assert((New MFExponential(1.03, 5) - New MFExponential(1.05, -7)).ToString = "5 * 1.03^x + 7 * 1.05^x")
        Debug.Assert((New MFExponential(1.03, 5) * New MFExponential(1.05, -7)).ToString = "-35 * 1.0815^x")
        Debug.Assert((New MFExponential(1.03, 5) / New MFExponential(1.05, -7)).ToString = "-0.714285714285714 * 0.980952380952381^x")
        Debug.Assert((New MFExponential(1.03, 5) + New MFExponential(1.03, -7)).ToString = "-2 * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) - New MFExponential(1.03, -7)).ToString = "12 * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) * New MFExponential(1.03, -7)).ToString = "-35 * 1.0609^x")
        Debug.Assert((New MFExponential(1.03, 5) / New MFExponential(1.03, -7)).ToString = "-0.714285714285714")

        Debug.Assert((New MFExponential(1.03, 5) + New MFPolynomial(3, -5, 7)).ToString = "5 * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFExponential(1.03, 5) - New MFPolynomial(3, -5, 7)).ToString = "5 * 1.03^x + (-3 + 5x - 7x^2)")
        Debug.Assert((New MFExponential(1.03, 5) * New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x")

        Debug.Assert((New MFExponential(1.03, 5) + 5).ToString = "5 * 1.03^x + 5")
        Debug.Assert((New MFExponential(1.03, 5) - 5).ToString = "5 * 1.03^x + -5")
        Debug.Assert((New MFExponential(1.03, 5) * 5).ToString = "25 * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) / 5).ToString = "1.03^x")

        Debug.Assert((New MFExponential(1.03, 5) + New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7)).ToString = "(-21 + 35x - 49x^2) * 1.05^x + 5 * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) - New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7)).ToString = "(21 - 35x + 49x^2) * 1.05^x + 5 * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) * New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7)).ToString = "(-105 + 175x - 245x^2) * 1.0815^x")
        Debug.Assert((New MFExponential(1.03, 5) + New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 3)).ToString = "(14 - 15x + 21x^2) * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) - New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 3)).ToString = "(-4 + 15x - 21x^2) * 1.03^x")
        Debug.Assert((New MFExponential(1.03, 5) * New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 3)).ToString = "(45 - 75x + 105x^2) * 1.0609^x")

        Debug.Assert((New MFExponential(1.03, 5) + (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(20 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert((New MFExponential(1.03, 5) - (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(-10 + 25x - 35x^2) * 1.03^x + (7 + 91x) * 1.05^x")
        Debug.Assert((New MFExponential(1.03, 5) * (New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(75 - 125x + 175x^2) * 1.0609^x + (-35 - 455x) * 1.0815^x + 1.03^x")

        Debug.Assert((-(New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5))).ToString = "(-15 + 25x - 35x^2) * 1.03^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) - New MFPolynomial(1, 13) * New MFExponential(1.05, -7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (7 + 91x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) * New MFPolynomial(1, 13) * New MFExponential(1.05, -7)).ToString = "(-105 - 1190x + 2030x^2 - 3185x^3) * 1.0815^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) - New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-3 + 5x - 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) * New MFPolynomial(3, -5, 7)).ToString = "(45 - 150x + 335x^2 - 350x^3 + 245x^4) * 1.03^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFExponential(1.03, 5)).ToString = "(20 - 25x + 35x^2) * 1.03^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) - New MFExponential(1.03, 5)).ToString = "(10 - 25x + 35x^2) * 1.03^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) * New MFExponential(1.03, 5)).ToString = "(75 - 125x + 175x^2) * 1.0609^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) / New MFExponential(1.03, 5)).ToString = "(3 - 5x + 7x^2) * 1")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + 5")
        Debug.Assert(((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)) - 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + -5")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) * 5).ToString = "(75 - 125x + 175x^2) * 1.03^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) / 5).ToString = "(3 - 5x + 7x^2) * 1.03^x")

        Debug.Assert((-(New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7))).ToString = "(-15 + 25x - 35x^2) * 1.03^x + (7 + 91x) * 1.05^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) + (New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7) + New MFPolynomial(1, 13) * New MFExponential(1.03, 5))).ToString = "(20 + 40x + 35x^2) * 1.03^x + (-28 - 56x - 49x^2) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) - (New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7) + New MFPolynomial(1, 13) * New MFExponential(1.03, 5))).ToString = "(10 - 90x + 35x^2) * 1.03^x + (14 - 126x + 49x^2) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) * (New MFPolynomial(3, -5, 7) * New MFExponential(1.05, -7) + New MFPolynomial(1, 13) * New MFExponential(1.03, 5))).ToString = "(147 + 1666x - 2842x^2 + 4459x^3) * 1.1025^x + (-35 - 910x - 5915x^2) * 1.0815^x + 1.05^x + (15 - 25x + 35x^2) * 1.03^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) + New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(30 - 50x + 70x^2) * 1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) - New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) * New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-105 - 1190x + 2030x^2 - 3185x^3) * 1.0815^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) + New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + (3 - 5x + 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) - New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + (-3 + 5x - 7x^2)")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) * New MFPolynomial(3, -5, 7)).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-21 - 238x + 406x^2 - 637x^3) * 1.05^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) + New MFExponential(1.03, 5)).ToString = "(20 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) - New MFExponential(1.03, 5)).ToString = "(10 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x")
        Debug.Assert(((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7)) * New MFExponential(1.03, 5)).ToString = "(75 - 125x + 175x^2) * 1.0609^x + (-35 - 455x) * 1.0815^x + 1.03^x")
        Debug.Assert(((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7)) / New MFExponential(1.03, 5)).ToString = "(3 - 5x + 7x^2) * 1 + (-1.4 - 18.2x) * 1.01941747572816^x + 0.970873786407767^x")

        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) + 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + 5")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) - 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-7 - 91x) * 1.05^x + -5")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) * 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-35 - 455x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.05, -7) / 5).ToString = "(15 - 25x + 35x^2) * 1.03^x + (-1.4 - 18.2x) * 1.05^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, 3) + 5).ToString = "(18 + 14x + 35x^2) * 1.03^x + 5")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, 3) - 5).ToString = "(18 + 14x + 35x^2) * 1.03^x + -5")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, 3) * 5).ToString = "(30 + 170x + 35x^2) * 1.03^x")
        Debug.Assert((New MFPolynomial(3, -5, 7) * New MFExponential(1.03, 5) + New MFPolynomial(1, 13) * New MFExponential(1.03, 3) / 5).ToString = "(15.6 - 17.2x + 35x^2) * 1.03^x")

    End Sub

End Class
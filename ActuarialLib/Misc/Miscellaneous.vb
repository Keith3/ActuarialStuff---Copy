Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Globalization

Public Module Miscellaneous

    Public Function ConvertIntArrayToString(ByVal separator As String,
                                            ByVal intArray As Integer()) As String

        If intArray Is Nothing Then Throw New ArgumentNullException(NameOf(intArray))

        Return String.Join(separator, Array.ConvertAll(intArray, Function(n) n.ToString(CultureInfo.CurrentCulture)))
    End Function

    Public Function ConvertStringToIntArray(ByVal separator As Char,
                                            ByVal valueIn As String) As Integer()

        If valueIn Is Nothing Then Throw New ArgumentNullException(NameOf(valueIn))

        Return Array.ConvertAll(valueIn.Split(separator), Function(s) Convert.ToInt32(s, CultureInfo.CurrentCulture))
    End Function

    Public Function MyJoin(separator As String,
                           ByVal values As IEnumerable(Of String)) As String

        If values Is Nothing Then Throw New ArgumentNullException(NameOf(values))

        Return MyJoin(separator, values.ToArray)
    End Function

    Public Function MyJoin(ByVal separator As String,
                           ByVal ParamArray values() As String) As String

        If values Is Nothing Then Throw New ArgumentNullException(NameOf(values))

        Return String.Join(separator, Array.ConvertAll(values, Function(n) n.ToString(CultureInfo.CurrentCulture)))
    End Function

    <Extension()>
    Public Function Includes(ByVal input As String,
                             ByVal searchOptions As RegexOptions,
                             ByVal ParamArray searchPatterns() As String) As Boolean

        Return searchPatterns.Any(Function(p) Regex.IsMatch(input, p, searchOptions))
    End Function

    <Extension()>
    Public Function AddParens(ByVal inValue As String) As String
        Return New StringBuilder("(").Append(inValue).Append(")").ToString
    End Function

    <Extension()>
    Public Function AddBrackets(ByVal inValue As String) As String
        Return New StringBuilder("[").Append(inValue).Append("]").ToString
    End Function

    <Extension()>
    Public Function AddBraces(ByVal inValue As String) As String
        Return New StringBuilder("{").Append(inValue).Append("}").ToString
    End Function

    <Extension()>
    Public Function AddQuotes(ByVal inValue As String) As String
        Return New StringBuilder("""").Append(inValue).Append("""").ToString
    End Function

End Module
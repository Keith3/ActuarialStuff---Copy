Imports ActuarialLib

Public Class SoaTableComparer

    Private _Table1 As SoaTable
    Private _Table2 As SoaTable

    Delegate Sub CompareResults(ByVal table1 As SoaTable, ByVal table2 As SoaTable)
    Private displayResult As CompareResults = AddressOf CompareDescription

    Public Property Table1 As SoaTable
        Get
            Return _Table1
        End Get
        Set(value As SoaTable)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            _Table1 = value
            If Table2 IsNot Nothing Then displayResult.Invoke(_Table1, _Table2)
        End Set
    End Property

    Public Property Table2 As SoaTable
        Get
            Return _Table2
        End Get
        Set(value As SoaTable)
            If value Is Nothing Then Throw New ArgumentNullException(NameOf(value))
            _Table2 = value
            If _Table1 IsNot Nothing Then displayResult.Invoke(_Table1, _Table2)
        End Set
    End Property

    Private Sub TabItem_GotFocus(sender As Object,
                                 e As RoutedEventArgs) Handles tabItemDescription.GotFocus,
                                                               tabItemStructure.GotFocus,
                                                               tabItemValuesText.GotFocus
        Select Case CType(sender, TabItem).Name
            Case "tabItemDescription" : displayResult = AddressOf CompareDescription
            Case "tabItemStructure" : displayResult = AddressOf CompareStructure
            Case "tabItemValuesText" : displayResult = AddressOf CompareValuesText
                'Case "tabItemValuesChart" : dr = AddressOf CompareValuesChart
                'Case "tabItemRatiosText" : dr = AddressOf CompareRatiosText
                'Case "tabItemRatiosChart" : dr = AddressOf CompareRatiosChart
            Case Else : Exit Sub
        End Select

        displayResult.Invoke(_Table1, _Table2)
    End Sub


    Private Sub CompareDescription(ByVal table1 As SoaTable,
                                   ByVal table2 As SoaTable)

        textBoxesDescription.Text1 = table1.DisplayDescription
        textBoxesDescription.Text2 = table2.DisplayDescription
    End Sub

    Private Sub CompareStructure(ByVal table1 As SoaTable,
                                 ByVal table2 As SoaTable)

        textBoxesStructure.Text1 = table1.DisplayStructure
        textBoxesStructure.Text2 = table2.DisplayStructure
    End Sub

    Private Sub CompareValuesText(ByVal table1 As SoaTable,
                                  ByVal table2 As SoaTable)

        textBoxesValues.Text1 = table1.DisplayValues
        textBoxesValues.Text2 = table2.DisplayValues
    End Sub

    'Private Sub CompareValuesChart(ByVal table1 As SoaTable,
    '                               ByVal table2 As SoaTable)

    'End Sub

    'Private Sub CompareRatiosText(ByVal table1 As SoaTable,
    '                              ByVal table2 As SoaTable)

    'End Sub

    'Private Sub CompareRatiosChart(ByVal table1 As SoaTable,
    '                               ByVal table2 As SoaTable)

    'End Sub

End Class
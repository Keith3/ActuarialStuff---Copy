Public Class TextBoxComparer

    Public Property Wrapping As TextWrapping
        Get
            Return textBoxOne.TextWrapping
        End Get
        Set(value As TextWrapping)
            textBoxOne.TextWrapping = value
            textBoxTwo.TextWrapping = value
        End Set
    End Property

    Public Property IsReadOnly As Boolean
        Get
            Return textBoxOne.IsReadOnly
        End Get
        Set(value As Boolean)
            textBoxOne.IsReadOnly = value
            textBoxTwo.IsReadOnly = value
        End Set
    End Property

    Public Property Text1 As String
        Get
            Return textBoxOne.Text
        End Get
        Set(value As String)
            textBoxOne.Text = value
        End Set
    End Property

    Public Property Text2 As String
        Get
            Return textBoxTwo.Text
        End Get
        Set(value As String)
            textBoxTwo.Text = value
        End Set
    End Property

End Class
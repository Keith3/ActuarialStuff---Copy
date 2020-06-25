Public Class Person

    Public ReadOnly Property PersonKey As Long
    Public Property NameLast As String
    Public Property NameFirst As String
    Public Property NameMiddle As String

    Public Sub New(ByVal nameLast As String,
                    ByVal nameFirst As String,
                    ByVal nameMiddle As String)

        '_PersonKey = Unique
        _NameLast = nameLast
        _NameFirst = nameFirst
        _NameMiddle = nameMiddle
    End Sub

End Class

Public Class InsuredPerson
    Inherits Person

    Public Property DateOfBirth As Date
    Public Property Sex As String

    Public Sub New(ByVal nameLast As String,
                   ByVal nameFirst As String,
                   ByVal nameMiddle As String,
                   ByVal dateOfBirth As Date,
                   ByVal sex As String)

        MyBase.New(nameLast, nameFirst, nameMiddle)

        _DateOfBirth = dateOfBirth
        _Sex = sex
    End Sub

End Class
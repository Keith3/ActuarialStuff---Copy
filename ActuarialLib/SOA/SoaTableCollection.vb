Imports System.Xml
Imports System.IO
Imports System.IO.Compression

Public Class SoaTableCollection

    Implements IEnumerable(Of SoaTable)

    Public ReadOnly Property CurrentSortOrder As String = ""
    Private ReadOnly _Tables As New List(Of SoaTable)

    Public Sub New()
    End Sub

    Public Sub LoadEmbeddedGZipDatabase()

        Dim a As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()
        Dim settings As New XmlReaderSettings() With {.IgnoreWhitespace = True,
                                                      .IgnoreComments = True}

        Dim strm As IO.Stream = a.GetManifestResourceStream("ActuarialLib.SOA.gz")

        Dim decompressedStream As GZipStream = New GZipStream(strm, CompressionMode.Decompress)

        Using rdr As XmlReader = XmlReader.Create(decompressedStream, settings)
            LoadXmlStream(rdr)
        End Using

    End Sub

    Public Sub LoadEmbeddedXmlDatabase()

        Dim a As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()
        Dim settings As New XmlReaderSettings() With {.IgnoreWhitespace = True,
                                                      .IgnoreComments = True}

        Dim strm As IO.Stream = a.GetManifestResourceStream("ActuarialLib.SOA.xml")

        Using rdr As XmlReader = XmlReader.Create(strm, settings)
            LoadXmlStream(rdr)
        End Using

    End Sub

    Public Sub LoadXmlDatabase(ByVal fileToLoad As String)

        If fileToLoad IsNot Nothing AndAlso File.Exists(fileToLoad) Then
            Dim settings As New XmlReaderSettings() With {.IgnoreWhitespace = True,
                                                          .IgnoreComments = True}

            Using rdr As XmlReader = XmlReader.Create(fileToLoad, settings)
                LoadXmlStream(rdr)
            End Using
        End If

    End Sub

    'Public Sub LoadDatabaseFolder(ByVal folderToLoad As DirectoryInfo)
    '    If folderToLoad IsNot Nothing AndAlso folderToLoad.Exists Then
    '        Dim files As FileInfo() = folderToLoad.GetFiles("t*.xml", SearchOption.TopDirectoryOnly)
    '        Parallel.ForEach(files, Sub(table)
    '                                    LoadIndividualFile(table.FullName)
    '                                End Sub)
    '    End If
    'End Sub

    Public Sub LoadSelectedFiles(ByVal filesToLoad() As String)

        If filesToLoad IsNot Nothing Then
            ''Parallel.ForEach(filesToLoad, Sub(table)
            ''                                  LoadIndividualFile(table)
            ''                              End Sub)
            For Each table In filesToLoad
                LoadIndividualFile(table)
            Next
        End If
    End Sub

    Private Sub LoadIndividualFile(ByVal fileToLoad As String)
        Dim settings As New XmlReaderSettings() With {.IgnoreWhitespace = True,
                                                      .IgnoreComments = True}

        If fileToLoad IsNot Nothing AndAlso File.Exists(fileToLoad) Then
            Using rdr As XmlReader = XmlReader.Create(fileToLoad, settings)
                LoadXmlStream(rdr)
            End Using
        End If
    End Sub

    Private Sub LoadXmlStream(ByVal rdr As XmlReader)

        rdr.MoveToContent()

        While Not rdr.EOF
            If rdr.NodeType = XmlNodeType.Element AndAlso rdr.Name = "XTbML" Then
                _Tables.Add(New SoaTable(TryCast(XNode.ReadFrom(rdr), XElement)))
            Else
                rdr.Read()
            End If
        End While

    End Sub

    Public Sub Add(table As SoaTable)
        _Tables.Add(table)
    End Sub

    Public Sub AddRange(tables As IEnumerable(Of SoaTable))
        If tables Is Nothing Then Throw New ArgumentNullException(NameOf(tables))

        If tables.Any Then
            For Each t In tables
                _Tables.Add(t)
            Next
        End If
    End Sub

    Public Sub Clear()
        _Tables.Clear()
    End Sub

    Public Function ToArrayList() As ArrayList
        Dim al As New ArrayList
        al.AddRange(_Tables)
        Return al
    End Function

    Public Function GetTable(ByVal id As Integer) As SoaTable
        For Each o In Me
            Dim t As SoaTable = o
            If t.TableIdentity = id Then Return t
        Next
        Return Nothing
    End Function

    Public Iterator Function GetEnumerator() As IEnumerator(Of SoaTable) Implements IEnumerable(Of SoaTable).GetEnumerator
        For Each t In _Tables
            Yield t
        Next
    End Function

    Private Iterator Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
        Yield GetEnumerator()
    End Function

    Public Sub SortBy(Optional ByVal order As String = "Name")
        If order = "Name" Then
            _CurrentSortOrder = "Name"
            _Tables.Sort(SoaTable.ComparerByTableName)
        ElseIf order = "ID" Then
            _CurrentSortOrder = "ID"
            _Tables.Sort(SoaTable.ComparerByTableId)
        End If
    End Sub

End Class
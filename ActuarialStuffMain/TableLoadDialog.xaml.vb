Public Class TableLoadDialog

    Private tables As String()

    Private Sub Window_Loaded(sender As Object,
                              e As RoutedEventArgs)

        txtSettingsDirectory.Text = My.Settings.TableDirectory
    End Sub

    Public Function TablesToLoad() As String()
        Return tables
    End Function

    Private Sub BtnBrowse_Click(sender As Object,
                                e As RoutedEventArgs) Handles btnBrowse.Click

        Using FolderBrowserDialog1 As New Forms.FolderBrowserDialog

            FolderBrowserDialog1.ShowNewFolderButton = False
            FolderBrowserDialog1.SelectedPath = txtSettingsDirectory.Text

            If FolderBrowserDialog1.ShowDialog() = Forms.DialogResult.OK Then
                txtSettingsDirectory.Text = FolderBrowserDialog1.SelectedPath
            End If

        End Using
    End Sub

    Private Sub BtnLoadDirectory_Click(sender As Object,
                                       e As RoutedEventArgs) Handles btnLoadDirectory.Click

        tables = IO.Directory.GetFiles(txtSettingsDirectory.Text,
                                       "t*.xml",
                                       IO.SearchOption.TopDirectoryOnly)

        My.Settings.TableDirectory = txtSettingsDirectory.Text
        DialogResult = True
    End Sub

    Private Sub BtnLoadSelected_Click(sender As Object,
                                      e As RoutedEventArgs) Handles btnLoadSelected.Click

        Dim dirInfo As New IO.DirectoryInfo(txtSettingsDirectory.Text)

        Using OpenFileDialog1 As New Forms.OpenFileDialog

            OpenFileDialog1.Filter = "SOA Tables (t*.xml)|t*.xml"
            OpenFileDialog1.Multiselect = True
            OpenFileDialog1.InitialDirectory = txtSettingsDirectory.Text

            If OpenFileDialog1.ShowDialog() = Forms.DialogResult.OK Then
                tables = OpenFileDialog1.FileNames
                My.Settings.TableDirectory = txtSettingsDirectory.Text
                DialogResult = True
            Else
                DialogResult = False
            End If

        End Using

    End Sub

End Class
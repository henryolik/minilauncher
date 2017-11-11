Imports System.Net
Public Class launcher_main
    Dim osVer As Version = Environment.OSVersion.Version
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "/mini.exe") Then
            Process.Start(Application.StartupPath & "/mini.exe")
        Else
            MsgBox("Nemůžu nalézt soubor hry. Prosím, přeinstalujte hru")
        End If
    End Sub

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If My.Computer.FileSystem.FileExists(Application.StartupPath & "/mini2.exe") Then
            Process.Start(Application.StartupPath & "/mini2.exe")
        Else
            MsgBox("Nemůžu nalézt soubor hry. Prosím, přeinstalujte hru")
        End If
    End Sub

    Private Sub OProgramuToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles OProgramuToolStripMenuItem.Click
        launcher_about.Show()
    End Sub

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim osVer As Version = Environment.OSVersion.Version
        If osVer.Major = 5 And osVer.Minor = 1 Then
            MsgBox("Windows XP již není podporován! Stáhněte si, prosím Míní Elder Launcher na https://ministudios.ml/ke-stazeni", MsgBoxStyle.Critical, "Operační systém není podporován")
            Me.Close()
        End If
        Me.Show()
        If IsConnectionAvailable() = True Then
            CheckForUpdates()
            msg()
        End If
        If update_internet() = False Then
            WebBrowser1.Hide()
        End If
    End Sub

    Public Sub CheckForUpdates()
        Dim version As String = Application.StartupPath & "/version.txt"
        Dim updater As String = Application.StartupPath & "/updater.exe"
        Dim MyVer As String = My.Application.Info.Version.ToString
        If My.Computer.FileSystem.FileExists(version) Then
            My.Computer.FileSystem.DeleteFile(version)
        End If
        Dim wc As WebClient = New WebClient()
        If IsConnectionAvailable() = True Then
            wc.DownloadFile("https://dl.ministudios.ml/mini/launcher/version.txt", version)
        End If
        Dim LastVer As String = My.Computer.FileSystem.ReadAllText(version)
        If Not MyVer = LastVer Then
            Dim result As Integer = MessageBox.Show("Je k dispozici aktualizace! Chcete ji stáhnout?", "Aktualizace", MessageBoxButtons.YesNo)
            If result = DialogResult.Yes Then
                If My.Computer.FileSystem.FileExists(updater) Then
                    My.Computer.FileSystem.DeleteFile(updater)
                End If
                If IsConnectionAvailable() = True Then
                    wc.DownloadFile("https://dl.ministudios.ml/updater/updater.exe", updater)
                End If
                Process.Start(Application.StartupPath & "/updater.exe", "-launcher")
                Me.Close()
            End If
        End If
    End Sub

    Public Function IsConnectionAvailable() As Boolean
        Dim objUrl As New System.Uri("https://dl.ministudios.ml/status.txt")
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        Dim objResp As System.Net.WebResponse
        Try
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            objWebReq = Nothing
            Return False
        End Try
    End Function

    Public Function update_internet() As Boolean
        Dim objUrl As New System.Uri("https://update.ministudios.ml")
        Dim objWebReq As System.Net.WebRequest
        objWebReq = System.Net.WebRequest.Create(objUrl)
        Dim objResp As System.Net.WebResponse
        Try
            objResp = objWebReq.GetResponse
            objResp.Close()
            objWebReq = Nothing
            Return True
        Catch ex As Exception
            objWebReq = Nothing
            Return False
        End Try
    End Function
    Public Sub msg()
        Dim MyVer As String = My.Application.Info.Version.ToString
        Dim osVer As Version = Environment.OSVersion.Version
        Dim wc As WebClient = New WebClient()
        Dim os As String = Nothing
        If osVer.Major = 6 And osVer.Minor = 0 Then
            os = "vista"
        End If
        If osVer.Major = 6 And osVer.Minor = 1 Then
            os = "7"
        End If
        If osVer.Major = 6 And osVer.Minor = 2 Then
            If My.Computer.Info.OSFullName.Contains("10") Then
                os = "10"
            Else
                If My.Computer.Info.OSFullName.Contains("8.1") Then
                    os = "8.1"
                Else
                    os = "8"
                End If
            End If
        End If
        If Not os = Nothing Then
            Dim msg As String = Application.StartupPath & "/msg.txt"
            wc.DownloadFile(New Uri("https://dl.ministudios.ml/mini/launcher/msgs/" & MyVer & "/" & os & "msg.txt"), msg)
            If Not My.Computer.FileSystem.ReadAllText(msg) = "" Then
                MsgBox(My.Computer.FileSystem.ReadAllText(msg), MsgBoxStyle.Information, "Oznámení")
            End If
        End If
    End Sub
End Class

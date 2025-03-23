Imports Microsoft.Win32
Public Class Form1
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        MsgBox("Created by NJ Bendall. https://git.hsho.me")
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Use 32-bit view (WOW6432Node)
            Dim baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
            Dim subKey As RegistryKey = baseKey.OpenSubKey("Software\His Smart Home\MQTT-Listener", True)

            ' If key doesn't exist, create it with sample/default values
            If subKey Is Nothing Then
                subKey = baseKey.CreateSubKey("Software\His Smart Home\MQTT-Listener", True)
                subKey.SetValue("Server", "broker.example.com", RegistryValueKind.String)
                subKey.SetValue("Port", 1883, RegistryValueKind.DWord)
                subKey.SetValue("Username", "myuser", RegistryValueKind.String)
                subKey.SetValue("Password", "mypassword", RegistryValueKind.String)
                subKey.SetValue("Topic", "my/topic/path", RegistryValueKind.String)
                subKey.SetValue("UrlToOpen", "https://example.com", RegistryValueKind.String)
                subKey.SetValue("UseTLS", 0, RegistryValueKind.DWord)
                subKey.SetValue("ClientId", "sample-client-id", RegistryValueKind.String)
            End If

            ' Prefill form fields with existing (or just-created) values
            txtServer.Text = CStr(subKey.GetValue("Server", ""))
            txtPort.Text = CStr(subKey.GetValue("Port", "1883"))
            txtUsername.Text = CStr(subKey.GetValue("Username", ""))
            txtPassword.Text = CStr(subKey.GetValue("Password", ""))
            txtTopic.Text = CStr(subKey.GetValue("Topic", ""))
            txtUrl.Text = CStr(subKey.GetValue("UrlToOpen", ""))
            txtClientId.Text = CStr(subKey.GetValue("ClientId", ""))

            Dim tlsValue As Integer = CInt(subKey.GetValue("UseTLS", 0))
            chkTLS.Checked = (tlsValue = 1)

            subKey.Close()
            baseKey.Close()

        Catch ex As Exception
            MessageBox.Show("Error loading or creating settings: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' Open the 64-bit view of HKLM
            Dim baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
            Dim subKey As RegistryKey = baseKey.CreateSubKey("Software\His Smart Home\MQTT-Listener", True)

            If subKey IsNot Nothing Then
                subKey.SetValue("Server", txtServer.Text, RegistryValueKind.String)

                ' Validate port
                Dim portValue As Integer
                If Integer.TryParse(txtPort.Text, portValue) Then
                    subKey.SetValue("Port", portValue, RegistryValueKind.DWord)
                Else
                    MessageBox.Show("Port must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                subKey.SetValue("Username", txtUsername.Text, RegistryValueKind.String)
                subKey.SetValue("Password", txtPassword.Text, RegistryValueKind.String)
                subKey.SetValue("Topic", txtTopic.Text, RegistryValueKind.String)
                subKey.SetValue("UrlToOpen", txtUrl.Text, RegistryValueKind.String)
                If chkTLS.Checked = True Then
                    subKey.SetValue("UseTLS", 1, RegistryValueKind.DWord)
                Else
                    subKey.SetValue("UseTLS", 0, RegistryValueKind.DWord)
                End If
                subKey.SetValue("ClientId", txtClientId.Text, RegistryValueKind.String)

                subKey.Close()
                baseKey.Close()

                    MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Failed to open registry key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As UnauthorizedAccessException
            MessageBox.Show("Access denied. Please run as administrator.", "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error saving settings: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class

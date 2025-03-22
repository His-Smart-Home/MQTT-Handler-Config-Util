Imports Microsoft.Win32
Public Class Form1
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        MsgBox("Created by NJ Bendall. https://hsho.me")
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            Dim baseKey As RegistryKey = Registry.LocalMachine.CreateSubKey("Software\His Smart Home\MQTT-Listener")

            If baseKey IsNot Nothing Then
                baseKey.SetValue("Server", txtServer.Text, RegistryValueKind.String)

                ' Port should be numeric
                Dim portValue As Integer
                If Integer.TryParse(txtPort.Text, portValue) Then
                    baseKey.SetValue("Port", portValue, RegistryValueKind.DWord)
                Else
                    MessageBox.Show("Port must be a valid number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                baseKey.SetValue("Username", txtUsername.Text, RegistryValueKind.String)
                baseKey.SetValue("Password", txtPassword.Text, RegistryValueKind.String)
                baseKey.SetValue("Topic", txtTopic.Text, RegistryValueKind.String)
                baseKey.SetValue("UrlToOpen", txtUrl.Text, RegistryValueKind.String)

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

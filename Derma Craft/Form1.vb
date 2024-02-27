Imports MySql.Data.MySqlClient

Public Class Form1

    Private Sub LoginButton_Click_1(sender As Object, e As EventArgs) Handles LoginButton.Click
        Dim connStr = "server=localhost; user=root; password=admin; database=dermatologyDB;"

        Using conn As New MySqlConnection(connStr)
            Try
                conn.Open()

                Dim cmd As New MySqlCommand("SELECT * FROM Admin WHERE username=@username", conn)
                cmd.Parameters.AddWithValue("@username", UsernameTextBox.Text)

                Dim adapter As New MySqlDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                If table.Rows.Count = 0 Then
                    MessageBox.Show("Invalid username.")
                Else
                    Dim storedPassword As String = table.Rows(0)("password").ToString()

                    If storedPassword = PasswordTextBox.Text Then
                        MessageBox.Show("Login successful!")
                        Admindash.Show()

                        ' Proceed to the next form or operations after login
                    Else
                        MessageBox.Show("Invalid password.")
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show($"Error: {ex.Message}")
            End Try
        End Using
    End Sub

End Class

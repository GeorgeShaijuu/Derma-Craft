Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class Admindash

    Private Sub btnAddCustomer_Click(sender As Object, e As EventArgs) Handles btnAddCustomer.Click
        Dim cusname = Guna2TextBox1.Text
        Dim skintype = If(Guna2ComboBox1.SelectedItem IsNot Nothing, Guna2ComboBox1.SelectedItem.ToString(), "")
        Dim skinissue = If(Guna2ComboBox2.SelectedItem IsNot Nothing, Guna2ComboBox2.SelectedItem.ToString(), "")
        Dim dob = Guna2DateTimePicker1.Value.Date

        AddNewCustomer(cusname, skintype, skinissue, dob)
    End Sub

    Public Sub AddNewCustomer(cusname As String, skintype As String, skinissue As String, dob As Date)
        Dim connStr As String = "server=localhost; user=root; password=admin; database=dermatologyDB;"
        Using conn As New MySqlConnection(connStr)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO Customer (cusname, skintype, skinissue, dob) VALUES (@cusname, @skintype, @skinissue, @dob)"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@cusname", cusname)
                    cmd.Parameters.AddWithValue("@skintype", skintype)
                    cmd.Parameters.AddWithValue("@skinissue", skinissue)
                    cmd.Parameters.AddWithValue("@dob", dob)
                    Dim result = cmd.ExecuteNonQuery()
                    If result > 0 Then
                        MessageBox.Show("Customer added successfully!")
                    Else
                        MessageBox.Show("Failed to add customer.")
                    End If
                End Using
            Catch ex As MySqlException
                MessageBox.Show("MySQL error occurred. Error details: " & ex.Message & vbCrLf & "Error code: " & ex.Number)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub Admindash_Load(sender As Object, e As EventArgs)
        Guna2GroupBox1.Visible = False

        Guna2ComboBox1.Items.AddRange({"Oily", "Dry", "Combination", "Sensitive", "Normal"})
        Guna2ComboBox2.Items.AddRange({"Acne", "Eczema", "Rosacea", "Psoriasis", "Hyperpigmentation"})
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton1.Click
        Guna2GroupBox1.Show()
    End Sub

    Private Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2GroupBox3.Visible = False
        Guna2ComboBox4.Items.Add("Acne")
        Guna2ComboBox4.Items.Add("Eczema")
        Guna2ComboBox4.Items.Add("Rosacea")
        Guna2ComboBox4.Items.Add("Psoriasis")
        Guna2ComboBox4.Items.Add("Hyperpigmentation")
    End Sub

    Public Sub AddNewProduct()
        Dim prodname As String = Guna2TextBox2.Text
        Dim description As String = Guna2TextBox5.Text
        Dim category As String = If(Guna2ComboBox4.SelectedItem IsNot Nothing, Guna2ComboBox4.SelectedItem.ToString(), "")
        Dim price As Decimal
        If Not Decimal.TryParse(Guna2TextBox4.Text, price) Then
            MessageBox.Show("Invalid price format.")
            Return
        End If

        Dim connStr As String = "server=localhost; user=root; password=admin; database=dermatologyDB;"
        Using conn As New MySqlConnection(connStr)
            Try
                conn.Open()
                Dim query As String = "INSERT INTO products (prodname, description, category, price) VALUES (@prodname, @description, @category, @price)"

                Using cmd As New MySqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@prodname", prodname)
                    cmd.Parameters.AddWithValue("@description", description)
                    cmd.Parameters.AddWithValue("@category", category)
                    cmd.Parameters.AddWithValue("@price", price)

                    Dim result As Integer = cmd.ExecuteNonQuery()
                    If result > 0 Then
                        MessageBox.Show("Product added successfully!")
                    Else
                        MessageBox.Show("Failed to add product.")
                    End If
                End Using
            Catch ex As MySqlException
                MessageBox.Show("An error occurred: " & ex.Message)
            Finally
                conn.Close()
            End Try
        End Using
    End Sub

    Private Sub Guna2GradientButton5_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton5.Click
        AddNewProduct()
    End Sub

    Private Sub Guna2GradientButton2_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton2.Click
        Guna2GroupBox3.Visible = True
    End Sub

    Private Sub Guna2GradientButton8_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton8.Click
        Me.Close()
        Form1.Close()
    End Sub

    Private Sub Guna2GradientButton4_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton4.Click
        reco.Show()
    End Sub

    Private Sub Guna2GradientButton7_Click(sender As Object, e As EventArgs) Handles Guna2GradientButton7.Click
        billerForm.ShowDialog()
    End Sub
End Class

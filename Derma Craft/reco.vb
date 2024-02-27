Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class reco
    ' Assuming dbConnectionString is declared and initialized somewhere in your code
    Dim dbConnectionString As String = "server=localhost; user=root; password=admin; database=dermatologyDB"

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        DisplayCustomerData()
        DisplayDataWithProducts()
    End Sub

    Public Sub DisplayCustomerData()
        Dim data As DataTable = GetAllCustomerDataFromDatabase()
        Guna2DataGridView1.DataSource = data
    End Sub

    Public Function GetAllCustomerDataFromDatabase() As DataTable
        Dim query As String = "SELECT Cusid, cusname, skintype, skinissue FROM Customer"

        Using conn As MySqlConnection = New MySqlConnection(dbConnectionString)
            Using cmd As MySqlCommand = New MySqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dataTable As DataTable = New DataTable()
                    adapter.Fill(dataTable)
                    Return dataTable
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message)
                    Return New DataTable()
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Function

    Public Sub DisplayDataWithProducts()
        Dim data As DataTable = GetAllCustomerDataWithProductsFromDatabase()
        Guna2DataGridView2.DataSource = data
    End Sub

    Public Function GetAllCustomerDataWithProductsFromDatabase() As DataTable
        Dim query As String = "SELECT Customer.Cusid, Customer.cusname, Customer.skintype, Customer.skinissue, products.prodname, products.price " &
                              "FROM Customer " &
                              "LEFT JOIN products ON Customer.skinissue = products.category"

        Using conn As MySqlConnection = New MySqlConnection(dbConnectionString)
            Using cmd As MySqlCommand = New MySqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dataTable As DataTable = New DataTable()
                    adapter.Fill(dataTable)
                    Return dataTable
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message)
                    Return New DataTable()
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Function

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        ' Assuming you want to use the first selected row
        If Guna2DataGridView1.SelectedRows.Count > 0 Then
            ' Get the values from the selected row
            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
            Dim customerID As Integer = Convert.ToInt32(selectedRow.Cells("Cusid").Value)
            Dim customerName As String = selectedRow.Cells("cusname").Value.ToString()
            Dim skinType As String = selectedRow.Cells("skintype").Value.ToString()
            Dim skinIssue As String = selectedRow.Cells("skinissue").Value.ToString()

            ' Retrieve product information from the database
            Dim productData As DataTable = GetProductDataFromDatabase()

            If productData.Rows.Count > 0 Then
                ' Assume you want the first product from the result set
                Dim productName As String = productData.Rows(0)("prodname").ToString()
                Dim productPrice As Decimal = Convert.ToDecimal(productData.Rows(0)("price"))

                ' Show the checkout form with actual product information and customer information
                Dim checkoutForm As New checkout(productName, productPrice, customerID, customerName, skinType, skinIssue, Guna2DataGridView1, dbConnectionString)
                checkoutForm.ShowDialog()

                ' After user interaction with the checkout form
                If checkoutForm.DialogResult = DialogResult.OK Then
                    ' Check if payment status is 'Yes'
                    Dim paymentStatus As String = If(checkoutForm.Guna2CheckBox1.Checked, "Yes", "No")

                    ' Get payment type from the combo box
                    Dim paymentType As String = checkoutForm.Guna2ComboBox1.SelectedItem.ToString()

                    ' Get total amount from the text box
                    Dim totalAmount As Decimal
                    If Decimal.TryParse(checkoutForm.Guna2TextBox2.Text, totalAmount) Then
                        ' Update the database with the billing information
                        AddBillingInfo(customerID, paymentStatus, paymentType, totalAmount)
                    Else
                        MessageBox.Show("Invalid amount format.")
                    End If
                End If
            Else
                MessageBox.Show("No product data found.")
            End If
        Else
            MessageBox.Show("Please select a customer from the list.")
        End If
    End Sub

    Private Function GetProductDataFromDatabase() As DataTable
        Dim query As String = "SELECT prodname, price FROM products LIMIT 1"

        Using conn As MySqlConnection = New MySqlConnection(dbConnectionString)
            Using cmd As MySqlCommand = New MySqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim adapter As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dataTable As DataTable = New DataTable()
                    adapter.Fill(dataTable)
                    Return dataTable
                Catch ex As Exception
                    MessageBox.Show("An error occurred while fetching product data: " & ex.Message)
                    Return New DataTable()
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Function



    ' Method to add billing information to the database
    Private Sub AddBillingInfo(customerID As Integer, paymentStatus As String, paymentType As String, totalAmount As Decimal)
        Dim query As String = "INSERT INTO biller (payment_status, payment_type, customer_analysis_id, total_amount) VALUES (@paymentStatus, @paymentType, @customerID, @totalAmount)"

        Using conn As MySqlConnection = New MySqlConnection(dbConnectionString)
            Using cmd As MySqlCommand = New MySqlCommand(query, conn)
                Try
                    conn.Open()
                    cmd.Parameters.AddWithValue("@paymentStatus", paymentStatus)
                    cmd.Parameters.AddWithValue("@paymentType", paymentType)
                    cmd.Parameters.AddWithValue("@customerID", customerID)
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmount)

                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Billing information added successfully.")
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message)
                Finally
                    conn.Close()
                End Try
            End Using
        End Using
    End Sub


End Class

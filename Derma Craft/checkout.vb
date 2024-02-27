Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient

Public Class checkout
    Private _dataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Private _dbConnectionString As String
    Private _customerName As String
    Private _skinType As String
    Private _skinIssue As String

    ' Constructor with additional parameters
    Public Sub New(productName As String, productPrice As Decimal, customerID As Integer, customerName As String, skinType As String, skinIssue As String, dataGridView1 As Guna.UI2.WinForms.Guna2DataGridView, dbConnectionString As String)
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ShowProductInfo(productName, productPrice)
        ShowCustomerInfo(customerID, customerName, skinType, skinIssue)

        ' Initialize DataGridView and connection string
        _dataGridView1 = dataGridView1
        _dbConnectionString = dbConnectionString

        ' Set the customer information
        _customerName = customerName
        _skinType = skinType
        _skinIssue = skinIssue
    End Sub

    Public Sub ShowProductInfo(productName As String, productPrice As Decimal)
        Guna2HtmlLabel2.Text = productName
        Guna2HtmlLabel5.Text = productPrice.ToString("C") ' Format product price as currency
    End Sub

    Public Sub ShowCustomerInfo(customerID As Integer, customerName As String, skinType As String, skinIssue As String)
        Guna2HtmlLabel11.Text = customerID.ToString()
        ' You can set other labels with customer information here
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Assuming you want to use the selected customer's information
        Dim customerID As Integer = Convert.ToInt32(Guna2HtmlLabel11.Text)

        ' Check if payment status is 'Yes'
        Dim paymentStatus As String = If(Guna2CheckBox1.Checked, "Yes", "No")

        ' Get payment type from the combo box
        Dim paymentType As String = Guna2ComboBox1.SelectedItem.ToString()

        ' Get total amount from the text box
        Dim totalAmount As Decimal
        If Decimal.TryParse(Guna2TextBox2.Text, totalAmount) Then
            ' Update the database with the billing information
            AddBillingInfo(customerID, paymentStatus, paymentType, totalAmount)

            ' Close the checkout form with OK result
            DialogResult = DialogResult.OK
            Close()
        Else
            MessageBox.Show("Invalid amount format.")
        End If
    End Sub

    ' Method to add billing information to the database
    Private Sub AddBillingInfo(customerID As Integer, paymentStatus As String, paymentType As String, totalAmount As Decimal)
        Dim query As String = "INSERT INTO biller (payment_status, payment_type, customer_analysis_id, total_amount) VALUES (@paymentStatus, @paymentType, @customerID, @totalAmount)"

        Using conn As MySqlConnection = New MySqlConnection(_dbConnectionString)
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
    Private Sub checkout_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Add payment options to Guna2ComboBox1
        Guna2ComboBox1.Items.Add("Card")
        Guna2ComboBox1.Items.Add("UPI")
        Guna2ComboBox1.Items.Add("Cash")
        Guna2ComboBox1.Items.Add("Netbanking")

        ' You can set the default selected option if needed
        Guna2ComboBox1.SelectedIndex = 0
    End Sub
End Class

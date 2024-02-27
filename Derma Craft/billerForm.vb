Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing

Public Class billerForm
    Private WithEvents PrintDocument1 As New Printing.PrintDocument

    Private Sub BillerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Load data into DataGridView when the form loads
        LoadBillerData()
    End Sub

    Private Sub LoadBillerData()
        ' Your code to load data from the 'biller' table into DataGridView
        ' Use a DataGridViewAdapter to fill the data, adjust the connection string accordingly
        Dim connectionString As String = "server=localhost; user=root; password=admin; database=dermatologyDB;"
        Dim adapter As New MySqlDataAdapter("SELECT * FROM biller", connectionString)
        Dim dataTable As New DataTable()
        adapter.Fill(dataTable)
        Guna2DataGridView1.DataSource = dataTable
    End Sub

    Private Sub Guna2GradientButton1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        ' Print button clicked
        PrintDocument1.Print()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        ' Your code to handle printing
        ' Example: Printing DataGridView content
        Dim bmp As New Bitmap(Guna2DataGridView1.Width, Guna2DataGridView1.Height)
        Guna2DataGridView1.DrawToBitmap(bmp, New Rectangle(0, 0, Guna2DataGridView1.Width, Guna2DataGridView1.Height))
        e.Graphics.DrawImage(bmp, 0, 0)
    End Sub
End Class

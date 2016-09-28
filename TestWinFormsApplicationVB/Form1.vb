Imports Mrozik.WinFormsBarCodeScanner

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim scaner As New BarCodeScanner
        Dim code As String = scaner.Show()

        recognizedCodeTextBox.Text = code



    End Sub
End Class

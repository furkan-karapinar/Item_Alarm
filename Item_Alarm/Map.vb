Imports System.ComponentModel

Public Class Map
    Public Sub IMG(path As String)
        PictureBox1.ImageLocation = path

    End Sub



    Private Sub Map_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        PictureBox1.Dispose()
    End Sub
End Class
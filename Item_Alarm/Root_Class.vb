Imports System.Reflection.Emit
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar

Public Class Root_Class
    Public Sub OpenMapForm(text As String, image_loc As String, owner_frm As Form1)
        Dim mapForm As New Map()
        mapForm.StartPosition = FormStartPosition.Manual
        mapForm.IMG(image_loc)
        ' Map formunun boyutunu Form1'in boyutunun %75'ine ayarlayın
        mapForm.Width = CInt(owner_frm.Width * 0.5)
        mapForm.Height = CInt(owner_frm.Height * 0.5)

        ' Map formunu Form1'in ortasında konumlandırın
        mapForm.StartPosition = FormStartPosition.Manual
        mapForm.Left = owner_frm.Left + (owner_frm.Width - mapForm.Width) \ 2
        mapForm.Top = owner_frm.Top + (owner_frm.Height - mapForm.Height) \ 2

        ' Map formunun başlık metnini belirleyin
        mapForm.Text = text

        ' Map formunu açın
        mapForm.Show()
    End Sub


    Public Function ScaleImage(image As Image, maxWidth As Integer, maxHeight As Integer) As Image
        Dim ratioX As Double = CDbl(maxWidth) / image.Width
        Dim ratioY As Double = CDbl(maxHeight) / image.Height
        Dim ratio As Double = Math.Min(ratioX, ratioY)

        Dim newWidth As Integer = CInt(image.Width * ratio) / 2
        Dim newHeight As Integer = CInt(image.Height * ratio) / 2

        Dim newImage As New Bitmap(newWidth, newHeight)
        Using g As Graphics = Graphics.FromImage(newImage)
            g.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using

        Return newImage
    End Function



End Class

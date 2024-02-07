Imports System.Data.SQLite
Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Reflection.Emit

Public Class Item_Settings
    Dim svr_id, item_id As Integer
    Dim dc As New Database_Control
    Public Sub item_settingss(server_id As Integer, id As Integer)
        svr_id = server_id
        item_id = id


        Dim item_name As String = dc.Get_ItemData("sunucu", server_id, id, 2, "String")

        TextBox1.Text = item_name
        PictureBox1.ImageLocation = Application.StartupPath & "\data\images\" & "Server_" & svr_id & "_Item_" & item_id & "_img"




    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim opt As New OpenFileDialog
        Dim result As DialogResult = opt.ShowDialog()
        If result = DialogResult.OK Then
            Label3.Text = opt.FileName
        End If
    End Sub

    Private Sub Label3_TextChanged(sender As Object, e As EventArgs) Handles Label3.TextChanged
        PictureBox1.ImageLocation = Label3.Text
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim x As String = "Server_" & svr_id & "_Item_" & item_id & "_img"

        If Label3.Text <> String.Empty Then

            If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\data\") Then
                My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\data\")
            End If
            If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\data\images\") Then
                My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\data\images\")
            End If
            If File.Exists(Application.StartupPath & "\data\images\" & x) Then
                File.Delete(Application.StartupPath & "\data\images\" & x)
                File.Copy(Label3.Text, Application.StartupPath & "\data\images\" & x)
            Else
                File.Copy(Label3.Text, Application.StartupPath & "\data\images\" & x)
            End If
        End If


        If dc.Update_ItemData("sunucu", svr_id, item_id, TextBox1.Text, "\data\images\" & x) > 0 Then
            Console.WriteLine("Veri güncellendi.")
        Else
            Console.WriteLine("Veri güncellenemedi.")
            dc.Insert_Data("sunucu", "id , sunucu_id , item_name , image_location", "'" & item_id & "' , '" & svr_id & "' , '" & TextBox1.Text & "' , '" & "\data\images\" & x & "'")
        End If



        Me.Close()


    End Sub
End Class
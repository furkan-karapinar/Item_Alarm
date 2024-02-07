Imports System.Data.SQLite
Imports System.IO

Public Class Settings

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click



        Dim connectionString As String = "Data Source=database.db;Version=3;"

        Using connection As New SQLiteConnection(connectionString)
            connection.Open()
            ' Güncelleme sorgusu
            Dim updateQuery As String = "UPDATE Info SET sunucu_sayisi = @server , sutun_sayisi = @sutun , dakika1 = @dk1 , dakika2 = @dk2 , dakika3 = @dk3  WHERE id = 1"

            ' Sorguyu hazırlıyoruz
            Using command As New SQLiteCommand(updateQuery, connection)
                ' Parametreleri ekliyoruz
                command.Parameters.AddWithValue("@server", NumericUpDown1.Value)
                command.Parameters.AddWithValue("@sutun", NumericUpDown2.Value)

                command.Parameters.AddWithValue("@dk1", NumericUpDown3.Value)
                command.Parameters.AddWithValue("@dk2", NumericUpDown4.Value)
                command.Parameters.AddWithValue("@dk3", NumericUpDown5.Value)



                ' Sorguyu çalıştırıyoruz
                Dim affectedRows As Integer = command.ExecuteNonQuery()

                ' Güncelleme işleminin başarılı olup olmadığını kontrol ediyoruz
                If affectedRows > 0 Then
                    Console.WriteLine("Veriler başarıyla güncellendi.")
                Else
                    Console.WriteLine("Güncelleme işlemi başarısız.")
                End If
            End Using
        End Using


        If Label7.Text <> String.Empty Then
            If Not My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\data\") Then
                My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\data\")
            End If
            If File.Exists(Application.StartupPath & "\data\alarm.mp3") Then
                File.Delete(Application.StartupPath & "\data\alarm.mp3")
                File.Copy(Label7.Text, Application.StartupPath & "\data\alarm.mp3")
            Else
                File.Copy(Label7.Text, Application.StartupPath & "\data\alarm.mp3")
            End If
        End If

        Form1.Show()
        Me.Close()
    End Sub

    Private Sub Settings_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        ' Bağlantı dizesi oluşturuyoruz
        Dim connectionString As String = "Data Source=database.db;Version=3;"

        ' Veritabanı bağlantısını açıyoruz
        Using connection As New SQLiteConnection(connectionString)
            connection.Open()

            ' Okuma sorgusu
            Dim selectQuery As String = "SELECT * FROM Info WHERE id = @id"

            ' Sorguyu hazırlıyoruz
            Using command As New SQLiteCommand(selectQuery, connection)
                ' Parametreyi ekliyoruz
                command.Parameters.AddWithValue("@id", 1)

                ' Sorguyu çalıştırıyoruz ve verileri alıyoruz
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        ' Verileri okuyoruz
                        NumericUpDown1.Value = reader.GetValue(1)
                        NumericUpDown2.Value = reader.GetValue(2)

                        NumericUpDown3.Value = reader.GetValue(3)
                        NumericUpDown4.Value = reader.GetValue(4)
                        NumericUpDown5.Value = reader.GetValue(5)

                    End If
                End Using
            End Using
        End Using


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim opt As New OpenFileDialog
        Dim result As DialogResult = opt.ShowDialog()
        If result = DialogResult.OK Then
            Label7.Text = opt.FileName
        End If

    End Sub
End Class
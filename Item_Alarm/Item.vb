Imports System.Data.SQLite
Imports System.Threading
Imports System.Timers
Imports NAudio.Wave

Public Class Item
    Dim svr_id, item_id As Integer
    Public owner_frm As Form1
    Private remainingTime As TimeSpan


    Dim rc As New Root_Class

    Public image_loc As String

    Public Sub StopTimers()
        Timer1.Stop()
    End Sub


    Private Sub Cmsgbox(message As String)
        owner_frm.Alarm(message)
    End Sub

    Private Sub Load_Image(ImageLocation As String)
        Try
            Dim originalImage As Image = Image.FromFile(ImageLocation)
            PictureBox1.Image = rc.ScaleImage(originalImage, PictureBox1.Width, PictureBox1.Height)
            originalImage.Dispose()
        Catch ex As Exception
        End Try

    End Sub

    Public Function gorev(server_id As String, id As Integer, item_name As String, item_image_location As String, dk1 As Integer, dk2 As Integer, dk3 As Integer)
        svr_id = server_id
        item_id = id

        Button1.Text = dk1
        Button2.Text = dk2
        Button3.Text = dk3

        image_loc = item_image_location
        Label1.Text = item_name
        Load_Image(item_image_location)


    End Function

    Private Sub StartCountdown(minute As String, color As Color)
        If color <> Color.Red Then
            Dim countdownDuration As Integer = Integer.Parse(minute) - 1 ' 45 dakika
            remainingTime = TimeSpan.FromMinutes(countdownDuration)

            Timer1.Stop()
            Timer1.Interval = 1000 ' 1 saniye (1000 milisaniye)
            Timer1.Start()
        End If


    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1))


        If remainingTime.TotalSeconds <= 0 Then
            Timer1.Stop()

            Cmsgbox("Sunucu-" & svr_id & " Item-Adı: " & Label1.Text & " Toplanma Zamanı Geldi!")
            Me.BackColor = Color.White
            Button1.BackColor = Color.FromKnownColor(KnownColor.Control)
            Button2.BackColor = Color.FromKnownColor(KnownColor.Control)
            Button3.BackColor = Color.FromKnownColor(KnownColor.Control)
            rc.OpenMapForm("Sunucu-" & svr_id & " Item-Adı: " & Label1.Text, image_loc, owner_frm)
            Label2.Visible = False
        End If

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True

        Me.Invoke(Sub()
                      Label2.Visible = True
                      Label2.Text = "Kalan: " & remainingTime.ToString("hh\:mm\:ss")
                  End Sub)

        If remainingTime.TotalSeconds <= 0 Then
            Label2.Visible = False
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button2.BackColor = Color.FromKnownColor(KnownColor.Control)
        Button3.BackColor = Color.FromKnownColor(KnownColor.Control)

        If Button1.BackColor = Color.Red Then
            Me.BackColor = Color.White


            Button2.Enabled = True
            Button3.Enabled = True
            Timer1.Stop()
            Button1.BackColor = Color.FromKnownColor(KnownColor.Gray)



        ElseIf Button1.BackColor = Color.Gray Then

            remainingTime = TimeSpan.Parse(Label2.Text.Substring(6))
            Button1.BackColor = Color.FromKnownColor(KnownColor.Red)

            Timer1.Start()
        Else

            StartCountdown(Button1.Text, Button1.BackColor)
            Button1.BackColor = Color.Red
            Button2.Enabled = False
            Button3.Enabled = False
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim settingss As New Item_Settings
        settingss.item_settingss(svr_id, item_id)
        settingss.Show()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button1.BackColor = Color.FromKnownColor(KnownColor.Control)
        Button3.BackColor = Color.FromKnownColor(KnownColor.Control)

        If Button2.BackColor = Color.Red Then
            Me.BackColor = Color.White


            Button1.Enabled = True
            Button3.Enabled = True

            Button2.BackColor = Color.FromKnownColor(KnownColor.Gray)

            Timer1.Stop()
        ElseIf Button2.BackColor = Color.Gray Then

            remainingTime = TimeSpan.Parse(Label2.Text.Substring(6))
            Button2.BackColor = Color.FromKnownColor(KnownColor.Red)

            Timer1.Start()
        Else

            StartCountdown(Button2.Text, Button2.BackColor)
            Button2.BackColor = Color.Red
            Button1.Enabled = False
            Button3.Enabled = False
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button2.BackColor = Color.FromKnownColor(KnownColor.Control)
        Button1.BackColor = Color.FromKnownColor(KnownColor.Control)

        If Button3.BackColor = Color.Red Then
            Me.BackColor = Color.White


            Button2.Enabled = True
            Button1.Enabled = True

            Button3.BackColor = Color.FromKnownColor(KnownColor.Gray)

            Timer1.Stop()
        ElseIf Button3.BackColor = Color.Gray Then

            remainingTime = TimeSpan.Parse(Label2.Text.Substring(6))
            Button3.BackColor = Color.FromKnownColor(KnownColor.Red)

            Timer1.Start()
        Else

            StartCountdown(Button3.Text, Button1.BackColor)
            Button3.BackColor = Color.Red
            Button2.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        rc.OpenMapForm("Sunucu-" & svr_id & " Item-Adı: " & Label1.Text, image_loc, owner_frm)
    End Sub


End Class

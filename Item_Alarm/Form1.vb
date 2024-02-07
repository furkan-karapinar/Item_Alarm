Imports System.Data.SQLite
Imports System.Threading
Imports NAudio.Wave

Public Class Form1


    Private audioFile As AudioFileReader
    Private audioOutput As DirectSoundOut
    Private itemControlsList As New List(Of Item)()
    Private panelcontrolsList As New List(Of Panel)()
    Private tabControlsList As New List(Of TabPage)()

    Dim item_info_list As New DataTable

    Dim columncount As Integer = 5
    Dim padding_size As Integer = 7

    Dim server_sayisi As Integer
    Dim satir_sayisi As Integer

    Dim dk1 As Integer
    Dim dk2 As Integer
    Dim dk3 As Integer

    Private Sub DisposeControlsInTabControl(tabControl As TabControl)
        For Each tabPage As TabPage In tabControl.TabPages
            ' Her bir sekmedeki kontrol öğelerini döngüyle alıp Dispose ediyoruz
            For Each control As Control In tabPage.Controls
                control.Dispose()
            Next
        Next
    End Sub

    ' TabControl nesnesini Dispose ederken bu işlevi çağırabilirsiniz


    Private Sub StopTimers()
        For Each itemControl As Item In itemControlsList
            itemControl.StopTimers() ' User control'deki bir fonksiyonu çağır
            itemControl.Dispose()
        Next
        For Each panelcontrol As Panel In panelcontrolsList
            panelcontrol.Dispose()
        Next
        For Each tabcontrolpage As TabPage In tabControlsList
            tabcontrolpage.Dispose()
        Next
        DisposeControlsInTabControl(TabControl1)

    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        get_info()
        creator2(TabControl1)
    End Sub

    Public Function Alarm(text As String)

        Label1.Text = text
        TableLayoutPanel1.Visible = True

        If audioOutput IsNot Nothing Then
            audioOutput.Stop()
            audioOutput.Dispose()
            audioOutput = Nothing
        End If

        If audioFile IsNot Nothing Then
            audioFile.Dispose()
        End If

        audioFile = New AudioFileReader(Application.StartupPath & "\data\alarm.mp3")
        audioOutput = New DirectSoundOut()
        audioOutput.Init(New WaveChannel32(audioFile))
        audioOutput.Play()

        Alarm_Timer.Start()
    End Function
    Private Function FindRowUsingDataView(dataTable As DataTable, idValue As Integer, sunucuValue As Integer, istenilen_deger As Integer)
        Dim satir As DataRow = dataTable.AsEnumerable().FirstOrDefault(Function(r) CInt(r("id")) = idValue AndAlso CInt(r("sunucu_id")) = sunucuValue)

        If satir IsNot Nothing Then
            ' Satırdaki tüm değerleri alıyoruz
            Dim id As Integer = CInt(satir("id"))
            Dim sunucu_id As Integer = CInt(satir("sunucu_id"))
            Dim ad As String = CStr(satir("item_name"))
            Dim img As String = CStr(satir("image_location"))

            Dim sdk1 As String = String.Empty
            Dim sdk2 As String = String.Empty
            Dim sdk3 As String = String.Empty

            Try
                sdk1 = CStr(satir("saved_dakika1"))
                sdk2 = CStr(satir("saved_dakika2"))
                sdk3 = CStr(satir("saved_dakika3"))

            Catch ex As Exception

            End Try




            Return New List(Of Object) From {sunucu_id, id, ad, img, sdk1, sdk2, sdk3}

        Else

            Return New List(Of Object) From {sunucuValue, idValue, "Null", String.Empty, String.Empty, String.Empty, String.Empty}
        End If
    End Function
    Private Sub get_info()


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
                        server_sayisi = reader.GetValue(1)
                        satir_sayisi = reader.GetValue(2)

                        dk1 = reader.GetValue(3)
                        dk2 = reader.GetValue(4)
                        dk3 = reader.GetValue(5)

                    End If
                End Using
            End Using
        End Using





        Try
            Using connection As New SQLiteConnection(connectionString)
                connection.Open()

                Dim query As String = "SELECT * FROM sunucu;"
                Dim command As New SQLiteCommand(query, connection)
                Dim adapter As New SQLiteDataAdapter(command)
                Dim dataTable As New DataTable()
                adapter.Fill(dataTable)

                ' Verileri kullanmak için dataTable nesnesini kullanabilirsiniz
                ' Örneğin, DataGridView'e verileri yüklemek istiyorsanız:
                item_info_list = dataTable



            End Using
        Catch ex As Exception
            MessageBox.Show("Veritabanı hatası: " & ex.Message)
        End Try

    End Sub

    Private Sub creator2(tabControl As TabControl)



        For serverIndex As Integer = 1 To server_sayisi
            Dim tabPage As New TabPage("Sunucu " & serverIndex.ToString())
            tabControlsList.Add(tabPage)
            tabControl.TabPages.Add(tabPage)
            tabPage.BackColor = Color.FromKnownColor(KnownColor.ControlDark)
            Dim panel As New Panel()
            panelcontrolsList.Add(panel)
            panel.Dock = DockStyle.Fill
            tabPage.Controls.Add(panel)
            Dim usr As New Item
            Dim totalWidth As Integer = columncount * (usr.Width + padding_size) ' (UserControl genişlik + boşluk)
            Dim totalHeight As Integer = satir_sayisi * (usr.Height + padding_size) ' (UserControl yükseklik + boşluk)

            panel.AutoScroll = True
            panel.AutoScrollMinSize = New Size(totalWidth, totalHeight)

            For row As Integer = 0 To satir_sayisi - 1
                For col As Integer = 0 To columncount - 1
                    Dim userControl As New Item()
                    userControl.owner_frm = Me
                    userControl.Name = "Server" & serverIndex.ToString() & "_Item" & ((row * columncount) + col + 1).ToString()

                    Dim info As New List(Of Object)
                    info = FindRowUsingDataView(item_info_list, ((row * columncount) + col + 1), serverIndex, 1)
                    Dim item_name As String = info(2)

                    Dim item_image_location As String = info(3)


                    userControl.gorev(serverIndex, ((row * columncount) + col + 1), item_name, Application.StartupPath & item_image_location, dk1, dk2, dk3)

                    userControl.Margin = New Padding(padding_size) ' Aralarda boşluk bırakmak için margin

                    userControl.Location = New Point(col * (userControl.Width + padding_size), row * (userControl.Height + padding_size))
                    itemControlsList.Add(userControl)
                    panel.Controls.Add(userControl)

                Next
            Next
        Next
    End Sub



    Private Sub YenileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles YenileToolStripMenuItem.Click
        Dim result = MsgBox("Yenilemek istediğinizden emin misiniz? Tüm sayaçlar sıfırlanır!", MsgBoxStyle.YesNo)

        If result = DialogResult.Yes Then
            StopTimers()

            TabControl1.Controls.Clear()
            itemControlsList.Clear()
            panelcontrolsList.Clear()
            tabControlsList.Clear()
            get_info()
            creator2(TabControl1)
        Else
            MsgBox("Seçim iptal edildi!")
        End If


    End Sub

    Private Sub SettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SettingsToolStripMenuItem.Click
        Settings.ShowDialog()
    End Sub

    Private Sub Alarm_Timer_Tick(sender As Object, e As EventArgs) Handles Alarm_Timer.Tick


        If Me.BackColor = Color.White Then
            Me.BackColor = Color.Red
        Else
            Me.BackColor = Color.White
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Alarm_Timer.Stop()
            Me.BackColor = Color.FromKnownColor(KnownColor.Control)

            If audioOutput IsNot Nothing Then
                audioOutput.Stop()
                audioOutput.Dispose()
                audioOutput = Nothing
            End If

            If audioFile IsNot Nothing Then
                audioFile.Dispose()
            End If
            Label1.Text = TabControl1.SelectedTab.Text
        Catch ex As Exception

        End Try


        'TableLayoutPanel1.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel1.Hide()
    End Sub

    Private Sub Label1_TextChanged(sender As Object, e As EventArgs) Handles Label1.TextChanged
        If ListBox1.Items.Count > 9 Then ' Eğer öğe sayısı 10'u aşıyorsa
            ' İlk 5 öğeyi sil
            For i As Integer = 0 To 4
                ListBox1.Items.RemoveAt(0)
            Next
        End If

        If Not Label1.Text.Contains("Sunucu ") Then
            ListBox1.Items.Add(Label1.Text)

        Else

        End If

        ' Yeni öğeyi sona ekle

    End Sub

    Private Sub Form1_SizeChanged(sender As Object, e As EventArgs) Handles MyBase.SizeChanged
        Panel1.Location = New Point((Me.Size.Width / 2) - (Panel1.Size.Width / 2), (Me.Size.Height / 2) - (Panel1.Size.Height / 2))

    End Sub

    Private Sub SonÇalanAlarmlarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SonÇalanAlarmlarToolStripMenuItem.Click
        Panel1.Visible = True
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.Controls.Count <> Nothing Then
            Try
                Me.Text = "Item Alarm (" & TabControl1.SelectedTab.Text & ")"
                Label1.Text = TabControl1.SelectedTab.Text & " Seçili"
            Catch ex As Exception

            End Try

        End If


    End Sub
End Class

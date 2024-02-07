Imports System
Imports System.Collections.Generic
Imports System.Data.SQLite
Imports System.IO
Imports System.Linq
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports ComboBox = System.Windows.Forms.ComboBox

Friend Class Database_Control
    Private path As String = "database.db", cs As String = "URI=file:" & Application.StartupPath & "\database.db"
    Private data_connection As SQLiteConnection
    Private command As SQLiteCommand
    Private reader As SQLiteDataReader

    Public Sub Create_Database(ByVal datatable_name As String, ByVal data_options As String)
        ' Veritabanı yoksa oluşturur.
        If Not System.IO.File.Exists(path) Then
            SQLiteConnection.CreateFile(path)
        End If

        Using sqlite = New SQLiteConnection("Data Source=" & path)
            sqlite.Open()
            Dim sql As String = "CREATE TABLE IF NOT EXISTS " & datatable_name & " (" & data_options & ")"
            Dim cmd As SQLiteCommand = New SQLiteCommand(sql, sqlite)
            cmd.ExecuteNonQuery()
            cmd.Cancel()
            sqlite.Close()
        End Using
    End Sub

    Public Function Get_Data_From_Database(ByVal datatable_name As String)
        ' Veritabanından tüm verileri alır.
        Try

            Dim data_connection = New SQLiteConnection(cs)
            data_connection.Open()
            Dim stm As String = "SELECT * FROM " & datatable_name
            Dim command = New SQLiteCommand(stm, data_connection)
            reader = command.ExecuteReader()


            While reader.Read()
                Return reader.GetValue(0)
            End While

        Catch
            MessageBox.Show("Veri Alımı Hatası")
        End Try
    End Function


    Public Function Get_Single_Data(ByVal datatable_name As String, ByVal column_name As String, ByVal data_value As String, ByVal value As Object, ByVal value_type As String) As Object

        Dim data As String = "Null"
        Try
            Dim data_connection = New SQLiteConnection(cs)
            data_connection.Open()
            Dim stm As String = "SELECT * FROM " & datatable_name & " WHERE " & column_name & " = " & data_value
            Dim command = New SQLiteCommand(stm, data_connection)
            reader = command.ExecuteReader()

            While reader.Read()
                Select Case value_type
                    Case "Boolean"
                        data = reader.GetBoolean(value)
                    Case "Byte"
                        data = reader.GetByte(value)
                    Case "Char"
                        data = reader.GetChar(value)
                    Case "DateTime"
                        data = reader.GetDateTime(value)
                    Case "Decimal"
                        data = reader.GetDecimal(value)
                    Case "Double"
                        data = reader.GetDouble(value)
                    Case "Float"
                        data = reader.GetFloat(value)
                    Case "String"
                        data = reader.GetString(value)
                    Case "Integer"
                        data = reader.GetValue(value)
                    Case "Equals"
                        data = reader.Equals(value)
                    Case "IsDbNull"
                        data = reader.IsDBNull(value)
                    Case Else
                        data = "Null"

                End Select
            End While
            command.Cancel()
            data_connection.Close()
            Return data
        Catch ex As Exception

            Return "Null"
        End Try
    End Function

    Public Function Get_ItemData(ByVal datatable_name As String, ByVal server_id As Integer, ByVal item_id As Integer, ByVal value As Object, ByVal value_type As String) As Object

        Dim data As String = "Null"
        Try
            Dim data_connection = New SQLiteConnection(cs)
            data_connection.Open()
            Dim stm As String = "SELECT * FROM " & datatable_name & " WHERE sunucu_id='" & server_id & "' AND id= '" & item_id & "'"
            Dim command = New SQLiteCommand(stm, data_connection)
            reader = command.ExecuteReader()

            While reader.Read()
                Select Case value_type
                    Case "Boolean"
                        data = reader.GetBoolean(value)
                    Case "Byte"
                        data = reader.GetByte(value)
                    Case "Char"
                        data = reader.GetChar(value)
                    Case "DateTime"
                        data = reader.GetDateTime(value)
                    Case "Decimal"
                        data = reader.GetDecimal(value)
                    Case "Double"
                        data = reader.GetDouble(value)
                    Case "Float"
                        data = reader.GetFloat(value)
                    Case "String"
                        data = reader.GetString(value)
                    Case "Integer"
                        data = reader.GetValue(value)
                    Case "Equals"
                        data = reader.Equals(value)
                    Case "IsDbNull"
                        data = reader.IsDBNull(value)
                    Case Else
                        data = "Null"

                End Select
            End While
            command.Cancel()
            data_connection.Close()
            Return data
        Catch ex As Exception

            Return "Null"
        End Try
    End Function


    Public Function Update_Data(ByVal datatable_name As String, ByVal column_name As String, ByVal value_data As String, ByVal id_or_other_data_name As String, ByVal id_or_data_value As String) As Integer
        ' Veritabanındaki veriyi günceller
        Try


            value_data = "'" & value_data & "'"
            Dim con = New SQLiteConnection(cs)
            con.Open()
            Dim cmd = New SQLiteCommand(con)
            Dim k As String = "UPDATE " & datatable_name & " SET " & column_name & " = " & value_data & " WHERE " & id_or_other_data_name & " = " & id_or_data_value
            cmd.CommandText = k
            cmd.ExecuteNonQuery()
            cmd.Cancel()
            con.Close()
            Return 1

        Catch ex As Exception

            Return 0
        End Try

    End Function

    Public Function Update_ItemTimeData(ByVal datatable_name As String, ByVal server_id As Integer, ByVal item_id As Integer, kacinci_sayac As Integer, deger As String) As Integer
        ' Veritabanındaki veriyi günceller
        Try



            Dim con = New SQLiteConnection(cs)
            con.Open()
            Dim cmd = New SQLiteCommand(con)
            Dim k As String = "UPDATE sunucu SET saved_dakika" & kacinci_sayac & "='"& deger &"' WHERE sunucu_id = '" & server_id & "' AND id = '" & item_id & "';"
            cmd.CommandText = k
            Dim rtrn As Integer = cmd.ExecuteNonQuery()
            cmd.Cancel()
            con.Close()
            Return rtrn

        Catch ex As Exception

            Return 0
        End Try

    End Function

    Public Function Update_ItemData(ByVal datatable_name As String, ByVal server_id As Integer, ByVal item_id As Integer, item_name As String, item_image_location As String) As Integer
        ' Veritabanındaki veriyi günceller
        Try
            Using con As New SQLiteConnection(cs)
                con.Open()
                Using cmd As New SQLiteCommand(con)
                    Dim sql As String = "UPDATE " & datatable_name & " SET item_name = @item_name, image_location = @image_location WHERE sunucu_id = @server_id AND id = @item_id;"
                    cmd.CommandText = sql
                    cmd.Parameters.AddWithValue("@item_name", item_name)
                    cmd.Parameters.AddWithValue("@image_location", item_image_location)
                    cmd.Parameters.AddWithValue("@server_id", server_id)
                    cmd.Parameters.AddWithValue("@item_id", item_id)

                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            ' Hata oluştuğunda veya sorgu başarısız olduğunda burada işlem yapabilirsiniz.
            Return 0
        End Try
    End Function

    Public Sub Delete_Data(ByVal datatable_name As String, ByVal database_item_name As String, ByVal item_name As String)
        ' Veritabanından veri siler
        Try
            Dim con = New SQLiteConnection(cs)
            con.Open()
            Dim cmd = New SQLiteCommand(con)
            cmd.CommandText = "DELETE FROM " & datatable_name & " WHERE " & database_item_name & "=@name"
            cmd.Prepare()
            cmd.Parameters.AddWithValue("@name", item_name)
            cmd.ExecuteNonQuery()
            cmd.Cancel()
            con.Close()
        Catch
            MessageBox.Show("Veri Silme Hatası")
        End Try
    End Sub

    Public Function Insert_Data(ByVal datatable_name As String, ByVal item_names As String, ByVal item_values As String) As Integer
        ' Veritabanına veri ekler
        Try
            Dim con = New SQLiteConnection(cs)
            con.Open()
            Dim cmd = New SQLiteCommand(con)
            cmd.CommandText = "INSERT INTO " & datatable_name & "(" & item_names & ") VALUES(" & item_values & "); SELECT last_insert_rowid();"
            'cmd.ExecuteNonQuery()
            Dim lastRowId = cmd.ExecuteScalar()
            cmd.Cancel()
            con.Close()
            Return lastRowId
        Catch
            MessageBox.Show("Veri Giriş Hatası")
            Return 0
        End Try
    End Function


End Class

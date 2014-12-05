﻿Imports System
Imports System.IO
Imports System.Text


Module saveData
    Dim saveDirMain As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\" & My.Application.Info.Title.ToString

    Public Sub trainPokemonList()
        If My.Computer.FileSystem.DirectoryExists(saveDirMain) = False Then
            Exit Sub
        Else
            Dim folder As New DirectoryInfo(saveDirMain)

            Dim longName As String, shortName As String

            For Each finfo In folder.EnumerateFiles("*.txt", SearchOption.AllDirectories)
                longName = finfo.Name.ToString()
                shortName = Left(longName, Len(longName) - 4)

                form_main.cmb_SelectedPok.Items.Add(shortName)
            Next
        End If
    End Sub

    Public Function askSave(pok As String) As Boolean
        Dim result As MsgBoxResult
        result = MsgBox("Do you want to save changes to " & pok & "?", MsgBoxStyle.YesNo, "Save changes?")
        Return result
    End Function

    Public Sub savePok(pok As String)
        'Check if main save folder exists. If not, create it
        checkDirMain()

        Dim savePathPok As String
        savePathPok = saveDirMain & "\" & pok & ".txt"
        'MsgBox(savePathPok)


        Dim fs As FileStream = File.Create(savePathPok)

        Dim str As String
        str = pok & vbNewLine & _
                "MaxEV=" & If(form_main.rd_255.Checked = True, "255", "252") & vbNewLine & _
                "PlannedHP=" & form_main.tb_PlannedHP.Text & vbNewLine & _
                "PlannedAtk=" & form_main.tb_PlannedAtk.Text & vbNewLine & _
                "PlannedDef=" & form_main.tb_PlannedDef.Text & vbNewLine & _
                "PlannedSpAtk=" & form_main.tb_PlannedSpAtk.Text & vbNewLine & _
                "PlannedSpDef=" & form_main.tb_PlannedSpDef.Text & vbNewLine & _
                "PlannedSpd=" & form_main.tb_PlannedSpd.Text & vbNewLine & _
                "CurrentHP=" & form_main.tb_CurrentHP.Text & vbNewLine & _
                "CurrentAtk=" & form_main.tb_CurrentAtk.Text & vbNewLine & _
                "CurrentDef=" & form_main.tb_CurrentDef.Text & vbNewLine & _
                "CurrentSpAtk=" & form_main.tb_CurrentSpAtk.Text & vbNewLine & _
                "CurrentSpDef=" & form_main.tb_CurrentSpDef.Text & vbNewLine & _
                "CurrentSpd=" & form_main.tb_CurrentSpd.Text & vbNewLine & vbNewLine & _
                "Battled Pokémon" & vbNewLine '

        Dim info As Byte() = New UTF8Encoding(True).GetBytes(str)
        fs.Write(info, 0, info.Length)
        fs.Close()
    End Sub

    Public Sub loadStats(pok As String)
        Dim fileReader As System.IO.StreamReader
        fileReader = My.Computer.FileSystem.OpenTextFileReader(saveDirMain & "\" & pok & ".txt")

        Dim line As String
        Dim eqPos As Integer
        Dim currentSetting As String
        Dim currentValue As Integer
        Dim tbName As String

        Do While fileReader.Peek() <> -1
            line = fileReader.ReadLine()
            eqPos = InStr(line, "=")

            If eqPos <> 0 Then
                currentSetting = Left(line, eqPos - 1)
                currentValue = Len(line) - eqPos

                If currentSetting = "MaxEV" Then
                    If currentValue = 255 Then
                        form_main.rd_255.Checked = True
                    Else
                        form_main.rd_252.Checked = True
                    End If
                Else
                    tbName = "tb_" & currentSetting
                    Dim myTextbox As TextBox = DirectCast(form_main.Controls.Find(tbName, True)(0), TextBox)
                    myTextbox.Text = Right(line, currentValue)
                End If
            End If
        Loop
    End Sub

    Private Sub checkDirMain()
        If My.Computer.FileSystem.DirectoryExists(saveDirMain) = False Then
            My.Computer.FileSystem.CreateDirectory(saveDirMain)
        End If
    End Sub
End Module

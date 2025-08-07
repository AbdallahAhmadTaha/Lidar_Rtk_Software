Imports OpenTK
Imports OpenTKLib
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports MathNet.Numerics
Imports MathNet.Numerics.Interpolation
Imports MathNet.Numerics.LinearAlgebra.Double

Public Class LiDARRTK_Form

#Region "Variables"
    Dim SyncRTK As Double = 200000
    Public Dump As PcapFile
    Public PcapFileName As String = Nothing

    Public Packets_Available As Boolean = False
    Public LiDAR_No_Frames As Integer = 0
    Public Current_Frame As Integer = 0
    'Public VerticalAngles As Integer() = {-15, 1, -13, -3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15}
    Public SENSOR_VERTICAL_ANGLE As Integer()
    Public SENSOR_VERTICAL_ANGLE_VLP16 As Integer() = {-15, 1, -13, 3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15, -15, 1, -13, 3, -11, 5, -9, 7, -7, 9, -5, 11, -3, 13, -1, 15}
    Public SENSOR_VERTICAL_ANGLE_Hi_Res As Integer() = {-10, 0.67, -8.67, 2, -7.33, 3.33, -6, 4.67, -4.67, 6, -3.33, 7.33, -2, 8.67, -0.67, 10, -10, 0.67, -8.67, 2, -7.33, 3.33, -6, 4.67, -4.67, 6, -3.33, 7.33, -2, 8.67, -0.67, 10}

    Public azimuth_ As Double = 0
    Public azimuthN As Double = 0
    Public azimuth_interpolated As Double = 0
    ' Public Azimuth_Packet As String
    Private deg_to_rad_coeef As Double = (Math.PI / 180.0)
    Dim TimeStamp_Decimal_start As Long = 0

    Const BYTES_IN_FRAME As Integer = 1206
    Const POINTS_IN_READ As Integer = 32
    Const DATA_BLOCKS_IN_FRAME As Integer = 12

    Dim cloudPoints As New cloud
    Dim OutputObjsFile As String = Nothing
    Dim NoDecimals As Integer = 5
    Dim LiDARDataPoints As List(Of LiDAR_DataItem) = New List(Of LiDAR_DataItem)()
    Dim GPSDataPoints As List(Of GPS_DataItem) = New List(Of GPS_DataItem)()
    Dim FramesDataPoints As List(Of Packets_DataItem) = New List(Of Packets_DataItem)()
    Dim TempNMEAList As List(Of String) = New List(Of String)
    Dim RTK_DataPoints As List(Of NMEA_DataItem) = New List(Of NMEA_DataItem)()
    Dim TrajectoryDataPoints As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim SlamTrajDataPoints As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim SlamTrajDataPoints_ModifiedStatic As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim SlamStaticFrames As List(Of String) = New List(Of String)
    Dim TrajectoryDataPointsAll As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim TrajectorySmoothedDataPointsAll As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim TrajectorySyncSmoothedDataPointsAll As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim TrajectorySyncSmoothedDataPoints As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()
    Dim TrajectoryTransDataPoints As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)()


    Dim Segments As List(Of String) = New List(Of String)

    Dim TempStaticGPSData As List(Of String) = New List(Of String)
    Dim StaticGPSData As List(Of String) = New List(Of String)
    Dim StaticSyncGPSData As List(Of String) = New List(Of String)
    Dim StaticPointsInfo As List(Of String) = New List(Of String)
    Dim StaticSlamPoints As List(Of String) = New List(Of String)
    Dim StaticPoint_Avg As List(Of Vector3d) = New List(Of Vector3d)

    Dim GGAListbox As List(Of String) = New List(Of String)

#End Region

#Region "Load Form + Set View"

    Private Sub LiDARRTK_Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HListView_LiDAR.Show()
        HListView_GPS.Hide()
        HListView_Packets.Hide()
        HListView_Trajectory.Hide()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Hide()

        Set_View_ListView_LiDAR()
        Set_View_ListView_Packets()
        Set_View_ListView_GPS()
        Set_View_ListView_Trajectory()
        Set_View_ListView_NMEA()
        Set_View_ListView_SlamTrajectory()

        Me.WindowState = FormWindowState.Maximized
    End Sub

    ' Time, Rx(Roll), Ry(Pitch), Rz(Yaw), X, Y, Z

    Private listViewProvider As ListViewLiDAR
    Private Sub Set_View_ListView_LiDAR()
        '----------------------------------------
        ''Set view property for LiDAR Data

        SENSOR_VERTICAL_ANGLE = SENSOR_VERTICAL_ANGLE_VLP16
        listViewProvider = New ListViewLiDAR()
        HListView_LiDAR.ListViewProvider = listViewProvider
        '------------------------------------------

    End Sub

    Private listViewProviderGPS As ListViewGPS
    Private Sub Set_View_ListView_GPS()
        '----------------------------------------
        ''Set view property for GPS Data
        listViewProviderGPS = New ListViewGPS()
        HListView_GPS.ListViewProvider = listViewProviderGPS
        '------------------------------------------

    End Sub

    Private listViewProviderPackets As ListViewPackets
    Private Sub Set_View_ListView_Packets()
        '----------------------------------------
        ''Set view property for Packets Data
        listViewProviderPackets = New ListViewPackets()
        HListView_Packets.ListViewProvider = listViewProviderPackets
        '------------------------------------------

    End Sub

    Private listViewProviderNMEA As ListViewNMEA
    Private Sub Set_View_ListView_NMEA()
        '----------------------------------------
        ''Set view property for NMEA Data

        listViewProviderNMEA = New ListViewNMEA()
        HListView_NMEA.ListViewProvider = listViewProviderNMEA
        '------------------------------------------

    End Sub

    Private listViewProviderTrajectory As ListViewTrajectory
    Private Sub Set_View_ListView_Trajectory()
        '----------------------------------------
        ''Set view property for Trajectory Data

        listViewProviderTrajectory = New ListViewTrajectory()
        HListView_Trajectory.ListViewProvider = listViewProviderTrajectory
        '------------------------------------------

    End Sub

    Private listViewProviderSlam As ListViewSlamTrajectory
    Private Sub Set_View_ListView_SlamTrajectory()
        '----------------------------------------
        ''Set view property for Trajectory Data

        listViewProviderSlam = New ListViewSlamTrajectory()
        HListView_Slam_Trajectory.ListViewProvider = listViewProviderSlam

        '------------------------------------------

    End Sub

#End Region

#Region "Ribbon Options"
    Private Sub RibbonOrbMenuItem2_Click(sender As Object, e As EventArgs) Handles RibbonOrbMenuItem2.Click
        MyBase.Close()
    End Sub

    Private Sub RibbonOrbMenuItem1_Click(sender As Object, e As EventArgs) Handles RibbonOrbMenuItem1.Click
        'open PCAP File
        Application.DoEvents()
        WaitForm.Show()
        Application.DoEvents()
        Open_PCAP()
        WaitForm.Hide()
    End Sub

    Private Sub Open_Pcap_File_RibbonButton1_Click(sender As Object, e As EventArgs) Handles Open_Pcap_File_RibbonButton1.Click
        'open PCAP File
        Open_PCAP()
    End Sub

    Private Sub RibbonButton_LiDAR_Click(sender As Object, e As EventArgs) Handles RibbonButton_LiDAR.Click
        'Show LiDAR Data
        HListView_LiDAR.Show()
        HListView_GPS.Hide()
        HListView_Packets.Hide()
        HListView_Trajectory.Hide()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Hide()
    End Sub

    Private Sub RibbonButton_GPS_Click(sender As Object, e As EventArgs) Handles RibbonButton_GPS.Click
        'Show GPS Data
        HListView_LiDAR.Hide()
        HListView_GPS.Show()
        HListView_Packets.Hide()
        HListView_Trajectory.Hide()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Hide()
    End Sub

    Private Sub RibbonButton_Traj_Click(sender As Object, e As EventArgs) Handles RibbonButton_Traj.Click
        'Show Trajectory Data
        HListView_LiDAR.Hide()
        HListView_GPS.Hide()
        HListView_Packets.Hide()
        HListView_Trajectory.Show()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Hide()
    End Sub

    Private Sub RibbonButtonGPS_Traj_Click(sender As Object, e As EventArgs) Handles RibbonButtonRTK_Traj.Click
        'Show GPS - Trajectory Data
        HListView_LiDAR.Hide()
        HListView_GPS.Hide()
        HListView_Packets.Hide()
        HListView_Trajectory.Hide()
        HListView_NMEA.Show()
        HListView_Slam_Trajectory.Hide()
    End Sub

    Private Sub RibbonButton_Info_Click(sender As Object, e As EventArgs) Handles RibbonButton_Frames.Click
        'Show LiDAR Info Data
        HListView_LiDAR.Hide()
        HListView_GPS.Hide()
        HListView_Packets.Show()
        HListView_Trajectory.Hide()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Hide()
    End Sub

    Private Sub RibbonButtonSlam_Traj_Click(sender As Object, e As EventArgs) Handles RibbonButtonSlam_Traj.Click
        'Show Slam Trajectory Data
        HListView_LiDAR.Hide()
        HListView_GPS.Hide()
        HListView_Packets.Hide()
        HListView_Trajectory.Hide()
        HListView_NMEA.Hide()
        HListView_Slam_Trajectory.Show()
    End Sub
    Private Sub RibbonButton_VLP16_Click(sender As Object, e As EventArgs) Handles VLP16.Click

        'LiDAR VLP16
        SENSOR_VERTICAL_ANGLE = SENSOR_VERTICAL_ANGLE_VLP16

    End Sub

    Private Sub RibbonButton_VLP16_HiRes_Click(sender As Object, e As EventArgs) Handles VLP16_Hi_Res.Click

        'LiDAR VLP16
        SENSOR_VERTICAL_ANGLE = SENSOR_VERTICAL_ANGLE_Hi_Res

    End Sub

    Private Sub RibbonButton_Next_Click(sender As Object, e As EventArgs) Handles RibbonButton_Next.Click
        'Show Next Data
        If Current_Frame <= LiDAR_No_Frames - 1 Then
            Current_Frame += 1
            RibbonTextBox_Frame.TextBoxText = Current_Frame
            Extract_LiDAR_Data(Current_Frame)
            StatusLabel.Text = "Status: Show Frame=" & Current_Frame
        End If

    End Sub
    Dim StopPlay As Boolean = False
    Private Sub RibbonButton_Play_Click(sender As Object, e As EventArgs) Handles RibbonButton_Play.Click
        'Show Next Data
        StopPlay = False
        If Current_Frame <= LiDAR_No_Frames - 1 Then
            For i = Current_Frame To LiDAR_No_Frames - 1
                If StopPlay = False Then
                    Current_Frame += 1
                    RibbonTextBox_Frame.TextBoxText = Current_Frame
                    Application.DoEvents()
                    Extract_LiDAR_Data(Current_Frame)
                    'Threading.Thread.CurrentThread.Sleep(60)
                    StatusLabel.Text = "Status: Show Frame=" & Current_Frame
                End If
            Next
        End If

    End Sub

    Private Sub RibbonButton_Stop_Click(sender As Object, e As EventArgs) Handles RibbonButton_Stop.Click
        StopPlay = True
    End Sub

    Private Sub RibbonButton_Previous_Click(sender As Object, e As EventArgs) Handles RibbonButton_Previous.Click
        'Show Previous Data
        If Current_Frame > 0 Then
            Current_Frame -= 1
            RibbonTextBox_Frame.TextBoxText = Current_Frame
            Extract_LiDAR_Data(Current_Frame)
            StatusLabel.Text = "Status: Show Frame=" & Current_Frame
        End If

    End Sub

#End Region

#Region "Read Slam Trajectory"

    Private Sub RibbonButtonAddSlamTraj_Click(sender As Object, e As EventArgs) Handles RibbonButtonAddSlamTraj.Click
        Dim OpenFileDialog As New OpenFileDialog
        'Try
        OpenFileDialog.Filter = "Trajectory file |*.poses;*.txt|All Files (*.*) |*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then

            Application.DoEvents()
            WaitForm.Show()
            Read_Trajectory(OpenFileDialog.FileName)
            WaitForm.Hide()
            'Else
            'MsgBox("Please select a valid Slam Trajectory file")
            'Exit Sub
        End If
        'Catch ex As Exception
        'MsgBox("Error in openning Slam Trajectory file")
        'End Try
    End Sub

    Private Sub Read_Trajectory(ByVal TrajFileName As String)

        '--------------------------------------------------------
        'Time,Rx(Roll),Ry(Pitch),Rz(Yaw),X,Y,Z
        Dim TimeStamp, RX, Ry, RZ, X, Y, Z, East, North, Height As String
        Dim count As Integer = 0
        '--------------------------------------------------------
        '
        Using sr As StreamReader = New StreamReader(TrajFileName)
            Dim Trajline As String

            ' Read and display lines from the file until the end of  
            ' the file is reached. 
            Trajline = sr.ReadLine() 'skip first line
            Do Until sr.EndOfStream
                Trajline = sr.ReadLine()
                Dim TrajSent() As String = Trajline.Split(",")
                Dim TrajPoint As New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
                TrajPoint.ID = count
                TrajPoint.TimeStamp = TrajSent(0) * 1000000
                TrajPoint.Rx = TrajSent(1)
                TrajPoint.Ry = TrajSent(2)
                TrajPoint.Rz = TrajSent(3)
                TrajPoint.East = TrajSent(4)
                TrajPoint.North = TrajSent(5)
                TrajPoint.Height = TrajSent(6)

                SlamTrajDataPoints.Add(TrajPoint)
                count += 1
            Loop
            sr.Close()
        End Using

        '--------------------------------------------------------
        listViewProviderSlam.DataList = SlamTrajDataPoints

        StatusLabel.Text = "Status: Complete Reading Slam Trajectory File."

    End Sub


#End Region

#Region "Read PCAP File"

    Private Sub Open_PCAP()
        Dim OpenFileDialog As New OpenFileDialog
        ' Try
        OpenFileDialog.Filter = "LiDAR PCAP file (*.pcap) |*.pcap|All Files (*.*) |*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Application.DoEvents()
            WaitForm.Show()
            Application.DoEvents()
            PcapFileName = OpenFileDialog.FileName
            Read_PCAP()
            LiDARRTK_Form.ActiveForm.Text = "LiDAR RTK:" & PcapFileName
            WaitForm.Hide()
            '  Else
            'MsgBox("Please select a valid PCAP file")
            'Exit Sub
        End If
        '     Catch ex As Exception
        'MsgBox("Error in openning a PCAP file")
        '     End Try
    End Sub

    Private Sub Read_PCAP()

        Dim ObjFileName_without_extention As String = System.IO.Path.GetFileNameWithoutExtension(PcapFileName)
        Dim ObjFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\Obj_Files\"
        If Not System.IO.Directory.Exists(ObjFileName_Path) Then
            System.IO.Directory.CreateDirectory(ObjFileName_Path)
        End If
        OutputObjsFile = ObjFileName_Path & ObjFileName_without_extention '& ".log"

        Dump = New PcapFile(PcapFileName)

        If Dump.Packets.Count > 0 Then
            Packets_Available = True

            'Extract GPS Positions
            Extract_GPS_Data(0, Dump.Packets.Count - 1)

            'Find No. of Frames 
            Extract_Packets()
            Extract_LiDAR_Data(Current_Frame)

        Else
            Packets_Available = False
        End If

        StatusLabel.Text = "Status: Complete Reading PCAC File. No. of Packets=" & Dump.Packets.Count
    End Sub

#End Region

#Region "Extract_LiDAR_Data"

    Private Sub Extract_Packets()

        'Dim CurrentAz As Double = 0
        Dim PreviousAz As Double = 0
        Dim NumberofChar As Integer = 0
        Dim count As Integer = 0
        Dim count2 As Integer = 0
        Dim PacketNo As Integer = 0
        Dim PacketPrev As Integer = 0
        Dim Packetcount As Integer = 0
        Dim Azimuth_Final As Double = 0
        ' Azimuth_Packet = Nothing
        Dim TimeStamp_Decimal As Long = 0
        Dim TimeStamp_Decimal_start As Long = 0

        HListView_Packets.Items.Clear()

        For Each packet As Packet In Dump.Packets

            If packet.Data.Length = 1248 Then 'LiDAR data
                '-----------------------------------------------------------
                'Calculate Azimuth
                '1) Get Azimuth Values (2 bytes, from 44 to 45)
                Dim Azimuth(1) As String
                Azimuth(0) = Conversion.Hex(packet.Data(44))
                Azimuth(1) = Conversion.Hex(packet.Data(45))
                Get_Azimuth(Azimuth, Azimuth_Final)

                'Calculate TimeStamp
                Dim TimeStamp(3) As String
                Dim k As Integer = 0
                '1) Get Time Stamp Values (4 bytes, from 1242 to 1245)
                For jj As UInteger = 1242 To 1245
                    TimeStamp(k) = Conversion.Hex(packet.Data(jj))
                    k += 1
                Next
                Get_TimeStamp(TimeStamp, TimeStamp_Decimal)
                If count2 = 0 Then
                    TimeStamp_Decimal_start = TimeStamp_Decimal
                End If
                If Azimuth_Final < PreviousAz Then
                    ' Azimuth_Packet &= Azimuth_Final
                    Dim Packets_Point As New Packets_DataItem(0, 0, 0, 0, 0)

                    ' Dim arr(4) As String
                    ' arr(0) = Packetcount : arr(1) = PacketPrev : arr(2) = count - 1
                    ' arr(3) = TimeStamp_Decimal_start : arr(4) = TimeStamp_Decimal
                    ' arr(3) = Azimuth_Packet
                    Packets_Point.Frame_ID = Packetcount : Packets_Point.Packet_From = PacketPrev : Packets_Point.Packet_To = count - 1
                    Packets_Point.TimeStampFrom = TimeStamp_Decimal_start : Packets_Point.TimeStampTo = TimeStamp_Decimal


                    PacketPrev = count
                    Packetcount += 1
                    PreviousAz = 0
                    count2 = -1
                    If count <= 2 Then
                        PacketPrev -= 1
                        Packetcount -= 1
                        PreviousAz = 0
                        count2 = 0
                        GoTo 10

                    End If
                    FramesDataPoints.Add(Packets_Point)
                    ' Dim itm As ListViewItem = New ListViewItem(arr)
                    ' HListView_Packets.Items.Add(itm)
                    '  Azimuth_Packet = Nothing
                Else
                    ' Azimuth_Packet &= Azimuth_Final & ","
                    PreviousAz = Azimuth_Final
                End If
                '------------------------------------------------------------
10:
                count += 1
                count2 += 1
            Else
                count += 1

                ' Azimuth_Packet &= "0,"
            End If
        Next
        listViewProviderPackets.DataList = FramesDataPoints

        RibbonLabel_NoFrames.Text = "No. of Frames: " & Packetcount - 1
        RibbonTextBox_Frame.TextBoxText = Current_Frame
        LiDAR_No_Frames = Packetcount - 1
    End Sub

    Private Sub Extract_LiDAR_Data(ByVal Frame_ID As Integer)

        Dim OutputObjFile As String = OutputObjsFile & "_frame_" & Frame_ID & ".obj"

        Dim Packet_From As Integer = FramesDataPoints(Frame_ID).Packet_From 'Val(HListView_Packets.Items(Frame_ID).SubItems(1).Text)
        Dim Packet_To As Integer = FramesDataPoints(Frame_ID).Packet_To 'Val(HListView_Packets.Items(Frame_ID).SubItems(2).Text)

        Dim PacketDataLength As String = Nothing
        Dim Azimuth_Final As Double
        Dim Distance_Final As Double
        Dim Channel_No As Integer = 0
        Dim Channel_Distances(31) As Double
        Dim Channel_Azimuth(31) As Double
        Dim Fring_Sequence As Integer = -1
        Dim TimeOffset As Double = 0
        Dim X, Y, Z As Double

        Dim TimeStamp_Decimal As Long = 0
        Dim Count As Integer = 0
        Dim Reflectivity As Integer = 0
        Dim NumberofChar As Integer = 0

        ' Dim Az_Next() As String = Split(ListView_LiDAR_Packets_Info.Items(Frame_ID).SubItems(3).Text, ",")

        FileOpen(3, OutputObjFile, OpenMode.Output)
        LiDARDataPoints = New List(Of LiDAR_DataItem)()
        HListView_LiDAR.Items.Clear()

        For i = Packet_From To Packet_To
            Dim P_TimestampSeconds As String = Dump.Packets(i).TimestampSeconds
            Dim P_TimestampMilliseconds As String = Dump.Packets(i).TimestampMilliseconds
            PacketDataLength = Dump.Packets(i).Data.Length
            If PacketDataLength = 1248 Then 'LiDAR data
                Fring_Sequence = -1
                TimeOffset = 0
                'Calculate TimeStamp
                Dim TimeStamp(3) As String
                Dim k As Integer = 0
                '1) Get Time Stamp Values (4 bytes, from 1242 to 1245)
                For jj As UInteger = 1242 To 1245
                    TimeStamp(k) = Conversion.Hex(Dump.Packets(i).Data(jj))
                    k += 1
                Next
                Get_TimeStamp(TimeStamp, TimeStamp_Decimal)
                '------------------------------------------------------------
                For DataBlock_index As Integer = 0 To DATA_BLOCKS_IN_FRAME - 1
                    Dim frame_index As Integer = 0
                    '-----------------------------------------------------------
                    'Calculate Azimuth
                    '1) Get Azimuth Values (2 bytes, from 44 to 45 (Data Block 1))
                    Dim Azimuth(1) As String
                    ' Azimuth(0) = Conversion.Hex(Dump.Packets(i).Data(44))
                    ' Azimuth(1) = Conversion.Hex(Dump.Packets(i).Data(45))
                    Azimuth(0) = Conversion.Hex(Dump.Packets(i).Data(((44 + DataBlock_index * 100))))
                    Azimuth(1) = Conversion.Hex(Dump.Packets(i).Data(((45 + DataBlock_index * 100))))
                    Get_Azimuth(Azimuth, Azimuth_Final)
                    azimuth_ = Azimuth_Final
                    'Get Azimuth Next for interpolation
                    If DataBlock_index < 11 Then 'Same Packet
                        Azimuth(0) = Conversion.Hex(Dump.Packets(i).Data(((44 + (DataBlock_index + 1) * 100))))
                        Azimuth(1) = Conversion.Hex(Dump.Packets(i).Data(((45 + (DataBlock_index + 1) * 100))))
                        Get_Azimuth(Azimuth, Azimuth_Final)
                        azimuthN = Azimuth_Final
                    ElseIf DataBlock_index = 11 Then 'Next Packet
                        'Check length of next packect
                        PacketDataLength = Dump.Packets(i + 1).Data.Length
                        If PacketDataLength = 1248 Then
                            Azimuth(0) = Conversion.Hex(Dump.Packets(i + 1).Data(((44))))
                            Azimuth(1) = Conversion.Hex(Dump.Packets(i + 1).Data(((45))))
                            Get_Azimuth(Azimuth, Azimuth_Final)
                            azimuthN = Azimuth_Final
                            ' azimuthN = Val(Az_Next((i - Packet_From) + 1))
                        Else
                            Azimuth(0) = Conversion.Hex(Dump.Packets(i + 2).Data(((44))))
                            Azimuth(1) = Conversion.Hex(Dump.Packets(i + 2).Data(((45))))
                            Get_Azimuth(Azimuth, Azimuth_Final)
                            azimuthN = Azimuth_Final
                            ' azimuthN = Val(Az_Next((i - Packet_From) + 1))
                            'azimuthN = Val(ListView_LiDAR_Packets_Info.Items(Frame_ID).SubItems(3).Text)
                            'MsgBox("Packet=" & i & " Az=" & azimuth_ & " Az Next=" & azimuthN & " Az Next data=" & Az_Next((i - Packet_From) + 1))
                            'GoTo 11
                        End If

                    End If

                    ' azimuth_interpolated = azimuth_interpolation(azimuthN, azimuth_)

                    azimuth_interpolation2(azimuthN, azimuth_, Channel_Azimuth)
                    '------------------------------------------------------------
                    For point_index As Integer = 0 To POINTS_IN_READ - 1
                        If point_index = 0 Then
                            Fring_Sequence += 1
                        End If

                        'Calculate Distance
                        '1) Get Distance Values (2 bytes, from 46 to 47 (Data Block 1))
                        Dim Distance(1) As String
                        'Distance(0) = Conversion.Hex(Dump.Packets(i).Data(46))
                        'Distance(1) = Conversion.Hex(Dump.Packets(i).Data(47))
                        Distance(0) = Conversion.Hex(Dump.Packets(i).Data(46 + DataBlock_index * 100 + 3 * point_index))
                        Distance(1) = Conversion.Hex(Dump.Packets(i).Data(47 + DataBlock_index * 100 + 3 * point_index))
                        Get_Distance(Distance, Distance_Final)
                        If Distance_Final = 0 Then GoTo 10
                        '------------------------------------------------------------
                        'Get Reflectivity (1 byte, from 48 (Data Block 1))
                        'Reflectivity = CInt("&H" & Conversion.Hex(Dump.Packets(i).Data(48)))
                        Reflectivity = CInt("&H" & Conversion.Hex(Dump.Packets(i).Data(48 + DataBlock_index * 100 + 3 * point_index)))
                        '------------------------------------------------------------
                        X = calc_X_cooridnates(Distance_Final, Channel_Azimuth(point_index), point_index)
                        Y = calc_Y_cooridnates(Distance_Final, Channel_Azimuth(point_index), point_index)
                        Z = calc_Z_cooridnates(Distance_Final, Channel_Azimuth(point_index), point_index)
                        '------------------------------------------------------------
                        Dim LiDAR_Point As New LiDAR_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                        LiDAR_Point.ID = Count
                        LiDAR_Point.Azimuth = Channel_Azimuth(point_index)
                        LiDAR_Point.Distance = Distance_Final
                        LiDAR_Point.Intensity = Reflectivity

                        If point_index > 15 Then
                            Channel_No = point_index - 16
                        ElseIf point_index < 15 Then
                            Channel_No = point_index
                        ElseIf point_index = 15 Then
                            Channel_No = point_index
                            Fring_Sequence += 1
                        End If
                        '---------------------------------------------------------
                        'Calculate TimeStap offset
                        TimeOffset = 55.296 * Fring_Sequence + 2.304 * Channel_No
                        Dim ExactPointTime As Double = TimeStamp_Decimal + TimeOffset
                        '---------------------------------------------------------
                        LiDAR_Point.Laser_ID = Channel_No
                        LiDAR_Point.TimeStamp = ExactPointTime
                        LiDAR_Point.Vertical_Angle = SENSOR_VERTICAL_ANGLE(point_index)

                        LiDAR_Point.X = Math.Round(X, NoDecimals)
                        LiDAR_Point.Y = Math.Round(Y, NoDecimals)
                        LiDAR_Point.Z = Math.Round(Z, NoDecimals)

                        PrintLine(3, "v " & X & " " & Y & " " & Z) ' & " " & LiDAR_Point.Intensity & " " & LiDAR_Point.Intensity & " " & LiDAR_Point.Intensity)
                        LiDARDataPoints.Add(LiDAR_Point)
                        Count += 1
10:
                    Next

                Next

            End If
11:

        Next
        listViewProvider.DataList = LiDARDataPoints

        Current_Frame = Frame_ID
        FileClose(3)
        LoadModelFromFile(OutputObjFile)
    End Sub

    Private Sub Get_Azimuth(ByVal Azimuth() As String, ByRef Azimuth_Final As Double)
        Dim NumberofChar As Integer = 0
        Dim Azimuth_Combined As String = Nothing
        Dim Azimuth_Decimal As Integer = 0
        '1) Get Azimuth Values (2 bytes, from 44 to 45)
        '2)Reverse the bytes
        Array.Reverse(Azimuth)

        '3) Combine the bytes
        For kk = 0 To 1
            NumberofChar = Azimuth(kk).Count
            If NumberofChar = 1 Then
                Azimuth_Combined &= "0" & Azimuth(kk)
            Else
                Azimuth_Combined &= Azimuth(kk)
            End If
        Next
        '4)Convert into decimal

        Azimuth_Decimal = CInt("&H" & Azimuth_Combined)
        Azimuth_Final = Azimuth_Decimal / 100
    End Sub

    Private Sub Get_Distance(ByVal Distance() As String, ByRef Distance_Final As Double)
        Dim NumberofChar As Integer = 0
        Dim Distance_Decimal As Integer = 0
        Dim Distance_Combined As String = Nothing
        '1) Get Distance Values (2 bytes, from 46 to 47)
        '2)Reverse the bytes
        Array.Reverse(Distance)
        NumberofChar = 0
        '3) Combine the bytes
        For kk = 0 To 1
            NumberofChar = Distance(kk).Count
            If NumberofChar = 1 Then
                Distance_Combined &= "0" & Distance(kk)
            Else
                Distance_Combined &= Distance(kk)
            End If
        Next
        '4)Convert into decimal

        Distance_Decimal = CInt("&H" & Distance_Combined)
        Distance_Final = Distance_Decimal * 0.002
    End Sub

    Private Sub Get_TimeStamp(ByVal TimeStamp() As String, ByRef TimeStamp_Decimal As Long)
        Dim NumberofChar As Integer = 0
        Dim TimeStamp_Combined As String = Nothing
        '1) Get Time Stamp Values (4 bytes, from 1242 to 1245)
        '2)Reverse the bytes
        Array.Reverse(TimeStamp)
        '3) Combine the bytes
        For kk = 0 To 3
            NumberofChar = TimeStamp(kk).Count
            If NumberofChar = 1 Then
                TimeStamp_Combined &= "0" & TimeStamp(kk)
            Else
                TimeStamp_Combined &= TimeStamp(kk)
            End If
        Next
        '4)Convert into decimal
        TimeStamp_Decimal = CLng("&H" & TimeStamp_Combined)
    End Sub

    Private Function azimuth_interpolation(ByVal azimuthT As Double, ByVal azimuth_tempT As Double) As Double
        Dim azimuth_interpolatedT As Double = 0

        If azimuthT < azimuth_tempT Then
            azimuthT = azimuthT + 360
        End If

        azimuth_interpolatedT = azimuth_tempT + ((azimuthT - azimuth_tempT) / 2)

        If azimuth_interpolatedT > 360 Then
            azimuth_interpolatedT = azimuth_interpolatedT - 360
        End If

        Return azimuth_interpolatedT
    End Function

    Private Sub azimuth_interpolation2(ByVal azimuthT As Double, ByVal azimuth_tempT As Double, ByRef Az_Inter() As Double)
        Dim azimuth_interpolatedT As Double = 0
        'ReDim Az_Inter(31)
        If azimuthT < azimuth_tempT Then
            azimuthT = azimuthT + 360
        End If
        For i = 0 To 31
            azimuth_interpolatedT = azimuth_tempT + ((azimuthT - azimuth_tempT) / 32 * i)

            If azimuth_interpolatedT > 360 Then
                azimuth_interpolatedT = azimuth_interpolatedT - 360
            End If
            Az_Inter(i) = azimuth_interpolatedT

        Next
    End Sub

    Private Function calc_X_cooridnates(ByVal distance As Double, ByVal azimuth As Double, ByVal i As Integer) As Double
        Dim coordinate_X As Double = distance * Math.Cos(SENSOR_VERTICAL_ANGLE(i) * deg_to_rad_coeef) * Math.Sin(azimuth * deg_to_rad_coeef)
        Return coordinate_X
    End Function

    Private Function calc_Y_cooridnates(ByVal distance As Double, ByVal azimuth As Double, ByVal i As Integer) As Double
        Dim coordinate_Y As Double = distance * Math.Cos(SENSOR_VERTICAL_ANGLE(i) * deg_to_rad_coeef) * Math.Cos(azimuth * deg_to_rad_coeef)
        Return coordinate_Y
    End Function

    Private Function calc_Z_cooridnates(ByVal distance As Double, ByVal azimuth As Double, ByVal i As Integer) As Double
        Dim coordinate_Z As Double = distance * Math.Sin(SENSOR_VERTICAL_ANGLE(i) * deg_to_rad_coeef)
        Return coordinate_Z
    End Function

#End Region

#Region "Extract_RTK DATA"

    Private Sub Extract_GPS_Data(ByVal From_Packet As Integer, ByVal To_Packet As Integer)
        '--------------------------------------------
        'Extract GPS position data from PCAP file
        '--------------------------------------------
        Dim TimestampSeconds As String = Nothing
        Dim TimestampMilliseconds As String = Nothing
        Dim PacketDataLength As String = Nothing
        '--------------------------------------------------------
        'GPRMC Nmea Data
        Dim TimeStamp(3) As String : Dim GPSTime As String = Nothing
        Dim validity As String : Dim Latitude As String
        Dim North_South As String : Dim Longitude As String
        Dim Altitude As String = "0"
        Dim East_West As String : Dim Speed As String
        Dim TrueCourse As String : Dim DateStamp As String
        Dim Variation As String : Dim East_West_Var As String
        Dim NoSats, HDOP, Geoid_Separation As String
        Dim NumberofChar As Integer = 0
        Dim East, North As Double
        '--------------------------------------------------------

        GPSDataPoints = New List(Of GPS_DataItem)()
        HListView_GPS.Items.Clear()

        If Packets_Available = True Then

            Dim count As Integer = 0

            For i = From_Packet To To_Packet
                'TimestampSeconds = Dump.Packets(i).TimestampSeconds
                'TimestampMilliseconds = Dump.Packets(i).TimestampMilliseconds
                PacketDataLength = Dump.Packets(i).Data.Length

                Dim TimeStamp_Combined As String = Nothing
                Dim k As Integer = 0

                If PacketDataLength = 554 Then 'GPS Position data
                    Dim GPRMC As String = Nothing
                    '------------------------------------------------------
                    '1) Get Time Stamp Values (4 bytes, from 240 to 243)
                    For jj As UInteger = 240 To 243
                        TimeStamp(k) = Conversion.Hex(Dump.Packets(i).Data(jj))
                        k += 1
                    Next
                    '2)Reverse the bytes
                    Array.Reverse(TimeStamp)
                    '3) Combine the bytes

                    For kk = 0 To 3
                        NumberofChar = TimeStamp(kk).Count
                        If NumberofChar = 1 Then
                            TimeStamp_Combined &= "0" & TimeStamp(kk)
                        Else
                            TimeStamp_Combined &= TimeStamp(kk)
                        End If
                    Next

                    '4)Convert into decimal

                    ' Dim TimeStamp_Decimal As Integer = CInt("&H" & TimeStamp_Combined)
                    Dim TimeStamp_Decimal As Long = CLng("&H" & TimeStamp_Combined)
                    ' MsgBox("TimeStamp_Decimal=" & TimeStamp_Decimal & " TimeStamp_Decimal lng=" & time_stamp)

                    ' Dim timestamp_HHbyte As Byte = Dump.Packets(i).Data(240) 'VLP16_frame(1204 + frame_index *  1206)
                    ' Dim timestamp_HLbyte As Byte = Dump.Packets(i).Data(241) 'VLP16_frame(1203 + frame_index *  1206)
                    ' Dim timestamp_LHbyte As Byte = Dump.Packets(i).Data(242) 'VLP16_frame(1202 + frame_index *  1206)
                    ' Dim timestamp_LLbyte As Byte = Dump.Packets(i).Data(243) 'VLP16_frame(1201 + frame_index *  1206)
                    '  Dim time_stamp As Integer = timestamp_HHbyte << 24 Or timestamp_HLbyte << 16 Or timestamp_LHbyte << 8 Or timestamp_LLbyte
                    'Data[idx] + Data[idx+1]*256 + data[idx+2]*256*256 + data[idx+3]*256*256*256
                    'Dim TimeStamp_Decimal As Integer = ((Dump.Packets(i).Data(240)) _
                    '+ ((Dump.Packets(i).Data(241) *  256) _
                    '+ ((Dump.Packets(i).Data(242) *  256 *  256) _
                    '+ Dump.Packets(i).Data(243) *  256 *  256 *  256)))
                    ' MsgBox("time_stamp=" & time_stamp & " TimeStamp_Decimal=" & TimeStamp_Decimal)

                    '-----------------------------------------------------
                    'Check if we have GPRMC
                    If (Dump.Packets(i).Data(248)) > 0 Then
                        'Get GPRMC Message
                        For j As UInteger = 248 To 320
                            GPRMC &= Convert.ToChar(Dump.Packets(i).Data(j))
                        Next
                        Dim NmeaSent() As String = GPRMC.Split(",")
                        If NmeaSent(0) = "$GPRMC" Then
                            ParseGPRMC(GPRMC, GPSTime, validity, Latitude, North_South, Longitude,
                                    East_West, Speed, TrueCourse, DateStamp, Variation, East_West_Var)
                        ElseIf NmeaSent(0) = "$GPGGA" Then
                            ParseGPGGA(GPRMC, GPSTime, validity, Latitude, North_South, Longitude,
                                    East_West, Altitude, NoSats, HDOP, Geoid_Separation)
                        End If

                        Dim GPS_Point As New GPS_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                        GPS_Point.ID = count : GPS_Point.GPSTime = GPSTime : GPS_Point.Latitude = Latitude
                        GPS_Point.Longitude = Longitude : GPS_Point.Height = Altitude : GPS_Point.Quality = validity
                        GPS_Point.Time_Stamp = TimeStamp_Decimal : GPS_Point.Heading = TrueCourse : GPS_Point.Speed = Speed

                        GPSDataPoints.Add(GPS_Point)

                        count += 1

                    End If
                End If
            Next
        End If
        listViewProviderGPS.DataList = GPSDataPoints


    End Sub

    Private Sub ParseGPGGA(ByVal GPGGA_Sentence As String, ByRef UTCTimeStamp As String,
                           ByRef validity As String, ByRef Latitude As String,
                           ByRef North_South As String, ByRef Longitude As String,
                           ByRef East_West As String, ByRef Altitude As String,
                           ByRef NoSats As String, ByRef HDOP As String,
                           ByRef Geoid_Separation As String)
        Dim GPGGA_Data As String() = GPGGA_Sentence.Split(",")
        If GPGGA_Data(2) = Nothing Then Exit Sub

        '$GPGGA,172814.0,3723.46587704,N,12202.26957864,W,2,6,1.2,18.893,M,-25.669,M,2.0 0031*4F
        '    0 ,    1   ,      2      ,3,    4         ,5,6,7, 8 ,  9   ,10,  11  ,12, 13  14 * 15
        '0   Message ID $GPGGA
        '1   UTC of position fix
        '2   Latitude
        '3   Direction of latitude:
        '    N:      North
        '    S:      South

        '4   Longitude
        '5   Direction of longitude:
        '    E:      East
        '    W:      West

        '6   GPS Quality indicator:
        '    0:      Fix Not valid
        '    1:      GPS Fix
        '    2:      Differential GPS fix (DGNSS), SBAS, OmniSTAR VBS, Beacon, RTX in GVBS mode
        '    3: Not applicable
        '    4:      RTK Fixed, xFill
        '    5:      RTK Float, OmniSTAR XP/HP, Location RTK, RTX
        '    6:      INS Dead reckoning

        '7   Number of SVs in use, range from 00 through to 24+
        '8   HDOP
        '9   Orthometric height (MSL reference)
        '10  M: unit of measure for orthometric height Is meters
        '11  Geoid separation
        '12  M: geoid separation measured in meters
        '13  Age of differential GPS data record, Type 1 Or Type 9. Null field when DGPS Is Not used.
        '14  Reference station ID, range 0000 to 4095. A null field when any reference station ID Is selected And no corrections are received. See table below for a description of the field values.
        '15  The checksum data, always begins with * 

        UTCTimeStamp = GPGGA_Data(1)
        Dim UtcHours As Integer = Convert.ToInt32(UTCTimeStamp.Substring(0, 2))
        Dim UtcMinutes As Integer = Convert.ToInt32(UTCTimeStamp.Substring(2, 2))
        Dim UtcSeconds As Integer = Convert.ToInt32(UTCTimeStamp.Substring(4, 2))
        ' Extract milliseconds if it Is available
        If UTCTimeStamp.Length > 7 Then
            Dim UtcMilliseconds As Integer = Convert.ToInt32(UTCTimeStamp.Substring(6) * 1000)
        End If

        Dim Lat As Double = Val(GPGGA_Data(2)) / 100
        Dim LatDM As String() = Lat.ToString.Split(".")
        Dim LatD As Integer = CInt(LatDM(0))
        Dim LatM As Double = CDbl("0." & LatDM(1)) * 100 / 60 'convert to decimal
        Lat = LatD + LatM

        North_South = GPGGA_Data(3)
        If North_South = "S" Then Lat *= -1
        Latitude = Lat.ToString

        Dim Lon As Double = Val(GPGGA_Data(4)) / 100
        Dim LonDM As String() = Lon.ToString.Split(".")

        Dim LonD As Integer = CInt(LonDM(0))
        Dim LonM As Double = CDbl("0." & LonDM(1)) * 100 / 60 'convert to decimal
        Lon = LonD + LonM

        East_West = GPGGA_Data(5)
        If East_West = "W" Then Lon *= -1
        Longitude = Lon.ToString

        'Quality
        If GPGGA_Data(6) = "0" Then
            validity = "Fix Not valid"
        ElseIf GPGGA_Data(6) = "1" Then
            validity = "GPS Fix"
        ElseIf GPGGA_Data(6) = "2" Then
            validity = "DGNSS"
        ElseIf GPGGA_Data(6) = "3" Then
            validity = "Not applicable"
        ElseIf GPGGA_Data(6) = "4" Then
            validity = "RTK Fixed"
        ElseIf GPGGA_Data(6) = "5" Then
            validity = "RTK Float"
        ElseIf GPGGA_Data(6) = "6" Then
            validity = "INS Dead reckoning"
        End If

        NoSats = GPGGA_Data(7)
        HDOP = GPGGA_Data(8)
        Altitude = GPGGA_Data(9)
        Geoid_Separation = GPGGA_Data(11)

    End Sub

    Private Sub ParseGPRMC(ByVal GPRMC_Sentence As String, ByRef UTCTimeStamp As String,
                           ByRef validity As String, ByRef Latitude As String,
                           ByRef North_South As String, ByRef Longitude As String,
                           ByRef East_West As String, ByRef Speed As String,
                           ByRef TrueCourse As String, ByRef DateStamp As String,
                           ByRef Variation As String, ByRef East_West_Var As String)
        Try


            'Dim GPRMC_Data As String() = GPRMC_Sentence.Split(" Then,")
            Dim GPRMC_Data As String() = GPRMC_Sentence.Split(",")
            If GPRMC_Data(2) = Nothing Then Exit Sub
            ' $GPRMC,220516,A,5133.82,N,00042.24,W,173.8,231.8,130694,004.2,W*70
            '    0      1   2    3    4     5    6   7    8      9      10 11 12
            '1 220516 Time Stamp
            '2 A validity - A-ok, V-invalid
            '3 5133.82 current Latitude
            '4 N North/South
            '5 00042.24 current Longitude
            '6 W East/West
            '7 173.8 Speed in knots
            '8 231.8 True course
            '9 130694 Date Stamp
            '10 004.2 Variation
            '11 W East/West
            '12 * 70 checksum
            UTCTimeStamp = GPRMC_Data(1)
            Dim UtcHours As Integer = Convert.ToInt32(UTCTimeStamp.Substring(0, 2))
            Dim UtcMinutes As Integer = Convert.ToInt32(UTCTimeStamp.Substring(2, 2))
            Dim UtcSeconds As Integer = Convert.ToInt32(UTCTimeStamp.Substring(4, 2))
            ' Extract milliseconds if it Is available
            If UTCTimeStamp.Length > 7 Then
                Dim UtcMilliseconds As Integer = Convert.ToInt32(UTCTimeStamp.Substring(6) * 1000)
            End If

            validity = GPRMC_Data(2)

            Dim Lat As Double = Val(GPRMC_Data(3)) / 100
            Dim LatDM As String() = Lat.ToString.Split(".")
            Dim LatD As Integer = CInt(LatDM(0))
            Dim LatM As Double = CDbl("0." & LatDM(1)) * 100 / 60 'convert to decimal
            Lat = LatD + LatM

            North_South = GPRMC_Data(4)
            If North_South = "S" Then Lat *= -1
            Latitude = Lat.ToString

            Dim Lon As Double = Val(GPRMC_Data(5)) / 100
            Dim LonDM As String() = Lon.ToString.Split(".")

            Dim LonD As Integer = CInt(LonDM(0))
            Dim LonM As Double = CDbl("0." & LonDM(1)) * 100 / 60 'convert to decimal
            Lon = LonD + LonM

            East_West = GPRMC_Data(6)
            If East_West = "W" Then Lon *= -1
            Longitude = Lon.ToString

            If GPRMC_Data(7) = Nothing Then
                Speed = "0.000"
            Else
                Speed = GPRMC_Data(7)
            End If

            If GPRMC_Data(8) = Nothing Then
                TrueCourse = "0.000"
            Else
                TrueCourse = GPRMC_Data(8)
            End If

            DateStamp = GPRMC_Data(9)
            Variation = GPRMC_Data(10)

            Dim Mag_Var As String() = GPRMC_Data(11).Split("*")
            East_West_Var = Mag_Var(0)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ReadRTKdataButton_Click(sender As Object, e As EventArgs) Handles ReadRTKdataButton.Click
        Dim OpenFileDialog As New OpenFileDialog
        ' Try
        OpenFileDialog.Filter = "NMEA file (*.txt) |*.txt|All Files (*.*) |*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Application.DoEvents()
            WaitForm.Show()
            Application.DoEvents()
            Read_NMEA(OpenFileDialog.FileName) '(Update for GPS position data from PCAP LiDAR File) 
            AddGPS2RTK_Traj()
            WaitForm.Hide()
        End If
        '     Catch ex As Exception
        'MsgBox("Error in openning a NMEA file")
        '     End Try
    End Sub

    Private Sub Read_NMEA(ByVal NMEAFileName As String)
        '--------------------------------------------------------
        'GPRMC Nmea Data
        Dim TimeStamp(3) As String : Dim GPSTime As String = Nothing
        Dim validity As String : Dim Latitude As String
        Dim North_South As String : Dim Longitude As String
        Dim Altitude As String = "0"
        Dim East_West As String : Dim Speed As String
        Dim TrueCourse As String : Dim DateStamp As String
        Dim Variation As String : Dim East_West_Var As String
        Dim NoSats, HDOP, Geoid_Separation As String
        Dim NumberofChar As Integer = 0
        Dim count As Integer = 0
        Dim index As Integer = 0
        Dim GGAdata() As String
        'GGAListbox
        '--------------------------------------------------------
        'Read NMEA file and store results in a temprary listbox
        Using sr As StreamReader = New StreamReader(NMEAFileName)
            Dim NMEAline As String

            ' Read and display lines from the file until the end of  
            ' the file is reached. 
            NMEAline = sr.ReadLine()
            Do Until sr.EndOfStream
                If NMEAline.Contains("$GPGGA") Then
                    GGAListbox.Add(NMEAline)
                End If
                NMEAline = sr.ReadLine()
            Loop
            sr.Close()
        End Using

        For Each GGAMessage As String In GGAListbox
            Dim NmeaSent() As String = GGAMessage.Split(",")
            If NmeaSent(2) = Nothing Then GoTo 10
            ParseGPGGA(GGAMessage, GPSTime, validity, Latitude, North_South, Longitude,
                                East_West, Altitude, NoSats, HDOP, Geoid_Separation)
            If North_South = "S" Then
                Latitude = -1 * Latitude
            End If
            If East_West = "W" Then
                Longitude = -1 * Longitude
            End If
            Dim c As CoordinateSharp.Coordinate = New CoordinateSharp.Coordinate(Latitude, Longitude)
            Dim NMEA_GGA As String = GPSTime & "," & Altitude & "," & validity & "," & c.UTM.Easting & "," & c.UTM.Northing
            TempNMEAList.Add(NMEA_GGA)
            'ListBoxNMEA.Items.Add(NMEA_GGA)
10:
        Next
        Application.DoEvents()
        StatusLabel.Text = "Status: End of Read NMEA file: Completed"
        ' End Using

        '--------------------------------------------------------
        'Add NMEA data to GPS data (Update for GPS position data from PCAP LiDAR File)
        Dim GPS_Time, East, North As String
        GPSTime = 0
        count = 0
        ' For Each myItem As GPS_DataItem In GPSDataPoints 'HListView_GPS.Items
        For i = 0 To GPSDataPoints.Count - 1
            GPS_Time = GPSDataPoints(i).GPSTime
            If GPS_Time = GPSTime Then
                GPSDataPoints(i).Height = Altitude
                GPSDataPoints(i).Quality = validity
                GPSDataPoints(i).East = East
                GPSDataPoints(i).North = North
            Else

                For k = index To TempNMEAList.Count - 1
                    GGAdata = TempNMEAList(k).Split(",")
                    If Val(GGAdata(0)) = Val(GPS_Time) Then
                        index = k
                        GoTo 11
                    End If
                Next
11:
                'index = ListBoxNMEA.FindString(GPS_Time)
                ' GGAdata = ListBoxNMEA.Items(index).split(",")
                GGAdata = TempNMEAList(index).Split(",")

                'MsgBox(ListBoxNMEA.Items(index))
                GPSTime = GGAdata(0)
                Altitude = GGAdata(1)
                validity = GGAdata(2)
                East = GGAdata(3)
                North = GGAdata(4)

                GPSDataPoints(i).Height = Altitude
                GPSDataPoints(i).Quality = validity
                GPSDataPoints(i).East = East
                GPSDataPoints(i).North = North
            End If

            'For i = 0 To ListBoxNMEA.Items.Count - 1
            'ListBoxNMEA.SelectedIndex = i
            'MsgBox("text: " & ListBoxNMEA.Text & "index: " & i)
            'Next
            count += 1
        Next

        listViewProviderGPS.DataList = GPSDataPoints
        Application.DoEvents()
        StatusLabel.Text = "Status: End Add NMEA to GPS Data: Completed"
        'AddGPS2RTK_Traj()
    End Sub

    Private Sub AddGPS2RTK_Traj()
        Dim Time, LastTime As String
        ' Dim arr(8) As String
        Dim H_Distance As Double = 0
        Dim Last_Point(1) As Double
        Dim Current_Point(1) As Double
        'Dim counter As Integer = 0
        'Try

        Dim startExport As Boolean = False
        'For Each myItem As ListViewItem In HListView_GPS.Items
        For Each myItem As GPS_DataItem In GPSDataPoints
            ' counter += 1
            If startExport = False Then
                LastTime = myItem.GPSTime
                If myItem.Quality = "RTK Fixed" Then
                    Last_Point(0) = Convert.ToDouble(myItem.East)
                    Last_Point(1) = Convert.ToDouble(myItem.North)

                    Dim NMEA_Point1 As New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)

                    NMEA_Point1.GPS_Time = myItem.GPSTime : NMEA_Point1.East = myItem.East
                    NMEA_Point1.North = myItem.North : NMEA_Point1.Height = myItem.Height
                    NMEA_Point1.Time_Stamp = myItem.TimeStamp : NMEA_Point1.Speed = myItem.Speed
                    NMEA_Point1.Distance = 0 : NMEA_Point1.Heading = myItem.Heading
                    NMEA_Point1.Azimuth = 0

                    RTK_DataPoints.Add(NMEA_Point1)

                    startExport = True
                End If


            Else

                Time = myItem.GPSTime
                If Time <> LastTime Then
                    LastTime = myItem.GPSTime
                    If myItem.Quality = "RTK Fixed" Then
                        Current_Point(0) = myItem.East
                        Current_Point(1) = myItem.North
                        Dim DE As Double = Current_Point(0) - Last_Point(0)
                        Dim DN As Double = Current_Point(1) - Last_Point(1)
                        H_Distance = Math.Sqrt((DE) ^ 2 + (DN) ^ 2)
                        Dim radians As Double = Math.Atan2(DE, DN)
                        Dim Azimuth = radians * (180 / Math.PI)
                        If Azimuth < 0 Then
                            Azimuth += 360
                        End If
                        ' arr(6) = H_Distance : arr(7) = myItem.SubItems(7).Text
                        ' arr(8) = Azimuth
                        Last_Point(0) = Convert.ToDouble(myItem.East)
                        Last_Point(1) = Convert.ToDouble(myItem.North)
                        'Dim itm As ListViewItem = New ListViewItem(arr)
                        Dim NMEA_Point As New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)

                        NMEA_Point.GPS_Time = myItem.GPSTime : NMEA_Point.East = myItem.East
                        NMEA_Point.North = myItem.North : NMEA_Point.Height = myItem.Height
                        NMEA_Point.Time_Stamp = myItem.TimeStamp : NMEA_Point.Speed = myItem.Speed
                        NMEA_Point.Distance = H_Distance : NMEA_Point.Heading = myItem.Heading
                        NMEA_Point.Azimuth = Azimuth

                        RTK_DataPoints.Add(NMEA_Point)
                    End If
                End If
            End If
        Next
        listViewProviderNMEA.DataList = RTK_DataPoints
        Application.DoEvents()
        StatusLabel.Text = "Status: Add GPS RTK Data to Trajectory: Completed"
        'Catch ex As Exception

        'End Try

    End Sub

    Private Sub LatLon2UTM_Test()
        '//Seattle coordinates on 5 Jun 2018 @ 10:10 AM (UTC)
        'Coordinate c = New Coordinate(47.6062, -122.3321, New DateTime(2018, 6, 5, 10, 10, 0));

        'Console.WriteLine(c);                       // N 47º 36' 22.32" W 122º 19' 55.56"
        'Console.WriteLine(c.CelestialInfo.SunSet);  // 5-Jun-2018 4:02:00 AM
        'Console.WriteLine(c.UTM);                   // 10T 550200Me 5272748mN
    End Sub

    Private Sub llh2xyz(ByVal latlonh As Vector3d, ByRef XYZ As Vector3d)

        ' Convert lat, long, height in WGS84 to ECEF X,Y,Z
        'lat and long given in decimal degrees.
        latlonh.X = latlonh.X / 180 * Math.PI 'converting to radians
        latlonh.Y = latlonh.Y / 180 * Math.PI 'converting to radians
        Dim a As Double = 6378137.0 ' earth semimajor axis in meters 
        Dim f As Double = 1 / 298.257223563 ' reciprocal flattening 
        Dim e2 As Double = 2 * f - f ^ 2 ' eccentricity squared 

        Dim chi As Double = Math.Sqrt(1 - e2 * (Math.Sin(latlonh.X)) ^ 2)
        XYZ.X = (a / chi + latlonh.Z) * Math.Cos(latlonh.X) * Math.Cos(latlonh.Y)
        XYZ.Y = (a / chi + latlonh.Z) * Math.Cos(latlonh.X) * Math.Sin(latlonh.Y)
        XYZ.Z = (a * (1 - e2) / chi + latlonh.Z) * Math.Sin(latlonh.X)


    End Sub

#End Region

#Region "Process"

    Private Sub ProcessButton_Click(sender As Object, e As EventArgs) Handles ProcessButton.Click

        Application.DoEvents()
        WaitForm.Show()

        FindStaticGPS()

        '-------------------------------------------------------------------------------
        'Not Used now!!!
        'Modify_Slam_Static_data(SlamTrajDataPoints, SlamTrajDataPoints_ModifiedStatic)
        '-------------------------------------------------------------------------------

        'Option 1:Interpolate data original slam data (SlamTrajDataPoints)
        InterpolateRTK_Traj(SlamTrajDataPoints, TrajectoryDataPoints, TrajectoryDataPointsAll, 0)

        'Option 2:Interpolate data out from Modify_Slam_Static_data
        'InterpolateRTK_Traj(SlamTrajDataPoints_ModifiedStatic, TrajectoryDataPoints, TrajectoryDataPointsAll, 0)

        Dim SyncGPS As Double = SyncRTK
        Smooth_Slam_AllData(TrajectoryDataPointsAll, TrajectorySmoothedDataPointsAll)

        InterpolateRTK_Traj(TrajectorySmoothedDataPointsAll, TrajectorySyncSmoothedDataPoints, TrajectorySyncSmoothedDataPointsAll, SyncGPS)
        'Remove duplicate points.
        RemoveDuplicate(TrajectorySyncSmoothedDataPointsAll)

        Dim Tranformed_Data As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)

        Transform_Slam_Trajectory(TrajectorySyncSmoothedDataPointsAll, Tranformed_Data, SyncGPS)
        TrajectoryTransDataPoints = Tranformed_Data

        WaitForm.Hide()

    End Sub

    Private Sub RemoveDuplicate(ByRef inData As List(Of Trajectory_DataItem))
        Dim outData As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
        For i = 0 To inData.Count - 2
            If inData(i).TimeStamp <> inData(i + 1).TimeStamp Then
                outData.Add(inData(i))
            End If
        Next
        If inData(inData.Count - 2).TimeStamp <> inData(inData.Count - 1).TimeStamp Then
            outData.Add(inData(inData.Count - 1))
        End If
        inData = New List(Of Trajectory_DataItem)
        inData = outData
    End Sub

    Private Sub FindStaticGPS()
        ' StaticGPSData:(GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp_Start,TimeStamp_Start,Speed,Distance,Heading,Azimuth)

        Dim LastFrame, CurrentFrame, FirstStaticFrame As NMEA_DataItem
        Dim StartStatic As Boolean = False : Dim FirstFrame As Boolean = True : Dim EndFrame As Boolean = False
        Dim count As Integer = -1
        Dim countAllFrames As Integer = RTK_DataPoints.Count - 1
        LastFrame = New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)
        CurrentFrame = New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)
        FirstStaticFrame = New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)

        For Each GPSTrajFrame In RTK_DataPoints
            CurrentFrame = GPSTrajFrame
            count += 1
            If count = 0 Then
                FirstFrame = False
                LastFrame = CurrentFrame
                GoTo 10
            Else
                'MsgBox("Distance=" & GPSTrajFrame.Dist)
                If GPSTrajFrame.Dist < 0.05 Then
                    'If GPSTrajFrame.Heading = 0 Then
                    If StartStatic = False Then
                        StartStatic = True
                        FirstStaticFrame = LastFrame
                        If countAllFrames = count Then 'last frame
                            TempStaticGPSData.Add(CurrentFrame.GPSTime & "," & CurrentFrame.GPSTime &
                                    "," & CurrentFrame.East & "," & CurrentFrame.North &
                                    "," & CurrentFrame.Height & "," & CurrentFrame.TimeStamp & "," & CurrentFrame.TimeStamp &
                                     "," & CurrentFrame.Speed & "," & CurrentFrame.Distance &
                                      "," & CurrentFrame.Heading & "," & CurrentFrame.Azimuth)
                        End If
                        GoTo 10
                    Else
                        LastFrame = CurrentFrame
                        If countAllFrames = count Then 'last frame
                            TempStaticGPSData.Add(FirstStaticFrame.GPSTime & "," & CurrentFrame.GPSTime &
                                    "," & CurrentFrame.East & "," & CurrentFrame.North &
                                    "," & CurrentFrame.Height & "," & FirstStaticFrame.TimeStamp & "," & CurrentFrame.TimeStamp &
                                     "," & CurrentFrame.Speed & "," & CurrentFrame.Distance &
                                      "," & CurrentFrame.Heading & "," & CurrentFrame.Azimuth)
                        End If
                        GoTo 10
                    End If
                Else
                    If StartStatic = True Then
                        StartStatic = False
                        TempStaticGPSData.Add(FirstStaticFrame.GPSTime & "," & LastFrame.GPSTime &
                                                        "," & LastFrame.East & "," & LastFrame.North &
                                    "," & LastFrame.Height & "," & FirstStaticFrame.TimeStamp & "," & LastFrame.TimeStamp &
                                     "," & LastFrame.Speed & "," & LastFrame.Distance &
                                      "," & LastFrame.Heading & "," & LastFrame.Azimuth)

                        If countAllFrames = count Then
                            TempStaticGPSData.Add(FirstStaticFrame.GPSTime & "," & CurrentFrame.GPSTime &
                                    "," & CurrentFrame.East & "," & CurrentFrame.North &
                                    "," & CurrentFrame.Height & "," & FirstStaticFrame.TimeStamp & "," & CurrentFrame.TimeStamp &
                                     "," & CurrentFrame.Speed & "," & CurrentFrame.Distance &
                                      "," & CurrentFrame.Heading & "," & CurrentFrame.Azimuth)
                        End If
                        LastFrame = CurrentFrame
                        GoTo 10
                    Else
                        ' MsgBox("Find Point" & LastFrame.GPSTime)
                        TempStaticGPSData.Add(LastFrame.GPSTime & "," & LastFrame.GPSTime &
                                                     "," & LastFrame.East & "," & LastFrame.North &
                                    "," & LastFrame.Height & "," & LastFrame.TimeStamp & "," & LastFrame.TimeStamp &
                                     "," & LastFrame.Speed & "," & LastFrame.Distance &
                                      "," & LastFrame.Heading & "," & LastFrame.Azimuth)
                        LastFrame = CurrentFrame
                        If countAllFrames = count Then
                            TempStaticGPSData.Add(CurrentFrame.GPSTime & "," & CurrentFrame.GPSTime &
                                    "," & CurrentFrame.East & "," & CurrentFrame.North &
                                    "," & CurrentFrame.Height & "," & CurrentFrame.TimeStamp & "," & CurrentFrame.TimeStamp &
                                     "," & CurrentFrame.Speed & "," & CurrentFrame.Distance &
                                      "," & CurrentFrame.Heading & "," & CurrentFrame.Azimuth)
                        End If
                        GoTo 10
                    End If
                End If
            End If
            '---------------------
10:
        Next

        '---------------------------------------------------
        'Find Static Info
        For i = 0 To TempStaticGPSData.Count - 1
            Dim TempStaticdata() As String = TempStaticGPSData(i).Split(",")
            If Val(TempStaticdata(1)) - Val(TempStaticdata(0)) <> 0 Then
                StaticPointsInfo.Add(TempStaticdata(0) & "," & TempStaticdata(1) & "," & TempStaticdata(5) & "," & TempStaticdata(6))
            End If
        Next

        Dim StaticCount As Integer = 0
        '-----------------------------------------------------------------
        'Calculate average RTK Static positions
        Dim index As Integer = 0
        Dim index_S As Integer = 0
        Dim index_E As Integer = 0
        Dim East_Avg As Double = 0
        Dim North_Avg As Double = 0
        Dim Height_Avg As Double = 0
        For epoch = 0 To TempStaticGPSData.Count - 1
            ' StaticGPSData:(GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp,Speed,Distance,Heading,Azimuth)

            Dim StaticData() As String = TempStaticGPSData(epoch).Split(",")
            If Val(StaticData(0)) <> Val(StaticData(1)) Then 'Static data
                index = 0
                index_S = 0 : index_E = 0
                East_Avg = 0
                North_Avg = 0
                Height_Avg = 0
                For i = index To RTK_DataPoints.Count - 1
                    If RTK_DataPoints(i).GPSTime = Val(StaticData(0)) Then
                        index_S = i
                    ElseIf RTK_DataPoints(i).GPSTime = Val(StaticData(1)) Then
                        index_E = i
                        index = i
                        GoTo 11
                    End If
                Next
11:
                For i = index_S To index_E
                    East_Avg += RTK_DataPoints(i).East
                    North_Avg += RTK_DataPoints(i).North
                    Height_Avg += RTK_DataPoints(i).Height
                Next
                Dim countSum As Integer = 0
                countSum = index_E - index_S + 1
                East_Avg = East_Avg / countSum
                North_Avg = North_Avg / countSum
                Height_Avg = Height_Avg / countSum
                StaticGPSData.Add(StaticData(0) & "," & StaticData(1) & "," &
                    East_Avg & "," & North_Avg & "," & Height_Avg & "," & StaticData(5) & "," &
                    StaticData(6) & "," & StaticData(7) & "," & StaticData(8))
            Else
                StaticGPSData.Add(TempStaticGPSData(epoch))
            End If
12:
        Next

        '------------------------------------------------
        'Static Sync data
        For i = 0 To StaticGPSData.Count - 1
            Dim MyData() As String = StaticGPSData(i).Split(",")
            Dim MySyncData As String = MyData(0) & "," & MyData(1) & "," & MyData(2) & "," & MyData(3) & "," & MyData(4) & "," &
                   Val(MyData(5)) + SyncRTK & "," & Val(MyData(6)) + SyncRTK & "," & MyData(7) & "," & MyData(8)
            StaticSyncGPSData.Add(MySyncData)
        Next



    End Sub

    Private Sub Modify_Slam_Static_data(ByVal In_Data As List(Of Trajectory_DataItem), ByRef Out_Data As List(Of Trajectory_DataItem))
        Out_Data = In_Data
        Dim index As Integer = 0
        Dim index_S As Integer = 0
        Dim index_E As Integer = 0
        Dim East_Avg As Double = 0
        Dim North_Avg As Double = 0
        Dim Height_Avg As Double = 0
        Dim StaticPoint_IDs As String = Nothing
        ' SlamTrajDataPoints_ModifiedStatic
        '-----------------------------------------------------------------
        'Calculate average Slam Static positions based on RTK static data
        Dim count_frames As Integer = 0
        Dim StaticPoint() As String
        Dim StaticAvg As Vector3d = New Vector3d(0, 0, 0)
        Dim StaticPointsAvg As List(Of Vector3d) = New List(Of Vector3d)
        '------------------------------------------------------------
        '1-Calculate Distance between all points
        Dim SlamDistance As List(Of Single) = New List(Of Single)
        For i = 0 To In_Data.Count - 2
            Dim Distance As Single = Math.Sqrt(((In_Data(i + 1).East - In_Data(i).East)) ^ 2 +
                ((In_Data(i + 1).North - In_Data(i).North)) ^ 2)
            SlamDistance.Add(Distance)
        Next
        '----------------------------------------------------------
        '2 Determine the static frames according to distance <0.05m
        Dim framecount As Integer = 0
        Dim FrameIDs As String = Nothing
        Dim NoFrames() As String
        For i = 0 To SlamDistance.Count - 2
            If SlamDistance(i) < 0.05 Then
                FrameIDs &= i & ","
                framecount += 1
            Else
                If framecount > 0 Then
                    NoFrames = FrameIDs.Split(",")
                    If SlamDistance(i + 1) < 0.05 Then
                        FrameIDs &= i & ","
                        framecount += 1
                        GoTo 10
                    Else
                        If NoFrames.Count > 100 Then
                            FrameIDs = FrameIDs.TrimEnd(CChar(","))
                            SlamStaticFrames.Add(FrameIDs)
                            FrameIDs = Nothing
                            framecount = 0
                        Else
                            framecount = 0
                            FrameIDs = Nothing
                            GoTo 10
                        End If
                    End If
                End If
            End If
            'SlamDistance.Add(Distance)
10:
        Next
        '----------------------------------------------------------
        '3 calculate average of static points
        For i = 0 To SlamStaticFrames.Count - 1
            East_Avg = 0
            North_Avg = 0
            Height_Avg = 0
            count_frames = 0
            NoFrames = SlamStaticFrames(i).Split(",")
            For j = 0 To NoFrames.Count - 2
                ' If NoFrames(j) <> Nothing Then
                If SlamDistance(Val(NoFrames(j))) < 0.05 Then
                    East_Avg += In_Data(Val(NoFrames(j))).East
                    North_Avg += In_Data(Val(NoFrames(j))).North
                    Height += In_Data(Val(NoFrames(j))).Height
                    count_frames += 1
                End If
                ' End If
            Next
            StaticAvg = New Vector3d(0, 0, 0)
            StaticAvg.X = East_Avg / (count_frames)
            StaticAvg.Y = North_Avg / (count_frames)
            StaticAvg.Z = Height_Avg / (count_frames)

            '4 Update Slam data according to Static data
            For j = 0 To NoFrames.Count - 1
                If NoFrames(j) <> Nothing Then
                    Out_Data(Val(NoFrames(j))).East = StaticAvg.X
                    Out_Data(Val(NoFrames(j))).North = StaticAvg.Y
                    Out_Data(Val(NoFrames(j))).Height = StaticAvg.Z
                End If
            Next
        Next
    End Sub

    Private Sub InterpolateRTK_Traj(ByVal InTraj As List(Of Trajectory_DataItem), ByRef OutTraj_Interp As List(Of Trajectory_DataItem), ByRef OutTraj_Interp_All As List(Of Trajectory_DataItem), ByVal SyncGPS As Double)
        'TimeStamp based interpolation
        Dim RTK_index As Integer = 0
        'Find the first RTK point that located within the SlamTrajectory
        If InTraj(0).TimeStamp > RTK_DataPoints(RTK_index).TimeStamp + SyncGPS Then
            For i = 0 To RTK_DataPoints.Count - 1
                If RTK_DataPoints(i).TimeStamp + SyncGPS < InTraj(0).TimeStamp Then
                    RTK_index += 1
                Else
                    GoTo 1
                End If
            Next
        End If
1:
        Dim TrajPoint As New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
        For i = 0 To InTraj.Count - 1
            If InTraj(i).TimeStamp > RTK_DataPoints(RTK_index).TimeStamp + SyncGPS Then
                TrajPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)

                TrajPoint.TimeStamp = RTK_DataPoints(RTK_index).TimeStamp + SyncGPS
                ' TrajPoint.Frame_ID = i - 1
                TrajPoint.Frame_ID = -1
                Interpolate_XYZ(InTraj(i - 1), InTraj(i), TrajPoint)
                OutTraj_Interp.Add(TrajPoint)
                OutTraj_Interp_All.Add(TrajPoint)

                TrajPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
                TrajPoint.ID = InTraj(i).ID
                TrajPoint.TimeStamp = InTraj(i).TimeStamp
                TrajPoint.Rotation_X = InTraj(i).Rotation_X
                TrajPoint.Rotation_Y = InTraj(i).Rotation_Y
                TrajPoint.Rotation_Z = InTraj(i).Rotation_Z
                TrajPoint.East = InTraj(i).East
                TrajPoint.North = InTraj(i).North
                TrajPoint.Height = InTraj(i).Height
                OutTraj_Interp_All.Add(TrajPoint)

                If RTK_index < RTK_DataPoints.Count - 1 Then
                    RTK_index += 1
                Else
                    GoTo 10
                End If
            Else
                TrajPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
                TrajPoint.ID = InTraj(i).ID
                TrajPoint.TimeStamp = InTraj(i).TimeStamp
                TrajPoint.Rotation_X = InTraj(i).Rotation_X
                TrajPoint.Rotation_Y = InTraj(i).Rotation_Y
                TrajPoint.Rotation_Z = InTraj(i).Rotation_Z
                TrajPoint.East = InTraj(i).East
                TrajPoint.North = InTraj(i).North
                TrajPoint.Height = InTraj(i).Height
                OutTraj_Interp_All.Add(TrajPoint)
            End If
        Next
10:

        'listViewProviderTrajectory.DataList = TrajectoryDataPoints
        listViewProviderTrajectory.DataList = TrajectoryDataPointsAll
    End Sub

    Private Sub Interpolate_XYZ(ByVal InXYZFrom As Trajectory_DataItem, ByVal InXYZTo As Trajectory_DataItem, ByRef OutXYZ As Trajectory_DataItem)
        'MsgBox("Input InXYZFrom X=" & InXYZFrom.X & ",Y=" & InXYZFrom.Y & ",Z=" & InXYZFrom.Z)
        'MsgBox("Output OutXYZ TimeStamp=" & OutXYZ.TimeStamp)

        Dim DeltaTt1 As Long = (OutXYZ.TimeStamp) - InXYZFrom.TimeStamp
        Dim DeltaT12 As Long = InXYZTo.TimeStamp - InXYZFrom.TimeStamp
        Dim RatioTime As Double = DeltaTt1 / DeltaT12
        ' MsgBox("DeltaTt1=" & DeltaTt1 & " DeltaTt2=" & DeltaT12& & " RatioTime=" & RatioTime)

        Dim DeltaX As Double = InXYZTo.East - InXYZFrom.East
        OutXYZ.East = InXYZFrom.East + DeltaX * RatioTime
        Dim DeltaY As Double = InXYZTo.North - InXYZFrom.North
        OutXYZ.North = InXYZFrom.North + DeltaY * RatioTime
        Dim DeltaZ As Double = InXYZTo.Height - InXYZFrom.Height
        OutXYZ.Height = InXYZFrom.Height + DeltaZ * RatioTime
        ' MsgBox("E=" & OutXYZ.East & ",N=" & OutXYZ.North & ",H=" & OutXYZ.Height)
    End Sub

    Dim SegmentRawData As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
    Dim SegmentSmoothedData As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
    Dim LastSegmentSmoothedData As List(Of String) = New List(Of String)

    Private Sub Smooth_Slam_AllData(ByVal In_Data As List(Of Trajectory_DataItem), ByRef OutSmooth_Data As List(Of Trajectory_DataItem))

        Find_Segments_StaticData()

        Dim Polynomials_Order As Integer = 6
        Dim countSlam As Integer = In_Data.Count - 1

        Dim Frame_Counter As Integer = 0

        Dim StartAdddata As Boolean = False
        Dim StartLoop As Integer = Frame_Counter
        Dim PointsCount_1 As Integer = 0
        Dim PointsCount_2 As Integer = 0
        Dim PointsCount_3 As Integer = 0
        Dim PointsCount_4 As Integer = 0
        Dim SmoothPoint As Trajectory_DataItem
        '---------------------------------------
        For i = 0 To Segments.Count - 1

            PointsCount_1 = 0
            PointsCount_2 = 0
            PointsCount_3 = 0
            PointsCount_4 = 0
            SegmentRawData = New List(Of Trajectory_DataItem)
            SegmentSmoothedData = New List(Of Trajectory_DataItem)

            Dim Segmentdata() As String = Segments(i).Split(",")
            StartAdddata = False

            Dim StaticCounter As Integer = 0
            'Find Trajectory data for the point
            '---------------------------------------
            'Find Data point 1
            If i = 0 Then
                '-------------------------------------------------------------
                'Add First Static data for interpolation
                'Dim FrameIDs() As String = SlamStaticFrames(0).Split(",")
                'For k = 0 To FrameIDs.Count - 1
                'AddSegmentData(In_Data(FrameIDs(k)), SegmentRawData)
                'StartLoop += 1
                'Next
                ' GoTo StartPoint1
                '-------------------------------------------------------------
                'Add all data in the first frame
                For k = StartLoop To In_Data.Count - 1
                    If In_Data(k).TimeStamp < Val(Segmentdata(0)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        StartLoop += 1
                    Else
                        GoTo StartPoint1
                    End If

                Next
StartPoint1:
                '-------------------------------------------------------------
                StartLoop = Frame_Counter
                For k = StartLoop To In_Data.Count - 1
                    Frame_Counter = k
                    If In_Data(k).TimeStamp >= Val(Segmentdata(0)) Then
                        If In_Data(k).TimeStamp >= Val(Segmentdata(1)) Then
                            AddSegmentData(In_Data(k), SegmentRawData)
                            GoTo Point2Start
                        Else
                            AddSegmentData(In_Data(k), SegmentRawData)
                        End If
                    End If
                Next
            Else
                StartLoop = Frame_Counter
                For k = StartLoop To In_Data.Count - 1
                    Frame_Counter = k
                    If In_Data(k).TimeStamp >= Val(Segmentdata(0)) Then
                        If In_Data(k).TimeStamp >= Val(Segmentdata(1)) Then
                            AddSegmentData(In_Data(k), SegmentRawData)
                            GoTo Point2Start
                        Else
                            AddSegmentData(In_Data(k), SegmentRawData)
                        End If
                    End If
                Next
            End If
Point2Start:
            PointsCount_1 = SegmentRawData.Count
            '---------------------------------------
            'Find Data point 2 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(2)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(3)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        GoTo Point3Start
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
Point3Start:
            PointsCount_2 = SegmentRawData.Count - PointsCount_1
            '---------------------------------------
            'Find Data point 3 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(4)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(5)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        GoTo Point4Start
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
Point4Start:
            PointsCount_3 = SegmentRawData.Count - PointsCount_1 - PointsCount_2
            '---------------------------------------
            'Find Data point 4 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(6)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(7)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        If i = Segments.Count - 1 Then
                            'Add last static data
                            For kk = k To In_Data.Count - 1
                                If In_Data(kk).TimeStamp > Val(Segmentdata(7)) Then
                                    AddSegmentData(In_Data(kk), SegmentRawData)
                                End If

                            Next

                            'Dim LastFrameIDs() As String = SlamStaticFrames(SlamStaticFrames.Count - 1).Split(",")
                            'For kk = 0 To LastFrameIDs.Count - 1
                            'AddSegmentData(In_Data(LastFrameIDs(kk)), SegmentRawData)
                            'Next
                        End If
                        GoTo NextStartSmooth
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
            '---------------------------------------
NextStartSmooth:
            PointsCount_4 = SegmentRawData.Count - PointsCount_1 - PointsCount_2 - PointsCount_3
            ' MsgBox("PointsCount_i Total+1,2,3,4=" & i & "," & SegmentRawData.Count & "," & PointsCount_1 & "," & PointsCount_2 & "," & PointsCount_3 & "," & PointsCount_4)

            If SegmentRawData.Count < 5 Then
                ' MsgBox(SegmentRawData.Count)
                GoTo 11
            End If
            Frame_Counter = 0
            'Smooth data
            Smooth_data_PolynomialFit(SegmentRawData, SegmentSmoothedData, Polynomials_Order)
            'Add data smoothed data
            If i = 0 Then
                For ii = PointsCount_1 To (PointsCount_1 + PointsCount_2)
                    LastSegmentSmoothedData.Add(SegmentSmoothedData(ii).ID & "," & SegmentSmoothedData(ii).TimeStamp & "," & SegmentSmoothedData(ii).Rx &
                                 "," & SegmentSmoothedData(ii).Ry & "," & SegmentSmoothedData(ii).Rz & "," & SegmentSmoothedData(ii).East &
                                  "," & SegmentSmoothedData(ii).North & "," & SegmentSmoothedData(ii).Height)
                Next
                For k = 0 To PointsCount_1 - 1
                    OutSmooth_Data.Add(SegmentSmoothedData(k))
                Next
            Else
                For ii = 0 To LastSegmentSmoothedData.Count - 1
                    Dim SmoothPoints() As String = LastSegmentSmoothedData(ii).Split(",")
                    SmoothPoint = SegmentSmoothedData(ii)
                    SmoothPoint.East = 0.5 * (SmoothPoint.East + Val(SmoothPoints(5)))
                    SmoothPoint.North = 0.5 * (SmoothPoint.North + Val(SmoothPoints(6)))
                    SmoothPoint.Height = 0.5 * (SmoothPoint.Height + Val(SmoothPoints(7)))
                    If OutSmooth_Data(OutSmooth_Data.Count - 1).ID <> SmoothPoint.ID Then
                        OutSmooth_Data.Add(SmoothPoint)
                    Else
                        If OutSmooth_Data(OutSmooth_Data.Count - 1).East <> SmoothPoint.East And OutSmooth_Data(OutSmooth_Data.Count - 1).North <> SmoothPoint.North And OutSmooth_Data(OutSmooth_Data.Count - 1).Height <> SmoothPoint.Height Then
                            OutSmooth_Data.Add(SmoothPoint)
                        End If
                    End If
                Next

                LastSegmentSmoothedData = New List(Of String)
                For ii = PointsCount_1 To (PointsCount_1 + PointsCount_2) - 1
                    LastSegmentSmoothedData.Add(SegmentSmoothedData(ii).ID & "," & SegmentSmoothedData(ii).TimeStamp & "," & SegmentSmoothedData(ii).Rx &
                                 "," & SegmentSmoothedData(ii).Ry & "," & SegmentSmoothedData(ii).Rz & "," & SegmentSmoothedData(ii).East &
                                  "," & SegmentSmoothedData(ii).North & "," & SegmentSmoothedData(ii).Height)
                Next
            End If
11:
        Next
        'Add last data
        For ii = 0 To SegmentSmoothedData.Count - PointsCount_1 - 1
            OutSmooth_Data.Add(SegmentSmoothedData(ii))
        Next

    End Sub

    Private Sub AddSegmentData(ByVal data As Trajectory_DataItem, ByRef SegData As List(Of Trajectory_DataItem))
        If SegData.Count = 0 Then
            SegData.Add(data)
        Else
            For i = 0 To SegData.Count - 1
                If SegData(i).TimeStamp = data.TimeStamp Then
                    GoTo 10
                End If
            Next
            SegData.Add(data)
        End If
10:
    End Sub

    Private Sub Smooth_data_PolynomialFit(ByVal inRawData As List(Of Trajectory_DataItem), ByRef outSmoothedData As List(Of Trajectory_DataItem), ByVal PolynomialDegree As Integer)
        outSmoothedData = New List(Of Trajectory_DataItem)
        Dim PointsInXYZ As New List(Of Vector3d)()
        Dim MyPoint As Vector3d = New Vector3d(0, 0, 0)
        Dim i As Integer = 0
        For Each inPoint As Trajectory_DataItem In inRawData
            MyPoint = New Vector3d(0, 0, 0)
            MyPoint.X = inPoint.East
            MyPoint.Y = inPoint.North
            MyPoint.Z = inPoint.Height
            PointsInXYZ.Add((MyPoint))
        Next

        Dim count As Integer = PointsInXYZ.Count - 1

        'Calculate Distance between points
        Dim AccumalatedDistance As Double = 0
        Dim Distdata(count) As Double
        Dim xdata(count) As Double
        Dim ydata(count) As Double
        Dim zdata(count) As Double

        Dim Distance As Double = 0
        Distdata(0) = AccumalatedDistance
        xdata(0) = PointsInXYZ(0).X
        ydata(0) = PointsInXYZ(0).Y
        zdata(0) = PointsInXYZ(0).Z

        For i = 1 To count
            Distance = Math.Sqrt((PointsInXYZ(i - 1).X - PointsInXYZ(i).X) ^ 2 + (PointsInXYZ(i - 1).Y - PointsInXYZ(i).Y) ^ 2)
            AccumalatedDistance += Distance
            Distdata(i) = AccumalatedDistance
            xdata(i) = PointsInXYZ(i).X
            ydata(i) = PointsInXYZ(i).Y
            zdata(i) = PointsInXYZ(i).Z
        Next

        'Polynomial Fit X,Y,Z
        Dim Polynomial_X() As Double = MathNet.Numerics.Fit.Polynomial(Distdata, xdata, PolynomialDegree)
        Dim Polynomial_Y() As Double = MathNet.Numerics.Fit.Polynomial(Distdata, ydata, PolynomialDegree)
        Dim Polynomial_Z() As Double = MathNet.Numerics.Fit.Polynomial(Distdata, zdata, PolynomialDegree)

        Dim xResult(count) As Double
        Dim yResult(count) As Double
        Dim zResult(count) As Double

        'Calculate Polynomail Interpolation X,Y,Z
        For i = 0 To count
            xResult(i) = Polynomial.Evaluate(Distdata(i), Polynomial_X)
            yResult(i) = Polynomial.Evaluate(Distdata(i), Polynomial_Y)
            zResult(i) = Polynomial.Evaluate(Distdata(i), Polynomial_Z)
        Next
        i = 0
        For Each inPoint As Trajectory_DataItem In inRawData
            Dim MySmoothPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
            MySmoothPoint = inPoint
            MySmoothPoint.ID = inPoint.ID
            MySmoothPoint.East = Polynomial.Evaluate(Distdata(i), Polynomial_X)
            MySmoothPoint.North = Polynomial.Evaluate(Distdata(i), Polynomial_Y)
            MySmoothPoint.Height = Polynomial.Evaluate(Distdata(i), Polynomial_Z)
            outSmoothedData.Add((MySmoothPoint))
            i += 1
        Next

    End Sub

    Private Sub Find_Segments_StaticData()

        '************Find Segments************
        '-----------------------------------------
        'Find segments based on static data
        Dim count As Integer = 0
        Dim Inline() As String
        Dim Inline2() As String
        Dim count_i As Integer = 0
        Dim Segment As String = Nothing
        Dim LastSegment As String = Nothing
        For i = 1 To StaticGPSData.Count - 2
            count_i = i
            Inline = StaticGPSData(i).Split(",")
            Inline2 = StaticGPSData(i + 1).Split(",")
            If count = 3 Then
                Segment &= Inline(5) & "," & Inline2(5)
                Segments.Add(Segment)
                Inline2 = Segment.Split(",")
                Segment = Inline2(2) & "," & Inline2(3) & "," & Inline2(4) & "," & Inline2(5) & "," & Inline2(6) & "," & Inline2(7) & ","
                count = 3
                GoTo 10
            Else
                Segment &= Inline(5) & "," & Inline2(5) & ","
                count += 1
            End If
10:
        Next

        '-----------------------------------------
        GoTo 11
        'Export Segemnts from Static data
        Dim PcapFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName)
        Dim myWriter As New IO.StreamWriter(PcapFileName_Path & "\Segments.txt")
        myWriter.WriteLine("Point1_TS-From,Point1_TS-To,Point2_TS-From,Point2_TS-To,Point3_TS-From,Point3_TS-To,Point4_TS-From,Point4_TS-To")
        For i = 0 To Segments.Count - 1
            myWriter.WriteLine(Segments(i))
        Next
        myWriter.Close()
11:
    End Sub

    Private Sub Transform_Slam_Trajectory(ByVal In_Data As List(Of Trajectory_DataItem), ByRef Trans_Trajectory As List(Of Trajectory_DataItem), ByVal SyncGPS As Integer)
        Dim Frame_Counter As Integer = 0
        Dim TransformedPoint As Trajectory_DataItem
        Dim CurrentTransformedPoint As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
        Dim InputLidarPoints As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
        Dim LastSegmentTransformedData As List(Of String) = New List(Of String)
        Dim AddMorePoints As Boolean = False
        Dim TransLiDARPointXY As Vector3d = New Vector3d
        Dim TransLiDARPointZ As Vector3d = New Vector3d
        Dim TransLiDARPointTemp As Vector3d = New Vector3d
        Dim ParamsXY(3, 0), ParamsZ(3, 0) As Double
        Dim NoCommonPoints As Integer = 0
        '---------------------------------------
        'Convert Static data to RTK-Data Items
        '"GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp_From,TimeStamp_To,Speed,Distance,Heading,Azimuth")
        Dim RTK_InPoints As List(Of NMEA_DataItem) = New List(Of NMEA_DataItem)
        For Each myItem As String In StaticGPSData
            Dim RTKPoint() As String = myItem.Split(",")
            Dim RTK_Point As NMEA_DataItem = New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)
            RTK_Point.East = Val(RTKPoint(2)) : RTK_Point.North = Val(RTKPoint(3))
            RTK_Point.Height = Val(RTKPoint(4)) : RTK_Point.TimeStamp = Val(RTKPoint(6)) + SyncGPS
            RTK_InPoints.Add(RTK_Point)
        Next
        '  Try
        InputLidarPoints = New List(Of Trajectory_DataItem)
        For i = 0 To 1000
            TransformedPoint = In_Data(i)
            TransformedPoint.ID = In_Data(i).ID
            InputLidarPoints.Add(TransformedPoint)
        Next
        AddMorePoints = False
        NoCommonPoints = 0
        ReDim ParamsXY(0, 0)
        ReDim ParamsZ(0, 0)
        CurrentTransformedPoint = New List(Of Trajectory_DataItem)
        Find_Trans_Parameters(RTK_InPoints, InputLidarPoints, CurrentTransformedPoint, NoCommonPoints, AddMorePoints)
        For ii = 0 To CurrentTransformedPoint.Count - 1
            Trans_Trajectory.Add(CurrentTransformedPoint(ii))
        Next
        'Exit Sub
        '-------------------------------------------------------------------
        'Helmert Test
        ' Dim HelmertParams(,) As Double
        'Dim HelmertRes() As Vector3d
        'Find_Trans_Parameters_Helmert(RTK_InPoints, InputLidarPoints, HelmertParams, HelmertRes, NoCommonPoints, AddMorePoints)
        'Exit Sub
        '-------------------------------------------------------------------

        InputLidarPoints = New List(Of Trajectory_DataItem)
        For i = 1001 To 2000
            TransformedPoint = In_Data(i)
            TransformedPoint.ID = In_Data(i).ID
            InputLidarPoints.Add(TransformedPoint)
        Next
        AddMorePoints = False
        NoCommonPoints = 0
        ReDim ParamsXY(0, 0)
        ReDim ParamsZ(0, 0)
        CurrentTransformedPoint = New List(Of Trajectory_DataItem)
        Find_Trans_Parameters(RTK_InPoints, InputLidarPoints, CurrentTransformedPoint, NoCommonPoints, AddMorePoints)
        For ii = 0 To CurrentTransformedPoint.Count - 1
            Trans_Trajectory.Add(CurrentTransformedPoint(ii))
        Next

        InputLidarPoints = New List(Of Trajectory_DataItem)
        For i = 2001 To 3000
            TransformedPoint = In_Data(i)
            TransformedPoint.ID = In_Data(i).ID
            InputLidarPoints.Add(TransformedPoint)
        Next
        AddMorePoints = False
        NoCommonPoints = 0
        ReDim ParamsXY(0, 0)
        ReDim ParamsZ(0, 0)
        CurrentTransformedPoint = New List(Of Trajectory_DataItem)
        Find_Trans_Parameters(RTK_InPoints, InputLidarPoints, CurrentTransformedPoint, NoCommonPoints, AddMorePoints)
        For ii = 0 To CurrentTransformedPoint.Count - 1
            Trans_Trajectory.Add(CurrentTransformedPoint(ii))
        Next

        InputLidarPoints = New List(Of Trajectory_DataItem)
        For i = 3001 To In_Data.Count - 1
            TransformedPoint = In_Data(i)
            TransformedPoint.ID = In_Data(i).ID
            InputLidarPoints.Add(TransformedPoint)
        Next
        AddMorePoints = False
        NoCommonPoints = 0
        ReDim ParamsXY(0, 0)
        ReDim ParamsZ(0, 0)
        CurrentTransformedPoint = New List(Of Trajectory_DataItem)
        Find_Trans_Parameters(RTK_InPoints, InputLidarPoints, CurrentTransformedPoint, NoCommonPoints, AddMorePoints)
        For ii = 0 To CurrentTransformedPoint.Count - 1
            Trans_Trajectory.Add(CurrentTransformedPoint(ii))
        Next
        ' Catch ex As Exception

        'End Try
    End Sub

    Private Sub Transform_Slam_Trajectory_Segments(ByVal In_Data As List(Of Trajectory_DataItem), ByRef Trans_Trajectory As List(Of Trajectory_DataItem), ByVal SyncGPS As Integer)
        Dim Frame_Counter As Integer = 0
        Dim StartAdddata As Boolean = False
        Dim StartLoop As Integer = Frame_Counter
        Dim PointsCount_1 As Integer = 0
        Dim PointsCount_2 As Integer = 0
        Dim PointsCount_3 As Integer = 0
        Dim PointsCount_4 As Integer = 0
        Dim TransformedPoint As Trajectory_DataItem
        Dim CurrentTransformedPoint As List(Of Trajectory_DataItem) = New List(Of Trajectory_DataItem)
        Dim LastSegmentTransformedData As List(Of String) = New List(Of String)
        Dim AddMorePoints As Boolean = False
        Dim SementsInfo As List(Of String) = New List(Of String)
        Dim MySegmentInfo As String = Nothing
        Dim MySegmentInfoSplit() As String
        Dim MyPreviousSegmentInfoSplit() As String
        Dim TransLiDARPointXY As Vector3d = New Vector3d
        Dim TransLiDARPointZ As Vector3d = New Vector3d
        Dim TransLiDARPointTemp As Vector3d = New Vector3d
        Dim DistanceLidar As Double
        Dim ParamsXY(3, 0), ParamsZ(3, 0) As Double
        Dim NoCommonPoints As Integer = 0
        '---------------------------------------
        'Convert Static data to RTK-Data Items
        '"GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp_From,TimeStamp_To,Speed,Distance,Heading,Azimuth")
        Dim RTK_InPoints As List(Of NMEA_DataItem) = New List(Of NMEA_DataItem)
        For Each myItem As String In StaticGPSData
            Dim RTKPoint() As String = myItem.Split(",")
            Dim RTK_Point As NMEA_DataItem = New NMEA_DataItem(0, 0, 0, 0, 0, 0, 0, 0, 0)
            RTK_Point.East = Val(RTKPoint(2)) : RTK_Point.North = Val(RTKPoint(3))
            RTK_Point.Height = Val(RTKPoint(4)) : RTK_Point.TimeStamp = Val(RTKPoint(6)) + SyncGPS
            RTK_InPoints.Add(RTK_Point)
        Next
        '  Try


        '-------------------------------------------
        For i = 0 To Segments.Count - 1

            PointsCount_1 = 0
            PointsCount_2 = 0
            PointsCount_3 = 0
            PointsCount_4 = 0
            SegmentRawData = New List(Of Trajectory_DataItem)
            SegmentSmoothedData = New List(Of Trajectory_DataItem)

            Dim Segmentdata() As String = Segments(i).Split(",")
            StartAdddata = False

            Dim StaticCounter As Integer = 0
            'Find Trajectory data for the point
            '---------------------------------------
            'Find Data point 1
            If i = 0 Then
                '-------------------------------------------------------------
                'Add First Static data for interpolation
                'Dim FrameIDs() As String = SlamStaticFrames(0).Split(",")
                'For k = 0 To FrameIDs.Count - 1
                'AddSegmentData(In_Data(FrameIDs(k)), SegmentRawData)
                'StartLoop += 1
                'Next
                ' GoTo StartPoint1
                '-------------------------------------------------------------
                'Add all data in the first frame
                For k = StartLoop To In_Data.Count - 1
                    If In_Data(k).TimeStamp < Val(Segmentdata(0)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        StartLoop += 1
                    Else
                        GoTo StartPoint1
                    End If

                Next
StartPoint1:
                '-------------------------------------------------------------
                StartLoop = Frame_Counter
                For k = StartLoop To In_Data.Count - 1
                    Frame_Counter = k
                    If In_Data(k).TimeStamp >= Val(Segmentdata(0)) Then
                        If In_Data(k).TimeStamp >= Val(Segmentdata(1)) Then
                            AddSegmentData(In_Data(k), SegmentRawData)
                            GoTo Point2Start
                        Else
                            AddSegmentData(In_Data(k), SegmentRawData)
                        End If
                    End If
                Next
            Else
                StartLoop = Frame_Counter
                For k = StartLoop To In_Data.Count - 1
                    Frame_Counter = k
                    If In_Data(k).TimeStamp >= Val(Segmentdata(0)) Then
                        If In_Data(k).TimeStamp >= Val(Segmentdata(1)) Then
                            AddSegmentData(In_Data(k), SegmentRawData)
                            GoTo Point2Start
                        Else
                            AddSegmentData(In_Data(k), SegmentRawData)
                        End If
                    End If
                Next
            End If
Point2Start:
            PointsCount_1 = SegmentRawData.Count
            MySegmentInfo = Frame_Counter.ToString & "," & SegmentRawData.Count.ToString
            SementsInfo.Add(MySegmentInfo)
            '---------------------------------------
            'Find Data point 2 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(2)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(3)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        GoTo Point3Start
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
Point3Start:
            PointsCount_2 = SegmentRawData.Count - PointsCount_1
            MySegmentInfo = Frame_Counter.ToString & "," & SegmentRawData.Count.ToString
            SementsInfo.Add(MySegmentInfo)
            '---------------------------------------
            'Find Data point 3 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(4)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(5)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        GoTo Point4Start
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
Point4Start:
            PointsCount_3 = SegmentRawData.Count - PointsCount_1 - PointsCount_2
            MySegmentInfo = Frame_Counter.ToString & "," & SegmentRawData.Count.ToString
            SementsInfo.Add(MySegmentInfo)
            '---------------------------------------
            'Find Data point 4 Start
            StartLoop = Frame_Counter
            For k = StartLoop To In_Data.Count - 1
                Frame_Counter = k
                If In_Data(k).TimeStamp >= Val(Segmentdata(6)) Then
                    If In_Data(k).TimeStamp >= Val(Segmentdata(7)) Then
                        AddSegmentData(In_Data(k), SegmentRawData)
                        If i = Segments.Count - 1 Then
                            'Add last static data
                            For kk = k To In_Data.Count - 1
                                If In_Data(kk).TimeStamp > Val(Segmentdata(7)) Then
                                    AddSegmentData(In_Data(kk), SegmentRawData)
                                End If

                            Next

                            'Dim LastFrameIDs() As String = SlamStaticFrames(SlamStaticFrames.Count - 1).Split(",")
                            'For kk = 0 To LastFrameIDs.Count - 1
                            'AddSegmentData(In_Data(LastFrameIDs(kk)), SegmentRawData)
                            'Next
                        End If
                        GoTo NextStartTransformation
                    Else
                        AddSegmentData(In_Data(k), SegmentRawData)
                    End If
                End If
            Next
            '---------------------------------------
NextStartTransformation:
            PointsCount_4 = SegmentRawData.Count - PointsCount_1 - PointsCount_2 - PointsCount_3
            MySegmentInfo = Frame_Counter.ToString & "," & SegmentRawData.Count.ToString
            SementsInfo.Add(MySegmentInfo)
            'MySegmentInfo = Frame_Counter.ToString & "," & SegmentRawData.Count.ToString
            'SementsInfo.Add(MySegmentInfo)

            ' MsgBox("PointsCount_i Total+1,2,3,4=" & i & "," & SegmentRawData.Count & "," & PointsCount_1 & "," & PointsCount_2 & "," & PointsCount_3 & "," & PointsCount_4)

            If SegmentRawData.Count < 5 Then
                ' MsgBox(SegmentRawData.Count)
                GoTo 11
            End If
            Frame_Counter = 0
            'Transform data
            AddMorePoints = False
            NoCommonPoints = 0
            ReDim ParamsXY(0, 0)
            ReDim ParamsZ(0, 0)
            CurrentTransformedPoint = New List(Of Trajectory_DataItem)
            Find_Trans_Parameters(RTK_InPoints, SegmentRawData, CurrentTransformedPoint, NoCommonPoints, AddMorePoints)

            If AddMorePoints = True Then
                GoTo 11
            End If
            For ii = 0 To PointsCount_1 - 1
                Trans_Trajectory.Add(CurrentTransformedPoint(ii))
            Next
11:
        Next
        ' Catch ex As Exception

        'End Try
    End Sub

    Private Sub Find_Trans_Parameters(ByVal RTK_InPoints As List(Of NMEA_DataItem), ByVal LiDAR_InPoints As List(Of Trajectory_DataItem), ByRef LiDAR_OutPoints As List(Of Trajectory_DataItem), ByRef NoCommonPoints As Integer, ByRef AddMorePoints As Boolean)

        Dim ParamsXY(3, 0) As Double
        Dim ParamsZ(3, 0) As Double
        '-------------------------------------
        '1- Find number of common points 
        ' Dim NoCommonPoints As Integer = 0
        Dim CommonPoint_IDs As List(Of String) = New List(Of String)
        For i = 0 To LiDAR_InPoints.Count - 1
            For j = 0 To RTK_InPoints.Count - 1
                If Val(RTK_InPoints(j).TimeStamp) = Val(LiDAR_InPoints(i).TimeStamp) Then
                    NoCommonPoints += 1
                    CommonPoint_IDs.Add(j & "," & i)
                End If
            Next
        Next
        ' MsgBox(NoCommonPoints & "," & LiDAR_InPoints)
        'MsgBox("NoCommonPoints=" & NoCommonPoints & " First common Point=" & CommonPoint_IDs(1))
        If NoCommonPoints < 4 Then
            AddMorePoints = True
            Exit Sub
        End If
        '-------------------------------------
        '2- Find Distances between First RTK point and other RTK Points & Find Max Distance
        Dim Distance As Double = 0
        Dim MaxDistance As Double = 0
        For i = 0 To RTK_InPoints.Count - 2
            Distance = Math.Sqrt(((RTK_InPoints(i).East - RTK_InPoints(i + 1).East) ^ 2) + (RTK_InPoints(i).North - RTK_InPoints(i + 1).North) ^ 2)
            If MaxDistance < Distance Then
                MaxDistance = Distance
            End If
        Next
        If MaxDistance < 1 Then
            AddMorePoints = True
            Exit Sub
        End If
        '-------------------------------------
        '3- Convert Points to Array of double
        Dim RTKArray_EN As List(Of Vector3d) = New List(Of Vector3d)
        Dim RTKArray_H As List(Of Vector3d) = New List(Of Vector3d)
        Dim LiDARArray_XY As List(Of Vector3d) = New List(Of Vector3d)
        Dim LiDARArray_Z As List(Of Vector3d) = New List(Of Vector3d)

        Dim Count2 As Integer = -1
        Dim ArrayCount As Integer = -1
        Dim inPoint As Vector3d = New Vector3d(0, 0, 0)
        For i = 0 To LiDAR_InPoints.Count - 1
            For j = 0 To RTK_InPoints.Count - 1
                If RTK_InPoints(j).TimeStamp = LiDAR_InPoints(i).TimeStamp Then
                    If j = 0 Then
                        Count2 += 1
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = RTK_InPoints(j).East
                        inPoint.Y = RTK_InPoints(j).North
                        inPoint.Z = RTK_InPoints(j).Height
                        RTKArray_EN.Add(inPoint)
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.Y = RTK_InPoints(j).Height
                        RTKArray_H.Add(inPoint)

                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = LiDAR_InPoints(i).East
                        inPoint.Y = LiDAR_InPoints(i).North
                        inPoint.Z = LiDAR_InPoints(i).Height
                        LiDARArray_XY.Add(inPoint)
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.Y = LiDAR_InPoints(i).Height
                        LiDARArray_Z.Add(inPoint)
                    Else
                        Distance = Math.Sqrt(((RTK_InPoints(j).East - RTK_InPoints(j - 1).East) ^ 2) + (RTK_InPoints(j).North - RTK_InPoints(j - 1).North) ^ 2)

                        Count2 += 1
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = RTK_InPoints(j).East
                        inPoint.Y = RTK_InPoints(j).North
                        inPoint.Z = RTK_InPoints(j).Height
                        RTKArray_EN.Add(inPoint)
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = Distance
                        inPoint.Y = RTK_InPoints(j).Height
                        RTKArray_H.Add(inPoint)

                        Distance = Math.Sqrt(((LiDAR_InPoints(i).East - LiDAR_InPoints(i - 1).East) ^ 2) + (LiDAR_InPoints(i).North - LiDAR_InPoints(i - 1).North) ^ 2)
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = LiDAR_InPoints(i).East
                        inPoint.Y = LiDAR_InPoints(i).North
                        inPoint.Z = LiDAR_InPoints(i).Height
                        LiDARArray_XY.Add(inPoint)
                        inPoint = New Vector3d(0, 0, 0)
                        inPoint.X = Distance
                        inPoint.Y = LiDAR_InPoints(i).Height
                        LiDARArray_Z.Add(inPoint)
                        'If Count2 = 100 Then GoTo 10
                    End If
                End If
            Next
        Next
10:

        If NoCommonPoints < 4 Then
            AddMorePoints = True
            Exit Sub
        End If
        '-------------------------------------
        '4- Perform 2D-Conformal transformation on X_Y data
        Trans2dLS(LiDARArray_XY, RTKArray_EN, ParamsXY)

        '-------------------------------------
        '5- Perform 2D-Conformal transformation on Dist_Z data
        Trans2dLS(LiDARArray_Z, RTKArray_H, ParamsZ)

        ' LiDAR_OutPoints
        '-------------------------------------
        '6- Calculate Transformed points
        Dim TransLiDARPointXY As Vector3d = New Vector3d(0, 0, 0)
        Dim TransformedPoint As Trajectory_DataItem = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
        For i = 0 To LiDAR_InPoints.Count - 1
            If i = 0 Then
                TransLiDARPointXY = New Vector3d(0, 0, 0)
                inPoint = New Vector3d(0, 0, 0)
                inPoint.X = LiDAR_InPoints(i).East
                inPoint.Y = LiDAR_InPoints(i).North
                inPoint.Z = LiDAR_InPoints(i).Height
                Transform_2d(inPoint, TransLiDARPointXY, ParamsXY)
                TransformedPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
                TransformedPoint.TimeStamp = LiDAR_InPoints(i).TimeStamp
                TransformedPoint.Rx = LiDAR_InPoints(i).Rx
                TransformedPoint.Ry = LiDAR_InPoints(i).Ry
                TransformedPoint.Rz = LiDAR_InPoints(i).Rz
                TransformedPoint.East = TransLiDARPointXY.X
                TransformedPoint.North = TransLiDARPointXY.Y
                inPoint = New Vector3d(0, 0, 0)
                inPoint.Y = LiDAR_InPoints(i).Height
                Transform_2d(inPoint, TransLiDARPointXY, ParamsZ)
                TransformedPoint.Height = TransLiDARPointXY.Y
                LiDAR_OutPoints.Add(TransformedPoint)
            Else
                Distance = Math.Sqrt(((LiDAR_InPoints(i).East - LiDAR_InPoints(0).East) ^ 2) + (LiDAR_InPoints(i).North - LiDAR_InPoints(0).North) ^ 2)
                TransLiDARPointXY = New Vector3d(0, 0, 0)
                inPoint = New Vector3d(0, 0, 0)
                inPoint.X = LiDAR_InPoints(i).East
                inPoint.Y = LiDAR_InPoints(i).North
                inPoint.Z = LiDAR_InPoints(i).Height
                Transform_2d(inPoint, TransLiDARPointXY, ParamsXY)
                TransformedPoint = New Trajectory_DataItem(0, 0, 0, 0, 0, 0, 0, 0)
                TransformedPoint.TimeStamp = LiDAR_InPoints(i).TimeStamp
                TransformedPoint.Rx = LiDAR_InPoints(i).Rx
                TransformedPoint.Ry = LiDAR_InPoints(i).Ry
                TransformedPoint.Rz = LiDAR_InPoints(i).Rz
                TransformedPoint.East = TransLiDARPointXY.X
                TransformedPoint.North = TransLiDARPointXY.Y
                inPoint = New Vector3d(0, 0, 0)
                inPoint.X = Distance
                inPoint.Y = LiDAR_InPoints(i).Height
                Transform_2d(inPoint, TransLiDARPointXY, ParamsZ)
                TransformedPoint.Height = TransLiDARPointXY.Y
                LiDAR_OutPoints.Add(TransformedPoint)
            End If
        Next

    End Sub

    Private Sub Trans2dLS(ByVal InputFileXY As List(Of Vector3d), ByVal InputFileEN As List(Of Vector3d), ByRef Params(,) As Double)
        ReDim Params(3, 0)
        '------------------------------------------------------------------------------
        'Note: InputFileEN are control points, InputFileXY are points to be transformed

        'Equations were taken from book: Adjustment Computations: Spatial Data Analysis, 4th Edition (pages.345-350)
        '-----------------------------------------------------------------------------------------------------------
        Dim a, b, Tx, Ty As Double ' a=S*cos Q, b=S*sin Q, S= Scale S=a/cos Q, Q=rotation angle Q=atan(b/a)
        Dim DataLine, DataLine2 As String
        Dim i, j, NoParams, NoObs As Integer
        Dim L_Matrix(,), A_Matrix(,), X_Matrix(,), V_Matrix(,) As Double
        NoObs = 0
        NoObs = InputFileXY.Count * 2 - 1
        ReDim L_Matrix(NoObs, 0)
        ReDim A_Matrix(NoObs, 3)
        Dim icount As Integer = 0

        For i = 0 To InputFileEN.Count - 1
            L_Matrix(icount, 0) = InputFileEN(i).X
            L_Matrix(icount + 1, 0) = InputFileEN(i).Y
            icount += 2
        Next

        icount = 0
        For i = 0 To InputFileXY.Count - 1
            'Form A matrix
            A_Matrix(icount, 0) = Val(InputFileXY(i).X)
            A_Matrix(icount, 1) = -1 * Val(InputFileXY(i).Y)
            A_Matrix(icount, 2) = 1
            A_Matrix(icount, 3) = 0
            A_Matrix(icount + 1, 0) = Val(InputFileXY(i).Y)
            A_Matrix(icount + 1, 1) = Val(InputFileXY(i).X)
            A_Matrix(icount + 1, 2) = 0
            A_Matrix(icount + 1, 3) = 1
            icount += 2
        Next
        '--------------------------------------------
        'Solution
        Dim At(,), AtA(,), AtAinv(,) As Double
        Dim AtB(,), AX(,) As Double
        '%%%%***********************************************************************
        '%%%Least Squares Solution
        '%1.Form the N (Normal) matrix
        'N = A '*W*A;
        At = MatLib.Transpose(A_Matrix)
        AtA = MatLib.Multiply(At, A_Matrix)
        AtB = MatLib.Multiply(At, L_Matrix)
        'MsgBox("i=" & UBound(AtWA, 1) & " j=" & UBound(AtWA, 2))
        AtAinv = MatLib.Inv(AtA) 'Q_Matrix

        '%2.Solve for X matrix (Corrections)
        'AWL = A '*W*OmC_Obs;
        'X=inv(N)*AWL;
        X_Matrix = MatLib.Multiply(AtAinv, AtB)
        Params(0, 0) = X_Matrix(0, 0)
        Params(1, 0) = X_Matrix(1, 0)
        Params(2, 0) = X_Matrix(2, 0)
        Params(3, 0) = X_Matrix(3, 0)
        NoParams = UBound(X_Matrix) + 1
        Dim Redandancy As Short = NoObs - NoParams
        'Residuals
        'V = A * X - OmC_Obs
        'Residuals
        'V = A * X - OmC_Obs
        AX = MatLib.Multiply(A_Matrix, X_Matrix)
        V_Matrix = MatLib.Subtract(AX, L_Matrix)

        'SoNew=sqrt(V'*W*V/(obs-pars)); %reference Std confidance=1 sigma(68.3% confidance)
        'Dim Vt(,) As Double = MatLib.Transpose(V_Matrix)
        'Dim VtV(,) As Double = MatLib.Multiply(Vt, V_Matrix)
        'Dim SoM(,) As Double = MatLib.ScalarDivide(Redandancy, VtV)
        'Dim So As Double = Math.Sqrt(SoM(0, 0))
        'Dim So2 As Double = So ^ 2

        'Dim C As Short = 0
        'Dim Q As Double = Math.Atan2(X_Matrix(1, 0), X_Matrix(0, 0))
        'If X_Matrix(1, 0) < 0 And X_Matrix(0, 0) < 0 Then
        'Q += 180
        'End If
        'Dim S As Double = Math.Sqrt(X_Matrix(0, 0) ^ 2 + X_Matrix(1, 0) ^ 2)
        'RoAngDMS = AngDec2DMS(Q)
        'ListBox1.Items.Add("***************************************")
        'ListBox1.Items.Add("Conformal Transformation 2D ")
        'ListBox1.Items.Add("***************************************")
        'ListBox1.Items.Add("Transformation Parameters: ")
        'ListBox1.Items.Add("Rotation = " & Q) ' & "˚ " & RoAngDMS.Y & "' " & Format(RoAngDMS.Z, "00.000") & """")
        'ListBox1.Items.Add("   Scale = " & Format(S, "0.0000000"))
        'ListBox1.Items.Add("      Tx = " & Format(X_Matrix(2, 0), "0.000"))
        'ListBox1.Items.Add("      Ty = " & Format(X_Matrix(3, 0), "0.000"))
        'ListBox1.Items.Add("")
        'ListBox1.Items.Add("Adjustment's Reference Variance = " & Format(So2, "0.0000"))
        'ListBox1.Items.Add("")
        'ListBox1.Items.Add("----------------------------------------------------------------------------------")
        'ListBox1.Items.Add("  Control Points  ")
        'ListBox1.Items.Add("----------------------------------------------------------------------------------")
        'ListBox1.Items.Add("Point ID         Easting         Northing       ")
        'ListBox1.Items.Add("----------------------------------------------------------------------------------")
        'Control Points Residuals
        'Dim ii As Integer = 0
        ' For i = 0 To NoObs - 1 Step 2
        'ListBox1.Items.Add(i & ", " & Format(L_Matrix(i, 0), "0.0000") & " ," & Format(L_Matrix(i + 1, 0), "0.0000") & "," &
        ' Format(V_Matrix(i, 0), "0.0000") & " ," & Format(V_Matrix(i + 1, 0), "0.0000"))
        'ii += 1
        ' Next
        'Calculate Transformed points
        'Dim InputXY As Vector3d = New Vector3d(0, 0, 0)
        'Dim OutputXY As Vector3d = New Vector3d(0, 0, 0)
        'ListBox1.Items.Add("----------------------------------------------------------------------------------")


        'For i = 0 To InputFileXY.Count - 1
        'InputXY = New Vector3d(InputFileXY(i).X, InputFileXY(i).Y, InputFileXY(i).Z)
        'Transform_2d(InputXY, OutputXY, Params)
        'ListBox1.Items.Add(i & ", " & Format(OutputXY.X, "0.0000") & " ," & Format(OutputXY.Y, "0.0000"))
        'Next


    End Sub

    Private Sub Transform_2d(ByRef InputXYZ As Vector3d, ByRef OutXYZ As Vector3d, ByVal Params(,) As Double)
        OutXYZ.X = InputXYZ.X * Params(0, 0) - InputXYZ.Y * Params(1, 0) + Params(2, 0)
        OutXYZ.Y = InputXYZ.Y * Params(0, 0) + InputXYZ.X * Params(1, 0) + Params(3, 0)
    End Sub



#Region "Test Helmert Transformation... Not working"
    Private Sub Find_Trans_Parameters_Helmert(ByVal RTK_InPoints As List(Of NMEA_DataItem), ByVal LiDAR_InPoints As List(Of Trajectory_DataItem), ByVal Params(,) As Double, ByRef Residuals() As Vector3d, ByRef NoCommonPoints As Integer, ByRef AddMorePoints As Boolean)
        '-------------------------------------
        '1- Find number of common points 
        ' Dim NoCommonPoints As Integer = 0
        Dim CommonPoint_IDs As List(Of String) = New List(Of String)
        For i = 0 To LiDAR_InPoints.Count - 1
            For j = 0 To RTK_InPoints.Count - 1
                If Val(RTK_InPoints(j).TimeStamp) = Val(LiDAR_InPoints(i).TimeStamp) Then
                    NoCommonPoints += 1
                    CommonPoint_IDs.Add(j & "," & i)
                End If
            Next
        Next

        'MsgBox("NoCommonPoints=" & NoCommonPoints & " First common Point=" & CommonPoint_IDs(1))
        If NoCommonPoints < 4 Then
            AddMorePoints = True
            Exit Sub
        End If
        '-------------------------------------
        '2- Find Distances between First RTK point and other RTK Points & Find Max Distance
        Dim Distance As Double = 0
        Dim MaxDistance As Double = 0
        For i = 0 To RTK_InPoints.Count - 2
            Distance = Math.Sqrt(((RTK_InPoints(i).East - RTK_InPoints(i + 1).East) ^ 2) + (RTK_InPoints(i).North - RTK_InPoints(i + 1).North) ^ 2)
            If MaxDistance < Distance Then
                MaxDistance = Distance
            End If
        Next
        ' MsgBox("MaxDistance=" & MaxDistance)
        If MaxDistance < 4 Then
            AddMorePoints = True
            Exit Sub
        End If
        '-------------------------------------
        '3- Convert Points to Array of double
        Dim RTKArray_EN As List(Of Vector3d) = New List(Of Vector3d)
        Dim RTKArray_H As List(Of Vector3d) = New List(Of Vector3d)
        Dim LiDARArray_XY As List(Of Vector3d) = New List(Of Vector3d)
        Dim LiDARArray_Z As List(Of Vector3d) = New List(Of Vector3d)

        Dim Count2 As Integer = -1
        Dim ArrayCount As Integer = -1
        Dim inPoint As Vector3d = New Vector3d(0, 0, 0)
        For i = 0 To LiDAR_InPoints.Count - 1
            For j = 0 To RTK_InPoints.Count - 1
                If RTK_InPoints(j).TimeStamp = LiDAR_InPoints(i).TimeStamp Then
                    Count2 += 1
                    inPoint = New Vector3d(0, 0, 0)
                    inPoint.X = RTK_InPoints(j).East
                    inPoint.Y = RTK_InPoints(j).North
                    inPoint.Z = RTK_InPoints(j).Height
                    RTKArray_EN.Add(inPoint)

                    inPoint = New Vector3d(0, 0, 0)
                    inPoint.X = LiDAR_InPoints(i).East
                    inPoint.Y = LiDAR_InPoints(i).North
                    inPoint.Z = LiDAR_InPoints(i).Height
                    LiDARArray_XY.Add(inPoint)
                End If
            Next
        Next
10:
        '-------------------------------------
        '5- Perform the 3D-Helmert transformation
        If RTK_InPoints.Count >= 4 Then
            'Call HelmertTrans(RTKArray_EN, LiDARArray_XY, Params, Residuals)
            Call HelmertTrans(LiDARArray_XY, RTKArray_EN, Params, Residuals)
        End If
        '--------------------------------------
        '6- Check with input RTK points
        Dim ParamsM(Params.Length - 1, 0) As Double
        For i = 3 To Params.Length - 1
            ParamsM(i, 0) = -1 * Params(i, 0)
        Next

        ' ListBox1.Items.Add("-------------------------------------------")
        ' ListBox1.Items.Add(" Helmert Transformation  TransformedPoint ")
        ' ListBox1.Items.Add("    X          Y          Z")
        ' ListBox1.Items.Add("-------------------------------------------")
        For i = 0 To LiDAR_InPoints.Count - 1
            Dim LiDARPoint As Vector3d = New Vector3d(0, 0, 0)
            LiDARPoint.X = LiDAR_InPoints(i).East
            LiDARPoint.X = LiDAR_InPoints(i).North
            LiDARPoint.X = LiDAR_InPoints(i).Height
            Dim TransformedPoint As Vector3d = New Vector3d(0, 0, 0)
            Transform3DPoint(LiDARPoint, TransformedPoint, ParamsM)
            ' ListBox1.Items.Add(TransformedPoint.X & "  ," & TransformedPoint.Y & " ," & TransformedPoint.Z)
        Next

    End Sub

    Private Sub Transform3DPoint(ByRef Inpoint As Vector3d, ByRef Outpoint As Vector3d, ByRef Params(,) As Double)

        Outpoint.Y = Params(1, 0) + Params(6, 0) * ((Math.Cos(Params(4, 0)) * Math.Sin(Params(5, 0))) * Inpoint.X + (Math.Sin(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Sin(Params(5, 0)) + Math.Cos(Params(3, 0)) * Math.Cos(Params(5, 0))) * Inpoint.Y + (Math.Cos(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Sin(Params(5, 0)) - Math.Sin(Params(3, 0)) * Math.Cos(Params(5, 0))) * Inpoint.Z)
        Outpoint.X = Params(0, 0) + Params(6, 0) * ((Math.Cos(Params(4, 0)) * Math.Cos(Params(5, 0))) * Inpoint.X + (Math.Sin(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Cos(Params(5, 0)) - Math.Cos(Params(3, 0)) * Math.Sin(Params(5, 0))) * Inpoint.Y + (Math.Cos(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Cos(Params(5, 0)) + Math.Sin(Params(3, 0)) * Math.Sin(Params(5, 0))) * Inpoint.Z)
        Outpoint.Z = Params(2, 0) + Params(6, 0) * ((-Math.Sin(Params(4, 0))) * Inpoint.X + (Math.Sin(Params(3, 0)) * Math.Cos(Params(4, 0))) * Inpoint.Y + (Math.Cos(Params(3, 0)) * Math.Cos(Params(4, 0))) * Inpoint.Z)

    End Sub

    Private Sub HelmertTrans(ByVal xyz1 As List(Of Vector3d), ByVal XYZ2 As List(Of Vector3d), ByRef Params(,) As Double, ByRef Residuals() As Vector3d)

        ' HERLMERT3D    overdetermined 3D similarity transformation
        '               Transformation type is origin-centered (i.e. earth-centered 
        '               or Bursa-Wolf with global cartesian coordinates)
        '
        ' [param, accur, resid] = helmert3D(datum1,datum2)
        '
        ' in:   datum1   n x 3 - matrix with coordinates in the origin datum (x y z)
        '       datum2   n x 3 - matrix with coordinates in the destination datum (x y z)
        ' out:  param    7 x 1 Parameter set of the 3D similarity transformation
        '                      3 translations (x y z) in [Unit of datums]
        '                      3 rotations (ex ey ez) in [rad]
        '                      1 scale factor
        '       accur    7 x 1 accuracy of the parameters
        '       resid    n x 3 - matrix with the residuals datum2-param(datum1)
        '
        ' 04/14/09 Peter Wasmeier - Technische Univerität München
        ' p.wasmeier@bv.tum.de

        '%% Argument checking
        Dim s1 As Integer = xyz1.Count - 1 ' UBound(xyz1)
        Dim s2 As Integer = XYZ2.Count - 1 ' UBound(XYZ2)
        If s1 <> s2 Then
            MsgBox("The datum sets are not of equal size")
            Exit Sub
        ElseIf s1 < 2 Or s2 < 2 Then
            MsgBox("At least one of the datum sets is not 3D")
            Exit Sub
        ElseIf s1 < 2 And s2 < 2 Then
            MsgBox("At least 3 points in each datum are necessary for calculating")
            Exit Sub
        End If

        '%% Adjustment

        Dim naeh() As Double = {0, 0, 0, 0, 0, 0, 1}
        Dim WertA() As Double = {0.000001, 0.000001}
        Dim zaehl As Integer = 0

        Dim x0 As Double = naeh(0)
        Dim y0 As Double = naeh(1)
        Dim z0 As Double = naeh(2)
        Dim ex As Double = naeh(3)
        Dim ey As Double = naeh(4)
        Dim ez As Double = naeh(5)
        Dim m As Double = naeh(6)
        ReDim Params(6, 0)
        Params(0, 0) = x0
        Params(1, 0) = y0
        Params(2, 0) = z0
        Params(3, 0) = ex
        Params(4, 0) = ey
        Params(5, 0) = ez
        Params(6, 0) = m

        Dim Qbb((s1 + 1) * 3 - 1, (s1 + 1) * 3 - 1), A((s1 + 1) * 3 - 1, 6), w((s1 + 1) * 3 - 1, 0) As Double
        For i = 0 To UBound(Qbb)
            Qbb(i, i) = 1
        Next

        Do
            For i = 0 To s1
                A(i * 3, 0) = -1
                A(i * 3 + 1, 1) = -1
                A(i * 3 + 2, 2) = -1
                A(i * 3, 3) = -m * ((Math.Cos(ex) * Math.Sin(ey) * Math.Cos(ez) + Math.Sin(ex) * Math.Sin(ez)) * xyz1(i).Y + (-Math.Sin(ex) * Math.Sin(ey) * Math.Cos(ez) + Math.Cos(ex) * Math.Sin(ey)) * xyz1(i).Z)
                A(i * 3, 4) = -m * ((-Math.Sin(ey) * Math.Cos(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Cos(ey) * Math.Cos(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Cos(ey) * Math.Cos(ez)) * xyz1(i).Z)
                A(i * 3, 5) = -m * ((-Math.Cos(ey) * Math.Sin(ez)) * xyz1(i).X + (-Math.Sin(ex) * Math.Sin(ey) * Math.Sin(ez) - Math.Cos(ex) * Math.Cos(ez)) * xyz1(i).Y + (-Math.Cos(ex) * Math.Sin(ey) * Math.Sin(ez) + Math.Sin(ex) * Math.Cos(ex)) * xyz1(i).Z)
                A(i * 3, 6) = -((Math.Cos(ey) * Math.Cos(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Sin(ey) * Math.Cos(ez) - Math.Cos(ex) * Math.Sin(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Sin(ey) * Math.Cos(ez) + Math.Sin(ex) * Math.Sin(ez)) * xyz1(i).Z)

                A(i * 3 + 1, 3) = -m * ((Math.Cos(ex) * Math.Sin(ey) * Math.Sin(ez) - Math.Sin(ex) * Math.Cos(ez)) * xyz1(i).Y + (-Math.Sin(ex) * Math.Sin(ey) * Math.Sin(ez) - Math.Cos(ex) * Math.Cos(ez)) * xyz1(i).Z)
                A(i * 3 + 1, 4) = -m * ((-Math.Sin(ey) * Math.Sin(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Cos(ey) * Math.Sin(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Cos(ey) * Math.Sin(ez)) * xyz1(i).Z)
                A(i * 3 + 1, 5) = -m * ((Math.Cos(ey) * Math.Cos(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Sin(ey) * Math.Cos(ez) - Math.Cos(ex) * Math.Sin(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Sin(ey) * Math.Cos(ez) - Math.Sin(ex) * Math.Sin(ez)) * xyz1(i).Z)
                A(i * 3 + 1, 6) = -((Math.Cos(ey) * Math.Sin(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Sin(ey) * Math.Sin(ez) + Math.Cos(ex) * Math.Cos(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Sin(ey) * Math.Sin(ez) - Math.Sin(ex) * Math.Cos(ez)) * xyz1(i).Z)

                A(i * 3 + 2, 3) = -m * ((Math.Cos(ex) * Math.Cos(ey)) * xyz1(i).Y + (-Math.Sin(ex) * Math.Cos(ey)) * xyz1(i).Z)
                A(i * 3 + 2, 4) = -m * ((-Math.Cos(ey)) * xyz1(i).X + (-Math.Sin(ex) * Math.Sin(ey)) * xyz1(i).Y + (-Math.Cos(ex) * Math.Sin(ey)) * xyz1(i).Z)
                A(i * 3 + 2, 5) = 0
                A(i * 3 + 2, 6) = -((-Math.Sin(ey)) * xyz1(i).X + (Math.Sin(ex) * Math.Cos(ey)) * xyz1(i).Y + (Math.Cos(ex) * Math.Cos(ey)) * xyz1(i).Z)

                w(i * 3, 0) = XYZ2(i).X - x0 - m * ((Math.Cos(ey) * Math.Cos(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Sin(ey) * Math.Cos(ez) - Math.Cos(ex) * Math.Sin(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Sin(ey) * Math.Cos(ez) + Math.Sin(ex) * Math.Sin(ez)) * xyz1(i).Z)
                w(i * 3 + 1, 0) = XYZ2(i).Y - y0 - m * ((Math.Cos(ey) * Math.Sin(ez)) * xyz1(i).X + (Math.Sin(ex) * Math.Sin(ey) * Math.Sin(ez) + Math.Cos(ex) * Math.Cos(ez)) * xyz1(i).Y + (Math.Cos(ex) * Math.Sin(ey) * Math.Sin(ez) - Math.Sin(ex) * Math.Cos(ez)) * xyz1(i).Z)
                w(i * 3 + 2, 0) = XYZ2(i).Z - z0 - m * ((-Math.Sin(ey)) * xyz1(i).X + (Math.Sin(ex) * Math.Cos(ey)) * xyz1(i).Y + (Math.Cos(ex) * Math.Cos(ey)) * xyz1(i).Z)
            Next

            'MsgBox(MatLib.PrintMat(A))

            w = MatLib.ScalarMultiply(-1, w)
            Dim r As Integer = UBound(A, 1) - UBound(A, 2)
            Dim Pbb(,) As Double = MatLib.Inv(Qbb)
            Dim At(,) As Double = MatLib.Transpose(A)
            Dim AtW(,) As Double = MatLib.Multiply(At, Pbb)
            Dim AtWA(,) As Double = MatLib.Multiply(AtW, A)
            Dim Qxxda(,) As Double = MatLib.Inv(AtWA)
            Dim AtWB(,) As Double = MatLib.Multiply(AtW, w)
            Dim deltax(,) As Double = MatLib.Multiply(Qxxda, AtWB)
            Dim Ax(,) As Double = MatLib.Multiply(A, deltax)
            Dim v(,) As Double = MatLib.Subtract(Ax, w)
            Dim VtW(,) As Double = MatLib.Multiply(MatLib.Transpose(v), Pbb)
            Dim VtWV(,) As Double = MatLib.Multiply(VtW, v)
            Dim Sig0p As Double = Math.Sqrt(VtWV(0, 0) / r)

            Dim Kxxda(,) As Double = MatLib.ScalarMultiply(Sig0p ^ 2, Qxxda)
            Dim ac(UBound(Kxxda), 0) As Double
            For i = 0 To UBound(Kxxda)
                ac(i, 0) = Math.Sqrt(Kxxda(i, i))
            Next

            Dim testv As Double = Math.Sqrt((deltax(0, 0) ^ 2 + deltax(1, 0) ^ 2 + deltax(2, 0) ^ 2) / 3)
            Dim testd As Double = Math.Sqrt((deltax(3, 0) ^ 2 + deltax(4, 0) ^ 2 + deltax(5, 0) ^ 2) / 3)
            zaehl = zaehl + 1

            x0 = x0 + deltax(0, 0)
            y0 = y0 + deltax(1, 0)
            z0 = z0 + deltax(2, 0)
            ex = ex + deltax(3, 0)
            ey = ey + deltax(4, 0)
            ez = ez + deltax(5, 0)
            m = m + deltax(6, 0)

            Params(0, 0) = x0
            Params(1, 0) = y0
            Params(2, 0) = z0
            Params(3, 0) = ex
            Params(4, 0) = ey
            Params(5, 0) = ez
            Params(6, 0) = m

            If testv < WertA(0) And testd < WertA(1) Then
                GoTo 100
            ElseIf zaehl > 100 Then
                MsgBox("Helmert3D:Too_many_iterations, Calculation not converging after 100 iterations. I am aborting. Results may be inaccurate.")
                GoTo 100
            End If

        Loop
100:
        '%% Transformation residuals
        Dim idz(s1) As Vector3d
        ReDim Residuals(s1)
        'ListBox1.Items.Add("-------------------------------------------")
        'ListBox1.Items.Add(" Helmert Transformation  Residuals ")
        'ListBox1.Items.Add("-------------------------------------------")
        For i = 0 To s1
            idz(i).Y = Params(1, 0) + Params(6, 0) * ((Math.Cos(Params(4, 0)) * Math.Sin(Params(5, 0))) * xyz1(i).X + (Math.Sin(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Sin(Params(5, 0)) + Math.Cos(Params(3, 0)) * Math.Cos(Params(5, 0))) * xyz1(i).Y + (Math.Cos(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Sin(Params(5, 0)) - Math.Sin(Params(3, 0)) * Math.Cos(Params(5, 0))) * xyz1(i).Z)
            idz(i).X = Params(0, 0) + Params(6, 0) * ((Math.Cos(Params(4, 0)) * Math.Cos(Params(5, 0))) * xyz1(i).X + (Math.Sin(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Cos(Params(5, 0)) - Math.Cos(Params(3, 0)) * Math.Sin(Params(5, 0))) * xyz1(i).Y + (Math.Cos(Params(3, 0)) * Math.Sin(Params(4, 0)) * Math.Cos(Params(5, 0)) + Math.Sin(Params(3, 0)) * Math.Sin(Params(5, 0))) * xyz1(i).Z)
            idz(i).Z = Params(2, 0) + Params(6, 0) * ((-Math.Sin(Params(4, 0))) * xyz1(i).X + (Math.Sin(Params(3, 0)) * Math.Cos(Params(4, 0))) * xyz1(i).Y + (Math.Cos(Params(3, 0)) * Math.Cos(Params(4, 0))) * xyz1(i).Z)

            'ListBox1.Items.Add("Calculated X=" & idz(i).X & "Calculated Y=" & idz(i).Y & "Calculated Z=" & idz(i).Z)

            Residuals(i).X = XYZ2(i).X - idz(i).X
            Residuals(i).Y = XYZ2(i).Y - idz(i).Y
            Residuals(i).Z = XYZ2(i).Z - idz(i).Z

        Next

        'Convert rotation angles to seconds
        ' Params(3, 0) = (Params(3, 0) * 180 / Math.PI) * 3600
        'Params(4, 0) = (Params(4, 0) * 180 / Math.PI) * 3600
        ' Params(5, 0) = (Params(5, 0) * 180 / Math.PI) * 3600
        'Convert scale to ppm
        ' Params(6, 0) = ((1 - Params(6, 0)) * 1000000)

    End Sub

#End Region

#End Region

#Region "Export"
    Private Sub RibbonButton_Export_Click(sender As Object, e As EventArgs) Handles RibbonButton_Export.Click
        Dim PcapFileName_without_extention As String = System.IO.Path.GetFileNameWithoutExtension(PcapFileName)
        Dim PcapFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\PCD_Files\"
        If Not System.IO.Directory.Exists(PcapFileName_Path) Then
            System.IO.Directory.CreateDirectory(PcapFileName_Path)
        End If
        Dim TrajFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\Trajectory_Files\"
        If Not System.IO.Directory.Exists(TrajFileName_Path) Then
            System.IO.Directory.CreateDirectory(TrajFileName_Path)
        End If

        Dim RTKFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\Static_Files\"
        If Not System.IO.Directory.Exists(RTKFileName_Path) Then
            System.IO.Directory.CreateDirectory(RTKFileName_Path)
        End If

        'Save RTK Interpolated Trajectory Data to file
        Application.DoEvents()
        Dim LiDAR_Traj_File As String = TrajFileName_Path & "Trajectory.txt"
        Export_LiDAR_Traj(LiDAR_Traj_File)

        'Save RTK Interpolated Traj Data + Slam Traj data (TrajectoryDataPointsAll)  to file
        Application.DoEvents()
        Dim Traj_All_Data As String = TrajFileName_Path & "Traj_All_Data.txt"
        Export_Traj_All_Data(Traj_All_Data)

        Application.DoEvents()
        Dim RTK_Traj_File As String = RTKFileName_Path & "RTK_Data.txt"
        Export_RTK_Traj(RTK_Traj_File)

        'Save RTK static data to file
        Dim StaticDataFile_Name As String = RTKFileName_Path & "RTK_StaticData.txt"
        ExportRTK_Static_Data(StaticDataFile_Name)

        'Save RTK Sync static data to file
        Dim StaticSyncDataFile_Name As String = RTKFileName_Path & "RTK_StaticSyncData.txt"
        ExportRTK_Static_Sync_Data(StaticSyncDataFile_Name)

        'Save Slam smoothed data to file
        Application.DoEvents()
        Dim Slam_Smoothed_File As String = TrajFileName_Path & "Smoothed_Trajectory.txt"
        Export_Slam_Smoothed_data(Slam_Smoothed_File)

        'Save Slam sync smoothed data to file
        Application.DoEvents()
        Dim Slam_Sync_Smoothed_File As String = TrajFileName_Path & "Smoothed_Sync_Trajectory.txt"
        Export_Traj_Sync_Smoothed_Data(Slam_Sync_Smoothed_File)

        'Save Slam sync smoothed data All to file
        Application.DoEvents()
        Dim Slam_Sync_Smoothed_AllFile As String = TrajFileName_Path & "Smoothed_Sync_All_Trajectory.txt"
        Export_Traj_Sync_Smoothed_DataAll(Slam_Sync_Smoothed_AllFile)

        'Save Slam sync Transformed data All to file
        Application.DoEvents()
        Dim Slam_Sync_Trans_AllFile As String = TrajFileName_Path & "Transformed_Sync_All_Trajectory.txt"
        Export_Traj_Sync_Trans_DataAll(Slam_Sync_Trans_AllFile)

        Exit Sub
        For Each myItem As Trajectory_DataItem In TrajectoryDataPoints
            Application.DoEvents()
            Dim PCD_File As String = PcapFileName_Path & "Frame_" & myItem.ID & ".pcd"

            Current_Frame = myItem.ID
            RibbonTextBox_Frame.TextBoxText = Current_Frame

            Export_PCD(PCD_File, Current_Frame)
        Next

    End Sub

    Private Sub Export_Slam_Smoothed_data(ByVal Slam_Smoothed_File As String)
        'Save Slam Smoothed data to file
        Application.DoEvents()
        Try
            Dim myWriter As New IO.StreamWriter(Slam_Smoothed_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectorySmoothedDataPointsAll
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Trajectory All Data File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Trajectory All Data File !!!"
        End Try

    End Sub

    Private Sub Export_Slam_with_Static_data()
        'Save Slam Static data to file
        Dim StaticFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\Trajectory_Files\"
        If Not System.IO.Directory.Exists(StaticFileName_Path) Then
            System.IO.Directory.CreateDirectory(StaticFileName_Path)
        End If
        Try
            Dim StaticDataFile_Name As String = StaticFileName_Path & "SlamTrajDataPoints_ModifiedStatic.txt"
            Dim myWriter As New IO.StreamWriter(StaticDataFile_Name)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In SlamTrajDataPoints_ModifiedStatic
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Slam Static data to a file: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Slam Static data to a file !!!"
        End Try

    End Sub

    Private Sub ExportRTK_Static_Sync_Data(ByVal StaticSyncDataFile_Name As String)
        'Save RTK static data to file
        Try
            Dim myWriterStatic As New IO.StreamWriter(StaticSyncDataFile_Name)
            myWriterStatic.WriteLine("GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp_From,TimeStamp_To,Speed,Distance,Heading,Azimuth")

            For Each myItem As String In StaticSyncGPSData
                myWriterStatic.WriteLine(myItem)
            Next
            myWriterStatic.Close()
            StatusLabel.Text = "Status: Export RTK Static data to a file: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export RTK Static data to a file !!!"
        End Try
    End Sub

    Private Sub ExportRTK_Static_Data(ByVal StaticDataFile_Name As String)
        'Save RTK static data to file
        Try
            Dim myWriterStatic As New IO.StreamWriter(StaticDataFile_Name)
            myWriterStatic.WriteLine("GPSTimeFrom,GPSTimeTo,East,North,Height,TimeStamp_From,TimeStamp_To,Speed,Distance,Heading,Azimuth")

            For Each myItem As String In StaticGPSData
                myWriterStatic.WriteLine(myItem)
            Next
            myWriterStatic.Close()
            StatusLabel.Text = "Status: Export RTK Static data to a file: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export RTK Static data to a file !!!"
        End Try
    End Sub

    Private Sub Export_Traj_Sync_Smoothed_Data(ByVal Traj_Sync_Smoothed_Data_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(Traj_Sync_Smoothed_Data_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectorySyncSmoothedDataPoints
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Trajectory Synchronized Smoothed Data File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Trajectory Synchronized Smoothed Data File !!!"
        End Try
    End Sub

    Private Sub Export_Traj_Sync_Smoothed_DataAll(ByVal Traj_Sync_Smoothed_DataAll_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(Traj_Sync_Smoothed_DataAll_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectorySyncSmoothedDataPointsAll
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Trajectory Synchronized Smoothed Data File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Trajectory Synchronized Smoothed Data File !!!"
        End Try
    End Sub

    Private Sub Export_Traj_All_Data(ByVal Traj_All_Data_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(Traj_All_Data_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectoryDataPointsAll
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Trajectory All Data File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Trajectory All Data File !!!"
        End Try
    End Sub

    Private Sub Export_Traj_Sync_Trans_DataAll(ByVal Traj_Trans_Data_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(Traj_Trans_Data_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectoryTransDataPoints
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export Trajectory Transformed All Data File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export Trajectory Transformed All Data File !!!"
        End Try
    End Sub

    Private Sub Export_LiDAR_Traj(ByVal LiDAR_Traj_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(LiDAR_Traj_File)
            myWriter.WriteLine("ID,TimeStamp,Rx,Ry,Rz,East,North,Height")
            For Each myItem As Trajectory_DataItem In TrajectoryDataPoints
                myWriter.WriteLine(myItem.ID & "," & myItem.TimeStamp & "," & myItem.Rx & "," & myItem.Ry & "," & myItem.Rz & "," & myItem.East & "," & myItem.North & "," & myItem.Height)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export LiDAR Trajectory File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export LiDAR Trajectory File !!!"
        End Try
    End Sub

    Private Sub Export_RTK_Traj(ByVal RTK_Traj_File As String)
        Try
            Dim myWriter As New IO.StreamWriter(RTK_Traj_File)
            myWriter.WriteLine("GPSTime,East,North,Height,TimeStamp,Speed,Distance,Heading,Azimuth")
            For Each myItem As NMEA_DataItem In RTK_DataPoints
                myWriter.WriteLine(myItem.GPSTime & "," & myItem.East & "," & myItem.North & "," & myItem.Height & "," & myItem.TimeStamp & "," & myItem.Speed & "," & myItem.Distance & "," & myItem.Heading & "," & myItem.Azimuth)
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export RTK Trajectory File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export RTK Trajectory File !!!"
        End Try
    End Sub

    Private Sub Export_PCD(ByVal PCD_Files As String, ByVal Frame_ID As Integer)
        Try

            Extract_LiDAR_Data(Frame_ID)

            Dim myWriter As New IO.StreamWriter(PCD_Files)
            myWriter.WriteLine("# .PCD v0.7 - Point Cloud Data file format")
            myWriter.WriteLine("Version 0.7")
            myWriter.WriteLine("FIELDS x y z intensity laser_id azimuth distance_m adjustedtime timestamp vertical_angle")
            myWriter.WriteLine("Size 4 4 4 1 1 2 8 8 4 8")
            myWriter.WriteLine("Type F F F U U U F U U F")
            myWriter.WriteLine("COUNT 1 1 1 1 1 1 1 1 1 1")
            myWriter.WriteLine("Width " & LiDARDataPoints.Count)
            myWriter.WriteLine("Height 1")
            myWriter.WriteLine("VIEWPOINT 0 0 0 1 0 0 0")
            myWriter.WriteLine("POINTS " & LiDARDataPoints.Count)
            myWriter.WriteLine("Data ascii")

            For Each myItem As LiDAR_DataItem In LiDARDataPoints
                myWriter.WriteLine(Math.Round(myItem.X, 8) & " " & Math.Round(myItem.Y, 8) & " " & Math.Round(myItem.Z, 8) & " " &
                                   myItem.Intensity & " " & myItem.LaserID & " " & Math.Round(myItem.Azimuth, 8) & " " &
                                   Math.Round(myItem.Distance, 8) & " " & myItem.TimeStamp & " " & myItem.TimeStamp & " " &
                                   Math.Round(myItem.VerticalAngle, 8))
            Next
            myWriter.Close()
            StatusLabel.Text = "Status: Export LiDAR PCD File: Completed"
        Catch ex As Exception
            StatusLabel.Text = "Status: Error in Export LiDAR PCD File !!!"
        End Try
    End Sub

#End Region

#Region "Load Obj file"
    Public Sub LoadModelFromFile(ByVal fileName As String)
        Dim myModel As Model = New Model(fileName)
        ShowModel(myModel)
    End Sub

    Public Sub ShowModel(ByVal myModel As Model)
        OpenGLUC1.RemoveAllModels()
        OpenGLUC1.ShowModel(myModel)
    End Sub
#End Region

#Region "Matching"

    ' OpenTK.GLControl
    Dim OGLControl As New GLControl

    Public Shared Function RotationAngles(ByVal r As Matrix4d) As Vector3d
        Dim thetaX, thetaY, thetaZ As Double
        If r(0, 2) < 1 Then
            If r(0, 2) > -1 Then
                thetaY = Convert.ToSingle(Math.Asin(r(0, 2)))
                thetaX = Convert.ToSingle(Math.Atan2(-r(1, 2), r(2, 2)))
                thetaZ = Convert.ToSingle(Math.Atan2(-r(0, 1), r(0, 0))) ' r 0 2 = −1
            Else
                ' Not a u n i q u e s o l u t i o n : thetaZ −thetaaX = Math.Atan2( r10 , r 1 1 )
                thetaY = -Convert.ToSingle(Math.PI / 2)
                thetaX = -Convert.ToSingle(Math.Atan2(r(1, 0), r(1, 1)))
                thetaZ = 0
            End If ' r 0 2 = +1
        Else
            ' Not a u n i q u e s o l u t i o n : thetaZ +thetaaX = Math.Atan2( r10 , r 1 1 )
            thetaY = Convert.ToSingle(Math.PI / 2)
            thetaX = Convert.ToSingle(Math.Atan2(r(1, 0), r(1, 1)))
            thetaZ = 0
        End If
        Dim v As Vector3d = New Vector3d(thetaX, thetaY, thetaZ)

        For i = 0 To 2
            v(i) = Math.Abs(v(i))
            If v(i) > 2 Then v(i) -= 2
            If v(i) > 1 Then v(i) -= 1

        Next

        Return v

    End Function

    Public Shared Function PutTheMatrix4dtogether(ByVal mat4d As Matrix4d, ByVal T As Vector3d, ByVal Rotation As Matrix3d) As Matrix4d

        'put the 4d matrix together
        Dim r3D As Matrix3d = Rotation.Clone()
        Dim myMatrix As Matrix4d = New Matrix4d(r3D)
        myMatrix(0, 3) = T.X
        myMatrix(1, 3) = T.Y
        myMatrix(2, 3) = T.Z
        myMatrix(3, 3) = 1.0F

        Return myMatrix

    End Function

    Private Sub ReadAllFiles(ByVal MyDirectory As String)
        Dim files() As String = IO.Directory.GetFiles(MyDirectory)
        Dim FileCount As Integer = 0
        For Each file As String In files
            ' Do work, example
            'Dim text As String = IO.File.ReadAllText(file)
            FileCount += 1
        Next
        MsgBox(FileCount)
    End Sub
    Private Sub SetSelectedFolder()
        Dim objFolderDlg As System.Windows.Forms.FolderBrowserDialog
        objFolderDlg = New System.Windows.Forms.FolderBrowserDialog
        'objFolderDlg.SelectedPath = "C:\Test"
        If objFolderDlg.ShowDialog() = DialogResult.OK Then
            MessageBox.Show(objFolderDlg.SelectedPath)
            ReadAllFiles(objFolderDlg.SelectedPath)
        End If
    End Sub

    Private Sub RibbonButton_Test_Match_Click(sender As Object, e As EventArgs) Handles RibbonButton_Test_Match.Click

        ' SetSelectedFolder()
        ' Exit Sub

        Me.Cursor = Cursors.WaitCursor
        LoadTwoClouds()
        DisplayObjects()

        Clouds_ICP()
        Me.Cursor = Cursors.Default

        '-----------------------------------
        'Clear original Models
        OpenGLUC1.RemoveAllModels()
        OpenGLUC1.Refresh()
        '-----------------------------------
        'Show Result Model
        LoadModelFromFile(ResultModel_FullName)

    End Sub

    Private registrationMatrix As Matrix4d
    Private pSource As PointCloud
    Private pTarget As PointCloud
    Private pResult As PointCloud

    Private pointCloudFirstAfterLoad As PointCloud
    Dim RegMatrix_Count As Integer = 0
    Dim RegMatrix_Name As String = Nothing
    Dim ResultModel_Name As String = Nothing
    Dim ResultModel_FullName As String = Nothing

    Private Sub LoadTwoClouds()

        ' Dim fileName1 As String = GLSettings.FileNamePointCloudLast1
        ' Dim fileName2 As String = GLSettings.FileNamePointCloudLast2
        Dim fileName1 As String = "D:\Abadallah_Palestine_Tests\Test_1_Noisy\Cleaned_Data\Obj_All_Files\2022-12-17_102051 (Frame 900 to 2920)_frame_0.obj"
        Dim fileName2 As String = "D:\Abadallah_Palestine_Tests\Test_1_Noisy\Cleaned_Data\Obj_All_Files\2022-12-17_102051 (Frame 900 to 2920)_frame_2.obj"

        ' Dim ObjFileName_without_extention As String = System.IO.Path.GetFileNameWithoutExtension(PcapFileName)
        ' Dim ObjFileName_Path As String = System.IO.Path.GetDirectoryName(PcapFileName) & "\Obj_Files\"

        GLSettings.PathModels = System.IO.Path.GetDirectoryName(fileName1)
        Dim FileName_without_extention As String = System.IO.Path.GetFileNameWithoutExtension(fileName1)
        Dim fileName11 As String = FileName_without_extention

        'tabControlImages.SelectTab(0);
        'Dim myModel1 As Model = New Model(GLSettings.PathModels, fileName11)
        Dim myModel1 As Model = New Model(fileName1)

        pSource = myModel1.PointCloud
        pointCloudFirstAfterLoad = pSource.Clone()
        pSource.SetColor(New OpenTK.Vector3d(0, 1, 0))

        pSource.FileNameShort = GLSettings.FileNamePointCloudLast1
        pSource.Path = GLSettings.PathModels
        pSource.FileNameLong = pSource.Path & "\" + pSource.FileNameShort

        FileName_without_extention = System.IO.Path.GetFileNameWithoutExtension(fileName2)
        Dim fileName22 As String = FileName_without_extention

        'Dim myModel2 As Model = New Model(GLSettings.PathModels, fileName22)
        Dim myModel2 As Model = New Model(fileName2)

        pTarget = myModel2.PointCloud

        pTarget.SetColor(New OpenTK.Vector3d(1, 0, 0))
        pTarget.FileNameShort = GLSettings.FileNamePointCloudLast2
        pTarget.Path = GLSettings.PathModels
        pTarget.FileNameLong = pTarget.Path & "\" + pTarget.FileNameShort

    End Sub

    Private Sub Clouds_ICP()
        RegMatrix_Count += 1
        ' If Not GetFirstTwoCloudsFromOpenGLControl() Then Return

        '------------------
        'ICP
        Dim icpInstance As ICPLib.IterativeClosestPointTransform = New ICPLib.IterativeClosestPointTransform()
        icpInstance.Settings_Reset_RealData()

        icpInstance.ICPSettings.ICPVersion = ICP_VersionUsed.Scaling_Zinsser
        icpInstance.ICPSettings.MaximumNumberOfIterations = 50
        'icpInstance.ICPSettings.KDTreeMode = KDTreeMode.Rednaxela_ExcludePoints;
        icpInstance.ICPSettings.KDTreeMode = KDTreeMode.Rednaxela


        Dim pointCloudResult As PointCloudVertices = icpInstance.PerformICP(Me.pSource.ToPointCloudVertices(), Me.pTarget.ToPointCloudVertices())

        pointCloudResult.AddPointCloud(Me.pTarget.ToPointCloudVertices())
        SaveResultCloudAndShow(pointCloudResult)


        Me.registrationMatrix = icpInstance.Matrix

        Dim MatrixFileName_Path As String = System.IO.Path.GetDirectoryName(GLSettings.PathModels) & "\Matrix\"
        If Not System.IO.Directory.Exists(MatrixFileName_Path) Then
            System.IO.Directory.CreateDirectory(MatrixFileName_Path)
        End If

        'registrationMatrix.Save(GLSettings.PathModels, "registrationMatrix.txt")

        RegMatrix_Name = "RegMatrix_" & RegMatrix_Count & ".txt"
        registrationMatrix.Save(MatrixFileName_Path, RegMatrix_Name)

        Dim Rotation_Angles As Vector3d = RotationAngles(registrationMatrix)

        Dim Translation_X As Double = registrationMatrix(0, 3)
        Dim Translation_Y As Double = registrationMatrix(1, 3)
        Dim Translation_Z As Double = registrationMatrix(2, 3)
        MsgBox(" Rotation_AnglesX=" & Rotation_Angles(0) & " Rotation_AnglesY=" & Rotation_Angles(1) & " Rotation_AnglesZ=" & Rotation_Angles(2) & " Translation_X=" & Translation_X & " Translation_Y=" & Translation_Y & " Translation_Z=" & Translation_Z)
        ' MsgBox(" Rotation_AnglesX=" & Rotation_AnglesX & " Rotation_Angles(0)=" & Rotation_Angles(0) & " Rotation_AnglesY=" & Rotation_AnglesY & " Rotation_Angles(1)=" & Rotation_Angles(1) & " Rotation_AnglesZ=" & Rotation_AnglesZ & " Rotation_Angles(2)=" & Rotation_Angles(2))


    End Sub
    Private Sub SaveResultCloudAndShow(ByVal pointCloudResult As PointCloudVertices)
        '-------------------------------------
        'save
        If pointCloudResult IsNot Nothing Then

            Dim path = String.Empty
            Dim fileNameShort = String.Empty
            IOUtils.ExtractDirectoryAndNameFromFileName(pSource.FileNameLong, fileNameShort, path)

            Dim ResultsFileName_Path As String = System.IO.Path.GetDirectoryName(GLSettings.PathModels) & "\Results\"
            If Not System.IO.Directory.Exists(ResultsFileName_Path) Then
                System.IO.Directory.CreateDirectory(ResultsFileName_Path)
            End If
            ResultModel_Name = "Result_" & RegMatrix_Count & ".obj"
            ResultModel_FullName = ResultsFileName_Path & ResultModel_Name
            'pointCloudResult.Save(pSource.Path, "Result.obj")
            pointCloudResult.Save(ResultsFileName_Path, ResultModel_Name)
            pResult = pointCloudResult.ToPointCloud()
            'pResult.SetColor(new OpenTK.Vector3d(1, 0, 0));

            DisplayResultPointCloud()


        End If

    End Sub

    Private Sub DisplayObjects()

        OpenGLUC1.OGLControl.GLrender.ClearAllObjects()

        OpenGLUC1.ShowPointCloud(pSource)
        OpenGLUC1.ShowPointCloud(pTarget)

    End Sub
    Private Sub DisplayResultPointCloud()
        If pResult IsNot Nothing Then
            Dim pcr As RenderableObject = New PointCloudRenderable()
            pcr.PointCloud = pResult
            OpenGLUC1.OGLControl.GLrender.AddRenderableObject(pcr)
        End If

    End Sub
    Private Function GetFirstTwoCloudsFromOpenGLControl() As Boolean
        If OpenGLUC1.OGLControl.GLrender.RenderableObjects.Count < 2 Then Return False

        pSource = OpenGLUC1.OGLControl.GLrender.RenderableObjects(0).PointCloud
        pTarget = OpenGLUC1.OGLControl.GLrender.RenderableObjects(1).PointCloud

        Return True
    End Function
















    Private Sub Test_Pointmatcher()
        '-----------------------------------------------------------------------
        'Source:
        'https://github.com/braddodson/pointmatcher.net
        'Pointmatcher.net
        'DataPoints reading = ...; // initialize your point cloud reading here
        'DataPoints reference = ...; // initialize your reference point cloud here
        'EuclideanTransform initialTransform = ...; // your initial guess at the transform from reading to reference
        'ICP icp = New ICP();
        'icp.ReadingDataPointsFilters = New RandomSamplingDataPointsFilter(prob:  0.1f);
        'icp.ReferenceDataPointsFilters = New SamplingSurfaceNormalDataPointsFilter(SamplingMethod.RandomSampling, ratio:  0.2f);
        'icp.OutlierFilter = New TrimmedDistOutlierFilter(ratio:  0.5f);
        'var transform = icp.Compute(reading, reference, initialTransform);
        '-------------------------------------------------------------------------

        Dim reference As List(Of Vector3d)
        Dim reading As List(Of Vector3d)

        Dim OpenFileDialog As New OpenFileDialog
        ' Try
        OpenFileDialog.Filter = "Point Cloud file 1|*.pcd|All Files (*.*) |*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            ReadPCD_File(OpenFileDialog.FileName, reference)
        End If

        OpenFileDialog.Filter = "Point Cloud file 2|*.pcd|All Files (*.*) |*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            ReadPCD_File(OpenFileDialog.FileName, reading)
        End If



    End Sub

    Private Sub ReadPCD_File(ByVal PCDFileName As String, ByRef PointCloud As List(Of Vector3d))
        '--------------------------------------------------------
        Dim PointCloud1 As New Vector3d(0, 0, 0)
        Dim count As Integer = 0
        PointCloud = New List(Of Vector3d)
        '--------------------------------------------------------
        '
        Using sr As StreamReader = New StreamReader(PCDFileName)
            Dim Dataline As String

            ' Read and display lines from the file until the end of  
            ' the file is reached. 
            For i = 0 To 10
                Dataline = sr.ReadLine() 'skip Header lines
            Next

            Do Until sr.EndOfStream
                Dataline = sr.ReadLine()
                Dim CloudSent() As String = Dataline.Split(" ")
                PointCloud1.X = Convert.ToDouble(CloudSent(0))
                PointCloud1.Y = Convert.ToDouble(CloudSent(1))
                PointCloud1.Z = Convert.ToDouble(CloudSent(2))
                PointCloud.Add(PointCloud1)
            Loop
            sr.Close()
        End Using
        MsgBox("End Reading Cloud File")
    End Sub

#End Region

End Class

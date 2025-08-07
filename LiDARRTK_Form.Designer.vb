<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LiDARRTK_Form
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LiDARRTK_Form))
        Me.ribbon1 = New System.Windows.Forms.Ribbon()
        Me.RibbonOrbMenuItem1 = New System.Windows.Forms.RibbonOrbMenuItem()
        Me.RibbonSeparator1 = New System.Windows.Forms.RibbonSeparator()
        Me.RibbonOrbMenuItem2 = New System.Windows.Forms.RibbonOrbMenuItem()
        Me.RibbonTab1 = New System.Windows.Forms.RibbonTab()
        Me.RibbonPanel_Open = New System.Windows.Forms.RibbonPanel()
        Me.Open_Pcap_File_RibbonButton1 = New System.Windows.Forms.RibbonButton()
        Me.RibbonPanel_Data = New System.Windows.Forms.RibbonPanel()
        Me.RibbonComboBox1 = New System.Windows.Forms.RibbonComboBox()
        Me.RibbonButton_LiDAR = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_GPS = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Traj = New System.Windows.Forms.RibbonButton()
        Me.RibbonButtonRTK_Traj = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Frames = New System.Windows.Forms.RibbonButton()
        Me.RibbonButtonSlam_Traj = New System.Windows.Forms.RibbonButton()
        Me.SensorComboBox = New System.Windows.Forms.RibbonComboBox()
        Me.VLP16_Hi_Res = New System.Windows.Forms.RibbonButton()
        Me.VLP16 = New System.Windows.Forms.RibbonButton()
        Me.RibbonPanel_View = New System.Windows.Forms.RibbonPanel()
        Me.RibbonButton_Previous = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Play = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton4 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Next = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton3 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton5 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton6 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton7 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Stop = New System.Windows.Forms.RibbonButton()
        Me.RibbonTextBox_Frame = New System.Windows.Forms.RibbonTextBox()
        Me.RibbonLabel_NoFrames = New System.Windows.Forms.RibbonLabel()
        Me.RibbonPanel_Trajectory = New System.Windows.Forms.RibbonPanel()
        Me.ReadRTKdataButton = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton1 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton2 = New System.Windows.Forms.RibbonButton()
        Me.RibbonButtonAddSlamTraj = New System.Windows.Forms.RibbonButton()
        Me.ProcessButton = New System.Windows.Forms.RibbonButton()
        Me.RibbonButton_Export = New System.Windows.Forms.RibbonButton()
        Me.RibbonPanel1 = New System.Windows.Forms.RibbonPanel()
        Me.RibbonButton_Test_Match = New System.Windows.Forms.RibbonButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.OpenGLUC1 = New OpenTKLib.OpenGLUC()
        Me.HListView_Slam_Trajectory = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.HListView_Trajectory = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.HListView_NMEA = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.HListView_GPS = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.HListView_Packets = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.HListView_LiDAR = New HarpyEagle.HighPerformanceControls.HighPerformanceListView()
        Me.ListBoxNMEA = New System.Windows.Forms.ListBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ribbon1
        '
        Me.ribbon1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ribbon1.Location = New System.Drawing.Point(0, 0)
        Me.ribbon1.Minimized = False
        Me.ribbon1.Name = "ribbon1"
        '
        '
        '
        Me.ribbon1.OrbDropDown.BorderRoundness = 8
        Me.ribbon1.OrbDropDown.Location = New System.Drawing.Point(0, 0)
        Me.ribbon1.OrbDropDown.MenuItems.Add(Me.RibbonOrbMenuItem1)
        Me.ribbon1.OrbDropDown.MenuItems.Add(Me.RibbonSeparator1)
        Me.ribbon1.OrbDropDown.MenuItems.Add(Me.RibbonOrbMenuItem2)
        Me.ribbon1.OrbDropDown.Name = ""
        Me.ribbon1.OrbDropDown.Size = New System.Drawing.Size(527, 163)
        Me.ribbon1.OrbDropDown.TabIndex = 0
        Me.ribbon1.OrbStyle = System.Windows.Forms.RibbonOrbStyle.Office_2013
        Me.ribbon1.OrbText = "FILE"
        Me.ribbon1.RibbonTabFont = New System.Drawing.Font("Trebuchet MS", 9.0!)
        Me.ribbon1.Size = New System.Drawing.Size(1006, 138)
        Me.ribbon1.TabIndex = 1
        Me.ribbon1.Tabs.Add(Me.RibbonTab1)
        Me.ribbon1.TabSpacing = 4
        Me.ribbon1.Text = "ribbon1"
        Me.ribbon1.ThemeColor = System.Windows.Forms.RibbonTheme.Blue
        '
        'RibbonOrbMenuItem1
        '
        Me.RibbonOrbMenuItem1.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left
        Me.RibbonOrbMenuItem1.Image = Global.LiDAR_RTK.My.Resources.Resources.open32
        Me.RibbonOrbMenuItem1.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.open32
        Me.RibbonOrbMenuItem1.Name = "RibbonOrbMenuItem1"
        Me.RibbonOrbMenuItem1.SmallImage = Global.LiDAR_RTK.My.Resources.Resources.open32
        Me.RibbonOrbMenuItem1.Text = "Open"
        '
        'RibbonSeparator1
        '
        Me.RibbonSeparator1.Name = "RibbonSeparator1"
        '
        'RibbonOrbMenuItem2
        '
        Me.RibbonOrbMenuItem2.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left
        Me.RibbonOrbMenuItem2.Image = Global.LiDAR_RTK.My.Resources.Resources.close32
        Me.RibbonOrbMenuItem2.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.close32
        Me.RibbonOrbMenuItem2.Name = "RibbonOrbMenuItem2"
        Me.RibbonOrbMenuItem2.SmallImage = Global.LiDAR_RTK.My.Resources.Resources.close32
        Me.RibbonOrbMenuItem2.Text = "Close"
        '
        'RibbonTab1
        '
        Me.RibbonTab1.Name = "RibbonTab1"
        Me.RibbonTab1.Panels.Add(Me.RibbonPanel_Open)
        Me.RibbonTab1.Panels.Add(Me.RibbonPanel_Data)
        Me.RibbonTab1.Panels.Add(Me.RibbonPanel_View)
        Me.RibbonTab1.Panels.Add(Me.RibbonPanel_Trajectory)
        Me.RibbonTab1.Panels.Add(Me.RibbonPanel1)
        Me.RibbonTab1.Text = "Start"
        '
        'RibbonPanel_Open
        '
        Me.RibbonPanel_Open.Items.Add(Me.Open_Pcap_File_RibbonButton1)
        Me.RibbonPanel_Open.Name = "RibbonPanel_Open"
        Me.RibbonPanel_Open.Text = "Open Pcap File"
        '
        'Open_Pcap_File_RibbonButton1
        '
        Me.Open_Pcap_File_RibbonButton1.Image = Global.LiDAR_RTK.My.Resources.Resources.open32
        Me.Open_Pcap_File_RibbonButton1.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.open32
        Me.Open_Pcap_File_RibbonButton1.Name = "Open_Pcap_File_RibbonButton1"
        Me.Open_Pcap_File_RibbonButton1.SmallImage = CType(resources.GetObject("Open_Pcap_File_RibbonButton1.SmallImage"), System.Drawing.Image)
        Me.Open_Pcap_File_RibbonButton1.Text = "Open"
        '
        'RibbonPanel_Data
        '
        Me.RibbonPanel_Data.Items.Add(Me.RibbonComboBox1)
        Me.RibbonPanel_Data.Items.Add(Me.SensorComboBox)
        Me.RibbonPanel_Data.Name = "RibbonPanel_Data"
        Me.RibbonPanel_Data.Text = "Data"
        '
        'RibbonComboBox1
        '
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButton_LiDAR)
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButton_GPS)
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButton_Traj)
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButtonRTK_Traj)
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButton_Frames)
        Me.RibbonComboBox1.DropDownItems.Add(Me.RibbonButtonSlam_Traj)
        Me.RibbonComboBox1.LabelWidth = 43
        Me.RibbonComboBox1.Name = "RibbonComboBox1"
        Me.RibbonComboBox1.SelectedIndex = 0
        Me.RibbonComboBox1.Text = "Data"
        Me.RibbonComboBox1.TextBoxText = "LiDAR Data"
        '
        'RibbonButton_LiDAR
        '
        Me.RibbonButton_LiDAR.Image = CType(resources.GetObject("RibbonButton_LiDAR.Image"), System.Drawing.Image)
        Me.RibbonButton_LiDAR.LargeImage = CType(resources.GetObject("RibbonButton_LiDAR.LargeImage"), System.Drawing.Image)
        Me.RibbonButton_LiDAR.Name = "RibbonButton_LiDAR"
        Me.RibbonButton_LiDAR.SmallImage = CType(resources.GetObject("RibbonButton_LiDAR.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_LiDAR.Text = "LiDAR Data"
        Me.RibbonButton_LiDAR.Value = "LiADR"
        '
        'RibbonButton_GPS
        '
        Me.RibbonButton_GPS.Image = CType(resources.GetObject("RibbonButton_GPS.Image"), System.Drawing.Image)
        Me.RibbonButton_GPS.LargeImage = CType(resources.GetObject("RibbonButton_GPS.LargeImage"), System.Drawing.Image)
        Me.RibbonButton_GPS.Name = "RibbonButton_GPS"
        Me.RibbonButton_GPS.SmallImage = CType(resources.GetObject("RibbonButton_GPS.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_GPS.Text = "GPS Data"
        '
        'RibbonButton_Traj
        '
        Me.RibbonButton_Traj.Image = CType(resources.GetObject("RibbonButton_Traj.Image"), System.Drawing.Image)
        Me.RibbonButton_Traj.LargeImage = CType(resources.GetObject("RibbonButton_Traj.LargeImage"), System.Drawing.Image)
        Me.RibbonButton_Traj.Name = "RibbonButton_Traj"
        Me.RibbonButton_Traj.SmallImage = CType(resources.GetObject("RibbonButton_Traj.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Traj.Text = "Trajectory Data"
        '
        'RibbonButtonRTK_Traj
        '
        Me.RibbonButtonRTK_Traj.Image = CType(resources.GetObject("RibbonButtonRTK_Traj.Image"), System.Drawing.Image)
        Me.RibbonButtonRTK_Traj.LargeImage = CType(resources.GetObject("RibbonButtonRTK_Traj.LargeImage"), System.Drawing.Image)
        Me.RibbonButtonRTK_Traj.Name = "RibbonButtonRTK_Traj"
        Me.RibbonButtonRTK_Traj.SmallImage = CType(resources.GetObject("RibbonButtonRTK_Traj.SmallImage"), System.Drawing.Image)
        Me.RibbonButtonRTK_Traj.Text = "RTK_Trajectory"
        '
        'RibbonButton_Frames
        '
        Me.RibbonButton_Frames.Image = CType(resources.GetObject("RibbonButton_Frames.Image"), System.Drawing.Image)
        Me.RibbonButton_Frames.LargeImage = CType(resources.GetObject("RibbonButton_Frames.LargeImage"), System.Drawing.Image)
        Me.RibbonButton_Frames.Name = "RibbonButton_Frames"
        Me.RibbonButton_Frames.SmallImage = CType(resources.GetObject("RibbonButton_Frames.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Frames.Text = "Frames Info"
        '
        'RibbonButtonSlam_Traj
        '
        Me.RibbonButtonSlam_Traj.Image = CType(resources.GetObject("RibbonButtonSlam_Traj.Image"), System.Drawing.Image)
        Me.RibbonButtonSlam_Traj.LargeImage = CType(resources.GetObject("RibbonButtonSlam_Traj.LargeImage"), System.Drawing.Image)
        Me.RibbonButtonSlam_Traj.Name = "RibbonButtonSlam_Traj"
        Me.RibbonButtonSlam_Traj.SmallImage = CType(resources.GetObject("RibbonButtonSlam_Traj.SmallImage"), System.Drawing.Image)
        Me.RibbonButtonSlam_Traj.Text = "Slam Trajectory"
        '
        'SensorComboBox
        '
        Me.SensorComboBox.DropDownItems.Add(Me.VLP16_Hi_Res)
        Me.SensorComboBox.DropDownItems.Add(Me.VLP16)
        Me.SensorComboBox.Name = "SensorComboBox"
        Me.SensorComboBox.SelectedIndex = 1
        Me.SensorComboBox.Text = "Sensor"
        Me.SensorComboBox.TextBoxText = "VLP 16"
        '
        'VLP16_Hi_Res
        '
        Me.VLP16_Hi_Res.Image = CType(resources.GetObject("VLP16_Hi_Res.Image"), System.Drawing.Image)
        Me.VLP16_Hi_Res.LargeImage = CType(resources.GetObject("VLP16_Hi_Res.LargeImage"), System.Drawing.Image)
        Me.VLP16_Hi_Res.Name = "VLP16_Hi_Res"
        Me.VLP16_Hi_Res.SmallImage = CType(resources.GetObject("VLP16_Hi_Res.SmallImage"), System.Drawing.Image)
        Me.VLP16_Hi_Res.Text = "VLP 16 Hi-Res"
        '
        'VLP16
        '
        Me.VLP16.Image = CType(resources.GetObject("VLP16.Image"), System.Drawing.Image)
        Me.VLP16.LargeImage = CType(resources.GetObject("VLP16.LargeImage"), System.Drawing.Image)
        Me.VLP16.Name = "VLP16"
        Me.VLP16.SmallImage = CType(resources.GetObject("VLP16.SmallImage"), System.Drawing.Image)
        Me.VLP16.Text = "VLP 16"
        '
        'RibbonPanel_View
        '
        Me.RibbonPanel_View.Items.Add(Me.RibbonButton_Previous)
        Me.RibbonPanel_View.Items.Add(Me.RibbonButton_Play)
        Me.RibbonPanel_View.Items.Add(Me.RibbonButton_Next)
        Me.RibbonPanel_View.Items.Add(Me.RibbonButton_Stop)
        Me.RibbonPanel_View.Items.Add(Me.RibbonTextBox_Frame)
        Me.RibbonPanel_View.Items.Add(Me.RibbonLabel_NoFrames)
        Me.RibbonPanel_View.Name = "RibbonPanel_View"
        Me.RibbonPanel_View.Text = "View"
        '
        'RibbonButton_Previous
        '
        Me.RibbonButton_Previous.Image = Global.LiDAR_RTK.My.Resources.Resources.Prev_45
        Me.RibbonButton_Previous.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.Prev_45
        Me.RibbonButton_Previous.Name = "RibbonButton_Previous"
        Me.RibbonButton_Previous.SmallImage = CType(resources.GetObject("RibbonButton_Previous.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Previous.Text = "Previous"
        '
        'RibbonButton_Play
        '
        Me.RibbonButton_Play.DropDownItems.Add(Me.RibbonButton4)
        Me.RibbonButton_Play.Image = Global.LiDAR_RTK.My.Resources.Resources.Play
        Me.RibbonButton_Play.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.Play
        Me.RibbonButton_Play.Name = "RibbonButton_Play"
        Me.RibbonButton_Play.SmallImage = CType(resources.GetObject("RibbonButton_Play.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Play.Text = "Play"
        '
        'RibbonButton4
        '
        Me.RibbonButton4.Image = CType(resources.GetObject("RibbonButton4.Image"), System.Drawing.Image)
        Me.RibbonButton4.LargeImage = CType(resources.GetObject("RibbonButton4.LargeImage"), System.Drawing.Image)
        Me.RibbonButton4.Name = "RibbonButton4"
        Me.RibbonButton4.SmallImage = CType(resources.GetObject("RibbonButton4.SmallImage"), System.Drawing.Image)
        Me.RibbonButton4.Text = "RibbonButton4"
        '
        'RibbonButton_Next
        '
        Me.RibbonButton_Next.DropDownItems.Add(Me.RibbonButton3)
        Me.RibbonButton_Next.DropDownItems.Add(Me.RibbonButton5)
        Me.RibbonButton_Next.DropDownItems.Add(Me.RibbonButton6)
        Me.RibbonButton_Next.DropDownItems.Add(Me.RibbonButton7)
        Me.RibbonButton_Next.Image = Global.LiDAR_RTK.My.Resources.Resources.Next_45
        Me.RibbonButton_Next.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.Next_45
        Me.RibbonButton_Next.Name = "RibbonButton_Next"
        Me.RibbonButton_Next.SmallImage = CType(resources.GetObject("RibbonButton_Next.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Next.Text = "Next"
        '
        'RibbonButton3
        '
        Me.RibbonButton3.Image = CType(resources.GetObject("RibbonButton3.Image"), System.Drawing.Image)
        Me.RibbonButton3.LargeImage = CType(resources.GetObject("RibbonButton3.LargeImage"), System.Drawing.Image)
        Me.RibbonButton3.Name = "RibbonButton3"
        Me.RibbonButton3.SmallImage = CType(resources.GetObject("RibbonButton3.SmallImage"), System.Drawing.Image)
        Me.RibbonButton3.Text = "RibbonButton3"
        '
        'RibbonButton5
        '
        Me.RibbonButton5.Image = CType(resources.GetObject("RibbonButton5.Image"), System.Drawing.Image)
        Me.RibbonButton5.LargeImage = CType(resources.GetObject("RibbonButton5.LargeImage"), System.Drawing.Image)
        Me.RibbonButton5.Name = "RibbonButton5"
        Me.RibbonButton5.SmallImage = CType(resources.GetObject("RibbonButton5.SmallImage"), System.Drawing.Image)
        Me.RibbonButton5.Text = "RibbonButton5"
        '
        'RibbonButton6
        '
        Me.RibbonButton6.Image = CType(resources.GetObject("RibbonButton6.Image"), System.Drawing.Image)
        Me.RibbonButton6.LargeImage = CType(resources.GetObject("RibbonButton6.LargeImage"), System.Drawing.Image)
        Me.RibbonButton6.Name = "RibbonButton6"
        Me.RibbonButton6.SmallImage = CType(resources.GetObject("RibbonButton6.SmallImage"), System.Drawing.Image)
        Me.RibbonButton6.Text = "RibbonButton6"
        '
        'RibbonButton7
        '
        Me.RibbonButton7.Image = CType(resources.GetObject("RibbonButton7.Image"), System.Drawing.Image)
        Me.RibbonButton7.LargeImage = CType(resources.GetObject("RibbonButton7.LargeImage"), System.Drawing.Image)
        Me.RibbonButton7.Name = "RibbonButton7"
        Me.RibbonButton7.SmallImage = CType(resources.GetObject("RibbonButton7.SmallImage"), System.Drawing.Image)
        Me.RibbonButton7.Text = "RibbonButton7"
        '
        'RibbonButton_Stop
        '
        Me.RibbonButton_Stop.Image = Global.LiDAR_RTK.My.Resources.Resources._Stop
        Me.RibbonButton_Stop.LargeImage = Global.LiDAR_RTK.My.Resources.Resources._Stop
        Me.RibbonButton_Stop.Name = "RibbonButton_Stop"
        Me.RibbonButton_Stop.SmallImage = CType(resources.GetObject("RibbonButton_Stop.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Stop.Text = "Stop"
        '
        'RibbonTextBox_Frame
        '
        Me.RibbonTextBox_Frame.Name = "RibbonTextBox_Frame"
        Me.RibbonTextBox_Frame.Text = "LiDAR Frame"
        Me.RibbonTextBox_Frame.TextBoxText = ""
        Me.RibbonTextBox_Frame.TextBoxWidth = 50
        '
        'RibbonLabel_NoFrames
        '
        Me.RibbonLabel_NoFrames.Name = "RibbonLabel_NoFrames"
        Me.RibbonLabel_NoFrames.Text = "No. of Frames: "
        '
        'RibbonPanel_Trajectory
        '
        Me.RibbonPanel_Trajectory.Items.Add(Me.ReadRTKdataButton)
        Me.RibbonPanel_Trajectory.Items.Add(Me.RibbonButtonAddSlamTraj)
        Me.RibbonPanel_Trajectory.Items.Add(Me.ProcessButton)
        Me.RibbonPanel_Trajectory.Items.Add(Me.RibbonButton_Export)
        Me.RibbonPanel_Trajectory.Name = "RibbonPanel_Trajectory"
        Me.RibbonPanel_Trajectory.Text = "Trajectory"
        '
        'ReadRTKdataButton
        '
        Me.ReadRTKdataButton.DropDownItems.Add(Me.RibbonButton1)
        Me.ReadRTKdataButton.DropDownItems.Add(Me.RibbonButton2)
        Me.ReadRTKdataButton.Image = Global.LiDAR_RTK.My.Resources.Resources.RTK
        Me.ReadRTKdataButton.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.RTK
        Me.ReadRTKdataButton.Name = "ReadRTKdataButton"
        Me.ReadRTKdataButton.SmallImage = CType(resources.GetObject("ReadRTKdataButton.SmallImage"), System.Drawing.Image)
        Me.ReadRTKdataButton.Text = "Add_RTK_DATA"
        '
        'RibbonButton1
        '
        Me.RibbonButton1.Image = CType(resources.GetObject("RibbonButton1.Image"), System.Drawing.Image)
        Me.RibbonButton1.LargeImage = CType(resources.GetObject("RibbonButton1.LargeImage"), System.Drawing.Image)
        Me.RibbonButton1.Name = "RibbonButton1"
        Me.RibbonButton1.SmallImage = CType(resources.GetObject("RibbonButton1.SmallImage"), System.Drawing.Image)
        Me.RibbonButton1.Text = "RibbonButton1"
        '
        'RibbonButton2
        '
        Me.RibbonButton2.Image = CType(resources.GetObject("RibbonButton2.Image"), System.Drawing.Image)
        Me.RibbonButton2.LargeImage = CType(resources.GetObject("RibbonButton2.LargeImage"), System.Drawing.Image)
        Me.RibbonButton2.Name = "RibbonButton2"
        Me.RibbonButton2.SmallImage = CType(resources.GetObject("RibbonButton2.SmallImage"), System.Drawing.Image)
        Me.RibbonButton2.Text = "RibbonButton2"
        '
        'RibbonButtonAddSlamTraj
        '
        Me.RibbonButtonAddSlamTraj.Image = Global.LiDAR_RTK.My.Resources.Resources.Slam2
        Me.RibbonButtonAddSlamTraj.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.Slam2
        Me.RibbonButtonAddSlamTraj.Name = "RibbonButtonAddSlamTraj"
        Me.RibbonButtonAddSlamTraj.SmallImage = CType(resources.GetObject("RibbonButtonAddSlamTraj.SmallImage"), System.Drawing.Image)
        Me.RibbonButtonAddSlamTraj.Text = "Add_Slam_Trajectory"
        '
        'ProcessButton
        '
        Me.ProcessButton.Image = CType(resources.GetObject("ProcessButton.Image"), System.Drawing.Image)
        Me.ProcessButton.LargeImage = CType(resources.GetObject("ProcessButton.LargeImage"), System.Drawing.Image)
        Me.ProcessButton.Name = "ProcessButton"
        Me.ProcessButton.SmallImage = CType(resources.GetObject("ProcessButton.SmallImage"), System.Drawing.Image)
        Me.ProcessButton.Text = "Process"
        '
        'RibbonButton_Export
        '
        Me.RibbonButton_Export.Image = Global.LiDAR_RTK.My.Resources.Resources.Trajectory_11
        Me.RibbonButton_Export.LargeImage = Global.LiDAR_RTK.My.Resources.Resources.Trajectory_11
        Me.RibbonButton_Export.Name = "RibbonButton_Export"
        Me.RibbonButton_Export.SmallImage = CType(resources.GetObject("RibbonButton_Export.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Export.Text = "Export"
        '
        'RibbonPanel1
        '
        Me.RibbonPanel1.Items.Add(Me.RibbonButton_Test_Match)
        Me.RibbonPanel1.Name = "RibbonPanel1"
        Me.RibbonPanel1.Text = "RibbonPanel1"
        '
        'RibbonButton_Test_Match
        '
        Me.RibbonButton_Test_Match.Image = CType(resources.GetObject("RibbonButton_Test_Match.Image"), System.Drawing.Image)
        Me.RibbonButton_Test_Match.LargeImage = CType(resources.GetObject("RibbonButton_Test_Match.LargeImage"), System.Drawing.Image)
        Me.RibbonButton_Test_Match.Name = "RibbonButton_Test_Match"
        Me.RibbonButton_Test_Match.SmallImage = CType(resources.GetObject("RibbonButton_Test_Match.SmallImage"), System.Drawing.Image)
        Me.RibbonButton_Test_Match.Text = "Test Match"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SplitContainer1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 138)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1006, 380)
        Me.Panel1.TabIndex = 2
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.OpenGLUC1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_Slam_Trajectory)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_Trajectory)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_NMEA)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_GPS)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_Packets)
        Me.SplitContainer1.Panel2.Controls.Add(Me.HListView_LiDAR)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ListBoxNMEA)
        Me.SplitContainer1.Size = New System.Drawing.Size(1006, 380)
        Me.SplitContainer1.SplitterDistance = 257
        Me.SplitContainer1.TabIndex = 0
        '
        'OpenGLUC1
        '
        Me.OpenGLUC1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OpenGLUC1.Location = New System.Drawing.Point(0, 0)
        Me.OpenGLUC1.Name = "OpenGLUC1"
        Me.OpenGLUC1.Size = New System.Drawing.Size(257, 380)
        Me.OpenGLUC1.TabIndex = 0
        '
        'HListView_Slam_Trajectory
        '
        Me.HListView_Slam_Trajectory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_Slam_Trajectory.ListViewProvider = Nothing
        Me.HListView_Slam_Trajectory.Location = New System.Drawing.Point(0, 3)
        Me.HListView_Slam_Trajectory.Name = "HListView_Slam_Trajectory"
        Me.HListView_Slam_Trajectory.Size = New System.Drawing.Size(739, 352)
        Me.HListView_Slam_Trajectory.TabIndex = 12
        '
        'HListView_Trajectory
        '
        Me.HListView_Trajectory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_Trajectory.ListViewProvider = Nothing
        Me.HListView_Trajectory.Location = New System.Drawing.Point(0, 0)
        Me.HListView_Trajectory.Name = "HListView_Trajectory"
        Me.HListView_Trajectory.Size = New System.Drawing.Size(739, 352)
        Me.HListView_Trajectory.TabIndex = 11
        '
        'HListView_NMEA
        '
        Me.HListView_NMEA.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_NMEA.ListViewProvider = Nothing
        Me.HListView_NMEA.Location = New System.Drawing.Point(0, 0)
        Me.HListView_NMEA.Name = "HListView_NMEA"
        Me.HListView_NMEA.Size = New System.Drawing.Size(739, 352)
        Me.HListView_NMEA.TabIndex = 10
        '
        'HListView_GPS
        '
        Me.HListView_GPS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_GPS.ListViewProvider = Nothing
        Me.HListView_GPS.Location = New System.Drawing.Point(0, 0)
        Me.HListView_GPS.Name = "HListView_GPS"
        Me.HListView_GPS.Size = New System.Drawing.Size(739, 352)
        Me.HListView_GPS.TabIndex = 9
        '
        'HListView_Packets
        '
        Me.HListView_Packets.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_Packets.ListViewProvider = Nothing
        Me.HListView_Packets.Location = New System.Drawing.Point(0, 0)
        Me.HListView_Packets.Name = "HListView_Packets"
        Me.HListView_Packets.Size = New System.Drawing.Size(739, 352)
        Me.HListView_Packets.TabIndex = 8
        '
        'HListView_LiDAR
        '
        Me.HListView_LiDAR.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.HListView_LiDAR.ListViewProvider = Nothing
        Me.HListView_LiDAR.Location = New System.Drawing.Point(0, 0)
        Me.HListView_LiDAR.Name = "HListView_LiDAR"
        Me.HListView_LiDAR.Size = New System.Drawing.Size(739, 352)
        Me.HListView_LiDAR.TabIndex = 7
        '
        'ListBoxNMEA
        '
        Me.ListBoxNMEA.FormattingEnabled = True
        Me.ListBoxNMEA.Location = New System.Drawing.Point(449, 18)
        Me.ListBoxNMEA.Name = "ListBoxNMEA"
        Me.ListBoxNMEA.Size = New System.Drawing.Size(210, 186)
        Me.ListBoxNMEA.TabIndex = 4
        Me.ListBoxNMEA.Visible = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 496)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1006, 22)
        Me.StatusStrip1.TabIndex = 3
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(42, 17)
        Me.StatusLabel.Text = "Status:"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'LiDARRTK_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1006, 518)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ribbon1)
        Me.KeyPreview = True
        Me.Name = "LiDARRTK_Form"
        Me.Text = "LiDAR RTK"
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Protected WithEvents ribbon1 As Ribbon
    Friend WithEvents RibbonTab1 As RibbonTab
    Friend WithEvents RibbonPanel_Open As RibbonPanel
    Friend WithEvents Open_Pcap_File_RibbonButton1 As RibbonButton
    Friend WithEvents RibbonOrbMenuItem1 As RibbonOrbMenuItem
    Friend WithEvents RibbonSeparator1 As RibbonSeparator
    Friend WithEvents RibbonOrbMenuItem2 As RibbonOrbMenuItem
    Friend WithEvents Panel1 As Panel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents RibbonPanel_Data As RibbonPanel
    Friend WithEvents RibbonComboBox1 As RibbonComboBox
    Friend WithEvents RibbonButton_LiDAR As RibbonButton
    Friend WithEvents RibbonButton_GPS As RibbonButton
    Friend WithEvents RibbonPanel_View As RibbonPanel
    Friend WithEvents RibbonButton_Previous As RibbonButton
    Friend WithEvents RibbonButton_Next As RibbonButton
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents RibbonButton_Frames As RibbonButton
    Friend WithEvents RibbonTextBox_Frame As RibbonTextBox
    Friend WithEvents RibbonLabel_NoFrames As RibbonLabel
    Friend WithEvents OpenGLUC1 As OpenTKLib.OpenGLUC
    Friend WithEvents RibbonPanel_Trajectory As RibbonPanel
    Friend WithEvents RibbonButton_Export As RibbonButton
    Friend WithEvents SensorComboBox As RibbonComboBox
    Friend WithEvents VLP16_Hi_Res As RibbonButton
    Friend WithEvents VLP16 As RibbonButton
    Friend WithEvents ProcessButton As RibbonButton
    Friend WithEvents ListBoxNMEA As ListBox
    Friend WithEvents RibbonButton_Traj As RibbonButton
    Friend WithEvents ReadRTKdataButton As RibbonButton
    Friend WithEvents RibbonButton1 As RibbonButton
    Friend WithEvents RibbonButton2 As RibbonButton
    Friend WithEvents RibbonButtonRTK_Traj As RibbonButton
    Friend WithEvents HListView_LiDAR As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents RibbonButton3 As RibbonButton
    Friend WithEvents RibbonButton_Play As RibbonButton
    Friend WithEvents RibbonButton5 As RibbonButton
    Friend WithEvents RibbonButton6 As RibbonButton
    Friend WithEvents RibbonButton7 As RibbonButton
    Friend WithEvents RibbonButton4 As RibbonButton
    Friend WithEvents RibbonButton_Stop As RibbonButton
    Friend WithEvents RibbonPanel1 As RibbonPanel
    Friend WithEvents RibbonButton_Test_Match As RibbonButton
    Friend WithEvents HListView_Packets As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents HListView_GPS As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents HListView_Trajectory As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents HListView_NMEA As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents RibbonButtonAddSlamTraj As RibbonButton
    Friend WithEvents HListView_Slam_Trajectory As HarpyEagle.HighPerformanceControls.HighPerformanceListView
    Friend WithEvents RibbonButtonSlam_Traj As RibbonButton
End Class

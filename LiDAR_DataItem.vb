Imports System
Imports System.Collections.Generic
Imports System.Text

Public Class LiDAR_DataItem

    Public ID As Integer
    Public Azimuth As Single
    Public Distance As Single
    Public Intensity As Integer
    Public Laser_ID As Integer
    Public Time_Stamp As Long
    Public Vertical_Angle As Double
    Public X As Double
    Public Y As Double
    Public Z As Double

    Public Sub New(ByVal ID As Integer, ByVal Azimuth As Single,
                       ByVal Distance As Single, ByVal Intensity As Integer,
                       ByVal Laser_ID As Integer, ByVal Time_Stamp As Long, ByVal Vertical_Angle As Double,
                       ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        Me.ID = PointID
        Me.Azimuth = Az
        Me.Distance = Dist
        Me.Intensity = Intsty
        Me.Laser_ID = LaserID
        Me.Time_Stamp = TimeStamp
        Me.Vertical_Angle = VerticalAngle
        Me.X = X_
        Me.Y = Y_
        Me.Z = Z_
    End Sub

    Public Property PointID As Integer
        Get
            Return ID
        End Get
        Set(ByVal value As Integer)
            ID = value
        End Set
    End Property

    Public Property Az As Single
        Get
            Return Azimuth
        End Get
        Set(ByVal value As Single)
            Azimuth = value
        End Set
    End Property

    Public Property Dist As Single
        Get
            Return Distance
        End Get
        Set(ByVal value As Single)
            Distance = value
        End Set
    End Property

    Public Property Intsty As Integer
        Get
            Return Intensity
        End Get
        Set(ByVal value As Integer)
            Intensity = value
        End Set
    End Property

    Public Property LaserID As Integer
        Get
            Return Laser_ID
        End Get
        Set(ByVal value As Integer)
            Laser_ID = value
        End Set
    End Property

    Public Property TimeStamp As Long
        Get
            Return Time_Stamp
        End Get
        Set(ByVal value As Long)
            Time_Stamp = value
        End Set
    End Property

    Public Property VerticalAngle As Double
        Get
            Return Vertical_Angle
        End Get
        Set(ByVal value As Double)
            Vertical_Angle = value
        End Set
    End Property

    Public Property X_ As Double
        Get
            Return X
        End Get
        Set(ByVal value As Double)
            X = value
        End Set
    End Property

    Public Property Y_ As Double
        Get
            Return Y
        End Get
        Set(ByVal value As Double)
            Y = value
        End Set
    End Property

    Public Property Z_ As Double
        Get
            Return Z
        End Get
        Set(ByVal value As Double)
            Z = value
        End Set
    End Property


End Class

Public Class GPS_DataItem

    Public ID As Integer
    Public GPSTime As Single
    Public Latitude As Double
    Public Longitude As Double
    Public Height As Double
    Public Quality As String
    Public Time_Stamp As Long
    Public Heading As Single
    Public East As Double
    Public North As Double
    Public Speed As Single

    Public Sub New(ByVal ID As Integer, ByVal GPSTime As Single,
                       ByVal Latitude As Double, ByVal Longitude As Double,
                       ByVal Height As Double, ByVal Quality As String,
                      ByVal Time_Stamp As Long, ByVal Heading As Single,
                     ByVal East As Double, ByVal North As Double, ByVal Speed As Single)
        Me.ID = PointID
        Me.GPSTime = GPS_Time
        Me.Latitude = Lat
        Me.Longitude = Lng
        Me.Height = h_
        Me.Quality = Qlty
        Me.Time_Stamp = TimeStamp
        Me.Heading = Heading_
        Me.East = E_
        Me.North = N_
        Me.Speed = Speed_

    End Sub

    Public Property PointID As Integer
        Get
            Return ID
        End Get
        Set(ByVal value As Integer)
            ID = value
        End Set
    End Property

    Public Property GPS_Time As Single
        Get
            Return GPSTime
        End Get
        Set(ByVal value As Single)
            GPSTime = value
        End Set
    End Property

    Public Property Lat As Double
        Get
            Return Latitude
        End Get
        Set(ByVal value As Double)
            Latitude = value
        End Set
    End Property

    Public Property Lng As Double
        Get
            Return Longitude
        End Get
        Set(ByVal value As Double)
            Longitude = value
        End Set
    End Property

    Public Property h_ As Double
        Get
            Return Height
        End Get
        Set(ByVal value As Double)
            Height = value
        End Set
    End Property

    Public Property Qlty As String
        Get
            Return Quality
        End Get
        Set(ByVal value As String)
            Quality = value
        End Set
    End Property

    Public Property TimeStamp As Long
        Get
            Return Time_Stamp
        End Get
        Set(ByVal value As Long)
            Time_Stamp = value
        End Set
    End Property

    Public Property Heading_ As Single
        Get
            Return Heading
        End Get
        Set(ByVal value As Single)
            Heading = value
        End Set
    End Property

    Public Property E_ As Double
        Get
            Return East
        End Get
        Set(ByVal value As Double)
            East = value
        End Set
    End Property

    Public Property N_ As Double
        Get
            Return North
        End Get
        Set(ByVal value As Double)
            North = value
        End Set
    End Property

    Public Property Speed_ As Single
        Get
            Return Speed
        End Get
        Set(ByVal value As Single)
            Speed = value
        End Set
    End Property

End Class

Public Class Packets_DataItem

    Public Frame_ID As Integer
    Public Packet_From As Integer
    Public Packet_To As Integer
    Public Time_Stamp_From As Long
    Public Time_Stamp_To As Long

    Public Sub New(ByVal Frame_ID As Integer, ByVal Packet_From As Integer,
                       ByVal Packet_To As Integer, ByVal Time_Stamp_From As Long,
                       ByVal Time_Stamp_To As Long)
        Me.Frame_ID = ID
        Me.Packet_From = PacketFrom
        Me.Packet_To = PacketTo
        Me.Time_Stamp_From = TimeStampFrom
        Me.Time_Stamp_To = TimeStampTo


    End Sub

    Public Property ID As Integer
        Get
            Return Frame_ID
        End Get
        Set(ByVal value As Integer)
            Frame_ID = value
        End Set
    End Property

    Public Property PacketFrom As Integer
        Get
            Return Packet_From
        End Get
        Set(ByVal value As Integer)
            Packet_From = value
        End Set
    End Property

    Public Property PacketTo As Integer
        Get
            Return Packet_To
        End Get
        Set(ByVal value As Integer)
            Packet_To = value
        End Set
    End Property

    Public Property TimeStampFrom As Long
        Get
            Return Time_Stamp_From
        End Get
        Set(ByVal value As Long)
            Time_Stamp_From = value
        End Set
    End Property

    Public Property TimeStampTo As Long
        Get
            Return Time_Stamp_To
        End Get
        Set(ByVal value As Long)
            Time_Stamp_To = value
        End Set
    End Property


End Class

Public Class NMEA_DataItem

    Public GPS_Time As Single
    Public East As Double
    Public North As Double
    Public Height As Double
    Public Time_Stamp As Long
    Public Speed As Single
    Public Distance As Single
    Public Heading As Single
    Public Azimuth As Single

    Public Sub New(ByVal GPS_Time As Integer, ByVal East As Double,
                       ByVal North As Double, ByVal Height As Double,
                       ByVal Time_Stamp As Long, ByVal Speed As Single,
                      ByVal Distance As Single, ByVal Heading As Single,
                   ByVal Azimuth As Single)
        Me.GPS_Time = GPSTime
        Me.East = E_
        Me.North = N_
        Me.Height = h_
        Me.Time_Stamp = TimeStamp
        Me.Speed = Speed
        Me.Distance = Dist
        Me.Heading = Heading_
        Me.Azimuth = Az


    End Sub

    Public Property GPSTime As Integer
        Get
            Return GPS_Time
        End Get
        Set(ByVal value As Integer)
            GPS_Time = value
        End Set
    End Property

    Public Property E_ As Double
        Get
            Return East
        End Get
        Set(ByVal value As Double)
            East = value
        End Set
    End Property

    Public Property N_ As Double
        Get
            Return North
        End Get
        Set(ByVal value As Double)
            North = value
        End Set
    End Property

    Public Property h_ As Double
        Get
            Return Height
        End Get
        Set(ByVal value As Double)
            Height = value
        End Set
    End Property


    Public Property TimeStamp As Long
        Get
            Return Time_Stamp
        End Get
        Set(ByVal value As Long)
            Time_Stamp = value
        End Set
    End Property

    Public Property Heading_ As Single
        Get
            Return Heading
        End Get
        Set(ByVal value As Single)
            Heading = value
        End Set
    End Property

    Public Property Dist As Double
        Get
            Return Distance
        End Get
        Set(ByVal value As Double)
            Distance = value
        End Set
    End Property

    Public Property Speed_ As Single
        Get
            Return Speed
        End Get
        Set(ByVal value As Single)
            Speed = value
        End Set
    End Property

    Public Property Az As Double
        Get
            Return Azimuth
        End Get
        Set(ByVal value As Double)
            Azimuth = value
        End Set
    End Property


End Class

Public Class Trajectory_DataItem

    Public Frame_ID As Integer
    Public Time_Stamp As Long
    Public Rotation_X As Double
    Public Rotation_Y As Double
    Public Rotation_Z As Double
    'Public X As Double
    'Public Y As Double
    'Public Z As Single
    Public East As Double
    Public North As Double
    Public Height As Double

    'Public Sub New(ByVal Frame_ID As Integer, ByVal Time_Stamp As Long, ByVal Rotation_X As Double,
    'ByVal Rotation_Y As Double, ByVal Rotation_Z As Double,
    ' ByVal X As Double, ByVal Y As Double, ByVal Z As Double,
    'ByVal East As Double, ByVal North As Double, ByVal Height As Double)

    Public Sub New(ByVal Frame_ID As Integer, ByVal Time_Stamp As Long, ByVal Rotation_X As Double,
                       ByVal Rotation_Y As Double, ByVal Rotation_Z As Double,
                       ByVal East As Double, ByVal North As Double, ByVal Height As Double)
        Me.Frame_ID = ID
        Me.Time_Stamp = TimeStamp
        Me.Rotation_X = Rx
        Me.Rotation_Y = Ry
        Me.Rotation_Z = Rz
        'Me.X = X_
        ' Me.Y = Y_
        ' Me.Z = Z_
        Me.East = E
        Me.North = N
        Me.Height = h_

    End Sub

    Public Property ID As Integer
        Get
            Return Frame_ID
        End Get
        Set(ByVal value As Integer)
            Frame_ID = value
        End Set
    End Property

    Public Property TimeStamp As Long
        Get
            Return Time_Stamp
        End Get
        Set(ByVal value As Long)
            Time_Stamp = value
        End Set
    End Property

    Public Property Rx As Double
        Get
            Return Rotation_X
        End Get
        Set(ByVal value As Double)
            Rotation_X = value
        End Set
    End Property

    Public Property Ry As Double
        Get
            Return Rotation_Y
        End Get
        Set(ByVal value As Double)
            Rotation_Y = value
        End Set
    End Property

    Public Property Rz As Double
        Get
            Return Rotation_Z
        End Get
        Set(ByVal value As Double)
            Rotation_Z = value
        End Set
    End Property
    ' Public Property X_ As Double
    'Get
    'Return X
    'End Get
    'Set(ByVal value As Double)
    ' X = value
    'End Set
    'End Property

    'Public Property Y_ As Double
    'Get
    'Return Y
    'End Get
    'Set(ByVal value As Double)
    ' Y = value
    'End Set
    'End Property

    ' Public Property Z_ As Double
    'Get
    'Return Z
    'End Get
    'Set(ByVal value As Double)
    '    Z = value
    ' End Set
    ' End Property

    Public Property E As Double
        Get
            Return East
        End Get
        Set(ByVal value As Double)
            East = value
        End Set
    End Property

    Public Property N As Double
        Get
            Return North
        End Get
        Set(ByVal value As Double)
            North = value
        End Set
    End Property

    Public Property h_ As Double
        Get
            Return Height
        End Get
        Set(ByVal value As Double)
            Height = value
        End Set
    End Property




End Class

Public Class SlamTraj_DataItemXXX
    Public Time_Stamp As Long
    Public Rotation_X As Double
    Public Rotation_Y As Double
    Public Rotation_Z As Double
    Public X As Double
    Public Y As Double
    Public Z As Single

    Public Sub New(ByVal Time_Stamp As Long, ByVal Rotation_X As Double,
                       ByVal Rotation_Y As Double, ByVal Rotation_Z As Double,
                       ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        Me.Time_Stamp = TimeStamp
        Me.Rotation_X = Rx
        Me.Rotation_Y = Ry
        Me.Rotation_Z = Rz
        Me.X = X_
        Me.Y = Y_
        Me.Z = Z_

    End Sub

    Public Property TimeStamp As Long
        Get
            Return Time_Stamp
        End Get
        Set(ByVal value As Long)
            Time_Stamp = value
        End Set
    End Property

    Public Property Rx As Double
        Get
            Return Rotation_X
        End Get
        Set(ByVal value As Double)
            Rotation_X = value
        End Set
    End Property

    Public Property Ry As Double
        Get
            Return Rotation_Y
        End Get
        Set(ByVal value As Double)
            Rotation_Y = value
        End Set
    End Property

    Public Property Rz As Double
        Get
            Return Rotation_Z
        End Get
        Set(ByVal value As Double)
            Rotation_Z = value
        End Set
    End Property

    Public Property X_ As Double
        Get
            Return X
        End Get
        Set(ByVal value As Double)
            X = value
        End Set
    End Property

    Public Property Y_ As Double
        Get
            Return Y
        End Get
        Set(ByVal value As Double)
            Y = value
        End Set
    End Property

    Public Property Z_ As Double
        Get
            Return Z
        End Get
        Set(ByVal value As Double)
            Z = value
        End Set
    End Property

End Class
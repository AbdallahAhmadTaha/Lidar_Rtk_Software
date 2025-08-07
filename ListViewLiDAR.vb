Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports HarpyEagle.HighPerformanceControls

Public Class ListViewLiDAR

    Inherits ListViewProviderBase

    Private data_List As List(Of LiDAR_DataItem)

    Public Property DataList As List(Of LiDAR_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of LiDAR_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewLiDAR Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "Point ID"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Azimuth"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Distance"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Intensity"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Laser ID"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "TimeStamp"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Vertical Angle"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "X"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Y"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Z"
            hdr.Width = 100
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem
        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).ID)
            result.SubItems.Add(DataList(dataIndex).Azimuth)
            result.SubItems.Add(DataList(dataIndex).Distance)
            result.SubItems.Add(DataList(dataIndex).Intensity)
            result.SubItems.Add(DataList(dataIndex).Laser_ID)
            result.SubItems.Add(DataList(dataIndex).TimeStamp)
            result.SubItems.Add(DataList(dataIndex).Vertical_Angle)
            result.SubItems.Add(DataList(dataIndex).X)
            result.SubItems.Add(DataList(dataIndex).Y)
            result.SubItems.Add(DataList(dataIndex).Z)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property

    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class

Public Class ListViewGPS

    Inherits ListViewProviderBase

    Private data_List As List(Of GPS_DataItem)

    Public Property DataList As List(Of GPS_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of GPS_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewGPS Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "Point ID"
            hdr.Width = 50
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "GPS Time"
            hdr.Width = 50
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Latitude"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Longitude"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Height"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Quality"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Time Stamp"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Heading"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Speed"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "East"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "North"
            hdr.Width = 75
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem
 
        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).ID)
            result.SubItems.Add(DataList(dataIndex).GPSTime)
            result.SubItems.Add(DataList(dataIndex).Latitude)
            result.SubItems.Add(DataList(dataIndex).Longitude)
            result.SubItems.Add(DataList(dataIndex).Height)
            result.SubItems.Add(DataList(dataIndex).Quality)
            result.SubItems.Add(DataList(dataIndex).TimeStamp)
            result.SubItems.Add(DataList(dataIndex).Heading)
            result.SubItems.Add(DataList(dataIndex).Speed)
            result.SubItems.Add(DataList(dataIndex).East)
            result.SubItems.Add(DataList(dataIndex).North)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property


    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class

Public Class ListViewPackets

    Inherits ListViewProviderBase

    Private data_List As List(Of Packets_DataItem)

    Public Property DataList As List(Of Packets_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of Packets_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewPackets Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "Frame ID"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Packet From"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Packet To"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Time Stamp From"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Time Stamp To"
            hdr.Width = 100
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem

        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).Frame_ID)
            result.SubItems.Add(DataList(dataIndex).Packet_From)
            result.SubItems.Add(DataList(dataIndex).Packet_To)
            result.SubItems.Add(DataList(dataIndex).Time_Stamp_From)
            result.SubItems.Add(DataList(dataIndex).Time_Stamp_To)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property


    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class

Public Class ListViewNMEA

    Inherits ListViewProviderBase

    Private data_List As List(Of NMEA_DataItem)

    Public Property DataList As List(Of NMEA_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of NMEA_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewNMEA Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "GPS Time"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "East"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "North"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Height"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "TimeStamp"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Speed"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Distance"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Heading"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Azimuth"
            hdr.Width = 100
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem
        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).GPS_Time)
            result.SubItems.Add(DataList(dataIndex).East)
            result.SubItems.Add(DataList(dataIndex).North)
            result.SubItems.Add(DataList(dataIndex).Height)
            result.SubItems.Add(DataList(dataIndex).Time_Stamp)
            result.SubItems.Add(DataList(dataIndex).Speed)
            result.SubItems.Add(DataList(dataIndex).Distance)
            result.SubItems.Add(DataList(dataIndex).Heading)
            result.SubItems.Add(DataList(dataIndex).Azimuth)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property

    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class

Public Class ListViewTrajectory

    Inherits ListViewProviderBase

    Private data_List As List(Of Trajectory_DataItem)

    Public Property DataList As List(Of Trajectory_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of Trajectory_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewTrajectory Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "Frame ID"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Time Stamp"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Rx"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Ry"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Rz"
            hdr.Width = 75
            result.Add(hdr)
            'hdr = New ColumnHeader()
            'hdr.Text = "X"
            'hdr.Width = 100
            'result.Add(hdr)
            'hdr = New ColumnHeader()
            'hdr.Text = "Y"
            'hdr.Width = 100
            'result.Add(hdr)
            'hdr = New ColumnHeader()
            'hdr.Text = "Z"
            'hdr.Width = 100
            'result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "E"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "N"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "H"
            hdr.Width = 100
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem
        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).Frame_ID)
            result.SubItems.Add(DataList(dataIndex).Time_Stamp)
            result.SubItems.Add(DataList(dataIndex).Rx)
            result.SubItems.Add(DataList(dataIndex).Ry)
            result.SubItems.Add(DataList(dataIndex).Rz)
            'result.SubItems.Add(DataList(dataIndex).X)
            'result.SubItems.Add(DataList(dataIndex).Y)
            'result.SubItems.Add(DataList(dataIndex).Z)
            result.SubItems.Add(DataList(dataIndex).East)
            result.SubItems.Add(DataList(dataIndex).North)
            result.SubItems.Add(DataList(dataIndex).Height)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property

    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class

Public Class ListViewSlamTrajectory

    Inherits ListViewProviderBase

    Private data_List As List(Of Trajectory_DataItem)

    Public Property DataList As List(Of Trajectory_DataItem)
        Get
            Return data_List
        End Get

        Set(ByVal value As List(Of Trajectory_DataItem))
            data_List = value
            If listView Is Nothing Then Throw New Exception("ListViewSlamTrajectory Error - the internal HighPerformanceListView has not been set, cannot display data")
            listView.DisplayDataItemList()
        End Set
    End Property

    Public Overrides ReadOnly Property ColumnHeaders As System.Collections.Generic.List(Of System.Windows.Forms.ColumnHeader)
        Get
            ' Time, Rx(Roll), Ry(Pitch), Rz(Yaw), X, Y, Z
            Dim hdr As ColumnHeader
            Dim result As List(Of ColumnHeader) = New List(Of ColumnHeader)()
            hdr = New ColumnHeader()
            hdr.Text = "Frame ID"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Time Stamp"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Rx"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Ry"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Rz"
            hdr.Width = 75
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "X"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Y"
            hdr.Width = 100
            result.Add(hdr)
            hdr = New ColumnHeader()
            hdr.Text = "Z"
            hdr.Width = 100
            result.Add(hdr)
            Return result
        End Get
    End Property

    Public Overrides Function ConvertDataItemToListViewItem(ByVal dataIndex As Integer) As ListViewItem
        Dim result As ListViewItem
        If DataList IsNot Nothing AndAlso dataIndex < DataList.Count Then
            result = New ListViewItem(Me.DataList(dataIndex).Frame_ID)
            result.SubItems.Add(DataList(dataIndex).TimeStamp)
            result.SubItems.Add(DataList(dataIndex).Rx)
            result.SubItems.Add(DataList(dataIndex).Ry)
            result.SubItems.Add(DataList(dataIndex).Rz)
            result.SubItems.Add(DataList(dataIndex).East)
            result.SubItems.Add(DataList(dataIndex).North)
            result.SubItems.Add(DataList(dataIndex).Height)
        Else
            result = Nothing
        End If

        Return result
    End Function

    Public Overrides ReadOnly Property DataCount As Integer
        Get
            Dim result As Integer
            If DataList IsNot Nothing Then result = DataList.Count Else result = 0
            Return result
        End Get
    End Property

    Public Overrides Sub SortDataList(ByVal sortColumnNumber As Integer, ByVal sortOrder As System.Windows.Forms.SortOrder)
        If DataList IsNot Nothing Then

        End If
    End Sub

End Class
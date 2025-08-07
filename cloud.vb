
Class cloud
    Public Property cloudX As IList(Of cloud_point)

    Public Sub New()
        cloudX = New List(Of cloud_point)()
    End Sub

    Public Sub cloud_add(ByVal point As cloud_point)
        cloudX.Add(point)
    End Sub

    Public Sub cloud_clear()
        cloudX.Clear()
    End Sub
End Class

Class cloud_point
    'Public Property ID As Integer
    'Public Property azimuth As Double
    Public Property X As Double
    Public Property Y As Double
    Public Property Z As Double

    Public Sub New(ByVal X_in As Double, ByVal Y_in As Double, ByVal Z_in As Double)

        ' Public Sub New(ByVal ID_in As Integer, ByVal azimuth_in As Double, ByVal X_in As Double, ByVal Y_in As Double, ByVal Z_in As Double)
        '  ID = ID_in
        ' azimuth = azimuth_in
        X = X_in
        Y = Y_in
        Z = Z_in
    End Sub
End Class

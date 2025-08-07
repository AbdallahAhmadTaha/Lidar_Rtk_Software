Public Class GPSParser
    '***********************************************************
    '**   NMEA - 0183 Parser Version 1.0 30th December 2005   * *
    '**                                                       * *
    '** Copyright Paul Bartlett paul.bartlett@btopenworld.com * *
    '**                                                       * *
    '**               http://www.kxlan.co.uk/                 * *
    '**                                                       * *
    '**  Permission to use this code is granted on condition  * *
    '**     this header must appear in every form/module.     * *
    '**                                                       * *
    '***********************************************************
    Public Structure GGA
        Public strUttcTime As String
        Public varLatitude As String
        Public strNSIndicator As String
        Public varLongitude As String
        Public strEWIndicator As String
        Public strPositionFix As String
        Public strSatsUsed As String
        Public strIIDOP As String
        Public strAltitude As String
        Public strAltUnits As String
        Public strGeoid As String
        Public strSepUnits As String
        Public strDgpsAge As String
        Public strDgpsid As String
        Public strchecksum As String
        Public strTerminator As String
    End Structure


    Public Structure VTG
        Public strCourse1 As String
        Public strReference1 As String
        Public strCourse2 As String
        Public strReference2 As String
        Public strSpeed1 As String
        Public strSpeed2 As String
        Public strSpeedUnit1 As String
        Public strSpeedUnit2 As String
    End Structure
    Public Structure GSA
        Public strMode As String
        Public strFixType As String
        Public strSat1 As String
        Public strSat2 As String
        Public strSat3 As String
        Public strSat4 As String
        Public strSat5 As String
        Public strSat6 As String
        Public strSat7 As String
        Public strSat8 As String
        Public strSat9 As String
        Public strSat10 As String
        Public strSat11 As String
        Public strSat12 As String
        Public strPDOP As String
        Public strHDOP As String
        Public strVDOP As String
    End Structure
    Public Structure GSV
        Public StrNoMessages As String
        Public StrSeq As String
        Public StrSatsinview As String
        Public StrSatId1 As String
        Public StrElevation1 As String
        Public StrAzimuth1 As String
        Public StrSNR1 As String
        Public StrSatId2 As String
        Public StrElevation2 As String
        Public StrAzimuth2 As String
        Public StrSNR2 As String
        Public StrSatId3 As String
        Public StrElevation3 As String
        Public StrAzimuth3 As String
        Public StrSNR3 As String
        Public StrSatId4 As String
        Public StrElevation4 As String
        Public StrAzimuth4 As String
        Public StrSNR4 As String
    End Structure

    Public Sub ParseSentence(ByVal InBuff As String, ByRef MyGGA As GGA, ByRef Myvtg As VTG, ByRef MyGSA As GSA, ByRef MyGSV As GSV)

        Dim aryData() As String = InBuff.Split(",")
        Dim intComma As Integer = aryData.Count

        Select Case Left(aryData(0), 6)
            Case "$GPGGA"
                MyGGA.strUttcTime = aryData(0)
                MyGGA.varLatitude = aryData(1)
                MyGGA.strNSIndicator = aryData(2)
                MyGGA.varLongitude = aryData(3)
                MyGGA.strEWIndicator = aryData(4)
                MyGGA.strPositionFix = aryData(5)
                MyGGA.strSatsUsed = aryData(6)
                MyGGA.strIIDOP = aryData(7)
                MyGGA.strAltitude = aryData(8)
                MyGGA.strAltUnits = aryData(9)
                MyGGA.strGeoid = aryData(10)
                MyGGA.strSepUnits = aryData(11)
                MyGGA.strDgpsAge = aryData(12)
                MyGGA.strDgpsid = aryData(13)
            Case "$GPVTG"
                Myvtg.strCourse1 = aryData(0)
                Myvtg.strReference1 = aryData(1)
                Myvtg.strCourse2 = aryData(2)
                Myvtg.strReference2 = aryData(3)
                Myvtg.strSpeed1 = aryData(4)
                Myvtg.strSpeedUnit1 = aryData(5)
                Myvtg.strSpeed2 = aryData(6)
                Myvtg.strSpeedUnit2 = aryData(7)
            Case "$GPGSA"
                MyGSA.strMode = aryData(0)
                MyGSA.strFixType = aryData(1)
                MyGSA.strSat1 = aryData(2)
                MyGSA.strSat2 = aryData(3)
                MyGSA.strSat3 = aryData(4)
                MyGSA.strSat4 = aryData(5)
                MyGSA.strSat5 = aryData(6)
                MyGSA.strSat6 = aryData(7)
                MyGSA.strSat7 = aryData(8)
                MyGSA.strSat8 = aryData(9)
                MyGSA.strSat9 = aryData(10)
                MyGSA.strSat10 = aryData(11)
                MyGSA.strSat11 = aryData(12)
                MyGSA.strSat12 = aryData(13)
                MyGSA.strPDOP = aryData(14)
                MyGSA.strHDOP = aryData(15)
                MyGSA.strVDOP = aryData(16)
            Case "$GPGSV"
                MyGSV.StrNoMessages = aryData(0)
                MyGSV.StrSeq = aryData(1)
                MyGSV.StrSatsinview = aryData(2)
                MyGSV.StrSatId1 = aryData(3)
                MyGSV.StrElevation1 = aryData(4)
                MyGSV.StrAzimuth1 = aryData(5)
                MyGSV.StrSNR1 = aryData(6)
                MyGSV.StrSatId2 = aryData(7)
                MyGSV.StrElevation2 = aryData(8)
                MyGSV.StrAzimuth2 = aryData(9)
                MyGSV.StrSNR2 = aryData(10)
                MyGSV.StrSatId3 = aryData(11)
                MyGSV.StrElevation3 = aryData(12)
                MyGSV.StrAzimuth3 = aryData(13)
                MyGSV.StrSNR3 = aryData(14)
                MyGSV.StrSatId4 = aryData(15)
                MyGSV.StrElevation4 = aryData(16)
                MyGSV.StrAzimuth4 = aryData(17)
                MyGSV.StrSNR4 = aryData(18)

        End Select
    End Sub

End Class

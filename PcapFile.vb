Imports System.IO

Public Class PcapFile
    Public Sub New(ByVal filepath As String)
        Using stream As Stream = System.IO.File.OpenRead(filepath)

            Using pcap As BinaryReader = New BinaryReader(stream)
                Dim magic As UInteger = pcap.ReadUInt32()
                Me.Major = pcap.ReadUInt16()
                Me.Minor = pcap.ReadUInt16()
                Me.Zone = pcap.ReadInt32()
                Me.SigFig = pcap.ReadUInt32()
                Me.MaxLength = pcap.ReadUInt32()
                Me.Network = pcap.ReadUInt32()
                Me.Packets = New List(Of Packet)()

                While pcap.BaseStream.Position < pcap.BaseStream.Length
                    Me.Packets.Add(PacketFactory.create(pcap))
                End While
            End Using
        End Using
    End Sub

    Public Property Packets As IList(Of Packet)
    Public Property Major As Integer
    Public Property Minor As Integer
    Public Property Zone As Integer
    Public Property SigFig As UInteger
    Public Property MaxLength As UInteger
    Public Property Network As UInteger

End Class

Public Class Packet
    Public Sub New(ByVal pcap As BinaryReader)
        Me.TimestampSeconds = pcap.ReadUInt32()
        Me.TimestampMilliseconds = pcap.ReadUInt32()
        Me.OctetLength = pcap.ReadUInt32()
        Me.Length = pcap.ReadUInt32()
        Me.Data = New Byte(Me.OctetLength - 1) {}

        For i As UInteger = 0 To Me.OctetLength - 1
            Me.Data(i) = pcap.ReadByte()
        Next
    End Sub

    Public Property TimestampSeconds As UInteger
    Public Property TimestampMilliseconds As UInteger
    Public Property OctetLength As UInteger
    Public Property Length As UInteger
    Public Property Data As Byte()
End Class

Module PacketFactory
    Function create(ByVal pcap As BinaryReader) As Packet
        Dim pkt As Packet = New Packet(pcap)
        Return pkt
    End Function
End Module

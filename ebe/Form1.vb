Imports Emgu.CV
Imports Emgu.CV.CvEnum
Imports Emgu.CV.Structure
Imports Emgu.CV.UI
Imports AForge.Video.DirectShow
Imports System.IO

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EnumerateVideoDevices()
    End Sub
    Sub adsd()
        On Error Resume Next
        Dim sSource = "C:\image.jpg"
        Dim sTarget = "C:\scr\image.jpg"
        File.Copy(sSource, sTarget, True)
        Dim face_cascade = New CascadeClassifier("haarcascade_frontalface_default.xml")
        Dim img = CvInvoke.Imread("C:\scr\image.jpg", LoadImageType.Color)


        Dim gray = New Mat()
        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray)
        Dim faces = face_cascade.DetectMultiScale(gray, scaleFactor:=1.1, minNeighbors:=4, minSize:=New Size(30, 30))

        For Each Facsce In faces
            CvInvoke.Rectangle(img, Facsce, New MCvScalar(255, 0, 0), 2)
        Next


        PictureBox1.Image = img.Bitmap
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        On Error Resume Next
        Dim sSource = "C:\image.jpg"
        Dim sTarget = "C:\scr\image.jpg"
        File.Copy(sSource, sTarget, True)


        Dim face_cascade = New CascadeClassifier("haarcascade_frontalface_default.xml")
        Dim img   = CvInvoke.Imread("C:\scr\image.jpg", LoadImageType.Color)


        Dim gray = New Mat()
        CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray)
        Dim faces = face_cascade.DetectMultiScale(gray, scaleFactor:=1.1, minNeighbors:=4, minSize:=New Size(30, 30))

        For Each Facsce In faces
            CvInvoke.Rectangle(img, Facsce, New MCvScalar(255, 0, 0), 2)
        Next


        PictureBox1.Image = img.Bitmap

    End Sub


    Private videoDevices As FilterInfoCollection
    Private videoCapabilities As VideoCapabilities()

    Private videoDevice As VideoCaptureDevice

    Private Sub EnumerateVideoDevices()
        ' enumerate video devices
        videoDevices = New FilterInfoCollection(FilterCategory.VideoInputDevice)

        If videoDevices.Count <> 0 Then
            ' add all devices to combo
            For Each device As FilterInfo In videoDevices
                ComboBoxSources.Items.Add(device.Name)
            Next
        Else
            ComboBoxSources.Items.Add("No DirectShow devices found")
        End If

        ComboBoxSources.SelectedIndex = 0
    End Sub

    Private Sub EnumerateVideoModes(device As VideoCaptureDevice)
        ' get resolutions for selected video source
        Me.Cursor = Cursors.WaitCursor

        ComboBoxModes.Items.Clear()

        Try
            videoCapabilities = videoDevice.VideoCapabilities

            For Each capabilty As VideoCapabilities In videoCapabilities
                If Not ComboBoxModes.Items.Contains(capabilty.FrameSize) Then
                    ComboBoxModes.Items.Add(capabilty.FrameSize)
                End If
            Next

            If videoCapabilities.Length = 0 Then
                ComboBoxModes.Items.Add("Not supported")
            End If

            ComboBoxModes.SelectedIndex = 0
        Finally
            Me.Cursor = Cursors.[Default]
        End Try

    End Sub

    Private Sub frmCamera_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ComboBoxSources_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxSources.SelectedIndexChanged
        If videoDevices.Count <> 0 Then
            videoDevice = New VideoCaptureDevice(videoDevices(ComboBoxSources.SelectedIndex).MonikerString)
            EnumerateVideoModes(videoDevice)
        End If
    End Sub

    Private Sub ButtonStart_Click(sender As Object, e As EventArgs) Handles ButtonStart.Click
        CameraStart()
    End Sub

    Private Sub ButtonStop_Click(sender As Object, e As EventArgs) Handles ButtonStop.Click
        CameraStop()
    End Sub

    Private Sub CameraStart()
        If videoDevice IsNot Nothing Then
            If (videoCapabilities IsNot Nothing) AndAlso (videoCapabilities.Length <> 0) Then
                videoDevice.DesiredFrameSize = DirectCast(ComboBoxModes.SelectedItem, Size)
            End If


            VideoSourcePlayer1.VideoSource = videoDevice
            VideoSourcePlayer1.Start()
        End If
    End Sub

    Private Sub CameraStop()
        If VideoSourcePlayer1.VideoSource IsNot Nothing Then
            ' stop video device
            VideoSourcePlayer1.SignalToStop()
            VideoSourcePlayer1.WaitForStop()
            VideoSourcePlayer1.VideoSource = Nothing
        End If
    End Sub

    Private Sub VideoSourcePlayer1_NewFrame(sender As Object, ByRef image As Bitmap) Handles VideoSourcePlayer1.NewFrame
        On Error Resume Next
        ' add overlay
        Dim g As Graphics = Graphics.FromImage(image)




        Dim SaveImagePath = "C:\"
        Dim SaveImageName = "image.jpg"
        Dim SaveImage As New Bitmap(image)


        SaveImage.Save(SaveImagePath + SaveImageName, Imaging.ImageFormat.Jpeg)
        SaveImage.Dispose()



        ' g.DrawString("Augmented reality?", New Font("Arial", 16), Brushes.Black, New Rectangle(10, 10, 200, 50))
        g.Dispose()
        adsd()
    End Sub
End Class

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace winform_open_cv_sharp_camera {
  public partial class Form1 : Form {

    VideoCapture capture;
    Mat frame;
    Bitmap image;
    private Thread camera;
    int isCameraRunning = 0;

    public Form1() {
      InitializeComponent();
    }

    private void cameraOpen_Click(object sender, EventArgs e) {
      try {
        if (cameraOpen.Text.Equals("打开相机")) {
          CaptureCamera();
          cameraOpen.Text = "关闭相机";
          isCameraRunning = 1;
        } else {
          capture.Release();
          cameraOpen.Text = "打开相机";
          isCameraRunning = 0;
        }
      } catch (NullReferenceException exception) {
        MessageBox.Show(exception.Message);
      }
    }

    private void CaptureCamera() {
      camera = new Thread(new ThreadStart(CaptureCameraCallback));
      camera.Start();
    }
    private void CaptureCameraCallback() {
      frame = new Mat();
      capture = new VideoCapture();
      capture.Open(0);
      while (isCameraRunning == 1) {
        bool read_success = capture.Read(frame);
        if (!read_success) {
          MessageBox.Show("无法读取摄像头的帧！！！", "提示：");
        } else {    //防止状态切换太快，读到空值
          if (frame.Height == 0) continue;
          image = BitmapConverter.ToBitmap(frame);
          pictureBox1.Image = image;
          image = null;
        }
        //Cv2.WaitKey(20);
      }

    }
  }
}

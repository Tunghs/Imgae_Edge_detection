using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Labeling
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static string _Path = null;

        private string GetFileName(string path)
        {
            string[] dirArray = path.Split(new string[] { @"\" }, StringSplitOptions.None);
            int num = dirArray.Length - 1;

            return dirArray[num];
        }

        private string GetFileDirUrl(string path)
        {
            string[] dirArray = path.Split(new string[] { @"\" }, StringSplitOptions.None);

            int num = dirArray.Length - 1;

            string dirUrl = path.Replace(@"\" + dirArray[num], "");

            return dirUrl;
        }

        private void OpenFileDialog()
        {
            try
            {
                filePathTxt.Text = string.Empty;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = GetFileName(openFileDialog.FileName);
                    filePathTxt.Text = filename;
                }
                
                _Path = openFileDialog.FileName;
            }
            catch (Exception e)
            {
                MessageBox.Show("취소하셨습니다");
            }
        }

        private void EdgeDetection(string path)
        {
            Mat src = new Mat(path, ImreadModes.Grayscale);
            Mat dst = src.Canny(50, 200);

            // 흑백 반전
            Mat inv = new Mat();
            Cv2.BitwiseNot(dst, inv);

            // 저장
            string saveDir = GetFileDirUrl(path) + @"\detection";
            string saveDirPath = GetFileDirUrl(path) + @"\detection" + GetFileName(path);


            DirectoryInfo dir = new DirectoryInfo(saveDir);
            if (dir.Exists == false)
                dir.Create();

            Cv2.ImWrite(saveDirPath, inv);

            MessageBox.Show("성공");
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
        }

        private void runBtn_Click(object sender, RoutedEventArgs e)
        {
            EdgeDetection(_Path);
        }
    }
}

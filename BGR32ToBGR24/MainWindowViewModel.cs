using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using libSevenTools.DataBinding;
using Microsoft.WindowsAPICodePack.Dialogs;
using OpenCvSharp.CPlusPlus;
using OpenCvSharp.Extensions;

namespace BGR32ToBGR24
{
    class MainWindowViewModel : BindableBase
    {
        private string _TargetPath;
        public string TargetPath
        {
            get { return _TargetPath; }
            set { SetProperty<string>(ref _TargetPath, value); }
        }

        public bool? TargetPathValid { get; set; }

        private string _ProgressText;
        public string ProgressText
        {
            get { return _ProgressText; }
            set { SetProperty<string>(ref _ProgressText, value); }
        }

        private bool _IsOverwriting;
        public bool IsOverwriting
        {
            get { return _IsOverwriting; }
            set { SetProperty<bool>(ref _IsOverwriting, value); }
        }

        private int AllBMPCount;

        internal void OpenFileDialog()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TargetPath = dialog.FileName;
            }
        }

        internal void DescendFromRoot()
        {
            int processed = 0;
            Descend(TargetPath, ref processed);
            MessageBox.Show(string.Format("{0}件のビットマップをBGRA->BGR変換しました．", processed));
        }


        private void Descend(string path, ref int processed)
        {
            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                Descend(directory, ref processed);
            }

            var bitmapFiles = Directory.GetFiles(path, "*.bmp");
            foreach (var bitmapFile in bitmapFiles)
            {
                try
                {
                    WriteableBitmap bitmap = LoadBitmap(bitmapFile);

                    bool willProcess = false;
                    {
                        using (Mat mat = new Mat(bitmapFile))
                        {
                            //OpenCVでは正しく読み込めないBGR32の形式であるか
                            if (bitmap.Format.Equals(PixelFormats.Bgr32) && mat.Cols == 0 && mat.Rows == 0)
                            {
                                willProcess = true;
                            }
                        }
                    }

                    if (willProcess)
                    {
                        try
                        {
                            using (Mat bgr32 = WriteableBitmapConverter.ToMat(bitmap))
                            {
                                using (Mat bgr24 = new Mat())
                                {
                                    Cv2.CvtColor(bgr32, bgr24, OpenCvSharp.ColorConversion.BgraToBgr);

                                    if (IsOverwriting)
                                    {
                                        string newFilepath = Path.GetFullPath(bitmapFile);
                                        bool saved = Cv2.ImWrite(newFilepath, bgr24);
                                    }
                                    else
                                    {
                                        string newDirpath = Path.GetDirectoryName(bitmapFile) + "\\BGR";
                                        if (!Directory.Exists(newDirpath))
                                            Directory.CreateDirectory(newDirpath);
                                        bool saved = Cv2.ImWrite(newDirpath + "\\" + Path.GetFileName(bitmapFile), bgr24);
                                    }
                                }
                            }
                        }
                        finally
                        {
                            ++processed;
                            int current = processed;
                            double progression = (double)current / AllBMPCount;
                            Dispatcher.CurrentDispatcher.Invoke(() => ProgressText = string.Format("{0:#,0}/{1:#,0} {2:0.00%}", current, AllBMPCount, progression));
                        }
                    }
                }
                finally
                {
                    GC.Collect();
                }
            }
        }

        private WriteableBitmap LoadBitmap(string bitmapFile)
        {
            using (FileStream fs = new FileStream(bitmapFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                BitmapDecoder decorder = BitmapDecoder.Create(fs, BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.Default);

                return new WriteableBitmap(decorder.Frames[0]);
            }
        }

        internal void ScoutFromRoot()
        {
            if (TargetPath == null) return;
            AllBMPCount = Scout(TargetPath);
            Dispatcher.CurrentDispatcher.Invoke(() => ProgressText = string.Format("{0}件のビットマップが見つかりました．", AllBMPCount));
        }

        internal int Scout(string path)
        {
            int bmpFileCount = 0;
            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories)
            {
                bmpFileCount += Scout(directory);
            }

            var bitmapFiles = Directory.GetFiles(path, "*.bmp");
            foreach (var bitmapFile in bitmapFiles)
            {
                try
                {
                    WriteableBitmap bitmap = new WriteableBitmap(new BitmapImage(new Uri(bitmapFile)));
                    using (Mat mat = new Mat(bitmapFile))
                    {
                        if (bitmap.Format.Equals(PixelFormats.Bgr32) && mat.Cols == 0 && mat.Rows == 0)
                        {
                            //読み込みできないBGR32
                            ++bmpFileCount;
                        }
                        else
                        {
                            //処理対象外
                        }
                    }
                }
                finally
                {
                    GC.Collect();
                }
            }
            return bmpFileCount;
        }
    }
}

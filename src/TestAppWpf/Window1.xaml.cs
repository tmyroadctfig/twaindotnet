using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using Microsoft.Win32;
using TwainDotNet;
using TwainDotNet.TwainNative;
using TwainDotNet.Wpf;
using TwainDotNet.Win32;

namespace TestAppWpf
{
    public partial class Window1 : Window
    {
        private static AreaSettings AreaSettings = new AreaSettings(Units.Centimeters, 0.1f, 5.7f, 0.1F + 2.6f, 5.7f + 2.6f);

        private Twain _twain;
        private ScanSettings _settings;

        private Bitmap resultImage;

        public Window1()
        {
            InitializeComponent();

            Loaded += delegate
            {
                _twain = new Twain(new WpfWindowMessageHook(this));
                _twain.TransferImage += delegate(Object sender, TransferImageEventArgs args)
                {
                    if (args.Image != null)
                    {
                        resultImage = args.Image;
                        IntPtr hbitmap = new Bitmap(args.Image).GetHbitmap();
                        MainImage.Source = Imaging.CreateBitmapSourceFromHBitmap(
                                hbitmap,
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                        Gdi32Native.DeleteObject(hbitmap);
                    }
                };
                _twain.ScanningComplete += delegate
                {
                    IsEnabled = true;
                };

                var sourceList = _twain.SourceNames;
                ManualSource.ItemsSource = sourceList;

                if (sourceList != null && sourceList.Count > 0)
                    ManualSource.SelectedItem = sourceList[0];
            };
        }

        private void OnSelectSourceButtonClick(object sender, RoutedEventArgs e)
        {
            _twain.SelectSource();
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            _settings = new ScanSettings
                {
                    UseDocumentFeeder = UseAdfCheckBox.IsChecked,
                    ShowTwainUI = UseUICheckBox.IsChecked ?? false,
                    ShowProgressIndicatorUI = ShowProgressCheckBox.IsChecked,
                    UseDuplex = UseDuplexCheckBox.IsChecked,
                    Resolution = (BlackAndWhiteCheckBox.IsChecked ?? false)
                                     ? ResolutionSettings.Fax
                                     : ResolutionSettings.ColourPhotocopier,
                    Area = !(GrabAreaCheckBox.IsChecked ?? false) ? null : AreaSettings,
                    ShouldTransferAllPages = true,
                    Rotation = new RotationSettings
                        {
                            AutomaticRotate = AutoRotateCheckBox.IsChecked ?? false,
                            AutomaticBorderDetection = AutoDetectBorderCheckBox.IsChecked ?? false
                        }
                };

            try
            {
                if (SourceUserSelected.IsChecked == true)
                    _twain.SelectSource(ManualSource.SelectedItem.ToString());
                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                MessageBox.Show(ex.Message);
            }

            IsEnabled = true;
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (resultImage != null)
            {
                var saveFileDialog = new SaveFileDialog();
                if (saveFileDialog.ShowDialog() == true)
                    resultImage.Save(saveFileDialog.FileName);
            }
        }
    }
}

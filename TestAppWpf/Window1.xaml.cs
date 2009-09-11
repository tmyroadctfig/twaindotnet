using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwainDotNet;
using System.Windows.Interop;
using Microsoft.Win32;
using TwainDotNet.Wpf;
using System.IO;

namespace TestAppWpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Twain _twain;
        ScanSettings _settings;

        public Window1()
        {
            InitializeComponent();

            Loaded += delegate
            {
                _twain = new Twain(new WpfWindowMessageHook(this));
                _twain.ScanningComplete += delegate
                {
                    IsEnabled = true;

                    if (_twain.Images.Count > 0)
                    {
                        image1.Source = Imaging.CreateBitmapSourceFromHBitmap(
                                new System.Drawing.Bitmap(_twain.Images[0]).GetHbitmap(),
                                IntPtr.Zero,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions());
                    }
                };

                var sourceList = _twain.SourceNames;
                ManualSource.ItemsSource = sourceList;

                if (sourceList != null && sourceList.Count > 0)
                {
                    ManualSource.SelectedItem = sourceList[0];
                }
            };
        }

        private void selectSourceButton_Click(object sender, RoutedEventArgs e)
        {
            _twain.SelectSource();
        }

        private void scanButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            _settings = new ScanSettings()
            {
                UseDocumentFeeder = useAdfCheckBox.IsChecked == true,
                ShowTwainUI = useTwainUICheckBox.IsChecked == true
            };

            try
            {
                if (SourceUserSelected.IsChecked == true)
                {
                    _twain.SelectSource(ManualSource.SelectedItem.ToString());
                }

                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                MessageBox.Show(ex.Message);
                IsEnabled = true;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (image1.Source != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();

                if (sfd.ShowDialog() == true)
                {
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource) image1.Source));

                    using (FileStream stream = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                    {
                        encoder.Save(stream);
                    }
                }
            }
        }
    }
}

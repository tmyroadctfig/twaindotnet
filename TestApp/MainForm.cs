using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TwainDotNet;
using System.IO;
using TwainDotNet.WinFroms;

namespace TestApp
{
    public partial class MainForm : Form
    {
        Twain _twain;
        ScanSettings _settings;

        public MainForm()
        {
            InitializeComponent();

            _twain = new Twain(new WinFormsWindowMessageHook(this));
            _twain.ScanningComplete += delegate
            {
                Enabled = true;

                if (_twain.Images.Count > 0)
                {
                    pictureBox1.Image = _twain.Images[0];
                    _twain.Images.Clear();

                    widthLabel.Text = "Width: " + pictureBox1.Image.Width;
                    heightLabel.Text = "Height: " + pictureBox1.Image.Height;
                }
            };
        }

        private void selectSource_Click(object sender, EventArgs e)
        {
            _twain.SelectSource();
        }

        private void scan_Click(object sender, EventArgs e)
        {
            Enabled = false;

            _settings = new ScanSettings()
            {
                UseDocumentFeeder = useAdfCheckBox.Checked,
                ShowTwainUI = useUICheckBox.Checked,
                Resolution = blackAndWhiteCheckBox.Checked
                    ? ResolutionSettings.Fax
                    : ResolutionSettings.ColourPhotocopier
            };

            try
            {
                _twain.StartScanning(_settings);
            }
            catch (TwainException ex)
            {
                MessageBox.Show(ex.Message);
                Enabled = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image.Save(sfd.FileName);
                }
            }
        }

        private void diagnostics_Click(object sender, EventArgs e)
        {
            var diagnostics = new Diagnostics(new WinFormsWindowMessageHook(this));
        }
    }
}

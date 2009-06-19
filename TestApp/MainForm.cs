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

namespace TestApp
{
    public partial class MainForm : Form
    {
        Twain _twain;
        ScanSettings _settings;

        public MainForm()
        {
            InitializeComponent();

            _twain = new Twain(Handle);
            _twain.ScanningComplete += delegate
            {
                Enabled = true;

                if (_twain.Bitmaps.Count > 0)
                {
                    pictureBox1.Image = _twain.Bitmaps[0];
                }
            };
        }

        private void selectSource_Click(object sender, EventArgs e)
        {
            _twain.Select();
        }

        private void scan_Click(object sender, EventArgs e)
        {
            Enabled = false;

            _settings = new ScanSettings()
            {
                UseDocumentFeeder = useAdfCheckBox.Checked,
                ShowTwainUI = useUICheckBox.Checked
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
    }
}

using System;
using System.Windows.Forms;

namespace avr_memusage
{
    public partial class MainForm : Form
    {
        private string _avr_objdump;
        private string _avr_elffile;

        public MainForm()
        {
            InitializeComponent();
            _avr_objdump = Properties.Settings.Default._avr_objdump;
            if (!System.IO.File.Exists(_avr_objdump))
                _avr_objdump = "";
            
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_avr_objdump))
            {
                openFileDialog1.Title = "Choose the file avr-objdump.exe";
                openFileDialog1.FileName = "avr-objdump.exe";
                openFileDialog1.FilterIndex = 1;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _avr_objdump = openFileDialog1.FileName;
                    tb_FilePath.Text = _avr_objdump;
                    Properties.Settings.Default._avr_objdump = _avr_objdump;
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                openFileDialog1.Title = "Choose a file *.elf";
                openFileDialog1.FileName = "";
                openFileDialog1.FilterIndex = 2;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _avr_elffile = openFileDialog1.FileName;
                    tb_FilePath.Text = _avr_elffile;
                    tree1.LoadFromFile(_avr_objdump, _avr_elffile);
                }
            }
        }
    }
}

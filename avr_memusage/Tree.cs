using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using Aga.Controls.Tree;


namespace avr_memusage
{
    public partial class Tree : UserControl
    {
        private TreeModel _model;

        public Tree()
        {
            InitializeComponent();

            _model = new TreeModel();
            _tree.Model = _model;

            //LoadFromFile();

        }

        public void LoadFromFile(string objdumpPath, string elfPath)
        {
            //string elfPath = @"C:\Work\arduino\Elements\project\project.elf";
            //string objdumpPath = @"C:\devutils\Arduino\hardware\tools\avr\bin\avr-objdump.exe";

            List<SizeUsage> sizes = new List<SizeUsage>();
            Process objdump = new Process();
            objdump.StartInfo.FileName = objdumpPath;
            objdump.StartInfo.Arguments = @"-t " + elfPath;
            objdump.StartInfo.UseShellExecute = false;
            objdump.StartInfo.RedirectStandardOutput = true;
            objdump.Start();

            StreamReader sr = objdump.StandardOutput;

            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                int tabPos = s.IndexOf('\x9');
                if (tabPos == -1)
                    continue;

                // Type
                string lineType = s.Substring(tabPos - 5, 5);
                SizeType st;
                if (lineType == ".text")
                    st = SizeType.Text;
                else if (lineType == ".data")
                    st = SizeType.Data;
                else if (lineType == " .bss")
                    st = SizeType.BSS;
                else
                    continue;

                // Size
                int size = Int32.Parse(s.Substring(tabPos + 1, 8), System.Globalization.NumberStyles.AllowHexSpecifier);
                if (size == 0)
                    continue;

                sizes.Add(new SizeUsage(st, size, s.Substring(tabPos + 10)));
            }

            _tree.BeginUpdate();

            _model.Nodes.Clear();
            Node textnode = new SizeUsage("Program Memory");
            _model.Nodes.Add(textnode);
            Node datanode = new SizeUsage("Data / BSS");
            _model.Nodes.Add(datanode);

            sizes.Sort((a, b) => b.Size.CompareTo(a.Size));
            int textSize = 0;
            int dataSize = 0;
            foreach (SizeUsage item in sizes)
            {
                if (item.Type == SizeType.Text)
                {
                    textnode.Nodes.Add(item);
                    textSize += item.Size;
                }
                else
                {
                    datanode.Nodes.Add(item);
                    dataSize += item.Size;
                }
            }
            ((SizeUsage)textnode).Size = textSize;
            ((SizeUsage)datanode).Size = dataSize;

            _tree.EndUpdate();
        }

    }
}

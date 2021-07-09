using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace BinaryFileEditor
{
    public partial class BinaryEditor : Form
    {
        bool hasOpenedFile = false;
        bool isFileValid = false;
        private OpenFileDialog ofd;
        private SaveFileDialog sfd;
        private OverrideWarning owd;
        List<byte> binaryIn;
        string openFile;

        enum FILE_ACTION
        {
            NONE,
            ADD,
            REMOVE
        }

        public BinaryEditor()
        {
            InitializeComponent();

            DragEnter += OnDragEnter;
            textBox.DragEnter += OnDragEnter;

            DragDrop += OnDragDrop;
            textBox.DragDrop += OnDragDrop;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            switch(GetFileAction())
            {
                case FILE_ACTION.ADD:
                    binaryIn.Insert(textBox.SelectionStart - 1, Convert.ToByte(textBox.Text[textBox.SelectionStart - 1]));
                    break;
                case FILE_ACTION.REMOVE:
                    binaryIn.RemoveAt(textBox.SelectionStart);
                    break;
                default:
                    break;
            }
        }

        private FILE_ACTION GetFileAction()
        {
            if (textBox.Text.Length > binaryIn.Count)
                return FILE_ACTION.ADD;
            else if (textBox.Text.Length < binaryIn.Count)
                return FILE_ACTION.REMOVE;
            else
                return FILE_ACTION.NONE;
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd = new OpenFileDialog();

            ofd.FileOk += Ofd_FileOk;

            ofd.ShowDialog();
        }

        private void Ofd_FileOk(object sender, CancelEventArgs e)
        {
            LoadFileBinary(ofd.FileName);
            ofd.Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            owd = new OverrideWarning();

            owd.Accepted += Owd_Accepted;

            owd.ShowDialog();
        }

        private void Owd_Accepted(object sender, EventArgs e)
        {
            SaveFileBinary();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd = new SaveFileDialog();
            sfd.FileOk += sfd_saveFileOk;

            sfd.ShowDialog();
        }

        private void sfd_saveFileOk(object sender, EventArgs e)
        {
            SaveFileBinary(sfd.FileName);
            sfd.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            string[] s = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            LoadFileBinary(s[0]);
        }

        private void LoadFileBinary(string fileName)
        {
            if (!File.Exists(fileName))
                return;

            textBox.TextChanged += TextBox_TextChanged;
            hasOpenedFile = true;

            openFile = fileName;

            binaryIn = new List<byte>(File.ReadAllBytes(fileName));

            List<char> fileText = new List<char>();
            
            foreach(byte b in binaryIn)
            {
                if (b == 0)
                    fileText.Add(' ');
                else
                    fileText.Add(Convert.ToChar(b));
            }

            string textOut = new string(fileText.ToArray());
            textBox.Text = textOut;
        }

        private void SaveFileBinary()
        { 
            SaveFileBinary(openFile);
        }

        private void SaveFileBinary(string fileName)
        {
            if (!hasOpenedFile)
                return;

            File.WriteAllBytes(fileName, binaryIn.ToArray());
        }
    }
}

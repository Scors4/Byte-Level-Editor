using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BinaryFileEditor
{
    public partial class OverrideWarning : Form
    {
        public OverrideWarning()
        {
            InitializeComponent();
        }

        public event EventHandler Accepted;

        private void OverrideWarning_Load(object sender, EventArgs e)
        {
            AcceptButton.Click += AcceptButton_Click;
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Invoke(Accepted);
            Close();
            Dispose();
        }
    }
}

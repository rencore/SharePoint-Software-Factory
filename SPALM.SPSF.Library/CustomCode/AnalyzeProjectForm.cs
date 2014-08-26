using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SPALM.SPSF.Library
{
    public partial class AnalyzeProjectForm : Form
    {
        public AnalyzeProjectForm(string hiddenFile)
        {
            InitializeComponent();

            this.label1.Text = "Warning: Hidden file " + hiddenFile + " found in project.";
        }
    }
}

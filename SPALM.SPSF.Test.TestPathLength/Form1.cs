using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SPALM.SPSF.Test.TestPathLength
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      int maxlength = 0;
      string maxFile = "";
      DirectoryInfo dir = new DirectoryInfo(textBox1.Text);
      foreach (FileInfo filePath in dir.GetFiles("*.*", SearchOption.AllDirectories))
      {
        string shortFileName = filePath.FullName.Substring(textBox1.Text.Length);
        textBox2.Text += shortFileName.Length + ": " + shortFileName + Environment.NewLine;

        if(shortFileName.Length > maxlength)
        {
          maxlength = shortFileName.Length;
          maxFile = shortFileName;
        }
      }

      MessageBox.Show(maxlength + ": " + maxFile);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.ComponentModel;

namespace SPALM.SPSF.Library
{
  /// <summary>
  /// Editor that will choose a file when it is used
  /// It has properties that can be overrided 
  /// to use different filter, title and initialdirectory
  /// </summary>
  public class MultiFileChooser : UITypeEditor
  {
    public virtual string InitialDirectory
    {
      get { return "C:\\"; }
    }

    public virtual string FileFilter
    {
      get { return "All files (*.*)|*.*"; }
    }

    public virtual string Title
    {
      get { return "Please choose a file"; }
    }

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
      return UITypeEditorEditStyle.Modal;
    }

    public override object EditValue(ITypeDescriptorContext context,
      IServiceProvider provider, object value)
    {
      object svc = provider.GetService(typeof(IWindowsFormsEditorService));
      if (svc == null)
      {
        return base.EditValue(context, provider, value);
      }

      OpenFileDialog fileDialog = new OpenFileDialog();
      fileDialog.Title = Title;
      fileDialog.InitialDirectory = InitialDirectory;
      fileDialog.Filter = FileFilter;
      fileDialog.FilterIndex = 1;
      fileDialog.RestoreDirectory = true;
      fileDialog.Multiselect = true;
      if (fileDialog.ShowDialog() == DialogResult.OK)
      {
        string files = "";
        foreach (string s in fileDialog.FileNames)
        {
          files += s + ";";
        }
        return files;
      }
      else
      {
        return string.Empty;
      }
    }
  }
}

#region Using Directives

using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.RecipeFramework.Services;
using Microsoft.Practices.RecipeFramework.VisualStudio;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    [ServiceDependency(typeof(DTE))]
  public class SetProjectPropertyGroupValue : ConfigurableAction
    {
      private string _Parameter = "";
      private object _Value = "";
        private Project _Project = null;

        [Input(Required = true)]
        public string Parameter
        {
          get { return _Parameter; }
          set { _Parameter = value; }
        }

        [Input(Required = true)]
        public object Value
        {
          get { return _Value; }
          set { _Value = value; }
        }

        [Input(Required = true)]
        public Project Project
        {
          get { return _Project; }
          set { _Project = value; }
        }

        public override void Execute()
        {
          DTE dte = GetService<DTE>(true);

          try
          {
            if (_Project != null)
            {
              string newValue = "";
              if (_Value.GetType().IsArray)
              {
                Array array = (Array)_Value;
                for (int i = 0; i < array.Length; i++)
                {
                  if (newValue != "")
                  {
                    newValue += ";";
                  }
                  newValue += array.GetValue(i).ToString();
                }
              }
              else
              {
                newValue = _Value.ToString();
              }
              Helpers.SetProjectPropertyGroupValue((IVsSolution)this.GetService(typeof(SVsSolution)), dte, _Parameter, newValue);
            
            }
          }
          catch (Exception ex)
          {
            Helpers.LogMessage(dte, this, ex.ToString());
          }
        }        


        /// <summary>
        /// Removes the previously added reference, if it was created
        /// </summary>
        public override void Undo()
        {
        }
    }    
}
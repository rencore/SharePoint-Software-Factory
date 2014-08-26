using System;
using System.Text;
using Microsoft.Practices.RecipeFramework;
using EnvDTE;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Security.Cryptography;
using System.Collections;
using System.Diagnostics;
using Microsoft.Practices.RecipeFramework.Library;
using Microsoft.Practices.Common;
using Microsoft.Practices.ComponentModel;

namespace SPALM.SPSF.Library.ValueProviders
{
    [ServiceDependency(typeof(DTE))]
    public class ProcessIdValueProvider : ValueProvider
    {
      //Returns the ID of a given Process

      private string _ProcessName = "";
        public string ProcessName
        {
          get { return _ProcessName; }
          set { _ProcessName = value; }
        }

        public override bool OnBeginRecipe(object currentValue, out object newValue)
        {
          if (currentValue != null)
          {
            // Do not assign a new value, and return false to flag that 
            // we don't want the current value to be changed.
            newValue = null;
            return false;
          }

            newValue = "";

            DTE service = (DTE)this.GetService(typeof(DTE));
            
           foreach(EnvDTE.Process proc in service.Debugger.LocalProcesses)
           {
             if(proc.Name.EndsWith("OWSTIMER.EXE", StringComparison.InvariantCultureIgnoreCase))
             {
               newValue = proc.ProcessID;
               return true;
             }
           }
          
           newValue = 0;
           return true;
        }       
    }
}


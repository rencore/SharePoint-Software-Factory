using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Microsoft.Practices.WizardFramework;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing;
using Microsoft.Practices.ComponentModel;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace SPALM.SPSF.Library
{

    [ServiceDependency(typeof(IServiceProvider)), ServiceDependency(typeof(IContainer))]
    public class ConditionPanelType : ArgumentPanelTypeEditor
    {
        public ConditionPanelType() : base()
        {
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
          base.OnControlAdded(e);
          string x = e.Control.GetType().ToString();
          Control ec = e.Control;
          if (ec.GetType() == typeof(ValueEditor))
          {
            ((ValueEditor)ec).TextBox.TextChanged += new EventHandler(ConditionPanelType_TextChanged);
          }
        }

        void ConditionPanelType_TextChanged(object sender, EventArgs e)
        {
          if (this.Parent != null)
          {
            string p = this.Parent.GetType().ToString();
            string va = ((TextBox)sender).Text;
          

            int i = 0;
            WizardForm f = FindWizardForm(this);
            if(f != null)
            {
              Microsoft.WizardFramework.WizardPage nextpage = f.NextPage;
              if (f != null)
              {
                foreach (Control c in nextpage.Controls)
                {
                  if (va == "Text")
                  {
                    c.Visible = true;
                  }
                  else
                  {
                    string c1 = c.GetType().ToString();
                    if ((i == 1) || (i == 3))
                    {
                      Type t = c.GetType();
                      c.Visible = false;
                    }
                  }

                  i++;
                }
              }
            }
          }
        }

        private WizardForm FindWizardForm(Control c)
        {
          if (c.Parent != null)
          {
            if (c.Parent.GetType() == typeof(WizardForm))
            {
              return (WizardForm)c.Parent;
            }
            else
            {
              return FindWizardForm(c.Parent);
            }
          }
          return null;
        }
    }
}

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
	public class CheckBoxPanel : ArgumentPanelTypeEditor
	{
		public CheckBoxPanel() : base()
		{			
		}

		public override void EndInit()
		{
			base.EndInit();
			if (!base.IsInitializing)
			{
				
				//this.textbox.Text = base.FieldConfig.Label;
				/*base.toolTip.SetToolTip(this.listbox, base.FieldConfig.Tooltip);
				IDictionaryService service = this.GetService(typeof(IDictionaryService)) as IDictionaryService;
				if (base.FieldConfig.ReadOnly)
				{
					this.listbox.Enabled = false;
				}
				 * */
			}
		}

	}
}

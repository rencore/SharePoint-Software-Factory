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
using System.Xml;
using EnvDTE80;

#endregion

namespace SPALM.SPSF.Library.Actions
{
	[ServiceDependency(typeof(DTE))]
	public class SaveApplicationConfiguration : ConfigurableAction
	{
		[Input(Required = false)]
		public object KeyValue
		{
			get { return _KeyValue; }
			set { _KeyValue = value; }
		}
		private object _KeyValue = null;

		[Input(Required = true)]
		public string KeyName
		{
			get { return _KeyName; }
			set { _KeyName = value; }
		}
		private string _KeyName = "";


		public override void Execute()
		{
			DTE dte = GetService<DTE>(true);

			if (_KeyValue == null)
			{
				_KeyValue = "";
			}

			Helpers.SetApplicationConfigValue(dte, _KeyName, _KeyValue.ToString());

		}

		/// <summary>
		/// Removes the previously added reference, if it was created
		/// </summary>
		public override void Undo()
		{
		}
	}
}
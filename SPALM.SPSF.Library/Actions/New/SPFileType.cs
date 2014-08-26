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
using Microsoft.Practices.RecipeFramework.VisualStudio.Library.Templates;
using Microsoft.Practices.Common.Services;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ComponentModel.Design;

#endregion

namespace SPALM.SPSF.Library.Actions
{
    public enum SPFileType
    {
        FeatureManifest,
        CustomCode,
        Resources,
        TemplateFile,
        RootFile,
        ClassResource,
        ElementFile,
        GACFile,
        BINFile,
        ElementManifest,
        AppGlobalResource,
        Resource
    }
}
using System;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Practices.RecipeFramework.VisualStudio.Templates;
using EnvDTE80;
using EnvDTE;
using Microsoft.Practices.RecipeFramework;

namespace SPALM.SPSF.Library.References
{
  [Serializable]
  public class DeploymentReferenceTemplate : UnboundTemplateReference
  {
    public DeploymentReferenceTemplate(string template)
      : base(template)
    {
    }

    public override bool IsEnabledFor(object target)
    {
      try
      {
        if (target is SolutionFolder || target is Solution)
        {
          return true;
        }
        if (((Project)target).Object is SolutionFolder)
        {
          //any solution folder with name deployment
          if (((SolutionFolder)((Project)target).Object).Parent.Name == "Deployment")
          {
            return true;
          }
        }
        return false;
      }
      catch (Exception)
      {
      }
      return false;
    }

    public override string AppliesTo
    {
      get { return "Deployment Folders"; }
    }

    #region ISerializable Members

    /// <summary>
    /// Required constructor for deserialization.
    /// </summary>
    protected DeploymentReferenceTemplate(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }

    #endregion ISerializable Members
  }
}

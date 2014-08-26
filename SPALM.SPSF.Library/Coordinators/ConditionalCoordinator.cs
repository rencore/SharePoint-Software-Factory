using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using EnvDTE;
using Microsoft.Practices.ComponentModel;
using Microsoft.Practices.RecipeFramework.Services;
using System.ComponentModel.Design;
using Microsoft.Practices.RecipeFramework.Library;
using System.Diagnostics;

namespace SPALM.SPSF.Library.Coordinators
{
    [ServiceDependency(typeof(DTE))]
    [ServiceDependency(typeof(IActionExecutionService))]
    [ServiceDependency(typeof(IDictionaryService))] 
    public class ConditionalCoordinator : SitedComponent, IActionCoordinationService
    {
        // Fields
        public const string ConditionalAttributeName = "Condition";
        private DTE visualStudio;

        // Methods
        protected override void OnSited()
        {
            base.OnSited();
            this.visualStudio = base.GetService<DTE>(true);
        }

        public void Run(Dictionary<string, Microsoft.Practices.RecipeFramework.Configuration.Action> declaredActions, XmlElement coordinationData)
        {
            IActionExecutionService service = base.GetService<IActionExecutionService>(true);
            int amountCompleted = 0;
            try
            {
              foreach (Microsoft.Practices.RecipeFramework.Configuration.Action action in declaredActions.Values)
              {
                amountCompleted++;
                bool flag = (action.AnyAttr == null) || (action.AnyAttr.Length == 0);
                if (!flag)
                {
                  IDictionaryService serviceToAdapt = (IDictionaryService)this.GetService(typeof(IDictionaryService));
                  ExpressionEvaluationService service3 = new ExpressionEvaluationService();
                  flag = true;
                  foreach (XmlAttribute attribute in action.AnyAttr)
                  {
                    if (attribute.Name.Equals("Condition", StringComparison.InvariantCultureIgnoreCase))
                    {
                      try
                      {
                        flag = (bool)service3.Evaluate(attribute.Value, new ServiceAdapterDictionary(serviceToAdapt));
                      }
                      catch (Exception exception)
                      {
                        flag = false;
                        Trace.TraceWarning("InvalidConditionException", new object[] { exception.Message, exception.StackTrace });
                      }
                      break;
                    }
                  }
                }
                if (flag)
                {
                  service.Execute(action.Name);
                }
              }
            }
            finally
            {
                this.visualStudio.StatusBar.Progress(false, "", 0, 0);
            }
        }
    }
}

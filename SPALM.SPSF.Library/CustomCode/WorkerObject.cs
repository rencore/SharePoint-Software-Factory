using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPALM.SPSF.SharePointBridge;
using EnvDTE;

namespace SPALM.SPSF.Library
{
    internal class WorkerObject
    {
        public string Operation = "";
        public DTE DTE;
        public List<SharePointDeploymentJob> WSPFiles;
    }
}

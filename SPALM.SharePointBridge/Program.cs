using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Threading;
using System.Runtime.Serialization.Formatters;
using SPALM.SPSF.SharePointBridge;

namespace SPALM.SharePointBridge
{
	class Program
	{
        static string logFile = Path.Combine(GetAssemblyDirectory(), "SharePointBridge.log");

		static void Main(string[] args)
		{
            //AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            if (args.Length == 0)
            {
                try
                {
                    SharePointRemoteObject obj = new SharePointRemoteObject();
                    Console.WriteLine(obj.GetSharePointVersion());
                    
                    foreach (SharePointWebApplication s in obj.GetAllWebApplications())
                    {
                        Console.WriteLine(s.Name);
                    }                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                Console.ReadLine();
                return;
            }

            IChannel ichannel = null;
            string channelName = "";
            try
            {
                channelName = args[0];
                if (channelName == "")
                {
                    throw new Exception("No channel name as parameter available");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(logFile, ex.ToString());
                //throw new Exception("Ex1", ex);
            }

			try
			{
				BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
				serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

				System.Collections.IDictionary properties = new System.Collections.Hashtable();
				properties["name"] = channelName;
				properties["priority"] = "20";
				properties["portName"] = channelName; //"ipc://" + channelName + "/RemoteObj"

				// Create the channel. 
				ichannel = new IpcChannel(properties, clientProv, serverProv);

				//ichannel = new IpcChannel(properties, clientProv, serverProv); 
				ChannelServices.RegisterChannel(ichannel, false);

				RemotingConfiguration.RegisterWellKnownServiceType(typeof(SharePointRemoteObject), "RemoteObj", WellKnownObjectMode.Singleton);

				// keep the process alive
				TimeSpan waitTime = new TimeSpan(0, 0, 5);
				while (1 == 1)
				{
					// pump messages
					Thread.CurrentThread.Join(waitTime);
					Thread.Sleep(waitTime);
				}
			}
			catch (Exception e)
			{
                File.AppendAllText(logFile, e.ToString());
				Console.WriteLine(e.ToString());
				Console.WriteLine(e.InnerException);
			}
			Console.ReadLine();
		}

        public static string GetAssemblyDirectory()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);        
        }


		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
            File.AppendAllText(logFile, "SharePointBridge.exe: Resolve " + args.Name + Environment.NewLine);
            string assemblyFullname = args.Name;

            if(assemblyFullname.StartsWith("Microsoft.SharePoint", StringComparison.InvariantCultureIgnoreCase))
            {
                File.AppendAllText(logFile, "SharePointBridge.exe: Microsoft.SharePoint.* reqeusted" + Environment.NewLine);
            
                if(assemblyFullname.Contains("14.0.0.0"))
                {
                    assemblyFullname = assemblyFullname.Replace("14.0.0.0", "15.0.0.0");
                    File.AppendAllText(logFile, "SharePointBridge.exe: Returning assembly " + assemblyFullname + Environment.NewLine);
                    Assembly assembly = Assembly.Load(assemblyFullname);
                    File.AppendAllText(logFile, "SharePointBridge.exe: Loaded from GAC = " + assembly.GlobalAssemblyCache.ToString() + Environment.NewLine);
                    return assembly;
                }
                else if(assemblyFullname.Contains("15.0.0.0"))
                {
                    assemblyFullname = assemblyFullname.Replace("15.0.0.0", "14.0.0.0");
                    File.AppendAllText(logFile, "SharePointBridge.exe: Returning assembly " + assemblyFullname + Environment.NewLine);
                    Assembly assembly = Assembly.Load(assemblyFullname);
                    File.AppendAllText(logFile, "SharePointBridge.exe: Loaded from GAC = " + assembly.GlobalAssemblyCache.ToString() + Environment.NewLine);
                    return assembly;
                }
            }

            //search in same dir
            string firstTest = Path.Combine(GetAssemblyDirectory(), args.Name);
            if (File.Exists(firstTest))
            {
                File.AppendAllText(logFile, "SharePointBridge.exe: Returned assembly " + firstTest + Environment.NewLine);
                return Assembly.LoadFile(firstTest, Assembly.GetExecutingAssembly().Evidence);
            }

            //search in different dirs
            string[] asmName = args.Name.Split(',');
            string[] assemblySearchDirs = new string[] { Helpers.GetSharePointHive() + @"\ISAPI", @"C:\Windows\Microsoft.NET\Framework\v4.0.30319", @"C:\Windows\Microsoft.NET\Framework\v2.0.50727" };
            foreach(string assemblySearchDir in assemblySearchDirs)
            {                
			    string asmPath = Path.Combine(assemblySearchDir, asmName[0] + ".dll");

			    if (File.Exists(asmPath))
			    {
                    File.AppendAllText(logFile, "SharePointBridge.exe: Assembly " + asmName[0] + " found in " + assemblySearchDir);
				    return Assembly.LoadFile(asmPath, Assembly.GetExecutingAssembly().Evidence);
			    }
            }
            
            File.AppendAllText(logFile, "SharePointBridge.exe: No assembly found" + Environment.NewLine);

            return Assembly.Load(args.Name, Assembly.GetExecutingAssembly().Evidence);
		}		
	}
}

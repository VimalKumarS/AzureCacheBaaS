using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.ApplicationServer.Caching;
using Microsoft.WindowsAzure.Storage;
using System.Diagnostics;
//using Microsoft.WindowsAzure.Storage;

namespace ProjectCS218
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            DiagnosticMonitorConfiguration diagConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();

            var perfCounters = new List<string>
            {
                @"\Processor(_Total)\% Processor Time",
                @"\Memory\Available Mbytes",
                @"\TCPv4\Connections Established",
                @"\ASP.NET Applications(__Total__)\Requests/Sec",
                @"\Network Interface(*)\Bytes Received/sec",
                @"\Network Interface(*)\Bytes Sent/sec"
            };

            // Add perf counters to configuration
            foreach (var counter in perfCounters)
            {
                var counterConfig = new PerformanceCounterConfiguration
                {
                    CounterSpecifier = counter,
                    SampleRate = TimeSpan.FromSeconds(1.0)
                };

                diagConfig.PerformanceCounters.DataSources.Add(counterConfig);
            }

            diagConfig.PerformanceCounters.ScheduledTransferPeriod = TimeSpan.FromSeconds(20.0);

            //Windows Event Logs
            diagConfig.WindowsEventLog.DataSources.Add("System!*");
            diagConfig.WindowsEventLog.DataSources.Add("Application!*");
            diagConfig.WindowsEventLog.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0);
            diagConfig.WindowsEventLog.ScheduledTransferLogLevelFilter = LogLevel.Warning;

            //Azure Trace Logs
            diagConfig.Logs.ScheduledTransferPeriod = TimeSpan.FromSeconds(20.0);
            diagConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Warning;
            diagConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Information;
          //  diagConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            diagConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Error;
            //Crash Dumps
            CrashDumps.EnableCollection(true);

            //IIS Logs
            diagConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1.0);

            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagConfig);

            Trace.TraceInformation(RoleEnvironment.Roles.Count.ToString());
            Trace.TraceInformation("Instance {0} started.", RoleEnvironment.CurrentRoleInstance.Id);

            RoleEnvironment.Changing += new EventHandler<RoleEnvironmentChangingEventArgs>(RoleEnvironment_Changing);

           // CloudStorageAccount account = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
          // account.
        //    RoleEnvironment.GetConfigurationSettingValue("CacheServiceRole");
            
            RoleEnvironment.Changed += new EventHandler<RoleEnvironmentChangedEventArgs>(RoleEnvironment_Changed);


            //if (RoleEnvironment.IsEmulated)
            //{
            //    var traceSource = CreateTraceSource();
            //    traceSource.TraceInformation("Instance {0} started.", RoleEnvironment.CurrentRoleInstance.Id);
            //}
            return base.OnStart();
            //// To enable the AzureLocalStorageTraceListner, uncomment relevent section in the web.config  
            //DiagnosticMonitorConfiguration diagnosticConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();
            //diagnosticConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            //diagnosticConfig.Directories.DataSources.Add(AzureLocalStorageTraceListener.GetLogDirectory());

            //// For information on handling configuration changes
            //// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            //return base.OnStart();
        }

        void RoleEnvironment_Changed(object sender, RoleEnvironmentChangedEventArgs e)
        {
            //var traceSource = CreateTraceSource();
            Trace.TraceInformation("RoleEnvironment.Changed for {0}:", RoleEnvironment.CurrentRoleInstance.Id);

            foreach (RoleEnvironmentChange rec in e.Changes)
            {
                //Trace.TraceInformation(rec.ToString());
                Trace.TraceInformation(" > {0}", rec.GetType());
                RoleEnvironmentConfigurationSettingChange recsc = rec as RoleEnvironmentConfigurationSettingChange;
                Trace.TraceInformation(RoleEnvironment.GetConfigurationSettingValue(recsc.ConfigurationSettingName), "Information");
            }

            //Trace.(arg.Changes.GetType().ToString());RoleEnvironmentTopologyChange
            if (e.Changes.Any(chg => chg is RoleEnvironmentConfigurationSettingChange))
            {
                Trace.TraceInformation(RoleEnvironment.Roles.Count.ToString() + " instance count");
                RoleEnvironment.RequestRecycle();
            }
        }

        private TraceSource CreateTraceSource()
        {
            var traceSource = new TraceSource("SampleApp", SourceLevels.All);
            traceSource.Listeners.Add(
                Activator.CreateInstance(
                    Type.GetType("Microsoft.ServiceHosting.Tools.DevelopmentFabric.Runtime.DevelopmentFabricTraceListener, Microsoft.ServiceHosting.Tools.DevelopmentFabric.Runtime, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")) as TraceListener);
            return traceSource;
        }
       
        void RoleEnvironment_Changing(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (RoleEnvironment.IsEmulated)
            {
               // var traceSource = CreateTraceSource();
                Trace.TraceInformation("RoleEnvironment.Changing for {0}:", RoleEnvironment.CurrentRoleInstance.Id);

                foreach (RoleEnvironmentChange change in e.Changes)
                {
                    Trace.TraceInformation(" > {0}", change.GetType());
                }
            }
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
            
        }

        public override void Run()
        {
            base.Run();
        }
    }
}

//http://msdn.microsoft.com/en-us/library/gg433023.aspx


/*
 
 void RoleEnvironment_Changing(object sender, RoleEnvironmentChangingEventArgs e)
        {
            foreach (RoleEnvironmentChange rec in e.Changes)
            {
                RoleEnvironmentConfigurationSettingChange recsc = rec as RoleEnvironmentConfigurationSettingChange;
                if (recsc.ConfigurationSettingName.ToUpper() == "TESTSETTING")
                {
                    e.Cancel = false; //read as e.Reboot
                }
            }
        }
        //Fires when role environment has changed
        void RoleEnvironment_Changed(object sender, RoleEnvironmentChangedEventArgs e)
        {
            foreach (RoleEnvironmentChange rec in e.Changes)
            {
                RoleEnvironmentConfigurationSettingChange recsc = rec as RoleEnvironmentConfigurationSettingChange;
                if (recsc.ConfigurationSettingName.ToUpper() == "TESTSETTING")
                {
                    Trace.TraceInformation(RoleEnvironment.GetConfigurationSettingValue(recsc.ConfigurationSettingName), "Information");
                }
            }
        }
 */
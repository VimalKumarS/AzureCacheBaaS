using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using CS218AzurePerformanceMonitor;

namespace WorkerRoleCS218
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("WorkerRoleCS218 entry point called", "Information");
            System.Timers.Timer checkCPUTimer = new System.Timers.Timer();
            checkCPUTimer.Interval = 300000;  //use 300000.0 for 5 minutes
            checkCPUTimer.Elapsed += new System.Timers.ElapsedEventHandler(checkCPUTimer_Elapsed);
            checkCPUTimer.Start();
            Trace.TraceInformation("WorkerRoleCS218", "Information");
            while (true)
            {
                Thread.Sleep(10000);
                //Trace.TraceInformation("WorkerRoleCS218", "Information");
            }
        }
        void checkCPUTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TestPerfData();
        }
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            Trace.TraceInformation("WorkerRoleCS218 On Start", "Information");
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            Microsoft.WindowsAzure.CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                RoleEnvironment.Changed += (sender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                        .Any((change) => (change.ConfigurationSettingName == configName)))
                    {
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });
            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        public void TestPerfData()
        {
            // Trace.TraceInformation("Starting TestPerfData");
            Trace.TraceInformation("Starting TestPerfData", "Information");
            try
            {
                var account = Microsoft.WindowsAzure.CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
                var context = new PerformanceDataContext(account.TableEndpoint.ToString(), account.Credentials);
                var data = context.PerfData;
                DateTime dtnow = DateTime.UtcNow.AddMinutes(-30);
                DateTime dtnow2 = DateTime.UtcNow.AddMinutes(-20);
                System.Collections.Generic.List<PerformanceData> selectedData = (from d in data
                                                                                 where d.CounterName == @"\Processor(_Total)\% Processor Time"
                                                                                 && d.EventTickCount >=
                                                                                 dtnow.Ticks && d.EventTickCount <=
                                                                                 dtnow2.Ticks
                                                                                  &&  d.Role == "ProjectCS218"
                                                                                 select d).ToList<PerformanceData>();


                Trace.TraceInformation(selectedData.Count.ToString() + "  " + dtnow.Ticks + "  " + dtnow2.Ticks); ;
                
                double AvgCPU = selectedData.Where(p => p.CounterName == @"\Processor(_Total)\% Processor Time").DefaultIfEmpty().Average(p => p == null ? 0 : p.CounterValue);
                /* double AvgCPU = (from d in selectedData
                                  where d.CounterName == @"\Processor(_Total)\% Processor Time"
                                     select d.CounterValue).Average();*/

                //Trace.WriteLine("Average CPU == " + AvgCPU.ToString());
                Trace.TraceInformation("Average CPU == " + AvgCPU.ToString(), "Information Role Name");
                Trace.TraceInformation("Max Time Stamp CPU == " + selectedData.Max(item => item.Timestamp).ToString(), "Information");
                Trace.TraceInformation("Min Time Stamp CPU == " + selectedData.Min(item => item.Timestamp).ToString(), "Information");
                //test threshold
                if (AvgCPU >= 1)
                {
                    Trace.TraceInformation("Increase the instance by adding one more role" + AvgCPU.ToString(), "Information");
                    // Trace.TraceInformation(selectedData.Max(item => item.Timestamp).ToString(), "Information");
                    //Trace.TraceInformation(selectedData.Min(item => item.Timestamp).ToString(), "Information");
                    //increase instances

                    string deploymentInfo = AzureRESTMgmtHelper.GetDeploymentInfo();
                    string svcconfig = AzureRESTMgmtHelper.GetServiceConfig(deploymentInfo);
                    int InstanceCount = System.Convert.ToInt32(AzureRESTMgmtHelper.GetInstanceCount(svcconfig, "ProjectCS218"));
                    if (InstanceCount == 1)
                    {
                        InstanceCount++;
                        Trace.TraceInformation("Average CPU == " + AvgCPU.ToString(), "Information" + " Instance Count increased by 1");
                        string UpdatedSvcConfig = AzureRESTMgmtHelper.ChangeInstanceCount(svcconfig, "ProjectCS218", InstanceCount.ToString());

                        AzureRESTMgmtHelper.ChangeConfigFile(UpdatedSvcConfig);
                    }

                }
                else if (AvgCPU < 1) //50
                {
                    Trace.TraceInformation("in the AvgCPU < 5 test.");

                    string deploymentInfo = AzureRESTMgmtHelper.GetDeploymentInfo();
                    string svcconfig = AzureRESTMgmtHelper.GetServiceConfig(deploymentInfo);
                    int InstanceCount = System.Convert.ToInt32(AzureRESTMgmtHelper.GetInstanceCount(svcconfig, "ProjectCS218"));

                    if (InstanceCount > 1)
                    {
                        InstanceCount--;
                        string UpdatedSvcConfig = AzureRESTMgmtHelper.ChangeInstanceCount(svcconfig, "ProjectCS218", InstanceCount.ToString());

                        AzureRESTMgmtHelper.ChangeConfigFile(UpdatedSvcConfig);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Trace.TraceInformation(ex.StackTrace, "Information");
                Trace.WriteLine("Worker Role Error: " + ex.Message);
            }


        }
    }
}

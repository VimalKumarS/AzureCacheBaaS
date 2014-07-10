<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CS218Project2" generation="1" functional="0" release="0" Id="a1f976b2-b905-453d-a8d8-69b8806ad93d" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CS218Project2Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ProjectCS218:Endpoint1" protocol="https">
          <inToChannel>
            <lBChannelMoniker name="/CS218Project2/CS218Project2Group/LB:ProjectCS218:Endpoint1" />
          </inToChannel>
        </inPort>
        <inPort name="ProjectCS218:Endpoint2" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CS218Project2/CS218Project2Group/LB:ProjectCS218:Endpoint2" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Certificate|ProjectCS218:CS218Project" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapCertificate|ProjectCS218:CS218Project" />
          </maps>
        </aCS>
        <aCS name="Certificate|WorkerRoleCS218:CS218Project" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapCertificate|WorkerRoleCS218:CS218Project" />
          </maps>
        </aCS>
        <aCS name="ProjectCS218:CacheServiceRole" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapProjectCS218:CacheServiceRole" />
          </maps>
        </aCS>
        <aCS name="ProjectCS218:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapProjectCS218:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </maps>
        </aCS>
        <aCS name="ProjectCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapProjectCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ProjectCS218:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapProjectCS218:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="ProjectCS218Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapProjectCS218Instances" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleCS218:CS218Project" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapWorkerRoleCS218:CS218Project" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleCS218:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapWorkerRoleCS218:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapWorkerRoleCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleCS218Instances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CS218Project2/CS218Project2Group/MapWorkerRoleCS218Instances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ProjectCS218:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/Endpoint1" />
          </toPorts>
        </lBChannel>
        <lBChannel name="LB:ProjectCS218:Endpoint2">
          <toPorts>
            <inPortMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/Endpoint2" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapCertificate|ProjectCS218:CS218Project" kind="Identity">
          <certificate>
            <certificateMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/CS218Project" />
          </certificate>
        </map>
        <map name="MapCertificate|WorkerRoleCS218:CS218Project" kind="Identity">
          <certificate>
            <certificateMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218/CS218Project" />
          </certificate>
        </map>
        <map name="MapProjectCS218:CacheServiceRole" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/CacheServiceRole" />
          </setting>
        </map>
        <map name="MapProjectCS218:Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" />
          </setting>
        </map>
        <map name="MapProjectCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapProjectCS218:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapProjectCS218Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218Instances" />
          </setting>
        </map>
        <map name="MapWorkerRoleCS218:CS218Project" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218/CS218Project" />
          </setting>
        </map>
        <map name="MapWorkerRoleCS218:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218/DataConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRoleCS218:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRoleCS218Instances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218Instances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ProjectCS218" generation="1" functional="0" release="0" software="F:\SJSU\cs218\CS218Practice\RestService\CS218Project2\csx\Release\roles\ProjectCS218" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="https" portRanges="443">
                <certificate>
                  <certificateMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/CS218Project" />
                </certificate>
              </inPort>
              <inPort name="Endpoint2" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="CacheServiceRole" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Caching.ClientDiagnosticLevel" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ProjectCS218&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ProjectCS218&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Endpoint2&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRoleCS218&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[20000,20000,20000]" defaultSticky="false" kind="Directory" />
              <resourceReference name="ProjectCS218.svclog" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0CS218Project" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218/CS218Project" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="CS218Project" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218Instances" />
            <sCSPolicyUpdateDomainMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="WorkerRoleCS218" generation="1" functional="0" release="0" software="F:\SJSU\cs218\CS218Practice\RestService\CS218Project2\csx\Release\roles\WorkerRoleCS218" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="CS218Project" defaultValue="" />
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRoleCS218&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ProjectCS218&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;e name=&quot;Endpoint2&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;WorkerRoleCS218&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
            <storedcertificates>
              <storedCertificate name="Stored0CS218Project" certificateStore="My" certificateLocation="System">
                <certificate>
                  <certificateMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218/CS218Project" />
                </certificate>
              </storedCertificate>
            </storedcertificates>
            <certificates>
              <certificate name="CS218Project" />
            </certificates>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218Instances" />
            <sCSPolicyUpdateDomainMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218UpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/CS218Project2/CS218Project2Group/WorkerRoleCS218FaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ProjectCS218UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="WorkerRoleCS218UpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ProjectCS218FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="WorkerRoleCS218FaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ProjectCS218Instances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="WorkerRoleCS218Instances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="19f60ce0-eeb3-45de-9364-b3a88ec2ab5e" ref="Microsoft.RedDog.Contract\ServiceContract\CS218Project2Contract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="646401dc-d05c-44e6-93ac-20fc9025d50f" ref="Microsoft.RedDog.Contract\Interface\ProjectCS218:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218:Endpoint1" />
          </inPort>
        </interfaceReference>
        <interfaceReference Id="fb21b337-2733-409c-8793-c9497f87c2d7" ref="Microsoft.RedDog.Contract\Interface\ProjectCS218:Endpoint2@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/CS218Project2/CS218Project2Group/ProjectCS218:Endpoint2" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
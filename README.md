AzureCacheBaaS
==============

Azure Cache service for Mobile Application [Backend as a Service]

Traditionally mobile cache storage is normally held on the device requesting the information. 
However there is a limitation due to size and cache validation.  Some of the older mobile 
devices are unable to retain its cache after a power cycle [19].  In Chapter 5 the 
implementation of the DCIM was discussed where the cache was implemented on any caching 
nodes which are mobile devices in the network.  The query directories are also represented 
as mobile device on the network.  However this is dependent on the CN and QD to have constant
network connection.  Furthermore, this solution also has a limited cache size since it is 
using mobile devices.  In this project we propose to put the cache and the query directories 
in a cloud instance, to take advantage of the redundancy and elasticity of the cloud.


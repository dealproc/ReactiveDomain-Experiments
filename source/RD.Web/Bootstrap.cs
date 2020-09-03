using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RD.Core.Services.Device;
using RD.Core.Services.Project;
using ReactiveDomain;
using ReactiveDomain.EventStore;
using ReactiveDomain.Foundation;
using ReactiveDomain.Foundation.StreamStore;
using ReactiveDomain.Messaging.Bus;

namespace RD.Web
{
    public class Bootstrap
    {

        public IDispatcher MainBus;
#pragma warning disable 618
        private readonly EventStoreLoader _esLoader = new EventStoreLoader();
#pragma warning restore 618
        public IStreamStoreConnection ESConn;
        public ICorrelatedRepository Repository;
        public void Configure() {
            
            MainBus = new Dispatcher("main Bus");
            

            //We assume a default ES instance is running here
            //consider a choco install 'choco install eventstore-oss --version=5.0.8'
            //start eventstore with command line options 'EventStore.ClusterNode.exe --config=config.yaml'
            //contents of config.yaml
            /*
---
StatsPeriodSec: 2400
RunProjections: ALL
StartStandardProjections: true
---
             */
            _esLoader.Connect(new UserCredentials("admin", "changeit"), IPAddress.Parse("127.0.0.1"), tcpPort: 1113);
            ESConn = _esLoader.Connection;
           
            Repository = new CorrelatedStreamStoreRepository(
                new PrefixedCamelCaseStreamNameBuilder("RD.Web"),
                ESConn,
                new JsonMessageSerializer(),
                repo => new ReadThroughAggregateCache(repo));
            var deviceService = new DeviceService(MainBus,Repository);
         
            
            //todo: register the created bus here with DI if that's the goal
        }
    }
}

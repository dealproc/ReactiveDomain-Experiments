﻿using System;
using System.Collections.Generic;
using System.Net;
using ReactiveDomain.Transport.Framing;
using ReactiveDomain.Messaging.Bus;
using ReactiveDomain.Transport.Serialization;

namespace ReactiveDomain.Transport
{
    public class TcpBusServerSide : TcpBusSide
    {
        private readonly TcpServerListener _commandPortListener;

        public TcpBusServerSide(
            IPAddress hostIp,
            int commandPort,
            IDispatcher messageBus,
            IMessageSerializer messageSerializer = null)
            : base(hostIp, commandPort, messageBus, messageSerializer)
        {
           
            Log.Info("ConfigureTcpListener(" + CommandEndpoint.AddressFamily + ", " + CommandEndpoint + ") entered.");
            
            var listener = new TcpServerListener(CommandEndpoint);

            listener.StartListening((endPoint, socket) =>
            {
               var conn = Transport.TcpConnection.CreateAcceptedTcpConnection(Guid.NewGuid(), endPoint, socket, verbose: true);

                LengthPrefixMessageFramer framer = new LengthPrefixMessageFramer();
                framer.RegisterMessageArrivedCallback(TcpMessageArrived);

                Action<ITcpConnection, IEnumerable<ArraySegment<byte>>> callback = null;
                callback = (x, data) =>
                {
                    try
                    {
                        framer.UnFrameData(data);
                    }
                    catch (PackageFramingException exc)
                    {
                        Log.ErrorException(exc, "LengthPrefixMessageFramer.UnFrameData() threw an exception:");
                        // SendBadRequestAndClose(Guid.Empty, string.Format("Invalid TCP frame received. Error: {0}.", exc.Message));
                        return;
                    }
                    conn.ReceiveAsync(callback);
                };
                conn.ReceiveAsync(callback);
                TcpConnection.Add(conn);
            }, "Standard");
            Log.Info("ConfigureTcpListener(" + CommandEndpoint.AddressFamily + ", " + CommandEndpoint + ") successfully constructed TcpServerListener.");
            _commandPortListener = listener;
        }
    }
}

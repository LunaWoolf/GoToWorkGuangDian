// OSC Jack - Open Sound Control plugin for Unity
// https://github.com/keijiro/OscJack

using UnityEngine;
using System;
using System.Reflection;

namespace extOSC
{
  
    public class OSCSender : MonoBehaviour
    {
        public void Update()
        {
            // Creating a transmitter.
            var transmitter = gameObject.AddComponent<OSCTransmitter>();

            // Set remote host address.
            transmitter.RemoteHost = "127.0.0.1";

            // Set remote port;
            transmitter.RemotePort = 7001;

            // Create message
            var message = new OSCMessage("/message/address");

            // Populate values.
            message.AddValue(OSCValue.String("Hello, world!"));
            message.AddValue(OSCValue.Float(1337f));

            // Send message
            transmitter.Send(message);

        }
      
    }
}

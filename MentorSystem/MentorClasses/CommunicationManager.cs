using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace MentorSystem.MentorClasses
{
    class CommunicationManager
    {
        Windows.Networking.Sockets.StreamSocketListener socketListener;
        public bool Connected = false;
        public bool Except = false;

        //Class constructor
        public CommunicationManager()
        {
            // while (true)
            //{
            InitConnection();
            //}

        }

        //Gets the virtual annotation selected flag
        public async void InitConnection()
        {
            try
            {
                //Create a StreamSocketListener to start listening for TCP connections.
                socketListener = new Windows.Networking.Sockets.StreamSocketListener();

                //Hook up an event handler to call when connections are received.
                socketListener.ConnectionReceived += SocketListener_ConnectionReceived;

                //Start listening for incoming TCP connections on the specified port. You can specify any port that' s not currently in use.
                await socketListener.BindServiceNameAsync("8989");
            }
            catch (Exception e)
            {
                //Handle exception.
                Debug.WriteLine("Not connected");
            }
        }

        private async void SocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender,
    Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            Debug.WriteLine("Entered here");
            //Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();


            Connected = true;
            /*
            //Send the line back to the remote client.
            Stream outStream = args.Socket.OutputStream.AsStreamForWrite();
            StreamWriter writer = new StreamWriter(outStream);
            await writer.WriteLineAsync(request);
            await writer.FlushAsync();*/
        }
    }
}

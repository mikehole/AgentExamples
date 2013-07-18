using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using System.IO;
using AgentExamples.Model;

namespace AgentExamples.WindowsPhoneDemoApp.Examples
{
    public partial class Objects : PhoneApplicationPage
    {
        private StreamSocket _socket = null;

        private DataWriter _dataWriter = null;

        public Objects()
        {
            InitializeComponent();
        }

        private async Task<bool> SetupDeviceConn()
        {
            //Connect to your paired host PC using BT + StreamSocket (over RFCOMM)
            PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = "";

            var devices = await PeerFinder.FindAllPeersAsync();

            if (devices.Count == 0)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            var peerInfo = devices.FirstOrDefault(c => c.DisplayName.Contains(TxtDevice.Text));
            if (peerInfo == null)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            _socket = new StreamSocket();

            //"{00001101-0000-1000-8000-00805f9b34fb}" - is the GUID for the serial port service.
            await _socket.ConnectAsync(peerInfo.HostName, "{00001101-0000-1000-8000-00805f9b34fb}");

            return true;
        }

        private async void CmdDoStuff_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (await SetupDeviceConn())
            {
                CmdDoStuff.IsEnabled = false;
                CmdSendStuff.IsEnabled = true;
            }
        }

        private void CmdSendStuff_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //send the text to the agent app with a trailing byte of zero st that the app knows it 
            //has all of the current data.

            var objectSender = new MicroSerialization.Pcl.ObjectSerializer<ToastMessage>( WindowsRuntimeStreamExtensions.AsStreamForWrite( _socket.OutputStream ));

            objectSender.SaveToStream(new ToastMessage() { Id = 1, Message = "Hello Mike" });

        }



    }
}
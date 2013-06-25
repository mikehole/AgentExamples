using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace AgentExamples.WindowsPhoneDemoApp.Examples
{
    public partial class SimpleComms : PhoneApplicationPage
    {
        private StreamSocket _socket = null;

        private DataWriter _dataWriter = null;

        public SimpleComms()
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

            _dataWriter = new DataWriter(_socket.OutputStream);

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

        private async void CmdSendStuff_OnTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //send the text to the agent app with a trailing byte of zero st that the app knows it 
            //has all of the current data.

            _dataWriter.WriteString(TxtText.Text + '\0');
            await _dataWriter.StoreAsync();
        }

    }
}
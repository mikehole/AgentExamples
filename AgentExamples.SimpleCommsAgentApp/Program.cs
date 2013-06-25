using System;
using System.IO.Ports;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;

namespace AgentExamples.SimpleCommsAgentApp
{
    public class Program
    {
        static Font _fontNinaB = null;

        static Bitmap _display;

        static System.IO.Ports.SerialPort _serial = null;

        public static void Main()
        {
            _fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);

            _serial = new SerialPort("COM1");

            //Handle the DataReceived event when data is sent to the serial port.
            _serial.DataReceived += _serial_DataReceived;

            _serial.Open();

            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // let the user know were ready...
            _display.Clear();
            _display.DrawText("Ready :)", _fontNinaB, Color.White, 10, 64);
            _display.Flush();

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        static void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _display.Clear();
            _display.DrawText(Getstring(), _fontNinaB, Color.White, 10, 64);
            _display.Flush();
        }

        /// <summary>
        /// Get a string from the serial port. 
        /// NOTE: This may not be the most efficient way of receiving strings from the serial port.
        /// </summary>
        /// <returns>The string that has been sent by the other application</returns>
        static string Getstring()
        {
            var ret = string.Empty;

            var latestBype = _serial.ReadByte();

            //Keep getting data until the latest byte is a zero byte
            while (latestBype != 0)
            {
                var character = (char)latestBype;

                ret += character;

                latestBype = _serial.ReadByte();
            }
            return ret;
        }
    }
}

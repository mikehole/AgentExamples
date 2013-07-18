﻿using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using System.IO.Ports;

namespace AgentExamples.ObjectMessagesAgentApp
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

            // sample "hello world" code
            _display.Clear();
            _display.DrawText("Ready :)", _fontNinaB, Color.White, 10, 64);
            _display.Flush();

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        static void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_serial.BytesToRead == 0) return;
            
            byte[] buffer = new byte[_serial.BytesToRead];
            _serial.Read(buffer, 0, buffer.Length);

            MicroSerialization.Mf.ObjectSerializer os = new MicroSerialization.Mf.ObjectSerializer();

            object objectRecieved = os.LoadFromBytes(buffer);

            switch (objectRecieved.GetType().Name)
            {
                case "ToastMessage":
                    _display.Clear();
                    _display.DrawText(((Model.ToastMessage)objectRecieved).Message, _fontNinaB, Color.White, 10, 64);
                    _display.Flush();
                    break;
            }


        }
    }
}

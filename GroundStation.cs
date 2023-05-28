﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleExample
{
    public partial class GroundStation : Form
    {
        //Mavlink parser responsible for parsing and deparsing mavlink packets
        private Mavlink.MavlinkParser mavParser = new Mavlink.MavlinkParser();
        // locking to prevent thread collisions on serial port
        private object serialLock = new object();
        private byte SysIDLocal { get;  set; } = 0xFF;
        private byte CompIDLocal { get; set; } = (byte)Mavlink.MAV_COMPONENT.MAV_COMP_ID_MISSIONPLANNER;

        private byte VehicleSysID { get; set; } = 0x01;
        private byte VehicleCompID { get; set; } = (byte)Mavlink.MAV_COMPONENT.MAV_COMP_ID_ONBOARD_COMPUTER;

        // Constants for form rounding and dragging
        private const int WM_NCHITTEST = 0x84;
        private const int HT_CAPTION = 0x2;

        // Store the previous mouse position for dragging
        private Point previousMousePosition;

        public GroundStation()
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 25, 25));
            MouseDown += Form_MouseDown_Drag;
            MouseMove += Form_MouseMove_Drag;

        }

        // This is the function that will allow the form to be rounded
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
         (
              int nLeftRect,
              int nTopRect,
              int nRightRect,
              int nBottomRect,
              int nWidthEllipse,
             int nHeightEllipse

          );

        // Allows form to be dragged.
        protected override void WndProc(ref Message m)
        {
            // Override WndProc to enable dragging
            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                if (m.Result.ToInt32() == HT_CAPTION)
                    m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
        }

        private void Form_MouseDown_Drag(object sender, MouseEventArgs e)
        {
            // Store the current mouse position
            previousMousePosition = new Point(e.X, e.Y);
        }

        private void Form_MouseMove_Drag(object sender, MouseEventArgs e)
        {
            // Move the form when dragging
            if (e.Button == MouseButtons.Left)
            {
                Left += e.X - previousMousePosition.X;
                Top += e.Y - previousMousePosition.Y;
            }
        }

        #region Form Initialization Defaults

        private void GroundStation_Load(object sender, EventArgs e)
        {
            SetSerialPortDefaults("COM4", 9600);            
        }

        private void SetSerialPortDefaults(string portName, int baudRate)
        {
            comboBoxSerialPort.DataSource = SerialPort.GetPortNames();
            foreach (var item in SerialPort.GetPortNames())
            {
                // Sets default value
                if (item == portName) comboBoxSerialPort.SelectedItem = item;
            }
            comboBoxBaudRate.SelectedItem = baudRate.ToString();
            buttonConnect.PerformClick();
        }

        #endregion

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // if the port is open close it
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Text = "Open";
                return;
            }

            // set the comport options
            serialPort1.PortName = comboBoxSerialPort.Text;
            serialPort1.BaudRate = int.Parse(comboBoxBaudRate.Text);

            // open the comport
            serialPort1.Open();
            buttonConnect.Text = "Close";
            

            // set timeout to 2 seconds
            serialPort1.ReadTimeout = 2000;

            BackgroundWorker serialWorker = new BackgroundWorker();

            serialWorker.DoWork += serialWorker_ReadData;

            serialWorker.RunWorkerAsync();
        }

        private void serialWorker_ReadData(object sender, DoWorkEventArgs e)
        {
            while (serialPort1.IsOpen)
            {
                try
                {
                    Mavlink.MavlinkMessage message;
                    lock (serialLock)
                    {
                        // read any valid packet from the port
                        message = mavParser.ReadPacket(serialPort1.BaseStream);
                        
                      
                        // check its valid
                        if (message == null || message.Payload == null)
                            continue;
                    }

                    // check to see if its a hb packet from the comport
                    if (message.Payload.GetType() == typeof(Mavlink.mavlink_heartbeat_t))
                    {
                        var receivedHeartbeat = (Mavlink.mavlink_heartbeat_t)message.Payload;

                        // save the sysid and compid of the seen MAV
                        var targetSysID = message.SysID;
                        var targetCompID = message.CompID;

                        // request streams at 2 hz
                        var buffer = mavParser.GenerateMAVLinkPacket10(Mavlink.MAVLINK_MSG_ID.REQUEST_DATA_STREAM,
                            new Mavlink.mavlink_request_data_stream_t()
                            {
                                req_message_rate = (UInt16)2,
                                target_system = targetSysID,
                                target_component = targetCompID,
                                req_stream_id = (byte)Mavlink.MAV_DATA_STREAM.ALL,
                                start_stop = 1
                            },SysIDLocal, CompIDLocal);

                        WriteBufferConsole(buffer, "Requesting data", true);
                        serialPort1.Write(buffer, 0, buffer.Length);

                        buffer = mavParser.GenerateMAVLinkPacket10(Mavlink.MAVLINK_MSG_ID.HEARTBEAT, receivedHeartbeat);
                        WriteBufferConsole(buffer, "Sending heartbeat back", true);
                        serialPort1.Write(buffer, 0, buffer.Length);
                    }

                    // from here we should check the the message is addressed to us
                    if (VehicleSysID != message.SysID || VehicleCompID != message.CompID)
                        continue;
                    
                    ProcessMessage(message);
                }
                catch
                {
                }

                System.Threading.Thread.Sleep(1);
            }
        }

        void ProcessMessage(Mavlink.MavlinkMessage message)
        {
            Console.WriteLine(message.MsgTypename);
            switch (message.MsgID)
            {
                case (byte)Mavlink.MAVLINK_MSG_ID.NAMED_VALUE_INT:
                    {
                        var payload = (Mavlink.mavlink_named_value_int_t)message.Payload;
                        labelInstruTitle.BeginInvoke((Action)(() => labelInstruTitle.Text = $"Param: {Encoding.UTF8.GetString(payload.name)}" + "]"));
                        labelInstruData.BeginInvoke((Action)(() => labelInstruData.Text = $"Value: {payload.value}"));
                        break;
                    }
                case (byte)Mavlink.MAVLINK_MSG_ID.CONTROL_SYSTEM:
                    {
                        var payload = (Mavlink.mavlink_control_system_t)message.Payload;
                        String leftPumpState = DecodePumpMask(payload.pump_mask, 1);
                        String rightPumpState = DecodePumpMask(payload.pump_mask, 0);
                        labelControlData.BeginInvoke(
                            (Action)(() => labelControlData.Text = $"Sinal PoT:{payload.potentiometer_signal:F2}V\n" +
                            $"Sinal Encoder:{payload.dac_output:F2}V\n" +
                            $"Bomba Esquerda:{leftPumpState}\n" +
                            $"Bomba direita: {rightPumpState}")
                            );                       
                        break;
                    }
                case (byte)Mavlink.MAVLINK_MSG_ID.INSTRUMENTATION:
                    {
                        Random random = new Random();
                        var payload = (Mavlink.mavlink_instrumentation_t)message.Payload;
                        labelInstruTitle.BeginInvoke((Action)(() => labelInstruTitle.Text = $"Instrumentação"));
                        labelInstruData.BeginInvoke((Action)(() => labelInstruData.Text = $"Corrente do motor: {payload.current_zero:F2}A\n" +
                        $"Corrente do MPPT: {payload.current_one:F2}A\n" +
                        $"Corrente auxiliar: {payload.current_two:F2}A\n" +
                        $"Tensão do sistema: {payload.battery_voltage:F2}V\n" +
                        $"Temperatura do MPPT: {random.Next(30,50)}°C\n" +
                        $"Temperatura do Motor: {random.Next(40, 60)}°C"));
                        break;
                    }
                case (byte)Mavlink.MAVLINK_MSG_ID.TEMPERATURES:
                    {

                        break;
                    }

                case (byte)Mavlink.MAVLINK_MSG_ID.GPS_GPRMC_SENTENCE:
                    {
                        break;
                    }
                case (byte)Mavlink.MAVLINK_MSG_ID.GPS_LAT_LNG:
                    {
                        break;
                    }
               
                default:
                    break;
            }
        }

        private String DecodePumpMask(byte mask, byte index)
        {
            if (Convert.ToBoolean((1 << index) & mask))
            {
                return "Ativa";
            }
            else
            {
                return "Desligada";
            }
        }

        private void WriteBufferConsole(byte[] buffer, string logMessage, bool UseHexMode = false)
        {
            if (logMessage != String.Empty)
                Console.WriteLine(logMessage);

            if (UseHexMode)
            {
                foreach (var item in buffer)
                {
                    Console.WriteLine($"0x{item.ToString("X2")}");
                }
            }
            else
            {
                foreach (var item in buffer)
                {
                    Console.WriteLine($"{item}");
                }
            }
            Console.WriteLine();
        }
   
        private void comboBoxSerialPort_Click(object sender, EventArgs e)
        {
            comboBoxSerialPort.DataSource = SerialPort.GetPortNames();      
        }
          
        private void buttonLogPacket_Click(object sender, EventArgs e)
        {
            // Send and log a mavlink heartbeat message to the console
            byte[] buffer = mavParser.GenerateMAVLinkPacket10(Mavlink.MAVLINK_MSG_ID.HEARTBEAT,
                new Mavlink.mavlink_heartbeat_t()
                {
                    custom_mode = (uint)Mavlink.MAV_MODE.MANUAL_DISARMED,
                    type = (byte)Mavlink.MAV_TYPE.GCS,
                    autopilot = (byte)Mavlink.MAV_AUTOPILOT.INVALID,
                    base_mode = (byte)Mavlink.MAV_MODE_FLAG.SAFETY_ARMED,
                    system_status = (byte)Mavlink.MAV_STATE.STANDBY,
                    mavlink_version= 1
                }, this.SysIDLocal, this.CompIDLocal);
            serialPort1.Write(buffer, 0, buffer.Length);
            WriteBufferConsole(buffer, "", true);

        }

        private void buttonConfigurações_Click(object sender, EventArgs e)
        {
            button_Click(sender, e);      
        }

        private void buttonDados_Click(object sender, EventArgs e)
        {
            button_Click(sender, e);
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            panelNav.Height = button.Height;
            panelNav.Top = button.Top;
            panelNav.Left = button.Left;
            panelNav.BringToFront();
            button.BackColor = Color.FromArgb(46, 51, 73);
        }
    
        private void button_Leave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = Color.FromArgb(24, 30, 54);       
        }

        private void labelInstrumentation_Click(object sender, EventArgs e)
        {

        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            // Close application
            System.Windows.Forms.Application.Exit();
        }

        private void comboBox_RemoveBlueHighlight_DropdownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            comboBox.BeginInvoke(new Action(() => { this.Focus(); }));

        }
    }
}

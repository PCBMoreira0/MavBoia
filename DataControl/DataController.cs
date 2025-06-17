using SimpleExample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MavBoia.Geography;
using System.Diagnostics;
using System.Timers;
using System.Dynamic;
using MavBoia;

namespace MavlinkDataController
{
    public class DataController
    {
        public event Action<AllSensorData> OnDataReceived;

        private AllSensorData lastDataProcessed = new AllSensorData();

        private double lastLatitude;
        private double lastLongitude;

        private float CalculateVelocity()
        {
            if (this.lastDataProcessed.Latitude == 0 || this.lastDataProcessed.Longitude == 0) return 0f;

            float velocity = (float)((GeoUtils.HaversineDistance(this.lastLatitude, this.lastLongitude, this.lastDataProcessed.Latitude, this.lastDataProcessed.Longitude) / 1.852) * 3600);
            this.lastLatitude = lastDataProcessed.Latitude;
            this.lastLongitude = lastDataProcessed.Longitude;

            return velocity;
        }


        public void ProcessMavlinkMessage(Mavlink.MavlinkMessage message)
        {
            //Console.WriteLine(message.MsgTypename);
            //switch (message.MsgID)
            //{
            //    // Delegates and lambda expressions must be used to update the UI from a different thread.
            //    case (uint)Mavlink.MAVLINK_MSG_ID.ALL_INFO:
            //        {
            //            Mavlink.mavlink_all_info_t payload = (Mavlink.mavlink_all_info_t)message.Payload;
            //            MavlinkUtilities.PrintMessageInfo(message);
            //            AllSensorData allSensorData = new AllSensorData(payload);
            //            allSensorData.Velocity = CalculateVelocity();
            //            lastDataProcessed = AllSensorData.Clone(allSensorData);
            //            SaveData(allSensorData);
            //            OnDataReceived?.Invoke(allSensorData);
            //            break;
            //        }
            //    default:
            //        break;
            //}
        }

        public void ProcessNetworkData(AllSensorData data)
        {
            data.Velocity = CalculateVelocity();
            lastDataProcessed = AllSensorData.Clone(data);
            SaveData(data);
            OnDataReceived?.Invoke(data);
        }

        public AllSensorData GetAllSensorData()
        {
            return lastDataProcessed;
        }

        #region Logging
        String GetLoggingDirectory()
        {
            string directory = String.Empty;
            directory = MavBoiaConfigurations.RjTextBoxLogDirectory;
            if (directory == String.Empty)
            {
                MessageBox.Show("Invalid directory!");
            }
            return directory;
        }

        void WriteDataCSV(string directory, string fileName, string data)
        {

            if (directory == String.Empty)
            {
                MessageBox.Show("Invalid directory!");
                return;
            }

            if (fileName == String.Empty)
            {
                MessageBox.Show("Invalid file name!");
                return;
            }

            string path = Path.Combine(directory, fileName);
            using (StreamWriter dataLogger = new StreamWriter(path, true))
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string csvData = $"{data},{timestamp}";
                Console.WriteLine(csvData);
                dataLogger.WriteLine(csvData);
            }
        }

        void SaveData(Mavlink.MavlinkMessage message)
        {
            string directory = GetLoggingDirectory();
            if (directory == null) return;
            string fileName = message.MsgTypename + ".csv";
            string data = MavlinkUtilities.GetMessageDataCSV(message);
            data += $"{GetAllSensorData().Velocity}";
            WriteDataCSV(directory, fileName, data);
        }
        
        void SaveData(AllSensorData data)
        {
            string directory = GetLoggingDirectory();
            if (directory == null) return;
            const string fileName = "InfluxDB.csv";

            WriteDataCSV(directory, fileName, data.ToCSV());
        }

        #endregion
    }
}

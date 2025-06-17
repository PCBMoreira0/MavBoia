using Opc.Cpx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavBoia.DataControl
{
    internal class BMSStatusData
    {
        int[] temperatures = new int[2]; /*< [cdegC] Temperature of the battery*/
        uint status; /*<  Charge/Discharge FET status and current charging status*/
        uint failure_flags_byte0; /*<  Failure flags: Voltage related (Byte 0).*/
        uint failure_flags_byte1; /*<  Failure flags: Temperature related (Byte 1).*/
        uint failure_flags_byte2; /*<  Failure flags: Current and SOC related (Byte 2).*/
        uint failure_flags_byte3; /*<  Failure flags: Differential V/T related (Byte 3).*/
        uint failure_flags_byte4; /*<  Failure flags: MOSFET and sensor related (Byte 4).*/
        uint failure_flags_byte5; /*<  Failure flags: System/Component related (Byte 5).*/
        uint failure_flags_byte6; /*<  Failure flags: Other faults (Byte 6).*/
        uint fault_code_byte7; /*<  Raw fault code (Byte 7 from original message).*/
    }
}

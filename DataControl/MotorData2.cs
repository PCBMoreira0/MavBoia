using Opc.Cpx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavBoia.DataControl
{
    enum MOTOR_STATUS
    {
        GEAR = 0,
        BREAK = 1, 
        OPERATION_MODE = 2,
        DC_CONTACTOR = 3
    }
    internal class MotorData2 : IBoatData
    {
        int controller_temperature; /*<  Controller temperature. Actual value = MAVLink value - 40. Unit: degC*/
        int motor_temperature; /*<  Motor temperature. Actual value = MAVLink value - 40. Unit: degC*/
        int status; /*<  Gear, brake, operation mode and DC contactor.*/
        int error_flags_byte4; /*<  Error flags from Byte 4.*/
        int error_flags_byte5; /*<  Error flags from Byte 5.*/
        int error_flags_byte6; /*<  Error flags from Byte 6.*/
        int life_signal; /*<  Life signal counter derived from bits 7-4 from byte 7, msg II.*/
        MOTOR_INSTANCE instance; /*<  0 for left motor, 1 for right motor*/
    }
}

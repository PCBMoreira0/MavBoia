using Opc.Cpx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavBoia.DataControl
{
    enum MOTOR_INSTANCE
    {
        LEFT = 0, RIGHT = 1,
    }

    internal class MotorData1
    {
        float bus_voltage; /*< [dV] 0.1V/bit, offset= 0*/
        float bus_current; /*< [dA] 0.1A/bit, offset= -32000*/
        int rpm; /*< [drpm/bit] 0.1rpm/bit, offset= -32000*/
        int accelerator_opening; /*<  1%/bit, throttle signal*/
        MOTOR_INSTANCE instance; /*<  0 for left motor, 1 for right motor*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavBoia.DataControl
{
    internal class InstrumentationData : IBoatData
    {
        float battery_current; /*< [cA] */
        float motor_current_left; /*< [cA] */
        float motor_current_right; /*< [cA] */
        float mppt_current; /*< [cA] */
        float auxiliary_battery_current; /*< [cA] */
        float battery_voltage; /*< [cV] */
        float auxiliary_battery_voltage; /*< [cV] */
        float irradiance; /*< [W/m^2] */
    }
}

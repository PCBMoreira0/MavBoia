using Opc.Cpx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MavBoia.DataControl
{
    internal class BMSData : IBoatData
    {
        uint[] voltages = new uint[16]; /*< [mV] Voltage of each cell.*/
        int[] temperatures = new int[2]; /*< [cdegC] Temperature of the battery*/
        int current_battery; /*< [dA] Battery current*/
        int state_of_charge; /*< [%] Remaining battery energy. Values: [0-100]*/
    }
}

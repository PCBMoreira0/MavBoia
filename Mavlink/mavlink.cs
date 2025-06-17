using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using static Mavlink;

public partial class MAVLink
{
    public const string MAVLINK_BUILD_DATE = "Fri Jun 13 2025";
    public const string MAVLINK_WIRE_PROTOCOL_VERSION = "1.0";
    public const int MAVLINK_MAX_PAYLOAD_LEN = 51;

    public const byte MAVLINK_CORE_HEADER_LEN = 9;///< Length of core header (of the comm. layer)
    public const byte MAVLINK_CORE_HEADER_MAVLINK1_LEN = 5;///< Length of MAVLink1 core header (of the comm. layer)
    public const byte MAVLINK_NUM_HEADER_BYTES = (MAVLINK_CORE_HEADER_LEN + 1);///< Length of all header bytes, including core and stx
    public const byte MAVLINK_NUM_CHECKSUM_BYTES = 2;
    public const byte MAVLINK_NUM_NON_PAYLOAD_BYTES = (MAVLINK_NUM_HEADER_BYTES + MAVLINK_NUM_CHECKSUM_BYTES);

    public const int MAVLINK_MAX_PACKET_LEN = (MAVLINK_MAX_PAYLOAD_LEN + MAVLINK_NUM_NON_PAYLOAD_BYTES + MAVLINK_SIGNATURE_BLOCK_LEN);///< Maximum packet length
    public const byte MAVLINK_SIGNATURE_BLOCK_LEN = 13;

    public const int MAVLINK_LITTLE_ENDIAN = 1;
    public const int MAVLINK_BIG_ENDIAN = 0;

    public const byte MAVLINK_STX = 254;

    public const byte MAVLINK_STX_MAVLINK1 = 0xFE;

    public const byte MAVLINK_ENDIAN = MAVLINK_LITTLE_ENDIAN;

    public const bool MAVLINK_ALIGNED_FIELDS = (1 == 1);

    public const byte MAVLINK_CRC_EXTRA = 1;
    
    public const byte MAVLINK_COMMAND_24BIT = 0;
        
    public const bool MAVLINK_NEED_BYTE_SWAP = (MAVLINK_ENDIAN == MAVLINK_LITTLE_ENDIAN);
        
    // msgid, name, crc, minlength, length, type
    public static message_info[] MAVLINK_MESSAGE_INFOS = new message_info[] {
        new message_info(1, "INSTRUMENTATION", 83, 22, 22, typeof( mavlink_instrumentation_t )), // none 24 bit
        new message_info(2, "TEMPERATURES", 87, 14, 14, typeof( mavlink_temperatures_t )), // none 24 bit
        new message_info(3, "GPS", 37, 20, 20, typeof( mavlink_gps_t )), // none 24 bit
        new message_info(4, "MPPT", 26, 14, 14, typeof( mavlink_mppt_t )), // none 24 bit
        new message_info(5, "MPPT_STATE", 40, 10, 10, typeof( mavlink_mppt_state_t )), // none 24 bit
        new message_info(6, "BMS", 65, 45, 45, typeof( mavlink_bms_t )), // none 24 bit
        new message_info(7, "BMS_STATUS", 161, 19, 19, typeof( mavlink_bms_status_t )), // none 24 bit
        new message_info(8, "EZKONTROL_MCU_METER_DATA_I", 194, 14, 14, typeof( mavlink_ezkontrol_mcu_meter_data_i_t )), // none 24 bit
        new message_info(9, "EZKONTROL_MCU_METER_DATA_II", 17, 14, 14, typeof( mavlink_ezkontrol_mcu_meter_data_ii_t )), // none 24 bit
        new message_info(10, "PUMPS", 246, 7, 7, typeof( mavlink_pumps_t )), // none 24 bit
        new message_info(20, "PARAM_REQUEST_READ", 151, 18, 18, typeof( mavlink_param_request_read_t )), // none 24 bit
        new message_info(22, "PARAM_VALUE", 220, 25, 25, typeof( mavlink_param_value_t )), // none 24 bit
        new message_info(23, "PARAM_SET", 22, 21, 21, typeof( mavlink_param_set_t )), // none 24 bit
        new message_info(109, "RADIO_STATUS", 44, 10, 10, typeof( mavlink_radio_status_t )), // none 24 bit
        new message_info(251, "NAMED_VALUE_FLOAT", 79, 24, 24, typeof( mavlink_named_value_float_t )), // none 24 bit
        new message_info(252, "NAMED_VALUE_INT", 106, 24, 24, typeof( mavlink_named_value_int_t )), // none 24 bit
        new message_info(253, "STATUSTEXT", 83, 51, 51, typeof( mavlink_statustext_t )), // none 24 bit
        new message_info(254, "DEBUG", 46, 9, 9, typeof( mavlink_debug_t )), // none 24 bit

    };

    public const byte MAVLINK_VERSION = 3;

    public const byte MAVLINK_IFLAG_SIGNED=  0x01;
    public const byte MAVLINK_IFLAG_MASK   = 0x01;

    public struct message_info
    {
        public uint msgid { get; internal set; }
        public string name { get; internal set; }
        public byte crc { get; internal set; }
        public uint minlength { get; internal set; }
        public uint length { get; internal set; }
        public Type type { get; internal set; }

        public message_info(uint msgid, string name, byte crc, uint minlength, uint length, Type type)
        {
            this.msgid = msgid;
            this.name = name;
            this.crc = crc;
            this.minlength = minlength;
            this.length = length;
            this.type = type;
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}",name,msgid);
        }
    }   

    public enum MAVLINK_MSG_ID 
    {

        INSTRUMENTATION = 1,
        TEMPERATURES = 2,
        GPS = 3,
        MPPT = 4,
        MPPT_STATE = 5,
        BMS = 6,
        BMS_STATUS = 7,
        EZKONTROL_MCU_METER_DATA_I = 8,
        EZKONTROL_MCU_METER_DATA_II = 9,
        PUMPS = 10,
        PARAM_REQUEST_READ = 20,
        PARAM_VALUE = 22,
        PARAM_SET = 23,
        RADIO_STATUS = 109,
        NAMED_VALUE_FLOAT = 251,
        NAMED_VALUE_INT = 252,
        STATUSTEXT = 253,
        DEBUG = 254,
    }
    
    
    ///<summary> Component ids (values) for the different types and instances of onboard hardware/software that might make up a MAVLink system (autopilot, cameras, servos, GPS systems, avoidance systems etc.).       Components must use the appropriate ID in their source address when sending messages. Components can also use IDs to determine if they are the intended recipient of an incoming message. The MAV_COMP_ID_ALL value is used to indicate messages that must be processed by all components.       When creating new entries, components that can have multiple instances (e.g. cameras, servos etc.) should be allocated sequential values. An appropriate number of values should be left free after these components to allow the number of instances to be expanded. </summary>
    public enum MAV_COMPONENT: int /*default*/
    {
        ///<summary> Target id (target_component) used to broadcast messages to all components of the receiving system. Components should attempt to process messages with this component ID and forward to components on any other interfaces. Note: This is not a valid *source* component id for a message. | </summary>
        [Description("Target id (target_component) used to broadcast messages to all components of the receiving system. Components should attempt to process messages with this component ID and forward to components on any other interfaces. Note: This is not a valid *source* component id for a message.")]
        MAV_COMP_ID_ALL=0, 
        ///<summary> TTGO Lora Radio(100mW) | </summary>
        [Description("TTGO Lora Radio(100mW)")]
        MAV_COMP_ID_RADIO_915=1, 
        ///<summary> EByte Lora Radio(2W) | </summary>
        [Description("EByte Lora Radio(2W)")]
        MAV_COMP_ID_RADIO_433=2, 
        ///<summary> Message sent via internet | </summary>
        [Description("Message sent via internet")]
        MAV_COMP_ID_INTERNET=3, 
        
    };
    
    ///<summary> Represents the status of the battery. Derived from input register 3200. </summary>
    public enum MPPT_BATTERY_STATUS: ushort
    {
        ///<summary> All monitored parameters are normal. For battery (ID=3200): Voltage Normal (D3-D0=00H), Temp Normal (D7-D4=00H), Internal Resistance Normal (D8=0), Rated Voltage ID Normal (D15=0). | </summary>
        [Description("All monitored parameters are normal. For battery (ID=3200): Voltage Normal (D3-D0=00H), Temp Normal (D7-D4=00H), Internal Resistance Normal (D8=0), Rated Voltage ID Normal (D15=0).")]
        MPPT_STATUS_NORMAL=0, 
        ///<summary> Battery overvoltage detected (e.g., ID=3200, D3-D0 = 01H). | </summary>
        [Description("Battery overvoltage detected (e.g., ID=3200, D3-D0 = 01H).")]
        MPPT_STATUS_OVERVOLTAGE=1, 
        ///<summary> Battery undervoltage detected (e.g., ID=3200, D3-D0 = 02H). | </summary>
        [Description("Battery undervoltage detected (e.g., ID=3200, D3-D0 = 02H).")]
        MPPT_STATUS_UNDERVOLTAGE=2, 
        ///<summary> Battery low voltage disconnect activated (e.g., ID=3200, D3-D0 = 03H). | </summary>
        [Description("Battery low voltage disconnect activated (e.g., ID=3200, D3-D0 = 03H).")]
        MPPT_STATUS_VOLTAGE_LOW_VOLT_DISCONNECT=3, 
        ///<summary> A general battery voltage fault detected (e.g., ID=3200, D3-D0 = 04H). | </summary>
        [Description("A general battery voltage fault detected (e.g., ID=3200, D3-D0 = 04H).")]
        MPPT_STATUS_VOLTAGE_FAULT=4, 
        ///<summary> Over temperature detected (e.g., ID=3200, D7-D4 = 01H). | </summary>
        [Description("Over temperature detected (e.g., ID=3200, D7-D4 = 01H).")]
        MPPT_STATUS_TEMP_OVER=5, 
        ///<summary> Low temperature detected (e.g., ID=3200, D7-D4 = 02H). | </summary>
        [Description("Low temperature detected (e.g., ID=3200, D7-D4 = 02H).")]
        MPPT_STATUS_TEMP_LOW=6, 
        ///<summary> Battery internal resistance is abnormal (e.g., ID=3200, D8 = 1). | </summary>
        [Description("Battery internal resistance is abnormal (e.g., ID=3200, D8 = 1).")]
        MPPT_STATUS_INTERNAL_RESISTANCE_ABNORMAL=7, 
        ///<summary> Wrong identification for rated voltage (e.g., ID=3200, D15 = 1). | </summary>
        [Description("Wrong identification for rated voltage (e.g., ID=3200, D15 = 1).")]
        MPPT_STATUS_RATED_VOLTAGE_ID_WRONG=8, 
        ///<summary> The status is unknown or not yet determined. | </summary>
        [Description("The status is unknown or not yet determined.")]
        MPPT_STATUS_UNKNOWN=255, 
        
    };
    
    ///<summary> Represents the primary status of the charging equipment. Derived by interpreting bits in input register 3201. The reported status may depend on the priority logic implemented in the monitoring software if multiple conditions are present. </summary>
    public enum MPPT_CHARGING_EQUIPMENT_STATUS: ushort
    {
        ///<summary> Equipment is running mode, else standby (register 3201: D0=1 = running). Typically implies no active charging and no faults. | </summary>
        [Description("Equipment is running mode, else standby (register 3201: D0=1 = running). Typically implies no active charging and no faults.")]
        MPPT_CHARGING_STATUS_RUNNING_STANDBY=0, 
        ///<summary> Equipment has detected a fault (D1=1=Fault) | </summary>
        [Description("Equipment has detected a fault (D1=1=Fault)")]
        MPPT_CHARGING_STATUS_NORMAL_FAULT=1, 
        ///<summary> Equipment is in float charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=01H (Float), and D15-D14=00H (Input Volt Normal)). | </summary>
        [Description("Equipment is in float charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=01H (Float), and D15-D14=00H (Input Volt Normal)).")]
        MPPT_CHARGING_STATUS_FLOAT=2, 
        ///<summary> Equipment is in boost charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=02H (Boost), and D15-D14=00H (Input Volt Normal)). | </summary>
        [Description("Equipment is in boost charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=02H (Boost), and D15-D14=00H (Input Volt Normal)).")]
        MPPT_CHARGING_STATUS_BOOST=3, 
        ///<summary> Equipment is in equalization charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=03H (Equalization), and D15-D14=00H (Input Volt Normal)). | </summary>
        [Description("Equipment is in equalization charging mode (register 3201: D0=1, D1=0 (No Fault), D3-D2=03H (Equalization), and D15-D14=00H (Input Volt Normal)).")]
        MPPT_CHARGING_STATUS_EQUALIZATION=4, 
        ///<summary> No power connected to input (register 3201: D15-D14=01H). Equipment may be off or in a pre-start state. | </summary>
        [Description("No power connected to input (register 3201: D15-D14=01H). Equipment may be off or in a pre-start state.")]
        MPPT_CHARGING_INPUT_VOLTAGE_NONE=5, 
        ///<summary> Input voltage is too high (register 3201: D15-D14=02H). This may lead to a fault condition (D1=1). | </summary>
        [Description("Input voltage is too high (register 3201: D15-D14=02H). This may lead to a fault condition (D1=1).")]
        MPPT_CHARGING_INPUT_VOLTAGE_HIGH=6, 
        ///<summary> General input voltage error detected (register 3201: D15-D14=03H). This may lead to a fault condition (D1=1). | </summary>
        [Description("General input voltage error detected (register 3201: D15-D14=03H). This may lead to a fault condition (D1=1).")]
        MPPT_CHARGING_INPUT_VOLTAGE_ERROR=7, 
        ///<summary> PV input short circuit detected (register 3201: D4=1. Typically, D1=1 will also be set). | </summary>
        [Description("PV input short circuit detected (register 3201: D4=1. Typically, D1=1 will also be set).")]
        MPPT_CHARGING_FAULT_PV_SHORT=8, 
        ///<summary> Fault on the load side, such as load over current (D9=1), load short circuit (D8=1), or load MOSFET short (D7=1) (register 3201. Typically, D1=1 will also be set). | </summary>
        [Description("Fault on the load side, such as load over current (D9=1), load short circuit (D8=1), or load MOSFET short (D7=1) (register 3201. Typically, D1=1 will also be set).")]
        MPPT_CHARGING_FAULT_LOAD_SIDE=9, 
        ///<summary> Input over current detected (register 3201: D10=1. Typically, D1=1 will also be set). | </summary>
        [Description("Input over current detected (register 3201: D10=1. Typically, D1=1 will also be set).")]
        MPPT_CHARGING_FAULT_INPUT_OVERCURRENT=10, 
        ///<summary> Internal MOSFET short circuit detected (Charging MOSFET D13=1, Charging or Anti-reverse MOSFET D12=1, or Anti-reverse MOSFET D11=1) (register 3201. Typically, D1=1 will also be set). | </summary>
        [Description("Internal MOSFET short circuit detected (Charging MOSFET D13=1, Charging or Anti-reverse MOSFET D12=1, or Anti-reverse MOSFET D11=1) (register 3201. Typically, D1=1 will also be set).")]
        MPPT_CHARGING_FAULT_MOSFET_SHORT=11, 
        ///<summary> A general system fault is active (register 3201: D1=1), not covered by other more specific fault entries in this enum. | </summary>
        [Description("A general system fault is active (register 3201: D1=1), not covered by other more specific fault entries in this enum.")]
        MPPT_CHARGING_FAULT_SYSTEM=12, 
        ///<summary> Equipment is running but not actively charging (register 3201: D0=1, D1=0 (No Fault), D3-D2=00H (No Charging), and D15-D14=00H (Input Volt Normal)). | </summary>
        [Description("Equipment is running but not actively charging (register 3201: D0=1, D1=0 (No Fault), D3-D2=00H (No Charging), and D15-D14=00H (Input Volt Normal)).")]
        MPPT_CHARGING_STATUS_NO_CHARGING=13, 
        ///<summary> The charging equipment status is unknown or not yet determined. | </summary>
        [Description("The charging equipment status is unknown or not yet determined.")]
        MPPT_CHARGING_STATUS_UNKNOWN=255, 
        
    };
    
    ///<summary>          Flags to describe the charge/discharge status of a battery system, designed to be packed into a single byte.         The status is composed of three parts:         1. Operational State (2 bits, bits 0-1): Indicates if the system is stationary, charging, or discharging.            - 0 (00b): Stationary            - 1 (01b): Charging            - 2 (10b): Discharging            - 3 (11b): Reserved for future use.         2. Charge MOSFET State (1 bit, bit 2): Indicates if the charge MOSFET is ON (1) or OFF (0).         3. Discharge MOSFET State (1 bit, bit 3): Indicates if the discharge MOSFET is ON (1) or OFF (0).          The enum entries provide values that can be ORed together to form the complete status byte.         For example, to represent 'Charging' with 'Charge MOS ON' and 'Discharge MOS OFF':         (CHARGE_DISCHARGE_STATE_CHARGING | CHARGE_DISCHARGE_CHARGE_MOS_ON)         This would result in a value of (1 | 4) = 5.          To decode:         - state = value and 0x03;         - charge_mos_on = (value and CHARGE_DISCHARGE_CHARGE_MOS_ON) != 0         - discharge_mos_on = (value and CHARGE_DISCHARGE_DISCHARGE_MOS_ON) != 0        </summary>
    [Flags]
	public enum BMS_CHARGE_DISCHARGE_STATUS_FLAGS: byte
    {
        ///<summary> Operational state is Stationary (neither charging nor discharging). This value (00b) occupies bits 0-1. | </summary>
        [Description("Operational state is Stationary (neither charging nor discharging). This value (00b) occupies bits 0-1.")]
        CHARGE_DISCHARGE_STATE_STATIONARY=1, 
        ///<summary> Operational state is Charging. This value (01b) occupies bits 0-1. | </summary>
        [Description("Operational state is Charging. This value (01b) occupies bits 0-1.")]
        CHARGE_DISCHARGE_STATE_CHARGING=2, 
        ///<summary> Operational state is Discharging. This value (10b) occupies bits 0-1. | </summary>
        [Description("Operational state is Discharging. This value (10b) occupies bits 0-1.")]
        CHARGE_DISCHARGE_STATE_DISCHARGING=4, 
        ///<summary> Charge MOSFET is ON. This flag occupies bit 2. If this flag is not set, Charge MOS is OFF. | </summary>
        [Description("Charge MOSFET is ON. This flag occupies bit 2. If this flag is not set, Charge MOS is OFF.")]
        CHARGE_DISCHARGE_CHARGE_MOS_ON=8, 
        ///<summary> Discharge MOSFET is ON. This flag occupies bit 3. If this flag is not set, Discharge MOS is OFF. | </summary>
        [Description("Discharge MOSFET is ON. This flag occupies bit 3. If this flag is not set, Discharge MOS is OFF.")]
        CHARGE_DISCHARGE_DISCHARGE_MOS_ON=16, 
        
    };
    
    ///<summary> Flags for Byte 0 of the Daly BMS failure status: Voltage-related failures. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE0_VOLTAGE_FLAGS: byte
    {
        ///<summary> Cell voltage high level 1 alarm. | </summary>
        [Description("Cell voltage high level 1 alarm.")]
        DALY_FAILURE_CELL_VOLT_HIGH_LVL1=1, 
        ///<summary> Cell voltage high level 2 alarm. | </summary>
        [Description("Cell voltage high level 2 alarm.")]
        DALY_FAILURE_CELL_VOLT_HIGH_LVL2=2, 
        ///<summary> Cell voltage low level 1 alarm. | </summary>
        [Description("Cell voltage low level 1 alarm.")]
        DALY_FAILURE_CELL_VOLT_LOW_LVL1=4, 
        ///<summary> Cell voltage low level 2 alarm. | </summary>
        [Description("Cell voltage low level 2 alarm.")]
        DALY_FAILURE_CELL_VOLT_LOW_LVL2=8, 
        ///<summary> Sum voltage high level 1 alarm. | </summary>
        [Description("Sum voltage high level 1 alarm.")]
        DALY_FAILURE_SUM_VOLT_HIGH_LVL1=16, 
        ///<summary> Sum voltage high level 2 alarm. | </summary>
        [Description("Sum voltage high level 2 alarm.")]
        DALY_FAILURE_SUM_VOLT_HIGH_LVL2=32, 
        ///<summary> Sum voltage low level 1 alarm. | </summary>
        [Description("Sum voltage low level 1 alarm.")]
        DALY_FAILURE_SUM_VOLT_LOW_LVL1=64, 
        ///<summary> Sum voltage low level 2 alarm. | </summary>
        [Description("Sum voltage low level 2 alarm.")]
        DALY_FAILURE_SUM_VOLT_LOW_LVL2=128, 
        
    };
    
    ///<summary> Flags for Byte 1 of the Daly BMS failure status: Temperature-related failures. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE1_TEMP_FLAGS: byte
    {
        ///<summary> Charge temperature high level 1 alarm. | </summary>
        [Description("Charge temperature high level 1 alarm.")]
        DALY_FAILURE_CHG_TEMP_HIGH_LVL1=1, 
        ///<summary> Charge temperature high level 2 alarm. | </summary>
        [Description("Charge temperature high level 2 alarm.")]
        DALY_FAILURE_CHG_TEMP_HIGH_LVL2=2, 
        ///<summary> Charge temperature low level 1 alarm. | </summary>
        [Description("Charge temperature low level 1 alarm.")]
        DALY_FAILURE_CHG_TEMP_LOW_LVL1=4, 
        ///<summary> Charge temperature low level 2 alarm. | </summary>
        [Description("Charge temperature low level 2 alarm.")]
        DALY_FAILURE_CHG_TEMP_LOW_LVL2=8, 
        ///<summary> Discharge temperature high level 1 alarm. | </summary>
        [Description("Discharge temperature high level 1 alarm.")]
        DALY_FAILURE_DISCHG_TEMP_HIGH_LVL1=16, 
        ///<summary> Discharge temperature high level 2 alarm. | </summary>
        [Description("Discharge temperature high level 2 alarm.")]
        DALY_FAILURE_DISCHG_TEMP_HIGH_LVL2=32, 
        ///<summary> Discharge temperature low level 1 alarm. | </summary>
        [Description("Discharge temperature low level 1 alarm.")]
        DALY_FAILURE_DISCHG_TEMP_LOW_LVL1=64, 
        ///<summary> Discharge temperature low level 2 alarm. | </summary>
        [Description("Discharge temperature low level 2 alarm.")]
        DALY_FAILURE_DISCHG_TEMP_LOW_LVL2=128, 
        
    };
    
    ///<summary> Flags for Byte 2 of the Daly BMS failure status: Current and SOC related failures. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE2_CURRENT_SOC_FLAGS: byte
    {
        ///<summary> Charge overcurrent level 1 alarm. | </summary>
        [Description("Charge overcurrent level 1 alarm.")]
        DALY_FAILURE_CHG_OVERCURRENT_LVL1=1, 
        ///<summary> Charge overcurrent level 2 alarm. | </summary>
        [Description("Charge overcurrent level 2 alarm.")]
        DALY_FAILURE_CHG_OVERCURRENT_LVL2=2, 
        ///<summary> Discharge overcurrent level 1 alarm. | </summary>
        [Description("Discharge overcurrent level 1 alarm.")]
        DALY_FAILURE_DISCHG_OVERCURRENT_LVL1=4, 
        ///<summary> Discharge overcurrent level 2 alarm. | </summary>
        [Description("Discharge overcurrent level 2 alarm.")]
        DALY_FAILURE_DISCHG_OVERCURRENT_LVL2=8, 
        ///<summary> SOC high level 1 alarm. | </summary>
        [Description("SOC high level 1 alarm.")]
        DALY_FAILURE_SOC_HIGH_LVL1=16, 
        ///<summary> SOC high level 2 alarm. | </summary>
        [Description("SOC high level 2 alarm.")]
        DALY_FAILURE_SOC_HIGH_LVL2=32, 
        ///<summary> SOC low level 1 alarm. | </summary>
        [Description("SOC low level 1 alarm.")]
        DALY_FAILURE_SOC_LOW_LVL1=64, 
        ///<summary> SOC low level 2 alarm. | </summary>
        [Description("SOC low level 2 alarm.")]
        DALY_FAILURE_SOC_LOW_LVL2=128, 
        
    };
    
    ///<summary> Flags for Byte 3 of the Daly BMS failure status: Differential voltage/temperature failures. Bits 4-7 are reserved. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE3_DIFFERENTIAL_FLAGS: byte
    {
        ///<summary> Differential voltage level 1 alarm. | </summary>
        [Description("Differential voltage level 1 alarm.")]
        DALY_FAILURE_DIFF_VOLT_LVL1=1, 
        ///<summary> Differential voltage level 2 alarm. | </summary>
        [Description("Differential voltage level 2 alarm.")]
        DALY_FAILURE_DIFF_VOLT_LVL2=2, 
        ///<summary> Differential temperature level 1 alarm. | </summary>
        [Description("Differential temperature level 1 alarm.")]
        DALY_FAILURE_DIFF_TEMP_LVL1=4, 
        ///<summary> Differential temperature level 2 alarm. | </summary>
        [Description("Differential temperature level 2 alarm.")]
        DALY_FAILURE_DIFF_TEMP_LVL2=8, 
        
    };
    
    ///<summary> Flags for Byte 4 of the Daly BMS failure status: MOSFET and related sensor failures. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE4_MOSFET_SENSOR_FLAGS: byte
    {
        ///<summary> Charge MOS temperature high alarm. | </summary>
        [Description("Charge MOS temperature high alarm.")]
        DALY_FAILURE_CHG_MOS_TEMP_HIGH_ALARM=1, 
        ///<summary> Discharge MOS temperature high alarm. | </summary>
        [Description("Discharge MOS temperature high alarm.")]
        DALY_FAILURE_DISCHG_MOS_TEMP_HIGH_ALARM=2, 
        ///<summary> Charge MOS temperature sensor error. | </summary>
        [Description("Charge MOS temperature sensor error.")]
        DALY_FAILURE_CHG_MOS_TEMP_SENSOR_ERR=4, 
        ///<summary> Discharge MOS temperature sensor error. | </summary>
        [Description("Discharge MOS temperature sensor error.")]
        DALY_FAILURE_DISCHG_MOS_TEMP_SENSOR_ERR=8, 
        ///<summary> Charge MOS adhesion error. | </summary>
        [Description("Charge MOS adhesion error.")]
        DALY_FAILURE_CHG_MOS_ADHESION_ERR=16, 
        ///<summary> Discharge MOS adhesion error. | </summary>
        [Description("Discharge MOS adhesion error.")]
        DALY_FAILURE_DISCHG_MOS_ADHESION_ERR=32, 
        ///<summary> Charge MOS open circuit error. | </summary>
        [Description("Charge MOS open circuit error.")]
        DALY_FAILURE_CHG_MOS_OPEN_CIRCUIT_ERR=64, 
        ///<summary> Discharge MOS open circuit error (Corrected from 'Discrg' in PDF). | </summary>
        [Description("Discharge MOS open circuit error (Corrected from 'Discrg' in PDF).")]
        DALY_FAILURE_DISCHG_MOS_OPEN_CIRCUIT_ERR=128, 
        
    };
    
    ///<summary> Flags for Byte 5 of the Daly BMS failure status: System/Component failures. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE5_SYSTEM_COMPONENT_FLAGS: byte
    {
        ///<summary> AFE collect chip error. | </summary>
        [Description("AFE collect chip error.")]
        DALY_FAILURE_AFE_COLLECT_CHIP_ERR=1, 
        ///<summary> Voltage collect dropped. | </summary>
        [Description("Voltage collect dropped.")]
        DALY_FAILURE_VOLTAGE_COLLECT_DROPPED=2, 
        ///<summary> Cell temperature sensor error. | </summary>
        [Description("Cell temperature sensor error.")]
        DALY_FAILURE_CELL_TEMP_SENSOR_ERR=4, 
        ///<summary> EEPROM error. | </summary>
        [Description("EEPROM error.")]
        DALY_FAILURE_EEPROM_ERR=8, 
        ///<summary> RTC error. | </summary>
        [Description("RTC error.")]
        DALY_FAILURE_RTC_ERR=16, 
        ///<summary> Precharge failure. | </summary>
        [Description("Precharge failure.")]
        DALY_FAILURE_PRECHARGE_FAILURE=32, 
        ///<summary> Communication failure (external). | </summary>
        [Description("Communication failure (external).")]
        DALY_FAILURE_COMMUNICATION_FAILURE=64, 
        ///<summary> Internal communication failure. | </summary>
        [Description("Internal communication failure.")]
        DALY_FAILURE_INTERNAL_COMM_FAILURE=128, 
        
    };
    
    ///<summary> Flags for Byte 6 of the Daly BMS failure status: Other faults. Bits 4-7 are reserved. </summary>
    [Flags]
	public enum DALY_FAILURE_BYTE6_OTHER_FAULTS_FLAGS: byte
    {
        ///<summary> Current module fault. | </summary>
        [Description("Current module fault.")]
        DALY_FAILURE_CURRENT_MODULE_FAULT=1, 
        ///<summary> Sum voltage detect fault. | </summary>
        [Description("Sum voltage detect fault.")]
        DALY_FAILURE_SUM_VOLTAGE_DETECT_FAULT=2, 
        ///<summary> Short circuit protect fault. | </summary>
        [Description("Short circuit protect fault.")]
        DALY_FAILURE_SHORT_CIRCUIT_PROTECT_FAULT=4, 
        ///<summary> Low voltage forbidden charge fault. | </summary>
        [Description("Low voltage forbidden charge fault.")]
        DALY_FAILURE_LOW_VOLT_FORBIDDEN_CHG_FAULT=8, 
        
    };
    
    ///<summary> ESC Gear status derived from bits 2-0 from message II, byte 3 </summary>
    public enum EZKONTROL_STATUS_GEAR: int /*default*/
    {
        ///<summary> Gear: NO (Neutral/No Gear) | </summary>
        [Description("Gear: NO (Neutral/No Gear)")]
        EZKONTROL_GEAR_NO=0, 
        ///<summary> Gear: R (Reverse) | </summary>
        [Description("Gear: R (Reverse)")]
        EZKONTROL_GEAR_R=1, 
        ///<summary> Gear: N (Neutral) | </summary>
        [Description("Gear: N (Neutral)")]
        EZKONTROL_GEAR_N=2, 
        ///<summary> Gear: D1 (Drive 1) | </summary>
        [Description("Gear: D1 (Drive 1)")]
        EZKONTROL_GEAR_D1=3, 
        ///<summary> Gear: D2 (Drive 2) | </summary>
        [Description("Gear: D2 (Drive 2)")]
        EZKONTROL_GEAR_D2=4, 
        ///<summary> Gear: D3 (Drive 3) | </summary>
        [Description("Gear: D3 (Drive 3)")]
        EZKONTROL_GEAR_D3=5, 
        ///<summary> Gear: S (Sport/Special) | </summary>
        [Description("Gear: S (Sport/Special)")]
        EZKONTROL_GEAR_S=6, 
        ///<summary> Gear: P (Park) | </summary>
        [Description("Gear: P (Park)")]
        EZKONTROL_GEAR_P=7, 
        
    };
    
    ///<summary> ESC operation mode derived from bits 6-4 from message II, byte 3 </summary>
    public enum EZKONTROL_STATUS_OPERATION_MODE: int /*default*/
    {
        ///<summary> Operation Mode: Stop | </summary>
        [Description("Operation Mode: Stop")]
        EZKONTROL_OP_MODE_STOP=0, 
        ///<summary> Operation Mode: Drive | </summary>
        [Description("Operation Mode: Drive")]
        EZKONTROL_OP_MODE_DRIVE=1, 
        ///<summary> Operation Mode: Cruise | </summary>
        [Description("Operation Mode: Cruise")]
        EZKONTROL_OP_MODE_CRUISE=2, 
        ///<summary> Operation Mode: EBS (Electronic Braking System) | </summary>
        [Description("Operation Mode: EBS (Electronic Braking System)")]
        EZKONTROL_OP_MODE_EBS=3, 
        ///<summary> Operation Mode: Hold | </summary>
        [Description("Operation Mode: Hold")]
        EZKONTROL_OP_MODE_HOLD=4, 
        
    };
    
    ///<summary> Status flags from Byte 3 of EZkontrol MCU Message II (Brake, DC Contactor). </summary>
    [Flags]
	public enum EZKONTROL_STATUS_BYTE3_FLAGS: int /*default*/
    {
        ///<summary> Brake is active. | </summary>
        [Description("Brake is active.")]
        EZKONTROL_STATUS_BRAKE_ACTIVE=8, 
        ///<summary> DC Contactor is ON. | </summary>
        [Description("DC Contactor is ON.")]
        EZKONTROL_STATUS_DC_CONTACTOR_ON=128, 
        
    };
    
    ///<summary> Error flags from Byte 4 of EZkontrol MCU Message II. </summary>
    [Flags]
	public enum EZKONTROL_ERROR_BYTE4_FLAGS: byte
    {
        ///<summary> Error: Overcurrent. | </summary>
        [Description("Error: Overcurrent.")]
        EZKONTROL_ERROR_OVERCURRENT=1, 
        ///<summary> Error: Overload. | </summary>
        [Description("Error: Overload.")]
        EZKONTROL_ERROR_OVERLOAD=2, 
        ///<summary> Error: Overvoltage. | </summary>
        [Description("Error: Overvoltage.")]
        EZKONTROL_ERROR_OVERVOLTAGE=4, 
        ///<summary> Error: Undervoltage. | </summary>
        [Description("Error: Undervoltage.")]
        EZKONTROL_ERROR_UNDERVOLTAGE=8, 
        ///<summary> Error: Controller Overheat. | </summary>
        [Description("Error: Controller Overheat.")]
        EZKONTROL_ERROR_CONTROLLER_OVERHEAT=16, 
        ///<summary> Error: Motor Overheat. | </summary>
        [Description("Error: Motor Overheat.")]
        EZKONTROL_ERROR_MOTOR_OVERHEAT=32, 
        ///<summary> Error: Motor Stalled. | </summary>
        [Description("Error: Motor Stalled.")]
        EZKONTROL_ERROR_MOTOR_STALLED=64, 
        ///<summary> Error: Motor Out of phase. | </summary>
        [Description("Error: Motor Out of phase.")]
        EZKONTROL_ERROR_MOTOR_OUT_OF_PHASE=128, 
        
    };
    
    ///<summary> Error flags from Byte 5 of EZkontrol MCU Message II. </summary>
    [Flags]
	public enum EZKONTROL_ERROR_BYTE5_FLAGS: byte
    {
        ///<summary> Error: Motor Sensor. | </summary>
        [Description("Error: Motor Sensor.")]
        EZKONTROL_ERROR_MOTOR_SENSOR=1, 
        ///<summary> Error: Motor AUX Sensor. | </summary>
        [Description("Error: Motor AUX Sensor.")]
        EZKONTROL_ERROR_MOTOR_AUX_SENSOR=2, 
        ///<summary> Error: Encoder Misaligned. | </summary>
        [Description("Error: Encoder Misaligned.")]
        EZKONTROL_ERROR_ENCODER_MISALIGNED=4, 
        ///<summary> Error: Anti-Runaway Engaged. | </summary>
        [Description("Error: Anti-Runaway Engaged.")]
        EZKONTROL_ERROR_ANTI_RUNAWAY_ENGAGED=8, 
        ///<summary> Error: Main Accelerator. | </summary>
        [Description("Error: Main Accelerator.")]
        EZKONTROL_ERROR_MAIN_ACCELERATOR=16, 
        ///<summary> Error: AUX Accelerator. | </summary>
        [Description("Error: AUX Accelerator.")]
        EZKONTROL_ERROR_AUX_ACCELERATOR=32, 
        ///<summary> Error: Pre-charge. | </summary>
        [Description("Error: Pre-charge.")]
        EZKONTROL_ERROR_PRE_CHARGE=64, 
        ///<summary> Error: DC Contactor fault (distinct from status). | </summary>
        [Description("Error: DC Contactor fault (distinct from status).")]
        EZKONTROL_ERROR_DC_CONTACTOR_FAULT=128, 
        
    };
    
    ///<summary> Error flags from Byte 6 of EZkontrol MCU Message II. </summary>
    [Flags]
	public enum EZKONTROL_ERROR_BYTE6_FLAGS: byte
    {
        ///<summary> Error: Power valve. | </summary>
        [Description("Error: Power valve.")]
        EZKONTROL_ERROR_POWER_VALVE=1, 
        ///<summary> Error: Current Sensor. | </summary>
        [Description("Error: Current Sensor.")]
        EZKONTROL_ERROR_CURRENT_SENSOR=2, 
        ///<summary> Error: Auto-tune. | </summary>
        [Description("Error: Auto-tune.")]
        EZKONTROL_ERROR_AUTO_TUNE=4, 
        ///<summary> Error: RS485 communication. | </summary>
        [Description("Error: RS485 communication.")]
        EZKONTROL_ERROR_RS485=8, 
        ///<summary> Error: CAN communication. | </summary>
        [Description("Error: CAN communication.")]
        EZKONTROL_ERROR_CAN_COMM=16, 
        ///<summary> Error: Software. | </summary>
        [Description("Error: Software.")]
        EZKONTROL_ERROR_SOFTWARE=32, 
        
    };
    
    ///<summary> Specifies the datatype of a MAVLink parameter. </summary>
    public enum MAV_PARAM_TYPE: byte
    {
        ///<summary> 8-bit unsigned integer | </summary>
        [Description("8-bit unsigned integer")]
        UINT8=1, 
        ///<summary> 8-bit signed integer | </summary>
        [Description("8-bit signed integer")]
        INT8=2, 
        ///<summary> 16-bit unsigned integer | </summary>
        [Description("16-bit unsigned integer")]
        UINT16=3, 
        ///<summary> 16-bit signed integer | </summary>
        [Description("16-bit signed integer")]
        INT16=4, 
        ///<summary> 32-bit unsigned integer | </summary>
        [Description("32-bit unsigned integer")]
        UINT32=5, 
        ///<summary> 32-bit signed integer | </summary>
        [Description("32-bit signed integer")]
        INT32=6, 
        ///<summary> 64-bit unsigned integer | </summary>
        [Description("64-bit unsigned integer")]
        UINT64=7, 
        ///<summary> 64-bit signed integer | </summary>
        [Description("64-bit signed integer")]
        INT64=8, 
        ///<summary> 32-bit floating-point | </summary>
        [Description("32-bit floating-point")]
        REAL32=9, 
        ///<summary> 64-bit floating-point | </summary>
        [Description("64-bit floating-point")]
        REAL64=10, 
        
    };
    
    ///<summary> Indicates the severity level, generally used for status messages to indicate their relative urgency. Based on RFC-5424 using expanded definitions at: http://www.kiwisyslog.com/kb/info:-syslog-message-levels/. </summary>
    public enum MAV_SEVERITY: byte
    {
        ///<summary> System is unusable. This is a 'panic' condition. | </summary>
        [Description("System is unusable. This is a 'panic' condition.")]
        EMERGENCY=0, 
        ///<summary> Action should be taken immediately. Indicates error in non-critical systems. | </summary>
        [Description("Action should be taken immediately. Indicates error in non-critical systems.")]
        ALERT=1, 
        ///<summary> Action must be taken immediately. Indicates failure in a primary system. | </summary>
        [Description("Action must be taken immediately. Indicates failure in a primary system.")]
        CRITICAL=2, 
        ///<summary> Indicates an error in secondary/redundant systems. | </summary>
        [Description("Indicates an error in secondary/redundant systems.")]
        ERROR=3, 
        ///<summary> Indicates about a possible future error if this is not resolved within a given timeframe. Example would be a low battery warning. | </summary>
        [Description("Indicates about a possible future error if this is not resolved within a given timeframe. Example would be a low battery warning.")]
        WARNING=4, 
        ///<summary> An unusual event has occurred, though not an error condition. This should be investigated for the root cause. | </summary>
        [Description("An unusual event has occurred, though not an error condition. This should be investigated for the root cause.")]
        NOTICE=5, 
        ///<summary> Normal operational messages. Useful for logging. No action is required for these messages. | </summary>
        [Description("Normal operational messages. Useful for logging. No action is required for these messages.")]
        INFO=6, 
        ///<summary> Useful non-operational messages that can assist in debugging. These should not occur during normal operation. | </summary>
        [Description("Useful non-operational messages that can assist in debugging. These should not occur during normal operation.")]
        DEBUG=7, 
        
    };
    
    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=22)]
    ///<summary> Instrumentation PCBs </summary>
    public struct mavlink_instrumentation_t
    {
        public mavlink_instrumentation_t(uint timestamp_seconds,short battery_current,short motor_current_left,short motor_current_right,short mppt_current,short auxiliary_battery_current,ushort battery_voltage,ushort auxiliary_battery_voltage,ushort irradiance,ushort timestamp_milliseconds) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.battery_current = battery_current;
              this.motor_current_left = motor_current_left;
              this.motor_current_right = motor_current_right;
              this.mppt_current = mppt_current;
              this.auxiliary_battery_current = auxiliary_battery_current;
              this.battery_voltage = battery_voltage;
              this.auxiliary_battery_voltage = auxiliary_battery_voltage;
              this.irradiance = irradiance;
              this.timestamp_milliseconds = timestamp_milliseconds;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>  [cA] </summary>
        [Units("[cA]")]
        [Description("")]
        public  short battery_current;
            /// <summary>  [cA] </summary>
        [Units("[cA]")]
        [Description("")]
        public  short motor_current_left;
            /// <summary>  [cA] </summary>
        [Units("[cA]")]
        [Description("")]
        public  short motor_current_right;
            /// <summary>  [cA] </summary>
        [Units("[cA]")]
        [Description("")]
        public  short mppt_current;
            /// <summary>  [cA] </summary>
        [Units("[cA]")]
        [Description("")]
        public  short auxiliary_battery_current;
            /// <summary>  [cV] </summary>
        [Units("[cV]")]
        [Description("")]
        public  ushort battery_voltage;
            /// <summary>  [cV] </summary>
        [Units("[cV]")]
        [Description("")]
        public  ushort auxiliary_battery_voltage;
            /// <summary>  [W/m^2] </summary>
        [Units("[W/m^2]")]
        [Description("")]
        public  ushort irradiance;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=14)]
    ///<summary>  Temperature data for motor and MPPT. </summary>
    public struct mavlink_temperatures_t
    {
        public mavlink_temperatures_t(uint timestamp_seconds,short temperature_battery_left,short temperature_battery_right,short temperature_mppt_left,short temperature_mppt_right,ushort timestamp_milliseconds) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.temperature_battery_left = temperature_battery_left;
              this.temperature_battery_right = temperature_battery_right;
              this.temperature_mppt_left = temperature_mppt_left;
              this.temperature_mppt_right = temperature_mppt_right;
              this.timestamp_milliseconds = timestamp_milliseconds;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Left side of battery pack  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("Left side of battery pack")]
        public  short temperature_battery_left;
            /// <summary>Right side of battery pack  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("Right side of battery pack")]
        public  short temperature_battery_right;
            /// <summary>MPPT temperature on its left side.  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("MPPT temperature on its left side.")]
        public  short temperature_mppt_left;
            /// <summary>MPPT temperature on its left side.  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("MPPT temperature on its left side.")]
        public  short temperature_mppt_right;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=20)]
    ///<summary>  </summary>
    public struct mavlink_gps_t
    {
        public mavlink_gps_t(int latitude,int longitude,uint timestamp_seconds,ushort speed,ushort timestamp_milliseconds,byte course,byte heading,byte satellites_visible,byte hdop) 
        {
              this.latitude = latitude;
              this.longitude = longitude;
              this.timestamp_seconds = timestamp_seconds;
              this.speed = speed;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.course = course;
              this.heading = heading;
              this.satellites_visible = satellites_visible;
              this.hdop = hdop;
            
        }
        /// <summary>  [degE7] </summary>
        [Units("[degE7]")]
        [Description("")]
        public  int latitude;
            /// <summary>  [degE7] </summary>
        [Units("[degE7]")]
        [Description("")]
        public  int longitude;
            /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>  [cm/s] </summary>
        [Units("[cm/s]")]
        [Description("")]
        public  ushort speed;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Direction of movement(not heading)  [deg] </summary>
        [Units("[deg]")]
        [Description("Direction of movement(not heading)")]
        public  byte course;
            /// <summary>Direction of the bow (source not from GPS, but from IMU)  [deg] </summary>
        [Units("[deg]")]
        [Description("Direction of the bow (source not from GPS, but from IMU)")]
        public  byte heading;
            /// <summary>   </summary>
        [Units("")]
        [Description("")]
        public  byte satellites_visible;
            /// <summary>Horizontal dilution of precision. Represents quality of GPS constellation. Values close to 1 are best. Above 5 is poor   </summary>
        [Units("")]
        [Description("Horizontal dilution of precision. Represents quality of GPS constellation. Values close to 1 are best. Above 5 is poor")]
        public  byte hdop;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=14)]
    ///<summary> MPPT data fields </summary>
    public struct mavlink_mppt_t
    {
        public mavlink_mppt_t(uint timestamp_seconds,ushort pv_voltage,short pv_current,ushort battery_voltage,short battery_current,ushort timestamp_milliseconds) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.pv_voltage = pv_voltage;
              this.pv_current = pv_current;
              this.battery_voltage = battery_voltage;
              this.battery_current = battery_current;
              this.timestamp_milliseconds = timestamp_milliseconds;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Solar array voltage,ID=3100  [cV] </summary>
        [Units("[cV]")]
        [Description("Solar array voltage,ID=3100")]
        public  ushort pv_voltage;
            /// <summary>Solar array current,ID=3101  [cV] </summary>
        [Units("[cV]")]
        [Description("Solar array current,ID=3101")]
        public  short pv_current;
            /// <summary>Battery voltage,ID=3104  [cV] </summary>
        [Units("[cV]")]
        [Description("Battery voltage,ID=3104")]
        public  ushort battery_voltage;
            /// <summary>Net battery current(positive=charging),ID=331B  [cA] </summary>
        [Units("[cA]")]
        [Description("Net battery current(positive=charging),ID=331B")]
        public  short battery_current;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=10)]
    ///<summary> MPPT error codes </summary>
    public struct mavlink_mppt_state_t
    {
        public mavlink_mppt_state_t(uint timestamp_seconds,/*MPPT_BATTERY_STATUS*/ushort battery_status,/*MPPT_CHARGING_EQUIPMENT_STATUS*/ushort charging_equipment_status,ushort timestamp_milliseconds) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.battery_status = battery_status;
              this.charging_equipment_status = charging_equipment_status;
              this.timestamp_milliseconds = timestamp_milliseconds;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Status of the battery,ID=3200. MPPT_BATTERY_STATUS  </summary>
        [Units("")]
        [Description("Status of the battery,ID=3200.")]
        public  /*MPPT_BATTERY_STATUS*/ushort battery_status;
            /// <summary>Status of the MPPT,ID=3201. MPPT_CHARGING_EQUIPMENT_STATUS  </summary>
        [Units("")]
        [Description("Status of the MPPT,ID=3201.")]
        public  /*MPPT_CHARGING_EQUIPMENT_STATUS*/ushort charging_equipment_status;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=45)]
    ///<summary> Battery information. </summary>
    public struct mavlink_bms_t
    {
        public mavlink_bms_t(uint timestamp_seconds,ushort[] voltages,short[] temperatures,short current_battery,ushort timestamp_milliseconds,sbyte state_of_charge) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.voltages = voltages;
              this.temperatures = temperatures;
              this.current_battery = current_battery;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.state_of_charge = state_of_charge;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Voltage of each cell.  [mV] </summary>
        [Units("[mV]")]
        [Description("Voltage of each cell.")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=16)]
		public ushort[] voltages;
            /// <summary>Temperature of the battery  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("Temperature of the battery")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
		public short[] temperatures;
            /// <summary>Battery current  [dA] </summary>
        [Units("[dA]")]
        [Description("Battery current")]
        public  short current_battery;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Remaining battery energy. Values: [0-100]  [%] </summary>
        [Units("[%]")]
        [Description("Remaining battery energy. Values: [0-100]")]
        public  sbyte state_of_charge;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=19)]
    ///<summary>          Daly BMS battery failure status (corresponds to original message 0x98).         Contains multiple bytes of failure flags and a final fault code.         Also includes temperatures and status bitfield of the battery management system (FET states, stationary, charging or discharging)        </summary>
    public struct mavlink_bms_status_t
    {
        public mavlink_bms_status_t(uint timestamp_seconds,short[] temperatures,ushort timestamp_milliseconds,/*BMS_CHARGE_DISCHARGE_STATUS_FLAGS*/byte status,/*DALY_FAILURE_BYTE0_VOLTAGE_FLAGS*/byte failure_flags_byte0,/*DALY_FAILURE_BYTE1_TEMP_FLAGS*/byte failure_flags_byte1,/*DALY_FAILURE_BYTE2_CURRENT_SOC_FLAGS*/byte failure_flags_byte2,/*DALY_FAILURE_BYTE3_DIFFERENTIAL_FLAGS*/byte failure_flags_byte3,/*DALY_FAILURE_BYTE4_MOSFET_SENSOR_FLAGS*/byte failure_flags_byte4,/*DALY_FAILURE_BYTE5_SYSTEM_COMPONENT_FLAGS*/byte failure_flags_byte5,/*DALY_FAILURE_BYTE6_OTHER_FAULTS_FLAGS*/byte failure_flags_byte6,byte fault_code_byte7) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.temperatures = temperatures;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.status = status;
              this.failure_flags_byte0 = failure_flags_byte0;
              this.failure_flags_byte1 = failure_flags_byte1;
              this.failure_flags_byte2 = failure_flags_byte2;
              this.failure_flags_byte3 = failure_flags_byte3;
              this.failure_flags_byte4 = failure_flags_byte4;
              this.failure_flags_byte5 = failure_flags_byte5;
              this.failure_flags_byte6 = failure_flags_byte6;
              this.fault_code_byte7 = fault_code_byte7;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Temperature of the battery  [cdegC] </summary>
        [Units("[cdegC]")]
        [Description("Temperature of the battery")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=2)]
		public short[] temperatures;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Charge/Discharge FET status and current charging status BMS_CHARGE_DISCHARGE_STATUS_FLAGS  </summary>
        [Units("")]
        [Description("Charge/Discharge FET status and current charging status")]
        public  /*BMS_CHARGE_DISCHARGE_STATUS_FLAGS*/byte status;
            /// <summary>Failure flags: Voltage related (Byte 0). DALY_FAILURE_BYTE0_VOLTAGE_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: Voltage related (Byte 0).")]
        public  /*DALY_FAILURE_BYTE0_VOLTAGE_FLAGS*/byte failure_flags_byte0;
            /// <summary>Failure flags: Temperature related (Byte 1). DALY_FAILURE_BYTE1_TEMP_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: Temperature related (Byte 1).")]
        public  /*DALY_FAILURE_BYTE1_TEMP_FLAGS*/byte failure_flags_byte1;
            /// <summary>Failure flags: Current and SOC related (Byte 2). DALY_FAILURE_BYTE2_CURRENT_SOC_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: Current and SOC related (Byte 2).")]
        public  /*DALY_FAILURE_BYTE2_CURRENT_SOC_FLAGS*/byte failure_flags_byte2;
            /// <summary>Failure flags: Differential V/T related (Byte 3). DALY_FAILURE_BYTE3_DIFFERENTIAL_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: Differential V/T related (Byte 3).")]
        public  /*DALY_FAILURE_BYTE3_DIFFERENTIAL_FLAGS*/byte failure_flags_byte3;
            /// <summary>Failure flags: MOSFET and sensor related (Byte 4). DALY_FAILURE_BYTE4_MOSFET_SENSOR_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: MOSFET and sensor related (Byte 4).")]
        public  /*DALY_FAILURE_BYTE4_MOSFET_SENSOR_FLAGS*/byte failure_flags_byte4;
            /// <summary>Failure flags: System/Component related (Byte 5). DALY_FAILURE_BYTE5_SYSTEM_COMPONENT_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: System/Component related (Byte 5).")]
        public  /*DALY_FAILURE_BYTE5_SYSTEM_COMPONENT_FLAGS*/byte failure_flags_byte5;
            /// <summary>Failure flags: Other faults (Byte 6). DALY_FAILURE_BYTE6_OTHER_FAULTS_FLAGS  </summary>
        [Units("")]
        [Description("Failure flags: Other faults (Byte 6).")]
        public  /*DALY_FAILURE_BYTE6_OTHER_FAULTS_FLAGS*/byte failure_flags_byte6;
            /// <summary>Raw fault code (Byte 7 from original message).   </summary>
        [Units("")]
        [Description("Raw fault code (Byte 7 from original message).")]
        public  byte fault_code_byte7;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=14)]
    ///<summary>        Electrical data from EZkontrol ESC based on CAN Message I, including accelerator opening for higher frequency publishing.       Phase current is excluded because it updates too rapidly for low bandwidth links        </summary>
    public struct mavlink_ezkontrol_mcu_meter_data_i_t
    {
        public mavlink_ezkontrol_mcu_meter_data_i_t(uint timestamp_seconds,ushort bus_voltage,short bus_current,short rpm,ushort timestamp_milliseconds,byte accelerator_opening,byte instance) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.bus_voltage = bus_voltage;
              this.bus_current = bus_current;
              this.rpm = rpm;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.accelerator_opening = accelerator_opening;
              this.instance = instance;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>0.1V/bit, offset= 0  [dV] </summary>
        [Units("[dV]")]
        [Description("0.1V/bit, offset= 0")]
        public  ushort bus_voltage;
            /// <summary>0.1A/bit, offset= -32000  [dA] </summary>
        [Units("[dA]")]
        [Description("0.1A/bit, offset= -32000")]
        public  short bus_current;
            /// <summary>0.1rpm/bit, offset= -32000  [drpm/bit] </summary>
        [Units("[drpm/bit]")]
        [Description("0.1rpm/bit, offset= -32000")]
        public  short rpm;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>1%/bit, throttle signal   </summary>
        [Units("")]
        [Description("1%/bit, throttle signal")]
        public  byte accelerator_opening;
            /// <summary>0 for left motor, 1 for right motor   </summary>
        [Units("")]
        [Description("0 for left motor, 1 for right motor")]
        public  byte instance;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=14)]
    ///<summary> Status data from EZkontrol ESC based on CAN Message II, excluding accelerator opening. Temperatures have an offset of -40 degrees Celsius. </summary>
    public struct mavlink_ezkontrol_mcu_meter_data_ii_t
    {
        public mavlink_ezkontrol_mcu_meter_data_ii_t(uint timestamp_seconds,ushort timestamp_milliseconds,sbyte controller_temperature,sbyte motor_temperature,byte status,/*EZKONTROL_ERROR_BYTE4_FLAGS*/byte error_flags_byte4,/*EZKONTROL_ERROR_BYTE5_FLAGS*/byte error_flags_byte5,/*EZKONTROL_ERROR_BYTE6_FLAGS*/byte error_flags_byte6,byte life_signal,byte instance) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.controller_temperature = controller_temperature;
              this.motor_temperature = motor_temperature;
              this.status = status;
              this.error_flags_byte4 = error_flags_byte4;
              this.error_flags_byte5 = error_flags_byte5;
              this.error_flags_byte6 = error_flags_byte6;
              this.life_signal = life_signal;
              this.instance = instance;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Controller temperature. Actual value = MAVLink value - 40. Unit: degC   </summary>
        [Units("")]
        [Description("Controller temperature. Actual value = MAVLink value - 40. Unit: degC")]
        public  sbyte controller_temperature;
            /// <summary>Motor temperature. Actual value = MAVLink value - 40. Unit: degC   </summary>
        [Units("")]
        [Description("Motor temperature. Actual value = MAVLink value - 40. Unit: degC")]
        public  sbyte motor_temperature;
            /// <summary>Gear, brake, operation mode and DC contactor.   </summary>
        [Units("")]
        [Description("Gear, brake, operation mode and DC contactor.")]
        public  byte status;
            /// <summary>Error flags from Byte 4. EZKONTROL_ERROR_BYTE4_FLAGS  </summary>
        [Units("")]
        [Description("Error flags from Byte 4.")]
        public  /*EZKONTROL_ERROR_BYTE4_FLAGS*/byte error_flags_byte4;
            /// <summary>Error flags from Byte 5. EZKONTROL_ERROR_BYTE5_FLAGS  </summary>
        [Units("")]
        [Description("Error flags from Byte 5.")]
        public  /*EZKONTROL_ERROR_BYTE5_FLAGS*/byte error_flags_byte5;
            /// <summary>Error flags from Byte 6. EZKONTROL_ERROR_BYTE6_FLAGS  </summary>
        [Units("")]
        [Description("Error flags from Byte 6.")]
        public  /*EZKONTROL_ERROR_BYTE6_FLAGS*/byte error_flags_byte6;
            /// <summary>Life signal counter derived from bits 7-4 from byte 7, msg II.   </summary>
        [Units("")]
        [Description("Life signal counter derived from bits 7-4 from byte 7, msg II.")]
        public  byte life_signal;
            /// <summary>0 for left motor, 1 for right motor   </summary>
        [Units("")]
        [Description("0 for left motor, 1 for right motor")]
        public  byte instance;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=7)]
    ///<summary> State of bilge pumps. </summary>
    public struct mavlink_pumps_t
    {
        public mavlink_pumps_t(uint timestamp_seconds,ushort timestamp_milliseconds,byte pump_states) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.pump_states = pump_states;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Bitmask of pump states. Bit 0: Pump Left, Bit 1: Pump Right   </summary>
        [Units("")]
        [Description("Bitmask of pump states. Bit 0: Pump Left, Bit 1: Pump Right")]
        public  byte pump_states;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=18)]
    ///<summary> Request to read the onboard parameter with the param_id string id. Onboard parameters are stored as key[const char*] -> value[float]. This allows to send a parameter to any other component (such as the GCS) without the need of previous knowledge of possible parameter names. Thus the same GCS can store different parameters for different autopilots. See also https://mavlink.io/en/services/parameter.html for a full documentation of QGroundControl and IMU code. </summary>
    public struct mavlink_param_request_read_t
    {
        public mavlink_param_request_read_t(short param_index,byte[] param_id) 
        {
              this.param_index = param_index;
              this.param_id = param_id;
            
        }
        /// <summary>Parameter index. Send -1 to use the param ID field as identifier (else the param id will be ignored)   </summary>
        [Units("")]
        [Description("Parameter index. Send -1 to use the param ID field as identifier (else the param id will be ignored)")]
        public  short param_index;
            /// <summary>Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string   </summary>
        [Units("")]
        [Description("Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=16)]
		public byte[] param_id;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=25)]
    ///<summary> Emit the value of a onboard parameter. The inclusion of param_count and param_index in the message allows the recipient to keep track of received parameters and allows him to re-request missing parameters after a loss or timeout. The parameter microservice is documented at https://mavlink.io/en/services/parameter.html </summary>
    public struct mavlink_param_value_t
    {
        public mavlink_param_value_t(float param_value,ushort param_count,ushort param_index,byte[] param_id,/*MAV_PARAM_TYPE*/byte param_type) 
        {
              this.param_value = param_value;
              this.param_count = param_count;
              this.param_index = param_index;
              this.param_id = param_id;
              this.param_type = param_type;
            
        }
        /// <summary>Onboard parameter value   </summary>
        [Units("")]
        [Description("Onboard parameter value")]
        public  float param_value;
            /// <summary>Total number of onboard parameters   </summary>
        [Units("")]
        [Description("Total number of onboard parameters")]
        public  ushort param_count;
            /// <summary>Index of this onboard parameter   </summary>
        [Units("")]
        [Description("Index of this onboard parameter")]
        public  ushort param_index;
            /// <summary>Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string   </summary>
        [Units("")]
        [Description("Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=16)]
		public byte[] param_id;
            /// <summary>Onboard parameter type. MAV_PARAM_TYPE  </summary>
        [Units("")]
        [Description("Onboard parameter type.")]
        public  /*MAV_PARAM_TYPE*/byte param_type;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=21)]
    ///<summary> Set a parameter value (write new value to permanent storage).         The receiving component should acknowledge the new parameter value by broadcasting a PARAM_VALUE message (broadcasting ensures that multiple GCS all have an up-to-date list of all parameters). If the sending GCS did not receive a PARAM_VALUE within its timeout time, it should re-send the PARAM_SET message. The parameter microservice is documented at https://mavlink.io/en/services/parameter.html.        </summary>
    public struct mavlink_param_set_t
    {
        public mavlink_param_set_t(float param_value,byte[] param_id,/*MAV_PARAM_TYPE*/byte param_type) 
        {
              this.param_value = param_value;
              this.param_id = param_id;
              this.param_type = param_type;
            
        }
        /// <summary>Onboard parameter value   </summary>
        [Units("")]
        [Description("Onboard parameter value")]
        public  float param_value;
            /// <summary>Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string   </summary>
        [Units("")]
        [Description("Onboard parameter id, terminated by NULL if the length is less than 16 human-readable chars and WITHOUT null termination (NULL) byte if the length is exactly 16 chars - applications have to provide 16+1 bytes storage if the ID is stored as string")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=16)]
		public byte[] param_id;
            /// <summary>Onboard parameter type. MAV_PARAM_TYPE  </summary>
        [Units("")]
        [Description("Onboard parameter type.")]
        public  /*MAV_PARAM_TYPE*/byte param_type;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=10)]
    ///<summary> Status generated by radio and injected into MAVLink stream. </summary>
    public struct mavlink_radio_status_t
    {
        public mavlink_radio_status_t(uint timestamp_seconds,ushort rxerrors,ushort timestamp_milliseconds,byte instance,byte rssi) 
        {
              this.timestamp_seconds = timestamp_seconds;
              this.rxerrors = rxerrors;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.instance = instance;
              this.rssi = rssi;
            
        }
        /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Count of radio packet receive errors (since boot).   </summary>
        [Units("")]
        [Description("Count of radio packet receive errors (since boot).")]
        public  ushort rxerrors;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Radio instance, 0 for primary radio, 1 for secondary radio.   </summary>
        [Units("")]
        [Description("Radio instance, 0 for primary radio, 1 for secondary radio.")]
        public  byte instance;
            /// <summary>Local (message sender) received signal strength indication in device-dependent units/scale. Values: [0-254], UINT8_MAX: invalid/unknown.   </summary>
        [Units("")]
        [Description("Local (message sender) received signal strength indication in device-dependent units/scale. Values: [0-254], UINT8_MAX: invalid/unknown.")]
        public  byte rssi;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=24)]
    ///<summary> Send a key-value pair as float. The use of this message is discouraged for normal packets, but a quite efficient way for testing new messages and getting experimental debug output. </summary>
    public struct mavlink_named_value_float_t
    {
        public mavlink_named_value_float_t(uint time_boot_ms,float value,uint timestamp_seconds,ushort timestamp_milliseconds,byte[] name) 
        {
              this.time_boot_ms = time_boot_ms;
              this.value = value;
              this.timestamp_seconds = timestamp_seconds;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.name = name;
            
        }
        /// <summary>Timestamp (time since system boot).  [ms] </summary>
        [Units("[ms]")]
        [Description("Timestamp (time since system boot).")]
        public  uint time_boot_ms;
            /// <summary>Floating point value   </summary>
        [Units("")]
        [Description("Floating point value")]
        public  float value;
            /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Name of the debug variable   </summary>
        [Units("")]
        [Description("Name of the debug variable")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=10)]
		public byte[] name;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=24)]
    ///<summary> Send a key-value pair as integer. The use of this message is discouraged for normal packets, but a quite efficient way for testing new messages and getting experimental debug output. </summary>
    public struct mavlink_named_value_int_t
    {
        public mavlink_named_value_int_t(uint time_boot_ms,int value,uint timestamp_seconds,ushort timestamp_milliseconds,byte[] name) 
        {
              this.time_boot_ms = time_boot_ms;
              this.value = value;
              this.timestamp_seconds = timestamp_seconds;
              this.timestamp_milliseconds = timestamp_milliseconds;
              this.name = name;
            
        }
        /// <summary>Timestamp (time since system boot).  [ms] </summary>
        [Units("[ms]")]
        [Description("Timestamp (time since system boot).")]
        public  uint time_boot_ms;
            /// <summary>Signed integer value   </summary>
        [Units("")]
        [Description("Signed integer value")]
        public  int value;
            /// <summary>Seconds since Unix time   </summary>
        [Units("")]
        [Description("Seconds since Unix time")]
        public  uint timestamp_seconds;
            /// <summary>Milliseconds within Unix time   </summary>
        [Units("")]
        [Description("Milliseconds within Unix time")]
        public  ushort timestamp_milliseconds;
            /// <summary>Name of the debug variable   </summary>
        [Units("")]
        [Description("Name of the debug variable")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=10)]
		public byte[] name;
    
    };

    
    /// extensions_start 2
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=51)]
    ///<summary> Status text message. These messages are printed in yellow in the COMM console of QGroundControl. WARNING: They consume quite some bandwidth, so use only for important status and error messages. If implemented wisely, these messages are buffered on the MCU and sent only at a limited rate (e.g. 10 Hz). </summary>
    public struct mavlink_statustext_t
    {
        public mavlink_statustext_t(/*MAV_SEVERITY*/byte severity,byte[] text) 
        {
              this.severity = severity;
              this.text = text;
            
        }
        /// <summary>Severity of status. Relies on the definitions within RFC-5424. MAV_SEVERITY  </summary>
        [Units("")]
        [Description("Severity of status. Relies on the definitions within RFC-5424.")]
        public  /*MAV_SEVERITY*/byte severity;
            /// <summary>Status text message, without null termination character   </summary>
        [Units("")]
        [Description("Status text message, without null termination character")]
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=50)]
		public byte[] text;
    
    };

    
    /// extensions_start 0
    [StructLayout(LayoutKind.Sequential,Pack=1,Size=9)]
    ///<summary> Send a debug value. The index is used to discriminate between values. These values show up in the plot of QGroundControl as DEBUG N. </summary>
    public struct mavlink_debug_t
    {
        public mavlink_debug_t(uint time_boot_ms,float value,byte ind) 
        {
              this.time_boot_ms = time_boot_ms;
              this.value = value;
              this.ind = ind;
            
        }
        /// <summary>Timestamp (time since system boot).  [ms] </summary>
        [Units("[ms]")]
        [Description("Timestamp (time since system boot).")]
        public  uint time_boot_ms;
            /// <summary>DEBUG value   </summary>
        [Units("")]
        [Description("DEBUG value")]
        public  float value;
            /// <summary>index of debug variable   </summary>
        [Units("")]
        [Description("index of debug variable")]
        public  byte ind;
    
    };

}

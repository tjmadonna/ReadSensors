using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Text.RegularExpressions;

namespace ReadSensors
{
    public partial class ReadSensor : Form
    {

        // Calibration factors for the sensors
        double dbl_Biceps_Load_Cal_Factor = 0.0213; // Converts raw output to newtons
        double dbl_Forearm_Load_Cal_Factor = 0.0223; // Converts raw output to newtons
        double dbl_Forearm_Torque_Cal_Factor = 0.2727; // Converts raw output to newton-mm
        double dbl_Rotation_Cal_Factor = -1;

        // DataSet is handle stored data from sensors
        DataSet _ds = new DataSet();

        // Data Row to be added to the Data set
        DataRow _dr = null;

        // Serial ports for sensor 1 and sensor 2
        SerialPort serialPort_Sensor1 = new SerialPort();
        SerialPort serialPort_Sensor2 = new SerialPort();

        // Program and data timing values
        int int_ThreadSleep_Sensor = 25;

        // Sensor 1 COM Port configuration
        string str_Sensor1_COMPort = "";
        int int_Sensor1_BaudRate = 0;
        int int_Sensor1_DataBit = 0;
        string str_Sensor1_Parity = "";
        string str_Sensor1_StopBit = "";
        string str_Sensor1_FlowControl = "";
        int int_Sensor1_ReadTimeout = 0;

        // Sensor 2 COM Port configuration
        string str_Sensor2_COMPort = "";
        int int_Sensor2_BaudRate = 0;
        int int_Sensor2_DataBit = 0;
        string str_Sensor2_Parity = "";
        string str_Sensor2_StopBit = "";
        string str_Sensor2_FlowControl = "";
        int int_Sensor2_ReadTimeout = 0;

        // For reading serial data from sensors
        string str_COMData_Sensor1 = "";
        string str_COMData_Sensor2 = "";
        string str_Load_Sensor1 = "";
        string str_Load_Sensor2 = "";

        // Testing configurations
        int int_Specimen_Number = 0;
        string str_Test_Type = "";
        int int_Trial_Number = 0;

        // Indicator for when data received from COMport
        bool bool_COMData_Received = false;

        // Delegates for updating GUI
        // The delegate solves issue of concurrency with updating UI (label/DataGridView) in the main thread
        private delegate void deleg_DGV_Update(string str_Sensor1_Data, string str_Sensor2_Data);

        // Application Startup and Close
        #region Application Startup and Close

        public ReadSensor()
        {
            InitializeComponent();
        }

        private void ReadSensor_Load(object sender, EventArgs e)
        {
            try
            {
                // Load Default Settings for Thread Sleep and Cycle Timeout from .config file
                int_ThreadSleep_Sensor = Properties.Settings.Default.ThreadSleep_Sensor;
               // int_ThreadSleep_Sensor2 = Properties.Settings.Default.ThreadSleep_Sensor2;
               // int_CycleTimeout = Properties.Settings.Default.CycleTimeout;

                // Load Default Settings for Sensor 1 COM Port Configuration from .config file
                int_Sensor1_BaudRate = Properties.Settings.Default.Sensor1_COM_Baud;
                int_Sensor1_DataBit = Properties.Settings.Default.Sensor1_COM_DataBit;
                str_Sensor1_Parity = Properties.Settings.Default.Sensor1_COM_Parity;
                str_Sensor1_StopBit = Properties.Settings.Default.Sensor1_COM_StopBit;
                str_Sensor1_FlowControl = Properties.Settings.Default.Sensor1_COM_Flow;
                int_Sensor1_ReadTimeout = Properties.Settings.Default.Sensor1_COM_Timeout;

                // Load Default Settings for Sensor 2 COM Port Configuration from .config file
                int_Sensor2_BaudRate = Properties.Settings.Default.Sensor2_COM_Baud;
                int_Sensor2_DataBit = Properties.Settings.Default.Sensor2_COM_DataBit;
                str_Sensor2_Parity = Properties.Settings.Default.Sensor2_COM_Parity;
                str_Sensor2_StopBit = Properties.Settings.Default.Sensor2_COM_StopBit;
                str_Sensor2_FlowControl = Properties.Settings.Default.Sensor2_COM_Flow;
                int_Sensor2_ReadTimeout = Properties.Settings.Default.Sensor2_COM_Timeout;

                // Get TestTypes from database names on server
                _ds.Tables.Add("Sensor_Data");

                // Set Up DataGridView Columns
                dgv_Sensor_Data.Columns.Add("Sensor1", "Sensor1");
                dgv_Sensor_Data.Columns.Add("Sensor2", "Sensor2");

                // Set Combo Box (Drop Down) for COM Port Selection to be Read Only
                dd_Sensor1_COMPort.DropDownStyle = ComboBoxStyle.DropDownList;
                dd_Sensor2_COMPort.DropDownStyle = ComboBoxStyle.DropDownList;

                // Set Combo Box (Drop Down) for Save Data options to be Read Only
                dd_Specimen_Number.DropDownStyle = ComboBoxStyle.DropDownList;
                dd_Test_Type.DropDownStyle = ComboBoxStyle.DropDownList;
                dd_Trial_Number.DropDownStyle = ComboBoxStyle.DropDownList;

                // Set Start and Stop buttons to their initial state
                btn_Start_Collection.Enabled = false;
                btn_Stop_Collection.Enabled = false;

                // Set Combo Box (Drop Down) items for Save Data options
                // Specimen Number Items
                dd_Specimen_Number.DataSource = null;
                dd_Specimen_Number.DisplayMember = "";
                dd_Specimen_Number.ValueMember = "";
                dd_Specimen_Number.Items.Clear();

                for (int i = 1; i < 21; i++)
                {
                    dd_Specimen_Number.Items.Add(i);
                }

                // Test Type Items
                dd_Test_Type.DataSource = null;
                dd_Test_Type.DisplayMember = "";
                dd_Test_Type.ValueMember = "";
                dd_Test_Type.Items.Clear();

                dd_Test_Type.Items.Add("Torsion Test");
                dd_Test_Type.Items.Add("Flexion Test");
                dd_Test_Type.Items.Add("Area Test");

                // Trial Number Items
                dd_Trial_Number.DataSource = null;
                dd_Trial_Number.DisplayMember = "";
                dd_Trial_Number.ValueMember = "";
                dd_Trial_Number.Items.Clear();

                for (int i = 1; i < 4; i++)
                {
                    dd_Trial_Number.Items.Add(i);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReadSensor_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Free COM ports on close
                if (serialPort_Sensor1.IsOpen)
                {
                    serialPort_Sensor1.Close();
                    serialPort_Sensor1.Dispose();
                }

                if (serialPort_Sensor2.IsOpen)
                {
                    serialPort_Sensor2.Close();
                    serialPort_Sensor2.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void dd_Sensor1_COMPort_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Retrieve selected Sensor 1 COM Port
            str_Sensor1_COMPort = dd_Sensor1_COMPort.SelectedItem.ToString();

            // Check to see if both COM port Combo Box (Drop Down) are selected
            if (dd_Sensor1_COMPort.SelectedIndex > -1 && dd_Sensor2_COMPort.SelectedIndex > -1)
            {
                btn_Start_Collection.Enabled = true;
            }

            // If Sensor 2 drop down is not selected
            if (dd_Sensor2_COMPort.SelectedIndex < 0)
            {
                // Clear values in Sensor 2 COM Port drop down
                dd_Sensor2_COMPort.DataSource = null;
                dd_Sensor2_COMPort.DisplayMember = "";
                dd_Sensor2_COMPort.ValueMember = "";
                dd_Sensor2_COMPort.Items.Clear();

                // Populate Sensor 2 COM Port drop down with available serial ports
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (!(dd_Sensor1_COMPort.SelectedItem.ToString() == s)) // remove port selected for Sensor 1
                        dd_Sensor2_COMPort.Items.Add(s);
                }
            }
        }

        private void dd_Sensor2_COMPort_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Retrieve selected Sensor 2 COM Port
            str_Sensor2_COMPort = dd_Sensor2_COMPort.SelectedItem.ToString();

            // Check to see if both COM port Combo Box (Drop Down) are selected
            if (dd_Sensor1_COMPort.SelectedIndex > -1 && dd_Sensor2_COMPort.SelectedIndex > -1)
            {
                btn_Start_Collection.Enabled = true;
            }

            // If Sensor 1 drop down is not selected
            if (dd_Sensor1_COMPort.SelectedIndex < 0)
            {
                // Clear values in Sensor 1 COM Port drop down
                dd_Sensor1_COMPort.DataSource = null;
                dd_Sensor1_COMPort.DisplayMember = "";
                dd_Sensor1_COMPort.ValueMember = "";
                dd_Sensor1_COMPort.Items.Clear();

                // Populate Sensor 1 COM Port drop down with available serial ports
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (!(dd_Sensor2_COMPort.SelectedItem.ToString() == s)) // remove port selected for Sensor 1
                        dd_Sensor1_COMPort.Items.Add(s);
                }
            }
        }

        private void btn_Start_Collection_Click(object sender, EventArgs e)
        {
            if (dd_Specimen_Number.SelectedIndex < 0)
            {
                MessageBox.Show("You must choose a specimen number");
                return;
            }

            if (dd_Test_Type.SelectedIndex < 0)
            {
                MessageBox.Show("You must choose a test type");
                return;
            }

            if (dd_Trial_Number.SelectedIndex < 0)
            {
                MessageBox.Show("You must choose a trial number");
                return;
            }

            // Remove any previous data inside of DataSet pertaining to Sensor List
            if (_ds.Tables.Contains("Sensor_Data"))
            {
                _ds.Tables.Remove("Sensor_Data");
            }
            _ds.Tables.Add("Sensor_Data");

            _ds.Tables["Sensor_Data"].Columns.Add("Sensor1");
            _ds.Tables["Sensor_Data"].Columns.Add("Sensor2");

            try
            {
                // Set up serial port for Serial 1 with appropriate COM settings
                serialPort_Sensor1 = new SerialPort(str_Sensor1_COMPort, int_Sensor1_BaudRate, getParity(str_Sensor1_Parity), int_Sensor1_DataBit, getStopBits(str_Sensor1_StopBit));
                serialPort_Sensor1.Handshake = getHandShake(str_Sensor1_FlowControl);
                serialPort_Sensor1.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived_Sensor1);
                serialPort_Sensor1.ReadTimeout = int_Sensor1_ReadTimeout;
                serialPort_Sensor1.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening serial port for Sensor 1 :: " + ex.Message, "Serial Port Error");
            }

            try
            {
                // Set up serial port for Serial 2 with appropriate COM settings
                serialPort_Sensor2 = new SerialPort(str_Sensor2_COMPort, int_Sensor2_BaudRate, getParity(str_Sensor2_Parity), int_Sensor2_DataBit, getStopBits(str_Sensor2_StopBit));
                serialPort_Sensor2.Handshake = getHandShake(str_Sensor2_FlowControl);
                serialPort_Sensor2.DataReceived += new SerialDataReceivedEventHandler(serial_DataReceived_Sensor2);
                serialPort_Sensor2.ReadTimeout = int_Sensor2_ReadTimeout;
                serialPort_Sensor2.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening serial port for Sensor 2 :: " + ex.Message, "Serial Port Error");
            }

            try
            {
                if (serialPort_Sensor1.IsOpen && serialPort_Sensor2.IsOpen)
                {
                    // Disable drop down menus
                    dd_Sensor1_COMPort.Enabled = false;
                    dd_Sensor2_COMPort.Enabled = false;

                    // Disable configuration buttons
                    btn_COMPort_Update.Enabled = false;
                    btn_Start_Collection.Enabled = false;
                    btn_Stop_Collection.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Error Occured in Opening Serial Ports", "Serial Port Error");
                    if (serialPort_Sensor1.IsOpen)
                    {
                        serialPort_Sensor1.Close();
                        serialPort_Sensor1.Dispose();
                    }

                    if (serialPort_Sensor2.IsOpen)
                    {
                        serialPort_Sensor2.Close();
                        serialPort_Sensor2.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Stop_Collection_Click(object sender, EventArgs e)
        {
            btn_Start_Collection.Enabled = true;
            btn_Stop_Collection.Enabled = false;

            try
            {
                // Free COM ports on close
                if (serialPort_Sensor1.IsOpen)
                {
                    serialPort_Sensor1.Close();
                    serialPort_Sensor1.Dispose();
                }

                if (serialPort_Sensor2.IsOpen)
                {
                    serialPort_Sensor2.Close();
                    serialPort_Sensor2.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_COMPort_Update_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear values for Sensor 1 and Sensor 2 COM Port drop down menus
                dd_Sensor1_COMPort.DataSource = null;
                dd_Sensor1_COMPort.DisplayMember = "";
                dd_Sensor1_COMPort.ValueMember = "";
                dd_Sensor1_COMPort.Items.Clear();

                dd_Sensor2_COMPort.DataSource = null;
                dd_Sensor2_COMPort.DisplayMember = "";
                dd_Sensor2_COMPort.ValueMember = "";
                dd_Sensor2_COMPort.Items.Clear();

                // Populate Sensor 1 and Sensor 2 COM Port drop down with available ports
                foreach (string s in SerialPort.GetPortNames())
                {
                    dd_Sensor1_COMPort.Items.Add(s);
                    dd_Sensor2_COMPort.Items.Add(s);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Serial Data Events
        // Data received by the Sensor 1 COMPort
        void serial_DataReceived_Sensor1(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Sleep for a few milliseconds to make sure all data is received
                Thread.Sleep(int_ThreadSleep_Sensor);

                // Read data from Serial Port
                str_COMData_Sensor1 = serialPort_Sensor1.ReadExisting();

                // Trim lead line returns
                str_COMData_Sensor1 = Regex.Replace(str_COMData_Sensor1, @"[\u001D]", "");
                str_COMData_Sensor1 = Regex.Replace(str_COMData_Sensor1, @"[^\w\].\\/:-]", "");

                // Trim leading zeros and trailing decimal point
                string str_Temp_COMData_Sensor1 = str_COMData_Sensor1.TrimEnd(new Char[] { '.' });

                // Set default sensor1 data integer and string
                int int_COMData_Sensor1 = 0;
                str_Load_Sensor1 = "0";

                // Try to parse the string into an integer
                if (Int32.TryParse(str_Temp_COMData_Sensor1, out int_COMData_Sensor1))
                {
                    // Multiply by calibration factor to get units in Newtons
                    double dbl_Load_Value = dbl_Biceps_Load_Cal_Factor * (double)int_COMData_Sensor1;
                    str_Load_Sensor1 = dbl_Load_Value.ToString();
                }


                if (bool_COMData_Received)
                {
                    // If data was received, then add the COMData to the existing row and add the row to the DataSet
                    _dr["Sensor1"] = str_COMData_Sensor1;
                    _ds.Tables["Sensor_Data"].Rows.Add(_dr);

                    // Change the COMData received flag to true
                    bool_COMData_Received = false;

                    // Update GUI display of trigger count and datagridview
                    this.BeginInvoke(new deleg_DGV_Update(gui_Update_DataGridView), new object[] { str_Load_Sensor1, str_COMData_Sensor2 });
                }
                else
                {
                    // If data was not received, then create a new DataRow and add the COMData
                    _dr = _ds.Tables["Sensor_Data"].NewRow();
                    _dr["Sensor1"] = str_COMData_Sensor1;

                    // Change the COMData received flag to true
                    bool_COMData_Received = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        // Data received by the Sensor 2 COMPort
        void serial_DataReceived_Sensor2(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Sleep for a few milliseconds to make sure all data is received
                Thread.Sleep(int_ThreadSleep_Sensor);

                // Read data from Serial Port
                str_COMData_Sensor2 = serialPort_Sensor2.ReadExisting();

                // Trim lead line returns
                str_COMData_Sensor2 = Regex.Replace(str_COMData_Sensor2, @"[\u001D]", "");
                str_COMData_Sensor2 = Regex.Replace(str_COMData_Sensor2, @"[^\w\].\\/:-]", "");

                // Trim leading zeros and trailing decimal point
                string str_Temp_COMData_Sensor2 = str_COMData_Sensor2.TrimEnd(new Char[] { '.' });

                // Set default sensor2 data integer and string
                int int_COMData_Sensor2 = 0;
                str_Load_Sensor2 = "0";

                // Try to parse the string into an integer
                if (Int32.TryParse(str_Temp_COMData_Sensor2, out int_COMData_Sensor2))
                {
                    double dbl_Load_Value = 0;

                    if (str_Test_Type == "Torsion")
                    {
                        // Multiply by calibration factor to get torque in Newtons-mm
                        dbl_Load_Value = dbl_Forearm_Torque_Cal_Factor * (double)int_COMData_Sensor2;
                    }
                    else if (str_Test_Type == "Flexion")
                    {
                        // Multiply by calibration factor to get load in Newtons
                        dbl_Load_Value = dbl_Forearm_Load_Cal_Factor * (double)int_COMData_Sensor2;
                    }
                    else if (str_Test_Type == "Area")
                    {
                        // Multiply by calibration factor to get rotation in
                        dbl_Load_Value = dbl_Rotation_Cal_Factor * (double)int_COMData_Sensor2;
                    }

                    // Multiply by calibration factor to get units in Newtons
                    str_Load_Sensor2 = dbl_Load_Value.ToString();
                }


                if (bool_COMData_Received)
                {
                    // If data was received, then add the COMData to the existing row and add the row to the DataSet
                    _dr["Sensor2"] = str_COMData_Sensor2;
                    _ds.Tables["Sensor_Data"].Rows.Add(_dr);

                    // Change the COMData received flag to true
                    bool_COMData_Received = false;

                    // Update GUI display of trigger count and datagridview
                    this.BeginInvoke(new deleg_DGV_Update(gui_Update_DataGridView), new object[] { str_Load_Sensor1, str_COMData_Sensor2 });
                }
                else
                {
                    // If data was not received, then create a new DataRow and add the COMData
                    _dr = _ds.Tables["Sensor_Data"].NewRow();
                    _dr["Sensor2"] = str_COMData_Sensor2;

                    // Change the COMData received flag to true
                    bool_COMData_Received = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #region Helper Functions

        // Functions to get COM settings for sensor
        private Parity getParity(string s)
        {
            Parity p = Parity.None;

            switch (s.ToUpper())
            {
                case "EVEN":
                    p = Parity.Even;
                    break;
                case "MARK":
                    p = Parity.Mark;
                    break;
                case "ODD":
                    p = Parity.Odd;
                    break;
                case "SPACE":
                    p = Parity.Space;
                    break;
            }
            return p;
        }

        private StopBits getStopBits(string s)
        {
            StopBits sb = StopBits.One;

            switch (s.ToUpper())
            {
                case "0":
                    sb = StopBits.None;
                    break;
                case "1.5":
                    sb = StopBits.OnePointFive;
                    break;
                case "2":
                    sb = StopBits.Two;
                    break;
            }
            return sb;
        }

        private Handshake getHandShake(string s)
        {
            Handshake h = Handshake.None;

            switch (s.ToUpper())
            {
                case "HARDWARE":
                    h = Handshake.RequestToSend;
                    break;
                case "REQUESTTOSENDXONXOFF":
                    h = Handshake.RequestToSendXOnXOff;
                    break;
                case "XONXOFF":
                    h = Handshake.XOnXOff;
                    break;
            }
            return h;
        }

        private string getFileName()
        {
            return int_Specimen_Number + "__" + str_Test_Type + "__" + int_Trial_Number + "__" + DateTime.Now.ToString("yyyy-MM-dd-H-mm-ss") + ".txt";
        }

        #endregion

        private void dd_Specimen_Number_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int_Specimen_Number = dd_Specimen_Number.SelectedIndex + 1;
        }

        private void dd_Test_Type_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (dd_Test_Type.SelectedIndex)
            {
                case 0:
                    str_Test_Type = "Torsion";
                    break;
                case 1:
                    str_Test_Type = "Flexion";
                    break;
                case 2:
                    str_Test_Type = "Area";
                    break;
            }
        }

        private void dd_Trial_Number_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int_Trial_Number = dd_Trial_Number.SelectedIndex + 1;
        }

        #region File Save Events
        // Function to save data to a local file

        private void btn_Save_Data_Click(object sender, EventArgs e)
        {
            if (int_Specimen_Number < 1)
            {
                MessageBox.Show("You must choose a specimen number");
                return;
            }

            if (String.IsNullOrEmpty(str_Test_Type))
            {
                MessageBox.Show("You must choose a test type");
                return;
            }

            if (int_Trial_Number < 1)
            {
                MessageBox.Show("You must choose a trial number");
                return;
            }

            string filename = getFileName();

            try
            {
                // Confirmation message
                DialogResult dialogResult = MessageBox.Show("Would you like to save the test data to '" + filename + "' in the current directory?", "Save Dialog", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    // Create variable list based on the testing type
                    string variablelist = "";

                    if (str_Test_Type.Equals("Torsion"))
                    {
                        variablelist = "Biceps Load \t Forearm Torque";
                    }
                    else if (str_Test_Type.Equals("Flexion"))
                    {
                        variablelist = "Biceps Load \t Forearm Load";
                    }
                    else if (str_Test_Type.Equals("Area"))
                    {
                        variablelist = "Biceps Load \t Rotation";
                    }
                    else
                    {
                        MessageBox.Show("File Not Saved: Can't find correct test type");
                        return;
                    }

                    // Save data to a folder in the application startup directory
                    string filepath = "_TestData";

                    if (!Directory.Exists(Path.Combine(Application.StartupPath, filepath)))
                    {
                        Directory.CreateDirectory(Path.Combine(Application.StartupPath, filepath));
                    }

                    filename = Path.Combine(Application.StartupPath, filepath + "\\" + filename);

                    bool datasaved = write_FileData(filename, variablelist, _ds.Tables["Sensor_Data"]);

                    if (datasaved)
                    {
                        MessageBox.Show("Data written to " + filename);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void write_FileData(string _fileName, string _variableName, DataTable _dt1, DataTable _dt2)
        {
            int numRows = 0;
            try
            {
                if (_dt1.Rows.Count > _dt2.Rows.Count)
                {
                    numRows = _dt2.Rows.Count;
                }
                else
                {
                    numRows = _dt1.Rows.Count;
                }

                using (StreamWriter sw = new StreamWriter(_fileName, false))
                {
                    sw.Write(_variableName);
                    sw.WriteLine();

                    for (int rowCount = 0; rowCount < numRows; rowCount++)
                    {
                        //for (int colCount ) //

                        if (_dt1.Rows[rowCount].IsNull("Data"))
                        {
                            sw.Write("NULL");
                        }
                        else
                        {
                            sw.Write(_dt1.Rows[rowCount]["Data"].ToString());
                        }

                        // *** Write second data table to second column

                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Function to write data to a file
        private bool write_FileData(string _fileName, string _variableName, DataTable _dt)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(_fileName, false))
                {
                    sw.Write(_variableName);
                    sw.WriteLine();

                    if (_dt != null)
                    {
                        if (_dt.Rows.Count > 0)
                        {
                            foreach (DataRow _row in _dt.Rows)
                            {
                                int count = 1;

                                foreach (DataColumn _col in _dt.Columns)
                                {
                                    if (_row.IsNull(_col))
                                    {
                                        sw.Write("NULL");
                                    }
                                    else
                                    {
                                        sw.Write(_row[_col].ToString());
                                    }

                                    if (count++ != _dt.Columns.Count)
                                    {
                                        sw.Write("\t");
                                    }
                                }
                                sw.WriteLine();
                            }
                        }
                        else
                        {
                            MessageBox.Show("No data is stored in the Data Table", "Data Table Empty");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Data Table is NULL", "Data Table Null");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }

            return true;
        }

        // Function to update data grid view in GUI
        private void gui_Update_DataGridView(string str_Sensor1_Data, string str_Sensor2_Data)
        {
            try
            {
                dgv_Sensor_Data.Rows.Add(str_Sensor1_Data, str_Sensor2_Data);

                dgv_Sensor_Data.Update();

                dgv_Sensor_Data.FirstDisplayedScrollingRowIndex = dgv_Sensor_Data.RowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        private void btn_Clear_DGV_Click(object sender, EventArgs e)
        {
            dgv_Sensor_Data.Rows.Clear();
            dgv_Sensor_Data.Refresh();
        }
    }
}

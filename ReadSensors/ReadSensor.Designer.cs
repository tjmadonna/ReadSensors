namespace ReadSensors
{
    partial class ReadSensor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dd_Sensor1_COMPort = new System.Windows.Forms.ComboBox();
            this.dd_Sensor2_COMPort = new System.Windows.Forms.ComboBox();
            this.btn_Start_Collection = new System.Windows.Forms.Button();
            this.btn_Stop_Collection = new System.Windows.Forms.Button();
            this.btn_COMPort_Update = new System.Windows.Forms.Button();
            this.dd_Specimen_Number = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dd_Test_Type = new System.Windows.Forms.ComboBox();
            this.btn_Save_Data = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dd_Trial_Number = new System.Windows.Forms.ComboBox();
            this.dgv_Sensor_Data = new System.Windows.Forms.DataGridView();
            this.btn_Clear_DGV = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Sensor_Data)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sensor 1 COM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Sensor 2 COM";
            // 
            // dd_Sensor1_COMPort
            // 
            this.dd_Sensor1_COMPort.FormattingEnabled = true;
            this.dd_Sensor1_COMPort.Location = new System.Drawing.Point(93, 59);
            this.dd_Sensor1_COMPort.Name = "dd_Sensor1_COMPort";
            this.dd_Sensor1_COMPort.Size = new System.Drawing.Size(145, 21);
            this.dd_Sensor1_COMPort.TabIndex = 2;
            this.dd_Sensor1_COMPort.SelectionChangeCommitted += new System.EventHandler(this.dd_Sensor1_COMPort_SelectionChangeCommitted);
            // 
            // dd_Sensor2_COMPort
            // 
            this.dd_Sensor2_COMPort.FormattingEnabled = true;
            this.dd_Sensor2_COMPort.Location = new System.Drawing.Point(94, 86);
            this.dd_Sensor2_COMPort.Name = "dd_Sensor2_COMPort";
            this.dd_Sensor2_COMPort.Size = new System.Drawing.Size(144, 21);
            this.dd_Sensor2_COMPort.TabIndex = 3;
            this.dd_Sensor2_COMPort.SelectionChangeCommitted += new System.EventHandler(this.dd_Sensor2_COMPort_SelectionChangeCommitted);
            // 
            // btn_Start_Collection
            // 
            this.btn_Start_Collection.Location = new System.Drawing.Point(15, 148);
            this.btn_Start_Collection.Name = "btn_Start_Collection";
            this.btn_Start_Collection.Size = new System.Drawing.Size(159, 62);
            this.btn_Start_Collection.TabIndex = 4;
            this.btn_Start_Collection.Text = "Start";
            this.btn_Start_Collection.UseVisualStyleBackColor = true;
            this.btn_Start_Collection.Click += new System.EventHandler(this.btn_Start_Collection_Click);
            // 
            // btn_Stop_Collection
            // 
            this.btn_Stop_Collection.Location = new System.Drawing.Point(180, 148);
            this.btn_Stop_Collection.Name = "btn_Stop_Collection";
            this.btn_Stop_Collection.Size = new System.Drawing.Size(171, 62);
            this.btn_Stop_Collection.TabIndex = 5;
            this.btn_Stop_Collection.Text = "Stop";
            this.btn_Stop_Collection.UseVisualStyleBackColor = true;
            this.btn_Stop_Collection.Click += new System.EventHandler(this.btn_Stop_Collection_Click);
            // 
            // btn_COMPort_Update
            // 
            this.btn_COMPort_Update.Location = new System.Drawing.Point(259, 59);
            this.btn_COMPort_Update.Name = "btn_COMPort_Update";
            this.btn_COMPort_Update.Size = new System.Drawing.Size(92, 48);
            this.btn_COMPort_Update.TabIndex = 6;
            this.btn_COMPort_Update.Text = "Update COM Ports";
            this.btn_COMPort_Update.UseVisualStyleBackColor = true;
            this.btn_COMPort_Update.Click += new System.EventHandler(this.btn_COMPort_Update_Click);
            // 
            // dd_Specimen_Number
            // 
            this.dd_Specimen_Number.FormattingEnabled = true;
            this.dd_Specimen_Number.Location = new System.Drawing.Point(590, 59);
            this.dd_Specimen_Number.Name = "dd_Specimen_Number";
            this.dd_Specimen_Number.Size = new System.Drawing.Size(136, 21);
            this.dd_Specimen_Number.TabIndex = 7;
            this.dd_Specimen_Number.SelectionChangeCommitted += new System.EventHandler(this.dd_Specimen_Number_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(490, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Specimen Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(529, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Test Type";
            // 
            // dd_Test_Type
            // 
            this.dd_Test_Type.FormattingEnabled = true;
            this.dd_Test_Type.Location = new System.Drawing.Point(590, 86);
            this.dd_Test_Type.Name = "dd_Test_Type";
            this.dd_Test_Type.Size = new System.Drawing.Size(136, 21);
            this.dd_Test_Type.TabIndex = 10;
            this.dd_Test_Type.SelectionChangeCommitted += new System.EventHandler(this.dd_Test_Type_SelectionChangeCommitted);
            // 
            // btn_Save_Data
            // 
            this.btn_Save_Data.Location = new System.Drawing.Point(590, 148);
            this.btn_Save_Data.Name = "btn_Save_Data";
            this.btn_Save_Data.Size = new System.Drawing.Size(136, 62);
            this.btn_Save_Data.TabIndex = 11;
            this.btn_Save_Data.Text = "Save Data";
            this.btn_Save_Data.UseVisualStyleBackColor = true;
            this.btn_Save_Data.Click += new System.EventHandler(this.btn_Save_Data_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(517, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Trial Number";
            // 
            // dd_Trial_Number
            // 
            this.dd_Trial_Number.FormattingEnabled = true;
            this.dd_Trial_Number.Location = new System.Drawing.Point(590, 113);
            this.dd_Trial_Number.Name = "dd_Trial_Number";
            this.dd_Trial_Number.Size = new System.Drawing.Size(136, 21);
            this.dd_Trial_Number.TabIndex = 13;
            this.dd_Trial_Number.SelectionChangeCommitted += new System.EventHandler(this.dd_Trial_Number_SelectionChangeCommitted);
            // 
            // dgv_Sensor_Data
            // 
            this.dgv_Sensor_Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Sensor_Data.Location = new System.Drawing.Point(15, 250);
            this.dgv_Sensor_Data.Name = "dgv_Sensor_Data";
            this.dgv_Sensor_Data.Size = new System.Drawing.Size(711, 168);
            this.dgv_Sensor_Data.TabIndex = 14;
            // 
            // btn_Clear_DGV
            // 
            this.btn_Clear_DGV.Location = new System.Drawing.Point(15, 438);
            this.btn_Clear_DGV.Name = "btn_Clear_DGV";
            this.btn_Clear_DGV.Size = new System.Drawing.Size(75, 23);
            this.btn_Clear_DGV.TabIndex = 15;
            this.btn_Clear_DGV.Text = "Clear";
            this.btn_Clear_DGV.UseVisualStyleBackColor = true;
            this.btn_Clear_DGV.Click += new System.EventHandler(this.btn_Clear_DGV_Click);
            // 
            // ReadSensor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 488);
            this.Controls.Add(this.btn_Clear_DGV);
            this.Controls.Add(this.dgv_Sensor_Data);
            this.Controls.Add(this.dd_Trial_Number);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btn_Save_Data);
            this.Controls.Add(this.dd_Test_Type);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dd_Specimen_Number);
            this.Controls.Add(this.btn_COMPort_Update);
            this.Controls.Add(this.btn_Stop_Collection);
            this.Controls.Add(this.btn_Start_Collection);
            this.Controls.Add(this.dd_Sensor2_COMPort);
            this.Controls.Add(this.dd_Sensor1_COMPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReadSensor";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ReadSensor_FormClosed);
            this.Load += new System.EventHandler(this.ReadSensor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Sensor_Data)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox dd_Sensor1_COMPort;
        private System.Windows.Forms.ComboBox dd_Sensor2_COMPort;
        private System.Windows.Forms.Button btn_Start_Collection;
        private System.Windows.Forms.Button btn_Stop_Collection;
        private System.Windows.Forms.Button btn_COMPort_Update;
        private System.Windows.Forms.ComboBox dd_Specimen_Number;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox dd_Test_Type;
        private System.Windows.Forms.Button btn_Save_Data;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dd_Trial_Number;
        private System.Windows.Forms.DataGridView dgv_Sensor_Data;
        private System.Windows.Forms.Button btn_Clear_DGV;
    }
}


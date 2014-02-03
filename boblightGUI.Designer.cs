namespace boblight_net
{
    partial class boblightGUI
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
            this.panelNetwork = new System.Windows.Forms.Panel();
            this.textPriority = new System.Windows.Forms.TextBox();
            this.lblPriority = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.textPort = new System.Windows.Forms.TextBox();
            this.textIP = new System.Windows.Forms.TextBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblIP = new System.Windows.Forms.Label();
            this.listLights = new System.Windows.Forms.ListBox();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.buttonInterpolate = new System.Windows.Forms.Button();
            this.checkInterpolate = new System.Windows.Forms.CheckBox();
            this.buttonUse = new System.Windows.Forms.Button();
            this.checkUse = new System.Windows.Forms.CheckBox();
            this.buttonSpeed = new System.Windows.Forms.Button();
            this.textSpeed = new System.Windows.Forms.TextBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.buttonColor = new System.Windows.Forms.Button();
            this.textHex = new System.Windows.Forms.TextBox();
            this.lblHex = new System.Windows.Forms.Label();
            this.lblAbout = new System.Windows.Forms.Label();
            this.tabsControl = new System.Windows.Forms.TabControl();
            this.tabLights = new System.Windows.Forms.TabPage();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.listGroups = new System.Windows.Forms.ListBox();
            this.buttonMisc = new System.Windows.Forms.Button();
            this.btnIterate = new System.Windows.Forms.Button();
            this.panelIterate = new System.Windows.Forms.Panel();
            this.btnRemoveItColor = new System.Windows.Forms.Button();
            this.btnAddItColor = new System.Windows.Forms.Button();
            this.listIterate = new System.Windows.Forms.ListBox();
            this.txtIterateFrom = new System.Windows.Forms.TextBox();
            this.btnIterateReverse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelNetwork.SuspendLayout();
            this.panelProperties.SuspendLayout();
            this.tabsControl.SuspendLayout();
            this.tabLights.SuspendLayout();
            this.tabGroups.SuspendLayout();
            this.panelIterate.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelNetwork
            // 
            this.panelNetwork.Controls.Add(this.label3);
            this.panelNetwork.Controls.Add(this.textPriority);
            this.panelNetwork.Controls.Add(this.lblPriority);
            this.panelNetwork.Controls.Add(this.buttonConnect);
            this.panelNetwork.Controls.Add(this.lblStatus);
            this.panelNetwork.Controls.Add(this.textPort);
            this.panelNetwork.Controls.Add(this.textIP);
            this.panelNetwork.Controls.Add(this.lblPort);
            this.panelNetwork.Controls.Add(this.lblIP);
            this.panelNetwork.Location = new System.Drawing.Point(173, 13);
            this.panelNetwork.Name = "panelNetwork";
            this.panelNetwork.Size = new System.Drawing.Size(178, 120);
            this.panelNetwork.TabIndex = 0;
            // 
            // textPriority
            // 
            this.textPriority.Location = new System.Drawing.Point(50, 73);
            this.textPriority.Name = "textPriority";
            this.textPriority.Size = new System.Drawing.Size(125, 20);
            this.textPriority.TabIndex = 3;
            this.textPriority.Text = "128";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.Location = new System.Drawing.Point(3, 76);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(41, 13);
            this.lblPriority.TabIndex = 6;
            this.lblPriority.Text = "Priority:";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(82, 95);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(93, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 100);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(73, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Disconnected";
            // 
            // textPort
            // 
            this.textPort.Location = new System.Drawing.Point(38, 50);
            this.textPort.Name = "textPort";
            this.textPort.Size = new System.Drawing.Size(137, 20);
            this.textPort.TabIndex = 2;
            this.textPort.Text = "19333";
            // 
            // textIP
            // 
            this.textIP.Location = new System.Drawing.Point(29, 27);
            this.textIP.Name = "textIP";
            this.textIP.Size = new System.Drawing.Size(146, 20);
            this.textIP.TabIndex = 1;
            this.textIP.Text = "127.0.0.1";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(3, 53);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // lblIP
            // 
            this.lblIP.AutoSize = true;
            this.lblIP.Location = new System.Drawing.Point(3, 30);
            this.lblIP.Name = "lblIP";
            this.lblIP.Size = new System.Drawing.Size(20, 13);
            this.lblIP.TabIndex = 0;
            this.lblIP.Text = "IP:";
            // 
            // listLights
            // 
            this.listLights.FormattingEnabled = true;
            this.listLights.Location = new System.Drawing.Point(3, 6);
            this.listLights.Name = "listLights";
            this.listLights.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listLights.Size = new System.Drawing.Size(141, 251);
            this.listLights.TabIndex = 5;
            // 
            // panelProperties
            // 
            this.panelProperties.Controls.Add(this.label2);
            this.panelProperties.Controls.Add(this.buttonInterpolate);
            this.panelProperties.Controls.Add(this.checkInterpolate);
            this.panelProperties.Controls.Add(this.buttonUse);
            this.panelProperties.Controls.Add(this.checkUse);
            this.panelProperties.Controls.Add(this.buttonSpeed);
            this.panelProperties.Controls.Add(this.textSpeed);
            this.panelProperties.Controls.Add(this.lblSpeed);
            this.panelProperties.Controls.Add(this.buttonColor);
            this.panelProperties.Controls.Add(this.textHex);
            this.panelProperties.Controls.Add(this.lblHex);
            this.panelProperties.Enabled = false;
            this.panelProperties.Location = new System.Drawing.Point(173, 138);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(178, 193);
            this.panelProperties.TabIndex = 2;
            // 
            // buttonInterpolate
            // 
            this.buttonInterpolate.Location = new System.Drawing.Point(82, 135);
            this.buttonInterpolate.Name = "buttonInterpolate";
            this.buttonInterpolate.Size = new System.Drawing.Size(93, 25);
            this.buttonInterpolate.TabIndex = 11;
            this.buttonInterpolate.Text = "Set Interpolation";
            this.buttonInterpolate.UseVisualStyleBackColor = true;
            // 
            // checkInterpolate
            // 
            this.checkInterpolate.AutoSize = true;
            this.checkInterpolate.Location = new System.Drawing.Point(3, 139);
            this.checkInterpolate.Name = "checkInterpolate";
            this.checkInterpolate.Size = new System.Drawing.Size(76, 17);
            this.checkInterpolate.TabIndex = 10;
            this.checkInterpolate.Text = "Interpolate";
            this.checkInterpolate.UseVisualStyleBackColor = true;
            // 
            // buttonUse
            // 
            this.buttonUse.Location = new System.Drawing.Point(91, 166);
            this.buttonUse.Name = "buttonUse";
            this.buttonUse.Size = new System.Drawing.Size(84, 25);
            this.buttonUse.TabIndex = 13;
            this.buttonUse.Text = "Set Use";
            this.buttonUse.UseVisualStyleBackColor = true;
            this.buttonUse.Click += new System.EventHandler(this.buttonUse_Click);
            // 
            // checkUse
            // 
            this.checkUse.AutoSize = true;
            this.checkUse.Checked = true;
            this.checkUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkUse.Location = new System.Drawing.Point(3, 170);
            this.checkUse.Name = "checkUse";
            this.checkUse.Size = new System.Drawing.Size(82, 17);
            this.checkUse.TabIndex = 12;
            this.checkUse.Text = "Use Light(s)";
            this.checkUse.UseVisualStyleBackColor = true;
            // 
            // buttonSpeed
            // 
            this.buttonSpeed.Location = new System.Drawing.Point(3, 104);
            this.buttonSpeed.Name = "buttonSpeed";
            this.buttonSpeed.Size = new System.Drawing.Size(172, 25);
            this.buttonSpeed.TabIndex = 9;
            this.buttonSpeed.Text = "Set Speed";
            this.buttonSpeed.UseVisualStyleBackColor = true;
            this.buttonSpeed.Click += new System.EventHandler(this.buttonSpeed_Click);
            // 
            // textSpeed
            // 
            this.textSpeed.Location = new System.Drawing.Point(50, 78);
            this.textSpeed.Name = "textSpeed";
            this.textSpeed.Size = new System.Drawing.Size(125, 20);
            this.textSpeed.TabIndex = 8;
            this.textSpeed.Text = "100.0";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(3, 81);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 9;
            this.lblSpeed.Text = "Speed:";
            // 
            // buttonColor
            // 
            this.buttonColor.Location = new System.Drawing.Point(3, 47);
            this.buttonColor.Name = "buttonColor";
            this.buttonColor.Size = new System.Drawing.Size(172, 25);
            this.buttonColor.TabIndex = 7;
            this.buttonColor.Text = "Set Color";
            this.buttonColor.UseVisualStyleBackColor = true;
            this.buttonColor.Click += new System.EventHandler(this.buttonColor_Click);
            // 
            // textHex
            // 
            this.textHex.Location = new System.Drawing.Point(38, 21);
            this.textHex.Name = "textHex";
            this.textHex.Size = new System.Drawing.Size(137, 20);
            this.textHex.TabIndex = 6;
            this.textHex.Text = "FFFFFF";
            // 
            // lblHex
            // 
            this.lblHex.AutoSize = true;
            this.lblHex.Location = new System.Drawing.Point(3, 24);
            this.lblHex.Name = "lblHex";
            this.lblHex.Size = new System.Drawing.Size(29, 13);
            this.lblHex.TabIndex = 6;
            this.lblHex.Text = "Hex:";
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Location = new System.Drawing.Point(393, 148);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(176, 13);
            this.lblAbout.TabIndex = 3;
            this.lblAbout.Text = "boblight-NET for boblightd version 5";
            // 
            // tabsControl
            // 
            this.tabsControl.Controls.Add(this.tabLights);
            this.tabsControl.Controls.Add(this.tabGroups);
            this.tabsControl.Enabled = false;
            this.tabsControl.Location = new System.Drawing.Point(12, 12);
            this.tabsControl.Name = "tabsControl";
            this.tabsControl.SelectedIndex = 0;
            this.tabsControl.Size = new System.Drawing.Size(155, 288);
            this.tabsControl.TabIndex = 7;
            this.tabsControl.SelectedIndexChanged += new System.EventHandler(this.tabsControl_SelectedIndexChanged);
            // 
            // tabLights
            // 
            this.tabLights.Controls.Add(this.listLights);
            this.tabLights.Location = new System.Drawing.Point(4, 22);
            this.tabLights.Name = "tabLights";
            this.tabLights.Padding = new System.Windows.Forms.Padding(3);
            this.tabLights.Size = new System.Drawing.Size(147, 262);
            this.tabLights.TabIndex = 0;
            this.tabLights.Text = "Lights";
            this.tabLights.UseVisualStyleBackColor = true;
            // 
            // tabGroups
            // 
            this.tabGroups.Controls.Add(this.listGroups);
            this.tabGroups.Location = new System.Drawing.Point(4, 22);
            this.tabGroups.Name = "tabGroups";
            this.tabGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroups.Size = new System.Drawing.Size(147, 262);
            this.tabGroups.TabIndex = 1;
            this.tabGroups.Text = "Light Groups";
            this.tabGroups.UseVisualStyleBackColor = true;
            // 
            // listGroups
            // 
            this.listGroups.DisplayMember = "name";
            this.listGroups.FormattingEnabled = true;
            this.listGroups.Location = new System.Drawing.Point(3, 6);
            this.listGroups.Name = "listGroups";
            this.listGroups.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listGroups.Size = new System.Drawing.Size(141, 251);
            this.listGroups.TabIndex = 8;
            this.listGroups.ValueMember = "lights";
            // 
            // buttonMisc
            // 
            this.buttonMisc.Enabled = false;
            this.buttonMisc.Location = new System.Drawing.Point(12, 306);
            this.buttonMisc.Name = "buttonMisc";
            this.buttonMisc.Size = new System.Drawing.Size(155, 25);
            this.buttonMisc.TabIndex = 14;
            this.buttonMisc.Text = "Group Lights";
            this.buttonMisc.UseVisualStyleBackColor = true;
            this.buttonMisc.Click += new System.EventHandler(this.buttonMisc_Click);
            // 
            // btnIterate
            // 
            this.btnIterate.Location = new System.Drawing.Point(128, 48);
            this.btnIterate.Name = "btnIterate";
            this.btnIterate.Size = new System.Drawing.Size(115, 25);
            this.btnIterate.TabIndex = 15;
            this.btnIterate.Text = "Iterate Colors";
            this.btnIterate.UseVisualStyleBackColor = true;
            this.btnIterate.Click += new System.EventHandler(this.btnIterate_Click);
            // 
            // panelIterate
            // 
            this.panelIterate.Controls.Add(this.label1);
            this.panelIterate.Controls.Add(this.btnIterateReverse);
            this.panelIterate.Controls.Add(this.btnRemoveItColor);
            this.panelIterate.Controls.Add(this.btnAddItColor);
            this.panelIterate.Controls.Add(this.listIterate);
            this.panelIterate.Controls.Add(this.btnIterate);
            this.panelIterate.Controls.Add(this.txtIterateFrom);
            this.panelIterate.Enabled = false;
            this.panelIterate.Location = new System.Drawing.Point(357, 12);
            this.panelIterate.Name = "panelIterate";
            this.panelIterate.Size = new System.Drawing.Size(246, 130);
            this.panelIterate.TabIndex = 16;
            // 
            // btnRemoveItColor
            // 
            this.btnRemoveItColor.Location = new System.Drawing.Point(4, 101);
            this.btnRemoveItColor.Name = "btnRemoveItColor";
            this.btnRemoveItColor.Size = new System.Drawing.Size(119, 25);
            this.btnRemoveItColor.TabIndex = 18;
            this.btnRemoveItColor.Text = "Remove Color(s)";
            this.btnRemoveItColor.UseVisualStyleBackColor = true;
            this.btnRemoveItColor.Click += new System.EventHandler(this.btnRemoveItColor_Click);
            // 
            // btnAddItColor
            // 
            this.btnAddItColor.Location = new System.Drawing.Point(177, 27);
            this.btnAddItColor.Name = "btnAddItColor";
            this.btnAddItColor.Size = new System.Drawing.Size(63, 20);
            this.btnAddItColor.TabIndex = 17;
            this.btnAddItColor.Text = "Add Color";
            this.btnAddItColor.UseVisualStyleBackColor = true;
            this.btnAddItColor.Click += new System.EventHandler(this.btnAddItColor_Click);
            // 
            // listIterate
            // 
            this.listIterate.FormattingEnabled = true;
            this.listIterate.Location = new System.Drawing.Point(5, 27);
            this.listIterate.Name = "listIterate";
            this.listIterate.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listIterate.Size = new System.Drawing.Size(117, 69);
            this.listIterate.TabIndex = 16;
            // 
            // txtIterateFrom
            // 
            this.txtIterateFrom.Location = new System.Drawing.Point(128, 27);
            this.txtIterateFrom.Name = "txtIterateFrom";
            this.txtIterateFrom.Size = new System.Drawing.Size(46, 20);
            this.txtIterateFrom.TabIndex = 14;
            this.txtIterateFrom.Text = "FFFFFF";
            this.txtIterateFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIterateFrom_KeyPress);
            // 
            // btnIterateReverse
            // 
            this.btnIterateReverse.Location = new System.Drawing.Point(128, 73);
            this.btnIterateReverse.Name = "btnIterateReverse";
            this.btnIterateReverse.Size = new System.Drawing.Size(115, 25);
            this.btnIterateReverse.TabIndex = 19;
            this.btnIterateReverse.Text = "Reverse Iterate";
            this.btnIterateReverse.UseVisualStyleBackColor = true;
            this.btnIterateReverse.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Color Smoothing/Iteration Tool";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Light Properties";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Connection Properties";
            // 
            // boblightGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 342);
            this.Controls.Add(this.panelIterate);
            this.Controls.Add(this.buttonMisc);
            this.Controls.Add(this.tabsControl);
            this.Controls.Add(this.lblAbout);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelNetwork);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "boblightGUI";
            this.Text = "boblight-NET";
            this.panelNetwork.ResumeLayout(false);
            this.panelNetwork.PerformLayout();
            this.panelProperties.ResumeLayout(false);
            this.panelProperties.PerformLayout();
            this.tabsControl.ResumeLayout(false);
            this.tabLights.ResumeLayout(false);
            this.tabGroups.ResumeLayout(false);
            this.panelIterate.ResumeLayout(false);
            this.panelIterate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelNetwork;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TextBox textPort;
        private System.Windows.Forms.TextBox textIP;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.ListBox listLights;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.TextBox textPriority;
        private System.Windows.Forms.Label lblPriority;
        private System.Windows.Forms.Button buttonUse;
        private System.Windows.Forms.CheckBox checkUse;
        private System.Windows.Forms.Button buttonSpeed;
        private System.Windows.Forms.TextBox textSpeed;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Button buttonColor;
        private System.Windows.Forms.TextBox textHex;
        private System.Windows.Forms.Label lblHex;
        private System.Windows.Forms.Label lblAbout;
        private System.Windows.Forms.Button buttonInterpolate;
        private System.Windows.Forms.CheckBox checkInterpolate;
        private System.Windows.Forms.TabControl tabsControl;
        private System.Windows.Forms.TabPage tabLights;
        private System.Windows.Forms.TabPage tabGroups;
        private System.Windows.Forms.ListBox listGroups;
        private System.Windows.Forms.Button buttonMisc;
        private System.Windows.Forms.Button btnIterate;
        private System.Windows.Forms.Panel panelIterate;
        private System.Windows.Forms.TextBox txtIterateFrom;
        private System.Windows.Forms.ListBox listIterate;
        private System.Windows.Forms.Button btnRemoveItColor;
        private System.Windows.Forms.Button btnAddItColor;
        private System.Windows.Forms.Button btnIterateReverse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}


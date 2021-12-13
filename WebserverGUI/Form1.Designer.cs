
namespace WebserverGUI
{
     partial class Form1
     {
          /// <summary>
          ///  Required designer variable.
          /// </summary>
          private System.ComponentModel.IContainer components = null;

          /// <summary>
          ///  Clean up any resources being used.
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
          ///  Required method for Designer support - do not modify
          ///  the contents of this method with the code editor.
          /// </summary>
          private void InitializeComponent()
          {
               this.startServerButton = new System.Windows.Forms.Button();
               this.stopServerButton = new System.Windows.Forms.Button();
               this.label1 = new System.Windows.Forms.Label();
               this.serverStateLabel = new System.Windows.Forms.Label();
               this.label2 = new System.Windows.Forms.Label();
               this.serverPortLabel = new System.Windows.Forms.Label();
               this.label3 = new System.Windows.Forms.Label();
               this.groupBox1 = new System.Windows.Forms.GroupBox();
               this.applyChangesButton = new System.Windows.Forms.Button();
               this.openMaintenanceFilePathButton = new System.Windows.Forms.Button();
               this.openRootPathButton = new System.Windows.Forms.Button();
               this.portNumericUpDown = new System.Windows.Forms.NumericUpDown();
               this.maintenanceFilePathTextBox = new System.Windows.Forms.TextBox();
               this.rootPathTextBox = new System.Windows.Forms.TextBox();
               this.label5 = new System.Windows.Forms.Label();
               this.label4 = new System.Windows.Forms.Label();
               this.groupBox2 = new System.Windows.Forms.GroupBox();
               this.switchToMaintenanceCheckBox = new System.Windows.Forms.CheckBox();
               this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
               this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
               this.groupBox1.SuspendLayout();
               ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).BeginInit();
               this.groupBox2.SuspendLayout();
               this.SuspendLayout();
               // 
               // startServerButton
               // 
               this.startServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
               this.startServerButton.Location = new System.Drawing.Point(622, 12);
               this.startServerButton.Name = "startServerButton";
               this.startServerButton.Size = new System.Drawing.Size(116, 32);
               this.startServerButton.TabIndex = 0;
               this.startServerButton.Text = "Start Server";
               this.startServerButton.UseVisualStyleBackColor = true;
               this.startServerButton.Click += new System.EventHandler(this.startServerButton_Click);
               // 
               // stopServerButton
               // 
               this.stopServerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
               this.stopServerButton.Enabled = false;
               this.stopServerButton.Location = new System.Drawing.Point(622, 50);
               this.stopServerButton.Name = "stopServerButton";
               this.stopServerButton.Size = new System.Drawing.Size(116, 32);
               this.stopServerButton.TabIndex = 0;
               this.stopServerButton.Text = "Stop Server";
               this.stopServerButton.UseVisualStyleBackColor = true;
               this.stopServerButton.Click += new System.EventHandler(this.stopServerButton_Click);
               // 
               // label1
               // 
               this.label1.AutoSize = true;
               this.label1.Location = new System.Drawing.Point(6, 19);
               this.label1.Name = "label1";
               this.label1.Size = new System.Drawing.Size(68, 15);
               this.label1.TabIndex = 1;
               this.label1.Text = "Server State";
               // 
               // serverStateLabel
               // 
               this.serverStateLabel.AutoSize = true;
               this.serverStateLabel.Location = new System.Drawing.Point(80, 19);
               this.serverStateLabel.Name = "serverStateLabel";
               this.serverStateLabel.Size = new System.Drawing.Size(51, 15);
               this.serverStateLabel.TabIndex = 2;
               this.serverStateLabel.Text = "Stopped";
               // 
               // label2
               // 
               this.label2.AutoSize = true;
               this.label2.Location = new System.Drawing.Point(5, 44);
               this.label2.Name = "label2";
               this.label2.Size = new System.Drawing.Size(64, 15);
               this.label2.TabIndex = 1;
               this.label2.Text = "Server Port";
               // 
               // serverPortLabel
               // 
               this.serverPortLabel.AutoSize = true;
               this.serverPortLabel.Location = new System.Drawing.Point(79, 44);
               this.serverPortLabel.Name = "serverPortLabel";
               this.serverPortLabel.Size = new System.Drawing.Size(46, 15);
               this.serverPortLabel.TabIndex = 2;
               this.serverPortLabel.Text = "Not Set";
               // 
               // label3
               // 
               this.label3.AutoSize = true;
               this.label3.Location = new System.Drawing.Point(6, 21);
               this.label3.Name = "label3";
               this.label3.Size = new System.Drawing.Size(29, 15);
               this.label3.TabIndex = 3;
               this.label3.Text = "Port";
               // 
               // groupBox1
               // 
               this.groupBox1.Controls.Add(this.applyChangesButton);
               this.groupBox1.Controls.Add(this.openMaintenanceFilePathButton);
               this.groupBox1.Controls.Add(this.openRootPathButton);
               this.groupBox1.Controls.Add(this.portNumericUpDown);
               this.groupBox1.Controls.Add(this.maintenanceFilePathTextBox);
               this.groupBox1.Controls.Add(this.rootPathTextBox);
               this.groupBox1.Controls.Add(this.label5);
               this.groupBox1.Controls.Add(this.label4);
               this.groupBox1.Controls.Add(this.label3);
               this.groupBox1.Location = new System.Drawing.Point(12, 118);
               this.groupBox1.Name = "groupBox1";
               this.groupBox1.Size = new System.Drawing.Size(547, 155);
               this.groupBox1.TabIndex = 4;
               this.groupBox1.TabStop = false;
               this.groupBox1.Text = "Configuration";
               // 
               // applyChangesButton
               // 
               this.applyChangesButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
               this.applyChangesButton.Location = new System.Drawing.Point(6, 122);
               this.applyChangesButton.Name = "applyChangesButton";
               this.applyChangesButton.Size = new System.Drawing.Size(104, 27);
               this.applyChangesButton.TabIndex = 8;
               this.applyChangesButton.Text = "Apply Changes";
               this.applyChangesButton.UseVisualStyleBackColor = true;
               this.applyChangesButton.Click += new System.EventHandler(this.applyChangesButton_Click);
               // 
               // openMaintenanceFilePathButton
               // 
               this.openMaintenanceFilePathButton.Location = new System.Drawing.Point(396, 77);
               this.openMaintenanceFilePathButton.Name = "openMaintenanceFilePathButton";
               this.openMaintenanceFilePathButton.Size = new System.Drawing.Size(75, 23);
               this.openMaintenanceFilePathButton.TabIndex = 7;
               this.openMaintenanceFilePathButton.Text = "...";
               this.openMaintenanceFilePathButton.UseVisualStyleBackColor = true;
               this.openMaintenanceFilePathButton.Click += new System.EventHandler(this.openMaintenanceFilePathButton_Click);
               // 
               // openRootPathButton
               // 
               this.openRootPathButton.Location = new System.Drawing.Point(397, 47);
               this.openRootPathButton.Name = "openRootPathButton";
               this.openRootPathButton.Size = new System.Drawing.Size(75, 23);
               this.openRootPathButton.TabIndex = 7;
               this.openRootPathButton.Text = "...";
               this.openRootPathButton.UseVisualStyleBackColor = true;
               this.openRootPathButton.Click += new System.EventHandler(this.openRootPathButton_Click);
               // 
               // portNumericUpDown
               // 
               this.portNumericUpDown.Location = new System.Drawing.Point(138, 19);
               this.portNumericUpDown.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
               this.portNumericUpDown.Name = "portNumericUpDown";
               this.portNumericUpDown.Size = new System.Drawing.Size(120, 23);
               this.portNumericUpDown.TabIndex = 6;
               // 
               // maintenanceFilePathTextBox
               // 
               this.maintenanceFilePathTextBox.Enabled = true;
               this.maintenanceFilePathTextBox.Location = new System.Drawing.Point(138, 77);
               this.maintenanceFilePathTextBox.Name = "maintenanceFilePathTextBox";
               this.maintenanceFilePathTextBox.Size = new System.Drawing.Size(252, 23);
               this.maintenanceFilePathTextBox.TabIndex = 5;
               // 
               // rootPathTextBox
               // 
               this.rootPathTextBox.Enabled = true;
               this.rootPathTextBox.Location = new System.Drawing.Point(138, 48);
               this.rootPathTextBox.Name = "rootPathTextBox";
               this.rootPathTextBox.Size = new System.Drawing.Size(252, 23);
               this.rootPathTextBox.TabIndex = 5;
               // 
               // label5
               // 
               this.label5.AutoSize = true;
               this.label5.Location = new System.Drawing.Point(6, 80);
               this.label5.Name = "label5";
               this.label5.Size = new System.Drawing.Size(124, 15);
               this.label5.TabIndex = 4;
               this.label5.Text = "Maintenance File Path";
               // 
               // label4
               // 
               this.label4.AutoSize = true;
               this.label4.Location = new System.Drawing.Point(6, 51);
               this.label4.Name = "label4";
               this.label4.Size = new System.Drawing.Size(59, 15);
               this.label4.TabIndex = 4;
               this.label4.Text = "Root Path";
               // 
               // groupBox2
               // 
               this.groupBox2.Controls.Add(this.switchToMaintenanceCheckBox);
               this.groupBox2.Controls.Add(this.label1);
               this.groupBox2.Controls.Add(this.label2);
               this.groupBox2.Controls.Add(this.serverPortLabel);
               this.groupBox2.Controls.Add(this.serverStateLabel);
               this.groupBox2.Location = new System.Drawing.Point(12, 12);
               this.groupBox2.Name = "groupBox2";
               this.groupBox2.Size = new System.Drawing.Size(200, 100);
               this.groupBox2.TabIndex = 5;
               this.groupBox2.TabStop = false;
               this.groupBox2.Text = "Info";
               // 
               // switchToMaintenanceCheckBox
               // 
               this.switchToMaintenanceCheckBox.AutoSize = true;
               this.switchToMaintenanceCheckBox.Enabled = false;
               this.switchToMaintenanceCheckBox.Location = new System.Drawing.Point(6, 75);
               this.switchToMaintenanceCheckBox.Name = "switchToMaintenanceCheckBox";
               this.switchToMaintenanceCheckBox.Size = new System.Drawing.Size(147, 19);
               this.switchToMaintenanceCheckBox.TabIndex = 3;
               this.switchToMaintenanceCheckBox.Text = "Switch to Maintenance";
               this.switchToMaintenanceCheckBox.UseVisualStyleBackColor = true;
               this.switchToMaintenanceCheckBox.CheckedChanged += new System.EventHandler(this.switchToMaintenanceCheckBox_CheckedChanged);
               // 
               // openFileDialog1
               // 
               this.openFileDialog1.FileName = "openFileDialog1";
               // 
               // Form1
               // 
               this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
               this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
               this.ClientSize = new System.Drawing.Size(750, 488);
               this.Controls.Add(this.groupBox2);
               this.Controls.Add(this.groupBox1);
               this.Controls.Add(this.stopServerButton);
               this.Controls.Add(this.startServerButton);
               this.Name = "Form1";
               this.Text = "WebServerGUI";
               this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
               this.Load += new System.EventHandler(this.Form1_Load);
               this.groupBox1.ResumeLayout(false);
               this.groupBox1.PerformLayout();
               ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).EndInit();
               this.groupBox2.ResumeLayout(false);
               this.groupBox2.PerformLayout();
               this.ResumeLayout(false);

          }

          #endregion

          private System.Windows.Forms.Button startServerButton;
          private System.Windows.Forms.Button stopServerButton;
          private System.Windows.Forms.Label label1;
          private System.Windows.Forms.Label serverStateLabel;
          private System.Windows.Forms.Label label2;
          private System.Windows.Forms.Label serverPortLabel;
          private System.Windows.Forms.Label label3;
          private System.Windows.Forms.GroupBox groupBox1;
          private System.Windows.Forms.Label label4;
          private System.Windows.Forms.GroupBox groupBox2;
          private System.Windows.Forms.Button openMaintenanceFilePathButton;
          private System.Windows.Forms.Button openRootPathButton;
          private System.Windows.Forms.NumericUpDown portNumericUpDown;
          private System.Windows.Forms.TextBox maintenanceFilePathTextBox;
          private System.Windows.Forms.TextBox rootPathTextBox;
          private System.Windows.Forms.Label label5;
          private System.Windows.Forms.OpenFileDialog openFileDialog1;
          private System.Windows.Forms.Button applyChangesButton;
          private System.Windows.Forms.CheckBox switchToMaintenanceCheckBox;
          private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
     }
}

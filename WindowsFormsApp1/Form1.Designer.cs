
using System;

namespace EpicTMR
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnStartTimers = new System.Windows.Forms.Button();
            this.cooldownTextBox = new System.Windows.Forms.TextBox();
            this.lblCdr = new System.Windows.Forms.Label();
            this.lblExtraSeconds = new System.Windows.Forms.Label();
            this.IntervalTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnStartTimers
            // 
            this.btnStartTimers.Location = new System.Drawing.Point(12, 95);
            this.btnStartTimers.Name = "btnStartTimers";
            this.btnStartTimers.Size = new System.Drawing.Size(101, 23);
            this.btnStartTimers.TabIndex = 0;
            this.btnStartTimers.Text = "Start/Stop all";
            this.btnStartTimers.UseVisualStyleBackColor = true;
            this.btnStartTimers.Click += new System.EventHandler(this.StartStopButtonClick);
            // 
            // cooldownTextBox
            // 
            this.cooldownTextBox.Location = new System.Drawing.Point(15, 25);
            this.cooldownTextBox.Name = "cooldownTextBox";
            this.cooldownTextBox.Size = new System.Drawing.Size(100, 20);
            this.cooldownTextBox.TabIndex = 1;
            this.cooldownTextBox.Text = "0%";
            this.cooldownTextBox.TextChanged += new System.EventHandler(this.CooldownTextBox_TextChanged);
            // 
            // lblCdr
            // 
            this.lblCdr.AutoSize = true;
            this.lblCdr.Location = new System.Drawing.Point(12, 9);
            this.lblCdr.Name = "lblCdr";
            this.lblCdr.Size = new System.Drawing.Size(133, 13);
            this.lblCdr.TabIndex = 2;
            this.lblCdr.Text = "Set % of cdr (from Patreon)";
            // 
            // lblExtraSeconds
            // 
            this.lblExtraSeconds.AutoSize = true;
            this.lblExtraSeconds.Location = new System.Drawing.Point(12, 48);
            this.lblExtraSeconds.Name = "lblExtraSeconds";
            this.lblExtraSeconds.Size = new System.Drawing.Size(129, 13);
            this.lblExtraSeconds.TabIndex = 7;
            this.lblExtraSeconds.Text = "Set interval extra seconds";
            // 
            // IntervalTextBox
            // 
            this.IntervalTextBox.Location = new System.Drawing.Point(13, 65);
            this.IntervalTextBox.Name = "IntervalTextBox";
            this.IntervalTextBox.Size = new System.Drawing.Size(100, 20);
            this.IntervalTextBox.TabIndex = 8;
            this.IntervalTextBox.Text = "5s";
            this.IntervalTextBox.TextChanged += new System.EventHandler(this.IntervalTextBox_TextChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 159);
            this.Controls.Add(this.IntervalTextBox);
            this.Controls.Add(this.lblExtraSeconds);
            this.Controls.Add(this.lblCdr);
            this.Controls.Add(this.cooldownTextBox);
            this.Controls.Add(this.btnStartTimers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "EpicTMR";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button btnStartTimers;
        private System.Windows.Forms.TextBox cooldownTextBox;
        private System.Windows.Forms.Label lblCdr;
        private System.Windows.Forms.Label lblExtraSeconds;
        private System.Windows.Forms.TextBox IntervalTextBox;
    }
}



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
            this.startStopBtn = new System.Windows.Forms.Button();
            this.cooldownTextBox = new System.Windows.Forms.TextBox();
            this.cooldownTitleLbl = new System.Windows.Forms.Label();
            this.intervalTitleLbl = new System.Windows.Forms.Label();
            this.intervalTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // startStopBtn
            // 
            this.startStopBtn.Location = new System.Drawing.Point(12, 95);
            this.startStopBtn.Name = "startStopBtn";
            this.startStopBtn.Size = new System.Drawing.Size(101, 23);
            this.startStopBtn.TabIndex = 0;
            this.startStopBtn.Text = "Start all timers";
            this.startStopBtn.UseVisualStyleBackColor = true;
            this.startStopBtn.Click += new System.EventHandler(this.StartStopButtonClick);
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
            // cooldownTitleLbl
            // 
            this.cooldownTitleLbl.AutoSize = true;
            this.cooldownTitleLbl.Location = new System.Drawing.Point(12, 9);
            this.cooldownTitleLbl.Name = "cooldownTitleLbl";
            this.cooldownTitleLbl.Size = new System.Drawing.Size(133, 13);
            this.cooldownTitleLbl.TabIndex = 2;
            this.cooldownTitleLbl.Text = "Set % of cdr (from Patreon)";
            // 
            // intervalTitleLbl
            // 
            this.intervalTitleLbl.AutoSize = true;
            this.intervalTitleLbl.Location = new System.Drawing.Point(12, 48);
            this.intervalTitleLbl.Name = "intervalTitleLbl";
            this.intervalTitleLbl.Size = new System.Drawing.Size(129, 13);
            this.intervalTitleLbl.TabIndex = 7;
            this.intervalTitleLbl.Text = "Set interval extra seconds";
            // 
            // intervalTextBox
            // 
            this.intervalTextBox.Location = new System.Drawing.Point(13, 65);
            this.intervalTextBox.Name = "intervalTextBox";
            this.intervalTextBox.Size = new System.Drawing.Size(100, 20);
            this.intervalTextBox.TabIndex = 8;
            this.intervalTextBox.Text = "5s";
            this.intervalTextBox.TextChanged += new System.EventHandler(this.IntervalTextBox_TextChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 159);
            this.Controls.Add(this.intervalTextBox);
            this.Controls.Add(this.intervalTitleLbl);
            this.Controls.Add(this.cooldownTitleLbl);
            this.Controls.Add(this.cooldownTextBox);
            this.Controls.Add(this.startStopBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "EpicTMR";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.Button startStopBtn;
        private System.Windows.Forms.TextBox cooldownTextBox;
        private System.Windows.Forms.Label cooldownTitleLbl;
        private System.Windows.Forms.Label intervalTitleLbl;
        private System.Windows.Forms.TextBox intervalTextBox;
    }
}


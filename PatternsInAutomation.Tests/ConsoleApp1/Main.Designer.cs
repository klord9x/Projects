﻿namespace AutoDataVPBank
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
            this.btnRun = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSignFo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSignTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboActive = new System.Windows.Forms.ComboBox();
            this.radioButtonCAS = new System.Windows.Forms.RadioButton();
            this.radioButtonCASSystem = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(132, 182);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "RUN";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(87, 40);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(120, 20);
            this.txtPass.TabIndex = 1;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(87, 14);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(120, 20);
            this.txtUser.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // txtSignFo
            // 
            this.txtSignFo.Location = new System.Drawing.Point(87, 103);
            this.txtSignFo.Name = "txtSignFo";
            this.txtSignFo.Size = new System.Drawing.Size(120, 20);
            this.txtSignFo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "DateFrom";
            // 
            // txtSignTo
            // 
            this.txtSignTo.Location = new System.Drawing.Point(87, 129);
            this.txtSignTo.Name = "txtSignTo";
            this.txtSignTo.Size = new System.Drawing.Size(120, 20);
            this.txtSignTo.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "DateTo";
            // 
            // cboActive
            // 
            this.cboActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActive.FormattingEnabled = true;
            this.cboActive.Location = new System.Drawing.Point(31, 155);
            this.cboActive.Name = "cboActive";
            this.cboActive.Size = new System.Drawing.Size(176, 21);
            this.cboActive.TabIndex = 5;
            // 
            // radioButtonCAS
            // 
            this.radioButtonCAS.AutoSize = true;
            this.radioButtonCAS.Checked = true;
            this.radioButtonCAS.Location = new System.Drawing.Point(31, 66);
            this.radioButtonCAS.Name = "radioButtonCAS";
            this.radioButtonCAS.Size = new System.Drawing.Size(46, 17);
            this.radioButtonCAS.TabIndex = 6;
            this.radioButtonCAS.TabStop = true;
            this.radioButtonCAS.Text = "CAS";
            this.radioButtonCAS.UseVisualStyleBackColor = true;
            // 
            // radioButtonCASSystem
            // 
            this.radioButtonCASSystem.AutoSize = true;
            this.radioButtonCASSystem.Location = new System.Drawing.Point(83, 66);
            this.radioButtonCASSystem.Name = "radioButtonCASSystem";
            this.radioButtonCASSystem.Size = new System.Drawing.Size(124, 17);
            this.radioButtonCASSystem.TabIndex = 6;
            this.radioButtonCASSystem.Text = "CAS Archival System";
            this.radioButtonCASSystem.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 214);
            this.Controls.Add(this.radioButtonCASSystem);
            this.Controls.Add(this.radioButtonCAS);
            this.Controls.Add(this.cboActive);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtSignTo);
            this.Controls.Add(this.txtSignFo);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "AutoDataVPBank";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSignFo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSignTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboActive;
        private System.Windows.Forms.RadioButton radioButtonCAS;
        private System.Windows.Forms.RadioButton radioButtonCASSystem;
    }
}
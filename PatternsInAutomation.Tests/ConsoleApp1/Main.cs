using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AutoDataVPBank.core;
using AutoDataVPBank.Pages.FecreditPage;
using static AutoDataVPBank.Library;

namespace AutoDataVPBank
{
    public class MainForm : Form
    {
        private string _serial = "";
        private static MainForm _inst;
        private static readonly FecreditLoginPage FecreditLoginPage = FecreditLoginPage.GetInstance;

        public MainForm()
        {
            InitializeComponent();
            // Initialize log4net.
            log4net.Config.XmlConfigurator.Configure();
        }

        public static MainForm GetInstance
        {
            get
            {
                if (_inst == null || _inst.IsDisposed)
                    _inst = new MainForm();
                return _inst;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DefaultData();
        }

        private void DefaultData()
        {
            //getSerial
            _serial = CheckKey.getSerial();
            txtSerial.Text = _serial;
            //
            labContactMe.Text = @"Contact me: CÔNG TY TNHH CÔNG NGHỆ METAFAT";
            labEmail.Text = @"Email: metafatvn@gmail.com - Phone: 0896892998";
            //
            var listProduct = new List<Product>
            {
                new Product {Val = "AUTO", Dis = "PRODUCT CATEGORY FOR AUTO LOANS"},
                new Product {Val = "PERSONAL", Dis = "PRODUCT CATEGORY FOR PERSONAL LOANS"}
            };
            cboProDuct.DataSource = listProduct.ToList();
            cboProDuct.DisplayMember = "dis";
            cboProDuct.ValueMember = "val";
            cboActive.Items.AddRange(new object[]
            {
                "Select",
                "Reject Review"
            });

            txtUser.Text = @"CC100278";
            txtPass.Text = @"Duyminh@2";
            txtSignFo.Text = @"01/10/2017";
            txtSignTo.Text = @"03/10/2017";
            cboActive.SelectedItem = "Reject Review";
            cboBrowser.SelectedItem = "Chrome"; //Firefox
            cboProDuct.SelectedValue = "AUTO";
            GetSetting();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckKey.checkSerial(_serial))
                {
                    Application.Exit();
                    return;
                }
                try
                {
                    var dateAsignFrom =
                        DateTime.ParseExact(txtSignFo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dateAsignTo = DateTime.ParseExact(txtSignTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (dateAsignTo.Subtract(dateAsignFrom).TotalDays >= 30 || dateAsignFrom >= dateAsignTo)
                    {
                        MessageBox.Show(@"Lỗi ngày tháng!");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(@"Lối thông tin nhâp!");
                    Logg.Error(@"Lối thông tin nhâp!");
                    return;
                }
                var text = btnRun.Text;
                switch (text)
                {
                    case "Start":
                        btnRun.Text = @"Stop";
                        FecreditLoginPage.Start();
                        break;
                    default:
                        btnRun.Text = @"Start";
                        BaseWorker.Stop();
                        break;
                }
            }
            catch (Exception exception)
            {
                Logg.Error(exception.Message);
                throw;
            }
            
        }

        private void Main_KeyDown(object sender, KeyEventArgs e) //
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode == Keys.K)
                txtSerial.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtSerial.Visible = true;
        }

        private void cboActive_SelectedValueChanged(object sender, EventArgs e)
        {

        }

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnRun = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboActive = new System.Windows.Forms.ComboBox();
            this.radioButtonCAS = new System.Windows.Forms.RadioButton();
            this.radioButtonCASSystem = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cboBrowser = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.labContactMe = new System.Windows.Forms.Label();
            this.labEmail = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cboProDuct = new System.Windows.Forms.ComboBox();
            this.lb_process_status = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSignFo = new System.Windows.Forms.TextBox();
            this.txtSignTo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(254, 142);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(60, 48);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Start";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(254, 42);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(60, 20);
            this.txtPass.TabIndex = 1;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(92, 42);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(97, 20);
            this.txtUser.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "User";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(195, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "DateFrom";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(195, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "to";
            // 
            // cboActive
            // 
            this.cboActive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboActive.FormattingEnabled = true;
            this.cboActive.Location = new System.Drawing.Point(92, 142);
            this.cboActive.Name = "cboActive";
            this.cboActive.Size = new System.Drawing.Size(156, 21);
            this.cboActive.TabIndex = 5;
            this.cboActive.SelectedValueChanged += new System.EventHandler(this.cboActive_SelectedValueChanged);
            // 
            // radioButtonCAS
            // 
            this.radioButtonCAS.AutoSize = true;
            this.radioButtonCAS.Checked = true;
            this.radioButtonCAS.Location = new System.Drawing.Point(96, 68);
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
            this.radioButtonCASSystem.Location = new System.Drawing.Point(172, 68);
            this.radioButtonCASSystem.Name = "radioButtonCASSystem";
            this.radioButtonCASSystem.Size = new System.Drawing.Size(124, 17);
            this.radioButtonCASSystem.TabIndex = 6;
            this.radioButtonCASSystem.Text = "CAS Archival System";
            this.radioButtonCASSystem.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Navy;
            this.label5.Location = new System.Drawing.Point(32, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(198, 24);
            this.label5.TabIndex = 7;
            this.label5.Text = "VPBANK CRAWLER";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Activity";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Browser";
            // 
            // cboBrowser
            // 
            this.cboBrowser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBrowser.FormattingEnabled = true;
            this.cboBrowser.Items.AddRange(new object[] {
                "Firefox",
                "Chrome"});
            this.cboBrowser.Location = new System.Drawing.Point(92, 169);
            this.cboBrowser.Name = "cboBrowser";
            this.cboBrowser.Size = new System.Drawing.Size(156, 21);
            this.cboBrowser.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Mode";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(0, 226);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(354, 20);
            this.txtSerial.TabIndex = 11;
            this.txtSerial.Visible = false;
            // 
            // labContactMe
            // 
            this.labContactMe.AutoSize = true;
            this.labContactMe.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labContactMe.ForeColor = System.Drawing.Color.Red;
            this.labContactMe.Location = new System.Drawing.Point(251, 9);
            this.labContactMe.Name = "labContactMe";
            this.labContactMe.Size = new System.Drawing.Size(83, 15);
            this.labContactMe.TabIndex = 12;
            this.labContactMe.Text = "Contact me:";
            this.labContactMe.Visible = false;
            // 
            // labEmail
            // 
            this.labEmail.AutoSize = true;
            this.labEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labEmail.ForeColor = System.Drawing.Color.Red;
            this.labEmail.Location = new System.Drawing.Point(266, 18);
            this.labEmail.Name = "labEmail";
            this.labEmail.Size = new System.Drawing.Size(48, 15);
            this.labEmail.TabIndex = 13;
            this.labEmail.Text = "Email:";
            this.labEmail.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Location = new System.Drawing.Point(344, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(10, 23);
            this.button1.TabIndex = 14;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 119);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Product";
            // 
            // cboProDuct
            // 
            this.cboProDuct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProDuct.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProDuct.FormattingEnabled = true;
            this.cboProDuct.Location = new System.Drawing.Point(92, 116);
            this.cboProDuct.Name = "cboProDuct";
            this.cboProDuct.Size = new System.Drawing.Size(222, 20);
            this.cboProDuct.TabIndex = 15;
            // 
            // lb_process_status
            // 
            this.lb_process_status.AutoSize = true;
            this.lb_process_status.Location = new System.Drawing.Point(93, 205);
            this.lb_process_status.Name = "lb_process_status";
            this.lb_process_status.Size = new System.Drawing.Size(38, 13);
            this.lb_process_status.TabIndex = 4;
            this.lb_process_status.Text = "Stop...";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 205);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Status:";
            // 
            // txtSignFo
            // 
            this.txtSignFo.Location = new System.Drawing.Point(92, 90);
            this.txtSignFo.Name = "txtSignFo";
            this.txtSignFo.Size = new System.Drawing.Size(97, 20);
            this.txtSignFo.TabIndex = 1;
            // 
            // txtSignTo
            // 
            this.txtSignTo.Location = new System.Drawing.Point(217, 90);
            this.txtSignTo.Name = "txtSignTo";
            this.txtSignTo.Size = new System.Drawing.Size(97, 20);
            this.txtSignTo.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 249);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cboProDuct);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labEmail);
            this.Controls.Add(this.labContactMe);
            this.Controls.Add(this.txtSerial);
            this.Controls.Add(this.cboBrowser);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.radioButtonCASSystem);
            this.Controls.Add(this.radioButtonCAS);
            this.Controls.Add(this.cboActive);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lb_process_status);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtSignTo);
            this.Controls.Add(this.txtSignFo);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "AutoDataVPBank";
            this.Load += new System.EventHandler(this.Main_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnRun;
        public System.Windows.Forms.TextBox txtPass;
        public System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cboActive;
        public System.Windows.Forms.RadioButton radioButtonCAS;
        private System.Windows.Forms.RadioButton radioButtonCASSystem;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox cboBrowser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.Label labContactMe;
        private System.Windows.Forms.Label labEmail;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox cboProDuct;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.Label lb_process_status;
        public System.Windows.Forms.TextBox txtSignFo;
        public System.Windows.Forms.TextBox txtSignTo;
    }
}

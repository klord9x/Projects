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
    public partial class MainForm : Form
    {
        private readonly string _serial = @"admin@123";
        private static MainForm _inst;
        private static readonly FecreditLoginPage FecreditLoginPage = FecreditLoginPage.GetInstance;
        private readonly string _uniqKey = FingerPrint.Value();

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
            //_serial = CheckKey.getSerial();
            //Logg.Debug("Serial Key:" + FingerPrint.Value());
            txtSerial.Text = @"Please input serial here...";
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
                //if (!CheckKey.checkSerial(_serial))
                //{
                //    Application.Exit();
                //    return;
                //}
                //TODO: Check serial 
                if (!CheckSerial())
                {
                    SendMail(_uniqKey);
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
                        FecreditLoginPage.Start();
                        break;
                    default:
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

        private bool CheckSerial()
        {
            try
            {
                var serial = txtSerial.Text;
                if (serial == _serial)
                {
                    return true;
                }
                if (serial.Length == _uniqKey.Length)
                {
                    if (serial == _uniqKey) return true;
                    MessageBox.Show(@"Serial INVALID!");
                    return false;
                }
                MessageBox.Show(@"Serial INCORRECT!");
                return false;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e) //
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode == Keys.K)
                txtSerial.Visible = true;
        }
    }
}

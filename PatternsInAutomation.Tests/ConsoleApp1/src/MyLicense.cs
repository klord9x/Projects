using System.ComponentModel;
using System.Xml.Serialization;
using QLicense;

namespace AutoDataVPBank
{
    public class MyLicense : LicenseEntity
    {
        [DisplayName("Enable Feature Full")]
        [Category("License Options")]        
        [XmlElement("EnableFeatureFull")]
        [ShowInLicenseInfo(true, "Enable Feature Full", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool EnableFeatureFull { get; set; }

        public MyLicense()
        {
            //Initialize app name for the license
            AppName = "AutoDataVPBank";
        }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus licStatus;
            validationMsg = string.Empty;

            switch (Type)
            {
                case LicenseTypes.Single:
                    //For Single License, check whether UID is matched
                    if (UID == LicenseHandler.GenerateUID(AppName))
                    {
                        licStatus = LicenseStatus.VALID;
                    }
                    else
                    {
                        validationMsg = "The license is NOT for this copy!";
                        licStatus = LicenseStatus.INVALID;                    
                    }
                    break;
                case LicenseTypes.Volume:
                    //No UID checking for Volume License
                    licStatus = LicenseStatus.VALID;
                    break;
                default:
                    validationMsg = "Invalid license";
                    licStatus= LicenseStatus.INVALID;
                    break;
            }

            return licStatus;
        }
    }
}

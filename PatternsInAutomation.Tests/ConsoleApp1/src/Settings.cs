using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace AutoDataVPBank
{
    public class Setting
    {
        public Setting(string moduleName, string key, string value = null)
        {
            ModuleName = moduleName;
            Key = key;
            Value = value;
        }
        [Required]
        [Display(Name = "module_name")]
        [JsonProperty(PropertyName = "module_name")]
        public string ModuleName { get; set; }
        [Required]
        [Display(Name = "key")]
        [JsonProperty(PropertyName = "setting_key")]
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [Required]
        [Display(Name = "value")]
        [JsonProperty(PropertyName = "setting_value")]
        public string Value { get; set; }
    }

    public class SpeedInput
    {
        public int Min { get; set; }
        public int Max { get; set; }
    }

    [XmlRoot(ElementName = "EndPointSettings")]
    public class EndPointSettings
    {
        [XmlAttribute(AttributeName = "url")]
        public string Url { get; set; } = @"http://traffic-sys.info";

        public string Value => Url;
    }

    [XmlRoot(ElementName = "PathSettings")]
    public class PathSettings
    {
        [XmlAttribute(AttributeName = "root")]
        public string Root { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        [XmlAttribute(AttributeName = "img")]
        public string Img { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\img";
        [XmlIgnore]
        public string Log => Root + "log";
        [XmlIgnore]
        public string Ini { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini";
        public string Value => Root;
    }

    [XmlRoot(ElementName = "Times")]
    public class Times
    {
        [XmlAttribute(AttributeName = "min")]
        public int Min { get; set; }
        [XmlAttribute(AttributeName = "max")]
        public int Max { get; set; }
        [XmlAttribute(AttributeName = "real")]
        public int Real { get; set; }

        public int GetValue(string type = null)
        {
            switch (type)
            {
                case "min":
                    return Min;
                case "max":
                    return Max;
                case "real":
                    return Real;
                default:
                    return RandomNumber.Between(Min, Max);
            }
        }

        public string GetAllTimes()
        {
            return $"min={Min}|max={Max}|real={Real}";
        }
    }

    [XmlRoot(ElementName = "Click")]
    public class Click
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();

        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "SendKey")]
    public class SendKey
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "Default")]
    public class Default
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "Total")]
    public class Total
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "SignUp")]
    public class SignUp
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "TryTimes")]
    public class TryTimes
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "ClickSendCode")]
    public class ClickSendCode
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "GetCodeConfirm")]
    public class GetCodeConfirm
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "PageLoad")]
    public class PageLoad
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();
        public string Value => Times.GetAllTimes();
    }

    [XmlRoot(ElementName = "CloneNickSettings")]
    public class CloneNickSettings
    {
        [XmlElement(ElementName = "Click")]
        public Click Click { get; set; } = new Click();
        [XmlElement(ElementName = "ClickSendCode")]
        public ClickSendCode ClickSendCode { get; set; } = new ClickSendCode();
        [XmlElement(ElementName = "TryTimes")]
        public TryTimes TryTimes { get; set; } = new TryTimes();
        [XmlElement(ElementName = "SendKey")]
        public SendKey SendKey { get; set; } = new SendKey();
        [XmlElement(ElementName = "Default")]
        public Default Default { get; set; } = new Default();
        [XmlElement(ElementName = "SignUp")]
        public SignUp SignUp { get; set; } = new SignUp();
        [XmlElement(ElementName = "GetCodeConfirm")]
        public GetCodeConfirm GetConfirmCode { get; set; } = new GetCodeConfirm();
        [XmlElement(ElementName = "PageLoad")]
        public PageLoad PageLoad { get; set; } = new PageLoad();
    }

    [XmlRoot(ElementName = "JoinGroupSettings")]
    public class JoinGroupSettings
    {
        [XmlElement(ElementName = "Click")]
        public Click Click { get; set; } = new Click();
        [XmlElement(ElementName = "SendKey")]
        public SendKey SendKey { get; set; } = new SendKey();
        [XmlElement(ElementName = "Default")]
        public Default Default { get; set; } = new Default();
        [XmlElement(ElementName = "Total")]
        public Total Total { get; set; } = new Total();
    }

    [XmlRoot(ElementName = "Settings")]
    public class Settings
    {
        private static Settings _instance;

        public static Settings GetInstance()
        {
            return _instance ?? (_instance = new Settings());
        }
        [XmlElement(ElementName = "EndPointSettings")]
        public EndPointSettings EndPointSettings { get; set; } = new EndPointSettings();
        [XmlElement(ElementName = "PathSettings")]
        public PathSettings PathSettings { get; set; } = new PathSettings();
        [XmlElement(ElementName = "CloneNickSettings")]
        public CloneNickSettings CloneNickSettings { get; set; } = new CloneNickSettings();
        [XmlElement(ElementName = "JoinGroupSettings")]
        public JoinGroupSettings JoinGroupSettings { get; set; } = new JoinGroupSettings();
        [XmlElement(ElementName = "SpeedInputSettings")]
        public SpeedInput SpeedInputSettings { get; set; } = new SpeedInput();
        
    }
}

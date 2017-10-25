using System;
using System.Xml.Serialization;

namespace AutoDataVPBank
{
    [XmlRoot(ElementName = "Urls")]
    public class Urls
    {
        [XmlAttribute(AttributeName = "login")]
        public string Login { get; set; } = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";

        public string Value => Login;
    }

    [XmlRoot(ElementName = "Paths")]
    public class Paths
    {
        [XmlAttribute(AttributeName = "root")]
        public string Root { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        [XmlAttribute(AttributeName = "data")]
        public string Data { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\data";

        [XmlIgnore]
        public string Ini { get; set; } = AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini";
        public string Value => Root;
    }

    [XmlRoot(ElementName = "Times")]
    public class Times
    {
        [XmlAttribute(AttributeName = "min")]
        public int Min { get; set; } = 5;

        [XmlAttribute(AttributeName = "max")]
        public int Max { get; set; } = 15;

        public int GetValue(string type = null)
        {
            switch (type)
            {
                case "min":
                    return Min;
                case "max":
                    return Max;
                default:
                    return RandomNumber.Between(Min, Max);
            }
        }
    }
    [XmlRoot(ElementName = "BaseTime")]
    public abstract class BaseTime
    {
        [XmlElement(ElementName = "Times")]
        public Times Times { get; set; } = new Times();

        public int Value => Times.GetValue();
    }

    [XmlRoot(ElementName = "Click")]
    public class Click : BaseTime { }

    [XmlRoot(ElementName = "Default")]
    public class Default : BaseTime { }

    [XmlRoot(ElementName = "Total")]
    public class Total : BaseTime { }

    [XmlRoot(ElementName = "TryTimes")]
    public class TryTimes : BaseTime { }

    [XmlRoot(ElementName = "PageLoad")]
    public class PageLoad : BaseTime { }

    [XmlRoot(ElementName = "Fecredit")]
    public class Fecredit
    {
        [XmlElement(ElementName = "Click")]
        public Click Click { get; set; } = new Click();
        [XmlElement(ElementName = "TryTimes")]
        public TryTimes TryTimes { get; set; } = new TryTimes();
        [XmlElement(ElementName = "Default")]
        public Default Default { get; set; } = new Default();
        [XmlElement(ElementName = "PageLoad")]
        public PageLoad PageLoad { get; set; } = new PageLoad();
        [XmlElement(ElementName = "Urls")]
        public Urls Urls { get; set; } = new Urls();
        [XmlElement(ElementName = "Paths")]
        public Paths Paths { get; set; } = new Paths();
    }

    [XmlRoot(ElementName = "Settings")]
    public class Settings
    {
        private static Settings _instance;
        public static Settings GetInstance => _instance ?? (_instance = new Settings());

        [XmlElement(ElementName = "Fecredit")]
        public Fecredit Fecredit { get; set; } = new Fecredit();
    }
}

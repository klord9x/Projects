using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using Newtonsoft.Json;
using OpenQA.Selenium;
using Exception = System.Exception;
using Image = System.Drawing.Image;

namespace AutoDataVPBank
{
    public class Library
    {
        private static Library _instance;
        private static readonly List<string> ProcessNames = new List<string>
        {
            "geckodriver.exe", "firefox.exe"
            , "chromedriver.exe", "chrome.exe"
        };
        public static Settings Set = Settings.GetInstance;

        public static CancellationTokenSource CancelTokenSrc = new CancellationTokenSource();
        public static readonly ManualResetEvent ShutdownEvent = new ManualResetEvent(false);
        public static readonly ManualResetEvent PauseEvent = new ManualResetEvent(true);
        public static bool IsRun;
        public static readonly MainForm MForm = MainForm.GetInstance;
        //Declare an instance for log4net
        public static readonly ILog Logg =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string JobName;

        [DllImport("wininet.dll")]
        private static extern bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        public bool CheckConnection(string url)
        {
            var s = new Stopwatch();
            var t = Set.Fecredit.PageLoad.Times.GetValue();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(t))
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Timeout = 1000;
                    request.Credentials = CredentialCache.DefaultNetworkCredentials;
                    var response = (HttpWebResponse)request.GetResponse();

                    if (response.StatusCode == HttpStatusCode.OK)
                        return true;
                }
                catch (Exception e)
                {
                    Logg.Error(e.Message);
                    Thread.Sleep(5000);
                }
            }

            s.Stop();
            return false;
        }

        public static bool CheckForInternetConnection(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead(url))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                return false;
            }
        }

        /// <summary>
        /// TODO: Need improve
        /// </summary>
        /// <param name="taskname"></param>
        public static void EndTask(string taskname = null)
        {
            var processName = taskname;
            if (taskname != null)
            {
                var fixstring = taskname.Replace(".exe", "");

                if (taskname.Contains(".exe"))
                {
                    foreach (var process in Process.GetProcessesByName(fixstring))
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (Win32Exception)
                        {
                             Logg.Error(@"The process is terminating or could not be terminated.");
                        }

                        catch (InvalidOperationException)
                        {
                             Logg.Error(@"The process has already exited.");
                        }

                        catch (Exception e)  // some other exception
                        {
                             Logg.Error($"{e} Exception caught.");
                        }
                    }
                }
                else if (!taskname.Contains(".exe"))
                {
                    foreach (var process in Process.GetProcessesByName(processName))
                    {
                        //System.ComponentModel.Win32Exception: 'Access is denied'
                        try
                        {
                            process.Kill();
                        }
                        catch (Win32Exception)
                        {
                             Logg.Error(@"The process is terminating or could not be terminated.");
                        }

                        catch (InvalidOperationException)
                        {
                             Logg.Error(@"The process has already exited.");
                        }

                        catch (Exception e)  // some other exception
                        {
                             Logg.Error($"{e} Exception caught.");
                        }
                    }
                }
            }
        }

        public static void KillProcess()
        {
            //TODO: Need exist all process running released app.
            Logg.Info("Kill Process...");
            foreach (var process in ProcessNames)
            {
                EndTask(process);
            }
        }

        public static Library GetInstance => _instance ?? (_instance = new Library());

        /// <summary>
        /// Remove Diacritics from a string
        /// This converts accented characters to nonaccented, which means it is
        /// easier to search for matching data with or without such accents.
        /// This code from Micheal Kaplans Blog:
        ///    http://blogs.msdn.com/b/michkap/archive/2007/05/14/2629747.aspx
        /// Respaced and converted to an Extension Method
        /// <example>
        ///    aàáâãäåçc
        /// is converted to
        ///    aaaaaaacc
        /// </example>
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String RemoveDiacritics(String s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool turnon);

        public static void ScreenCapture(IWebDriver driver)
        {
            var arrScreen = ((ITakesScreenshot)driver).GetScreenshot().AsByteArray;
            using (var msScreen = new MemoryStream(arrScreen))
            {
                var bmpScreen = new Bitmap(msScreen);
                var rcCrop = new Rectangle(driver.Manage().Window.Position,driver.Manage().Window.Size);
                Image imgCap = bmpScreen.Clone(rcCrop, bmpScreen.PixelFormat);

                using (var msCaptcha = new MemoryStream())
                {
                    imgCap.Save(msCaptcha, ImageFormat.Png);
                    Logg.Debug("Saved screen capture");
                }
            }
        }

        public static string GetPropertyValue(object ob, string propertyName = "Value")
        {
            if (ob != null)
                return ob.GetType().GetProperties()
                    .Single(pi => pi.Name == propertyName)
                    .GetValue(ob, null).ToString();
            return null;
        }

        public static object GetPropValue(object src, string propName = "Value")
        {
            var propertyInfo = src.GetType().GetProperty(propName);
            if (propertyInfo == null) return null;

            return propertyInfo.GetValue(src, null);
        }
        /// <summary>
        /// Get Object from xml string and Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T ObjectToXml<T>(string xml, Type type) where T : new()
        {
            XmlTextReader xmlReader = null;
            try
            {
                var strReader = new StringReader(xml);
                var serializer = new XmlSerializer(type);
                xmlReader = new XmlTextReader(strReader);
                return (T) serializer.Deserialize(xmlReader);
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                xmlReader?.Close();
            }
            return new T();
        }

        public static string GetXmlFromObject(object o)
        {
            var sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Writes the given object instance to a binary file.
        /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
        /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the XML file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the XML file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }

        public static TimeSpan TimeOuts => TimeSpan.FromSeconds(69);

        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = RandomNumber.Between(0, n - 1);
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Pause()
        {
            Logg.Info("Thread Paused for Job: " + JobName);
            PauseEvent.Reset();
        }

        public static void Resume()
        {
            Logg.Info("Thread Resumed for Job:" + JobName);
            PauseEvent.Set();
        }

        public static bool CancelTask()
        {
            try
            {
                var token = CancelTokenSrc.Token;
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                    return true;
                }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
            return false;
        }

        public static bool PauseTask()
        {
            try
            {
                PauseEvent.WaitOne(Timeout.Infinite);
                return ShutdownEvent.WaitOne(0);
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        public static void GetSetting()
        {
            try
            {
                if (File.Exists(Set.Fecredit.Paths.Ini))
                {
                    Set = ReadFromXmlFile<Settings>(Set.Fecredit.Paths.Ini);
                }
                else
                {
                    StoreSetting(Set);
                }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
            
        }

        public static void StoreSetting(Settings data)
        {
            try
            {
                WriteToXmlFile(data.Fecredit.Paths.Ini, data);
                Set = data;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        public static void SendMail(string key)
        {
            try
            {
                var fromAddress = new MailAddress("fromEmail", "From Name");
                var toAddress = new MailAddress("toEmail", "To Name");
                const string fromPassword = "fromPassword";
                const string subject = "[Key AutoData VPBank]";
                var body = "Serial:" + key;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }
    }
}

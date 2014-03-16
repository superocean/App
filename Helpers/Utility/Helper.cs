using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace System
{
    public static class Helper
    {
        public static string DecodeXml(this string st)
        {
            return System.Web.HttpUtility.UrlDecode(st);
        }

        public static string EncodeXml(this string st)
        {
            return System.Web.HttpUtility.UrlEncode(st);
        }

        public static string MapPathFiles()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + "Content\\Files\\";
        }

        public static string MapPathData()
        {
            return System.Web.HttpContext.Current.Request.MapPath("~/Data/");
        }

        public static string PathFiles()
        {
            return "/Content/Files/";
        }

        public static string GetRandomString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, false));
            builder.Append(RandomInt(1000, 9999));
            builder.Append(RandomString(4, false));
            return builder.ToString();
        }

        public static int RandomInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static int GetRandomInt()
        {
            return RandomInt(100000000, 999999999);
        }

        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(26 * random.NextDouble() + 65));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        static public byte[] GetBytesFromUrl(string url)
        {
            byte[] b;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse myResp = myReq.GetResponse();

            Stream stream = myResp.GetResponseStream();

            //int i;
            using (BinaryReader br = new BinaryReader(stream))
            {
                //i = (int)(stream.Length);
                b = br.ReadBytes(5000000);
                br.Close();
            }
            myResp.Close();
            return b;
        }

        static public void WriteBytesToFile(string fileName, byte[] content)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs);
            try
            {
                w.Write(content);
            }
            finally
            {
                fs.Close();
                w.Close();
            }
        }

        public static string MapPathImageTemp()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
        }

        public static string RemoveUnicode(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich <= stFormD.Length - 1; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string MapPathSearchIndex()
        {
            string rootPath = System.AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);
            string parentPath = rootDir.Parent.FullName;
            return parentPath + "\\DealSearchEngine\\DataSearch\\";
        }

        public static string MapPathImageFile()
        {
            return "/DataSearch/Images/";
        }

        public static string ImageLogoUrl(string logoName)
        {
            return "~/Content/Images/Logo/" + logoName;
        }

        public static string Serialize(object oObject, bool Indent = false)
        {
            System.Xml.Serialization.XmlSerializer oXmlSerializer = null;
            System.Text.StringBuilder oStringBuilder = null;
            System.Xml.XmlWriter oXmlWriter = null;
            string sXML = null;
            System.Xml.XmlWriterSettings oXmlWriterSettings = null;
            System.Xml.Serialization.XmlSerializerNamespaces oXmlSerializerNamespaces = null;

            // -----------------------------------------------------------------------------------------------------------------------
            // Lage XML
            // -----------------------------------------------------------------------------------------------------------------------
            oStringBuilder = new System.Text.StringBuilder();
            oXmlSerializer = new System.Xml.Serialization.XmlSerializer(oObject.GetType());
            oXmlWriterSettings = new System.Xml.XmlWriterSettings();
            oXmlWriterSettings.OmitXmlDeclaration = true;
            oXmlWriterSettings.Indent = Indent;
            oXmlWriter = System.Xml.XmlWriter.Create(new System.IO.StringWriter(oStringBuilder), oXmlWriterSettings);
            oXmlSerializerNamespaces = new System.Xml.Serialization.XmlSerializerNamespaces();
            oXmlSerializerNamespaces.Add(string.Empty, string.Empty);
            oXmlSerializer.Serialize(oXmlWriter, oObject, oXmlSerializerNamespaces);
            oXmlWriter.Close();
            sXML = oStringBuilder.ToString();

            return sXML;
        }

        public static object DeSerialize(string sXML, Type ObjectType)
        {
            System.IO.StringReader oStringReader = null;
            System.Xml.Serialization.XmlSerializer oXmlSerializer = null;
            object oObject = null;

            // -----------------------------------------------------------------------------------------------------------------------
            // Hvis mangler info, lage tom
            // -----------------------------------------------------------------------------------------------------------------------
            if (sXML == string.Empty)
            {
                Type[] types = new Type[-1 + 1];
                ConstructorInfo info = ObjectType.GetConstructor(types);
                object targetObject = info.Invoke(null);
                if (targetObject == null)
                    return null;
                return targetObject;
            }

            // -----------------------------------------------------------------------------------------------------------------------
            // Gjøre om fra XML til objekt
            // -----------------------------------------------------------------------------------------------------------------------
            oStringReader = new System.IO.StringReader(sXML);
            oXmlSerializer = new System.Xml.Serialization.XmlSerializer(ObjectType);
            oObject = oXmlSerializer.Deserialize(oStringReader);

            return oObject;
        }

        public static string FormatPrice(string textPrice)
        {
            string sReturn;
            string text = textPrice;
            List<string> group = new List<string>();
            Group3Character(ref text, ref group);
            string groupstr = string.Empty;
            foreach (string item in group)
            {
                groupstr += item;
            }
            if (text.Length != 0)
            {
                sReturn = text + groupstr;
            }
            else
            {
                sReturn = groupstr.Substring(1, groupstr.Length);
            }
            return sReturn;
        }

        //16000
        public static void Group3Character(ref string text, ref List<string> group)
        {
            if (text.Length > 2)
            {
                group.Add("." + text.Substring(text.Length - 3, text.Length));
                text = text.Substring(0, text.Length - 4);
                Group3Character(ref text, ref group);
            }
        }

        public static string StringTrim(string text, int nbrChar)
        {
            string result = "";
            if (text.Length > nbrChar)
            {
                bool flag = false;
                string st = text.Trim().Substring(0, nbrChar);
                for (int i = 1; i < nbrChar; i++)
                {
                    int stlength = st.Length;
                    int length = stlength - i;
                    int length1 = length - 1;
                    if (st.EndsWith(" "))
                    {
                        result = st + "...";
                        break;
                    }
                    else
                    {
                        if (st.Substring(0, length).EndsWith(" "))
                        {
                            flag = false;
                            result = st.Substring(0, length1) + "...";
                            break;
                        }
                        else
                        {
                            flag = true;
                        }
                    }
                }
                if (flag)
                {
                    result = "String Name";
                }
            }
            else
            {
                result = text;
            }
            return result;
        }
    }
}
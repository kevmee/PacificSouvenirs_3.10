using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Payments.PaymentExpress.Models
{
    /// <summary>
    /// Class containing properties describing the output of the request
    /// </summary>
    public class RequestOutput
    {

        public RequestOutput(string Xml)
        {
            _Xml = Xml;
            SetProperty();
        }

        private string _valid;
        private string _URI;

        private string _Xml;

        public string valid
        {
            get
            {
                return _valid;
            }
            set
            {
                _valid = value;
            }
        }

        public string URI
        {
            get
            {
                return _URI;
            }
            set
            {
                _URI = value;
            }
        }

        public string Url
        {
            get
            {
                return _URI.Replace("&amp;", "&");
            }

        }

        private void SetProperty()
        {

            XmlReader reader = XmlReader.Create(new StringReader(_Xml));

            while (reader.Read())
            {
                PropertyInfo prop;
                if (reader.NodeType == XmlNodeType.Element)
                {
                    prop = this.GetType().GetProperty(reader.Name);
                    if (prop != null)
                    {
                        this.GetType().GetProperty(reader.Name).SetValue(this, reader.ReadString(), System.Reflection.BindingFlags.Default, null, null, null);
                    }
                    if (reader.HasAttributes)
                    {

                        for (int count = 0; count < reader.AttributeCount; count++)
                        {
                            //Read the current attribute
                            reader.MoveToAttribute(count);
                            prop = this.GetType().GetProperty(reader.Name);
                            if (prop != null)
                            {
                                this.GetType().GetProperty(reader.Name).SetValue(this, reader.Value, System.Reflection.BindingFlags.Default, null, null, null);
                            }
                        }
                    }
                }
            }

        }
    }
}

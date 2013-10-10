using Nop.Plugin.Payments.PaymentExpress.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Nop.Plugin.Payments.PaymentExpress
{
    /// <summary>
    /// Main class for submitting transactions via PxPay using static methods
    /// </summary>
    public class PaymentExpressHelper
    {
        private string _WebServiceUrl;
        private string _PxPayUserId;
        private string _PxPayKey;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PxPayUserId"></param>
        /// <param name="PxPayKey"></param>
        public PaymentExpressHelper(string webserviceUrl, string PxPayUserId, string PxPayKey)
        {
            _WebServiceUrl = webserviceUrl;
            _PxPayUserId = PxPayUserId;
            _PxPayKey = PxPayKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ResponseOutput ProcessResponse(string result)
        {
            ResponseOutput myResult = new ResponseOutput(SubmitXml(ProcessResponseXml(result)));
            return myResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public RequestOutput GenerateRequest(RequestInput input)
        {
            RequestOutput result = new RequestOutput(SubmitXml(GenerateRequestXml(input)));
            return result;
        }

        private string SubmitXml(string InputXml)
        {
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(_WebServiceUrl);
            webReq.Method = "POST";

            byte[] reqBytes;

            reqBytes = System.Text.Encoding.UTF8.GetBytes(InputXml);
            webReq.ContentType = "application/x-www-form-urlencoded";
            webReq.ContentLength = reqBytes.Length;
            webReq.Timeout = 5000;
            Stream requestStream = webReq.GetRequestStream();
            requestStream.Write(reqBytes, 0, reqBytes.Length);
            requestStream.Close();

            HttpWebResponse webResponse = (HttpWebResponse)webReq.GetResponse();
            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.ASCII))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Generates the XML required for a GenerateRequest call
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GenerateRequestXml(RequestInput input)
        {

            StringWriter sw = new StringWriter();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("GenerateRequest");
                writer.WriteElementString("PxPayUserId", _PxPayUserId);
                writer.WriteElementString("PxPayKey", _PxPayKey);

                PropertyInfo[] properties = input.GetType().GetProperties();

                foreach (PropertyInfo prop in properties)
                {
                    if (prop.CanWrite)
                    {
                        string val = (string)prop.GetValue(input, null);

                        if (val != null || val != string.Empty)
                        {

                            writer.WriteElementString(prop.Name, val);
                        }
                    }
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            return sw.ToString();
        }

        /// <summary>
        /// Generates the XML required for a ProcessResponse call
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string ProcessResponseXml(string result)
        {

            StringWriter sw = new StringWriter();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = false;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(sw, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ProcessResponse");
                writer.WriteElementString("PxPayUserId", _PxPayUserId);
                writer.WriteElementString("PxPayKey", _PxPayKey);
                writer.WriteElementString("Response", result);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
            }

            return sw.ToString();
        }

    }
}

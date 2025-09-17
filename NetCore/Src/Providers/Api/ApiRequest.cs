/*
    This file is part of the FACe (R) project.
    Copyright (c) 2025-2026 Irene Solutions SL
    Authors: Irene Solutions SL.

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation with the addition of the
    following permission added to Section 15 as permitted in Section 7(a):
    FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
    IRENE SOLUTIONS SL. IRENE SOLUTIONS SL DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
    OF THIRD PARTY RIGHTS
    
    This program is distributed in the hope that it will be useful, but
    WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
    or FITNESS FOR A PARTICULAR PURPOSE.
    See the GNU Affero General Public License for more details.
    You should have received a copy of the GNU Affero General Public License
    along with this program; if not, see http://www.gnu.org/licenses or write to
    the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
    Boston, MA, 02110-1301 USA, or download the license from the following URL:
        http://www.irenesolutions.com/terms-of-use.pdf
    
    The interactive user interfaces in modified source and object code versions
    of this program must display Appropriate Legal Notices, as required under
    Section 5 of the GNU Affero General Public License.
    
    You can be released from the requirements of the license by purchasing
    a commercial license. Buying such a license is mandatory as soon as you
    develop commercial activities involving the FACe software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving FACe services on the fly in a web application, 
    shipping FACe with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using FACe.Config;
using System.IO;
using System.Net;
using System.Text;

namespace FACe.Providers.Api
{

    /// <summary>
    /// Representa una petición http al API de FACe.
    /// </summary>
    public class ApiRequest
    {

        #region Construtores Estáticos

        /// <summary>
        /// Constructor estático.
        /// </summary>
        static ApiRequest()
        {

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;

        }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="action"> Acción a solicitar. Por defecto entrada de facturas.</param>
        public ApiRequest(string action = "/v1/invoices")
        {

            Action = action;

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Devuelve una instancia de la clase HttpWebRequest
        /// preparada para realizar una petición POST
        /// al API.
        /// </summary>
        /// <param name="tokenJWT">Token JWT.</param>
        /// <param name="url">Url del endpoint.</param>
        /// <returns> Clase HttpWebRequest
        /// preparada para realizar una petición POST</returns>
        private HttpWebRequest CreateHttpWebRequest(string tokenJWT, string url = null)
        {

            if (url == null)
                url = Url;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            httpWebRequest.ContentType = "application/json;charset=utf-8";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.UserAgent = "IsolutionsFace/0.0.0";

            httpWebRequest.Headers.Add("Authorization", $"Bearer {tokenJWT}");
            httpWebRequest.Headers.Add("Cache-Control", "no-cache");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip,deflate");

            return httpWebRequest;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Acción a solicitar.
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Url de la petición.
        /// </summary>
        public string Url
        {

            get
            {

                return $"{Settings.Current.FACeSettings.FACeEndPointPrefix}{Action}";

            }

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Enviar petición a FACe
        /// </summary>
        /// <param name="json">JSON de entrada de la petición.</param>
        /// <param name="tokeJWT">Token JWT.</param>
        /// <returns> Respuesta de la AEAT como texto.</returns>
        public string Send(string json, string tokeJWT)
        {

            string response = null;

            try
            {

                var httpWebRequest = CreateHttpWebRequest(tokeJWT);

                byte[] content = Encoding.UTF8.GetBytes(json);
                httpWebRequest.ContentLength = content.Length;

                using (Stream stream = httpWebRequest.GetRequestStream())
                    stream.Write(content, 0, content.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream dataStream = httpWebResponse.GetResponseStream();

                using (StreamReader reader = new StreamReader(dataStream))
                {

                    response = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    httpWebResponse.Close();

                }

            }
            catch (WebException wex)
            {

                HttpStatusCode? status = null;

                if (wex.Response is HttpWebResponse errResponse)
                {

                    status = errResponse.StatusCode;

                    using (var sr = new StreamReader(errResponse.GetResponseStream(), Encoding.UTF8))
                        response = sr.ReadToEnd();

                    errResponse.Dispose();

                }

            }

            return response;

        }

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return Action;

        }

        #endregion

    }

}
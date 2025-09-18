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
    develop commercial activities involving the Facturae software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using FACe.Providers.Api;
using FACe.Providers.Jwt;
using FACe.Xml.Facturae;
using FACe.Xml.Facturae.Bies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FACe.Business
{

    /// <summary>
    /// Gestiona las operaciones con FACe para un documento
    /// Facturae determinado
    /// </summary>
    public class FACeManager
    {

        #region Variables Privadas Estáticas

        /// <summary>
        /// Constructor de tokens JWT en curso.
        /// </summary>
        static JwtBuilder _CurrentJwtBuilder = null;

        /// <summary>
        /// Bloqueo para thread safe.
        /// </summary>
        static readonly object _Locker = new object();

        #endregion

        #region Propiedades Privadas de Instacia

        /// <summary>
        /// FacturaeManager para trabajos con Facturae.
        /// </summary>
        private FacturaeManager FacturaeManager { get; set; }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="facturae">Instancia de clase Facturae.</param>
        public FACeManager(Facturae facturae)
        {

            FacturaeManager = new FacturaeManager(facturae);

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// A partir de un texto xml de un documento
        /// Facturae sin firmar, devuelve el texto
        /// de un documento Facturae firmado.
        /// </summary>
        /// <param name="certificate">Certificado digital
        /// para la firma.</param>        
        /// <returns>Texto xml de un documento
        /// Facturae firmado.</returns>
        public string GetXmlTextSigned(X509Certificate2 certificate)
        {

            return FacturaeManager.GetXmlTextSigned(certificate);

        }

        /// <summary>
        /// Firma y envía documento a FACe.
        /// </summary>
        /// <param name="certificate">Certificado para la firma.</param>
        /// <param name="attachments">Archivos adjuntos a enviar.</param>
        /// <returns>Texto JSON de la respuesta de FACe.</returns>
        public string Send(X509Certificate2 certificate, List<byte[]> attachments = null)
        {

            var signedXml = GetXmlTextSigned(certificate);
            return SendXmlTextSigned(signedXml, certificate, attachments);

        }

        /// <summary>
        /// Firma y envía documento a FACe.
        /// </summary>
        /// <param name="xmlTextSigned">Texto del xml del documento
        /// Facturae firmado.</param>
        /// <param name="certificate">Certificado para la firma.</param>
        /// <param name="attachments">Archivos adjuntos a enviar.</param>
        /// <returns>Texto JSON de la respuesta de FACe.</returns>
        public string SendXmlTextSigned(string xmlTextSigned, X509Certificate2 certificate, List<byte[]> attachments = null)
        {

            string tokenJWT = null;
            var secondsOffset = 60;

            if (_CurrentJwtBuilder == null || _CurrentJwtBuilder.LifeSeconds < secondsOffset)
            {

                InvalidOperationException refreshException = null;

                lock (_Locker)
                {

                    try
                    {

                        _CurrentJwtBuilder = new JwtBuilder(certificate);

                    }
                    catch (Exception ex)
                    {

                        refreshException = new InvalidOperationException($"Error al renovar el token JWT: {ex.Message}", ex);

                    }

                }

                if (refreshException != null)
                    throw refreshException;

                tokenJWT = _CurrentJwtBuilder.GetToken();

            }
            else
            {

                tokenJWT = _CurrentJwtBuilder.Token;
                Debug.Print($"LifeSeconds = {_CurrentJwtBuilder.LifeSeconds}");

            }

            var invoiceEntry = new InvoiceEntry()
            {
                FileName = "facturae.test.xsig",
                Content = Encoding.UTF8.GetBytes(xmlTextSigned),
                Email = "manuel@irenesolutions.com"
            };

            if (attachments != null && attachments.Count > 0)
            {

                invoiceEntry.Attachements = new List<InvoiceEntryAttachment>();

                foreach (var attachment in attachments)
                {

                    invoiceEntry.Attachements.Add(new InvoiceEntryAttachment()
                    {
                        FileName = "anexo",
                        Content = attachment
                    });

                }

            }

            var apiRequest = new ApiRequest();

            return apiRequest.Send(invoiceEntry.ToJson(), tokenJWT);

        }

        #endregion

    }

}
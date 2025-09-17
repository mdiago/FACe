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

using FACe.Common;
using FACe.Xml.Xades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace FACe.Xml.Facturae
{

    /// <summary>
    /// Gestiona operaciones de una instancia de la
    /// clase Facturae.
    /// </summary>
    public class FacturaeManager
    {

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="facturae">Instancia de Facturae a gestionar</param>
        public FacturaeManager(Bies.Facturae facturae) 
        { 
        
            Facturae = facturae;
        
        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Devuelve true si la cadena codificada en
        /// UTF8 en el aaray de bytes contiene BOM.
        /// En UTF-8 no hay problema de orden de bytes
        /// (endianness), por lo que no es necesario
        /// incluir el BOM. Sin embargo, algunos programas
        /// (por ejemplo, ciertos editores de texto de
        /// Windows o librerías .NET antiguas) lo añaden
        /// al principio de los ficheros para indicar
        /// explícitamente que están codificados en UTF-8.
        /// </summary>
        /// <param name="utf8">Array de bytes con la
        /// cadena codificada en UTF8.</param>
        /// <returns>True si existe BOM.</returns>
        private bool HasUtf8BOM(byte[] utf8) 
        {

            return (utf8.Length >= 3 && utf8[0] == 0xEF && utf8[1] == 0xBB && utf8[2] == 0xBF);

        }

        /// <summary>
        /// Devuelve true si la cadena codificada en
        /// UTF8 en el aaray de bytes contiene BOM.
        /// En UTF-8 no hay problema de orden de bytes
        /// (endianness), por lo que no es necesario
        /// incluir el BOM. Sin embargo, algunos programas
        /// (por ejemplo, ciertos editores de texto de
        /// Windows o librerías .NET antiguas) lo añaden
        /// al principio de los ficheros para indicar
        /// explícitamente que están codificados en UTF-8.
        /// </summary>
        /// <param name="utf8">Array de bytes con la
        /// cadena codificada en UTF8.</param>
        /// <returns>True si existe BOM.</returns>
        private byte[] ClearUtf8BOM(byte[] utf8)
        {

            if (!HasUtf8BOM(utf8))
                return utf8;

            return utf8.Skip(3).ToArray(); 

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Instancia de Facturae gestionada.
        /// </summary>
        public Bies.Facturae Facturae { get; private set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Recupera el xml de la instancia de Facturae como
        /// cadena codificada en UTF8 en un array de bytes.
        /// </summary>
        /// <returns> Xml del documento Facturae en cadena
        /// codificada en UTF8 en un array de bytes.</returns>
        public byte[] GetUTF8Xml()
        {

            byte[] xml = null;

            try 
            {                

                XmlSerializer serializer = new XmlSerializer(Facturae.GetType());

                var xmlNs = new XmlSerializerNamespaces();

                foreach (KeyValuePair<string, string> n in FacturaeNamespaces.Items)
                    xmlNs.Add(n.Key, n.Value);

                var facturaeMS = new MemoryStream();

                using (StreamWriter streamWriter = new StreamWriter(facturaeMS, Encoding.UTF8))
                {
                    serializer.Serialize(streamWriter, Facturae, xmlNs);
                    xml = facturaeMS.ToArray();
                }                

            }
            catch (Exception ex) 
            {

                Utils.Throw($"Error serializando en xml el objeto Facturae.", ex);

            }

            return ClearUtf8BOM(xml);

        }

        /// <summary>
        /// Recupera el xml de la instancia de Facturae como
        /// texto codificado en UTF8.
        /// </summary>
        /// <returns> Xml del documento Facturae en texto
        /// codificado en UTF8.</returns>
        public string GetUTF8XmlText()
        {
            
            return Encoding.UTF8.GetString(GetUTF8Xml());

        }

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

            XadesSigned xadesSigned = new XadesSigned(GetUTF8XmlText(), certificate);
            return xadesSigned.GetSignedXml();

        }

        #endregion

    }

}
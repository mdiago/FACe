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
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FACe.Xml
{

    /// <summary>
    /// Representa un documento de factura para la serialización xml:
    /// Faturae, UBL o CII.
    /// </summary>
    public abstract class InvoiceDocument
    {

        #region Métodos Privados de Instancia

        /// <summary>
        /// Obteine los espacios de nombres del documento.
        /// </summary>
        /// <returns> Espacios de nombres del documento.</returns>

        protected virtual XmlSerializerNamespaces GetNamespaces()
        {

            return new XmlSerializerNamespaces();

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Obtiene el texto xml del documento.
        /// </summary>
        /// <returns> Texto xml del documento.</returns>
        public virtual string GetXml()
        {
            var serializer = new XmlSerializer(GetType());

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, this, GetNamespaces());
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        #endregion

    }

}
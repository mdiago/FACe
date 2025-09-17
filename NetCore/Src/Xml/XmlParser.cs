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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;

namespace FACe.Xml
{

    /// <summary>
    /// Responsable serialización xml.
    /// </summary>
    public class XmlParser
    {

        #region Propiedades Privadas Estáticas

        /// <summary>
        /// Separador decimal por defecto.
        /// </summary>
        internal static string DefaultNumberDecimalSeparator = ".";

        /// <summary>
        /// Formato de importes para los xml.
        /// </summary>
        internal static NumberFormatInfo DefaultNumberFormatInfo = new NumberFormatInfo()
        {
            CurrencyDecimalSeparator = DefaultNumberDecimalSeparator,
            NumberDecimalSeparator = DefaultNumberDecimalSeparator,
            CurrencyDecimalDigits = 2,
            NumberDecimalDigits = 2,
            CurrencyGroupSeparator = "",
            NumberGroupSeparator = ""
        };

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        public XmlParser()
        {

            Encoding = Encoding.GetEncoding("UTF-8");

        }

        #endregion

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Codificación de texto a utilizar. UTF8 por defecto.
        /// </summary>
        public Encoding Encoding { get; private set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Devuelve un importe formateado para un campo decimal
        /// de la especificación de facturae.
        /// </summary>
        /// <param name="amount">Impote a formatear</param>
        /// <param name="numberFormatInfo">Información de formato.</param> 
        /// <returns>Importe formateado.</returns>
        public static string FromDecimal(decimal amount, NumberFormatInfo numberFormatInfo = null)
        {

            if (numberFormatInfo == null)
                numberFormatInfo = DefaultNumberFormatInfo;

            return amount.ToString("N", numberFormatInfo);
        }

        /// <summary>
        /// Convierte en decimal un importe representado por un texto en 
        /// un tag de un xml del facturae.
        /// </summary>
        /// <param name="amount">Texto de importe a convertir.</param>
        /// <param name="numberFormatInfo">Formato de número.</param>
        /// <returns>Importe decimal representado.</returns>
        public static decimal ToDecimal(string amount, NumberFormatInfo numberFormatInfo = null)
        {

            if (numberFormatInfo == null)
                numberFormatInfo = DefaultNumberFormatInfo;

            return Convert.ToDecimal(amount, numberFormatInfo);
        }

        /// <summary>
        /// Serializa el objeto como xml y lo devuelve
        /// como archivo xml como cadena de bytes.
        /// </summary>
        /// <param name="instance">Instancia de objeto a serializar.</param>
        /// <param name="namespaces">Espacios de nombres.</param> 
        /// <param name="indent">Indica si se debe utilizar indentación.</param>
        /// <param name="omitXmlDeclaration">Indica si se se omite la delcaración xml.</param>
        /// <returns>string con el archivo xml.</returns>
        public byte[] GetBytes(object instance, Dictionary<string, string> namespaces,
            bool indent = false, bool omitXmlDeclaration = true)
        {

            XmlSerializer serializer = new XmlSerializer(instance.GetType());

            var xmlSerializerNamespaces = new XmlSerializerNamespaces();

            foreach (KeyValuePair<string, string> ns in namespaces)
                xmlSerializerNamespaces.Add(ns.Key, ns.Value);

            var ms = new MemoryStream();
            byte[] result = null;

            var settings = new XmlWriterSettings
            {
                Indent = indent,
                IndentChars = "",
                Encoding = Encoding,
                OmitXmlDeclaration = omitXmlDeclaration
            };

            using (var writer = new StreamWriter(ms))
            {
                using (var xmlWriter = XmlWriter.Create(writer, settings))
                {

                    serializer.Serialize(xmlWriter, instance, xmlSerializerNamespaces);
                    result = ms.ToArray();

                }
            }

            return result;

        }

        #endregion

    }

}
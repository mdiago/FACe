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
using System.Globalization;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Descuento en factura.
    /// </summary>
    [Serializable()]
    public class Discount
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Concepto por el que se aplica descuento.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DiscountReason { get; set; }

        /// <summary>
        /// Porcentaje de descuento.
        /// </summary>
        [XmlIgnore]
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Porcentaje a descontar del Total Importe Bruto (TIB). 
        /// Los porcentajes se reflejan con hasta 8 decimales.
        /// </summary>
        [XmlElement(ElementName = "DiscountRate", Form = XmlSchemaForm.Unqualified)]
        public string DiscountRateString
        {
            get
            {

                return DiscountRate == 0 ? null : XmlParser.FromDecimal(DiscountRate, new NumberFormatInfo()
                {
                    CurrencyDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    NumberDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    CurrencyDecimalDigits = 4,
                    NumberDecimalDigits = 4,
                    CurrencyGroupSeparator = "",
                    NumberGroupSeparator = ""
                });

            }
            set
            {
                DiscountRate = XmlParser.ToDecimal(value, new NumberFormatInfo()
                {
                    CurrencyDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    NumberDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    CurrencyDecimalDigits = 4,
                    NumberDecimalDigits = 4,
                    CurrencyGroupSeparator = "",
                    NumberGroupSeparator = ""
                });
            }
        }

        /// <summary>
        /// Importe a descontar.
        /// </summary>
        [XmlIgnore]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Importe a descontar sobre el TIB. Hasta ocho decimales.
        /// </summary>
        [XmlElement(ElementName = "DiscountAmount", Form = XmlSchemaForm.Unqualified)]
        public string DiscountAmountString
        {
            get
            {
                return DiscountAmount == 0 ? null : XmlParser.FromDecimal(DiscountAmount, new NumberFormatInfo()
                {
                    CurrencyDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    NumberDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    CurrencyDecimalDigits = 6,
                    NumberDecimalDigits = 6,
                    CurrencyGroupSeparator = "",
                    NumberGroupSeparator = ""
                });
            }
            set
            {
                DiscountAmount = XmlParser.ToDecimal(value, new NumberFormatInfo()
                {
                    CurrencyDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    NumberDecimalSeparator = XmlParser.DefaultNumberDecimalSeparator,
                    CurrencyDecimalDigits = 6,
                    NumberDecimalDigits = 6,
                    CurrencyGroupSeparator = "",
                    NumberGroupSeparator = ""
                });
            }
        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de DiscountType.
        /// </summary>
        /// <returns>Representación textual de la instancia de DiscountType.</returns>
        public override string ToString()
        {
            return $"{DiscountReason}, {DiscountRate}, {DiscountAmount}";
        }

        #endregion

    }

}
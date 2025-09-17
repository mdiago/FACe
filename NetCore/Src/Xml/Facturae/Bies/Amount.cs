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
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Representa un importe en factura.
    /// </summary>
    [Serializable()]
    public class Amount
    {

        #region Conversion Operators

        /// <summary>
        /// Convierte de objeto Amount a decimal.
        /// </summary>
        /// <param name="amount">A number of monetary units specified using a given unit of currency.</param>
        public static implicit operator decimal(Amount amount) => amount.TotalAmount;

        /// <summary>
        /// Convierte de decimal a objeto Amount.
        /// </summary>
        /// <param name="value">Valor decimal de Amount.</param>
        public static implicit operator Amount(decimal value) => new Amount() { TotalAmount = value};

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Importe.
        /// </summary>
        [XmlIgnore]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Importe en la moneda original de la facturación. Siempre que 
        /// la divisa de facturación sea distinta de EURO, el elemento 
        /// EquivalentInEuros deberá cumplimentarse para satisfacer los 
        /// requerimientos del Art.10.1 del Reglamento sobre facturación, 
        /// RD 1496/2003 de 28 de Noviembre.
        /// </summary>
        [XmlElement(ElementName = "TotalAmount", Form = XmlSchemaForm.Unqualified)]
        public string TotalAmountString
        {
            get
            {
                return XmlParser.FromDecimal(TotalAmount);
            }
            set
            {
                TotalAmount = XmlParser.ToDecimal(value);
            }
        }

        /// <summary>
        /// Importe equivalente en Euros. Siempre con dos decimales.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public double EquivalentInEuros { get; set; }

        /// <summary>
        /// Indica si se ha especificado el valor
        /// equivalente en euros.
        /// </summary>
        [XmlIgnore()]
        public bool EquivalentInEurosSpecified { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de AmountType.
        /// </summary>
        /// <returns>Representación textual de la instancia de AmountType.</returns>
        public override string ToString()
        {
            return $"{TotalAmount}";
        }

        #endregion

    }

}
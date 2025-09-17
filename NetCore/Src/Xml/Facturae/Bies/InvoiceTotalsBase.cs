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
using System.Xml.Serialization;
using System.Xml.Schema;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Clase base para InvoiceTotalsBaseType. De ella derivan los InvoiceTotals
    /// de cada una de las versiones: 3.0, 3.1, 3.2, 3.2.2
    /// </summary>
    [Serializable()]
    [XmlType(Namespace = "http://www.facturae.es/Facturae/2009/v3.2/Facturae")]
    public class InvoiceTotalsBase
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Importe bruto
        /// </summary>
        [XmlIgnore]
        public decimal TotalGrossAmount { get; set; }

        /// <summary>
        /// Representación en dos decimales del importe bruto.
        /// </summary>
        [XmlElement(ElementName = "TotalGrossAmount", Form = XmlSchemaForm.Unqualified)]
        public string TotalGrossAmountString
        {

            get
            {

                return XmlParser.FromDecimal(TotalGrossAmount);

            }
            set
            {

                TotalGrossAmount = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total descuentos
        /// </summary>
        [XmlIgnore]
        public decimal TotalGeneralDiscounts { get; set; }

        /// <summary>
        /// Representación en dos decimales de total descuentos.
        /// </summary>
        [XmlElement(ElementName = "TotalGeneralDiscounts", Form = XmlSchemaForm.Unqualified)]
        public string TotalGeneralDiscountsString
        {

            get
            {

                return XmlParser.FromDecimal(TotalGeneralDiscounts);

            }
            set
            {

                TotalGeneralDiscounts = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total cargos.
        /// </summary>
        [XmlIgnore]
        public decimal TotalGeneralSurcharges { get; set; }

        /// <summary>
        /// Total cargos con dos decimales.
        /// </summary>
        [XmlElement(ElementName = "TotalGeneralSurcharges", Form = XmlSchemaForm.Unqualified)]
        public string TotalGeneralSurchargesString
        {

            get
            {

                return XmlParser.FromDecimal(TotalGeneralSurcharges);

            }
            set
            {

                TotalGeneralSurcharges = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Bruto antes de impuestos.
        /// </summary>
        [XmlIgnore]
        public decimal TotalGrossAmountBeforeTaxes { get; set; }

        /// <summary>
        /// Bruto antes de impuestos dos decimales.
        /// </summary>
        [XmlElement(ElementName = "TotalGrossAmountBeforeTaxes", Form = XmlSchemaForm.Unqualified)]
        public string TotalGrossAmountBeforeTaxesString
        {

            get
            {

                return XmlParser.FromDecimal(TotalGrossAmountBeforeTaxes);

            }
            set
            {

                TotalGrossAmountBeforeTaxes = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>
        [XmlIgnore]
        public decimal TotalTaxOutputs { get; set; }

        /// <summary>
        /// Total impuestos soportados dos decimales.
        /// </summary>
        [XmlElement(ElementName = "TotalTaxOutputs", Form = XmlSchemaForm.Unqualified)]
        public string TotalTaxOutputsString
        {

            get
            {

                return XmlParser.FromDecimal(TotalTaxOutputs);

            }
            set
            {

                TotalTaxOutputs = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total impuestos retenidos.
        /// </summary>
        [XmlIgnore]
        public decimal TotalTaxesWithheld { get; set; }

        /// <summary>
        /// Representación dos decimales impuestos retenidos totales.
        /// </summary>
        [XmlElement(ElementName = "TotalTaxesWithheld", Form = XmlSchemaForm.Unqualified)]
        public string TotalTaxesWithheldString
        {

            get
            {

                return XmlParser.FromDecimal(TotalTaxesWithheld);

            }
            set
            {

                TotalTaxesWithheld = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total factura.
        /// </summary>
        [XmlIgnore]
        public decimal InvoiceTotal { get; set; }

        /// <summary>
        /// Total factura con dos decimales.
        /// </summary>
        [XmlElement(ElementName = "InvoiceTotal", Form = XmlSchemaForm.Unqualified)]
        public string InvoiceTotalString
        {

            get
            {

                return XmlParser.FromDecimal(InvoiceTotal);

            }
            set
            {

                InvoiceTotal = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total importe pendiente.
        /// </summary>
        [XmlIgnore]
        public decimal TotalOutstandingAmount { get; set; }

        /// <summary>
        /// Total importe pendiente dos decimales.
        /// </summary>
        [XmlElement(ElementName = "TotalOutstandingAmount", Form = XmlSchemaForm.Unqualified)]
        public string TotalOutstandingAmountString
        {

            get
            {

                return XmlParser.FromDecimal(TotalOutstandingAmount);

            }
            set
            {

                TotalOutstandingAmount = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total importe ejecutable.
        /// </summary>
        [XmlIgnore]
        public decimal TotalExecutableAmount { get; set; }

        /// <summary>
        /// Total importe ejecutable dos decimales.
        /// </summary>
        [XmlElement(ElementName = "TotalExecutableAmount", Form = XmlSchemaForm.Unqualified)]
        public string TotalExecutableAmountString
        {

            get
            {

                return XmlParser.FromDecimal(TotalExecutableAmount);

            }
            set
            {

                TotalExecutableAmount = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Total gastos reembolsables. En la 3.0 no está
        /// </summary>
        [XmlIgnore]
        public decimal TotalReimbursableExpenses { get; set; }

        /// <summary>
        /// Total gastos reembolsables. En la 3.0 no está
        /// </summary>
        [XmlElement(ElementName = "TotalReimbursableExpenses", Form = XmlSchemaForm.Unqualified)]
        public virtual string TotalReimbursableExpensesString
        {

            get
            {

                if (TotalReimbursableExpenses == 0)
                    return null;

                return XmlParser.FromDecimal(TotalReimbursableExpenses);

            }
            set
            {

                TotalReimbursableExpenses = XmlParser.ToDecimal(value);

            }

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{TotalGrossAmount}";

        }

        #endregion

    }

}
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
using System.Globalization;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Representa una línea de factura.
    /// </summary>
    [Serializable()]
    public class InvoiceLineBase
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Referencia contrato.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string IssuerContractReference { get; set; }

        /// <summary>
        /// Fecha de contrato.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime IssuerContractDate { get; set; }

        /// <summary>
        /// Indica si se ha especificado IssuerContractDate.
        /// </summary>
        [XmlIgnore()]
        public bool IssuerContractDateSpecified { get; set; }

        /// <summary>
        /// Referencia de la transacción para el emisor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string IssuerTransactionReference { get; set; }

        /// <summary>
        /// Fecha de la transacción para el emisor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime IssuerTransactionDate { get; set; }

        /// <summary>
        /// Indica si se ha especificado IssuerTransactionDate. 
        /// </summary>
        [XmlIgnore()]
        public bool IssuerTransactionDateSpecified { get; set; }

        /// <summary>
        /// Referencia de contrato para el receptor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ReceiverContractReference { get; set; }

        /// <summary>
        /// Fecha contrato receptor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime ReceiverContractDate { get; set; }

        /// <summary>
        /// Indica si se ha especificado ReceiverContractDate.
        /// </summary>
        [XmlIgnore()]
        public bool ReceiverContractDateSpecified { get; set; }

        /// <summary>
        /// ReceiverTransactionReference
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ReceiverTransactionReference { get; set; }

        /// <summary>
        /// ReceiverTransactionDate
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime ReceiverTransactionDate { get; set; }

        /// <summary>
        /// Si se ha especificado ReceiverTransactionDate.
        /// </summary>
        [XmlIgnore()]
        public bool ReceiverTransactionDateSpecified { get; set; }

        /// <summary>
        /// FileReference
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FileReference { get; set; }

        /// <summary>
        /// Fecha archivo.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime FileDate { get; set; }

        /// <summary>
        /// Fecha de archivo espedificada.
        /// </summary>
        [XmlIgnore()]
        public bool FileDateSpecified { get; set; }

        /// <summary>
        /// SequenceNumber
        /// </summary>
        [XmlIgnore()]
        public decimal SequenceNumber { get; set; }

        /// <summary>
        /// Representación con dos decimales del precio unitario
        /// sin impuestos.
        /// </summary>
        [XmlElement(ElementName = "SequenceNumber", Form = XmlSchemaForm.Unqualified)]
        public string SequenceNumberString
        {

            get
            {

                NumberFormatInfo sequenceFormatInfo = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 1
                };

                return SequenceNumber.ToString("N", sequenceFormatInfo);

            }
            set
            {

                SequenceNumber = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// SequenceNumberSpecified
        /// </summary>
        [XmlIgnore()]
        public bool SequenceNumberSpecified { get; set; }

        /// <summary>
        /// DeliveryNotesReferences
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("DeliveryNote", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public DeliveryNote[] DeliveryNotesReferences { get; set; }

        /// <summary>
        /// ItemDescription
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ItemDescription { get; set; }

        /// <summary>
        /// Cantidad
        /// </summary>
        [XmlIgnore()]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Representación con dos decimales del precio unitario
        /// sin impuestos.
        /// </summary>
        [XmlElement(ElementName = "Quantity", Form = XmlSchemaForm.Unqualified)]
        public string QuantityString
        {

            get
            {

                NumberFormatInfo quantityFormatInfo = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 1
                };

                return Quantity.ToString("N", quantityFormatInfo);

            }
            set
            {

                Quantity = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        /// <summary>
        /// Indica si se ha especificado UnitOfMeasure.
        /// </summary>
        [XmlIgnore()]
        public bool UnitOfMeasureSpecified { get; set; }

        /// <summary>
        /// Precio unitario sin impuestos.
        /// </summary>
        [XmlIgnore()]
        public decimal UnitPriceWithoutTax { get; set; }

        /// <summary>
        /// Representación con dos decimales del precio unitario
        /// sin impuestos.
        /// </summary>
        [XmlElement(ElementName = "UnitPriceWithoutTax", Form = XmlSchemaForm.Unqualified)]
        public string UnitPriceWithoutTaxString
        {

            get
            {

                NumberFormatInfo priceFormatInfo = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 6
                };

                return UnitPriceWithoutTax.ToString("N", priceFormatInfo);
            }
            set
            {

                UnitPriceWithoutTax = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Coste total
        /// </summary>
        [XmlIgnore()]
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Representación con dos decimales del Coste total
        /// sin impuestos.
        /// </summary>
        [XmlElement(ElementName = "TotalCost", Form = XmlSchemaForm.Unqualified)]
        public string TotalCostString
        {
            get
            {
                NumberFormatInfo costFormatInfo = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 6
                };

                return TotalCost.ToString("N", costFormatInfo);

            }
            set
            {

                TotalCost = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Descuentos reducciones
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Discount", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Discount[] DiscountsAndRebates { get; set; }

        /// <summary>
        /// Cargos
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Charge", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Charge[] Charges { get; set; }

        /// <summary>
        /// Importe bruto
        /// </summary>
        [XmlIgnore()]
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// Representación con dos decimales del bruto
        /// sin impuestos.
        /// </summary>
        [XmlElement(ElementName = "GrossAmount", Form = XmlSchemaForm.Unqualified)]
        public string GrossAmountString
        {

            get
            {
                NumberFormatInfo amountFormatInfo = new NumberFormatInfo()
                {
                    NumberDecimalSeparator = ".",
                    NumberGroupSeparator = "",
                    NumberDecimalDigits = 6
                };

                return GrossAmount.ToString("N", amountFormatInfo);

            }
            set
            {

                GrossAmount = XmlParser.ToDecimal(value);

            }

        }

        /// <summary>
        /// Impuestos soportados
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Tax", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Tax[] TaxesOutputs { get; set; }

        /// <summary>
        /// Impuestos retenidos.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Tax", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Tax[] TaxesWithheld { get; set; }

        /// <summary>
        /// Periodo
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PeriodDates LineItemPeriod { get; set; }

        /// <summary>
        /// Fecha transacción
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime TransactionDate { get; set; }

        /// <summary>
        /// Indica si se facilitado TransactionDate
        /// </summary>
        [XmlIgnore()]
        public bool TransactionDateSpecified { get; set; }

        /// <summary>
        /// Información adicional de lá línea.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AdditionalLineItemInformation { get; set; }

        /// <summary>
        /// Código de artículo.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ArticleCode { get; set; }

        /// <summary>
        /// Extensiones.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Extensions Extensions { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{ItemDescription}, {TotalCost}";

        }

        #endregion

    }

}
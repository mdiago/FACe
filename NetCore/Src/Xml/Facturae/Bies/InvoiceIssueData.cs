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
    /// Fecha factura.
    /// </summary>
    [Serializable()]
    public class InvoiceIssueData
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Fecha de expedición. Fecha en la que se genera la factura con efectos fiscales. 
        /// ISO 8601:2004. Esta fecha no podrá ser posterior a la fecha de la firma electrónica.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Válido a partir de la versión 3.1.
        /// Fecha de Operación. Fecha en la que se realiza el servicio o se entrega el bien. 
        /// ISO 8601:2004. Esta fecha solo será obligatoria si es distinta de la fecha de expedición.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime OperationDate { get; set; }

        /// <summary>
        /// Indica si se ha proporcionado OperationDate.
        /// </summary>
        [XmlIgnore()]
        public bool OperationDateSpecified { get; set; }

        /// <summary>
        /// Lugar de expedición. Plaza en la que se expide el documento.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PlaceOfIssue PlaceOfIssue { get; set; }

        /// <summary>
        /// Periodo de facturación. Sólo cuando se requiera: Servicio prestado 
        /// temporalmente o Factura Recapitulativa.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PeriodDates InvoicingPeriod { get; set; }

        /// <summary>
        /// Moneda de la operación. Código ISO 4217:2001 Alpha-3 de la moneda en 
        /// la que se emite la factura. Si la moneda de la operación difiere de
        /// la moneda del impuesto (EURO), los campos del contravalor ExchangeRate y
        /// ExchangeRateDate deberán cumplimentarse, en cumplimiento del Artº 10.1 
        /// del Reglamento sobre facturación. RD 1496/2003 de 28 de Noviembre.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public CurrencyCode InvoiceCurrencyCode { get; set; }

        /// <summary>
        /// Detalles del tipo de cambio.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ExchangeRateDetails ExchangeRateDetails { get; set; }

        /// <summary>
        /// Moneda del Impuesto. Código ISO 4217:2001 Alpha-3 de la moneda en la que 
        /// se liquida el impuesto.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public CurrencyCode TaxCurrencyCode { get; set; }

        /// <summary>
        /// Lengua. Código ISO 639-1:2002 Alpha-2 de la lengua en la que se emite el documento.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public LanguageCode LanguageName { get; set; }

        /// <summary>
        /// Váido a partir de la versión 3.2.2.
        /// Descripción general de la factura.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string InvoiceDescription { get; set; }

        /// <summary>
        /// Váido a partir de la versión 3.2.2.
        /// Referencia de pedido.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ReceiverTransactionReference { get; set; }

        /// <summary>
        /// Váido a partir de la versión 3.2.2.
        /// Código del expediente de contratación.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FileReference { get; set; }

        /// <summary>
        /// Váido a partir de la versión 3.2.2.
        /// Referencia del contrato del receptor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ReceiverContractReference { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de InvoiceIssueDataType.
        /// </summary>
        /// <returns>Representación textual de la instancia de InvoiceIssueDataType.</returns>
        public override string ToString()
        {

            return $"{IssueDate}";

        }

        #endregion

    }

}
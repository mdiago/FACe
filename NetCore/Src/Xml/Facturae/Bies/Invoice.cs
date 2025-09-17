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
    /// Factura versión 3.2.2.
    /// </summary>
    [Serializable()]
    [XmlType(Namespace = FacturaeNamespaces.NamespaceFE)]
    public class Invoice
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Encabezado factura.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public InvoiceHeader InvoiceHeader { get; set; }

        /// <summary>
        /// Fecha emisión factura.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public InvoiceIssueData InvoiceIssueData { get; set; }

        /// <summary>
        /// Impuestos soportados.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Tax", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public TaxOutput[] TaxesOutputs { get; set; }

        /// <summary>
        /// Impuestos retenidos.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Tax", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Tax[] TaxesWithheld { get; set; }

        /// <summary>
        /// Totales factura.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public InvoiceTotals InvoiceTotals { get; set; }

        /// <summary>
        /// Líneas factura.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("InvoiceLine", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public InvoiceLine[] Items { get; set; }

        /// <summary>
        /// Detalles de pago.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Installment", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public Installment[] PaymentDetails { get; set; }

        /// <summary>
        /// Textos legales.
        /// </summary>
        [XmlArray(Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("LegalReference", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public string[] LegalLiterals { get; set; }

        /// <summary>
        /// Datos adicionales.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public AdditionalData AdditionalData { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de InvoiceHeaderType.
        /// </summary>
        /// <returns>Representación textual de la instancia de InvoiceHeaderType.</returns>
        public override string ToString()
        {

            return $"{InvoiceHeader}";

        }

        #endregion

    }

}
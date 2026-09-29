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
    Boston, MA 02110-1301 USA, or download the license from the following URL:
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

using FACe.Xml.Ubl.Cac;
using FACe.Xml.Ubl.Cbc;
using System.Xml.Serialization;

namespace FACe.Xml.Ubl.Invoice
{

    /// <summary>
    /// Factura UBL.
    /// </summary>
    [XmlRoot("Invoice", Namespace = UblNamespaces.NamespaceInvoice)]
    public class Invoice : InvoiceDocument
    {

        #region Propiedades Públicas

        /// <summary>
        /// Identificador de personalización.
        /// </summary>
        [XmlElement("CustomizationID", Namespace = UblNamespaces.NamespaceCBC)]
        public Identifier CustomizationID { get; set; }

        /// <summary>
        /// Identificador de la factura.
        /// </summary>
        [XmlElement("ID", Namespace = UblNamespaces.NamespaceCBC)]
        public Identifier ID { get; set; }

        /// <summary>
        /// Fecha de emisión.
        /// </summary>
        [XmlElement("IssueDate", Namespace = UblNamespaces.NamespaceCBC)]
        public string IssueDate { get; set; }

        /// <summary>
        /// Fecha de vencimiento.
        /// </summary>
        [XmlElement("DueDate", Namespace = UblNamespaces.NamespaceCBC)]
        public string DueDate { get; set; }

        /// <summary>
        /// Código del tipo de factura.
        /// </summary>
        [XmlElement("InvoiceTypeCode", Namespace = UblNamespaces.NamespaceCBC)]
        public Code InvoiceTypeCode { get; set; }

        /// <summary>
        /// Notas de la factura.
        /// </summary>
        [XmlElement("Note", Namespace = UblNamespaces.NamespaceCBC)]
        public Text[] Note { get; set; }

        /// <summary>
        /// Código de moneda de la factura.
        /// </summary>
        [XmlElement("DocumentCurrencyCode", Namespace = UblNamespaces.NamespaceCBC)]
        public Code DocumentCurrencyCode { get; set; }

        /// <summary>
        /// Referencia del comprador.
        /// </summary>
        [XmlElement("BuyerReference", Namespace = UblNamespaces.NamespaceCBC)]
        public Text BuyerReference { get; set; }

        /// <summary>
        /// Periodos de facturación.
        /// </summary>
        [XmlElement("InvoicePeriod", Namespace = UblNamespaces.NamespaceCAC)]
        public InvoicePeriod[] InvoicePeriod { get; set; }

        /// <summary>
        /// Referencia al pedido.
        /// </summary>
        [XmlElement("OrderReference", Namespace = UblNamespaces.NamespaceCAC)]
        public OrderReference OrderReference { get; set; }

        /// <summary>
        /// Referencias a facturas.
        /// </summary>
        [XmlElement("BillingReference", Namespace = UblNamespaces.NamespaceCAC)]
        public BillingReference[] BillingReference { get; set; }

        /// <summary>
        /// Referencias contractuales.
        /// </summary>
        [XmlElement("ContractDocumentReference", Namespace = UblNamespaces.NamespaceCAC)]
        public DocumentReference[] ContractDocumentReference { get; set; }
        /// <summary>
        /// Proveedor.
        /// </summary>
        [XmlElement("AccountingSupplierParty", Namespace = UblNamespaces.NamespaceCAC)]
        public AccountingSupplierParty AccountingSupplierParty { get; set; }

        /// <summary>
        /// Cliente.
        /// </summary>
        [XmlElement("AccountingCustomerParty", Namespace = UblNamespaces.NamespaceCAC)]
        public AccountingCustomerParty AccountingCustomerParty { get; set; }

        /// <summary>
        /// Entregas.
        /// </summary>
        [XmlElement("Delivery", Namespace = UblNamespaces.NamespaceCAC)]
        public Delivery[] Delivery { get; set; }

        /// <summary>
        /// Medios de pago.
        /// </summary>
        [XmlElement("PaymentMeans", Namespace = UblNamespaces.NamespaceCAC)]
        public PaymentMeans[] PaymentMeans { get; set; }

        /// <summary>
        /// Condiciones de pago.
        /// </summary>
        [XmlElement("PaymentTerms", Namespace = UblNamespaces.NamespaceCAC)]
        public PaymentTerms[] PaymentTerms { get; set; }

        /// <summary>
        /// Descuentos y cargos generales.
        /// </summary>
        [XmlElement("AllowanceCharge", Namespace = UblNamespaces.NamespaceCAC)]
        public AllowanceCharge[] AllowanceCharge { get; set; }

        /// <summary>
        /// Totales de impuestos.
        /// </summary>
        [XmlElement("TaxTotal", Namespace = UblNamespaces.NamespaceCAC)]
        public TaxTotal[] TaxTotal { get; set; }

        /// <summary>
        /// Totales monetarios.
        /// </summary>
        [XmlElement("LegalMonetaryTotal", Namespace = UblNamespaces.NamespaceCAC)]
        public LegalMonetaryTotal LegalMonetaryTotal { get; set; }

        /// <summary>
        /// Líneas de factura.
        /// </summary>
        [XmlElement("InvoiceLine", Namespace = UblNamespaces.NamespaceCAC)]
        public InvoiceLine[] InvoiceLine { get; set; }

        #endregion

    }

}
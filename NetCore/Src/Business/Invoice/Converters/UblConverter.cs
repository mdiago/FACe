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

using FACe.Business.Invoice;
using FACe.Xml;
using FACe.Xml.Ubl.Cbc;
using System;
using UblInvoice = FACe.Xml.Ubl.Invoice.Invoice;

namespace FACe.Business.Invoice.Converters
{

    /// <summary>
    /// Conversor de factura a formato UBL.
    /// </summary>
    public class UblConverter : InvoiceConverter<UblInvoice>
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Identificador de personalización CIUS-ES-FACE.
        /// </summary>
        private const string CustomizationID =
            "urn:cen.eu:en16931:2017#compliant#urn:face.gob.es:CIUS-ES-FACE:1.0.0";

        #endregion

        #region Constructores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoice">Objeto Invoice.</param>
        public UblConverter(Invoice invoice) : base(invoice)
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Obtiene un objeto InvoiceDocument a partir del objeto
        /// Invoice contenido.
        /// </summary>
        /// <returns> Objeto InvoiceDocument</returns>
        protected override UblInvoice GetFromInvoice()
        {

            // Modeda por defecto EUR
            if (string.IsNullOrEmpty(Invoice.CurrencyID))
                Invoice.CurrencyID = "EUR";

            var ubl = new UblInvoice()
            {
                CustomizationID = CustomizationID,
                ID = Invoice.InvoiceID,
                IssueDate = XmlParser.GetXmlDate(Invoice.InvoiceDate),
                Note = string.IsNullOrEmpty(Invoice.Text) ? null :
                    new Text[] { Invoice.Text },
                DocumentCurrencyCode = Invoice.CurrencyID
            };

            return ubl;
        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Obtiene un objeto Invoice a partir del objeto
        /// InvoiceDocument contenido.
        /// </summary>
        /// <returns> Objeto Invoice.</returns>
        public override Invoice GetInvoice()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
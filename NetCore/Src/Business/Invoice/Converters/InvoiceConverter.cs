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

namespace FACe.Business.Invoice.Converters
{

    /// <summary>
    /// Encargado de convertir una instancia de Invoice en un objeto
    /// InvoiceDocument determinado y de obtener de un objeto InvoiceDocument
    /// un objeto Invoice con su representación.
    /// </summary>
    /// <typeparam name="T"> Tipo del objeto InvoiceDocument.</typeparam>
    public abstract class InvoiceConverter<T> where T : Xml.InvoiceDocument
    {

        #region Propiedades Privadas de Instancia

        /// <summary>
        /// Objeto Invoice.
        /// </summary>
        protected Invoice Invoice { get; }

        /// <summary>
        /// Objeto InvoiceDocument.
        /// </summary>
        protected T InvoiceDocument { get; }

        #endregion

        #region Propiedades Privadas de Instancia

        /// <summary>
        /// Obtiene un objeto InvoiceDocument  a partir del objeto
        /// Invoice contenido.
        /// </summary>
        /// <returns> Objeto Invoice.</returns>
        protected abstract T GetFromInvoice();

        #endregion

        #region Constructores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoice">Objeto Invoice.</param>
        protected InvoiceConverter(Invoice invoice)
        {

            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            Invoice = invoice;

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoiceDocument">Objeto InvoiceDocument.</param>
        protected InvoiceConverter(T invoiceDocument)
        {

            if (invoiceDocument == null)
                throw new ArgumentNullException(nameof(invoiceDocument));

            InvoiceDocument = invoiceDocument;

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Recupera el interlocutor con el TaxId
        /// pasado como parámetro.
        /// </summary>
        /// <param name="taxId">Identificador fiscal.</param>
        /// <returns>El interlocutor con ese TaxId o null si no existe.</returns>
        protected Party GetPartyByTaxId(string taxId)
        {

            foreach (var current in Invoice.Parties)
                if (current.TaxID == taxId)
                    return current;

            return null;

        }

        /// <summary>
        /// Recupera el interlocutor con el role
        /// pasado como parámetro.
        /// </summary>
        /// <param name="role">Rol del interlocutor en la factura.</param>
        /// <returns>El interlocutor con ese TaxId o null si no existe.</returns>
        protected Party GetPartyByPartyRole(string role)
        {

            foreach (var current in Invoice.Parties)
                if (current.PartyRole == role)
                    return current;

            return null;

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Obtiene un objeto InvoiceDocument a partir del objeto
        /// Invoice contenido.
        /// </summary>
        /// <returns> Objeto InvoiceDocument</returns>
        public virtual T GetDocument() 
        {

            if (InvoiceDocument != null)
                return InvoiceDocument;

            return GetFromInvoice();
        
        }

        /// <summary>
        /// Obtiene un objeto Invoice a partir del objeto
        /// InvoiceDocument contenido.
        /// </summary>
        /// <returns> Objeto Invoice.</returns>
        public abstract Invoice GetInvoice();

        #endregion

    }

}
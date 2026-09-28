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

using FACe.Business.Invoice.Converters;
using FACe.Net.Rest.Json;
using FACe.Net.Rest.Json.Kivu;
using FACe.Xml.Facturae.Bies;
using System;
using System.Collections.Generic;

namespace FACe.Business.Invoice
{

    /// <summary>
    /// Representa un factura en el sistema FACe.
    /// </summary>
    public class Invoice : JsonSerializableKivu
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Suma de las bases imponibles.
        /// </summary>   
        decimal _NetAmount;

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoiceID">Identificador de la factura.</param>
        /// <param name="invoiceDate">Fecha emisión de documento.</param>
        /// <param name="sellerID">Identificador del vendedor.</param>        
        /// <exception cref="ArgumentNullException">Los argumentos invoiceID y sellerID no pueden ser nulos</exception>
        public Invoice(string invoiceID, DateTime invoiceDate, string sellerID) 
        {

            if (invoiceID == null || sellerID == null)
                throw new ArgumentNullException($"Los argumentos invoiceID y sellerID no pueden ser nulos.");

            InvoiceID = invoiceID.Trim(); // La AEAT calcula el Hash sin espacios
            InvoiceDate = invoiceDate;

            var tSellerID = sellerID.Trim(); // La AEAT calcula el Hash sin espacios
            SellerID = tSellerID.ToUpper(); // https://github.com/mdiago/VeriFactu/issues/65

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Calcula los totales de la factura.
        /// </summary>
        internal void CalculateTotals() 
        {

            TotalAmount = TotalTaxOutput = TotalTaxWithheld = TotalTaxOutputSurcharge = _NetAmount = 0;

            if (TaxItems == null || TaxItems.Count == 0)
                return;

            foreach (var taxitem in TaxItems) 
            {

                if (taxitem.TaxClass == "TO")
                {
                    _NetAmount += taxitem.TaxBase;
                    TotalTaxOutput += taxitem.TaxAmount;
                    TotalTaxOutputSurcharge += taxitem.TaxAmountSurcharge;
                }
                else 
                {
                    TotalTaxWithheld += taxitem.TaxAmount;
                }

            }

            TotalAmount = _NetAmount + TotalTaxOutput + TotalTaxOutputSurcharge - TotalTaxWithheld;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Clave del tipo de factura.</para>
        /// </summary>
        public string InvoiceType { get; set; }

        /// <summary>
        ///  Identifica si el tipo de factura rectificativa
        ///  es por sustitución o por diferencia.
        /// </summary>
        public string RectificationType { get; set; }

        /// <summary>
        /// Identificador de la factura.
        /// </summary>
        public string InvoiceID { get; private set; }

        /// <summary>
        /// Fecha emisión de documento.
        /// </summary>        
        public DateTime InvoiceDate { get; private set; }

        /// <summary>
        /// Fecha operación.
        /// </summary>        
        public DateTime? OperationDate { get; set; }

        /// <summary>
        /// Identificador del vendedor.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        public string SellerID { get; private set; }

        /// <summary>
        /// Nombre del vendedor.
        /// </summary>        
        [Json(Name = "CompanyName")]
        public string SellerName { get; set; }

        /// <summary>
        /// Identidicador del comprador.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        [Json(Name = "RelatedPartyID")]
        public string BuyerID { get; set; }

        /// <summary>
        /// Nombre del comprador.
        /// </summary>        
        [Json(Name = "RelatedPartyName")]
        public string BuyerName { get; set; }

        /// <summary>
        /// Código del país del destinatario (a veces también denominado contraparte,
        /// es decir, el cliente) de la operación de la factura expedida.
        /// <para>Alfanumérico (2) (ISO 3166-1 alpha-2 codes) </para>
        /// </summary>        
        [Json(Name = "CountryID")]
        public string BuyerCountryID { get; set; }

        /// <summary>
        /// Clave para establecer el tipo de identificación
        /// en el pais de residencia.
        /// </summary>        
        [Json(Name = "RelatedPartyIDType")]
        public string BuyerIDType { get; set; }

        /// <summary>
        /// Código de moneda de la factura.
        /// </summary>        
        [Json(Name = "DocumentCurrencyID")]
        public string CurrencyID { get; set; }

        /// <summary>
        /// Esta propiedad se utiliza para almacenar un identificador
        /// de sistema externo.
        /// </summary>        
        public string ExternKey { get; set; }

        /// <summary>
        /// Importe total: Total neto + impuestos soportado
        /// - impuestos retenidos.
        /// </summary>        
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        public decimal TotalTaxOutput { get; private set; }

        /// <summary>
        /// Total impuestos soportados recargo.
        /// </summary>        
        public decimal TotalTaxOutputSurcharge { get; private set; }

        /// <summary>
        /// Importe total impuestos retenidos.
        /// </summary>        
        public decimal TotalTaxWithheld { get; set; }

        /// <summary>
        /// Texto del documento.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Líneas de impuestos.
        /// </summary>
        public List<TaxItem> TaxItems { get; set; }

        /// <summary>
        /// Facturas rectificadas.
        /// </summary>
        public List<RectificationItem> RectificationItems { get; set; }

        /// <summary>
        /// Interlocutores de negocio.
        /// </summary>
        public List<Party> Parties { get; set; }

        /// <summary>
        /// Líneas de la factura.
        /// </summary>
        public List<InvoiceLine> InvoiceLines { get; set; }

        /// <summary>
        /// Vencimientos de la factura.
        /// </summary>
        public List<Installment> Installments { get; set; }

        /// <summary>
        /// BaseRectificada para rectificativas por sustitución 'S'.
        /// </summary>        
        public decimal RectificationTaxBase { get; set; }

        /// <summary>
        /// CuotaRectificada para rectificativas por sustitución 'S'.
        /// </summary>        
        public decimal RectificationTaxAmount { get; set; }

        /// <summary>
        /// CuotaRecargoRectificado para rectificativas por sustitución 'S'.
        /// </summary>
        public decimal RectificationTaxAmountSurcharge { get; set; }

        #endregion     

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns> Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{SellerID}-{InvoiceID}-{InvoiceDate:yyyy-MM-dd}";

        }

        #endregion

    }

}

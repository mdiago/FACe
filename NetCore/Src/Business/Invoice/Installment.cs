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

using FACe.Net.Rest.Json;
using System;

namespace FACe.Business.Invoice
{

    /// <summary>
    /// Representa una factura rectificada.
    /// </summary>
    public class Installment : JsonSerializable
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Fechas en las que se deben atender los pagos. ISO 8601:2004.
        /// </summary>        
        public DateTime DueDate { get; set; }

        /// <summary>
        ///Importe a satisfacer en cada plazo. Siempre con dos decimales.
        /// </summary>        
        public decimal Amount { get;  set; }

        /// <summary>
        /// Cada vencimiento/importe podrá tener un medio de pago concreto.
        /// <para>'01': Al contado.</para>
        /// <para>'02': Recibo Domiciliado.</para>
        /// <para>'03': Recibo.</para>
        /// <para>'04': Transferencia.</para>
        /// <para>'05': Letra Aceptada.</para>
        /// <para>'06': Crédito Documentario.</para>
        /// <para>'07': Contrato Adjudicación.</para>
        /// <para>'08': Letra de cambio.</para>
        /// <para>'09': Pagaré a la  Orden.</para>
        /// <para>'10': Pagaré No a la Orden.</para>
        /// <para>'11': Cheque.</para>
        /// <para>'12': Reposición.</para>
        /// <para>'13': Especiales.</para>
        /// <para>'14': Compensación.</para>
        /// <para>'15': Giro postal.</para>
        /// <para>'16': Cheque conformado.</para>
        /// <para>'17': Cheque bancario.</para>
        /// <para>'18': Pago contra reembolso.</para>
        /// <para>'19': Pago mediante tarjeta.</para>
        /// </summary>
        public string PaymentMeans { get; set; }

        /// <summary>
        /// Tipo de identificador de cuenta.
        /// <para>'IBAN': IBAN</para>
        /// </summary>
        public string BankAccountType { get; set; }

        /// <summary>
        /// Identificador de la cuenta bancaria. En versión Facturae 3.0
        /// sólo es válido el valor IBAN.
        /// </summary>
        public string BankAccount { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{PaymentMeans}, {DueDate}, {Amount}";

        }

        #endregion

    }

}
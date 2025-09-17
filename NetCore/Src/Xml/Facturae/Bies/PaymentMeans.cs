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

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Formas de pago.
    /// </summary>
    [Serializable()]
    public enum PaymentMeans
    {

        /// <summary>
        /// Al contado
        /// </summary>
        [XmlEnum("01")]
        InCash = 1,

        /// <summary>
        /// Recibo Domiciliado
        /// </summary>
        [XmlEnum("02")]
        DirectDebit,

        /// <summary>
        /// Recibo
        /// </summary>
        [XmlEnum("03")]
        Receipt,

        /// <summary>
        /// Transferencia
        /// </summary>
        [XmlEnum("04")]
        CreditTransfer,

        /// <summary>
        /// Letra Aceptada
        /// </summary>
        [XmlEnum("05")]
        AcceptedBillOfExchange,

        /// <summary>
        /// Crédito Documentario
        /// </summary>
        [XmlEnum("06")]
        DocumentaryCredit,

        /// <summary>
        /// Contrato Adjudicación
        /// </summary>
        [XmlEnum("07")]
        ContractAward,

        /// <summary>
        /// Letra de cambio
        /// </summary>
        [XmlEnum("08")]
        BillOfExchange,

        /// <summary>
        /// Pagaré a la  Orden
        /// </summary>
        [XmlEnum("09")]
        TransferablePromissoryNote,

        /// <summary>
        /// Pagaré No a la Orden
        /// </summary>
        [XmlEnum("10")]
        NonTransferablePromissoryNote,

        /// <summary>
        /// Cheque
        /// </summary>
        [XmlEnum("11")]
        Cheque,

        /// <summary>
        /// Reposición
        /// </summary>
        [XmlEnum("12")]
        OpenAccountReimbursement,

        /// <summary>
        /// Especiales
        /// </summary>
        [XmlEnum("13")]
        SpecialPayment,

        /// <summary>
        /// Compensación
        /// </summary>
        [XmlEnum("14")]
        SetOffByReciprocalCredits,

        /// <summary>
        /// Giro postal
        /// </summary>
        [XmlEnum("15")]
        PaymentByPostgiro,

        /// <summary>
        /// Cheque conformado
        /// </summary>
        [XmlEnum("16")]
        CertifiedCheque,

        /// <summary>
        /// Cheque bancario
        /// </summary>
        [XmlEnum("17")]
        BankersDraft,

        /// <summary>
        /// Pago contra reembolso
        /// </summary>
        [XmlEnum("18")]
        CashOnDelivery,

        /// <summary>
        /// Pago mediante tarjeta
        /// </summary>
        [XmlEnum("19")]
        PaymentByCard

    }

}
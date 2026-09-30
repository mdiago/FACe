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

namespace FACe.Business.Invoice
{

    /// <summary>
    /// Clase estática que contiene los tipos de factura soportados por FACe.
    /// </summary>
    public static class InvoiceType
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Factura completa u ordinaria, Original Ordinaria.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass OO.</para>
        /// <para>UBL: InvoiceTypeCode 380.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_OO = "FC.OO";

        /// <summary>
        /// Factura completa u ordinaria, Original Rectificativa.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass OR.</para>
        /// <para>UBL: InvoiceTypeCode 384.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_OR = "FC.OR";

        /// <summary>
        /// Factura completa u ordinaria, Original Recapitulativa.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass OC.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_OC = "FC.OC";

        /// <summary>
        /// Factura completa u ordinaria, Copia Ordinaria.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass CO.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_CO = "FC.CO";

        /// <summary>
        /// Factura completa u ordinaria, Copia Rectificativa.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass CR.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_CR = "FC.CR";

        /// <summary>
        /// Factura completa u ordinaria, Copia Recapitulativa.
        /// <para>Facturae: InvoiceDocumentType FC, InvoiceClass CC.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FC_CC = "FC.CC";

        /// <summary>
        /// Factura simplificada, Original Ordinaria.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass OO.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_OO = "FA.OO";

        /// <summary>
        /// Factura simplificada, Original Rectificativa.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass OR.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_OR = "FA.OR";

        /// <summary>
        /// Factura simplificada, Original Recapitulativa.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass OC.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_OC = "FA.OC";

        /// <summary>
        /// Factura simplificada, Copia Ordinaria.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass CO.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_CO = "FA.CO";

        /// <summary>
        /// Factura simplificada, Copia Rectificativa.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass CR.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_CR = "FA.CR";

        /// <summary>
        /// Factura simplificada, Copia Recapitulativa.
        /// <para>Facturae: InvoiceDocumentType FA, InvoiceClass CC.</para>
        /// <para>UBL: Pendiente de determinar.</para>
        /// <para>CII: Pendiente de determinar.</para>
        /// </summary>
        public const string FA_CC = "FA.CC";

        #endregion

    }

}
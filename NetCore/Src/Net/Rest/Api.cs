﻿/*
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

namespace FACe.Net.Rest
{

    /// <summary>
    /// Representa el API REST de Irene Solutions para Verifactu.
    /// </summary>
    public class Api
    {

        #region Propiedades Privadas Estáticas


        /// <summary>
        /// Endpoint ct.
        /// </summary>
        internal static readonly string EndPointCt = "https://facturae.irenesolutions.com:8050/Kivu/Taxes/Verifactu/Ct/Create/Test/v17";

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        public Api()
        {
        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Endpoint creación de factruas.
        /// </summary>
        public string EndPointCreate { get; set; }

        /// <summary>
        /// Endpoint anulación de factruas.
        /// </summary>
        public string EndPointCancel { get; set; }

        /// <summary>
        /// Endpoint generación código QR.
        /// </summary>
        public string EndPointGetQrCode { get; set; }

        /// <summary>
        /// Endpoint consulta emisores.
        /// </summary>
        public string EndPointGetSellers { get; set; }

        /// <summary>
        /// Endpoint consulta registros envíados.
        /// </summary>
        public string EndPointGetRecords { get; set; }

        /// <summary>
        /// Endpoint validación NIF.
        /// </summary>
        public string EndPointValidateNIF { get; set; }

        /// <summary>
        /// Endpoint consulta registros en AEAT.
        /// </summary>
        public string EndPointGetAeatInvoices { get; set; }

        /// <summary>
        /// Endpoint consulta envíos realizados.
        /// </summary>
        public string EndPointGetFilteredList { get; set; }

        /// <summary>
        /// Endpoint envío de lotes.
        /// </summary>
        public string EndPointCreateBatch { get; set; }

        /// <summary>
        /// Endpoint validación NIF por lotes.
        /// </summary>
        public string EndPointValidateNIFs { get; set; }

        /// <summary>
        /// Endpoint validación número IVA intracomunitario.
        /// </summary>
        public string EndPointValidateViesVatNumber { get; set; }

        /// <summary>
        /// Clave de acceso al API REST para Verifactu de
        /// Irene Solutions. Puede conseguir su clave en
        /// https://facturae.irenesolutions.com/verifactu/go
        /// </summary>
        public string ServiceKey { get; set; }

        #endregion

    }

}
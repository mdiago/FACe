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
    develop commercial activities involving the VeriFactu software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using System;
using System.Runtime.InteropServices;

namespace FACe
{

    #region Interfaz COM

    /// <summary>
    /// Interfaz COM para la clase RectificationItem.
    /// </summary>
    [Guid("CB8E049B-4FCE-4CA8-B0BE-1F6CE4264A30")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFeInvoiceResult
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Código del resultado de la operación. '0' si todo
        /// ha ido vien.
        /// </summary>
        string ResultCode { get; set; }

        /// <summary>
        /// Mensaje del resultado de la petición. 'OK' si todo ha ido
        /// bien.
        /// </summary>
        string ResultMessage { get; set; }

        /// <summary>
        /// Código seguro de verificación devuelto por la AEAT
        /// si todo ha ido bien.
        /// </summary>
        string CSV { get; set; }

        #endregion

    }

    #endregion

    #region Clase COM

    /// <summary>
    /// Resultado de un envio de alta o anulación a la AEAT.
    /// </summary>
    [Guid("F16243F8-62B7-4862-8088-A746A988DE3E")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("FACe.FeInvoiceResult")]
    public class FeInvoiceResult : IFeInvoiceResult
    {

        #region Construtores de Instancia

        /// <summary>
        /// Constructor. Para COM necesitamos un constructor
        /// sin parametros.
        /// </summary>
        public FeInvoiceResult()
        {
        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Código del resultado de la operación. '0' si todo
        /// ha ido vien.
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// Mensaje del resultado de la petición. 'OK' si todo ha ido
        /// bien.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Código seguro de verificación devuelto por la AEAT
        /// si todo ha ido bien.
        /// </summary>
        public string CSV { get; set; }

        #endregion

    }

    #endregion

}
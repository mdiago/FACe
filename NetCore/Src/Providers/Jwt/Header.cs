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

using FACe.Net.Rest.Json;

namespace FACe.Providers.Jwt
{

    /// <summary>
    /// Header: Un JSON que contiene información sobre
    /// el tipo de token (JWT), el algoritmo de firma
    /// utilizado y otros metadatos. 
    /// </summary>
    public class Header : JsonSerializable
    {

        #region Construtores de Instancia

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="typ">Tipo de token</param>
        /// <param name="alg">Algoritmo de firma utilizado.</param>
        public Header(string typ = "JWT", string alg = "RS256")
        {

            Typ = typ;
            Alg = alg;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Tipo de token.
        /// </summary>
        [Json(Name = "typ")]
        public string Typ { get; set; }

        /// <summary>
        /// Algoritmo de firma utilizado.
        /// </summary>
        [Json(Name = "alg")]
        public string Alg { get; set; }

        /// <summary>
        /// Certificado en PEM limpio (sin
        /// espacios ni saltos de línea, ni
        /// los tags de
        /// -----BEGIN/END CERTIFICATE----- 
        /// </summary>
        [Json(Name = "x5c")]
        public string[] ClearedPem { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{Typ}, {Alg}";

        }

        #endregion

    }

}
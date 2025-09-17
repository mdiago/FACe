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
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

namespace FACe.Net.Rest.Json.Parser.Lexer.Tokens
{

    /// <summary>
    /// Representa un fragmento de texto resultado
    /// del análisis léxico de la cadena JSON.
    /// </summary>
    internal class JsonToken
    {

        #region Propiedades Privadas de Instacia

        /// <summary>
        /// Analizador léxico al cual pertenece
        /// el fragmento de texto obtenido.
        /// </summary>
        internal JsonLexer JsonLexer { get; private set; }

        /// <summary>
        /// Posición del inicio del
        /// fragmento de texto dentro de la cadena completa JSON.
        /// </summary>
        internal int Start { get; private set; }

        /// <summary>
        /// Longitud de la cadena de texto.
        /// </summary>
        internal virtual int Length => 1;

        /// <summary>
        /// Valor de la cadena de texto.
        /// </summary>
        internal virtual string Value
        {

            get
            {

                return $"{JsonLexer.JsonText[Start]}";

            }

        }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="jsonLexer">Analizador léxico al que
        /// pertenece el fragmento de texto.</param>
        /// <param name="start">Posición del inicio del
        /// fragmento de texto dentro de la cadena completa JSON.</param>
        internal JsonToken(JsonLexer jsonLexer, int start)
        {

            JsonLexer = jsonLexer;
            Start = start;

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Convierte el valor del fragmento de texto
        /// en el tipo al que se interpreta que pertenece.
        /// </summary>
        /// <returns>Valor del fragmento de texto
        /// en el tipo al que se interpreta que pertenece.</returns>
        internal virtual object Covert()
        {

            return Value;

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {
            return $"{Value}";
        }

        #endregion

    }

}
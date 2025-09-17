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

using FACe.Net.Rest.Json.Serializer;
using System.Collections;
using System.Reflection;
using System.Text;

namespace FACe.Net.Rest.Json
{

    /// <summary>
    /// Clase serializable en JSON.
    /// </summary>
    public class JsonSerializable
    {

        #region Métodos Privados de Instancia

        /// <summary>
        /// Añade fragmento json al StringBuilder de 
        /// destino.
        /// </summary>
        /// <param name="stringBuilder">StringBuilder de 
        /// destino.</param>
        /// <param name="keyValueText">Fragmento json clave-valor.</param>
        private void AppendTokenJson(StringBuilder stringBuilder, string keyValueText)
        {

            string json = stringBuilder.Length == 0 ? "" : ",";
            json += keyValueText;
            stringBuilder.Append(json);

        }

        /// <summary>
        /// Añade fragmento json al StringBuilder de 
        /// destino.
        /// </summary>
        /// <param name="stringBuilder">StringBuilder de 
        /// destino.</param>
        /// <param name="pInf">Info de propiedad que contiene el valor
        /// a serializar.</param>
        /// <param name="value">Valor a serializar.</param>
        private void AppendJson(StringBuilder stringBuilder, PropertyInfo pInf, object value)
        {

            if (typeof(JsonSerializable).IsAssignableFrom(pInf.PropertyType))
            {

                AppendTokenJson(stringBuilder, $"\"{pInf.Name}\"={{{this}}}");

            }
            else
            {

                var jsonSerializer = new JsonSerializer(pInf, value);
                var keyValueJson = jsonSerializer.ToJson();

                if (keyValueJson != null && !(jsonSerializer.Attribute != null && jsonSerializer.Attribute.JsonIgnore))
                    AppendTokenJson(stringBuilder, keyValueJson);

            }

        }

        #endregion      

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación de la clase en formato
        /// JSON.
        /// </summary>
        /// <returns>Representación de la clase en formato
        /// JSON.</returns>
        public string ToJson()
        {

            var stringBuilder = new StringBuilder();

            foreach (var pInf in GetType().GetProperties())
            {

                var value = pInf.GetValue(this);

                if (value == null)
                    continue;

                var iList = value as IList;

                if (iList == null || pInf.PropertyType == typeof(byte[]))
                {

                    AppendJson(stringBuilder, pInf, value);
                    continue;

                }
                else
                {                   
                    
                    var jsonAtt = pInf.GetCustomAttribute(typeof(JsonAttribute)) as JsonAttribute;
                    var key = string.IsNullOrEmpty(jsonAtt.Name) ? pInf.Name : jsonAtt.Name;

                    string[] jsons = new string[iList.Count];

                    for (int i = 0; i < iList.Count; i++)
                    {

                        var item = iList[i];

                        if (typeof(JsonSerializable).IsAssignableFrom(item.GetType()))
                            jsons[i] = (item as JsonSerializable)?.ToJson();
                        else
                            jsons[i] = new JsonSerializer("", item).ToJson();

                    }

                    AppendTokenJson(stringBuilder, $"\"{key}\":[{string.Join(",", jsons)}]");

                }

            }

            return $"{{{stringBuilder}}}";

        }

        #endregion

    }

}
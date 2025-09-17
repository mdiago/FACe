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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace FACe.Xml.Xades
{

    /// <summary>
    /// Utilidades para adaptar la representación de X509IssuerName
    /// a un estilo RFC 2253 compatible con muchos firmadores XAdES.
    /// Implementa:
    ///   OID.2.5.4.97=VATES-...  ->  2.5.4.97=#0c..(DER UTF8String)
    ///   (opcional) S=VALENCIA   ->  ST=VALENCIA
    /// </summary>
    public static class Rfc2253IssuerFormatter
    {

        #region Propiedades Privadas Estáticas

        /// <summary>
        /// Regex básica para detectar "OID.2.5.4.97=valor"
        /// Nota: esto no maneja DN con comas escapadas en el valor; si se necesitase,
        /// habría que parsear el DN con un analizador completo de RFC 2253.
        /// </summary>
        private static readonly Regex OrgIdRegex =
            new Regex(@"OID\.2\.5\.4\.97\s*=\s*([^,]+)", RegexOptions.Compiled);

        #endregion

        #region Métodos Privados Estáticos

        /// <summary>
        /// Codifica la longitud en DER (forma corta/long-form según valor).
        /// </summary>
        private static byte[] EncodeDerLength(int len)
        {

            if (len < 0) 
                throw new ArgumentOutOfRangeException(nameof(len));

            if (len <= 127)
                return new[] { (byte)len };

            // Long-form: 0x80 | num_octetos + bytes big-endian de la longitud
            byte[] tmp = BitConverter.GetBytes(len); // little-endian
            Array.Reverse(tmp); // a big-endian
            int i = 0;

            while (i < tmp.Length && tmp[i] == 0) i++; // elimina ceros a la izquierda
            int n = tmp.Length - i;

            byte[] result = new byte[1 + n];
            result[0] = (byte)(0x80 | n);
            Buffer.BlockCopy(tmp, i, result, 1, n);

            return result;

        }

        /// <summary>
        /// Representa un array de bytes en hexadecimal
        /// con caracteres en minusculas.
        /// </summary>
        /// <param name="data">Matriz de bytes a representar.</param>
        /// <returns>Texto representando el array de bytes
        /// en formato hexadecimal con minusculas.</returns>
        private static string BytesToHexLower(byte[] data)
        {

            if (data == null || data.Length == 0)
                return string.Empty;

            var sb = new StringBuilder(data.Length * 2);

            for (int i = 0; i < data.Length; i++)
                sb.Append(data[i].ToString("x2"));

            return sb.ToString();

        }

        /// <summary>
        /// Quita espacios redundantes alrededor de comas: ",  " -> ", "
        /// y asegura un único espacio tras coma para legibilidad estable.
        /// No afecta al valor semántico del DN.
        /// </summary>
        /// <param name="dn">DN a normalizar.</param>
        /// <returns>DN normalizado.</returns>
        private static string NormalizeCommaSpaces(string dn)
        {

            // Quita espacios redundantes alrededor de comas: ",  " -> ", "
            // y asegura un único espacio tras coma para legibilidad estable.
            // No afecta al valor semántico del DN.
            string s = dn;
            s = Regex.Replace(s, @"\s*,\s*", ", ");
            s = Regex.Replace(s, @"\s{2,}", " ");

            return s.Trim();

        }

        #endregion

        #region Métodos Públicos Estáticos

        /// <summary>
        /// Devuelve el Issuer del certificado en formato .NET y lo transforma
        /// a la forma RFC 2253-like:
        ///   OID.2.5.4.97=VALUE -> 2.5.4.97=#(DER UTF8String)
        ///   (opcional) S= -> ST=
        /// </summary>
        /// <param name="cert">Certificado X509</param>
        /// <param name="replaceSwithST">Si true, reemplaza S= por ST=</param>
        public static string FromCertificate(X509Certificate2 cert, bool replaceSwithST = true)
        {

            if (cert == null) 
                throw new ArgumentNullException(nameof(cert));

            return CanonIssuerRfc2253Like(cert.IssuerName?.Name ?? string.Empty, replaceSwithST);

        }

        /// <summary>
        /// Transforma una cadena Issuer .NET:
        ///   - "OID.2.5.4.97=..." -> "2.5.4.97=#0c..(DER)"
        ///   - opcionalmente "S=" -> "ST="
        /// </summary>
        /// <param name="issuerDotNet">Issuer tal como lo da .NET (X509Certificate2.IssuerName.Name)</param>
        /// <param name="replaceSwithST">Si true, reemplaza S= por ST=</param>
        public static string CanonIssuerRfc2253Like(string issuerDotNet, bool replaceSwithST = true)
        {
            
            if (issuerDotNet == null) 
                issuerDotNet = string.Empty;

            // Normaliza espacios alrededor de comas
            // (evita resultados con dobles espacios al reemplazar)
            var issuer = NormalizeCommaSpaces(issuerDotNet);

            // Reemplaza OID.2.5.4.97=VAL -> 2.5.4.97=#0c..hex
            issuer = OrgIdRegex.Replace(issuer, m =>
            {
                var val = (m.Groups[1].Value ?? string.Empty).Trim();
                string derHex = ToDerHexUtf8(val);
                return "2.5.4.97=#" + derHex;
            });

            // Reemplaza "S=" por "ST=" solo cuando es un atributo (inicio o tras coma)
            if (replaceSwithST)
                issuer = Regex.Replace(issuer, @"(^|,\s*)S=", "$1ST=");

            return issuer;

        }

        /// <summary>
        /// Codifica un string como DER UTF8String:
        ///  Tag(0x0C) + Length + UTF8Bytes, y lo devuelve en hex minúscula.
        /// </summary>
        public static string ToDerHexUtf8(string s)
        {

            if (s == null) 
                s = string.Empty;

            byte[] utf8 = Encoding.UTF8.GetBytes(s);
            byte[] lenBytes = EncodeDerLength(utf8.Length);

            byte[] der = new byte[1 + lenBytes.Length + utf8.Length];
            der[0] = 0x0C; // UTF8String
            Buffer.BlockCopy(lenBytes, 0, der, 1, lenBytes.Length);
            Buffer.BlockCopy(utf8, 0, der, 1 + lenBytes.Length, utf8.Length);

            return BytesToHexLower(der);

        }

        #endregion

    }

}
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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FACe.Providers.Jwt
{

    /// <summary>
    /// Genera tokens JWT según las especificaciones
    /// para la comunicación con el API REST de FACe.
    /// </summary>
    public class JwtBuilder
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Certificado para la firma.
        /// </summary>
        X509Certificate2 _Certificate;

        /// <summary>
        /// PEM del certificado.
        /// </summary>
        string _CertPem;

        /// <summary>
        /// Minutos de validadez del token.
        /// </summary>
        int _TokenLifeMinutes = 5;

        #endregion

        #region Propiedades Privadas de Instacia

        /// <summary>
        /// Fecha hora creación.
        /// </summary>
        internal DateTime DateCreated { get; private set; }

        /// <summary>
        /// Fecha hora expiración.
        /// </summary>
        internal DateTime DateExpires { get; private set; }

        /// <summary>
        /// Indica si el token ha caducado.
        /// </summary>
        internal bool IsExpired => DateTime.UtcNow.CompareTo(DateExpires) < 0;

        /// <summary>
        /// Momento de creación.
        /// </summary>
        internal int Created { get; private set; }

        /// <summary>
        /// Momento en el que caduca.
        /// </summary>
        internal int Expires {  get; private set; }

        /// <summary>
        /// Segundos de vida restantes del token.
        /// </summary>
        internal int LifeSeconds => Expires - GetUnixTime(DateTime.UtcNow);

        /// <summary>
        /// Token JWT.
        /// </summary>
        internal string Token { get; private set; }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="certificate">Certificado de integrador
        /// dado de alta en FACe.</param>
        public JwtBuilder(X509Certificate2 certificate)
        {

            Header = new Header();
            Payload = new Payload();
            _Certificate = certificate;
            _CertPem = GetCertPem(_Certificate);

        }

        #endregion

        #region Métodos Privados Estáticos

        /// <summary>
        /// Convierte facha a formato de
        /// tiempo Unix.
        /// </summary>
        /// <param name="date"> Fecha.</param>
        /// <returns> Formato de tiempo Unix</returns>
        public static int GetUnixTime(DateTime date)
        {

            return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Obtiene el Jose Header.
        /// </summary>
        /// <returns>Jose Header del JWT.</returns>
        private Header GetHeader()
        {

            return new Header()
            {
                ClearedPem = new string[] { _CertPem }
            };

        }

        /// <summary>
        /// Devuelve el PayLoad del JWT.
        /// </summary>
        /// <param name="certPem">PEM limpio.</param>
        /// <returns>PayLoad.</returns>
        private Payload GetPayload(string certPem)
        {

            DateCreated = DateTime.UtcNow;
            DateExpires = DateCreated.AddMinutes(_TokenLifeMinutes);

            Created = GetUnixTime(DateCreated);
            Expires = GetUnixTime(DateExpires);

            return new Payload()
            {
                UserName = GetCertHash(certPem),
                Iat = Created,       
                Exp = Expires                     
            };

        }

        /// <summary>
        /// Devuelve el texto en el array de bytes codificado
        /// en Base64 Url.
        /// </summary>
        /// <param name="bytes"> Texto a codificar.</param>
        /// <returns> Texto codificado en Base64 Url.</returns>
        private string GetB64Url(byte[] bytes)
        {

            var text = Convert.ToBase64String(bytes);
            return GetB64Url(text);

        }

        /// <summary>
        /// Devuelve el texto codificado
        /// en Base64 Url.
        /// </summary>
        /// <param name="text"> Texto a codificar.</param>
        /// <returns> Texto codificado en Base64 Url.</returns>
        private string GetB64Url(string text)
        {

            return text.TrimEnd('=').Replace('+', '-').Replace('/', '_');

        }

        /// <summary>
        /// Calcula la firma.
        /// </summary>
        /// <param name="encHeader"> Encabezado codificaco.</param>
        /// <param name="encPayload"> Payload codificado.</param>
        /// <param name="certificate"> Certificado para la firma.</param>
        /// <returns> Firma computada.</returns>
        private string ComputeSignature(string encHeader, string encPayload, X509Certificate2 certificate)
        {

            string signingInput = encHeader + "." + encPayload;

            using (var rsa = certificate.GetRSAPrivateKey())
            {

                byte[] sig = rsa.SignData(
                    Encoding.ASCII.GetBytes(signingInput),
                    HashAlgorithmName.SHA256,
                    RSASignaturePadding.Pkcs1
                );

                return GetB64Url(sig);

            }

        }

        /// <summary>
        /// Devuelve la parte publica del certificado
        /// como PEM limpio según las especificaciones.
        /// </summary>
        /// <param name="certificate">Certificado de integrador
        /// registrado en FACe</param>
        /// <returns>
        /// PEM, o parte publica, del certificado con
        /// el que se va a firmar en base 64, en una línea,
        /// sin espacios ni saltos de línea, ni los tags de
        /// -----BEGIN/END CERTIFICATE-----.
        /// Todo ello, codificado a base 64
        /// </returns>
        private string GetCertPem(X509Certificate2 certificate)
        {

            // 1) Exportar el certificado público a DER (binario)
            byte[] certDer = certificate.Export(X509ContentType.Cert);

            // 2) Convertir a Base64 (línea continua, sin BEGIN/END, sin saltos)
            return Convert.ToBase64String(certDer);

        }

        /// <summary>
        /// Devuelve el hash SHA1 del PEM limpio.
        /// </summary>
        /// <param name="certPem"> PEM limpio.</param>
        /// <returns> Hash SHA1 del PEM limpio.</returns>
        private string GetCertHash(string certPem)
        {

            var buffPem = Encoding.UTF8.GetBytes(certPem);
            var hashPem = new SHA1Managed().ComputeHash(buffPem);

            return BitConverter.ToString(hashPem).Replace("-", "").ToLowerInvariant();

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// JoseHeader del JWT. Contiene información
        /// sobre el tipo de token, el algoritmo de 
        /// firma y metadatos.
        /// </summary>
        public Header Header { get; private set; }

        /// <summary>
        /// Contiene los pares clave-valor a
        /// transmitir.
        /// </summary>
        public Payload Payload { get; private set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Devuelve token JWT.
        /// </summary>
        /// <returns> Token JWT.</returns>
        public string GetToken()
        {

            var header = GetHeader();
            var payLoad = GetPayload(_CertPem);

            var headerJson = header.ToJson();
            var payloadJson = payLoad.ToJson();

            var encHeader = GetB64Url(Encoding.UTF8.GetBytes(headerJson));
            var encPayload = GetB64Url(Encoding.UTF8.GetBytes(payloadJson));
            var signature = ComputeSignature(encHeader, encPayload, _Certificate);

            Token = $"{encHeader}.{encPayload}.{signature}";

            return Token;

        }

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{Header}, {Payload}";

        }

        #endregion

    }

}
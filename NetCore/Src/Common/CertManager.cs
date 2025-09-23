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
    develop commercial activities involving the Facturae software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using FACe.Config;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FACe.Common
{

    /// <summary>
    /// Gestor de certificados
    /// </summary>
    public static class CertManager
    {

        #region Métodos Públicos Estáticos

        /// <summary>
        /// Devuelve el certificado configurado siguiendo la siguiente
        /// jerarquía de llamadas: En primer lugar prueba a cargar el
        /// certificado desde un archivo, si no prueba por el hash y en
        /// último lugar prueba por el número de serie.
        /// </summary>
        /// <returns>Devuelve el certificado de la 
        /// configuración para las comunicaciones.</returns>
        public static X509Certificate2 GetCertificateFromSettings()
        {

            if(!string.IsNullOrEmpty(Settings.Current.CertificatePath) && !string.IsNullOrEmpty(Settings.Current.CertificatePassword))
                return GetCertificateByFile(Settings.Current.CertificatePath, Settings.Current.CertificatePassword); // 1. Valor en Settings por fichero.

            if (!string.IsNullOrEmpty(Settings.Current.CertificateThumbprint))
                return GetCertificateByThumbprint(Settings.Current.CertificateThumbprint); // 2. Valor en Settings por huella digital (Almacén Certificados Windows).

            if (!string.IsNullOrEmpty(Settings.Current.CertificateSerial))
                return GetCertificateBySerial(Settings.Current.CertificateSerial); // 3. Valor en Settings por número de serie (Almacén Certificados Windows).

            return null;
        }

        /// <summary>
        /// Busca un certificado en el almacén de windows
        /// por número de serie.
        /// </summary>
        /// <param name="serial">Número de serie del certificado</param>
        /// <returns>
        /// Devuelve el certificado por número de serie.
        /// Si no existe devuelve null.
        /// </returns>

        public static X509Certificate2 GetCertificateBySerial(string serial)
        {

            if (string.IsNullOrEmpty(serial))
                throw new ArgumentException("El parámetro serial no puedes ser nulo o vacío.");

            foreach (var store in new X509Store[] { new X509Store(), new X509Store(StoreLocation.LocalMachine) })
            {

                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 cert in store.Certificates)
                    if (cert.SerialNumber.Equals(serial, StringComparison.OrdinalIgnoreCase))
                        return cert;

            }

            return null;

        }

        /// <summary>
        /// Busca un certificado en el almacén de windows
        /// por la huella o hash.
        /// </summary>
        /// <param name="thumprint">Huella o hash del certificado</param>
        /// <returns>Devuelve el certificado por hash.
        /// Si no existe devuelve null.</returns>
        public static X509Certificate2 GetCertificateByThumbprint(string thumprint)
        {

            if (string.IsNullOrEmpty(thumprint))
                throw new ArgumentException("El parámetro thumprint no puedes ser nulo o vacío.");

            foreach (var store in new X509Store[] { new X509Store(), new X509Store(StoreLocation.LocalMachine) })
            {

                store.Open(OpenFlags.ReadOnly);

                foreach (X509Certificate2 cert in store.Certificates)
                    if (cert.Thumbprint.Equals(thumprint, StringComparison.OrdinalIgnoreCase))
                        return cert;

            }

            return null;

        }

        /// <summary>
        /// Devuelve un certificado desde un archivo de certificado
        /// .pfx o .p12.
        /// </summary>
        /// <param name="path">Ruta del archivo de certificado.</param>
        /// <param name="pass">Contraseña del certificado.</param> 
        /// <returns>Devuelve el certificado.</returns>
        public static X509Certificate2 GetCertificateByFile(string path, string pass)
        {

            if(string.IsNullOrEmpty(path))
                throw new ArgumentException("El parámetro path no puedes ser nulo o vacío.");

            if (string.IsNullOrEmpty(pass))
                throw new ArgumentException("El parámetro pass no puedes ser nulo o vacío.");

            if(!File.Exists(path))
                throw new ArgumentException($"El archivo {path} no existe.");

            return new X509Certificate2(path, pass, X509KeyStorageFlags.Exportable);

        }

        /// <summary>
        /// Verifica que el certificado está configurado correctamente
        /// y no tiene problemas.
        /// </summary>
        internal static void CheckCertificate(X509Certificate2 certificate)
        {
  

            if (DateTime.Now.CompareTo(certificate.NotAfter) > 0)
                throw new InvalidOperationException($"El certificado está caducado desde {certificate.NotAfter}");

            if (certificate.PrivateKey == null)
                throw new InvalidOperationException($"El certificado no tiene clave privada.");

            try
            {

                var exportedKeyMaterial = certificate.PrivateKey.ToXmlString(true);

            }
            catch (CryptographicException crypEx)
            {

                throw new InvalidOperationException("Se ha producido un error al intentar obtener la clave privada.", crypEx);

            }
            catch (Exception ex) 
            {

                throw new InvalidOperationException("Se ha producido un error al intentar obtener la clave privada.", ex);

            }            

        }

        #endregion

    }

}
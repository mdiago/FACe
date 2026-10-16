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
    Boston, MA 02110-1301 USA, or download the license from the following URL:
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

using FACe.Xml.Xades;
using FACe.Xml.Xades.Signature.Xades.Props.BigInt;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace FACe.Xml.Ubl.Signature
{

    /// <summary>
    /// Firma electrónica XAdES de documentos UBL.
    /// </summary>
    public class UblSigned
    {

        #region Constantes

        /// <summary>
        /// Espacio de nombres XML para firmas digitales XMLDSig.
        /// </summary>
        private const string NamespaceDS = "http://www.w3.org/2000/09/xmldsig#";

        /// <summary>
        /// Espacio de nombres XML para las propiedades de firma XAdES.
        /// </summary>
        private const string NamespaceXADES = "http://uri.etsi.org/01903/v1.3.2#";

        /// <summary>
        /// Identificador del tipo de referencia a las propiedades firmadas XAdES.
        /// </summary>
        private const string SignedPropertiesType = "http://uri.etsi.org/01903#SignedProperties";

        /// <summary>
        /// Identificador del algoritmo de resumen criptográfico SHA-512.
        /// </summary>
        private const string SHA512Algorithm = "http://www.w3.org/2001/04/xmlenc#sha512";

        #endregion

        #region Métodos Públicos Estáticos

        /// <summary>
        /// Firma electrónicamente un documento UBL mediante XAdES.
        /// </summary>
        /// <param name="xml">Documento UBL sin firmar.</param>
        /// <param name="certificate">Certificado con clave privada.</param>
        /// <returns>Documento UBL firmado.</returns>
        public static string Sign(string xml, X509Certificate2 certificate)
        {

            if (string.IsNullOrWhiteSpace(xml))
                throw new ArgumentException(
                    "El documento XML está vacío.",
                    nameof(xml));

            if (certificate == null || !certificate.HasPrivateKey)
                throw new ArgumentException(
                    "El certificado debe contener una clave privada.",
                    nameof(certificate));

            // Normalizamos antes de calcular los digest.
            xml = ClearWindowsLineEndings(xml);
            xml = ClearWindowsTagEndings(xml);

            var document = LoadDocument(xml);

            var container = PrepareSignatureContainer(document);

            var signatureID = "Signature-" + Guid.NewGuid().ToString("N");

            var propertiesID = signatureID + "-SignedProperties";

            using (var rsa = certificate.GetRSAPrivateKey())
            {

                if (rsa == null)
                    throw new InvalidOperationException(
                        "El certificado no contiene una clave RSA.");

                var signedXml = new XadesSignedXml(document)
                {
                    SigningKey = rsa
                };

                signedXml.Signature.Id = signatureID;

                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

                signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

                // Referencia al documento completo.
                var documentReference = new Reference()
                {
                    Uri = "",
                    DigestMethod = SHA512Algorithm
                };

                documentReference.AddTransform(new XmlDsigEnvelopedSignatureTransform());

                documentReference.AddTransform(new XmlDsigExcC14NTransform());

                signedXml.AddReference(documentReference);

                // Propiedades XAdES.
                var properties = GetSignedProperties(certificate, signatureID, propertiesID);

                signedXml.AddObject(properties);

                var propertiesReference = new Reference()
                {
                    Uri = "#" + propertiesID,
                    Type = SignedPropertiesType,
                    DigestMethod = SHA512Algorithm
                };

                propertiesReference.AddTransform(new XmlDsigExcC14NTransform());

                signedXml.AddReference(propertiesReference);

                // Información del certificado.
                var keyInfo = new KeyInfo()
                {
                    Id = signatureID + "-KeyInfo"
                };

                keyInfo.AddClause(new KeyInfoX509Data(certificate));

                signedXml.KeyInfo = keyInfo;

                // Generación criptográfica de la firma.
                signedXml.ComputeSignature();

                container.AppendChild(document.ImportNode(signedXml.GetXml(), true));

            }

            // Verificación criptográfica del resultado.
            if (!Verify(document, certificate))
                throw new CryptographicException(
                    "La firma electrónica UBL no supera la verificación criptográfica.");

            return SaveDocument(document);

        }

        /// <summary>
        /// Comprueba la firma criptográfica de un documento UBL.
        /// </summary>
        /// <param name="xml">Documento UBL firmado.</param>
        /// <param name="certificate">Certificado de verificación.</param>
        /// <returns>True cuando la firma es correcta.</returns>
        public static bool Verify(string xml, X509Certificate2 certificate)
        {

            if (certificate == null)
                throw new ArgumentNullException(nameof(certificate));

            return Verify(LoadDocument(xml), certificate);

        }

        #endregion

        #region Métodos Privados Estáticos

        /// <summary>
        /// Carga un documento XML conservando los espacios en blanco.
        /// </summary>
        /// <param name="xml">Contenido XML.</param>
        /// <returns>Documento XML.</returns>
        private static XmlDocument LoadDocument(string xml)
        {

            var document = new XmlDocument()
            {
                PreserveWhitespace = true,
                XmlResolver = null
            };

            document.LoadXml(xml);

            return document;

        }

        /// <summary>
        /// Prepara el contenedor de firma UBL.
        /// </summary>
        /// <param name="document">Documento XML.</param>
        /// <returns>Elemento SignatureInformation.</returns>
        private static XmlElement PrepareSignatureContainer(XmlDocument document)
        {

            var root = document.DocumentElement;

            if (root == null ||
                root.LocalName != "Invoice" ||
                root.NamespaceURI != UblNamespaces.NamespaceInvoice)
            {
                throw new InvalidOperationException(
                    "El documento no es una factura UBL.");
            }

            // Esta implementación admite una única firma.
            var existingExtensions = root.GetElementsByTagName(
                "UBLExtensions",
                UblNamespaces.NamespaceEXT);

            if (existingExtensions.Count > 0)
                throw new InvalidOperationException(
                    "El documento ya contiene extensiones UBL. " +
                    "Es necesario conservarlas y localizar una extensión de firma apropiada.");

            var extensions = document.CreateElement(
                "ext",
                "UBLExtensions",
                UblNamespaces.NamespaceEXT);

            var extension = document.CreateElement(
                "ext",
                "UBLExtension",
                UblNamespaces.NamespaceEXT);

            var content = document.CreateElement(
                "ext",
                "ExtensionContent",
                UblNamespaces.NamespaceEXT);

            var signatures = document.CreateElement(
                "sig",
                "UBLDocumentSignatures",
                UblNamespaces.NamespaceSIG);

            var information = document.CreateElement(
                "sac",
                "SignatureInformation",
                UblNamespaces.NamespaceSAC);

            signatures.AppendChild(information);
            content.AppendChild(signatures);
            extension.AppendChild(content);
            extensions.AppendChild(extension);

            root.PrependChild(extensions);

            return information;

        }

        /// <summary>
        /// Construye el objeto que contiene las propiedades XAdES.
        /// </summary>
        /// <param name="certificate">Certificado de firma.</param>
        /// <param name="signatureID">Identificador de firma.</param>
        /// <param name="propertiesID">Identificador de propiedades.</param>
        /// <returns>Objeto XMLDSig.</returns>
        private static DataObject GetSignedProperties(X509Certificate2 certificate, string signatureID, string propertiesID)
        {

            var document = new XmlDocument()
            {
                PreserveWhitespace = true
            };

            var qualifyingProperties = document.CreateElement("xades", "QualifyingProperties", NamespaceXADES);

            qualifyingProperties.SetAttribute("Target", "#" + signatureID);

            var signedProperties = document.CreateElement("xades", "SignedProperties", NamespaceXADES);

            signedProperties.SetAttribute("Id", propertiesID);

            var signatureProperties = document.CreateElement("xades", "SignedSignatureProperties", NamespaceXADES);

            var signingTime = document.CreateElement("xades", "SigningTime", NamespaceXADES);

            signingTime.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);

            signatureProperties.AppendChild(signingTime);

            var signingCertificate = document.CreateElement("xades", "SigningCertificate", NamespaceXADES);

            var cert = document.CreateElement("xades", "Cert", NamespaceXADES);

            var certDigest = document.CreateElement("xades", "CertDigest", NamespaceXADES);

            var digestMethod = document.CreateElement("ds", "DigestMethod", NamespaceDS);

            digestMethod.SetAttribute("Algorithm", SHA512Algorithm);

            var digestValue = document.CreateElement("ds", "DigestValue", NamespaceDS);

            byte[] hash;

            using (var sha512 = SHA512.Create())
            {
                hash = sha512.ComputeHash(certificate.RawData);
            }

            digestValue.InnerText = Convert.ToBase64String(hash);

            certDigest.AppendChild(digestMethod);
            certDigest.AppendChild(digestValue);

            cert.AppendChild(certDigest);

            var issuerSerial = document.CreateElement("xades", "IssuerSerial", NamespaceXADES);

            var issuerName = document.CreateElement("ds", "X509IssuerName", NamespaceDS);

            issuerName.InnerText = Rfc2253IssuerFormatter.FromCertificate(certificate, replaceSwithST: true);

            var serialNumber = document.CreateElement("ds", "X509SerialNumber", NamespaceDS);

            serialNumber.InnerText = $"{new BInt(certificate.GetSerialNumber())}";

            issuerSerial.AppendChild(issuerName);
            issuerSerial.AppendChild(serialNumber);

            cert.AppendChild(issuerSerial);
            signingCertificate.AppendChild(cert);

            signatureProperties.AppendChild(signingCertificate);

            signedProperties.AppendChild(signatureProperties);

            qualifyingProperties.AppendChild(signedProperties);

            var fragment = document.CreateDocumentFragment();

            fragment.AppendChild(qualifyingProperties);

            return new DataObject()
            {
                Data = fragment.ChildNodes
            };

        }

        /// <summary>
        /// Verifica criptográficamente la firma XML.
        /// </summary>
        /// <param name="document">Documento firmado.</param>
        /// <param name="certificate">Certificado.</param>
        /// <returns>True si la firma es válida.</returns>
        private static bool Verify(XmlDocument document, X509Certificate2 certificate)
        {

            var signatures = document.GetElementsByTagName("Signature", NamespaceDS);

            if (signatures.Count != 1)
                return false;

            var signedXml = new XadesSignedXml(document);

            signedXml.LoadXml((XmlElement)signatures[0]);

            // Verifica SignatureValue y las referencias.
            return signedXml.CheckSignature(certificate, true);

        }

        /// <summary>
        /// Serializa el documento XML sin introducir
        /// cambios de formato posteriores a la firma.
        /// </summary>
        /// <param name="document">Documento XML.</param>
        /// <returns>Documento XML serializado.</returns>
        private static string SaveDocument(XmlDocument document)
        {

            var settings = new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(false),
                Indent = false,
                OmitXmlDeclaration = false
            };

            using (var stream = new MemoryStream())
            {

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    document.Save(writer);
                }

                return Encoding.UTF8.GetString(stream.ToArray());

            }

        }

        /// <summary>
        /// Normaliza los saltos de línea de Windows.
        /// </summary>
        /// <param name="text">Texto XML.</param>
        /// <returns>Texto con saltos de línea LF.</returns>
        private static string ClearWindowsLineEndings(string text)
        {

            return text.Replace("\r\n", "\n");

        }

        /// <summary>
        /// Elimina los espacios anteriores al cierre
        /// de las etiquetas XML vacías.
        /// </summary>
        /// <param name="text">Texto XML.</param>
        /// <returns>Texto XML normalizado.</returns>
        private static string ClearWindowsTagEndings(string text)
        {

            return text.Replace(" />", "/>");

        }

        #endregion

    }

}

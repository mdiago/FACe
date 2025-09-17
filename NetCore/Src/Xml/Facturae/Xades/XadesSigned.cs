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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Xml;

namespace FACe.Xml.Xades
{

    /// <summary>
    /// Representa un documento para firmar con
    /// XADes.
    /// </summary>
    public class XadesSigned
    {

        #region Propiedades Privadas Estáticas

        /// <summary>
        /// Tipo de proveedor para RSA.
        /// </summary>
        internal readonly static int PROV_RSA_AES = 24;

        /// <summary>
        /// Espacio nombres Xades.
        /// </summary>
        internal const string XadesNamespaceUrl = "http://uri.etsi.org/01903/v1.3.2#";

        /// <summary>
        /// Politica de firma.
        /// </summary>
        internal const string SigPolicyId = "http://www.facturae.es/politica_de_firma_formato_facturae/politica_de_firma_formato_facturae_v3_1.pdf";

        /// <summary>
        /// Calificador politica de firma.
        /// </summary>
        internal const string SigPolicyQualifier = null;

        /// <summary>
        /// Politica de firma.
        /// </summary>
        internal const string SigPolicyHash = "Ohixl6upD6av8N7pEvDABhEL6hM=";

        #endregion

        #region Propiedades Privadas de Instacia

        /// <summary>
        /// Documento xml original transformado en los
        /// saltos de línea.
        /// </summary>
        internal XmlDocument XmlDocumentSource { get; private set; }

        /// <summary>
        /// Documento xml creado para contener el bloque
        /// Signature.
        /// </summary>
        internal XmlDocument XmlDocumentSignature { get; private set; }

        /// <summary>
        /// Documento xml creado para contener el bloque
        /// SignedProperties.
        /// </summary>
        internal XmlDocument XmlDocumentSignedProperties { get; private set; }

        /// <summary>
        /// Documento xml creado para contener el bloque
        /// KeyInfo.
        /// </summary>
        internal XmlDocument XmlDocumentKeyInfo { get; private set; }

        /// <summary>
        /// Identificador de la firma.
        /// </summary>
        internal string SignatureId { get; private set; }

        /// <summary>
        /// Identificador del objeto firmado.
        /// </summary>
        internal string ReferenceId { get; private set; }

        /// <summary>
        /// Nombre del elemento root del documento original.
        /// </summary>
        internal string SourceName { get; private set; }

        /// <summary>
        /// Firma únicamente con el bloque Object.
        /// </summary>
        internal string XmlSignatureOnlyObject { get; private set; }

        /// <summary>
        /// Texto xml del bloque KeyInfo.
        /// </summary>
        internal string XmlKeyInfo { get; private set; }

        /// <summary>
        /// Texto xml del bloque SignedInfo.
        /// </summary>
        internal string XmlSignedInfo { get; private set; }

        /// <summary>
        /// Texto xml documento original.
        /// </summary>
        internal string XmlTextDocumentSource { get; private set; }

        /// <summary>
        /// Texto xml documento original con saltos de línea Unix.
        /// </summary>
        internal string XmlTextDocumentSourceCleared { get; private set; }

        /// <summary>
        /// KeyInfo para la firma.
        /// </summary>
        internal KeyInfo KeyInfo { get; private set; }

        /// <summary>
        /// Espacios de nombres del documento original.
        /// </summary>
        internal Dictionary<string, string> SourceNamespaces { get; private set; }

        /// <summary>
        /// Espacios de nombres de la firma.
        /// </summary>
        internal Dictionary<string, string> SignatureNamespaces { get; private set; }

        /// <summary>
        /// Todos los espacios de nombres.
        /// </summary>
        internal Dictionary<string, string> AllNamespaces
        {

            get
            {

                var allNamespaces = new Dictionary<string, string>();

                foreach (var ns in SourceNamespaces)
                    allNamespaces.Add(ns.Key, ns.Value);

                foreach (var ns in SignatureNamespaces)
                    if (!allNamespaces.ContainsKey(ns.Key))
                        allNamespaces.Add(ns.Key, ns.Value);

                return allNamespaces;

            }

        }

        /// <summary>
        /// Hash del documento original.
        /// </summary>
        internal string SignedInfoObjHash { get; private set; }

        /// <summary>
        /// Hash de SignedProperties.
        /// </summary>
        internal string SignedInfoSignedPropertiesHash { get; private set; }

        /// <summary>
        /// Hash de KeyInfo.
        /// </summary>
        internal string SignedInfoKeyInfoHash { get; private set; }

        /// <summary>
        /// Certificado para la firma.
        /// </summary>
        internal X509Certificate2 Certificate { get; private set; }

        /// <summary>
        /// Objeto Signature.
        /// </summary>
        internal Signature.Xades.Props.Signature Signature { get; private set; }

        /// <summary>
        /// Clave privada para la firma.
        /// </summary>
        internal RSACryptoServiceProvider SigningKey { get; private set; }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="xml">Texto xml del documento a firmar.</param>
        /// <param name="certificate">Certificado para la firma.</param>
        public XadesSigned(string xml, X509Certificate2 certificate)
        {
            
            try 
            {

                SignatureId = GetSignatureId();
                ReferenceId = GetReferenceId();
                Certificate = certificate;
                XmlTextDocumentSource = xml;
                XmlTextDocumentSourceCleared = ClearWindowsLineEndings(xml);
                XmlDocumentSource = GetSource(XmlTextDocumentSourceCleared);
                SignedInfoObjHash = GetXmlDocumentHash(XmlDocumentSource);
                SourceName = XmlDocumentSource.DocumentElement.Name;
                SourceNamespaces = GetNamespaces(XmlDocumentSource);
                Signature = GetSignature(XmlDocumentSource, certificate);

                XmlDocumentSignature = GetXmlDocumentSignature(Signature);

                XmlSignatureOnlyObject = XmlDocumentSignature.OuterXml;

                SignatureNamespaces = GetNamespaces(XmlDocumentSignature);

                XmlDocumentSignedProperties = GetXmlDocumentSignedProperties(XmlDocumentSignature);

                var orderedPrefixes = GetOrderedPrefixes(AllNamespaces);

                for (int p = 0; p < orderedPrefixes.Count; p++)
                    XmlDocumentSignedProperties.DocumentElement.SetAttribute(
                        $"xmlns{(orderedPrefixes[p] == "" ? "" : $":{orderedPrefixes[p]}")}",
                        AllNamespaces[orderedPrefixes[p]]);

                SignedInfoSignedPropertiesHash = GetXmlDocumentHash(XmlDocumentSignedProperties);

                SigningKey = GetCertificateKey(certificate);

                KeyInfo = GetKeyInfo();

                XmlDocumentKeyInfo = GetXmlDocumentKeyInfo(XmlDocumentSignature, KeyInfo);

                XmlKeyInfo = XmlDocumentKeyInfo.OuterXml;

                orderedPrefixes = GetOrderedPrefixes(SourceNamespaces);

                for (int p = 0; p < orderedPrefixes.Count; p++)
                    XmlDocumentKeyInfo.DocumentElement.SetAttribute(
                        $"xmlns{(orderedPrefixes[p] == "" ? "" : $":{orderedPrefixes[p]}")}",
                        AllNamespaces[orderedPrefixes[p]]);

                SignedInfoKeyInfoHash = GetXmlDocumentHash(XmlDocumentKeyInfo);

            }
            catch (Exception ex) 
            {

                Common.Utils.Throw($"Error contruyendo instancia de XadesSigned.", ex);

            }

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Obtiene un nuevo identificador para una firma.
        /// </summary>
        /// <returns>Id. para la firma.</returns>
        private string GetSignatureId()
        {

            return $"Signature-{Guid.NewGuid()}";

        }

        /// <summary>
        /// Obtiene un nuevo identificador para el objeto a firmar.
        /// </summary>
        /// <returns>Id. para el objeto a firmar.</returns>
        private string GetReferenceId()
        {

            return $"Reference-{Guid.NewGuid()}";

        }

        /// <summary>
        /// Paso saltos de línea "Windows" CR-LF a formtato 
        /// "Unix" F line endings (0x0A, o #xA en xml-ese).
        /// Si el archivo se ha creado en un sistema Windows
        /// existen bastante posibilidades de tener el final
        /// de línea incorrecto.
        /// </summary>
        /// <param name="text">Texto a limpiar.</param>
        /// <returns>Texto con saltos de linea 'Unix'.</returns>
        private string ClearWindowsLineEndings(string text)
        {

            return text.Replace("\r\n", "\n");

        }

        /// <summary>
        /// Reemplaza el final de tag xml de XmlDocument
        /// de Windows ' />' por '/>'.
        /// </summary>
        /// <param name="text">Texto a limpiar.</param>
        /// <returns>Texto con saltos de linea 'Unix'.</returns>
        private string ClearWindowsTagEndings(string text)
        {

            return text.Replace(" />", "/>");

        }

        /// <summary>
        /// Crea un documento xml a partir del texto.
        /// </summary>
        /// <param name="xml">Texto xml documento.</param>
        /// <returns>Documento xml creado.</returns>
        private XmlDocument GetSource(string xml)
        {

            try 
            {

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.PreserveWhitespace = true;
                xmlDocument.LoadXml(xml);

                return xmlDocument;

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetSource de XadesSigned.", ex);

            }

            return null;

        }

        /// <summary>
        /// Devuelve los bytes del texto xml canónico.
        /// </summary>
        /// <param name="xmlDocument">Documento xml.</param>
        /// <returns> Bytes del texto xml canónico.</returns>
        internal byte[] GetXmlDocumentCn14(XmlDocument xmlDocument)
        {

            byte[] cn14 = null;

            try 
            {

                var c14nMethodTransform = new XmlDsigC14NTransform();
                c14nMethodTransform.LoadInput(xmlDocument);

                using (var ms = new MemoryStream())
                {

                    using (var stream = c14nMethodTransform.GetOutput() as Stream)
                        stream.CopyTo(ms);

                    cn14 = ms.ToArray();

                }

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentCn14 de XadesSigned.", ex);

            }

            return cn14;

        }

        /// <summary>
        /// Devuelve el hash del documento xml.
        /// </summary>
        /// <param name="xmlDocument">Documento xml.</param>
        /// <returns>Hash en base 64.</returns>
        internal string GetXmlDocumentHash(XmlDocument xmlDocument)
        {

            string c14nHashB64 = null;

            try 
            {

                byte[] cn14 = GetXmlDocumentCn14(xmlDocument);

                var hash = new SHA512Managed();

                byte[] c14nHash = hash.ComputeHash(cn14);
                c14nHashB64 = Convert.ToBase64String(c14nHash);

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentHash de XadesSigned.", ex);

            }

            return c14nHashB64;

        }

        /// <summary>
        /// Recupera los espacion de nombres existentes
        /// en un nodo xml y sus hijos.
        /// </summary>
        /// <param name="node">Nodo en el que buscar.</param>
        /// <returns>Diccionario con los espacios
        /// de nombres con sus prefijos.</returns>
        internal Dictionary<string, string> GetNamespaces(XmlNode node)
        {

            Dictionary<string, string> namespaces = new Dictionary<string, string>();

            try 
            {

                if (node.Attributes != null)
                    foreach (XmlAttribute att in node.Attributes)
                        if (Regex.IsMatch(att.Name, @"xmlns:{0,1}\w*"))
                            namespaces.Add($"{Regex.Match(att.Name, @"(?<xmlns>xmlns)(?<separator>:{0,1})(?<prefix>\w*)").Groups["prefix"]}", att.Value);

                Dictionary<string, string> childNamespaces = new Dictionary<string, string>();

                foreach (XmlNode child in node.ChildNodes)
                {

                    childNamespaces = GetNamespaces(child);

                    foreach (var ns in childNamespaces)
                        if (!namespaces.ContainsKey(ns.Key))
                            namespaces.Add(ns.Key, ns.Value);

                }

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetNamespaces de XadesSigned.", ex);

            }

            return namespaces;

        }

        /// <summary>
        /// Devuelve los prefijos de espacios de nombres ordenados.
        /// </summary>
        /// <param name="namespaces">Diccionario de espacios de nombres</param>
        /// <returns>Lista de prefijos de espacios de nombre
        /// ordenada.</returns>
        internal List<string> GetOrderedPrefixes(Dictionary<string, string> namespaces)
        {

            var orderedPrefixes = namespaces.Keys.ToList();
            orderedPrefixes.Sort();

            return orderedPrefixes;

        }

        /// <summary>
        /// Obtiene un objeto Signature únicamente con el bloque
        /// Object preparado.
        /// </summary>
        /// <param name="xmlDocument">Documento xml de orígen.</param>
        /// <param name="certificate">Certificado digital de la firma.</param>
        /// <returns></returns>
        internal Signature.Xades.Props.Signature GetSignature(XmlDocument xmlDocument, X509Certificate2 certificate)
        {

            Signature.Xades.Props.Signature signature = null;

            try 
            {

                signature = new Signature.Xades.Props.Signature(xmlDocument.DocumentElement, ReferenceId);

                // signatureID            
                signature.XmlElement.SetAttribute("Id", $"{SignatureId}-Signature");
                signature.Object.QualifyingProperties.XmlElement.SetAttribute("xmlns:xades", $"http://uri.etsi.org/01903/v1.3.2#");
                signature.Object.QualifyingProperties.XmlElement.SetAttribute("Id", $"{SignatureId}-QualifyingProperties");
                signature.Object.QualifyingProperties.XmlElement.SetAttribute("Target", $"#{SignatureId}-Signature");
                signature.Object.QualifyingProperties.SignedProperties.XmlElement.SetAttribute("Id", $"{SignatureId}-SignedProperties");

                signature.Object.QualifyingProperties.SignedProperties.SignedSignatureProperties.SigningCertificate.Cert.Certificate = certificate;

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetSignature de XadesSigned.", ex);

            }

            return signature;

        }

        /// <summary>
        /// Obtiene un documento xml con el bloque Signature.
        /// </summary>
        /// <param name="signature">Instancia de la clase Signature.</param>
        /// <returns>Documento xml compuesto a partir de 
        /// la instancia Signatue pasada como parámetro.</returns>
        internal XmlDocument GetXmlDocumentSignature(Signature.Xades.Props.Signature signature)
        {

            XmlDocument xmlDoc = new XmlDocument();

            try 
            {

                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(signature.XmlElement.OuterXml);

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentSignature de XadesSigned.", ex);

            }

            return xmlDoc;

        }

        /// <summary>
        /// Obtiene un documento xml con el bloque
        /// SignedProperties.
        /// </summary>
        /// <param name="xmlDocumentSignature">Documento xml con el bloque
        /// Signature.</param>
        /// <returns>Documento xml con el bloque SignedProperties.</returns>
        internal XmlDocument GetXmlDocumentSignedProperties(XmlDocument xmlDocumentSignature)
        {

            XmlNode signedProperties = null;

            try 
            {

                signedProperties = xmlDocumentSignature.GetElementsByTagName("xades:SignedProperties")[0];

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentSignedProperties de XadesSigned.", ex);

            }

            XmlDocument xmlDoc = new XmlDocument();

            try
            {

                xmlDoc.PreserveWhitespace = true;
                xmlDoc.AppendChild(xmlDoc.ImportNode(signedProperties, true));

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentSignedProperties de XadesSigned.", ex);

            }

            return xmlDoc;

        }

        /// <summary>
        /// Obtiene un objeto KeyInfo.
        /// </summary>
        /// <returns>KeyInfo referente a la firma.</returns>
        internal KeyInfo GetKeyInfo()
        {

            KeyInfo keyInfo = new KeyInfo();

            try 
            {

                keyInfo.Id = $"{SignatureId}-KeyInfo";
                keyInfo.AddClause(new KeyInfoX509Data(Certificate, X509IncludeOption.WholeChain)); // Añade elemento KeyInfo.X509Data.RSAKeyValue.X509Certificate
                keyInfo.AddClause(new RSAKeyValue(SigningKey));  // Añade elemento KeyInfo.KeyValue.RSAKeyValue

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentSignedProperties de XadesSigned.", ex);

            }

            return keyInfo;

        }

        /// <summary>
        /// Obtiene un documento xml con el bloque KeyInfo.
        /// </summary>
        /// <param name="xmlDocumentSignature">Documento xml con el bloque Signatures.</param>
        /// <param name="keyInfo">KeyInfo para la firma.</param> 
        /// <returns>Documento xml con el bloque KeyInfo.</returns>
        internal XmlDocument GetXmlDocumentKeyInfo(XmlDocument xmlDocumentSignature, KeyInfo keyInfo)
        {

            XmlDocument xmlDocumentKeyInfo = null;

            try 
            {

                var xmlNodeSignature = XmlDocumentSignature.GetElementsByTagName("ds:Signature")[0];
                var signature = XmlDocumentSignature.FirstChild;
                var xmlKeyInfo = keyInfo.GetXml();

                SetPrefix("ds", xmlKeyInfo);

                XmlDocument xmlDocumentNodeSignature = new XmlDocument();
                xmlDocumentNodeSignature.PreserveWhitespace = true;
                xmlDocumentNodeSignature.AppendChild(xmlDocumentNodeSignature.ImportNode(xmlNodeSignature, false));

                XmlDocument xmlDocumentSignatureKeyInfo = new XmlDocument();
                xmlDocumentSignatureKeyInfo.PreserveWhitespace = true;
                xmlDocumentSignatureKeyInfo.LoadXml(xmlDocumentNodeSignature.OuterXml);

                xmlDocumentSignatureKeyInfo.DocumentElement.InnerXml = xmlKeyInfo.OuterXml;

                var xmlNodeKeyInfo = xmlDocumentSignatureKeyInfo.GetElementsByTagName("ds:KeyInfo")[0];
                xmlDocumentKeyInfo = new XmlDocument();
                xmlDocumentKeyInfo.PreserveWhitespace = true;
                xmlDocumentKeyInfo.AppendChild(xmlDocumentKeyInfo.ImportNode(xmlNodeKeyInfo, true));

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetXmlDocumentKeyInfo de XadesSigned.", ex);

            }

            return xmlDocumentKeyInfo;

        }

        /// <summary>
        /// Establece la propiedad 'Prefix' entre los
        /// nodos hijos de un nodo y sobre sus nodos
        /// hijos de forma recursiva.
        /// Se realiza el proceso hasta alcanzar el
        /// nodo 'xades:QualifyingProperties'.
        /// </summary>
        /// <param name="prefix">Prefijo a establecer.</param>
        /// <param name="node">Nodo a tratar.</param>
        private void SetPrefix(string prefix, XmlNode node)
        {

            foreach (XmlNode n in node.ChildNodes)
                SetPrefix(prefix, n);

            node.Prefix = prefix;

        }

        /// <summary>
        /// Devuelve una clave válida para la firma con SA256.
        /// </summary>
        /// <param name="certificate">Certificado del que obtener la clave.</param>
        /// <returns>Clave válida para firma SA256.</returns>
        private RSACryptoServiceProvider GetCertificateKey(X509Certificate2 certificate)
        {

            // Proveedor para SA256
            var exportedKeyMaterial = certificate.PrivateKey.ToXmlString(true); //Include Private Parameters
            var key = new RSACryptoServiceProvider(new CspParameters(PROV_RSA_AES));
            key.PersistKeyInCsp = false;
            key.FromXmlString(exportedKeyMaterial);
            return key;

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Devuelve el xml firmado.
        /// </summary>
        /// <returns>Xml firmado.</returns>
        public string GetSignedXml()
        {

            try 
            {

                XmlDocument xmlDocSignature = new XmlDocument();
                xmlDocSignature.PreserveWhitespace = true;
                xmlDocSignature.LoadXml(XmlSignatureOnlyObject);

                var dsNamespaceUri = "http://www.w3.org/2000/09/xmldsig#";

                var signedInfo = xmlDocSignature.CreateElement("ds:SignedInfo", dsNamespaceUri);

                var canonicalizationMethod = xmlDocSignature.CreateElement("ds:CanonicalizationMethod", dsNamespaceUri);
                canonicalizationMethod.SetAttribute("Algorithm", "http://www.w3.org/TR/2001/REC-xml-c14n-20010315");
                var signatureMethod = xmlDocSignature.CreateElement("ds:SignatureMethod", dsNamespaceUri);
                signatureMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");

                signedInfo.AppendChild(canonicalizationMethod);
                signedInfo.AppendChild(signatureMethod);

                var refInXml = xmlDocSignature.CreateElement("ds:Reference", dsNamespaceUri);
                refInXml.SetAttribute("Id", $"{ReferenceId}");
                refInXml.SetAttribute("URI", "");
                var transforms = xmlDocSignature.CreateElement("ds:Transforms", dsNamespaceUri);
                var transform = xmlDocSignature.CreateElement("ds:Transform", dsNamespaceUri);
                transform.SetAttribute("Algorithm", "http://www.w3.org/2000/09/xmldsig#enveloped-signature");
                var digestMethodRefIn = xmlDocSignature.CreateElement("ds:DigestMethod", dsNamespaceUri);
                digestMethodRefIn.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha512");
                var digestValue = xmlDocSignature.CreateElement("ds:DigestValue", dsNamespaceUri);
                digestValue.InnerText = SignedInfoObjHash;

                transforms.AppendChild(transform);
                refInXml.AppendChild(transforms);
                refInXml.AppendChild(digestMethodRefIn);
                refInXml.AppendChild(digestValue);

                var refSignedProperties = xmlDocSignature.CreateElement("ds:Reference", dsNamespaceUri);
                refSignedProperties.SetAttribute("Type", "http://uri.etsi.org/01903#SignedProperties");
                refSignedProperties.SetAttribute("URI", $"#{SignatureId}-SignedProperties");
                var digestMethodSignedProperties = xmlDocSignature.CreateElement("ds:DigestMethod", dsNamespaceUri);
                digestMethodSignedProperties.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha512");
                var digestValueSignedProperties = xmlDocSignature.CreateElement("ds:DigestValue", dsNamespaceUri);
                digestValueSignedProperties.InnerText = SignedInfoSignedPropertiesHash;

                refSignedProperties.AppendChild(digestMethodSignedProperties);
                refSignedProperties.AppendChild(digestValueSignedProperties);

                var refKeyInfo = xmlDocSignature.CreateElement("ds:Reference", dsNamespaceUri);
                refKeyInfo.SetAttribute("URI", $"#{SignatureId}-KeyInfo");
                var digestMethodKeyInfo = xmlDocSignature.CreateElement("ds:DigestMethod", dsNamespaceUri);
                digestMethodKeyInfo.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha512");

                var digestValueKeyInfo = xmlDocSignature.CreateElement("ds:DigestValue", dsNamespaceUri);
                digestValueKeyInfo.InnerText = SignedInfoKeyInfoHash;

                refKeyInfo.AppendChild(digestMethodKeyInfo);
                refKeyInfo.AppendChild(digestValueKeyInfo);

                signedInfo.AppendChild(refInXml);
                signedInfo.AppendChild(refSignedProperties);
                signedInfo.AppendChild(refKeyInfo);

                // Calculo de la firma

                XmlDocument xmlDocSignedInfo = new XmlDocument();
                xmlDocSignedInfo.PreserveWhitespace = true;
                xmlDocSignedInfo.AppendChild(xmlDocSignedInfo.ImportNode(signedInfo, true));

                var orderedPrefixes = GetOrderedPrefixes(SourceNamespaces);

                for (int p = 0; p < orderedPrefixes.Count; p++)
                    xmlDocSignedInfo.DocumentElement.SetAttribute(
                        $"xmlns{(orderedPrefixes[p] == "" ? "" : $":{orderedPrefixes[p]}")}",
                        SourceNamespaces[orderedPrefixes[p]]);

                var cn14SignedInfo = GetXmlDocumentCn14(xmlDocSignedInfo);

                var hash = new SHA256Managed();

                var signatureValue = SigningKey.SignData(cn14SignedInfo, hash);
                var signatureValueB64 = Convert.ToBase64String(signatureValue);

                var sigValue = xmlDocSignature.CreateElement("ds:SignatureValue", dsNamespaceUri);
                sigValue.SetAttribute("Id", $"{SignatureId}-SignatureValue");
                sigValue.InnerText = signatureValueB64;

                // Fin calculo de la firma

                var sigNode = xmlDocSignature.GetElementsByTagName("ds:Signature")[0];
                var objNode = xmlDocSignature.GetElementsByTagName("ds:Object")[0];

                sigNode.InnerXml = $"{signedInfo.OuterXml}{sigValue.OuterXml}{XmlKeyInfo}{sigNode.InnerXml}";

                XmlDocument xmlDocSigned = new XmlDocument();
                xmlDocSigned.PreserveWhitespace = true;
                xmlDocSigned.LoadXml(sigNode.OuterXml);

                var cn14Signature = GetXmlDocumentCn14(xmlDocSigned);
                var xmlSignature = System.Text.Encoding.UTF8.GetString(cn14Signature);

                XmlDocument xmlResult = new XmlDocument();
                xmlResult.PreserveWhitespace = true;
                xmlResult.LoadXml(XmlTextDocumentSourceCleared);

                xmlResult.DocumentElement.InnerXml = $"{xmlResult.DocumentElement.InnerXml}{sigNode.OuterXml}";

                return $"<?xml version=\"1.0\" encoding=\"utf-8\"?>{ClearWindowsTagEndings(xmlResult.DocumentElement.OuterXml)}";

            }
            catch (Exception ex)
            {

                Common.Utils.Throw($"Error en método GetSignedXml de XadesSigned.", ex);

            }

            return null;

        }

        #endregion

    }

}
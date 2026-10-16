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


using System;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace FACe.Xml.Ubl.Signature
{

    /// <summary>
    /// Extiende SignedXml para resolver referencias
    /// a propiedades XAdES contenidas en DataObject.
    /// </summary>
    internal class XadesSignedXml : SignedXml
    {

        #region Constructores

        /// <summary>
        /// Inicializa la firma XML.
        /// </summary>
        /// <param name="document">Documento XML.</param>
        public XadesSignedXml(XmlDocument document)
            : base(document)
        {
        }

        #endregion

        #region Métodos Públicos

        /// <summary>
        /// Localiza un elemento mediante su identificador,
        /// incluyendo las propiedades XAdES de los DataObject.
        /// </summary>
        /// <param name="document">Documento XML.</param>
        /// <param name="idValue">Identificador del elemento.</param>
        /// <returns>Elemento encontrado o null.</returns>
        public override XmlElement GetIdElement(
            XmlDocument document,
            string idValue)
        {

            // Primero utilizamos la resolución estándar.
            var element = base.GetIdElement(
                document, idValue);

            if (element != null)
                return element;

            const string namespaceXADES =
                "http://uri.etsi.org/01903/v1.3.2#";

            // Durante ComputeSignature, los objetos
            // todavía pueden estar fuera del documento.
            foreach (DataObject dataObject in Signature.ObjectList)
            {

                var nodes = dataObject.Data;

                if (nodes == null)
                    continue;

                foreach (XmlNode node in nodes)
                {

                    var root = node as XmlElement;

                    if (root == null)
                        continue;

                    if (root.LocalName == "SignedProperties" &&
                        root.NamespaceURI == namespaceXADES &&
                        root.GetAttribute("Id") == idValue)
                    {
                        return root;
                    }

                    var properties = root.GetElementsByTagName(
                        "SignedProperties",
                        namespaceXADES);

                    foreach (XmlElement property in properties)
                    {

                        if (property.GetAttribute("Id") == idValue)
                            return property;

                    }

                }

            }

            // Durante Verify, SignedProperties también
            // puede encontrarse en el documento firmado.
            var documentProperties = document.GetElementsByTagName(
                "SignedProperties",
                namespaceXADES);

            foreach (XmlElement property in documentProperties)
            {

                if (property.GetAttribute("Id") == idValue)
                    return property;

            }

            return null;

        }

        #endregion

    }

}

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
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Cabecera de factura. Para cada una de las facturas que pueden 
    /// componer un Lote, recoge datos que determinan inequívocamente 
    /// cada factura.
    /// </summary>
    [Serializable()]
    public class InvoiceHeader
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Número de factura. Número asignado por el Emisor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Número de serie asignado por el Emisor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string InvoiceSeriesCode { get; set; }

        /// <summary>
        /// Tipo documento factura. Puede tomar 3 valores (FC, FA y AF), que se 
        /// describen como “Factura completa u ordinaria”, “Factura simplificada” y 
        /// “Código sin uso desde la entrada en vigor del RD 1789/2010. Se mantiene 
        /// por compatibilidad hacia atrás. Antes significaba autofactura. Para indicar
        /// que se trata de una factura expedida por el destinatario, se recomienda 
        /// emplear el elemento InvoiceIssuerType empleando el valor RE, que significa 
        /// Destinatario”, respectivamente.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public InvoiceDocumentType InvoiceDocumentType { get; set; }

        /// <summary>
        /// Clase de Factura. Puede tomar 6 valores (OO, OR, OC, CO, CR y CC), que se 
        /// describen como “Original”, “Original rectificativa”, “Original recapitulativa”, 
        /// “Duplicado original”, “Duplicado rectificativa” y “Duplicado recapitulativa”.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public InvoiceClass InvoiceClass { get; set; }

        /// <summary>
        /// Rectificativa.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Corrective Corrective { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de InvoiceHeaderType.
        /// </summary>
        /// <returns>Representación textual de la instancia de InvoiceHeaderType.</returns>
        public override string ToString()
        {
            
            return $"{InvoiceDocumentType}, {InvoiceClass}, {InvoiceNumber}";

        }

        #endregion

    }

}
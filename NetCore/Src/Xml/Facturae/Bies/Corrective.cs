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
    /// Datos corrección.
    /// </summary>
    [Serializable()]
    public class Corrective
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Número de la factura que se rectifica. Será obligatorio 
        /// cuando el dato "CorrectionMethod" (Código del criterio 
        /// de la rectificación) sea "01" o "02".
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string InvoiceNumber { get; set; }

        /// <summary>
        /// Número de serie de la factura que se rectifica.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string InvoiceSeriesCode { get; set; }

        /// <summary>
        /// Código del motivo. Código numérico del motivo de rectificación. "01" a "16" 
        /// errores según reglamento RD 1496/2003; "80" a "85" errores según Artº 80
        /// Ley 37/92 el IVA.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ReasonCode ReasonCode { get; set; }

        /// <summary>
        /// Descripción motivo. Descripción del motivo de rectificación y que se
        /// corresponde con cada código. Ver tabla de códigos y descripciones.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ReasonDescription ReasonDescription { get; set; }

        /// <summary>
        /// Período natural en el que se produjeron los efectos fiscales de la factura
        /// a rectificar; y, por lo tanto, se tributó, y que ahora, es objeto de
        /// rectificación. ISO 8601:2004. 
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public PeriodDates TaxPeriod { get; set; }

        /// <summary>
        /// Método de corrección.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public CorrectionMethod CorrectionMethod { get; set; }

        /// <summary>
        /// Descripción del método de correción.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public CorrectionMethodDescription CorrectionMethodDescription { get; set; }

        /// <summary>
        /// Válido a partir de la versión 3.2
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string AdditionalReasonDescription { get; set; }

        /// <summary>
        /// Válido a partir de la versión 3.2.2
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified, DataType = "date")]
        public DateTime InvoiceIssueDate { get; set; }

        /// <summary>
        /// Válido a partir de la versión 3.2.2
        /// </summary>
        [XmlIgnore()]
        public bool InvoiceIssueDateSpecified { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de CorrectiveType.
        /// </summary>
        /// <returns>Representación textual de la instancia de CorrectiveType.</returns>
        public override string ToString()
        {
            return $"{ReasonCode}, {ReasonDescription}";
        }

        #endregion

    }

}
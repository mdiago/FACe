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
    /// Representa un lote de facturas.
    /// </summary>
    [Serializable()]
    public class Batch
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Identificador del lote. Concatenación del nº de documento del emisor con el número 
        /// de la primera factura y el número de serie caso de existir.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string BatchIdentifier { get; set; }

        /// <summary>
        /// Número total de facturas. Refleja, cuando es lote, el número de facturas del mismo.
        /// Siempre será valor "1" cuando el campo Modality (Modalidad) tenga el valor "I".
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public long InvoicesCount { get; set; }

        /// <summary>
        /// Total facturas. Suma de los importes InvoiceTotal del Fichero. Este importe lo es a
        /// efectos de total de factura y fiscales, sin tener en cuenta subvenciones, anticipos y/o 
        /// retenciones que pudieran haberse practicado.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Amount TotalInvoicesAmount { get; set; }

        /// <summary>
        /// Total a pagar. Suma de los importes TotalOutstandingAmount del Fichero, hasta ocho decimales. 
        /// Es el importe que efectivamente se adeuda, una vez descontados los anticipos y sin tener en 
        /// cuenta las retenciones.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Amount TotalOutstandingAmount { get; set; }

        /// <summary>
        /// Total a Ejecutar. Sumatorio de los Importes TotalExecutableAmount, hasta ocho decimales.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public Amount TotalExecutableAmount { get; set; }

        /// <summary>
        /// Código ISO 4217:2001 Alpha-3 de la moneda en la que se emite la factura. Si difiere de la 
        /// moneda EURO o del campo ExchangeRateDetails será obligatorio indicar el contravalor y el 
        /// tipo/fecha de cambio para los campos de base imponible y cuota, retenida como repercutida, 
        /// así como en los totales TotalInvoicesAmount, TotalOutstandingAmount, y TotalExecutableAmount.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public CurrencyCode InvoiceCurrencyCode { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de BatchType.
        /// </summary>
        /// <returns>Representación textual de la instancia de BatchType.</returns>
        public override string ToString()
        {
            return $"{BatchIdentifier} ({InvoicesCount}) ";
        }

        #endregion

    }

}
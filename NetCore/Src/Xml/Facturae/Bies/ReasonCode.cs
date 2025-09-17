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
using System.Xml.Serialization;

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Código del motivo. Código numérico del motivo de rectificación. 
    /// "01" a "16" errores según reglamento RD 1496/2003; "80" a "85" 
    /// errores según Artº 80 Ley 37/92 el IVA.
    /// </summary>
    [Serializable()]
    public enum ReasonCode
    {

        /// <summary>
        /// Número de la factura
        /// </summary>
        [XmlEnum("01")]
        InvoiceNumber,

        /// <summary>
        /// Serie de la factura
        /// </summary>
        [XmlEnum("02")]
        InvoiceSerialNumber,

        /// <summary>
        /// Fecha expedición
        /// </summary>
        [XmlEnum("03")]
        IssueDate,

        /// <summary>
        /// Nombre y apellidos/Razón social - Emisor
        /// </summary>
        [XmlEnum("04")]
        IssuerName,

        /// <summary>
        /// Nombre y apellidos/Razón social - Receptor
        /// </summary>
        [XmlEnum("05")]
        ReceiverName,

        /// <summary>
        /// Identificación fiscal Emisor/Obligado
        /// </summary>
        [XmlEnum("06")]
        IssuerTaxIdentificationNumber,

        /// <summary>
        /// Identificación fiscal Receptor
        /// </summary>
        [XmlEnum("07")]
        ReceiverTaxIdentificationNumber,

        /// <summary>
        /// Domicilio Emisor/Obligado
        /// </summary>
        [XmlEnum("08")]
        IssuerAddress,

        /// <summary>
        /// Domicilio Receptor
        /// </summary>
        [XmlEnum("09")]
        ReceiverAddress,

        /// <summary>
        /// Detalle Operación
        /// </summary>
        [XmlEnum("10")]
        ItemLine,

        /// <summary>
        /// Porcentaje impositivo a aplicar
        /// </summary>
        [XmlEnum("11")]
        ApplicableTaxRate,

        /// <summary>
        /// Cuota tributaria a aplicar
        /// </summary>
        [XmlEnum("12")]
        ApplicableTaxAmount,

        /// <summary>
        /// Fecha/Periodo a aplicar
        /// </summary>
        [XmlEnum("13")]
        ApplicablePeriod,

        /// <summary>
        /// Clase de factura
        /// </summary>
        [XmlEnum("14")]
        InvoiceClass,

        /// <summary>
        /// Literales legales
        /// </summary>
        [XmlEnum("15")]
        LegalLiterals,

        /// <summary>
        /// Base imponible
        /// </summary>
        [XmlEnum("16")]
        TaxableBase,

        /// <summary>
        /// Cálculo de cuotas repercutidas
        /// </summary>
        [XmlEnum("80")]
        CalculationOfTaxOutputs,

        /// <summary>
        /// Cálculo de cuotas retenidas
        /// </summary>
        [XmlEnum("81")]
        CalculationOfTaxInputs,

        /// <summary>
        /// Base imponible modificada por devolución de envases/embalajes
        /// </summary>
        [XmlEnum("82")]
        ReturnOfPackageAndPackagingMaterials,

        /// <summary>
        /// Base imponible modificada por descuentos y bonificaciones
        /// </summary>
        [XmlEnum("83")]
        DiscountsAndRebates,

        /// <summary>
        /// Base imponible modificada por resolución firme, judicial o administrativa
        /// </summary>
        [XmlEnum("84")]
        FirmCourtRulingOrAdministrativeDecision,

        /// <summary>
        /// Base imponible modificada cuotas repercutidas no satisfechas. Auto de declaración de concurso
        /// </summary>
        [XmlEnum("85")]
        JudgementOpeningInsolvencyProceedings

    }

}
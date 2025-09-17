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
    /// Motivo de rectificación de factura.
    /// </summary>
    [Serializable()]
    [XmlType(Namespace = FacturaeNamespaces.NamespaceFE)]
    public enum ReasonDescription
    {

        /// <summary>
        /// Número de la factura
        /// </summary>
        [XmlEnum("Número de la factura")]
        NumeroDeLaFactura,

        /// <summary>
        /// Serie de la factura
        /// </summary>
        [XmlEnum("Serie de la factura")]
        SerieDeLaFactura,

        /// <summary>
        /// Fecha expedición
        /// </summary>
        [XmlEnum("Fecha expedición")]
        FechaExpedicion,

        /// <summary>
        /// Razón social emisor
        /// </summary>
        [XmlEnum("Nombre y apellidos/Razón Social-Emisor")]
        NombreyapellidosRazonSocialEmisor,

        /// <summary>
        /// Razón social receptor
        /// </summary>
        [XmlEnum("Nombre y apellidos/Razón Social-Receptor")]
        NombreyapellidosRazonSocialReceptor,

        /// <summary>
        /// Id fecscal emisor
        /// </summary>
        [XmlEnum("Identificación fiscal Emisor/obligado")]
        IdentificacionFiscalEmisorObligado,

        /// <summary>
        /// Id fiscal receptor
        /// </summary>
        [XmlEnum("Identificación fiscal Receptor")]
        IdentificacionFiscalReceptor,

        /// <summary>
        /// Domicilio Emisor/Obligado
        /// </summary>
        [XmlEnum("Domicilio Emisor/Obligado")]
        DomicilioEmisorObligado,

        /// <summary>
        /// Domicilio Receptor
        /// </summary>
        [XmlEnum("Domicilio Receptor")]
        DomicilioReceptor,

        /// <summary>
        /// Detalle Operación
        /// </summary>
        [XmlEnum("Detalle Operación")]
        DetalleOperacion,

        /// <summary>
        /// Porcentaje impositivo a aplicar
        /// </summary>
        [XmlEnum("Porcentaje impositivo a aplicar")]
        PorcentajeImpositivoaAplicar,

        /// <summary>
        /// Cuota tributaria a aplica
        /// </summary>
        [XmlEnum("Cuota tributaria a aplicar")]
        CuotaTributariaaAplicar,

        /// <summary>
        /// Fecha/Periodo a aplicar
        /// </summary>
        [XmlEnum("Fecha/Periodo a aplicar")]
        FechaPeriodoaaplicar,

        /// <summary>
        /// Clase de factura
        /// </summary>
        [XmlEnum("Clase de factura")]
        Clasedefactura,

        /// <summary>
        /// Literales legales
        /// </summary>
        [XmlEnum("Literales legales")]
        Literaleslegales,

        /// <summary>
        /// Base imponible
        /// </summary>
        [XmlEnum("Base imponible")]
        Baseimponible,

        /// <summary>
        /// Cálculo de cuotas repercutidas
        /// </summary>
        [XmlEnum("Cálculo de cuotas repercutidas")]
        Calculodecuotasrepercutidas,

        /// <summary>
        /// Cálculo de cuotas retenida
        /// </summary>
        [XmlEnum("Cálculo de cuotas retenidas")]
        Calculodecuotasretenidas,

        /// <summary>
        /// Base imponible modificada por devolución de envases / embalajes
        /// </summary>
        [XmlEnum("Base imponible modificada por devolución de envases / embalajes")]
        Baseimponiblemodificadapordevolucióndeenvasesembalajes,

        /// <summary>
        /// Base imponible modificada por descuentos y bonificaciones
        /// </summary>
        [XmlEnum("Base imponible modificada por descuentos y bonificaciones")]
        Baseimponiblemodificadapordescuentosybonificaciones,

        /// <summary>
        /// Base imponible modificada por resolución firme, judicial o administrativa
        /// </summary>
        [XmlEnum("Base imponible modificada por resolución firme, judicial o administrativa")]
        Baseimponiblemodificadaporresoluciónfirmejudicialoadministrativa,

        /// <summary>
        /// Base imponible modificada cuotas repercutidas no satisfechas. Auto de declaración de concurso
        /// </summary>
        [XmlEnum("Base imponible modificada cuotas repercutidas no satisfechas. Auto de declaración" +
            " de concurso")]
        BaseimponiblemodificadacuotasrepercutidasnosatisfechasAutodedeclaracióndeconcurso

    }

}
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
    /// Tipo de impuesto.
    /// </summary>
    [Serializable()]
    public enum TaxTypeCode
    {

        /// <summary>
        /// IVA: Impuesto sobre el valor añadido
        /// </summary>
        [XmlEnum("01")]
        IVA = 1,

        /// <summary>
        /// IPSI: Impuesto sobre la producción, los servicios y la importación
        /// </summary>
        [XmlEnum("02")]
        IPSI,

        /// <summary>
        /// IGIC: Impuesto general indirecto de Canarias
        /// </summary>
        [XmlEnum("03")]
        IGIC,

        /// <summary>
        /// IRPF: Impuesto sobre la Renta de las personas físicas
        /// </summary>
        [XmlEnum("04")]
        IRPF,

        /// <summary>
        /// Otro
        /// </summary>
        [XmlEnum("05")]
        OTHER,

        /// <summary>
        /// ITPAJD: Impuesto sobre transmisiones patrimoniales y actos jurídicos documentados
        /// </summary>
        [XmlEnum("06")]
        ITPAJD,

        /// <summary>
        /// IE: Impuestos especiales
        /// </summary>
        [XmlEnum("07")]
        IE,

        /// <summary>
        /// Ra: Renta aduanas
        /// </summary>
        [XmlEnum("08")]
        RA,

        /// <summary>
        /// IGTECM: Impuesto general sobre el tráfico de empresas que se aplica en Ceuta y Melilla
        /// </summary>
        [XmlEnum("09")]
        IGTECM,

        /// <summary>
        /// IECDPCAC: Impuesto especial sobre los combustibles derivados del petróleo en la Comunidad Autonoma Canaria
        /// </summary>
        [XmlEnum("10")]
        IECDPCAC,

        /// <summary>
        /// IIIMAB: Impuesto sobre las instalaciones que inciden sobre el medio ambiente en las Baleares
        /// </summary>
        [XmlEnum("11")]
        IIIMAB,

        /// <summary>
        /// ICIO: Impuesto sobre las construcciones, instalaciones y obras
        /// </summary>
        [XmlEnum("12")]
        ICIO,

        /// <summary>
        /// IMVDN: Impuesto municipal sobre las viviendas desocupadas en Navarra
        /// </summary>
        [XmlEnum("13")]
        IMVDN,

        /// <summary>
        /// IMSN: Impuesto municipal sobre solares en Navarra
        /// </summary>
        [XmlEnum("14")]
        IMSN,

        /// <summary>
        /// IMGSN: Impuesto municipal sobre gastos suntuarios en Navarra
        /// </summary>
        [XmlEnum("15")]
        IMGSN,

        /// <summary>
        /// IMPN: Impuesto municipal sobre publicidad en Navarra
        /// </summary>
        [XmlEnum("16")]
        IMPN,

        /// <summary>
        /// REIVA: Régimen especial de IVA para agencias de viajes
        /// </summary>
        [XmlEnum("17")]
        REIVA,

        /// <summary>
        /// REIGIC: Régimen especial de IGIC: para agencias de viajes
        /// </summary>
        [XmlEnum("18")]
        REIGIC,

        /// <summary>
        /// REIPSI: Régimen especial de IPSI para agencias de viajes
        /// </summary>
        [XmlEnum("19")]
        Item19,

        /// <summary>
        /// IPS: Impuestos sobre las primas de seguros
        /// </summary>
        [XmlEnum("20")]
        IPS,

        /// <summary>
        /// RLEA: Recargo destinado a financiar las funciones de liquidación de entidades aseguradoras
        /// </summary>
        [XmlEnum("21")]
        RLEA,

        /// <summary>
        /// IVPEE: Impuesto sobre el valor de la producción de la energía eléctrica
        /// </summary>
        [XmlEnum("22")]
        IVPEE,

        /// <summary>
        /// Impuesto sobre la producción de combustible nuclear gastado y residuos 
        /// radiactivos resultantes de la generación de energía nucleoeléctrica
        /// </summary>
        [XmlEnum("23")]
        NUCLEOELETRICAS,

        /// <summary>
        /// Impuesto sobre el almacenamiento de combustible nuclear gastado y residuos 
        /// radioactivos en instalaciones centralizadas
        /// </summary>
        [XmlEnum("24")]
        RADIOACTIVOS,

        /// <summary>
        /// IDEC: Impuesto sobre los Depósitos en las Entidades de Crédito
        /// </summary>
        [XmlEnum("25")]
        IDEC,

        /// <summary>
        /// Impuesto sobre las labores del tabaco en la Comunidad Autónoma de Canarias
        /// </summary>
        [XmlEnum("26")]
        ILTCAC,

        /// <summary>
        /// IGFEI: Impuesto sobre los Gases Fluorados de Efecto Invernadero
        /// </summary>
        [XmlEnum("27")]
        IGFEI,

        /// <summary>
        /// IRNR: Impuesto sobre la Renta de No Residentes
        /// </summary>
        [XmlEnum("28")]
        IRNR,

        /// <summary>
        /// Impuesto sobre Sociedades
        /// </summary>
        [XmlEnum("29")]
        IS

    }

}
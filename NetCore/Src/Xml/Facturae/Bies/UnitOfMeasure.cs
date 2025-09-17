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
    /// Unidades de medida
    /// </summary>
    [Serializable()]
    public enum UnitOfMeasure
    {

        /// <summary>
        /// Unidades
        /// </summary>
        [XmlEnum("01")]
        Units,

        /// <summary>
        /// Horas-HUR
        /// </summary>
        [XmlEnum("02")]
        HUR,

        /// <summary>
        /// Kilogramos-KGM
        /// </summary>
        [XmlEnum("03")]
        KGM,

        /// <summary>
        /// Litros-LTR
        /// </summary>
        [XmlEnum("04")]
        LTR,

        /// <summary>
        /// Otros
        /// </summary>
        [XmlEnum("05")]
        Other,

        /// <summary>
        /// Cajas-BX
        /// </summary>
        [XmlEnum("06")]
        BX,

        /// <summary>
        /// Bandejas-DS
        /// </summary>
        [XmlEnum("07")]
        DS,

        /// <summary>
        /// Barriles-BA
        /// </summary>
        [XmlEnum("08")]
        BA,

        /// <summary>
        /// Bidones-JY
        /// </summary>
        [XmlEnum("09")]
        JY,

        /// <summary>
        /// Bolsas-BG
        /// </summary>
        [XmlEnum("10")]
        BG,

        /// <summary>
        /// Bombonas-CO
        /// </summary>
        [XmlEnum("11")]
        CO,

        /// <summary>
        /// Botellas-BO
        /// </summary>
        [XmlEnum("12")]
        BO,

        /// <summary>
        /// Botes-CI
        /// </summary>
        [XmlEnum("13")]
        CI,

        /// <summary>
        /// Tetra Briks
        /// </summary>
        [XmlEnum("14")]
        TetraBrik,

        /// <summary>
        /// Centilitros-CLT
        /// </summary>
        [XmlEnum("15")]
        CLT,

        /// <summary>
        /// Centímetros-CMT
        /// </summary>
        [XmlEnum("16")]
        CMT,

        /// <summary>
        /// Cubos-BI
        /// </summary>
        [XmlEnum("17")]
        BI,

        /// <summary>
        /// Docenas
        /// </summary>
        [XmlEnum("18")]
        Dozens,

        /// <summary>
        /// Estuches-CS
        /// </summary>
        [XmlEnum("19")]
        CS,

        /// <summary>
        /// Garrafas-DJ
        /// </summary>
        [XmlEnum("20")]
        DJ,

        /// <summary>
        /// Gramos-GRM
        /// </summary>
        [XmlEnum("21")]
        GRM,

        /// <summary>
        /// Kilómetros-KMT
        /// </summary>
        [XmlEnum("22")]
        KMT,

        /// <summary>
        /// Latas-CA
        /// </summary>
        [XmlEnum("23")]
        CA,

        /// <summary>
        /// Manojos-BH
        /// </summary>
        [XmlEnum("24")]
        BH,

        /// <summary>
        /// Metros-MTR
        /// </summary>
        [XmlEnum("25")]
        MTR,

        /// <summary>
        /// Milímetros-MMT
        /// </summary>
        [XmlEnum("26")]
        MMT,

        /// <summary>
        /// 6-Packs
        /// </summary>
        [XmlEnum("27")]
        SixPacks,

        /// <summary>
        /// Paquetes-PK
        /// </summary>
        [XmlEnum("28")]
        PK,

        /// <summary>
        /// Raciones
        /// </summary>
        [XmlEnum("29")]
        Portions,

        /// <summary>
        /// Rollos-RO
        /// </summary>
        [XmlEnum("30")]
        RO,

        /// <summary>
        /// Sobres-EN
        /// </summary>
        [XmlEnum("31")]
        EN,

        /// <summary>
        /// Tarrinas-TB
        /// </summary>
        [XmlEnum("32")]
        TB,

        /// <summary>
        /// Metro cúbico-MTQ
        /// </summary>
        [XmlEnum("33")]
        MTQ,

        /// <summary>
        /// Segundo-SEC
        /// </summary>
        [XmlEnum("34")]
        SEC,

        /// <summary>
        /// Vatio-WTT
        /// </summary>
        [XmlEnum("35")]
        WTT,

        /// <summary>
        /// Kilovatio por hora-KWh
        /// </summary>
        [XmlEnum("36")]
        KWh

    }

}
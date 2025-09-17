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

namespace FACe.Xml.Facturae.Bies
{

    /// <summary>
    /// Código País. Código según la ISO 3166-1:2006 Alpha-3. 
    /// Al ser un domicilio ubicado en España siempre será ESP.
    /// </summary>
    [Serializable()]
    public enum Country
    {

        /// <summary>
        ///  Aruba
        /// </summary>
        ABW,

        /// <summary>
        ///  Afganistán
        /// </summary>
        AFG,

        /// <summary>
        ///  Angola
        /// </summary>
        AGO,

        /// <summary>
        ///  Anguila
        /// </summary>
        AIA,

        /// <summary>
        ///  Albania
        /// </summary>
        ALB,

        /// <summary>
        ///  Andorra
        /// </summary>
        AND,

        /// <summary>
        /// ???????
        /// </summary>
        ANT,

        /// <summary>
        ///  Emiratos Árabes Unidos
        /// </summary>
        ARE,

        /// <summary>
        ///  Argentina
        /// </summary>
        ARG,

        /// <summary>
        ///  Armenia
        /// </summary>
        ARM,

        /// <summary>
        ///  Samoa Americana
        /// </summary>
        ASM,

        /// <summary>
        ///  Antigua y Barbuda
        /// </summary>
        ATG,

        /// <summary>
        ///  Australia
        /// </summary>
        AUS,

        /// <summary>
        ///  Austria
        /// </summary>
        AUT,

        /// <summary>
        ///  Azerbaiyán
        /// </summary>
        AZE,

        /// <summary>
        ///  Burundi
        /// </summary>
        BDI,

        /// <summary>
        ///  Bélgica
        /// </summary>
        BEL,

        /// <summary>
        ///  Benín
        /// </summary>
        BEN,

        /// <summary>
        ///  Burkina Faso
        /// </summary>
        BFA,

        /// <summary>
        ///  Bangladés
        /// </summary>
        BGD,

        /// <summary>
        ///  Bulgaria
        /// </summary>
        BGR,

        /// <summary>
        ///  Baréin
        /// </summary>
        BHR,

        /// <summary>
        ///  Bahamas
        /// </summary>
        BHS,

        /// <summary>
        ///  Bosnia y Herzegovina
        /// </summary>
        BIH,

        /// <summary>
        ///  Bielorrusia
        /// </summary>
        BLR,

        /// <summary>
        ///  Belice
        /// </summary>
        BLZ,

        /// <summary>
        ///  Bermudas
        /// </summary>
        BMU,

        /// <summary>
        ///  Bolivia
        /// </summary>
        BOL,

        /// <summary>
        ///  Brasil
        /// </summary>
        BRA,

        /// <summary>
        ///  Barbados
        /// </summary>
        BRB,

        /// <summary>
        ///  Brunéi
        /// </summary>
        BRN,

        /// <summary>
        ///  Bután
        /// </summary>
        BTN,

        /// <summary>
        ///  Botsuana
        /// </summary>
        BWA,

        /// <summary>
        ///  República Centroafricana
        /// </summary>
        CAF,

        /// <summary>
        ///  Canadá
        /// </summary>
        CAN,

        /// <summary>
        ///  Suiza
        /// </summary>
        CHE,

        /// <summary>
        ///  Chile
        /// </summary>
        CHL,

        /// <summary>
        ///  China
        /// </summary>
        CHN,

        /// <summary>
        ///  Costa de Marfil
        /// </summary>
        CIV,

        /// <summary>
        ///  Camerún
        /// </summary>
        CMR,

        /// <summary>
        ///  República Democrática del Congo
        /// </summary>
        COD,

        /// <summary>
        ///  República del Congo
        /// </summary>
        COG,

        /// <summary>
        ///  Islas Cook
        /// </summary>
        COK,

        /// <summary>
        ///  Colombia
        /// </summary>
        COL,

        /// <summary>
        ///  Comoras
        /// </summary>
        COM,

        /// <summary>
        ///  Cabo Verde
        /// </summary>
        CPV,

        /// <summary>
        ///  Costa Rica
        /// </summary>
        CRI,

        /// <summary>
        ///  Cuba
        /// </summary>
        CUB,

        /// <summary>
        ///  Islas Caimán
        /// </summary>
        CYM,

        /// <summary>
        ///  Chipre
        /// </summary>
        CYP,

        /// <summary>
        ///  República Checa
        /// </summary>
        CZE,

        /// <summary>
        ///  Alemania
        /// </summary>
        DEU,

        /// <summary>
        ///  Yibuti
        /// </summary>
        DJI,

        /// <summary>
        ///  Dominica
        /// </summary>
        DMA,

        /// <summary>
        ///  Dinamarca
        /// </summary>
        DNK,

        /// <summary>
        ///  República Dominicana
        /// </summary>
        DOM,

        /// <summary>
        ///  Argelia
        /// </summary>
        DZA,

        /// <summary>
        ///  Ecuador
        /// </summary>
        ECU,

        /// <summary>
        ///  Egipto
        /// </summary>
        EGY,

        /// <summary>
        ///  Eritrea
        /// </summary>
        ERI,

        /// <summary>
        ///  República Árabe Saharaui Democrática
        /// </summary>
        ESH,

        /// <summary>
        ///  España
        /// </summary>
        ESP,

        /// <summary>
        ///  Estonia
        /// </summary>
        EST,

        /// <summary>
        ///  Etiopía
        /// </summary>
        ETH,

        /// <summary>
        ///  Finlandia
        /// </summary>
        FIN,

        /// <summary>
        ///  Fiyi
        /// </summary>
        FJI,

        /// <summary>
        ///  Islas Malvinas
        /// </summary>
        FLK,

        /// <summary>
        ///  Francia
        /// </summary>
        FRA,

        /// <summary>
        ///  Islas Feroe
        /// </summary>
        FRO,

        /// <summary>
        ///  Micronesia
        /// </summary>
        FSM,

        /// <summary>
        ///  Gabón
        /// </summary>
        GAB,

        /// <summary>
        ///  Reino Unido
        /// </summary>
        GBR,

        /// <summary>
        ///  Georgia
        /// </summary>
        GEO,

        /// <summary>
        ///  Guernsey
        /// </summary>
        GGY,

        /// <summary>
        ///  Ghana
        /// </summary>
        GHA,

        /// <summary>
        ///  Gibraltar
        /// </summary>
        GIB,

        /// <summary>
        ///  Guinea
        /// </summary>
        GIN,

        /// <summary>
        ///  Guadalupe
        /// </summary>
        GLP,

        /// <summary>
        ///  Gambia
        /// </summary>
        GMB,

        /// <summary>
        ///  Guinea-Bisáu
        /// </summary>
        GNB,

        /// <summary>
        ///  Guinea Ecuatorial
        /// </summary>
        GNQ,

        /// <summary>
        ///  Grecia
        /// </summary>
        GRC,

        /// <summary>
        ///  Granada
        /// </summary>
        GRD,

        /// <summary>
        ///  Groenlandia
        /// </summary>
        GRL,

        /// <summary>
        ///  Guatemala
        /// </summary>
        GTM,

        /// <summary>
        ///  Guayana Francesa
        /// </summary>
        GUF,

        /// <summary>
        ///  Guam
        /// </summary>
        GUM,

        /// <summary>
        ///  Guyana
        /// </summary>
        GUY,

        /// <summary>
        ///  Hong Kong
        /// </summary>
        HKG,

        /// <summary>
        ///  Honduras
        /// </summary>
        HND,

        /// <summary>
        ///  Croacia
        /// </summary>
        HRV,

        /// <summary>
        ///  Haití
        /// </summary>
        HTI,

        /// <summary>
        ///  Hungría
        /// </summary>
        HUN,

        /// <summary>
        ///  Indonesia
        /// </summary>
        IDN,

        /// <summary>
        ///  Isla de Man
        /// </summary>
        IMN,

        /// <summary>
        ///  India
        /// </summary>
        IND,

        /// <summary>
        ///  Irlanda
        /// </summary>
        IRL,

        /// <summary>
        ///  Irán
        /// </summary>
        IRN,

        /// <summary>
        ///  Irak
        /// </summary>
        IRQ,

        /// <summary>
        ///  Islandia
        /// </summary>
        ISL,

        /// <summary>
        ///  Israel
        /// </summary>
        ISR,

        /// <summary>
        ///  Italia
        /// </summary>
        ITA,

        /// <summary>
        ///  Jamaica
        /// </summary>
        JAM,

        /// <summary>
        ///  Jersey
        /// </summary>
        JEY,

        /// <summary>
        ///  Jordania
        /// </summary>
        JOR,

        /// <summary>
        ///  Japón
        /// </summary>
        JPN,

        /// <summary>
        ///  Kazajistán
        /// </summary>
        KAZ,

        /// <summary>
        ///  Kenia
        /// </summary>
        KEN,

        /// <summary>
        ///  Kirguistán
        /// </summary>
        KGZ,

        /// <summary>
        ///  Camboya
        /// </summary>
        KHM,

        /// <summary>
        ///  Kiribati
        /// </summary>
        KIR,

        /// <summary>
        ///  San Cristóbal y Nieves
        /// </summary>
        KNA,

        /// <summary>
        ///  Corea del Sur
        /// </summary>
        KOR,

        /// <summary>
        ///  Kuwait
        /// </summary>
        KWT,

        /// <summary>
        ///  Laos
        /// </summary>
        LAO,

        /// <summary>
        ///  Líbano
        /// </summary>
        LBN,

        /// <summary>
        ///  Liberia
        /// </summary>
        LBR,

        /// <summary>
        ///  Libia
        /// </summary>
        LBY,

        /// <summary>
        ///  Santa Lucía
        /// </summary>
        LCA,

        /// <summary>
        ///  Liechtenstein
        /// </summary>
        LIE,

        /// <summary>
        ///  Sri Lanka
        /// </summary>
        LKA,

        /// <summary>
        ///  Lesoto
        /// </summary>
        LSO,

        /// <summary>
        ///  Lituania
        /// </summary>
        LTU,

        /// <summary>
        ///  Luxemburgo
        /// </summary>
        LUX,

        /// <summary>
        ///  Letonia
        /// </summary>
        LVA,

        /// <summary>
        ///  Macao
        /// </summary>
        MAC,

        /// <summary>
        ///  Marruecos
        /// </summary>
        MAR,

        /// <summary>
        ///  Mónaco
        /// </summary>
        MCO,

        /// <summary>
        ///  Moldavia
        /// </summary>
        MDA,

        /// <summary>
        ///  Madagascar
        /// </summary>
        MDG,

        /// <summary>
        ///  Maldivas
        /// </summary>
        MDV,

        /// <summary>
        ///  México
        /// </summary>
        MEX,

        /// <summary>
        ///  Islas Marshall
        /// </summary>
        MHL,

        /// <summary>
        ///  Macedonia del Norte
        /// </summary>
        MKD,

        /// <summary>
        ///  Malí
        /// </summary>
        MLI,

        /// <summary>
        ///  Malta
        /// </summary>
        MLT,

        /// <summary>
        ///  Birmania
        /// </summary>
        MMR,

        /// <summary>
        /// Montenegro
        /// </summary>
        MNE,

        /// <summary>
        ///  Mongolia
        /// </summary>
        MNG,

        /// <summary>
        ///  Islas Marianas del Norte
        /// </summary>
        MNP,

        /// <summary>
        ///  Mozambique
        /// </summary>
        MOZ,

        /// <summary>
        ///  Mauritania
        /// </summary>
        MRT,

        /// <summary>
        ///  Montserrat
        /// </summary>
        MSR,

        /// <summary>
        ///  Martinica
        /// </summary>
        MTQ,

        /// <summary>
        ///  Mauricio
        /// </summary>
        MUS,

        /// <summary>
        ///  Malaui
        /// </summary>
        MWI,

        /// <summary>
        ///  Malasia
        /// </summary>
        MYS,

        /// <summary>
        ///  Mayotte
        /// </summary>
        MYT,

        /// <summary>
        ///  Namibia
        /// </summary>
        NAM,

        /// <summary>
        ///  Nueva Caledonia
        /// </summary>
        NCL,

        /// <summary>
        ///  Níger
        /// </summary>
        NER,

        /// <summary>
        ///  Norfolk
        /// </summary>
        NFK,

        /// <summary>
        ///  Nigeria
        /// </summary>
        NGA,

        /// <summary>
        ///  Nicaragua
        /// </summary>
        NIC,

        /// <summary>
        ///  Niue
        /// </summary>
        NIU,

        /// <summary>
        ///  Países Bajos
        /// </summary>
        NLD,

        /// <summary>
        ///  Noruega
        /// </summary>
        NOR,

        /// <summary>
        ///  Nepal
        /// </summary>
        NPL,

        /// <summary>
        ///  Nauru
        /// </summary>
        NRU,

        /// <summary>
        ///  Nueva Zelanda
        /// </summary>
        NZL,

        /// <summary>
        ///  Omán
        /// </summary>
        OMN,

        /// <summary>
        ///  Pakistán
        /// </summary>
        PAK,

        /// <summary>
        ///  Panamá
        /// </summary>
        PAN,

        /// <summary>
        ///  Islas Pitcairn
        /// </summary>
        PCN,

        /// <summary>
        ///  Perú
        /// </summary>
        PER,

        /// <summary>
        ///  Filipinas
        /// </summary>
        PHL,

        /// <summary>
        ///  Palaos
        /// </summary>
        PLW,

        /// <summary>
        ///  Papúa Nueva Guinea
        /// </summary>
        PNG,

        /// <summary>
        ///  Polonia
        /// </summary>
        POL,

        /// <summary>
        ///  Puerto Rico
        /// </summary>
        PRI,

        /// <summary>
        ///  Corea del Norte
        /// </summary>
        PRK,

        /// <summary>
        ///  Portugal
        /// </summary>
        PRT,

        /// <summary>
        ///  Paraguay
        /// </summary>
        PRY,

        /// <summary>
        ///  Palestina
        /// </summary>
        PSE,

        /// <summary>
        ///  Polinesia Francesa
        /// </summary>
        PYF,

        /// <summary>
        ///  Catar
        /// </summary>
        QAT,

        /// <summary>
        ///  Reunión
        /// </summary>
        REU,

        /// <summary>
        ///  Rumania
        /// </summary>
        ROU,

        /// <summary>
        ///  Rusia
        /// </summary>
        RUS,

        /// <summary>
        ///  Ruanda
        /// </summary>
        RWA,

        /// <summary>
        ///  Arabia Saudita
        /// </summary>
        SAU,

        /// <summary>
        ///  Sudán
        /// </summary>
        SDN,

        /// <summary>
        ///  Senegal
        /// </summary>
        SEN,

        /// <summary>
        ///  Singapur
        /// </summary>
        SGP,

        /// <summary>
        ///  Santa Elena, Ascensión y Tristán de Acuña
        /// </summary>
        SHN,

        /// <summary>
        ///  Svalbard y Jan Mayen
        /// </summary>
        SJM,

        /// <summary>
        ///  Islas Salomón
        /// </summary>
        SLB,

        /// <summary>
        ///  Sierra Leona
        /// </summary>
        SLE,

        /// <summary>
        ///  El Salvador
        /// </summary>
        SLV,

        /// <summary>
        ///  San Marino
        /// </summary>
        SMR,

        /// <summary>
        ///  Somalia
        /// </summary>
        SOM,

        /// <summary>
        ///  San Pedro y Miquelón
        /// </summary>
        SPM,

        /// <summary>
        ///  Serbia
        /// </summary>
        SRB,

        /// <summary>
        ///  Santo Tomé y Príncipe
        /// </summary>
        STP,

        /// <summary>
        ///  Surinam
        /// </summary>
        SUR,

        /// <summary>
        ///  Eslovaquia
        /// </summary>
        SVK,

        /// <summary>
        ///  Eslovenia
        /// </summary>
        SVN,

        /// <summary>
        ///  Suecia
        /// </summary>
        SWE,

        /// <summary>
        ///  Suazilandia
        /// </summary>
        SWZ,

        /// <summary>
        ///  Seychelles
        /// </summary>
        SYC,

        /// <summary>
        ///  Siria
        /// </summary>
        SYR,

        /// <summary>
        ///  Islas Turcas y Caicos
        /// </summary>
        TCA,

        /// <summary>
        ///  Chad
        /// </summary>
        TCD,

        /// <summary>
        ///  Togo
        /// </summary>
        TGO,

        /// <summary>
        ///  Tailandia
        /// </summary>
        THA,

        /// <summary>
        ///  Tayikistán
        /// </summary>
        TJK,

        /// <summary>
        ///  Tokelau
        /// </summary>
        TKL,

        /// <summary>
        ///  Turkmenistán
        /// </summary>
        TKM,

        /// <summary>
        ///  Timor Oriental
        /// </summary>
        TLS,

        /// <summary>
        ///  Tonga
        /// </summary>
        TON,

        /// <summary>
        ///  Trinidad y Tobago
        /// </summary>
        TTO,

        /// <summary>
        ///  Túnez
        /// </summary>
        TUN,

        /// <summary>
        ///  Turquía
        /// </summary>
        TUR,

        /// <summary>
        ///  Tuvalu
        /// </summary>
        TUV,

        /// <summary>
        ///  Taiwán (República de China)
        /// </summary>
        TWN,

        /// <summary>
        ///  Tanzania
        /// </summary>
        TZA,

        /// <summary>
        ///  Uganda
        /// </summary>
        UGA,

        /// <summary>
        ///  Ucrania
        /// </summary>
        UKR,

        /// <summary>
        ///  Uruguay
        /// </summary>
        URY,

        /// <summary>
        ///  Estados Unidos
        /// </summary>
        USA,

        /// <summary>
        ///  Uzbekistán
        /// </summary>
        UZB,

        /// <summary>
        ///  Vaticano, Ciudad del
        /// </summary>
        VAT,

        /// <summary>
        ///  San Vicente y las Granadinas
        /// </summary>
        VCT,

        /// <summary>
        ///  Venezuela
        /// </summary>
        VEN,

        /// <summary>
        ///  Islas Vírgenes Británicas
        /// </summary>
        VGB,

        /// <summary>
        ///  Islas Vírgenes de los Estados Unidos
        /// </summary>
        VIR,

        /// <summary>
        ///  Vietnam
        /// </summary>
        VNM,

        /// <summary>
        ///  Vanuatu
        /// </summary>
        VUT,

        /// <summary>
        ///  Wallis y Futuna
        /// </summary>
        WLF,

        /// <summary>
        ///  Samoa
        /// </summary>
        WSM,

        /// <summary>
        ///  Yemen
        /// </summary>
        YEM,

        /// <summary>
        ///  Sudáfrica
        /// </summary>
        ZAF,

        /// <summary>
        /// ???????
        /// </summary>
        ZAR,

        /// <summary>
        ///  Zambia
        /// </summary>
        ZMB,

        /// <summary>
        ///  Zimbabue
        /// </summary>
        ZWE

    }

}
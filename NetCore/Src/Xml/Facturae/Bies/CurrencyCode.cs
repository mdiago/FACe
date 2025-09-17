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
    /// Códigos ISO 4217:2001 Alpha-3 de moneda. 
    /// </summary>
    [Serializable()]
    public enum CurrencyCode
    {

        /// <summary>
        /// Afgani (Afganistán)
        /// </summary>
        AFN,

        /// <summary>
        /// Lek (Albania)
        /// </summary>
        ALL,

        /// <summary>
        /// Dram armenio (Armenia)
        /// </summary>
        AMD,

        /// <summary>
        /// Florín antillano neerlandés (Curazao, Sint Maarten)
        /// </summary>
        ANG,

        /// <summary>
        /// Kwanza (Angola)
        /// </summary>
        AOA,

        /// <summary>
        /// Peso argentino (Argentina)
        /// </summary>
        ARS,

        /// <summary>
        /// Dólar australiano (Australia, Bandera de Isla de Navidad Isla de Navidad,
        /// Bandera de Islas Cocos Islas Cocos, Bandera de Islas Heard y McDonald Islas
        /// Heard y McDonald, Bandera de Kiribati Kiribati, Bandera de Nauru Nauru, 
        /// Flag of Norfolk Island.svg Norfolk, Flag of Tuvalu.svg Tuvalu)
        /// </summary>
        AUD,

        /// <summary>
        /// Florín arubeño (Aruba)
        /// </summary>
        AWG,

        /// <summary>
        /// Manat azerbaiyano (Azerbaiyán)
        /// </summary>
        AZN,

        /// <summary>
        /// Dinar (Bosnia y Herzegovina)
        /// </summary>
        BAD,

        /// <summary>
        /// Dólar de Barbados (Barbados)
        /// </summary>
        BBD,

        /// <summary>
        /// Taka (Bangladés)
        /// </summary>
        BDT,

        /// <summary>
        /// Lev búlgaro (Bulgaria)
        /// </summary>
        BGN,

        /// <summary>
        /// Dinar bareiní (Dinar bareiní)
        /// </summary>
        BHD,

        /// <summary>
        /// Franco de Burundi (Burundi)
        /// </summary>
        BIF,

        /// <summary>
        /// Dólar bermudeño (Bermudas)
        /// </summary>
        BMD,

        /// <summary>
        /// Dólar de Brunéi (Brunéi)
        /// </summary>
        BND,

        /// <summary>
        /// Boliviano (Bolivia)
        /// </summary>
        BOB,

        /// <summary>
        /// Real brasileño (Brasil)
        /// </summary>
        BRL,

        /// <summary>
        /// Real brasileño (Brasil)
        /// </summary>
        BRR,

        /// <summary>
        /// Dólar bahameño (Bahamas)
        /// </summary>
        BSD,

        /// <summary>
        /// Pula (Botsuana)
        /// </summary>
        BWP,

        /// <summary>
        /// Rublo bielorruso (Bielorrusia )
        /// </summary>
        BYR,

        /// <summary>
        /// Dólar beliceño ( Belice)
        /// </summary>
        BZD,

        /// <summary>
        /// Dólar canadiense ( Canadá)
        /// </summary>
        CAD,

        /// <summary>
        /// Franco congoleño ( República Democrática del Congo)
        /// </summary>
        CDF,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        CDP,

        /// <summary>
        /// Franco suizo ( Liechtenstein,  Suiza)
        /// </summary>
        CHF,

        /// <summary>
        /// Peso chileno ( Chile)
        /// </summary>
        CLP,

        /// <summary>
        /// Yuan chino ( China)
        /// </summary>
        CNY,

        /// <summary>
        /// Peso colombiano ( Colombia)
        /// </summary>
        COP,

        /// <summary>
        /// Colón costarricense ( Costa Rica)
        /// </summary>
        CRC,

        /// <summary>
        /// Peso cubano ( Cuba)
        /// </summary>
        CUP,

        /// <summary>
        /// Escudo caboverdiano ( Cabo Verde)
        /// </summary>
        CVE,

        /// <summary>
        /// Corona checa ( República Checa)
        /// </summary>
        CZK,

        /// <summary>
        /// Franco yibutiano ( Yibuti)
        /// </summary>
        DJF,

        /// <summary>
        /// Corona danesa ( Dinamarca,  Groenlandia,  Islas Feroe)
        /// </summary>
        DKK,

        /// <summary>
        /// Peso dominicano ( República Dominicana)
        /// </summary>
        DOP,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        DRP,

        /// <summary>
        /// Dinar argelino ( Argelia)
        /// </summary>
        DZD,

        /// <summary>
        /// Corona Estonia (Estonia)
        /// </summary>
        EEK,

        /// <summary>
        /// Libra egipcia ( Egipto)
        /// </summary>
        EGP,

        /// <summary>
        /// Peseta (España)
        /// </summary>
        ESP,

        /// <summary>
        /// Birr etíope ( Etiopía)
        /// </summary>
        ETB,

        /// <summary>
        /// Euro ( Åland,  Alemania,  Andorra,  Austria,  Bélgica,  Chipre,  
        /// Ciudad del Vaticano,  Eslovaquia,  Eslovenia,  España,  Estonia,  Finlandia, 
        /// Francia,  Grecia,  Guadalupe,  Guayana Francesa,  Irlanda,  Italia,  Letonia, 
        /// Lituania,  Luxemburgo,  Malta,  Martinica,  Mayotte,  Mónaco, Montenegro, 
        /// Países Bajos,  Portugal,  Reunión,  San Bartolomé,  San Marino,  San Martín,  
        /// San Pedro y Miquelón,  Tierras Australes y Antárticas Francesas,  Unión Europea)
        /// </summary>
        EUR,

        /// <summary>
        /// Dólar fiyiano ( Fiyi)
        /// </summary>
        FJD,

        /// <summary>
        /// Libra malvinense ( Islas Malvinas)
        /// </summary>
        FKP,

        /// <summary>
        /// Libra esterlina ( Guernsey,  Isla de Man,  Jersey,  Reino Unido)
        /// </summary>
        GBP,

        /// <summary>
        /// Lari georgiano (Georgia)
        /// </summary>
        GEK,

        /// <summary>
        /// Cedi (Ghana)
        /// </summary>
        GHC,

        /// <summary>
        /// Libra de Gibraltar ( Gibraltar)
        /// </summary>
        GIP,

        /// <summary>
        /// Dalasi ( Gambia)
        /// </summary>
        GMD,

        /// <summary>
        /// Franco guineano ( Guinea)
        /// </summary>
        GNF,

        /// <summary>
        /// Quetzal ( Guatemala)
        /// </summary>
        GTQ,

        /// <summary>
        /// Escudo guineano (Guinea-Bisáu)
        /// </summary>
        GWP,

        /// <summary>
        /// Dólar guyanés ( Guyana)
        /// </summary>
        GYD,

        /// <summary>
        /// Dólar de Hong Kong ( Hong Kong)
        /// </summary>
        HKD,

        /// <summary>
        /// Lempira ( Honduras)
        /// </summary>
        HNL,

        /// <summary>
        /// Kuna ( Croacia)
        /// </summary>
        HRK,

        /// <summary>
        /// Gourde ( Haití)
        /// </summary>
        HTG,

        /// <summary>
        /// Forinto ( Hungría)
        /// </summary>
        HUF,

        /// <summary>
        /// Rupia indonesia ( Indonesia)
        /// </summary>
        IDR,

        /// <summary>
        /// Nuevo shéquel israelí ( Israel)
        /// </summary>
        ILS,

        /// <summary>
        /// Rupia india ( Bután,  India)
        /// </summary>
        INR,

        /// <summary>
        /// Dinar iraquí ( Irak)
        /// </summary>
        IQD,

        /// <summary>
        /// Rial iraní ( Irán)
        /// </summary>
        IRR,

        /// <summary>
        /// Corona islandesa ( Islandia)
        /// </summary>
        ISK,

        /// <summary>
        /// Dólar jamaiquino ( Jamaica)
        /// </summary>
        JMD,

        /// <summary>
        /// Dinar jordano ( Jordania)
        /// </summary>
        JOD,

        /// <summary>
        /// Yen ( Japón)
        /// </summary>
        JPY,

        /// <summary>
        /// Chelín keniano ( Kenia)
        /// </summary>
        KES,

        /// <summary>
        /// Som ( Kirguistán)
        /// </summary>
        KGS,

        /// <summary>
        /// Riel ( Camboya)
        /// </summary>
        KHR,

        /// <summary>
        /// Franco comorense ( Comoras)
        /// </summary>
        KMF,

        /// <summary>
        /// Won norcoreano ( Corea del Norte)
        /// </summary>
        KPW,

        /// <summary>
        /// Won ( Corea del Sur)
        /// </summary>
        KRW,

        /// <summary>
        /// Dinar kuwaití ( Kuwait)
        /// </summary>
        KWD,

        /// <summary>
        /// Dólar de las Islas Caimán ( Islas Caimán)
        /// </summary>
        KYD,

        /// <summary>
        /// Tenge ( Kazajistán)
        /// </summary>
        KZT,

        /// <summary>
        /// Kip ( Laos)
        /// </summary>
        LAK,

        /// <summary>
        /// Libra libanesa ( Líbano)
        /// </summary>
        LBP,

        /// <summary>
        /// Rupia de Sri Lanka ( Sri Lanka)
        /// </summary>
        LKR,

        /// <summary>
        /// Dólar liberiano ( Liberia)
        /// </summary>
        LRD,

        /// <summary>
        /// Loti ( Lesoto)
        /// </summary>
        LSL,

        /// <summary>
        /// Litas lituana (Lituania)
        /// </summary>
        LTL,

        /// <summary>
        /// Lats letón (Letonia )
        /// </summary>
        LVL,

        /// <summary>
        /// Dinar libio ( Libia)
        /// </summary>
        LYD,

        /// <summary>
        /// Dírham marroquí ( Marruecos,  República Árabe Saharaui Democrática)
        /// </summary>
        MAD,

        /// <summary>
        /// Leu moldavo ( Moldavia)
        /// </summary>
        MDL,

        /// <summary>
        /// Ariary malgache (Madagascar)
        /// </summary>
        MGF,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        MNC,

        /// <summary>
        /// Tugrik ( Mongolia)
        /// </summary>
        MNT,

        /// <summary>
        /// Pataca ( Macao)
        /// </summary>
        MOP,

        /// <summary>
        /// Uguiya (Mauritania)
        /// </summary>
        MRO,

        /// <summary>
        /// Rupia de Mauricio ( Mauricio)
        /// </summary>
        MUR,

        /// <summary>
        /// Rufiyaa ( Maldivas)
        /// </summary>
        MVR,

        /// <summary>
        /// Kwacha ( Malaui)
        /// </summary>
        MWK,

        /// <summary>
        /// Peso mexicano ( México)
        /// </summary>
        MXN,

        /// <summary>
        /// Ringgit malayo ( Malasia)
        /// </summary>
        MYR,

        /// <summary>
        /// Metical mozambiqueño (Mozambique)
        /// </summary>
        MZM,

        /// <summary>
        /// Naira ( Nigeria)
        /// </summary>
        NGN,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        NIC,

        /// <summary>
        /// Córdoba ( Nicaragua)
        /// </summary>
        NIO,

        /// <summary>
        /// Nuevo séquel (Israel )
        /// </summary>
        NIS,

        /// <summary>
        /// Corona noruega ( Isla Bouvet,  Noruega,  Svalbard y Jan Mayen)
        /// </summary>
        NOK,

        /// <summary>
        /// Rupia nepalí ( Nepal)
        /// </summary>
        NPR,

        /// <summary>
        /// Dólar neozelandés ( Islas Cook,  Islas Pitcairn,  Niue,  Nueva Zelanda,  Tokelau)
        /// </summary>
        NZD,

        /// <summary>
        /// Rial omaní ( Omán)
        /// </summary>
        OMR,

        /// <summary>
        /// Balboa ( Panamá)
        /// </summary>
        PAB,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        PEI,

        /// <summary>
        /// Sol ( Perú)
        /// </summary>
        PEN,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        PES,

        /// <summary>
        /// Kina ( Papúa Nueva Guinea)
        /// </summary>
        PGK,

        /// <summary>
        /// Peso filipino ( Filipinas)
        /// </summary>
        PHP,

        /// <summary>
        /// Rupia pakistaní ( Pakistán)
        /// </summary>
        PKR,

        /// <summary>
        /// Złoty ( Polonia)
        /// </summary>
        PLN,

        /// <summary>
        /// Guaraní ( Paraguay)
        /// </summary>
        PYG,

        /// <summary>
        /// Riyal qatarí ( Catar)
        /// </summary>
        QAR,

        /// <summary>
        /// Renminbi (República Popular China)
        /// </summary>
        RMB,

        /// <summary>
        /// Leu rumano ( Rumania)
        /// </summary>
        RON,

        /// <summary>
        /// Rublo ruso ( Rusia)
        /// </summary>
        RUB,

        /// <summary>
        /// Franco ruandés ( Ruanda)
        /// </summary>
        RWF,

        /// <summary>
        /// Riyal saudí ( Arabia Saudita)
        /// </summary>
        SAR,

        /// <summary>
        /// Dólar de las Islas Salomón ( Islas Salomón)
        /// </summary>
        SBD,

        /// <summary>
        /// Rupia seychelense ( Seychelles)
        /// </summary>
        SCR,

        /// <summary>
        /// Libra sudanesa (Sudán )
        /// </summary>
        SDP,

        /// <summary>
        /// Corona sueca ( Suecia)
        /// </summary>
        SEK,

        /// <summary>
        /// Dólar de Singapur ( Singapur)
        /// </summary>
        SGD,

        /// <summary>
        /// Libra de Santa Elena ( Santa Elena, Ascensión y Tristán de Acuña)
        /// </summary>
        SHP,

        /// <summary>
        /// Corona eslovaca (Eslovaquia )
        /// </summary>
        SKK,

        /// <summary>
        /// Leone ( Sierra Leona)
        /// </summary>
        SLL,

        /// <summary>
        /// Sol (Perú )
        /// </summary>
        SOL,

        /// <summary>
        /// Chelín somalí ( Somalia)
        /// </summary>
        SOS,

        /// <summary>
        /// Dólar surinamés ( Surinam)
        /// </summary>
        SRD,

        /// <summary>
        /// Dobra santotomense (Santo Tomé y Príncipe)
        /// </summary>
        STD,

        /// <summary>
        /// Colon Salvadoreño ( El Salvador)
        /// </summary>
        SVC,

        /// <summary>
        /// Libra siria ( Siria)
        /// </summary>
        SYP,

        /// <summary>
        /// Lilangeni ( Suazilandia)
        /// </summary>
        SZL,

        /// <summary>
        /// Baht ( Tailandia)
        /// </summary>
        THB,

        /// <summary>
        /// Somoni tayiko ( Tayikistán)
        /// </summary>
        TJS,

        /// <summary>
        /// Manat turcomano (Turkmenistán)
        /// </summary>
        TMM,

        /// <summary>
        /// Dinar tunecino ( Túnez)
        /// </summary>
        TND,

        /// <summary>
        /// Paʻanga ( Tonga)
        /// </summary>
        TOP,

        /// <summary>
        /// ?????? (??????)
        /// </summary>
        TPE,

        /// <summary>
        /// Lira turca ( Turquía)
        /// </summary>
        TRY,

        /// <summary>
        /// Dólar de Trinidad y Tobago ( Trinidad y Tobago)
        /// </summary>
        TTD,

        /// <summary>
        /// Nuevo dólar taiwanés ( República de China)
        /// </summary>
        TWD,

        /// <summary>
        /// Chelín tanzano ( Tanzania)
        /// </summary>
        TZS,

        /// <summary>
        /// Grivna ( Ucrania)
        /// </summary>
        UAH,

        /// <summary>
        /// Chelín ugandés (Uganda)
        /// </summary>
        UGS,

        /// <summary>
        /// Dólar estadounidense ( Caribe Neerlandés,  Ecuador,  El Salvador,  
        /// Estados Unidos,  Guam,  Haití,  Islas Marianas del Norte,  Islas Marshall, 
        /// Islas Turcas y Caicos,  Islas ultramarinas de Estados Unidos,  Islas Vírgenes Británicas, 
        /// Islas Vírgenes de los Estados Unidos,  Micronesia,  Palaos,  Panamá,  Puerto Rico, 
        /// Samoa Americana,  Territorio Británico del Océano Índico,  Timor Oriental)
        /// </summary>
        USD,

        /// <summary>
        /// Peso uruguayo (Uruguay)
        /// </summary>
        UYP,

        /// <summary>
        /// Peso uruguayo ( Uruguay)
        /// </summary>
        UYU,

        /// <summary>
        /// Bolívar (Venezuela)
        /// </summary>
        VEF,

        /// <summary>
        /// Dong vietnamita ( Vietnam)
        /// </summary>
        VND,

        /// <summary>
        /// Vatu ( Vanuatu)
        /// </summary>
        VUV,

        /// <summary>
        /// Tala ( Samoa)
        /// </summary>
        WST,

        /// <summary>
        /// Franco CFA de África Central ( Camerún,  Chad,  Gabón,  
        /// Guinea Ecuatorial,  República Centroafricana,  República del Congo)
        /// </summary>
        XAF,

        /// <summary>
        /// Dólar del Caribe Oriental ( Anguila,  Antigua y Barbuda,  Dominica, 
        /// Granada,  Montserrat,  San Cristóbal y Nieves,  San Vicente y las Granadinas,  
        /// Santa Lucía)
        /// </summary>
        XCD,

        /// <summary>
        /// Franco CFA de África Occidental ( Benín,  Burkina Faso,  Costa de Marfil,  
        /// Guinea-Bisáu,  Malí,  Níger,  Senegal,  Togo)
        /// </summary>
        XOF,

        /// <summary>
        /// Rial yemení ( Yemen)
        /// </summary>
        YER,

        /// <summary>
        /// Rand ( Lesoto,  Namibia,  Sudáfrica)
        /// </summary>
        ZAR,

        /// <summary>
        /// Kwacha zambiano (Zambia)
        /// </summary>
        ZMK,

        /// <summary>
        /// Dólar zimbabuense (Zimbabue )
        /// </summary>
        ZWD

    }

}
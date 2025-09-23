﻿/* 
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
    develop commercial activities involving the VeriFactu software without
    disclosing the source code of your own applications.
    These activities include: offering paid services to customers as an ASP,
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using System;
using System.Runtime.InteropServices;

namespace FACe
{

    #region Interfaz COM

    /// <summary>
    /// Interfaz COM para la clase TaxItem.
    /// </summary>
    [Guid("F1D58936-58EE-4694-A856-B3AB9672DFB8")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFeTaxItem
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Indicador que determina si la línea de impuestos
        /// contiene información de impuestos repecutidos 
        /// (IVA, IGIC, IPSI...) o de retenciones.
        /// Para Facturae:
        /// <para>'TO': TaxesOutputs</para>
        /// <para>'TW': TaxesWithheld</para>
        /// </summary>       
        string TaxClass { get; set; }

        /// <summary>
        /// <para>Impuesto de aplicación. Si no se informa este campo
        /// se entenderá que el impuesto de aplicación es el IVA.
        /// Este campo es necesario porque contribuye a completar el
        /// detalle de la tipología de la factura.</para>
        /// <para>Alfanumérico (1) L1:</para>
        /// <para>01: Impuesto sobre el Valor Añadido (IVA)</para>
        /// <para>02: Impuesto sobre la Producción, los Servicios y la Importación (IPSI) de Ceuta y Melilla</para>
        /// <para>03: Impuesto General Indirecto Canario (IGIC)</para>
        /// <para>05: Otros</para>
        /// </summary>
        string Tax { get; set; }

        /// <summary>
        /// Esquema impositivo.
        /// </summary>        
        string TaxScheme { get; set; }

        /// <summary>
        /// Identificador la categoría de impuestos.
        /// </summary>        
        string TaxType { get; set; }

        /// <summary>
        /// <para>Campo que especifica la causa de exención.</para>
        /// <para>Alfanumérico(2). L10.</para>
        /// </summary>
        string TaxException { get; set; }

        /// <summary>
        /// Base imponible.
        /// </summary>
        float TaxBase { get; set; }

        /// <summary>
        /// Tipo impositivo.
        /// </summary>
        float TaxRate { get; set; }

        /// <summary>
        /// Importe cuota impuesto.
        /// </summary>
        float TaxAmount { get; set; }

        /// <summary>
        /// Tipo impositivo recargo.
        /// </summary>
        float TaxRateSurcharge { get; set; }

        /// <summary>
        /// Importe cuota recargo impuesto.
        /// </summary>
        float TaxAmountSurcharge { get; set; }

        #endregion

    }

    #endregion

    #region Clase COM

    /// <summary>
    /// Representa una línea de impuestos.
    /// </summary>
    [Guid("CAB9D9D7-338A-4EBA-AAE4-A8365D3E1411")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("FACe.FeTaxItem")]
    public class FeTaxItem : IFeTaxItem
    {

        #region Construtores de Instancia

        /// <summary>
        /// Constructor. Para COM necesitamos un constructor
        /// sin parametros.
        /// </summary>
        public FeTaxItem()
        {
        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Indicador que determina si la línea de impuestos
        /// contiene información de impuestos repecutidos 
        /// (IVA, IGIC, IPSI...) o de retenciones.
        /// Para Facturae:
        /// <para>'TO': TaxesOutputs</para>
        /// <para>'TW': TaxesWithheld</para>
        /// </summary>       
        public string TaxClass { get; set; }

        /// <summary>
        /// <para>Impuesto de aplicación. Si no se informa este campo
        /// se entenderá que el impuesto de aplicación es el IVA.
        /// Este campo es necesario porque contribuye a completar el
        /// detalle de la tipología de la factura.</para>
        /// <para>Alfanumérico (1) L1:</para>
        /// <para>01: Impuesto sobre el Valor Añadido (IVA)</para>
        /// <para>02: Impuesto sobre la Producción, los Servicios y la Importación (IPSI) de Ceuta y Melilla</para>
        /// <para>03: Impuesto General Indirecto Canario (IGIC)</para>
        /// <para>05: Otros</para>
        /// </summary>
        public string Tax { get; set; }

        /// <summary>
        /// Esquema impositivo.
        /// </summary>        
        public string TaxScheme { get; set; }

        /// <summary>
        /// Identificador la categoría de impuestos.
        /// </summary>        
        public string TaxType { get; set; }

        /// <summary>
        /// <para>Campo que especifica la causa de exención.</para>
        /// <para>Alfanumérico(2). L10.</para>
        /// </summary>
        public string TaxException { get; set; }

        /// <summary>
        /// Base imponible.
        /// </summary>
        public float TaxBase { get; set; }

        /// <summary>
        /// Tipo impositivo.
        /// </summary>
        public float TaxRate { get; set; }

        /// <summary>
        /// Importe cuota impuesto.
        /// </summary>
        public float TaxAmount { get; set; }

        /// <summary>
        /// Tipo impositivo recargo.
        /// </summary>
        public float TaxRateSurcharge { get; set; }

        /// <summary>
        /// Importe cuota recargo impuesto.
        /// </summary>
        public float TaxAmountSurcharge { get; set; }

        #endregion

    }

    #endregion

}

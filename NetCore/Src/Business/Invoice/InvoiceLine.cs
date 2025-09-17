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
    serving FACe XML data on the fly in a web application, shipping FACe
    with a closed source product.
    
    For more information, please contact Irene Solutions SL. at this
    address: info@irenesolutions.com
 */

using FACe.Net.Rest.Json;

namespace FACe.Business.Invoice
{

    /// <summary>
    /// Representa una factura rectificada.
    /// </summary>
    public class InvoiceLine : JsonSerializable
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Número de posición de la línea de factura.
        /// Equivale a 'SequenceNumber' de Facturae.
        /// </summary>
        public int ItemPosition { get; set; }

        /// <summary>
        /// Referencia del comprador para esta línea:
        /// Número de pedido, contrato...
        /// </summary>
        public string BuyerReference { get; set; }

        /// <summary>
        /// Identificador del item. Código de artículo,
        /// código de servicio...
        /// </summary>        
        public string ItemID { get; set; }

        /// <summary>
        /// Nombre del item. Nombre o descripción
        /// del artítulo o servicio.
        /// </summary>        
        public string ItemName { get; set; }

        /// <summary>
        /// Cantidad.
        /// </summary>        
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unidad de medida.
        /// </summary>        
        public string UnitOfMeasure { get; set; }

        /// <summary>
        /// Precio bruto antes de descuentos.
        /// </summary>        
        public decimal GrossPrice { get; set; }

        /// <summary>
        /// Precio neto después de descuentos.
        /// </summary>        
        public decimal NetPrice { get; set; }

        /// <summary>
        /// Importe bruto antes de descuentos. Es igual
        /// al producto de GrossPrice por Quantity. 
        /// </summary>        
        public decimal GrossAmount { get; set; }

        /// <summary>
        /// Importe neto después de descuentos. Igual a
        /// NetPrice por Quantity.
        /// </summary>        
        public decimal NetAmount { get; set; }

        /// <summary>
        /// Porcentaje de descuento aplicado.
        /// </summary>
        public decimal DiscountRate { get; set; }

        /// <summary>
        /// Importe de descuento aplicado.
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Tipo impuestos soportados / repercutidos.
        /// </summary>        
        public decimal TaxesOutputRate { get; set; }

        /// <summary>
        /// Total base impuestos soportados / repercutidos.
        /// </summary>        
        public decimal TaxesOutputBase { get; set; }

        /// <summary>
        /// Total impuestos soportados / repercutidos.
        /// </summary>        
        public decimal TaxesOutputAmount { get; set; }

        /// <summary>
        /// Tipo impositivo recargo.
        /// </summary>
        public decimal TaxesOutputRateSurcharge { get; set; }

        /// <summary>
        /// Total recargo impuestos soportados / repercutidos.
        /// </summary>        
        public decimal TaxesOutputAmounSurcharget { get; set; }

        /// <summary>
        /// <para>Impuesto de aplicación. Si no se informa este campo
        /// se entenderá que el impuesto de aplicación es el IVA.
        /// Este campo es necesario porque contribuye a completar el
        /// detalle de la tipología de la factura.</para>
        /// <para>Alfanumérico (1) L1:</para>
        /// <para>01: Impuesto sobre el Valor Añadido (IVA)</para>
        /// <para>02: Impuesto sobre la Producción, los Servicios y la Importación (IPSI) de Ceuta y Melilla</para>
        /// <para>03: Impuesto General Indirecto Canario (IGIC)</para>
        /// <para>04: IRPF</para>
        /// <para>05: Otros</para>
        /// </summary>
        public string TaxesOutputTax { get; set; }

        /// <summary>
        /// Tipo impuestos retenidos.
        /// </summary>        
        public decimal TaxesWithheldRate { get; set; }

        /// <summary>
        /// Total base impuestos retenidos.
        /// </summary>        
        public decimal TaxesWithheldBase { get; set; }

        /// <summary>
        /// Total impuestos retenidos.
        /// </summary>        
        public decimal TaxesWithheldAmount { get; set; }

        /// <summary>
        /// <para>Impuesto de aplicación. Si no se informa este campo
        /// se entenderá que el impuesto de aplicación es el IVA.
        /// Este campo es necesario porque contribuye a completar el
        /// detalle de la tipología de la factura.</para>
        /// <para>Alfanumérico (1) L1:</para>
        /// <para>01: Impuesto sobre el Valor Añadido (IVA)</para>
        /// <para>02: Impuesto sobre la Producción, los Servicios y la Importación (IPSI) de Ceuta y Melilla</para>
        /// <para>03: Impuesto General Indirecto Canario (IGIC)</para>
        /// <para>04: IRPF</para>
        /// <para>05: Otros</para>
        /// </summary>
        public string TaxesWithheldTax { get; set; }


        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns>Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{ItemName}, {NetAmount}";

        }

        #endregion

    }

}
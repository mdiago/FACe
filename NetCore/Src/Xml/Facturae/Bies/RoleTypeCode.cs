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
    /// Tipo rol. Indica la función de un Punto Operacional (P.O.) definido como Centro/Departamento. 
    /// Estas funciones son: "Receptor" - Centro del NIF receptor destinatario de la factura. 
    /// <para>"Pagador" - Centro del NIF receptor responsable de pagar la factura.</para>
    /// <para>"Comprador" - Centro del NIF receptor que emitió el pedido.</para>
    /// <para>"Cobrador" - Centro del NIF emisor responsable de gestionar el cobro.</para>
    /// <para>"Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O. buzón es 
    /// compartido por varias empresas clientes con diferentes NIF.s y es necesario 
    /// diferenciar el receptor del mensaje (buzón común) del lugar donde debe depositarse 
    /// (empresa destinataria).</para>
    /// </summary>
    [Serializable()]
    public enum RoleTypeCode
    {

        /// <summary>
        /// Fiscal
        /// </summary>
        [XmlEnum("01")]
        Fiscal,

        /// <summary>
        /// Receptor
        /// </summary>
        [XmlEnum("02")]
        Receiver,

        /// <summary>
        /// Pagador
        /// </summary>
        [XmlEnum("03")]
        Payer,

        /// <summary>
        /// Comprador
        /// </summary>
        [XmlEnum("04")]
        Buyer,

        /// <summary>
        /// Cobrador
        /// </summary>
        [XmlEnum("05")]
        Collector,

        /// <summary>
        /// Vendedor
        /// </summary>
        [XmlEnum("06")]
        Seller,

        /// <summary>
        /// Receptor del pago
        /// </summary>
        [XmlEnum("07")]
        Item07,

        /// <summary>
        /// Receptor del cobro
        /// </summary>
        [XmlEnum("08")]
        CollectioReceiver,

        /// <summary>
        /// Emisor
        /// </summary>
        [XmlEnum("09")]
        Issuer

    }

}
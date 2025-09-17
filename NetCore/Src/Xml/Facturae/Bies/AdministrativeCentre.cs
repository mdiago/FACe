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
    /// Departamento emisor de la factura.
    /// </summary>
    [Serializable()]
    public class AdministrativeCentre
    {

        #region Propiedades Públicas Estáticas

        /// <summary>
        /// Número del Departamento Emisor.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CentreCode { get; set; }


        /// <summary>
        /// Tipo rol. Indica la función de un Punto Operacional (P.O.)
        /// definido como Centro/Departamento. Estas funciones son: 
        /// <para>"Receptor" - Centro del NIF receptor destinatario de la factura. </para>
        /// <para>"Pagador" - Centro del NIF receptor responsable de pagar la factura.</para> 
        /// <para>"Comprador" - Centro del NIF receptor que emitió el pedido.</para> 
        /// <para>"Cobrador" - Centro del NIF emisor responsable de gestionar el cobro.</para> 
        /// <para>"Fiscal" - Centro del NIF receptor de las facturas, cuando un P.O.
        /// buzón es compartido por varias empresas clientes con diferentes NIF.s
        /// y es necesario diferenciar el receptor del mensaje (buzón común) del
        /// lugar donde debe depositarse (empresa destinataria).</para>
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public RoleTypeCode RoleTypeCode { get; set; }

        /// <summary>
        /// Indica is se ha especificado el tipo de rol.
        /// </summary>
        [XmlIgnore()]
        public bool RoleTypeCodeSpecified { get; set; }

        /// <summary>
        /// Nombre de la persona responsable o de relación del centro.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Name { get; set; }

        /// <summary>
        /// Primer apellido de la persona responsable o de relación del centro.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FirstSurname { get; set; }

        /// <summary>
        /// Segundo apellido de la persona responsable o de relación del centro.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SecondSurname { get; set; }

        /// <summary>
        /// Dirección nacional, en España o
        /// Dirección en el extranjero.
        /// </summary>
        [XmlElement("AddressInSpain", typeof(Address), Form = XmlSchemaForm.Unqualified)]
        [XmlElement("OverseasAddress", typeof(OverseasAddress), Form = XmlSchemaForm.Unqualified)]
        public object Address { get; set; }

        /// <summary>
        /// Datos de contacto.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public ContactDetails ContactDetails { get; set; }

        /// <summary>
        /// GLN Físico. Identificación del punto de conexión a la VAN EDI 
        /// (Global Location Number). Código de barras de 13 posiciones estándar. 
        /// Valores registrados por AECOC. Recoge el código de País (2p) España es
        /// "84" + Empresa (5p) + los restantes - el último es el producto + dígito de control.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string PhysicalGLN { get; set; }

        /// <summary>
        /// Punto Lógico Operacional. Código identificativo de la Empresa. Código de barras
        /// de 13 posiciones estándar. Valores registrados por AECOC. Recoge el código de 
        /// País (2p) España es "84" + Empresa (5p) + los restantes - el último es el 
        /// producto + dígito de control.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string LogicalOperationalPoint { get; set; }

        /// <summary>
        /// Válido a partir de la versión 3.2.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CentreDescription { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Representación textual de la instancia de AdministrativeCentreType.
        /// </summary>
        /// <returns>Representación textual de la instancia de AdministrativeCentreType.</returns>
        public override string ToString()
        {
            return $"{Name}";
        }

        #endregion

    }

}
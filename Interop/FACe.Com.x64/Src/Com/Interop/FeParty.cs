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
    /// Interfaz COM para la clase Party.
    /// </summary>
    [Guid("46CF6B37-E8EB-4B98-90F7-5998F8F0E9EF")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFeParty
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>'P':Persona física.</para>
        /// <para>'J':Persona jurídica.</para>
        /// </summary>
        string PartyType { get; set; }

        /// <summary>
        /// <para>'SL': Seller.</para>
        /// <para>'BY': Buyer.</para>
        /// <para>'OC': Oficina contable.</para>
        /// <para>'OG': Organo gestor.</para>
        /// <para>'UT': Unidad tramitadora.</para>
        /// <para>'OT': Otros.</para>
        /// </summary>
        string PartyRole { get; set; }

        /// <summary>
        /// Identificador del interlocutor de negocio.
        /// Texto (20).
        /// </summary>
        string PartyID { get; set; }

        /// <summary>
        /// Nombre asignado al interlocutor.
        /// Texto (150).
        /// </summary>        
        string PartyName { get; set; }

        /// <summary>
        /// Código de identificación fiscal.
        /// Texto (30).
        /// </summary>        
        string TaxID { get; set; }

        /// <summary>
        /// Dirección.
        /// Texto (120).
        /// </summary>        
        string Address { get; set; }

        /// <summary>
        /// Población.
        /// Texto (70).
        /// </summary>        
        string City { get; set; }

        /// <summary>
        /// Código postal.
        /// Texto (120).
        /// </summary>        
        string PostalCode { get; set; }

        /// <summary>
        /// Código región: Ej. provincia.
        /// Texto (120).
        /// </summary>        
        string Region { get; set; }

        /// <summary>
        /// Código país ISO-3166 (EJ. ES).
        /// Texto (2).
        /// </summary>        
        string CountryID { get; set; }

        /// <summary>
        /// Dirección de correo principal.
        /// Texto (150).
        /// </summary>
        string Mail { get; set; }

        /// <summary>
        /// Número de teléfono movil.
        /// Texto (20).
        /// </summary>
        string Mobile { get; set; }

        /// <summary>
        /// Número de teléfono fijo.
        /// Texto (20).
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// Página web.
        /// Texto (200).
        /// </summary>
        string WebAddress { get; set; }

        #endregion

    }

    #endregion

    #region Clase COM

    /// <summary>
    /// Representa un interlocutor de negocio.
    /// </summary>
    [Guid("C48697A7-6FFE-42DC-AF9C-37790C375069")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("FACe.FeParty")]
    public class FeParty : IFeParty
    {

        #region Construtores de Instancia

        /// <summary>
        /// Constructor. Para COM necesitamos un constructor
        /// sin parametros.
        /// </summary>
        public FeParty()
        {
        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>'P':Persona física.</para>
        /// <para>'J':Persona jurídica.</para>
        /// </summary>
        public string PartyType { get; set; }

        /// <summary>
        /// <para>'SL': Seller.</para>
        /// <para>'BY': Buyer.</para>
        /// <para>'OC': Oficina contable.</para>
        /// <para>'OG': Organo gestor.</para>
        /// <para>'UT': Unidad tramitadora.</para>
        /// <para>'OT': Otros.</para>
        /// </summary>
        public string PartyRole { get; set; }

        /// <summary>
        /// Identificador del interlocutor de negocio.
        /// Texto (20).
        /// </summary>
        public string PartyID { get; set; }

        /// <summary>
        /// Nombre asignado al interlocutor.
        /// Texto (150).
        /// </summary>        
        public string PartyName { get; set; }

        /// <summary>
        /// Código de identificación fiscal.
        /// Texto (30).
        /// </summary>        
        public string TaxID { get; set; }

        /// <summary>
        /// Dirección.
        /// Texto (120).
        /// </summary>        
        public string Address { get; set; }

        /// <summary>
        /// Población.
        /// Texto (70).
        /// </summary>        
        public string City { get; set; }

        /// <summary>
        /// Código postal.
        /// Texto (120).
        /// </summary>        
        public string PostalCode { get; set; }

        /// <summary>
        /// Código región: Ej. provincia.
        /// Texto (120).
        /// </summary>        
        public string Region { get; set; }

        /// <summary>
        /// Código país ISO-3166 (EJ. ES).
        /// Texto (2).
        /// </summary>        
        public string CountryID { get; set; }

        /// <summary>
        /// Dirección de correo principal.
        /// Texto (150).
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Número de teléfono movil.
        /// Texto (20).
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Número de teléfono fijo.
        /// Texto (20).
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Página web.
        /// Texto (200).
        /// </summary>
        public string WebAddress { get; set; }

        #endregion

    }

    #endregion

}

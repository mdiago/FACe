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

using FACe.Common;
using FACe.Config;
using FACe.Net.Rest.Json.Kivu;
using System;
using System.Reflection;

namespace FACe.Net.Rest
{

    /// <summary>
    /// API Ct.
    /// </summary>
    internal class Ct : JsonSerializableKivu
    {

        #region Variables Privadas Estáticas

        /// <summary>
        /// Ensamblado en ejecución.
        /// </summary>
        static Assembly _Assembly = Assembly.GetExecutingAssembly();

        /// <summary>
        /// Atributos personalizados del ensamblado.
        /// </summary>
        static object[] _Atributtes = Assembly.GetExecutingAssembly().GetCustomAttributes(false);

        #endregion

        #region Propiedades Privadas Estáticas

        /// <summary>
        /// Titulo del ensamblado.
        /// </summary>
        internal static string AssemblyTitle { get; private set; }

        /// <summary>
        /// Versión del ensamblado.
        /// </summary>
        internal static string AssemblyVersion => $"{_Assembly.GetName().Version}";

        /// <summary>
        /// Descripción del ensamblado.
        /// </summary>
        internal static string AssemblyDescription { get; private set; }

        /// <summary>
        /// Producto del ensamblado.
        /// </summary>
        internal static string AssemblyProduct { get; private set; }

        /// <summary>
        /// Copyright del ensamblado.
        /// </summary>
        internal static string AssemblyCopyright { get; private set; }

        /// <summary>
        /// Empresa del ensamblado.
        /// </summary>
        internal static string AssemblyCompany { get; private set; }

        /// <summary>
        /// Sistema operativo: plataforma y versión.
        /// </summary>
        internal static string OSVersion { get; private set; }

        #endregion

        #region Construtores Estáticos

        /// <summary>
        /// Constructor.
        /// </summary>
        static Ct()
        {

            AssemblyTitle = GetAttValue("Title");
            AssemblyDescription = GetAttValue("Description");
            AssemblyProduct = GetAttValue("Product");
            AssemblyCopyright = GetAttValue("Copyright");
            AssemblyCompany = GetAttValue("Company");

            try
            {

                OSVersion = Environment.OSVersion.VersionString;

            }
            catch (Exception ex)
            {

                Utils.Log($"{ex}");

            }

        }

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        public Ct()
        {

            Title = AssemblyTitle;
            VersionID = AssemblyVersion;
            Description = AssemblyDescription;
            Product = AssemblyProduct;
            InstallationNumber = Settings.Current.InstallationNumber;
            Copyright = AssemblyCopyright;
            Company = AssemblyCompany;
            OS = OSVersion;

        }

        #endregion

        #region Métodos Privados Estáticos

        /// <summary>
        /// Obtiene el valor del atributo.
        /// </summary>
        /// <param name="attName">Nombre del atributo.</param>
        /// <returns>Valkor</returns>
        private static string GetAttValue(string attName)
        {

            var typeName = $"System.Reflection.Assembly{attName}Attribute";
            var type = Type.GetType(typeName);
            var pInf = type.GetProperty(attName);

            if (type == null)
                return null;

            foreach (var att in _Atributtes)
                if (att.GetType().Equals(type))
                    return $"{pInf.GetValue(att)}";

            return null;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// Titulo del ensamblado.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Versión del ensamblado.
        /// </summary>
        public string VersionID { get; set; }

        /// <summary>
        /// Descripción del ensamblado.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Producto del ensamblado.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// <para>Número de instalación del sistema informático de facturación (SIF) utilizado.
        /// Deberá distinguirlo de otros posibles SIF utilizados para realizar la facturación del
        /// obligado a expedir facturas, es decir, de otras posibles instalaciones de SIF pasadas,
        /// presentes o futuras utilizadas para realizar la facturación del obligado a expedir
        /// facturas, incluso aunque en dichas instalaciones se emplee el mismo SIF de un productor.</para>
        /// <para>Alfanumérico(100).</para>
        /// </summary>
        public string InstallationNumber { get; set; }

        /// <summary>
        /// Copyright del ensamblado.
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Empresa del ensamblado.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Sistema operativo: plataforma y versión.
        /// </summary>
        public string OS { get; set; }

        /// <summary>
        /// Registro.
        /// </summary>
        public string Log { get; set; }

        #endregion

    }

}
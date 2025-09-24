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

using FACe.Config;
using System;
using System.Runtime.InteropServices;

namespace FACe
{

    #region Interfaz COM

    /// <summary>
    /// Interfaz COM para la clase RectificationItem.
    /// </summary>
    [Guid("C5A82206-7449-45B6-A3A8-39917B2382F9")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFeSettings
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Identificación de la versión actual del esquema o
        /// estructura de información utilizada para la generación y
        /// conservación / remisión de los registros de facturación.
        /// Este campo forma parte del detalle de las circunstancias
        /// de generación de los registros de facturación.</para>
        /// <para>Alfanumérico(3) L15:</para>
        /// <para>1.0: Versión actual (1.0) del esquema utilizado </para>
        /// </summary>
        string IDVersion { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como bandeja de entrada.
        /// En este directorio se almacenarán todos los mensajes
        /// recibidos de la AEAT mediante VERI*FACTU.
        /// </summary>
        string InboxPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como bandeja de salida.
        /// En este directorio se almacenará una copia de cualquier
        /// envío realizado a la AEAT mediante el VERI*FACTU.
        /// </summary>
        string OutboxPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como almacén
        /// de las distintas cadenas de bloques por emisor.
        /// </summary>
        string BlockchainPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará almacenamiento
        /// de las facturas emitidas por emisor.
        /// </summary>
        string InvoicePath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará almacenamiento
        /// del registro de mensajes del sistema.
        /// </summary>
        string LogPath { get; set; }

        /// <summary>
        /// Número de serie del certificado a utilizar. Mediante este número
        /// de serie se selecciona del almacén de certificados de windows
        /// el certificado con el que realizar las comunicaciones.
        /// </summary>
        string CertificateSerial { get; set; }

        /// <summary>
        /// Hash o Huella digital del certificado a utilizar. Mediante esta
        /// huella digital se selecciona del almacén de certificados de
        /// windows el certificado con el que realizar las comunicaciones.
        /// </summary>
        string CertificateThumbprint { get; set; }

        /// <summary>
        /// Ruta al archivo del certificado a utilizar.
        /// Sólo se utiliza en los certificados cargados desde el sistema de archivos. 
        /// </summary>
        string CertificatePath { get; set; }

        /// <summary>
        /// Password del certificado. Este valor sólo es necesario si
        /// tenemos establecido el valor para 'CertificatePath' y el certificado
        /// tiene clave de acceso. Sólo se utiliza en los certificados
        /// cargados desde el sistema de archivos.
        /// </summary>
        string CertificatePassword { get; set; }

        /// <summary>
        /// EndPoint del web service de la AEAT para envío registros alta y anulación.
        /// </summary>
        string FACeEndPointPrefix { get; set; }

        /// <summary>
        /// Indica si está activado el log de mensajes
        /// del sistema.
        /// </summary>
        bool LoggingEnabled { get; set; }

        /// <summary>
        /// Desactiva TL.
        /// del sistema.
        /// </summary>
        bool TlRuntime { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Guarda la configuración.
        /// </summary>
        void Save();

        /// <summary>
        /// Carga la configuración.
        /// </summary>
        void Load();

        /// <summary>
        /// Esteblece el archivo de configuración con el cual trabajar.
        /// </summary>
        /// <param name="fileName">Nombre del archivo de configuración a utilizar.</param>
        void SetConfigFileName(string fileName);

        #endregion

    }

    #endregion

    #region Clase COM

    /// <summary>
    /// Representa una línea de impuestos.
    /// </summary>
    [Guid("F06F1FC8-DD40-4C3D-9049-E0508A42FC5F")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("FACe.FeSettings")]
    public class FeSettings : IFeSettings
    {

        #region Construtores de Instancia

        /// <summary>
        /// Constructor. Para COM necesitamos un constructor
        /// sin parametros.
        /// </summary>
        public FeSettings()
        {
        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Devuelve una instancia de clase Settings creada a partir
        /// de los datos de esta instancia.
        /// </summary>
        /// <returns>Instancia de clase Settings.</returns>
        private Config.Settings GetSettings()
        {

            var result = new Config.Settings()
            {
                LogPath = LogPath,
                CertificateSerial = CertificateSerial,
                CertificateThumbprint = CertificateThumbprint,
                CertificatePath = CertificatePath,
                CertificatePassword = CertificatePassword,
                FACeSettings = new FACeSettings()
                {
                    FACeEndPointPrefix = FACeEndPointPrefix,
                },
                LoggingEnabled = LoggingEnabled,
                TlRuntime = TlRuntime
            };

            return result;

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Guarda la configuración.
        /// </summary>
        public void Save()
        {

            Settings.Current = GetSettings();
            Settings.Save();

        }

        /// <summary>
        /// Carga la configuración.
        /// </summary>
        public void Load()
        {

            var settings = Settings.Current;

            LogPath = settings.LogPath;
            CertificateSerial = settings.CertificateSerial;
            CertificateThumbprint = settings.CertificateThumbprint;
            CertificatePath = settings.CertificatePath;
            CertificatePassword = settings.CertificatePassword;
            FACeEndPointPrefix = settings.FACeSettings.FACeEndPointPrefix;
            LoggingEnabled = settings.LoggingEnabled;
            TlRuntime = settings.TlRuntime;

        }

        /// <summary>
        /// Esteblece el archivo de configuración con el cual trabajar.
        /// </summary>
        /// <param name="fileName">Nombre del archivo de configuración a utilizar.</param>
        public void SetConfigFileName(string fileName)
        {

            Settings.SetConfigFileName(fileName);

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Identificación de la versión actual del esquema o
        /// estructura de información utilizada para la generación y
        /// conservación / remisión de los registros de facturación.
        /// Este campo forma parte del detalle de las circunstancias
        /// de generación de los registros de facturación.</para>
        /// <para>Alfanumérico(3) L15:</para>
        /// <para>1.0: Versión actual (1.0) del esquema utilizado </para>
        /// </summary>
        public string IDVersion { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como bandeja de entrada.
        /// En este directorio se almacenarán todos los mensajes
        /// recibidos de la AEAT mediante VERI*FACTU.
        /// </summary>
        public string InboxPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como bandeja de salida.
        /// En este directorio se almacenará una copia de cualquier
        /// envío realizado a la AEAT mediante el VERI*FACTU.
        /// </summary>
        public string OutboxPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará como almacén
        /// de las distintas cadenas de bloques por emisor.
        /// </summary>
        public string BlockchainPath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará almacenamiento
        /// de las facturas emitidas por emisor.
        /// </summary>
        public string InvoicePath { get; set; }

        /// <summary>
        /// Ruta al directorio que actuará almacenamiento
        /// del registro de mensajes del sistema.
        /// </summary>
        public string LogPath { get; set; }

        /// <summary>
        /// Número de serie del certificado a utilizar. Mediante este número
        /// de serie se selecciona del almacén de certificados de windows
        /// el certificado con el que realizar las comunicaciones.
        /// </summary>
        public string CertificateSerial { get; set; }

        /// <summary>
        /// Hash o Huella digital del certificado a utilizar. Mediante esta
        /// huella digital se selecciona del almacén de certificados de
        /// windows el certificado con el que realizar las comunicaciones.
        /// </summary>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Ruta al archivo del certificado a utilizar.
        /// Sólo se utiliza en los certificados cargados desde el sistema de archivos. 
        /// </summary>
        public string CertificatePath { get; set; }

        /// <summary>
        /// Password del certificado. Este valor sólo es necesario si
        /// tenemos establecido el valor para 'CertificatePath' y el certificado
        /// tiene clave de acceso. Sólo se utiliza en los certificados
        /// cargados desde el sistema de archivos.
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// EndPoint del API REST de la AEAT para envío FACe.
        /// </summary>
        public string FACeEndPointPrefix { get; set; }

        /// <summary>
        /// Indica si está activado el log de mensajes
        /// del sistema.
        /// </summary>
        public bool LoggingEnabled { get; set; }

        /// <summary>
        /// Desactiva TL.
        /// del sistema.
        /// </summary>
        public bool TlRuntime { get; set; }

        #endregion

    }

    #endregion

}
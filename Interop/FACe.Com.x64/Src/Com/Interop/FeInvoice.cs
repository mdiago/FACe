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

using FACe.Common;
using FACe.Net.Rest;
using FACe.Net.Rest.Json.Parser;
using FACe.Xml.Facturae;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FACe
{

    #region Interfaz COM

    /// <summary>
    /// Interfaz COM para la clase VfInvoice.
    /// </summary>
    [Guid("4512F03E-80EA-478E-BA04-0AB324D9AF40")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    [ComVisible(true)]
    public interface IFeInvoice
    {

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Clave del tipo de factura.</para>
        /// </summary>
        string InvoiceType { get; set; }

        /// <summary>
        ///  Identifica si el tipo de factura rectificativa
        ///  es por sustitución o por diferencia.
        /// </summary>
        string RectificationType { get; set; }

        /// <summary>
        /// Identificador de la factura.
        /// </summary>
        string InvoiceID { get; set; }

        /// <summary>
        /// Fecha emisión de documento.
        /// </summary>        
        DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Identificador del vendedor.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        string SellerID { get; set; }

        /// <summary>
        /// Nombre del vendedor.
        /// </summary>        
        string SellerName { get; set; }

        /// <summary>
        /// Identidicador del comprador.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        string BuyerID { get; set; }

        /// <summary>
        /// Nombre del comprador.
        /// </summary>        
        string BuyerName { get; set; }

        /// <summary>
        /// Código del país del destinatario (a veces también denominado contraparte,
        /// es decir, el cliente) de la operación de la factura expedida.
        /// <para>Alfanumérico (2) (ISO 3166-1 alpha-2 codes) </para>
        /// </summary>        
        string BuyerCountryID { get; set; }

        /// <summary>
        /// Clave para establecer el tipo de identificación
        /// en el pais de residencia. L7.
        /// </summary>        
        int BuyerIDType { get; set; }

        /// <summary>
        /// Código de moneda de la factura.
        /// </summary>        
        string CurrencyID { get; set; }

        /// <summary>
        /// Esta propiedad se utiliza para almacenar un identificador
        /// de sistema externo.
        /// </summary>        
        string ExternKey { get; set; }

        /// <summary>
        /// Importe total: Total neto + impuestos soportado
        /// - impuestos retenidos.
        /// </summary>        
        decimal TotalAmount { get; }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        decimal TotalTaxOutput { get; }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        decimal TotalTaxOutputSurcharge { get; }

        /// <summary>
        /// Importe total impuestos retenidos.
        /// </summary>        
        decimal TotalTaxWithheld { get; }

        /// <summary>
        /// Texto del documento.
        /// </summary>
        string Text { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Establece la fecha operación en caso
        /// de que se necesaria.
        /// </summary>
        /// <param name="operationDate">Fecha operación.</param>
        [DispId(1)]
        void SeOperationDate(DateTime operationDate);

        /// <summary>
        /// Añade datos en rectificativa por sustitución de la
        /// factura sustituida.
        /// </summary>
        /// <param name="taxItem">Datos factura sustituida.</param>
        [DispId(3)]
        void SetSubstitution(IFeTaxItem taxItem);

        /// <summary>
        /// Añade una línea de impuestos a la factura.
        /// </summary>
        /// <param name="taxItem">Línea de impuestos a añadir.</param>
        void InsertTaxItem(IFeTaxItem taxItem);

        /// <summary>
        /// Elimina una línea de impuestos de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        void DeleteTaxItemAt(int index);

        /// <summary>
        /// Añade una línea de factura rectificada.
        /// </summary>
        /// <param name="rectificationItem">Línea de factura rectificada a añadir.</param>
        void InsertRectificationItem(IFeRectificationItem rectificationItem);

        /// <summary>
        /// Elimina una línea de factura rectificada de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        void DeleteRectificationItemAt(int index);

        /// <summary>
        /// Añade una línea a la factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura a añadir.</param>
        void InsertInvoiceLine(IFeInvoiceLine invoiceLine);

        /// <summary>
        /// Elimina una línea de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        void DeleteInvoiceLineAt(int index);

        /// <summary>
        /// Añade una línea de vencimiento a la factura.
        /// </summary>
        /// <param name="installment">Línea de vencimiento a añadir.</param>
        void InsertInstallment(IFeInstallment installment);

        /// <summary>
        /// Elimina una línea de vencimiento de la factura.
        /// </summary>
        /// <param name="index">Número de la línea de vencimiento a eliminar.</param>
        void DeleteInstallmentAt(int index);

        /// <summary>
        /// Añade un interlocutor a la factura.
        /// </summary>
        /// <param name="party">Línea de interlocutor a añadir.</param>
        void InsertParty(IFeParty party);

        /// <summary>
        /// Elimina una línea de interlocutor de la factura.
        /// </summary>
        /// <param name="index">Número de la línea de interlocutor a eliminar.</param>
        void DeletePartyAt(int index);

        /// <summary>
        /// Devuelve el texto xml del documento
        /// factura-e compuesto con la factura.
        /// </summary>
        /// <returns>Texto xml del documento
        /// factura-e compuesto con la factura.</returns>
        string GetFacturae();

        /// <summary>
        /// Devuelve el texto xml del documento
        /// factura-e compuesto con la factura
        /// y firmado.
        /// </summary>
        /// <returns>Texto xml del documento
        /// factura-e compuesto con la factura
        /// y firmado.</returns>
        string GetFacturaeSigned();

        /// <summary>
        /// Genera y guarda un documento factura-e
        /// firmado.
        /// </summary>
        /// <param name="path">Ruta en la que guardar el
        /// archivo firmado.</param>
        void SaveFacturaeSigned(string path);

        /// <summary>
        /// Envía la factura a Verifactu de la AEAT.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        IFeInvoiceResult Send();

        /// <summary>
        /// Envía la anulación de la factura a Verifactu de la AEAT.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        IFeInvoiceResult Delete();

        #endregion

    }

    #endregion

    #region Clase COM

    /// <summary>
    /// Representa una factura.
    /// </summary>
    [Guid("4C062775-B475-43AE-8A94-6EF34FCEA090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    [ProgId("FACe.FeInvoice")]
    public class FeInvoice : IFeInvoice
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Fecha operación.
        /// </summary>        
        DateTime? _OperationDate;

        /// <summary>
        /// Objeto factura generado con los datos.
        /// </summary>
        Business.Invoice.Invoice _Invoice;

        /// <summary>
        /// Lista de líneas de impuestos.
        /// </summary>
        List<Business.Invoice.TaxItem> _TaxItems;

        /// <summary>
        /// Interlocutores de negocio.
        /// </summary>
        List<Business.Invoice.Party> _Parties;

        /// <summary>
        /// Líneas de la factura.
        /// </summary>
        List<Business.Invoice.InvoiceLine> _InvoiceLines;

        /// <summary>
        /// Vencimientos de la factura.
        /// </summary>
        List<Business.Invoice.Installment> _Installments;

        /// <summary>
        /// Facturas rectificadas.
        /// </summary>
        List<FACe.Business.Invoice.RectificationItem> _RectificationItems { get; set; }

        /// <summary>
        /// Información de rectificación sustitutiva.
        /// </summary>
        Business.Invoice.TaxItem _TaxItemSubstitution;

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor. Para COM necesitamos un constructor
        /// sin parametros.
        /// </summary>
        public FeInvoice()
        {

            _TaxItems = new List<Business.Invoice.TaxItem>();
            _InvoiceLines = new List<Business.Invoice.InvoiceLine>();
            _Installments = new List<Business.Invoice.Installment>();
            _Parties = new List<Business.Invoice.Party>();            
            _RectificationItems = new List<Business.Invoice.RectificationItem>();

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Devuelve una instancia de clase Invoice creada a partir
        /// de los datos de esta instancia.
        /// </summary>
        /// <returns>Instancia de clase Invoice.</returns>
        private Business.Invoice.Invoice GetInvoice()
        {

            var result = new Business.Invoice.Invoice(InvoiceID, InvoiceDate, SellerID)
            {
                SellerName = SellerName,
                BuyerID = BuyerID,
                BuyerName = BuyerName,
                Text = Text
            };

            if (_OperationDate != null)
                result.OperationDate = _OperationDate;

            var invoiceType = string.IsNullOrEmpty(InvoiceType) ? "FC" : InvoiceType;

            result.TaxItems = _TaxItems;
            result.Parties = _Parties;
            result.Installments = _Installments;
            result.InvoiceLines = _InvoiceLines;
            result.CalculateTotals();
            result.RectificationItems = _RectificationItems;

            return result;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Clave del tipo de factura.</para>
        /// </summary>
        public string InvoiceType { get; set; }

        /// <summary>
        ///  Identifica si el tipo de factura rectificativa
        ///  es por sustitución o por diferencia.
        /// </summary>
        public string RectificationType { get; set; }

        /// <summary>
        /// Identificador de la factura.
        /// </summary>
        public string InvoiceID { get; set; }

        /// <summary>
        /// Fecha emisión de documento.
        /// </summary>        
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Identificador del vendedor.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        public string SellerID { get; set; }

        /// <summary>
        /// Nombre del vendedor.
        /// </summary>        
        public string SellerName { get; set; }

        /// <summary>
        /// Identidicador del comprador.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        public string BuyerID { get; set; }

        /// <summary>
        /// Nombre del comprador.
        /// </summary>        
        public string BuyerName { get; set; }

        /// <summary>
        /// Código del país del destinatario (a veces también denominado contraparte,
        /// es decir, el cliente) de la operación de la factura expedida.
        /// <para>Alfanumérico (2) (ISO 3166-1 alpha-2 codes) </para>
        /// </summary>        
        public string BuyerCountryID { get; set; }

        /// <summary>
        /// Clave para establecer el tipo de identificación
        /// en el pais de residencia.
        /// </summary>        
        public int BuyerIDType { get; set; }

        /// <summary>
        /// Código de moneda de la factura.
        /// </summary>        
        public string CurrencyID { get; set; }

        /// <summary>
        /// Esta propiedad se utiliza para almacenar un identificador
        /// de sistema externo.
        /// </summary>        
        public string ExternKey { get; set; }

        /// <summary>
        /// Importe total: Total neto + impuestos soportado
        /// - impuestos retenidos.
        /// </summary>        
        public decimal TotalAmount
        {

            get
            {

                _Invoice = GetInvoice();
                return _Invoice.TotalAmount;

            }

        }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        public decimal TotalTaxOutput
        {

            get
            {

                _Invoice = GetInvoice();
                return _Invoice.TotalTaxOutput;

            }

        }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        public decimal TotalTaxOutputSurcharge
        {

            get
            {

                _Invoice = GetInvoice();
                return _Invoice.TotalTaxOutputSurcharge;

            }

        }

        /// <summary>
        /// Importe total impuestos retenidos.
        /// </summary>        
        public decimal TotalTaxWithheld
        {

            get
            {

                _Invoice = GetInvoice();
                return _Invoice.TotalTaxWithheld;

            }

        }

        /// <summary>
        /// Texto del documento.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Establece la fecha operación en caso
        /// de que se necesaria.
        /// </summary>
        /// <param name="operationDate">Fecha operación.</param>
        public void SeOperationDate(DateTime operationDate)
        {

            _OperationDate = operationDate;

        }

        /// <summary>
        /// Añade una línea de impuestos a la factura.
        /// </summary>
        /// <param name="taxItem">Línea de impuestos a añadir.</param>
        public void InsertTaxItem(IFeTaxItem taxItem)
        {

            var tax = string.IsNullOrEmpty(taxItem.Tax) ? "01" : taxItem.Tax;
            var taxClass = string.IsNullOrEmpty(taxItem.TaxClass) ? "TO" : taxItem.TaxClass;

            _TaxItems.Add(new Business.Invoice.TaxItem()
            {
                TaxClass = taxClass,
                Tax = tax,
                TaxBase = Math.Round(Convert.ToDecimal(taxItem.TaxBase), 2),
                TaxRate = Math.Round(Convert.ToDecimal(taxItem.TaxRate), 2),
                TaxAmount = Math.Round(Convert.ToDecimal(taxItem.TaxAmount), 2),
                TaxRateSurcharge = Math.Round(Convert.ToDecimal(taxItem.TaxRateSurcharge), 2),
                TaxAmountSurcharge = Math.Round(Convert.ToDecimal(taxItem.TaxAmountSurcharge), 2)
            });


        }

        /// <summary>
        /// Elimina una línea de impuestos de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        public void DeleteTaxItemAt(int index)
        {

            _TaxItems.RemoveAt(index);

        }

        /// <summary>
        /// Añade una línea de factura rectificada.
        /// </summary>
        /// <param name="rectificationItem">Línea de factura rectificada a añadir.</param>
        public void InsertRectificationItem(IFeRectificationItem rectificationItem)
        {

            _RectificationItems.Add(new Business.Invoice.RectificationItem()
            {
                InvoiceID = rectificationItem.InvoiceID,
                InvoiceDate = rectificationItem.InvoiceDate
            });

        }

        /// <summary>
        /// Elimina una línea de impuestos de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        public void DeleteRectificationItemAt(int index)
        {

            _RectificationItems.RemoveAt(index);

        }

        /// <summary>
        /// Añade una línea a la factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura a añadir.</param>
        public void InsertInvoiceLine(IFeInvoiceLine invoiceLine)
        {

            _InvoiceLines.Add(new Business.Invoice.InvoiceLine()
            {
                ItemPosition = invoiceLine.ItemPosition,
                BuyerReference = invoiceLine.BuyerReference,
                ItemID = invoiceLine.ItemID,
                ItemName = invoiceLine.ItemName,
                Quantity = Math.Round(Convert.ToDecimal(invoiceLine.Quantity), 2),
                UnitOfMeasure = invoiceLine.UnitOfMeasure,
                GrossPrice = Math.Round(Convert.ToDecimal(invoiceLine.GrossPrice), 6),
                NetPrice = Math.Round(Convert.ToDecimal(invoiceLine.NetPrice), 6),
                GrossAmount = Math.Round(Convert.ToDecimal(invoiceLine.GrossAmount), 6),
                NetAmount = Math.Round(Convert.ToDecimal(invoiceLine.NetAmount), 6),
                DiscountRate = Math.Round(Convert.ToDecimal(invoiceLine.DiscountRate), 4),
                DiscountAmount = Math.Round(Convert.ToDecimal(invoiceLine.DiscountAmount), 6),
                TaxesOutputRate = Math.Round(Convert.ToDecimal(invoiceLine.TaxesOutputRate), 2),
                TaxesOutputBase = Math.Round(Convert.ToDecimal(invoiceLine.TaxesOutputBase), 2),
                TaxesOutputAmount = Math.Round(Convert.ToDecimal(invoiceLine.TaxesOutputAmount), 2),
                TaxesOutputRateSurcharge = Math.Round(Convert.ToDecimal(invoiceLine.TaxesOutputRateSurcharge), 2),
                TaxesOutputAmounSurcharge = Math.Round(Convert.ToDecimal(invoiceLine.TaxesOutputAmounSurcharge), 2),
                TaxesOutputTax = invoiceLine.TaxesOutputTax,
                TaxesWithheldRate = Math.Round(Convert.ToDecimal(invoiceLine.TaxesWithheldRate), 2),
                TaxesWithheldBase = Math.Round(Convert.ToDecimal(invoiceLine.TaxesWithheldBase), 2),
                TaxesWithheldAmount = Math.Round(Convert.ToDecimal(invoiceLine.TaxesWithheldAmount), 2),
                TaxesWithheldTax = invoiceLine.TaxesWithheldTax
            });

        }

        /// <summary>
        /// Elimina una línea de la factura.
        /// </summary>
        /// <param name="index">Número de la línea a eliminar.</param>
        public void DeleteInvoiceLineAt(int index)
        {

            _InvoiceLines.RemoveAt(index);

        }

        /// <summary>
        /// Añade una línea de vencimiento a la factura.
        /// </summary>
        /// <param name="installment">Línea de vencimiento a añadir.</param>
        public void InsertInstallment(IFeInstallment installment)
        {

            _Installments.Add(new Business.Invoice.Installment()
            {
                DueDate = installment.DueDate,
                Amount = Math.Round(Convert.ToDecimal(installment.Amount), 2),
                PaymentMeans = installment.PaymentMeans,
                BankAccountType = installment.BankAccountType,
                BankAccount = installment.BankAccount
            });

        }

        /// <summary>
        /// Elimina una línea de vencimiento de la factura.
        /// </summary>
        /// <param name="index">Número de la línea de vencimiento a eliminar.</param>
        public void DeleteInstallmentAt(int index)
        {

            _Installments.RemoveAt(index);

        }

        /// <summary>
        /// Añade un interlocutor a la factura.
        /// </summary>
        /// <param name="party">Línea de interlocutor a añadir.</param>
        public void InsertParty(IFeParty party)
        {

            _Parties.Add(new Business.Invoice.Party()
            {
                PartyType = party.PartyType,
                PartyRole = party.PartyRole,
                PartyID = party.PartyID,
                PartyName = party.PartyName,
                TaxID = party.TaxID,
                Address = party.Address,
                City = party.City,
                PostalCode = party.PostalCode,
                Region = party.Region,
                CountryID = party.CountryID,
                Mail = party.Mail,
                Mobile = party.Mobile,
                Phone = party.Phone,
                WebAddress = party.WebAddress
            });

        }

        /// <summary>
        /// Elimina una línea de interlocutor de la factura.
        /// </summary>
        /// <param name="index">Número de la línea de interlocutor a eliminar.</param>
        public void DeletePartyAt(int index)
        {

            _Parties.RemoveAt(index);

        }

        /// <summary>
        /// Añade datos en rectificativa por sustitución de la
        /// factura sustituida.
        /// </summary>
        /// <param name="taxItem">Datos factura sustituida.</param>
        public void SetSubstitution(IFeTaxItem taxItem)
        {

            _TaxItemSubstitution = new Business.Invoice.TaxItem() 
            { 
                TaxBase = Math.Round(Convert.ToDecimal(taxItem.TaxBase), 2),
                TaxAmount = Math.Round(Convert.ToDecimal(taxItem.TaxAmount), 2),
                TaxAmountSurcharge = Math.Round(Convert.ToDecimal(taxItem.TaxAmountSurcharge), 2)
            };

        }

        /// <summary>
        /// Devuelve el texto xml del documento
        /// factura-e compuesto con la factura.
        /// </summary>
        /// <returns>Texto xml del documento
        /// factura-e compuesto con la factura.</returns>
        public string GetFacturae() 
        {

            _Invoice = GetInvoice();
            var facturae = _Invoice.GetFacturae();
            var facturaeManager = new FacturaeManager(facturae);

            return facturaeManager.GetUTF8XmlText();

        }

        /// <summary>
        /// Devuelve el texto xml del documento
        /// factura-e compuesto con la factura
        /// y firmado.
        /// </summary>
        /// <returns>Texto xml del documento
        /// factura-e compuesto con la factura
        /// y firmado.</returns>
        public string GetFacturaeSigned()
        {

            var certificate = CertManager.GetCertificateFromSettings();

            if (certificate != null)
                throw new ArgumentException($"No se puede" +
                    $" firmar el documento ya que el certificado digital es nulo.");

            _Invoice = GetInvoice();
            var facturae = _Invoice.GetFacturae();
            var facturaeManager = new FacturaeManager(facturae);

            return facturaeManager.GetXmlTextSigned(certificate);

        }

        /// <summary>
        /// Genera y guarda un documento factura-e
        /// firmado.
        /// </summary>
        /// <param name="path">Ruta en la que guardar el
        /// archivo firmado.</param>
        public void SaveFacturaeSigned(string path) 
        {

            var signedXml = GetFacturaeSigned();
            File.WriteAllText(path, signedXml);

        }

        /// <summary>
        /// Envía la factura a Verifactu de la AEAT.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        public IFeInvoiceResult Send()
        {

            FeInvoiceResult result =  new FeInvoiceResult()
            {
                ResultCode = "999",
                ResultMessage = "Se ha producido un error desconocido.",
                CSV = null
            }; 

            _Invoice = GetInvoice();

            dynamic response = ApiClient.Create(_Invoice);

            var faceResponseJson = Regex.Unescape($"{response.Return.Response}");
            var faceResponseJsonDecoded = Regex.Unescape(faceResponseJson); // Carácteres unicode

            var jsonParser = new JsonParser(faceResponseJsonDecoded);
            var faceResponse = jsonParser.GetResult();

            if ((faceResponse as IDictionary<string, object>).ContainsKey("status"))
                result = new FeInvoiceResult()
                {
                    ResultCode = $"{response.ResultCode}",
                    ResultMessage = $"{response.ResultMessage}",
                    CSV = $"{response.Return.CSV}"
                };
            else if ((faceResponse as IDictionary<string, object>).ContainsKey("code"))
                result = new FeInvoiceResult()
                {
                    ResultCode = $"{faceResponse.code}",
                    ResultMessage = $"{string.Join("\n", faceResponse.errors)}",
                    CSV = $"{response.Return.CSV}"
                };

            return result;

        }

        /// <summary>
        /// Envía la anulación de la factura a Verifactu de la AEAT.
        /// </summary>
        /// <returns>Resultado de la operación.</returns>
        public IFeInvoiceResult Delete()
        {

            throw new NotImplementedException();

            var result = new FeInvoiceResult()
            {
                ResultCode = "0"
            };

            _Invoice = GetInvoice();


            return result;

        }

        #endregion

    }

    #endregion

}
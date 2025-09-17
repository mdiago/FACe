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
using FACe.Net.Rest.Json.Kivu;
using FACe.Xml.Facturae.Bies;
using System;
using System.Collections.Generic;

namespace FACe.Business.Invoice
{

    /// <summary>
    /// Representa un factura en el sistema FACe.
    /// </summary>
    public class Invoice : JsonSerializableKivu
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Suma de las bases imponibles.
        /// </summary>   
        decimal _NetAmount;

        /// <summary>
        /// Documento Facturae que representa
        /// la factura.
        /// </summary>
        Facturae _FacturaeSource;

        #endregion

        #region Construtores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoiceID">Identificador de la factura.</param>
        /// <param name="invoiceDate">Fecha emisión de documento.</param>
        /// <param name="sellerID">Identificador del vendedor.</param>        
        /// <exception cref="ArgumentNullException">Los argumentos invoiceID y sellerID no pueden ser nulos</exception>
        public Invoice(string invoiceID, DateTime invoiceDate, string sellerID) 
        {

            if (invoiceID == null || sellerID == null)
                throw new ArgumentNullException($"Los argumentos invoiceID y sellerID no pueden ser nulos.");

            InvoiceID = invoiceID.Trim(); // La AEAT calcula el Hash sin espacios
            InvoiceDate = invoiceDate;

            var tSellerID = sellerID.Trim(); // La AEAT calcula el Hash sin espacios
            SellerID = tSellerID.ToUpper(); // https://github.com/mdiago/VeriFactu/issues/65

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="facturae">Documento Facturae de alta a partir del que se crea la factura.</param>
        public Invoice(Facturae facturae) : this(facturae.Invoices[0].InvoiceHeader.InvoiceNumber,
                facturae.Invoices[0].InvoiceIssueData.IssueDate, $"{facturae.Parties.SellerParty.TaxIdentification.TaxIdentificationNumber}")
        {

            _FacturaeSource = facturae;

            // Unimos dos propiedades de Facturae por compatibilidad con otros estandares
            InvoiceType = $"{facturae.Invoices[0].InvoiceHeader.InvoiceDocumentType}" +
                $"{facturae.Invoices[0].InvoiceHeader.InvoiceClass}";

            var sellerLegalEntity = (facturae.Parties?.SellerParty?.Party) as LegalEntity;
            var sellerIndividual = (facturae.Parties?.SellerParty?.Party) as Individual;

            SellerName = sellerLegalEntity != null ? sellerLegalEntity.CorporateName : 
                $"{sellerIndividual.Name} {sellerIndividual.FirstSurname} {sellerIndividual.SecondSurname}".Trim();
            
            Text = facturae.Invoices[0].AdditionalData.InvoiceAdditionalInformation;

            if (facturae.Invoices[0].InvoiceHeader?.Corrective?.ReasonCode != null)
                RectificationType = $"{facturae.Invoices[0].InvoiceHeader?.Corrective?.ReasonCode}";


            BuyerID = $"{facturae.Parties.BuyerParty.TaxIdentification.TaxIdentificationNumber}";

            var buyerLegalEntity = (facturae.Parties?.BuyerParty?.Party) as LegalEntity;
            var buyerIndividual = (facturae.Parties?.BuyerParty?.Party) as Individual;

            BuyerName = buyerLegalEntity != null ? buyerLegalEntity.CorporateName :
                $"{buyerIndividual.Name} {buyerIndividual.FirstSurname} {buyerIndividual.SecondSurname}".Trim();
       

            TaxItems = GetTaxItems(facturae.Invoices[0]);
            CalculateTotals();

        }

        #endregion

        #region Métodos Privados Estáticos

        /// <summary>
        /// Devuelve una lista de TaxItem a partir
        /// de un elemento Invoice de documento Facturae.
        /// </summary>
        /// <param name="invoice"> Elemento Invoice de documento Facturae.</param>
        /// <returns> Lista de TaxItems</returns>
        internal static List<TaxItem> GetTaxItems(Xml.Facturae.Bies.Invoice invoice)
        {

            var taxitems = new List<TaxItem>();

            foreach (var taxOutput in invoice.TaxesOutputs)
            {
                var taxItem = new TaxItem()
                {
                    TaxClass = "TO",
                    TaxBase = taxOutput.TaxableBase,
                    TaxRate = taxOutput.TaxRate,
                    TaxAmount = taxOutput.TaxAmount,
                    TaxRateSurcharge = (decimal)taxOutput.EquivalenceSurcharge,
                    TaxAmountSurcharge = (decimal)taxOutput.EquivalenceSurchargeAmount
                };

                taxItem.Tax = $"{taxOutput.TaxTypeCode}";

                taxitems.Add(taxItem);

            }

            foreach (var taxOutput in invoice.TaxesWithheld)
            {
                var taxItem = new TaxItem()
                {
                    TaxClass = "TW",
                    TaxBase = taxOutput.TaxableBase,
                    TaxRate = taxOutput.TaxRate,
                    TaxAmount = - taxOutput.TaxAmount, // Retenciones
                };

                taxItem.Tax = $"{taxOutput.TaxTypeCode}";

                taxitems.Add(taxItem);

            }


            return taxitems;

        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Calcula los totales de la factura.
        /// </summary>
        internal void CalculateTotals() 
        {

            TotalAmount = TotalTaxOutput = TotalTaxWithheld = TotalTaxOutputSurcharge = _NetAmount = 0;

            if (TaxItems == null || TaxItems.Count == 0)
                return;

            foreach (var taxitem in TaxItems) 
            {

                if (taxitem.TaxClass == "TO")
                {
                    _NetAmount += taxitem.TaxBase;
                    TotalTaxOutput += taxitem.TaxAmount;
                    TotalTaxOutputSurcharge += taxitem.TaxAmountSurcharge;
                }
                else 
                {
                    TotalTaxWithheld += taxitem.TaxAmount;
                }

            }

            TotalAmount = _NetAmount + TotalTaxOutput + TotalTaxOutputSurcharge - TotalTaxWithheld;

        }

        /// <summary>
        /// Obtiene el desglose de la factura.
        /// </summary>
        /// <returns>Desglose de la factura.</returns>
        private Xml.Facturae.Bies.Invoice GetTaxes() 
        {            

            if (TaxItems == null || TaxItems?.Count == 0) 
                throw new InvalidOperationException("No se puede obtener el bloque obligatorio" +
                    " 'DetalleDesglose' ya que la lista de TaxItems no contiene elementos.");

            var taxesOutputs = new List<TaxOutput>(); 
            var taxesWithheld = new List<Tax>();

            decimal taxBaseTotal = 0;
            decimal taxTaxesOutputsTotal = 0;
            decimal taxAmountSurchargeTotal = 0;
            decimal taxTaxesWithheldTotal = 0;


            foreach (var taxitem in TaxItems)
            {
                
                TaxTypeCode taxTypeCode = TaxTypeCode.IVA;

                if(!string.IsNullOrEmpty(taxitem.Tax) && !Enum.TryParse<TaxTypeCode>(taxitem.Tax, out taxTypeCode))
                    throw new ArgumentException($"El valor '{taxitem.Tax}' no es una valor válido para TaxTypeCode.");

                // Máximo dos decimales
                var taxRate = Math.Round(taxitem.TaxRate, 2, MidpointRounding.AwayFromZero);
                var taxBase = Math.Round(taxitem.TaxBase, 2, MidpointRounding.AwayFromZero);
                var taxAmount = Math.Round(taxitem.TaxAmount, 2, MidpointRounding.AwayFromZero);
                var taxRateSurcharge = Math.Round(taxitem.TaxRateSurcharge, 2, MidpointRounding.AwayFromZero);
                var taxAmountSurcharge = Math.Round(taxitem.TaxAmountSurcharge, 2, MidpointRounding.AwayFromZero);

                // Totales
                taxBaseTotal += (taxitem.TaxClass == "TO") ? taxBase : 0;
                taxTaxesOutputsTotal += (taxitem.TaxClass == "TO") ? taxAmount : 0;
                taxTaxesWithheldTotal += (taxitem.TaxClass == "TW") ? taxAmount : 0;
                taxAmountSurchargeTotal += taxAmountSurcharge;

                if (taxitem.TaxClass == "TO")
                    taxesOutputs.Add( new TaxOutput()
                        {
                            TaxTypeCode = taxTypeCode,
                            TaxRate = taxRate,
                            TaxableBase = taxBase,
                            TaxAmount = taxAmount,
                        });

                if (taxitem.TaxClass == "TW")
                    taxesWithheld.Add(new Tax()
                    {
                        TaxTypeCode = taxTypeCode,
                        TaxRate = taxRate,
                        TaxableBase = taxBase,
                        TaxAmount = taxAmount,
                    });             

            }

            return new Xml.Facturae.Bies.Invoice() 
            {
                TaxesOutputs = taxesOutputs.Count > 0 ? taxesOutputs.ToArray() : null,
                TaxesWithheld = taxesWithheld.Count > 0 ? taxesWithheld.ToArray() : null,
                InvoiceTotals = new InvoiceTotals()
                {
                    TotalGrossAmount = 0,
                    TotalGeneralDiscounts = 0,
                    TotalGeneralSurcharges = 0,
                    TotalGrossAmountBeforeTaxes = taxBaseTotal,
                    TotalTaxOutputs = taxTaxesOutputsTotal,
                    TotalTaxesWithheld = -taxTaxesWithheldTotal,
                    InvoiceTotal = taxBaseTotal + taxTaxesOutputsTotal+ taxTaxesWithheldTotal+ taxAmountSurchargeTotal,
                    TotalOutstandingAmount = taxBaseTotal + taxTaxesOutputsTotal + taxTaxesWithheldTotal + taxAmountSurchargeTotal,
                    TotalExecutableAmount = taxBaseTotal + taxTaxesOutputsTotal + taxTaxesWithheldTotal + taxAmountSurchargeTotal
                },

            };

        }

        /// <summary>
        /// Recupera el interlocutor con el TaxId
        /// pasado como parámetro.
        /// </summary>
        /// <param name="taxId">Identificador fiscal.</param>
        /// <returns>El interlocutor con ese TaxId o null si no existe.</returns>
        private Party GetPartyByTaxId(string taxId) 
        {

            foreach(var current in Parties)
                if(current.TaxID == taxId)
                   return current;

            return null;
        
        }

        /// <summary>
        /// Recupera el interlocutor con el role
        /// pasado como parámetro.
        /// </summary>
        /// <param name="role">Rol del interlocutor en la factura.</param>
        /// <returns>El interlocutor con ese TaxId o null si no existe.</returns>
        private Party GetPartyByPartyRole(string role)
        {

            foreach (var current in Parties)
                if (current.PartyRole == role)
                    return current;

            return null;

        }

        /// <summary>
        /// Devuelve el tipo de residencia según el
        /// código de pais.
        /// </summary>
        /// <param name="countryID">Código pais.</param>
        /// <returns>Tipo de residencia.</returns>
        private ResidenceTypeCode GetResidenceTypeCode(string countryID) 
        {

            if (countryID == "ES")
                return ResidenceTypeCode.R;

            string[] ueCountries = { "DE", "AT", "BE", "BG", "CY", "HR", "DK", "SK", "SI", "ES", "EE",
            "FI", "FR", "GR", "HU", "IE", "IT", "LV", "LT", "LU", "MT", "NL", "PL", "PT", "CZ", "RO", "SE" }; 
            
            if (Array.IndexOf(ueCountries, countryID) != -1)
                return ResidenceTypeCode.U;
            
            return ResidenceTypeCode.U;

        }

        /// <summary>
        /// Devuelve un LegalEntity o Individual
        /// según el caso.
        /// </summary>
        /// <param name="party"></param>
        /// <returns>Devuelve un LegalEntity o Individual.</returns>
        private object GetParty(Party party) 
        {

            var contactDetails = new ContactDetails()
            {
                Telephone = party.Phone,
                WebAddress = party.WebAddress,
                ElectronicMail = party.Mail
            };

            var address = new Address()
            {
                AddressText = party.Address,
                PostCode = party.PostalCode,
                Town = party.City,
                Province = party.Region,
                CountryCode = Country.ESP
            };

            if (party.PartyType == "J")
                return new LegalEntity()
                {
                    CorporateName = party.PartyName,
                    TradeName = party.PartyName,
                    Address = address,
                    ContactDetails = contactDetails
                };

            var names = party.PartyName.Split(' ');            

            if (party.PartyType == "F")
                return new Individual()
                {
                    Name = names[0] + (names.Length > 3 ? names[1]: ""),
                    FirstSurname = names.Length > 2 ? names[names.Length - 2] : null,
                    SecondSurname = names.Length > 2 ? names[names.Length - 1] : null,
                    Address = address,
                    ContactDetails = contactDetails
                }; 

            return null;

        }

        /// <summary>
        /// Devuelve los vencimientos para FActurae.
        /// </summary>
        /// <param name="installments"> 
        /// Vencimeintos
        /// de la factura de la que extraer
        /// los vencimientos
        /// </param>
        /// <returns> Vencimientos para Facturae.</returns>
        private Xml.Facturae.Bies.Installment[] GetPaymentDetails(List<Installment> installments) 
        {

            var details = new Xml.Facturae.Bies.Installment[installments.Count];

            for (int i = 0; i < installments.Count; i++) 
            {

                var installemt = installments[i];

                AccountChoice accountChoice = AccountChoice.IBAN;
                PaymentMeans paymentMeans = PaymentMeans.CreditTransfer;

                if (!string.IsNullOrEmpty(installemt.PaymentMeans) && !Enum.TryParse<PaymentMeans>(installemt.PaymentMeans, out paymentMeans))
                    throw new ArgumentException($"Valor {installemt.PaymentMeans} no válido para PaymentMeans."); 

                if (!string.IsNullOrEmpty(installemt.BankAccountType) && !Enum.TryParse<AccountChoice>(installemt.BankAccountType, out accountChoice))
                    throw new ArgumentException($"Valor {installemt.BankAccountType} no válido para AccountChoice.");

                details[i] = new Xml.Facturae.Bies.Installment()
                {
                    InstallmentDueDate = installemt.DueDate,
                    InstallmentAmount = installemt.Amount,
                    PaymentMeans = paymentMeans,
                    AccountToBeCredited = new Account()
                    {
                        AccountElementName = accountChoice,
                        BankAccount = installemt.BankAccount
                    }
                };

            }

            return details;

        }

        /// <summary>
        /// Devuelve las líneas de factura para FActurae.
        /// </summary>
        /// <param name="invoiceLines"> 
        /// Líneas de la factura.
        /// </param>
        /// <returns> Líneas para Facturae.</returns>
        private Xml.Facturae.Bies.InvoiceLine[] GetLines(List<InvoiceLine> invoiceLines)
        {

            var lines = new Xml.Facturae.Bies.InvoiceLine[invoiceLines.Count];

            for (int i = 0; i < invoiceLines.Count; i++)
            {

                var invoiceLine = invoiceLines[i];

                UnitOfMeasure unitOfMeasure = UnitOfMeasure.Units;

                if (!string.IsNullOrEmpty(invoiceLine.UnitOfMeasure) && !Enum.TryParse<UnitOfMeasure>(invoiceLine.UnitOfMeasure, out unitOfMeasure))
                    throw new ArgumentException($"Valor {invoiceLine.UnitOfMeasure} no válido para UnitOfMeasure.");

                TaxTypeCode taxesOutputsTaxTypeCode = TaxTypeCode.IVA;

                if (!string.IsNullOrEmpty(invoiceLine.TaxesOutputTax) && !Enum.TryParse<TaxTypeCode>(invoiceLine.TaxesOutputTax, out taxesOutputsTaxTypeCode))
                    throw new ArgumentException($"El valor '{invoiceLine.TaxesOutputTax}' no es una valor válido para TaxTypeCode.");

                TaxTypeCode taxesWithheldTaxTypeCode = TaxTypeCode.IRPF;

                if (!string.IsNullOrEmpty(invoiceLine.TaxesWithheldTax) && !Enum.TryParse<TaxTypeCode>(invoiceLine.TaxesWithheldTax, out taxesWithheldTaxTypeCode))
                    throw new ArgumentException($"El valor '{invoiceLine.TaxesWithheldTax}' no es una valor válido para TaxTypeCode.");

                lines[i] = new Xml.Facturae.Bies.InvoiceLine()
                {
                    ReceiverTransactionReference = invoiceLine.BuyerReference,
                    SequenceNumber = invoiceLine.ItemPosition,
                    ArticleCode = invoiceLine.ItemID,
                    ItemDescription = invoiceLine.ItemName,
                    Quantity = invoiceLine.Quantity,
                    UnitOfMeasure = unitOfMeasure,
                    UnitPriceWithoutTax = invoiceLine.NetPrice,
                    DiscountsAndRebates = new Discount[1] { new Discount() { DiscountReason = "DTO GENERAL", DiscountRate = invoiceLine.DiscountRate, DiscountAmount = invoiceLine.DiscountAmount } },
                    TotalCost = invoiceLine.NetAmount,
                    GrossAmount = invoiceLine.GrossAmount,
                    TaxesOutputs = new Tax[1]
                    {
                        new Tax()
                        {
                            TaxTypeCode = taxesOutputsTaxTypeCode,
                            TaxRate = invoiceLine.TaxesOutputRate,
                            TaxableBase = invoiceLine.TaxesOutputBase,
                            TaxAmount  = invoiceLine.TaxesOutputAmount,
                        }
                    }                    
                };

                // DE MOMENTO EL VALIDADOR DE FACE NO ACEPTA IMPUESTOS RETENIDOS EN LA LÍNEA
                if (invoiceLine.TaxesWithheldAmount != 0)
                    lines[i].TaxesWithheld = new Tax[1]
                    {
                        new Tax()
                        {
                            TaxTypeCode = taxesWithheldTaxTypeCode,
                            TaxRate = invoiceLine.TaxesWithheldRate,
                            TaxableBase = invoiceLine.TaxesWithheldBase,
                            TaxAmount  = -invoiceLine.TaxesWithheldAmount,
                        }
                    };

            }

            return lines;

        }

        #endregion

        #region Propiedades Públicas de Instancia

        /// <summary>
        /// <para>Clave del tipo de factura (L2).</para>
        /// </summary>
        public string InvoiceType { get; set; }

        /// <summary>
        ///  Identifica si el tipo de factura rectificativa
        ///  es por sustitución o por diferencia (L3).
        /// </summary>
        public string RectificationType { get; set; }

        /// <summary>
        /// Identificador de la factura.
        /// </summary>
        public string InvoiceID { get; private set; }

        /// <summary>
        /// Fecha emisión de documento.
        /// </summary>        
        public DateTime InvoiceDate { get; private set; }

        /// <summary>
        /// Fecha operación.
        /// </summary>        
        public DateTime? OperationDate { get; set; }

        /// <summary>
        /// Identificador del vendedor.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        public string SellerID { get; private set; }

        /// <summary>
        /// Nombre del vendedor.
        /// </summary>        
        [Json(Name = "CompanyName")]
        public string SellerName { get; set; }

        /// <summary>
        /// Identidicador del comprador.
        /// Debe utilizarse el identificador fiscal si existe (NIF, VAT Number...).
        /// En caso de no existir, se puede utilizar el número DUNS 
        /// o cualquier otro identificador acordado.
        /// </summary>        
        [Json(Name = "RelatedPartyID")]
        public string BuyerID { get; set; }

        /// <summary>
        /// Nombre del comprador.
        /// </summary>        
        [Json(Name = "RelatedPartyName")]
        public string BuyerName { get; set; }

        /// <summary>
        /// Código del país del destinatario (a veces también denominado contraparte,
        /// es decir, el cliente) de la operación de la factura expedida.
        /// <para>Alfanumérico (2) (ISO 3166-1 alpha-2 codes) </para>
        /// </summary>        
        [Json(Name = "CountryID")]
        public string BuyerCountryID { get; set; }

        /// <summary>
        /// Clave para establecer el tipo de identificación
        /// en el pais de residencia. L7.
        /// </summary>        
        [Json(Name = "RelatedPartyIDType")]
        public string BuyerIDType { get; set; }

        /// <summary>
        /// Código de moneda de la factura.
        /// </summary>        
        [Json(Name = "DocumentCurrencyID")]
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
        public decimal TotalAmount { get; private set; }

        /// <summary>
        /// Total impuestos soportados.
        /// </summary>        
        public decimal TotalTaxOutput { get; private set; }

        /// <summary>
        /// Total impuestos soportados recargo.
        /// </summary>        
        public decimal TotalTaxOutputSurcharge { get; private set; }

        /// <summary>
        /// Importe total impuestos retenidos.
        /// </summary>        
        public decimal TotalTaxWithheld { get; set; }

        /// <summary>
        /// Texto del documento.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Líneas de impuestos.
        /// </summary>
        public List<TaxItem> TaxItems { get; set; }

        /// <summary>
        /// Facturas rectificadas.
        /// </summary>
        public List<RectificationItem> RectificationItems { get; set; }

        /// <summary>
        /// Interlocutores de negocio.
        /// </summary>
        public List<Party> Parties { get; set; }

        /// <summary>
        /// Líneas de la factura.
        /// </summary>
        public List<InvoiceLine> InvoiceLines { get; set; }

        /// <summary>
        /// Vencimientos de la factura.
        /// </summary>
        public List<Installment> Installments { get; set; }

        /// <summary>
        /// BaseRectificada para rectificativas por sustitución 'S'.
        /// </summary>        
        public decimal RectificationTaxBase { get; set; }

        /// <summary>
        /// CuotaRectificada para rectificativas por sustitución 'S'.
        /// </summary>        
        public decimal RectificationTaxAmount { get; set; }

        /// <summary>
        /// CuotaRecargoRectificado para rectificativas por sustitución 'S'.
        /// </summary>
        public decimal RectificationTaxAmountSurcharge { get; set; }

        /// <summary>
        /// RegistroAlta a partir del cual se ha creado la factura, en el
        /// caso de que la instancia se haya creado a partir de un registro
        /// de alta.
        /// </summary>
        [Json(JsonIgnore = true)]
        public Facturae Facturae
        {

            get 
            {

                return _FacturaeSource;

            }

        }

        #endregion     

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Obtiene el registro de alta para verifactu.
        /// </summary>
        /// <returns>Registro de alta para verifactu</returns>
        public Facturae GetFacturae()
        {

            if (_FacturaeSource != null)
                return _FacturaeSource;

            // Modeda por defecto EUR
            if (string.IsNullOrEmpty(CurrencyID))
                CurrencyID = "EUR";

            CalculateTotals();

            // Máximo dos decimales
            var totalTaxAmount = Math.Round(TotalTaxOutput + TotalTaxOutputSurcharge, 2);
            var totalAmount = Math.Round(TotalAmount, 2);

            var taxFacturae = GetTaxes();

            var seller = GetPartyByTaxId(SellerID);
            var buyer = GetPartyByTaxId(BuyerID);

            if(!Enum.TryParse<PersonTypeCode>(seller.PartyType, out var personTypeCodeSeller))
                throw new ArgumentException($"PartyType '{seller.PartyType}'" +
                    $" no es un PersonTypeCode adecuado.");

            if (!Enum.TryParse<PersonTypeCode>(seller.PartyType, out var personTypeCodeBuyer))
                throw new ArgumentException($"PartyType '{seller.PartyType}'" +
                    $" no es un PersonTypeCode adecuado.");

            if (!Enum.TryParse<CurrencyCode>(CurrencyID, out var currencyCode))
                throw new ArgumentException($"CurrencyID '{CurrencyID}'" +
                    $" no es un CurrencyCode adecuado.");

            seller.PartyName = SellerName;
            buyer.PartyName = BuyerName;
            var partySeller = GetParty(seller);
            var partyBuyer = GetParty(buyer);

            var oc = GetPartyByPartyRole("OC");     // Oficina contable
            var og = GetPartyByPartyRole("OG");     // Organo gestor
            var ut = GetPartyByPartyRole("UT");     // Unidad tramitadora          

            var facturae = new Facturae()
            {
                // FileHeader
                FileHeader = new FileHeader()
                {
                    Modality = Modality.I,
                    InvoiceIssuerType = InvoiceIssuerType.EM,
                    Batch = new Batch()
                    {
                        BatchIdentifier = $"BATCH-{InvoiceID}-{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds:#0}",
                        InvoicesCount = 1,
                        TotalInvoicesAmount = taxFacturae.InvoiceTotals.InvoiceTotal,
                        TotalOutstandingAmount = taxFacturae.InvoiceTotals.TotalExecutableAmount,
                        TotalExecutableAmount = taxFacturae.InvoiceTotals.TotalExecutableAmount,
                        InvoiceCurrencyCode = currencyCode
                    },
                },
                // Parties
                Parties = new PartiesType()
                {
                    SellerParty = new Xml.Facturae.Bies.Business()
                    {
                        TaxIdentification = new TaxIdentification()
                        {
                            PersonTypeCode = personTypeCodeSeller,
                            ResidenceTypeCode = GetResidenceTypeCode(seller.CountryID),
                            TaxIdentificationNumber = SellerID
                        },
                        Party = partySeller
                    },
                    BuyerParty = new Xml.Facturae.Bies.Business()
                    {
                        TaxIdentification = new TaxIdentification()
                        {
                            PersonTypeCode = personTypeCodeSeller,
                            ResidenceTypeCode = GetResidenceTypeCode(buyer.CountryID),
                            TaxIdentificationNumber = BuyerID
                        },
                        AdministrativeCentres = new AdministrativeCentre[3]
                        {
                            new AdministrativeCentre()
                            {
                                CentreCode = oc.PartyID,
                                RoleTypeCode = RoleTypeCode.Fiscal,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = oc.Address,
                                    PostCode = oc.PostalCode,
                                    Town = oc.City,
                                    Province = oc.Region,
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Oficina Contable"
                            },
                            new AdministrativeCentre()
                            {
                                CentreCode = og.PartyID,
                                RoleTypeCode = RoleTypeCode.Receiver,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = og.Address,
                                    PostCode = og.PostalCode,
                                    Town = og.City,
                                    Province = og.Region,
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Organo Gestor"
                            },
                            new AdministrativeCentre()
                            {
                                CentreCode = ut.PartyID,
                                RoleTypeCode = RoleTypeCode.Payer,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = ut.Address,
                                    PostCode = ut.PostalCode,
                                    Town = ut.City,
                                    Province = ut.Region,
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Unidad Tramitadora"
                            }
                        },
                        Party = partyBuyer
                    }
                },
                // Invoices
                Invoices = new Xml.Facturae.Bies.Invoice[1]
                { 
                    // Invoice
                    new Xml.Facturae.Bies.Invoice()
                    {
                        InvoiceHeader = new InvoiceHeader()
                        {
                             InvoiceNumber = InvoiceID,
                             InvoiceSeriesCode = null,
                             InvoiceDocumentType = InvoiceDocumentType.FC,
                             InvoiceClass = InvoiceClass.OO
                        },
                        InvoiceIssueData = new InvoiceIssueData()
                        {
                            IssueDate = InvoiceDate,
                            InvoiceCurrencyCode = CurrencyCode.EUR,
                            TaxCurrencyCode = CurrencyCode.EUR,
                            LanguageName = LanguageCode.es
                        },
                        TaxesOutputs =  taxFacturae.TaxesOutputs,
                        TaxesWithheld = taxFacturae.TaxesWithheld,
                        InvoiceTotals = new InvoiceTotals()
                        {
                             TotalGrossAmount = 0,
                             TotalGeneralDiscounts = 0,
                             TotalGeneralSurcharges = 0,
                             TotalGrossAmountBeforeTaxes = 0,
                             TotalTaxOutputs = taxFacturae.InvoiceTotals.TotalTaxOutputs,
                             TotalTaxesWithheld = taxFacturae.InvoiceTotals.TotalTaxesWithheld,
                             InvoiceTotal = taxFacturae.InvoiceTotals.InvoiceTotal,
                             TotalOutstandingAmount = taxFacturae.InvoiceTotals.TotalOutstandingAmount,
                             TotalExecutableAmount = taxFacturae.InvoiceTotals.TotalExecutableAmount
                        },
                        Items = GetLines(InvoiceLines),
                        // Forma de pago
                        PaymentDetails = GetPaymentDetails(Installments)
                    }
                }
            };

            return facturae;           

        }

        /// <summary>
        /// Representación textual de la instancia.
        /// </summary>
        /// <returns> Representación textual de la instancia.</returns>
        public override string ToString()
        {

            return $"{SellerID}-{InvoiceID}-{InvoiceDate:yyyy-MM-dd}";

        }

        #endregion

    }

}

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

using FACe.Xml.Facturae.Bies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACe.Business.Invoice.Converters
{

    /// <summary>
    /// Encargado de convertir una instancia de Invoice en un objeto
    /// Facturae determinado y de otener de un objeto Facturae
    /// un objeto Invoice con su representación.
    /// </summary>
    public class FacturaeConverter : InvoiceConverter<Facturae>
    {

        #region Constructores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoice">Objeto Invoice.</param>
        public FacturaeConverter(Invoice invoice) : base(invoice) 
        { 
        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Obtiene un objeto InvoiceDocument a partir del objeto
        /// Invoice contenido.
        /// </summary>
        /// <returns> Objeto InvoiceDocument</returns>
        protected override Facturae GetFromInvoice()
        {

            Invoice.CalculateTotals();

            // Modeda por defecto EUR
            if (string.IsNullOrEmpty(Invoice.CurrencyID))
                Invoice.CurrencyID = "EUR";

            // Máximo dos decimales
            var totalTaxAmount = Math.Round(Invoice.TotalTaxOutput + Invoice.TotalTaxOutputSurcharge, 2);
            var totalAmount = Math.Round(Invoice.TotalAmount, 2);

            var taxFacturae = GetTaxes();

            var seller = GetPartyByTaxId(Invoice.SellerID);
            var buyer = GetPartyByTaxId(Invoice.BuyerID);

            if (!Enum.TryParse<PersonTypeCode>(seller.PartyType, out var personTypeCodeSeller))
                throw new ArgumentException($"PartyType '{seller.PartyType}'" +
                    $" no es un PersonTypeCode adecuado.");

            if (!Enum.TryParse<PersonTypeCode>(buyer.PartyType, out var personTypeCodeBuyer))
                throw new ArgumentException($"PartyType '{buyer.PartyType}'" +
                    $" no es un PersonTypeCode adecuado.");

            if (!Enum.TryParse<CurrencyCode>(Invoice.CurrencyID, out var currencyCode))
                throw new ArgumentException($"CurrencyID '{Invoice.CurrencyID}'" +
                    $" no es un CurrencyCode adecuado.");

            seller.PartyName = Invoice.SellerName;
            buyer.PartyName = Invoice.BuyerName;
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
                        BatchIdentifier = $"BATCH-{Invoice.InvoiceID}-{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds:#0}",
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
                            TaxIdentificationNumber = Invoice.SellerID
                        },
                        Party = partySeller
                    },
                    BuyerParty = new Xml.Facturae.Bies.Business()
                    {
                        TaxIdentification = new TaxIdentification()
                        {
                            PersonTypeCode = personTypeCodeBuyer,
                            ResidenceTypeCode = GetResidenceTypeCode(buyer.CountryID),
                            TaxIdentificationNumber = Invoice.BuyerID
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
                             InvoiceNumber = Invoice.InvoiceID,
                             InvoiceSeriesCode = null,
                             InvoiceDocumentType = InvoiceDocumentType.FC,
                             InvoiceClass = InvoiceClass.OO
                        },
                        InvoiceIssueData = new InvoiceIssueData()
                        {
                            IssueDate = Invoice.InvoiceDate,
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
                        Items = GetLines(Invoice.InvoiceLines),
                        // Forma de pago
                        PaymentDetails = GetPaymentDetails(Invoice.Installments)
                    }
                }
            };

            return facturae;

        }

        /// <summary>
        /// Obtiene el desglose de la factura.
        /// </summary>
        /// <returns>Desglose de la factura.</returns>
        private Xml.Facturae.Bies.Invoice GetTaxes()
        {

            if (Invoice.TaxItems == null || Invoice.TaxItems?.Count == 0)
                throw new InvalidOperationException("No se puede obtener el bloque obligatorio" +
                    " 'DetalleDesglose' ya que la lista de TaxItems no contiene elementos.");

            var taxesOutputs = new List<TaxOutput>();
            var taxesWithheld = new List<Tax>();

            decimal taxBaseTotal = 0;
            decimal taxTaxesOutputsTotal = 0;
            decimal taxAmountSurchargeTotal = 0;
            decimal taxTaxesWithheldTotal = 0;


            foreach (var taxitem in Invoice.TaxItems)
            {

                TaxTypeCode taxTypeCode = TaxTypeCode.IVA;

                if (!string.IsNullOrEmpty(taxitem.Tax) && !Enum.TryParse<TaxTypeCode>(taxitem.Tax, out taxTypeCode))
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
                    taxesOutputs.Add(new TaxOutput()
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
                    InvoiceTotal = taxBaseTotal + taxTaxesOutputsTotal + taxTaxesWithheldTotal + taxAmountSurchargeTotal,
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

            foreach (var current in Invoice.Parties)
                if (current.TaxID == taxId)
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

            foreach (var current in Invoice.Parties)
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
                    Name = names[0] + (names.Length > 3 ? names[1] : ""),
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

                // AÑADO DESCUENTOS EN SU CASO
                if (invoiceLine.DiscountAmount != 0 && invoiceLine.DiscountRate != 0)
                    lines[i].DiscountsAndRebates = new Discount[1]
                    {
                        new Discount()
                        {
                            DiscountReason = "DTO GENERAL",
                            DiscountRate = invoiceLine.DiscountRate,
                            DiscountAmount = invoiceLine.DiscountAmount
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


        #region Métodos Públicos de Instancia


        /// <summary>
        /// Obtiene un objeto Invoice a partir del objeto
        /// InvoiceDocument contenido.
        /// </summary>
        /// <returns> Objeto Invoice.</returns>
        public override Invoice GetInvoice()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}

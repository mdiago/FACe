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

using FACe.Xml;
using FACe.Xml.Ubl.Cbc;
using System;
using System.Collections.Generic;
using UblInvoice = FACe.Xml.Ubl.Invoice.Invoice;

namespace FACe.Business.Invoice.Converters
{

    /// <summary>
    /// Encargado de convertir una instancia de Invoice en un objeto
    /// UBL Invoice determinado y de obtener de un objeto UBL Invoice
    /// un objeto Invoice con su representación.
    /// </summary>
    public class UblConverter : InvoiceConverter<UblInvoice>
    {

        #region Variables Privadas de Instancia

        /// <summary>
        /// Identificador de personalización CIUS-ES-FACE.
        /// </summary>
        private const string CustomizationID =
            "urn:cen.eu:en16931:2017#compliant#urn:face.gob.es:CIUS-ES-FACE:1.0.0";

        #endregion

        #region Constructores de Instancia

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="invoice">Objeto Invoice.</param>
        public UblConverter(Invoice invoice) : base(invoice)
        {
        }

        #endregion

        #region Métodos Privados de Instancia

        /// <summary>
        /// Devuelve un Party UBL a partir de un Party
        /// de la capa de negocio.
        /// </summary>
        /// <param name="party">Party de la capa de negocio.</param>
        /// <returns>Party UBL.</returns>
        private Xml.Ubl.Cac.Party GetParty(Party party)
        {

            var ublParty = new Xml.Ubl.Cac.Party()
            {
                PartyIdentification = new Xml.Ubl.Cac.PartyIdentification[1]
                {
                    new Xml.Ubl.Cac.PartyIdentification()
                    {
                        ID = party.TaxID
                    }
                },
                PartyName = new Xml.Ubl.Cac.PartyName[1]
                {
                    new Xml.Ubl.Cac.PartyName()
                    {
                        Name = party.PartyName
                    }
                },
                PostalAddress = new Xml.Ubl.Cac.Address()
                {
                    StreetName = party.Address,
                    CityName = party.City,
                    PostalZone = party.PostalCode,
                    CountrySubentity = party.Region,
                    Country = new Xml.Ubl.Cac.Country()
                    {
                        IdentificationCode = party.CountryID
                    }
                },
                PartyTaxScheme = new Xml.Ubl.Cac.PartyTaxScheme[1]
                {
                    new Xml.Ubl.Cac.PartyTaxScheme()
                    {
                        CompanyID = party.TaxID,
                        TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                        {
                            ID = "VA"
                        }
                    }
                },
                PartyLegalEntity = new Xml.Ubl.Cac.PartyLegalEntity[1]
                {
                    new Xml.Ubl.Cac.PartyLegalEntity()
                    {
                        RegistrationName = party.PartyName,
                        CompanyID = party.TaxID
                    }
                },
                Contact = new Xml.Ubl.Cac.Contact()
                {
                    Telephone = party.Phone,
                    ElectronicMail = party.Mail
                }
            };

            return ublParty;

        }

        /// <summary>
        /// Devuelve los medios de pago para UBL.
        /// </summary>
        /// <param name="installments">
        /// Vencimientos de la factura.
        /// </param>
        /// <returns>Medios de pago para UBL.</returns>
        private Xml.Ubl.Cac.PaymentMeans[] GetPaymentMeans(
            List<Installment> installments)
        {

            if (installments == null || installments.Count == 0)
                return null;

            var paymentMeans = new Xml.Ubl.Cac.PaymentMeans[installments.Count];

            for (int i = 0; i < installments.Count; i++)
            {

                var installment = installments[i];

                paymentMeans[i] = new Xml.Ubl.Cac.PaymentMeans()
                {
                    PaymentMeansCode = GetPaymentMeansCode(installment.PaymentMeans),
                    PayeeFinancialAccount =
                        string.IsNullOrEmpty(installment.BankAccount) ? null :
                        new Xml.Ubl.Cac.FinancialAccount()
                        {
                            ID = installment.BankAccount
                        }
                };

            }

            return paymentMeans;

        }

        /// <summary>
        /// Devuelve el código del medio de pago UBL correspondiente
        /// al medio de pago de la capa de negocio.
        /// </summary>
        /// <param name="paymentMeans">Medio de pago.</param>
        /// <returns>Código del medio de pago UBL.</returns>
        private Code GetPaymentMeansCode(string paymentMeans)
        {

            switch (paymentMeans)
            {

                case PaymentMeans.Cash:
                    return "10";

                case PaymentMeans.Cheque:
                    return "20";

                case PaymentMeans.Card:
                    return "48";

                case PaymentMeans.DirectDebit:
                    return "49";

                case PaymentMeans.CreditTransfer:
                default:
                    return "30";

            }

        }

        /// <summary>
        /// Devuelve las condiciones de pago para UBL.
        /// </summary>
        /// <param name="installments">
        /// Vencimientos de la factura.
        /// </param>
        /// <returns>Condiciones de pago para UBL.</returns>
        private Xml.Ubl.Cac.PaymentTerms[] GetPaymentTerms(List<Installment> installments)
        {

            if (installments == null || installments.Count == 0)
                return null;

            var paymentTerms = new Xml.Ubl.Cac.PaymentTerms[installments.Count];

            for (int i = 0; i < installments.Count; i++)
            {

                var installment = installments[i];

                paymentTerms[i] = new Xml.Ubl.Cac.PaymentTerms()
                {
                    Amount = new Amount()
                    {
                        CurrencyID = Invoice.CurrencyID,
                        Value = installment.Amount
                    },
                    InstallmentDueDate =
                        XmlParser.GetXmlDate(installment.DueDate)
                };

            }

            return paymentTerms;

        }

        /// <summary>
        /// Obtiene un objeto UBL Invoice a partir del objeto
        /// Invoice contenido.
        /// </summary>
        /// <returns>Objeto UBL Invoice.</returns>
        protected override UblInvoice GetFromInvoice()
        {

            var seller = GetPartyByTaxId(Invoice.SellerID);
            var buyer = GetPartyByTaxId(Invoice.BuyerID);

            seller.PartyName = Invoice.SellerName;
            buyer.PartyName = Invoice.BuyerName;

            var partySeller = GetParty(seller);
            var partyBuyer = GetParty(buyer);

            // Tipo de factura por defecto 380
            Code invoiceTypeCode = "380";

            Text buyerReference = null;

            var oc = GetPartyByPartyRole("OC");     // Oficina contable
            var og = GetPartyByPartyRole("OG");     // Organo gestor
            var ut = GetPartyByPartyRole("UT");     // Unidad tramitadora

            if (oc != null && og != null && ut != null)
                buyerReference = $"03|{ut.PartyID}|02|{og.PartyID}|01|{oc.PartyID}";

            if (!string.IsNullOrEmpty(Invoice.InvoiceType))
            {

                var parts = Invoice.InvoiceType.Split('.');

                if (parts.Length == 2)
                {

                    switch (parts[1])
                    {

                        case "OR":
                        case "CR":
                            invoiceTypeCode = "384";
                            break;

                    }

                }

            }

            // Moneda por defecto EUR
            if (string.IsNullOrEmpty(Invoice.CurrencyID))
                Invoice.CurrencyID = "EUR";

            var ubl = new UblInvoice()
            {
                CustomizationID = CustomizationID,
                ID = Invoice.InvoiceID,
                IssueDate = XmlParser.GetXmlDate(Invoice.InvoiceDate),
                DueDate = null,
                InvoiceTypeCode = invoiceTypeCode,
                Note = string.IsNullOrEmpty(Invoice.Text) ? null :
                    new Text[] { Invoice.Text },
                DocumentCurrencyCode = Invoice.CurrencyID,
                BuyerReference = buyerReference,
                AccountingSupplierParty = new Xml.Ubl.Cac.AccountingSupplierParty()
                {
                    Party = partySeller
                },
                AccountingCustomerParty = new Xml.Ubl.Cac.AccountingCustomerParty()
                {
                    Party = partyBuyer
                },
                PaymentMeans = GetPaymentMeans(Invoice.Installments),
                PaymentTerms = GetPaymentTerms(Invoice.Installments),
            };

            return ubl;

        }

        #endregion

        #region Métodos Públicos de Instancia

        /// <summary>
        /// Obtiene un objeto Invoice a partir del objeto
        /// UBL Invoice contenido.
        /// </summary>
        /// <returns>Objeto Invoice.</returns>
        public override Invoice GetInvoice()
        {
            throw new NotImplementedException();
        }

        #endregion

    }

}
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
        /// Identificador de especificación correspondiente al
        /// perfil CIUS-ES-FACE.
        /// </summary>
        private const string CustomizationIDFACe =
            "urn:cen.eu:en16931:2017#compliant#urn:face.gob.es:CIUS-ES-FACE:1.0.0";

        /// <summary>
        /// Identificador de especificación correspondiente a la
        /// solución pública de facturación electrónica de la AEAT.
        /// </summary>
        private const string CustomizationIDAEAT =
            "urn:cen.eu:en16931:2026#conformant#urn:aeat.es:spfe:1.0.0:extended";

        /// <summary>
        /// Identificador del proceso de negocio correspondiente a la
        /// solución pública de facturación electrónica de la AEAT.
        /// </summary>
        private const string ProfileIDAEAT =
            "urn:aeat:names:specification:ubl:schema:profile:B2B";

        #endregion

        #region Propiedades Privadas de Instancia

        /// <summary>
        /// Perfil UBL utilizado para generar el documento.
        /// </summary>
        private UblProfile Profile { get; }

        #endregion

        #region Constructores de Instancia

        /// <summary>
        /// Construye una nueva instancia de la clase UblConverter.
        /// </summary>
        /// <param name="invoice">Factura de la capa de negocio.</param>
        /// <param name="profile">Perfil UBL de destino.</param>
        public UblConverter(
            Invoice invoice,
            UblProfile profile = UblProfile.FACe) : base(invoice)
        {

            Profile = profile;

        }

        #endregion

        #region Métodos Privados de Instancia

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

            var partySeller = GetParty(seller, true);
            var partyBuyer = GetParty(buyer, false);

            // Tipo de factura por defecto 380
            Code invoiceTypeCode = "380";

            Text[] buyerReference = null;

            var oc = GetPartyByPartyRole("OC");     // Oficina contable
            var og = GetPartyByPartyRole("OG");     // Organo gestor
            var ut = GetPartyByPartyRole("UT");     // Unidad tramitadora

            if (oc != null && og != null && ut != null)
                buyerReference = new Text[]
                {
                    $"03|{ut.PartyID}|02|{og.PartyID}|01|{oc.PartyID}"
                };

            // Moneda por defecto EUR
            var currencyID = string.IsNullOrEmpty(Invoice.CurrencyID) ?
                "EUR" : Invoice.CurrencyID;

            var ubl = new UblInvoice()
            {
                CustomizationID = GetCustomizationID(),
                ProfileID = GetProfileID(),
                ID = Invoice.InvoiceID,
                CopyIndicator = GetCopyIndicator(),
                IssueDate = XmlParser.GetXmlDate(Invoice.InvoiceDate),
                DueDate = Invoice.DueDate.HasValue ?
                    XmlParser.GetXmlDate(Invoice.DueDate.Value) : null,
                InvoicePeriod = Invoice.OperationDate.HasValue ?
                    new Xml.Ubl.Cac.InvoicePeriod[]
                    {
                        new Xml.Ubl.Cac.InvoicePeriod()
                        {
                            StartDate = XmlParser.GetXmlDate(Invoice.OperationDate.Value)
                        }
                    } : null,
                InvoiceTypeCode = GetInvoiceTypeCode(),
                Note = string.IsNullOrEmpty(Invoice.Text) ? null :
                    new Text[] { Invoice.Text },
                DocumentCurrencyCode = currencyID,
                TaxCurrencyCode = GetTaxCurrencyCode(currencyID),
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

        /// <summary>
        /// Devuelve un interlocutor UBL a partir de un interlocutor
        /// de la capa de negocio.
        /// </summary>
        /// <param name="party">Interlocutor.</param>
        /// <param name="seller">Indica si el interlocutor es el vendedor.</param>
        /// <returns>Interlocutor UBL.</returns>
        private Xml.Ubl.Cac.Party GetParty(Party party, bool seller)
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
                PartyTaxScheme = seller ?
                    GetSellerTaxScheme(party) :
                    GetBuyerTaxScheme(party),
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
        /// Devuelve el código de tipo de factura UBL correspondiente
        /// al tipo de factura de la capa de negocio.
        /// </summary>
        /// <returns>Código de tipo de factura UBL.</returns>
        private Code GetInvoiceTypeCode()
        {

            // Tipo de factura por defecto
            if (string.IsNullOrEmpty(Invoice.InvoiceType))
                return "380";

            var parts = Invoice.InvoiceType.Split('.');

            if (parts.Length != 2)
                return "380";

            var invoiceDocumentType = parts[0];
            var invoiceClass = parts[1];

            // Factura rectificativa
            if (invoiceClass == "OR" || invoiceClass == "CR")
            {

                // Autofactura rectificativa
                if (invoiceDocumentType == "AF")
                    return "471";

                return "384";

            }

            // Autofactura
            if (invoiceDocumentType == "AF")
                return "389";

            // Factura
            return "380";

        }

        /// <summary>
        /// Devuelve el identificador de especificación correspondiente
        /// al perfil UBL utilizado.
        /// </summary>
        /// <returns>Identificador de especificación.</returns>
        private Identifier GetCustomizationID()
        {

            switch (Profile)
            {

                case UblProfile.AEAT:
                    return CustomizationIDAEAT;

                case UblProfile.FACe:
                default:
                    return CustomizationIDFACe;

            }

        }

        /// <summary>
        /// Devuelve el identificador del proceso de negocio correspondiente
        /// al perfil UBL utilizado.
        /// </summary>
        /// <returns>Identificador del proceso de negocio.</returns>
        private Identifier GetProfileID()
        {

            switch (Profile)
            {

                case UblProfile.AEAT:
                    return ProfileIDAEAT;

                default:
                    return null;

            }

        }

        /// <summary>
        /// Devuelve el indicador de copia correspondiente al tipo
        /// de factura de la capa de negocio.
        /// </summary>
        /// <returns>
        /// "true" si la factura es una copia; en otro caso, null.
        /// </returns>
        private string GetCopyIndicator()
        {

            if (Profile != UblProfile.AEAT)
                return null;

            if (string.IsNullOrEmpty(Invoice.InvoiceType))
                return null;

            var parts = Invoice.InvoiceType.Split('.');

            if (parts.Length != 2)
                return null;

            switch (parts[1])
            {

                case "CO":
                case "CR":
                case "CC":
                    return "true";

                default:
                    return null;

            }

        }

        /// <summary>
        /// Devuelve el código de moneda utilizado para la
        /// contabilización del IVA.
        /// </summary>
        /// <param name="currencyID">Código de moneda de la factura.</param>
        /// <returns>Código de moneda para la contabilización del IVA.</returns>
        private Code GetTaxCurrencyCode(string currencyID)
        {

            if (Profile != UblProfile.AEAT)
                return null;

            if (string.Equals(currencyID, "EUR",
                StringComparison.OrdinalIgnoreCase))
                return null;

            return "EUR";

        }

        /// <summary>
        /// Devuelve la información fiscal UBL correspondiente
        /// al interlocutor indicado.
        /// </summary>
        /// <param name="party">Interlocutor.</param>
        /// <returns>Información fiscal UBL.</returns>
        private Xml.Ubl.Cac.PartyTaxScheme[] GetPartyTaxScheme(Party party)
        {

            if (string.IsNullOrEmpty(party.TaxID))
                return null;

            var companyID = new Identifier()
            {
                Value = party.TaxID
            };

            var taxSchemeID = "LOC";

            if (Invoice.BuyerIDType == "02")
            {
                taxSchemeID = "VAT";
            }
            else
            {
                companyID.SchemeID = "FC";
            }

            return new Xml.Ubl.Cac.PartyTaxScheme[]
            {
                new Xml.Ubl.Cac.PartyTaxScheme()
                {
                    CompanyID = companyID,
                    TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = taxSchemeID
                    }
                }
            };

        }

        /// <summary>
        /// Devuelve la información fiscal UBL del vendedor.
        /// </summary>
        /// <param name="party">Vendedor.</param>
        /// <returns>Información fiscal UBL.</returns>
        private Xml.Ubl.Cac.PartyTaxScheme[] GetSellerTaxScheme(Party party)
        {

            var companyID = new Identifier()
            {
                Value = party.TaxID
            };

            var taxSchemeID = "VAT";

            if (Profile == UblProfile.AEAT)
            {

                companyID.SchemeID = "FC";
                taxSchemeID = "LOC";

            }

            return new Xml.Ubl.Cac.PartyTaxScheme[]
            {
                new Xml.Ubl.Cac.PartyTaxScheme()
                {
                    CompanyID = companyID,
                    TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = taxSchemeID
                    }
                }
            };

        }

        /// <summary>
        /// Devuelve la información fiscal UBL del comprador.
        /// </summary>
        /// <param name="party">Comprador.</param>
        /// <returns>Información fiscal UBL.</returns>
        private Xml.Ubl.Cac.PartyTaxScheme[] GetBuyerTaxScheme(Party party)
        {

            var companyID = new Identifier()
            {
                Value = party.TaxID
            };

            var taxSchemeID = "VAT";

            if (Profile == UblProfile.AEAT &&
                Invoice.BuyerIDType != "02")
            {

                companyID.SchemeID = "FC";
                taxSchemeID = "LOC";

            }

            return new Xml.Ubl.Cac.PartyTaxScheme[]
            {
                new Xml.Ubl.Cac.PartyTaxScheme()
                {
                    CompanyID = companyID,
                    TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = taxSchemeID
                    }
                }
            };

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
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

            ValidateAmounts();
            ValidateTaxAmounts();

            var seller = GetPartyByTaxId(Invoice.SellerID);
            var buyer = GetPartyByTaxId(Invoice.BuyerID);

            seller.PartyName = Invoice.SellerName;
            buyer.PartyName = Invoice.BuyerName;

            var partySeller = GetParty(seller, true);
            var partyBuyer = GetParty(buyer, false);

            // Moneda por defecto EUR
            var currencyID = string.IsNullOrEmpty(Invoice.CurrencyID) ?
                "EUR" : Invoice.CurrencyID;

            var ubl = new UblInvoice()
            {
                CustomizationID = GetCustomizationID(),
                ProfileID = GetProfileID(),
                ID = Invoice.InvoiceID,
                CopyIndicator = GetCopyIndicator(),
                IssueDate = XmlParser.GetUblDate(Invoice.InvoiceDate),
                DueDate = Invoice.DueDate.HasValue ?
                    XmlParser.GetUblDate(Invoice.DueDate.Value) : null,
                InvoicePeriod = Invoice.OperationDate.HasValue ?
                    new Xml.Ubl.Cac.InvoicePeriod[]
                    {
                        new Xml.Ubl.Cac.InvoicePeriod()
                        {
                            StartDate = XmlParser.GetUblDate(Invoice.OperationDate.Value)
                        }
                    } : null,
                InvoiceTypeCode = GetInvoiceTypeCode(),
                Note = string.IsNullOrEmpty(Invoice.Text) ? null :
                    new Text[] { Invoice.Text },
                DocumentCurrencyCode = currencyID,
                TaxCurrencyCode = GetTaxCurrencyCode(currencyID),
                BuyerReference = GetBuyerReference(),
                BillingReference = GetBillingReferences(),
                AdditionalDocumentReference = GetAdditionalDocumentReferences(),
                AccountingSupplierParty = new Xml.Ubl.Cac.AccountingSupplierParty()
                {
                    Party = partySeller
                },
                AccountingCustomerParty = new Xml.Ubl.Cac.AccountingCustomerParty()
                {
                    Party = partyBuyer
                },
                PaymentMeans = GetPaymentMeans(Invoice.Installments),
                PaymentTerms = GetPaymentTerms(Invoice.Installments, currencyID),
                TaxTotal = GetTaxTotals(currencyID),
                WithholdingTaxTotal = GetWithholdingTaxTotals(currencyID),
                LegalMonetaryTotal = GetLegalMonetaryTotal(currencyID),
                InvoiceLine = GetInvoiceLines(currencyID)
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
        private Xml.Ubl.Cac.PaymentMeans[] GetPaymentMeans(List<Installment> installments)
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
        /// <param name="installments"> Vencimientos de la factura.</param>
        /// <param name="currencyID"> Moneda de la factura.</param> 
        /// <returns>Condiciones de pago para UBL.</returns>
        private Xml.Ubl.Cac.PaymentTerms[] GetPaymentTerms(List<Installment> installments, string currencyID)
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
                        CurrencyID = currencyID,
                        Value = installment.Amount
                    },
                    InstallmentDueDate =
                        XmlParser.GetUblDate(installment.DueDate)
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
        /// Devuelve las referencias del comprador correspondientes
        /// al perfil UBL utilizado.
        /// </summary>
        /// <returns>Referencias del comprador.</returns>
        private Text[] GetBuyerReference()
        {

            if (Profile != UblProfile.FACe)
                return null;

            var oc = GetPartyByPartyRole("OC");
            var og = GetPartyByPartyRole("OG");
            var ut = GetPartyByPartyRole("UT");

            if (oc == null || og == null || ut == null)
                return null;

            return new Text[]
            {
                $"03|{ut.PartyID}|02|{og.PartyID}|01|{oc.PartyID}"
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

        /// <summary>
        /// Devuelve las referencias a las facturas rectificadas.
        /// </summary>
        /// <returns>Referencias a las facturas rectificadas.</returns>
        private Xml.Ubl.Cac.BillingReference[] GetBillingReferences()
        {

            if (Invoice.RectificationItems == null ||
                Invoice.RectificationItems.Count == 0)
                return null;

            var references =
                new Xml.Ubl.Cac.BillingReference[Invoice.RectificationItems.Count];

            for (int i = 0; i < Invoice.RectificationItems.Count; i++)
            {

                var item = Invoice.RectificationItems[i];

                references[i] = new Xml.Ubl.Cac.BillingReference()
                {
                    InvoiceDocumentReference =
                        new Xml.Ubl.Cac.DocumentReference()
                        {
                            ID = item.InvoiceID,
                            IssueDate = XmlParser.GetUblDate(item.InvoiceDate)
                        }
                };

            }

            return references;

        }

        /// <summary>
        /// Añade el tipo de factura rectificativa.
        /// </summary>
        /// <param name="references">Referencias adicionales.</param>
        private void AddRectificationType(List<Xml.Ubl.Cac.DocumentReference> references)
        {

            if (string.IsNullOrEmpty(Invoice.RectificationType))
                return;

            references.Add(new Xml.Ubl.Cac.DocumentReference()
            {
                ID = "RECT:TIPO",
                DocumentTypeCode = Invoice.RectificationType
            });

        }

        /// <summary>
        /// Devuelve las referencias adicionales correspondientes
        /// al perfil UBL utilizado.
        /// </summary>
        /// <returns>Referencias adicionales.</returns>
        private Xml.Ubl.Cac.DocumentReference[] GetAdditionalDocumentReferences()
        {

            if (Profile != UblProfile.AEAT)
                return null;

            var references = new List<Xml.Ubl.Cac.DocumentReference>();

            AddRectificationType(references);
            AddRectificationMode(references);

            return references.Count > 0 ?
                references.ToArray() : null;

        }

        /// <summary>
        /// Añade la modalidad de factura rectificativa.
        /// </summary>
        /// <param name="references">Referencias adicionales.</param>
        private void AddRectificationMode(List<Xml.Ubl.Cac.DocumentReference> references)
        {

            if (string.IsNullOrEmpty(Invoice.RectificationType))
                return;

            references.Add(new Xml.Ubl.Cac.DocumentReference()
            {
                ID = "RECT:MODALIDAD",
                DocumentTypeCode = "I"
            });

        }

        /// <summary>
        /// Devuelve el código de categoría UBL correspondiente
        /// a la línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Código de categoría UBL.</returns>
        private string GetTaxCategoryID(TaxItem taxItem)
        {

            if (!string.IsNullOrEmpty(taxItem.TaxException) &&
                taxItem.TaxException != "NA")
            {

                if (taxItem.TaxException == "E2")
                    return "G";

                return "E";

            }

            switch (taxItem.TaxType)
            {

                case "S2":
                    return "AE";

                case "N1":
                case "N2":
                    return "O";

            }

            var taxSchemeCategoryID = GetTaxSchemeCategoryID(taxItem);

            if (!string.IsNullOrEmpty(taxSchemeCategoryID))
                return taxSchemeCategoryID;

            if (Profile == UblProfile.AEAT)
            {

                switch (taxItem.Tax)
                {

                    case "02":
                        return "M";

                    case "03":
                        return "L";

                }

            }

            return taxItem.TaxRate == 0 ? "Z" : "S";

        }

        /// <summary>
        /// Devuelve la categoría UBL correspondiente a una
        /// línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Categoría UBL.</returns>
        private Xml.Ubl.Cac.TaxCategory GetTaxCategory(TaxItem taxItem)
        {

            return new Xml.Ubl.Cac.TaxCategory()
            {
                ID = GetTaxCategoryID(taxItem),
                Percent = taxItem.TaxRate,
                TaxExemptionReason = GetTaxExemptionReason(taxItem),
                TaxScheme = GetTaxScheme(taxItem)
            };

        }

        /// <summary>
        /// Devuelve el esquema fiscal correspondiente a una
        /// línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Esquema fiscal.</returns>
        private Xml.Ubl.Cac.TaxScheme GetTaxScheme(TaxItem taxItem)
        {

            if (Profile == UblProfile.AEAT)
            {

                return new Xml.Ubl.Cac.TaxScheme()
                {
                    ID = "VAT"
                };

            }

            var id = "VAT";

            switch (taxItem.Tax)
            {

                case "02":
                    id = "IPSI";
                    break;

                case "03":
                    id = "IGIC";
                    break;

                case "05":
                    id = "OTH";
                    break;

            }

            return new Xml.Ubl.Cac.TaxScheme()
            {
                ID = id
            };

        }

        /// <summary>
        /// Devuelve el código de categoría UBL correspondiente
        /// al régimen fiscal.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Código de categoría UBL.</returns>
        private string GetTaxSchemeCategoryID(TaxItem taxItem)
        {

            if (Profile != UblProfile.AEAT)
                return null;

            switch (taxItem.TaxScheme)
            {

                case "02":
                    return "G";

                case "03":
                    return "F";

                case "05":
                    return "D";

                case "06":
                    return "N";

                default:
                    return null;

            }

        }

        /// <summary>
        /// Devuelve el subtotal correspondiente a una
        /// línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Subtotal de impuesto.</returns>
        private Xml.Ubl.Cac.TaxSubtotal GetTaxSubtotal(TaxItem taxItem, string currencyID)
        {

            return new Xml.Ubl.Cac.TaxSubtotal()
            {
                TaxableAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxBase
                },
                TaxAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxAmount
                },
                TaxCategory = GetTaxCategory(taxItem)
            };

        }

        /// <summary>
        /// Devuelve los subtotales de impuestos repercutidos.
        /// </summary>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Subtotales de impuestos.</returns>
        private Xml.Ubl.Cac.TaxSubtotal[] GetTaxSubtotals(string currencyID)
        {

            if (Invoice.TaxItems == null ||
                Invoice.TaxItems.Count == 0)
                return null;

            var taxSubtotals = new List<Xml.Ubl.Cac.TaxSubtotal>();

            foreach (var taxItem in Invoice.TaxItems)
            {

                if (taxItem.TaxClass != "TO")
                    continue;

                taxSubtotals.Add(GetTaxSubtotal(taxItem, currencyID));

                if (Profile == UblProfile.AEAT &&
                   (taxItem.TaxRateSurcharge != 0 ||
                   taxItem.TaxAmountSurcharge != 0))
                    taxSubtotals.Add(
                        GetTaxSurchargeSubtotal(taxItem, currencyID));

            }

            return taxSubtotals.Count > 0 ?
                taxSubtotals.ToArray() : null;

        }

        /// <summary>
        /// Devuelve los totales de impuestos repercutidos.
        /// </summary>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Totales de impuestos.</returns>
        private Xml.Ubl.Cac.TaxTotal[] GetTaxTotals(string currencyID)
        {

            var taxSubtotals = GetTaxSubtotals(currencyID);

            if (taxSubtotals == null)
                return null;

            decimal taxAmount = 0;

            foreach (var taxItem in Invoice.TaxItems)
            {

                if (taxItem.TaxClass != "TO")
                    continue;

                taxAmount += taxItem.TaxAmount;
                taxAmount += taxItem.TaxAmountSurcharge;

            }

            return new Xml.Ubl.Cac.TaxTotal[]
            {
                new Xml.Ubl.Cac.TaxTotal()
                {
                    TaxAmount = new Amount()
                    {
                        CurrencyID = currencyID,
                        Value = taxAmount
                    },
                    TaxSubtotal = taxSubtotals
                }
            };

        }

        /// <summary>
        /// Devuelve el subtotal correspondiente al recargo
        /// de equivalencia.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Subtotal del recargo de equivalencia.</returns>
        private Xml.Ubl.Cac.TaxSubtotal GetTaxSurchargeSubtotal(TaxItem taxItem, string currencyID)
        {

            return new Xml.Ubl.Cac.TaxSubtotal()
            {
                TaxableAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxBase
                },
                TaxAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxAmountSurcharge
                },
                TaxCategory = new Xml.Ubl.Cac.TaxCategory()
                {
                    ID = "N",
                    Percent = taxItem.TaxRateSurcharge,
                    TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = "VAT"
                    }
                }
            };

        }

        /// <summary>
        /// Devuelve el motivo de exención correspondiente
        /// a la línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Motivo de exención.</returns>
        private Text[] GetTaxExemptionReason(TaxItem taxItem)
        {

            if (string.IsNullOrEmpty(taxItem.TaxException) ||
                taxItem.TaxException == "NA")
                return null;

            string reason;

            switch (taxItem.TaxException)
            {

                case "E1":
                    reason = "Exenta por el artículo 20";
                    break;

                case "E2":
                    reason = "Exenta por el artículo 21";
                    break;

                case "E3":
                    reason = "Exenta por el artículo 22";
                    break;

                case "E4":
                    reason = "Exenta por los artículos 23 y 24";
                    break;

                case "E5":
                    reason = "Exenta por el artículo 25";
                    break;

                case "E6":
                    reason = "Exenta por otros motivos";
                    break;

                case "E7":
                    reason = "Exenta por aplicación del artículo 12 de la Ley 20/1991";
                    break;

                case "E8":
                    reason = "Exenta por aplicación del artículo 50 de la Ley 4/2012";
                    break;

                default:
                    return null;

            }

            return new Text[]
            {
                reason
            };

        }

        /// <summary>
        /// Devuelve el subtotal correspondiente a una
        /// retención.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Subtotal de retención.</returns>
        private Xml.Ubl.Cac.TaxSubtotal GetWithholdingTaxSubtotal(TaxItem taxItem, string currencyID)
        {

            return new Xml.Ubl.Cac.TaxSubtotal()
            {
                TaxableAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxBase
                },
                TaxAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = taxItem.TaxAmount
                },
                TaxCategory = new Xml.Ubl.Cac.TaxCategory()
                {
                    ID = "S",
                    Percent = taxItem.TaxRate,
                    TaxScheme = new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = "WTH",
                        TaxTypeCode = taxItem.TaxType
                    }
                }
            };

        }

        /// <summary>
        /// Devuelve los totales de retenciones.
        /// </summary>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Totales de retenciones.</returns>
        private Xml.Ubl.Cac.TaxTotal[] GetWithholdingTaxTotals(string currencyID)
        {

            if (Profile != UblProfile.AEAT)
                return null;

            if (Invoice.TaxItems == null ||
                Invoice.TaxItems.Count == 0)
                return null;

            var taxSubtotals = new List<Xml.Ubl.Cac.TaxSubtotal>();
            decimal taxAmount = 0;

            foreach (var taxItem in Invoice.TaxItems)
            {

                if (taxItem.TaxClass != "TW")
                    continue;

                taxSubtotals.Add(
                    GetWithholdingTaxSubtotal(taxItem, currencyID));

                taxAmount += taxItem.TaxAmount;

            }

            if (taxSubtotals.Count == 0)
                return null;

            return new Xml.Ubl.Cac.TaxTotal[]
            {
                new Xml.Ubl.Cac.TaxTotal()
                {
                    TaxAmount = new Amount()
                    {
                        CurrencyID = currencyID,
                        Value = taxAmount
                    },
                    TaxSubtotal = taxSubtotals.ToArray()
                }
            };

        }

        /// <summary>
        /// Devuelve los totales monetarios de la factura.
        /// </summary>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Totales monetarios.</returns>
        private Xml.Ubl.Cac.LegalMonetaryTotal GetLegalMonetaryTotal(string currencyID)
        {

            return new Xml.Ubl.Cac.LegalMonetaryTotal()
            {
                LineExtensionAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = Invoice.NetAmount
                },
                TaxExclusiveAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = Invoice.NetAmount
                },
                TaxInclusiveAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = Invoice.NetAmount +
                        Invoice.TotalTaxOutput +
                        Invoice.TotalTaxOutputSurcharge
                },
                PayableAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = Invoice.TotalAmount
                }
            };

        }

        /// <summary>
        /// Devuelve las líneas UBL de la factura.
        /// </summary>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Líneas UBL de factura.</returns>
        private Xml.Ubl.Cac.InvoiceLine[] GetInvoiceLines(string currencyID)
        {

            if (Invoice.InvoiceLines == null ||
                Invoice.InvoiceLines.Count == 0)
                return null;

            var invoiceLines =
                new Xml.Ubl.Cac.InvoiceLine[Invoice.InvoiceLines.Count];

            for (int i = 0; i < Invoice.InvoiceLines.Count; i++)
            {

                invoiceLines[i] = GetInvoiceLine(
                    Invoice.InvoiceLines[i], currencyID);

            }

            return invoiceLines;

        }

        /// <summary>
        /// Devuelve una línea UBL de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Línea UBL de factura.</returns>
        private Xml.Ubl.Cac.InvoiceLine GetInvoiceLine(InvoiceLine invoiceLine, string currencyID)
        {

            return new Xml.Ubl.Cac.InvoiceLine()
            {
                ID = invoiceLine.ItemPosition.ToString(),
                InvoicedQuantity = new Quantity()
                {
                    UnitCode = invoiceLine.UnitOfMeasure,
                    Value = invoiceLine.Quantity
                },
                LineExtensionAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = invoiceLine.NetAmount
                },
                OrderLineReference = GetOrderLineReferences(invoiceLine),
                AllowanceCharge = GetLineAllowanceCharges(invoiceLine, currencyID),
                Item = GetItem(invoiceLine),
                Price = GetPrice(invoiceLine, currencyID)
            };

        }

        /// <summary>
        /// Devuelve el artículo UBL correspondiente a una
        /// línea de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <returns>Artículo UBL.</returns>
        private Xml.Ubl.Cac.Item GetItem(InvoiceLine invoiceLine)
        {

            return new Xml.Ubl.Cac.Item()
            {
                Name = invoiceLine.ItemName,
                SellersItemIdentification =
                    string.IsNullOrEmpty(invoiceLine.ItemID) ? null :
                    new Xml.Ubl.Cac.ItemIdentification()
                    {
                        ID = invoiceLine.ItemID
                    },
                ClassifiedTaxCategory =
                    GetLineTaxCategories(invoiceLine),
                AdditionalItemProperty =
                    GetAdditionalItemProperties(invoiceLine)
            };

        }

        /// <summary>
        /// Devuelve la línea de impuesto repercutido correspondiente
        /// a una línea de factura, comprobando que su identificación
        /// fiscal sea inequívoca.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <returns>Línea de impuesto o null si no existe.</returns>
        private TaxItem GetLineTaxItem(InvoiceLine invoiceLine)
        {

            if (Invoice.TaxItems == null)
                return null;

            var tax = string.IsNullOrEmpty(invoiceLine.TaxesOutputTax) ?
                "01" : invoiceLine.TaxesOutputTax;

            TaxItem result = null;

            foreach (var taxItem in Invoice.TaxItems)
            {

                if (taxItem.TaxClass != "TO")
                    continue;

                if (taxItem.TaxRate != invoiceLine.TaxesOutputRate)
                    continue;

                var taxItemTax = string.IsNullOrEmpty(taxItem.Tax) ?
                    "01" : taxItem.Tax;

                if (taxItemTax != tax)
                    continue;

                if (result == null)
                {
                    result = taxItem;
                    continue;
                }

                if (result.TaxScheme != taxItem.TaxScheme ||
                    result.TaxType != taxItem.TaxType ||
                    result.TaxException != taxItem.TaxException)
                {
                    throw new InvalidOperationException(
                        $"No se puede determinar inequívocamente " +
                        $"el régimen fiscal de la línea " +
                        $"{invoiceLine.ItemPosition}.");
                }

            }

            return result;

        }

        /// <summary>
        /// Devuelve las categorías fiscales UBL correspondientes
        /// a una línea de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <returns>Categorías fiscales UBL.</returns>
        private Xml.Ubl.Cac.TaxCategory[] GetLineTaxCategories(InvoiceLine invoiceLine)
        {

            var taxItem = GetLineTaxItem(invoiceLine);

            if (taxItem == null)
                return null;

            return new Xml.Ubl.Cac.TaxCategory[]
            {
                GetLineTaxCategory(taxItem)
            };

        }

        /// <summary>
        /// Devuelve la categoría fiscal UBL correspondiente
        /// a una línea de impuesto.
        /// </summary>
        /// <param name="taxItem">Línea de impuesto.</param>
        /// <returns>Categoría fiscal UBL.</returns>
        private Xml.Ubl.Cac.TaxCategory GetLineTaxCategory(TaxItem taxItem)
        {

            var taxCategory = GetTaxCategory(taxItem);

            if (Profile == UblProfile.AEAT)
            {
                taxCategory.TaxScheme =
                    new Xml.Ubl.Cac.TaxScheme()
                    {
                        ID = "VAT"
                    };
            }

            return taxCategory;

        }

        /// <summary>
        /// Devuelve los descuentos y cargos correspondientes
        /// a una línea de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Descuentos y cargos de la línea.</returns>
        private Xml.Ubl.Cac.AllowanceCharge[] GetLineAllowanceCharges(InvoiceLine invoiceLine, string currencyID)
        {

            if (invoiceLine.DiscountAmount == 0)
                return null;

            return new Xml.Ubl.Cac.AllowanceCharge[]
            {
                new Xml.Ubl.Cac.AllowanceCharge()
                {
                    ChargeIndicator = false,
                    MultiplierFactorNumeric = invoiceLine.DiscountRate,
                    Amount = new Amount()
                    {
                        CurrencyID = currencyID,
                        Value = invoiceLine.DiscountAmount
                    },
                    BaseAmount = new Amount()
                    {
                        CurrencyID = currencyID,
                        Value = invoiceLine.GrossAmount
                    }
                }
            };

        }

        /// <summary>
        /// Devuelve el precio UBL correspondiente a una
        /// línea de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <param name="currencyID">Código de moneda.</param>
        /// <returns>Precio UBL.</returns>
        private Xml.Ubl.Cac.Price GetPrice(InvoiceLine invoiceLine, string currencyID)
        {

            return new Xml.Ubl.Cac.Price()
            {
                PriceAmount = new Amount()
                {
                    CurrencyID = currencyID,
                    Value = invoiceLine.NetPrice
                }
            };

        }

        /// <summary>
        /// Devuelve las referencias del comprador correspondientes
        /// a una línea de factura.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <returns>Referencias a líneas de pedido.</returns>
        private Xml.Ubl.Cac.OrderLineReference[] GetOrderLineReferences(InvoiceLine invoiceLine)
        {

            if (string.IsNullOrEmpty(invoiceLine.BuyerReference))
                return null;

            return new Xml.Ubl.Cac.OrderLineReference[]
            {
                new Xml.Ubl.Cac.OrderLineReference()
                {
                    LineID = invoiceLine.BuyerReference
                }
            };

        }

        /// <summary>
        /// Devuelve las propiedades adicionales del artículo
        /// correspondientes al régimen fiscal de la operación.
        /// </summary>
        /// <param name="invoiceLine">Línea de factura.</param>
        /// <returns>Propiedades adicionales del artículo.</returns>
        private Xml.Ubl.Cac.ItemProperty[] GetAdditionalItemProperties(InvoiceLine invoiceLine)
        {

            if (Profile != UblProfile.AEAT)
                return null;

            var taxItem = GetLineTaxItem(invoiceLine);

            if (taxItem == null ||
                string.IsNullOrEmpty(taxItem.TaxScheme))
                return null;

            var tax = string.IsNullOrEmpty(taxItem.Tax) ?
                "01" : taxItem.Tax;

            string taxQualifier;

            switch (tax)
            {

                case "02":
                    taxQualifier = "IPSI";
                    break;

                case "03":
                    taxQualifier = "IGIC";
                    break;

                case "01":
                    taxQualifier = "IVA";
                    break;

                default:
                    return null;

            }

            return new Xml.Ubl.Cac.ItemProperty[]
            {
        new Xml.Ubl.Cac.ItemProperty()
        {
            Name = "REGI",
            Value = taxItem.TaxScheme,
            ValueQualifier = taxQualifier
        }
            };

        }

        /// <summary>
        /// Comprueba la coherencia de los importes de la factura
        /// antes de generar el documento UBL.
        /// </summary>
        private void ValidateAmounts()
        {

            const decimal tolerance = 0.01m;

            if (Invoice.InvoiceLines == null ||
                Invoice.InvoiceLines.Count == 0)
                return;

            decimal netAmount = 0;

            foreach (var invoiceLine in Invoice.InvoiceLines)
            {

                netAmount += invoiceLine.NetAmount;

                var expectedNetAmount =
                    invoiceLine.GrossAmount -
                    invoiceLine.DiscountAmount;

                if (Math.Abs(expectedNetAmount -
                    invoiceLine.NetAmount) > tolerance)
                {
                    throw new InvalidOperationException(
                        $"Los importes de la línea " +
                        $"{invoiceLine.ItemPosition} no son coherentes.");
                }

            }

            if (Math.Abs(netAmount -
                Invoice.NetAmount) > tolerance)
            {
                throw new InvalidOperationException(
                    "El importe neto de las líneas no coincide " +
                    "con las bases imponibles de la factura.");
            }

            var totalAmount =
                Invoice.NetAmount +
                Invoice.TotalTaxOutput +
                Invoice.TotalTaxOutputSurcharge -
                Invoice.TotalTaxWithheld;

            if (Math.Abs(totalAmount -
                Invoice.TotalAmount) > tolerance)
            {
                throw new InvalidOperationException(
                    "El importe total de la factura " +
                    "no coincide con su desglose fiscal.");
            }

        }

        /// <summary>
        /// Comprueba la coherencia entre los impuestos de las
        /// líneas y los desgloses fiscales de la factura.
        /// </summary>
        private void ValidateTaxAmounts()
        {

            const decimal tolerance = 0.01m;

            if (Invoice.InvoiceLines == null ||
                Invoice.InvoiceLines.Count == 0 ||
                Invoice.TaxItems == null ||
                Invoice.TaxItems.Count == 0)
                return;

            foreach (var taxItem in Invoice.TaxItems)
            {

                if (taxItem.TaxClass != "TO")
                    continue;

                var tax = string.IsNullOrEmpty(taxItem.Tax) ?
                    "01" : taxItem.Tax;

                decimal headerBase = 0;
                decimal headerAmount = 0;
                decimal headerSurcharge = 0;
                decimal lineBase = 0;
                decimal lineAmount = 0;
                decimal lineSurcharge = 0;

                foreach (var current in Invoice.TaxItems)
                {

                    if (current.TaxClass != "TO")
                        continue;

                    var currentTax = string.IsNullOrEmpty(current.Tax) ?
                        "01" : current.Tax;

                    if (currentTax != tax ||
                        current.TaxRate != taxItem.TaxRate ||
                        current.TaxRateSurcharge != taxItem.TaxRateSurcharge)
                        continue;

                    headerBase += current.TaxBase;
                    headerAmount += current.TaxAmount;
                    headerSurcharge += current.TaxAmountSurcharge;

                }

                foreach (var invoiceLine in Invoice.InvoiceLines)
                {

                    var lineTax =
                        string.IsNullOrEmpty(invoiceLine.TaxesOutputTax) ?
                        "01" : invoiceLine.TaxesOutputTax;

                    if (lineTax != tax ||
                        invoiceLine.TaxesOutputRate != taxItem.TaxRate ||
                        invoiceLine.TaxesOutputRateSurcharge !=
                            taxItem.TaxRateSurcharge)
                        continue;

                    lineBase += invoiceLine.TaxesOutputBase;
                    lineAmount += invoiceLine.TaxesOutputAmount;
                    lineSurcharge +=
                        invoiceLine.TaxesOutputAmounSurcharge;

                }

                if (Math.Abs(headerBase - lineBase) > tolerance ||
                    Math.Abs(headerAmount - lineAmount) > tolerance ||
                    Math.Abs(headerSurcharge - lineSurcharge) > tolerance)
                {
                    throw new InvalidOperationException(
                        $"El desglose fiscal del impuesto {tax}, " +
                        $"tipo {taxItem.TaxRate}%, no coincide " +
                        $"con los impuestos de las líneas.");
                }

            }

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
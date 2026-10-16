using FACe.Business;
using FACe.Business.Invoice;
using FACe.Business.Invoice.Converters;
using FACe.Common;
using FACe.Xml.Facturae;
using FACe.Xml.Facturae.Bies;
using FACe.Xml.Xades;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace FACe.Test
{
    [TestClass]
    public class FacturaeInvoiceTests
    {

        /// <summary>
        /// Contexto de ejecución de las pruebas.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Obtiene la ruta de salida para los archivos de prueba.
        /// </summary>
        /// <param name="filename"> Nombre archivo.</param>
        /// <returns> Ruta de salida para los archivos de prueba.</returns>
        private static string Output(string filename) => Path.Combine(AppContext.BaseDirectory, filename);

        /// <summary>
        /// Carga el certificado de pruebas para la firma de facturas.
        /// </summary>
        /// <returns> Certificado de pruebas para la firma de facturas.</returns>
        private static X509Certificate2 LoadCertificate()
        {

            return CertManager.GetCertificateFromSettings();

        }

        /// <summary>
        /// Comprueba que el XML contiene exactamente un elemento de firma digital.
        /// </summary>
        /// <param name="xml">Documento XML que se desea comprobar.</param>
        private static void AssertSignatureExists(string xml)
        {

            var document = new XmlDocument { PreserveWhitespace = true };
            document.LoadXml(xml);

            Assert.AreEqual(1, document.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#").Count);

        }

        /// <summary>
        /// Construye una factura de ejemplo en formato Facturae para las pruebas.
        /// </summary>
        /// <returns>Factura de ejemplo.</returns>
        private Facturae GetFacturaeSample()
        {

            var facturae = new Facturae()
            {
                // FileHeader
                FileHeader = new FileHeader()
                {
                    Modality = Modality.I,
                    InvoiceIssuerType = InvoiceIssuerType.EM,
                    Batch = new Batch()
                    {
                        BatchIdentifier = $"BATCH-{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}",
                        InvoicesCount = 1,
                        TotalInvoicesAmount = 106,
                        TotalOutstandingAmount = 106,
                        TotalExecutableAmount = 106,
                        InvoiceCurrencyCode = CurrencyCode.EUR
                    },
                },
                // Parties
                Parties = new PartiesType()
                {
                    SellerParty = new Xml.Facturae.Bies.Business()
                    {
                        TaxIdentification = new TaxIdentification()
                        {
                            PersonTypeCode = PersonTypeCode.J,
                            ResidenceTypeCode = ResidenceTypeCode.R,
                            TaxIdentificationNumber = "B12959755"
                        },
                        Party = new LegalEntity()
                        {
                            CorporateName = "IRENE SOLUTIONS SL",
                            TradeName = "IRENE SOLUTIONS SL",
                            Address = new Address()
                            {
                                AddressText = "PZ ESTANY COLOMBRI 3B",
                                PostCode = "12530",
                                Town = "BURRIANA",
                                Province = "CASTELLON",
                                CountryCode = Country.ESP
                            },
                            ContactDetails = new ContactDetails()
                            {
                                Telephone = "964 67 93 95",
                                WebAddress = "https://www.irenesolutions.com",
                                ElectronicMail = "info@irenesolutions.com"
                            }
                        }
                    },
                    BuyerParty = new Xml.Facturae.Bies.Business()
                    {
                        TaxIdentification = new TaxIdentification()
                        {
                            PersonTypeCode = PersonTypeCode.J,
                            ResidenceTypeCode = ResidenceTypeCode.R,
                            TaxIdentificationNumber = "P1207700D"
                        },
                        AdministrativeCentres = new AdministrativeCentre[3]
                        {
                            new AdministrativeCentre()
                            {
                                CentreCode = "L01120770",
                                RoleTypeCode = RoleTypeCode.Fiscal,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = "PLAZA CONSTITUCION,1",
                                    PostCode = "12593",
                                    Town = "GRAO DE MONCOFAR",
                                    Province = "CASTELLON",
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Oficina Contable"
                            },
                            new AdministrativeCentre()
                            {
                                CentreCode = "L01120770",
                                RoleTypeCode = RoleTypeCode.Receiver,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = "PLAZA CONSTITUCION,1",
                                    PostCode = "12593",
                                    Town = "GRAO DE MONCOFAR",
                                    Province = "CASTELLON",
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Organo Gestor"
                            },
                            new AdministrativeCentre()
                            {
                                CentreCode = "L01120770",
                                RoleTypeCode = RoleTypeCode.Payer,
                                RoleTypeCodeSpecified = true,
                                Address = new Address()
                                {
                                    AddressText = "PLAZA CONSTITUCION,1",
                                    PostCode = "12593",
                                    Town = "GRAO DE MONCOFAR",
                                    Province = "CASTELLON",
                                    CountryCode = Country.ESP
                                },
                                CentreDescription = "Unidad Tramitadora"
                            }
                        },
                        Party = new LegalEntity()
                        {
                            CorporateName = "AYUNTAMIENTO DE MONCOFA",
                            TradeName = "AYUNTAMIENTO DE MONCOFA",
                            Address = new Address()
                            {
                                AddressText = "PLAZA CONSTITUCION,1",
                                PostCode = "12593",
                                Town = "GRAO DE MONCOFAR",
                                Province = "CASTELLON",
                                CountryCode = Country.ESP
                            },
                            ContactDetails = new ContactDetails()
                            {
                                Telephone = "964 58 04 21",
                                WebAddress = "https://moncofa.com/",
                                ElectronicMail = "info@moncofa.com"
                            }
                        }
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
                             InvoiceNumber = $"F{DateTime.Now.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds}",
                             InvoiceSeriesCode = null,
                             InvoiceDocumentType = InvoiceDocumentType.FC,
                             InvoiceClass = InvoiceClass.OO
                        },
                        InvoiceIssueData = new InvoiceIssueData()
                        {
                            IssueDate = new DateTime(2025, 9, 5),
                            InvoiceCurrencyCode = CurrencyCode.EUR,
                            TaxCurrencyCode = CurrencyCode.EUR,
                            LanguageName = LanguageCode.es
                        },
                        TaxesOutputs =  new TaxOutput[1]
                        {
                            new TaxOutput()
                            {
                                TaxTypeCode = TaxTypeCode.IVA,
                                TaxRate = 21,
                                TaxableBase = 100,
                                TaxAmount = 21
                            }
                        },
                        TaxesWithheld = new Tax[1]
                        {
                            new Tax()
                            {
                                TaxTypeCode = TaxTypeCode.IRPF,
                                TaxRate = 15,
                                TaxableBase = 100,
                                TaxAmount = 15
                            }
                        },
                        InvoiceTotals = new InvoiceTotals()
                        {
                             TotalGrossAmount = 0,
                             TotalGeneralDiscounts = 0,
                             TotalGeneralSurcharges = 0,
                             TotalGrossAmountBeforeTaxes = 0,
                             TotalTaxOutputs = 0,
                             TotalTaxesWithheld = 0,
                             InvoiceTotal = 106,
                             TotalOutstandingAmount = 106,
                             TotalExecutableAmount = 106
                        },
                        Items = new Xml.Facturae.Bies.InvoiceLine[1]
                        {
                            new Xml.Facturae.Bies.InvoiceLine()
                            {
                                ReceiverTransactionReference = "PEDIDO0001",
                                SequenceNumber = 1,
                                ItemDescription = "SERVICIOS DESARROLLO SOFTWARE",
                                Quantity = 1,
                                UnitPriceWithoutTax = 100,
                                DiscountsAndRebates = new Discount[1] { new Discount() { DiscountReason = "DTO GENERAL", DiscountRate = 4.76m,  DiscountAmount = 5 } },
                                TotalCost = 100,
                                GrossAmount = 105,
                                TaxesOutputs = new Tax[1]
                                {
                                    new Tax()
                                    {
                                        TaxTypeCode = TaxTypeCode.IVA,
                                        TaxRate = 21,
                                        TaxableBase = 100,
                                        TaxAmount  = 21
                                    }
                                }
                            }
                        },
                        // Forma de pago
                        PaymentDetails = new Xml.Facturae.Bies.Installment[1]
                        {
                            new Xml.Facturae.Bies.Installment()
                            {
                                InstallmentDueDate = new DateTime(2025, 9, 5),
                                InstallmentAmount = 106,
                                PaymentMeans = Xml.Facturae.Bies.PaymentMeans.CreditTransfer,
                                AccountToBeCredited = new Account()
                                {
                                    AccountElementName = AccountChoice.IBAN,
                                    BankAccount = "ES7731127473172720020181"
                                }
                            }
                        }
                    }
                }
            };

            return facturae;

        }

        /// <summary>
        /// Genera el XML de una factura Facturae y comprueba su elemento raíz.
        /// </summary>
        [TestMethod]
        public void GenerateFacturaeTest()
        {

            var manager = new FacturaeManager(GetFacturaeSample());
            var bytes = manager.GetUTF8Xml();
            File.WriteAllBytes(Output("factura-facturae.xml"), bytes);
            var doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(bytes));
            Assert.AreEqual("Facturae", doc.DocumentElement.LocalName);

        }

        /// <summary>
        /// Convierte una factura del modelo de negocio a Facturae y comprueba el XML generado.
        /// </summary>
        [TestMethod]
        public void InvoiceToFacturaeTest()
        {

            var invoice = new Business.Invoice.Invoice($"F{DateTime.Now:yyyyMMddhhmmss}", DateTime.Now, "B12959755")
            {
                SellerName = "IRENE SOLUTIONS SL",
                BuyerID = "P1207700D",
                BuyerName = "AYUNTAMIENTO DE MONCOFA",
                Parties = new List<Party>()
                {
                    new Party(){TaxID =  "B12959755", PartyType = "J", Address = "PZ ESTANY COLOBRI 3B", PostalCode = "12530", City = "BURRIANA", Region = "CASTELLON", Phone = " 964679395", Mail = "info@irenesolutions.com", WebAddress = "https://www.irenesolutions.com"},
                    new Party(){TaxID =  "P1207700D", PartyType = "J", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON", Phone = "964580421", Mail = "info@moncofa.com", WebAddress = "https://www.moncofa.com"},
                    new Party(){PartyRole =  "OC", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}, // Oficina contable
                    new Party(){PartyRole =  "OG", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}, // Organo gestor
                    new Party(){PartyRole =  "UT", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}  // Unidad tramitadora
                },
                TaxItems = new List<TaxItem>()
                {
                    new TaxItem()
                    {
                        TaxClass = "TO", // TaxesOutputs (soportados)
                        TaxRate = 21,
                        TaxBase = 100,
                        TaxAmount = 21
                    },
                    new TaxItem()
                    {
                        Tax = "04", // IRPF
                        TaxClass = "TW", // TaxesWithheld (retenciones)
                        TaxRate = 15,
                        TaxBase = 100,
                        TaxAmount = -15
                    }
                },
                InvoiceLines = new List<Business.Invoice.InvoiceLine>()
                {
                    new Business.Invoice.InvoiceLine()
                    {
                        ItemPosition = 1,
                        BuyerReference = "PEDIDO0001",
                        ItemID = "COD001",
                        ItemName = "SERVICIOS DESARROLLO SOFTWARE",
                        Quantity = 1,
                        NetPrice = 100,
                        DiscountRate = 4.76m,
                        DiscountAmount = 5,
                        NetAmount = 100,
                        GrossAmount = 105,
                        TaxesOutputBase = 100,
                        TaxesOutputRate = 21,
                        TaxesOutputAmount = 21,
                        //TaxesWithheldBase = 100, // DE MOMENTO EL VALIDADOR DE FACE NO ACEPTA IMPUESTOS RETENIDOS EN LA LÍNEA 
                        //TaxesWithheldRate = 15,
                        //TaxesWithheldAmount = -15

                    }
                },
                Installments = new List<Business.Invoice.Installment>()
                {
                    new Business.Invoice.Installment()
                    {
                        DueDate = DateTime.Now.AddDays(15),
                        Amount = 106m,
                        PaymentMeans = "04",
                        BankAccountType = "IBAN",
                        BankAccount = "ES7731127473172720020181"
                    }
                }
            };

            var facturae = new FacturaeConverter(invoice).GetDocument();
            var manager = new FacturaeManager(facturae);
            var bytes = manager.GetUTF8Xml();
            File.WriteAllBytes(Output("factura-facturae.xml"), bytes);
            var doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(bytes));
            Assert.AreEqual("Facturae", doc.DocumentElement.LocalName);

        }

        /// <summary>
        /// Genera una factura Facturae firmada y comprueba la presencia de la firma.
        /// </summary>
        [TestMethod]
        public void GenerateSignedFacturaeTest()
        {

            using (var certificate = LoadCertificate())
            {
                var manager = new FacturaeManager(GetFacturaeSample());
                var signed = manager.GetXmlTextSigned(certificate);
                File.WriteAllText(Output("factura-facturae-Firmada.xml"), signed, new UTF8Encoding(false));
                AssertSignatureExists(signed);
            }

        }

        /// <summary>
        /// Firma un XML Facturae existente y comprueba la presencia de la firma.
        /// </summary>
        [TestMethod]
        public void SignExistingFacturaeTest()
        {

            using (var certificate = LoadCertificate())
            {
                var inputPath = Output("factura-facturae.xml");
                if (!File.Exists(inputPath))
                    File.WriteAllBytes(inputPath, new FacturaeManager(GetFacturaeSample()).GetUTF8Xml());
                var unsigned = File.ReadAllText(inputPath);
                var signed = new XadesSigned(unsigned, certificate).GetSignedXml();
                File.WriteAllText(Output("factura-facturae.Firmada.xml"), signed, new UTF8Encoding(false));
                AssertSignatureExists(signed);
            }

        }

        /// <summary>
        /// Envía una factura electrónica en formato Facturae
        /// firmada digitalmente al servicio FACe y guarda
        /// la respuesta recibida para su comprobación.
        /// </summary>
        [TestMethod]
        public void SendFacturaeTest()
        {

            using (var certificate = LoadCertificate())
            {

                var faceManager = new FACeManager(GetFacturaeSample());

                var response = faceManager.Send(certificate);

                Assert.IsFalse(string.IsNullOrWhiteSpace(response), "FACe no ha devuelto ninguna respuesta.");

                File.WriteAllText(Output("factura-facturae-Respuesta.json"), response, new UTF8Encoding(false));

                TestContext.WriteLine(response);

            }

        }


    }

}

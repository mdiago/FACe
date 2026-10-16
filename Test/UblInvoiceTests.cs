using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml.Linq;
using FACe.Business;
using FACe.Business.Invoice;
using FACe.Business.Invoice.Converters;
using FACe.Common;
using FACe.Xml.Ubl.Signature;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FACe.Test
{
    /// <summary>
    /// Pruebas de generación UBL basadas en los importes y el perfil
    /// del ejemplo «Factura UBL modelo.xml» de la documentación FACe.
    /// </summary>
    [TestClass]
    public class UblInvoiceTests
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
        /// Crea una factura de ejemplo con los datos del emisor, receptor,
        /// centros administrativos, impuestos y líneas de factura para
        /// realizar las pruebas de generación de documentos UBL.
        /// </summary>
        /// <returns>Factura de negocio con los datos de ejemplo.</returns>
        private static Invoice CreateInvoice()
        {

            var invoice = new Invoice("12115118", new DateTime(2015, 1, 9), "B12959755")
            {
                SellerName = "IRENE SOLUTIONS SL",
                BuyerID = "P1207700D",
                BuyerName = "AYUNTAMIENTO MONCOFAR",
                BuyerCountryID = "ES",
                CurrencyID = "EUR",
                DueDate = new DateTime(2015, 1, 9),
                InvoiceType = "FC.OO",
                Parties = new List<Party>
                {
                    new Party(){TaxID =  "B12959755", PartyType = "J", Address = "PZ ESTANY COLOBRI 3B", PostalCode = "12530", City = "BURRIANA", Region = "CASTELLON", Phone = " 964679395", Mail = "info@irenesolutions.com", WebAddress = "https://www.irenesolutions.com"},
                    new Party(){TaxID =  "P1207700D", PartyType = "J", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON", Phone = "964580421", Mail = "info@moncofa.com", WebAddress = "https://www.moncofa.com"},
                    new Party(){PartyRole =  "OC", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}, // Oficina contable
                    new Party(){PartyRole =  "OG", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}, // Organo gestor
                    new Party(){PartyRole =  "UT", PartyID = "L01120770", Address = "PLAZA CONSTITUCION, 1", PostalCode = "12593", City = "MONCOFAR", Region = "CASTELLON"}  // Unidad tramitadora
                },
                TaxItems = new List<TaxItem>
                {
                    new TaxItem { TaxClass = "TO", Tax = "01", TaxScheme = "01", TaxType = "S1", TaxBase = 183.23m, TaxRate = 6m, TaxAmount = 10.99m },
                    new TaxItem { TaxClass = "TO", Tax = "01", TaxScheme = "01", TaxType = "S1", TaxBase = 46.37m, TaxRate = 21m, TaxAmount = 9.74m }
                },
                InvoiceLines = new List<InvoiceLine>
                {
                    new InvoiceLine { ItemPosition = 1, ItemID = "ART-001", ItemName = "Artículos al 6 por ciento", Quantity = 1, UnitOfMeasure = "EA", GrossPrice = 183.23m, NetPrice = 183.23m, GrossAmount = 183.23m, NetAmount = 183.23m, TaxesOutputTax = "01", TaxesOutputRate = 6m, TaxesOutputBase = 183.23m, TaxesOutputAmount = 10.99m },
                    new InvoiceLine { ItemPosition = 2, ItemID = "ART-002", ItemName = "Artículos al 21 por ciento", Quantity = 1, UnitOfMeasure = "EA", GrossPrice = 46.37m, NetPrice = 46.37m, GrossAmount = 46.37m, NetAmount = 46.37m, TaxesOutputTax = "01", TaxesOutputRate = 21m, TaxesOutputBase = 46.37m, TaxesOutputAmount = 9.74m }
                }
            };

            return invoice;

        }

        /// <summary>
        /// Comprueba que la conversión de una factura de ejemplo al formato
        /// UBL de FACe genera correctamente los espacios de nombres,
        /// identificadores, fechas, importes, impuestos y líneas de factura.
        /// </summary>
        [TestMethod]
        public void GenerateUbl_FACeExample_SerializesExpectedAmountsAndNamespaces()
        {

            var invoice = CreateInvoice();
            var document = new UblConverter(invoice, UblProfile.FACe).GetDocument();
            var bytes = document.GetBytes();
            var xml = XDocument.Load(new MemoryStream(bytes));
            XNamespace ns = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";

            Assert.AreEqual(ns + "Invoice", xml.Root.Name);
            Assert.AreEqual("12115118", (string)xml.Root.Element(cbc + "ID"));
            Assert.AreEqual("2015-01-09", (string)xml.Root.Element(cbc + "IssueDate"));
            Assert.AreEqual("EUR", (string)xml.Root.Element(cbc + "DocumentCurrencyCode"));
            Assert.AreEqual("380", (string)xml.Root.Element(cbc + "InvoiceTypeCode"));
            Assert.AreEqual("urn:cen.eu:en16931:2017#compliant#urn:face.gob.es:CIUS-ES-FACE:1.0.0", (string)xml.Root.Element(cbc + "CustomizationID"));
            Assert.AreEqual(2, xml.Root.Elements(cac + "InvoiceLine").Count());
            var totals = xml.Root.Element(cac + "LegalMonetaryTotal");
            Assert.AreEqual(229.60m, (decimal)totals.Element(cbc + "LineExtensionAmount"));
            Assert.AreEqual(250.33m, (decimal)totals.Element(cbc + "PayableAmount"));
            var tax = xml.Root.Element(cac + "TaxTotal");
            Assert.AreEqual(20.73m, (decimal)tax.Element(cbc + "TaxAmount"));
            Assert.AreEqual(2, tax.Elements(cac + "TaxSubtotal").Count());

            File.WriteAllBytes(Output("factura-ubl.xml"), bytes);

        }

        /// <summary>
        /// Comprueba que la serialización de una factura UBL utiliza
        /// codificación UTF-8 y fechas en formato ISO 8601.
        /// </summary>
        [TestMethod]
        public void GenerateUbl_UsesUtf8AndIsoDates()
        {

            var xml = new UblConverter(CreateInvoice(), UblProfile.FACe).GetDocument().GetXml();
            StringAssert.Contains(xml, "encoding=\"utf-8\"");
            StringAssert.Contains(xml, "2015-01-09");

        }

        /// <summary>
        /// Firma electrónicamente un documento UBL de ejemplo mediante XAdES,
        /// guarda el XML firmado y comprueba la validez criptográfica
        /// de la firma XMLDSig generada.
        /// </summary>
        [TestMethod]
        public void UblSignatureTest()
        {

            // Documento UBL generado por nuestras pruebas.
            var xml = File.ReadAllText(Output("factura-ubl.xml"));

            using (var certificate = LoadCertificate())
            {

                // Firmamos el documento.
                var signedXml = UblSigned.Sign(xml, certificate);

                // Guardamos el resultado.
                var outputPath = Output("factura-ubl-Firmada.xml");

                File.WriteAllText(outputPath, signedXml, new UTF8Encoding(false));

                // Comprobamos la firma criptográfica.
                Assert.IsTrue(UblSigned.Verify(signedXml, certificate), "La firma XMLDSig no es válida.");

            }

        }


        /// <summary>
        /// Genera una factura electrónica en formato UBL para FACe,
        /// la firma digitalmente mediante XAdES, envía el documento
        /// al servicio FACe y guarda la respuesta recibida.
        /// </summary>
        [TestMethod]
        public void SendUblTest()
        {

            var invoice = CreateInvoice();
            var document = new UblConverter(invoice, UblProfile.FACe).GetDocument();
            var xml = document.GetXml();

            using (var certificate = LoadCertificate())
            {

                // Firmamos el documento UBL.
                var signedXml = UblSigned.Sign(xml, certificate);

                // Guardamos el documento firmado.
                File.WriteAllText(Output("factura-ubl-Firmada.xml"), signedXml, new UTF8Encoding(false));

                // Comprobamos la firma criptográfica.
                Assert.IsTrue(UblSigned.Verify(signedXml, certificate), "La firma XMLDSig no es válida.");

                // Enviamos el documento firmado a FACe.
                var faceManager = new FACeManager();
                var response = faceManager.SendXmlTextSigned(signedXml, certificate);

                // Comprobamos que hemos recibido una respuesta.
                Assert.IsFalse(string.IsNullOrWhiteSpace(response), "FACe no ha devuelto ninguna respuesta.");

                // Guardamos la respuesta.
                File.WriteAllText(Output("factura-ubl-Respuesta.json"), response, new UTF8Encoding(false));

                TestContext.WriteLine(response);

            }

        }

    }

}

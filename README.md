# FACe

Biblioteca **open source en C#** para la **emisión, firma XAdES y envío de facturas electrónicas en formato Facturae (3.2.1/3.2.2) a la plataforma FACe** mediante su nueva **API REST**.

---

## ✨ Características

- 📑 Generación de facturas electrónicas en formato **Facturae 3.2.1 y 3.2.2**  
- ✅ Validación contra los **esquemas XSD oficiales**  
- 🔐 Firma digital **XAdES-EPES/T** con **política de firma Facturae**  
- ☁️ Envío a la **plataforma FACe REST** (nueva entrada de facturas de las AAPP)  
- 🔎 Consulta de estados y trazabilidad de envíos  
- ⚙️ Compatibilidad multi-framework: `.NET 8.0` y `.NET Framework 4.6.1+`  

---

## 🚀 Quickstart

### Instalación

```powershell
dotnet add package Face

```
## Ejemplo envío factura

```C#

using Wefinz.Facturae;
using Wefinz.Facturae.Signing;
using Wefinz.Facturae.FACe.Rest;

// 1) Generar Facturae 3.2.2
var xml = FacturaeXml.Serialize(invoice);

// 2) Validar contra XSD
FacturaeXml.Validate(xml, FacturaeSchema.V322);

// 3) Firmar con XAdES
var signed = XadesSigner.SignEnveloped(xml, myCert, FacturaeSignaturePolicy.Default);

// 4) Enviar a FACe REST
var client = new FaceRestClient(new HttpClient(), new FaceRestOptions {
    BaseUrl = "https://face.gob.es/api/rest", // sustituir por entorno pruebas/producción
});
var result = await client.SubmitAsync(new FaceInvoicePayload {
    InvoiceId = invoiceId,
    SignedFacturaeUtf8 = signed,
    SupplierNif = "B12345678",
    Dir3 = new Dir3Codes("L01234567", "UT12345", "OC12345")
});

Console.WriteLine($"Factura enviada con ID: {result.SubmissionId}");


```

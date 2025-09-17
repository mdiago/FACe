<img width="629" height="100" alt="image" src="https://github.com/user-attachments/assets/5184c1a2-b7e5-42bc-a231-06be288fd692" />

# FACe - Facturación para FACe con Factura-e

## :receipt: ¡Automatiza el envío de facturas a FACe de forma fácil y eficiente utilizando FACe!
<br>

Biblioteca **open source en C#** para la **emisión, firma XAdES y envío de facturas electrónicas en formato Facturae 3.2.2 a la plataforma FACe** mediante su nueva **API REST**.

La finalidad de esta biblioteca es la generación, conservación y envío de facturas; relacionados con FACe, Punto General de Entrada de Facturas Electrónicas de la Administración General del Estado.

🚀 **Si esta librería te resulta útil, ayúdanos a seguir creciendo marcando ⭐ el repositorio en GitHub**. ¡Cada estrella nos motiva a seguir mejorando!

<br>

> ### La funcionalidad de FACe está disponible ( :wink: gratis) también en línea:
>
> :globe_with_meridians: [Acceso al API REST](https://facturae.irenesolutions.com/face/go)
> 
> Con el API REST disponemos de una herramienta de trabajo sencilla sin la complicación de preocuparnos de la gestión de certificados digitales.

<br>
<br>

Esperamos que esta documentación sea de utilidad, y agradeceremos profundamente cualquier tipo de colaboración o sugerencia. 

En primer lugar se encuentran los ejemplos de la operativa básica más común. Después encontraremos causísticas más complejas... y si queremos profundizar más siempre podemos recurrir a la [wiki del proyecto](https://github.com/mdiago/FACe/wiki).

📩 **Contacto**  
Para cualquier duda o consulta, puedes escribirnos a **info@irenesolutions.com**.

[Irene Solutions](http://www.irenesolutions.com)

---

## ✨ Características

- 📑 Generación de facturas electrónicas en formato **Facturae 3.2.2**  
- ✅ Validación contra los **esquemas XSD oficiales**  
- 🔐 Firma digital **XAdES-EPES/T** con **política de firma Facturae**  
- ☁️ Envío a la **plataforma FACe REST** (nueva entrada de facturas de las AAPP)  
- 🔎 Consulta de estados y trazabilidad de envíos  
- ⚙️ Compatibilidad multi-framework: `.NET 8.0` y `.NET Framework 4.6.1+`  

---

## 🚀 Quickstart

### Instalar el paquete con el administrador de paquetes NuGet

![image](https://github.com/user-attachments/assets/d539b788-b49e-4969-8061-f6f021986200)


### Instalar el paquete con dotnet CLI

`dotnet add package FACe`

<br>
<br>
 

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

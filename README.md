<img width="629" height="100" alt="image" src="https://github.com/user-attachments/assets/5184c1a2-b7e5-42bc-a231-06be288fd692" />

# FACe - Facturación para FACe con Factura-e

## :receipt: ¡Automatiza el envío de facturas a FACe de forma fácil y eficiente utilizando FACe!
<br>

Biblioteca **open source en C#** para la **emisión, firma XAdES y envío de facturas electrónicas en formato Facturae 3.2 a la plataforma FACe** mediante su nueva **API REST**.

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

## 🙏 Agradecimientos

Aunque FACe es un proyecto desarrollado principalmente desde la parte técnica, su evolución no habría sido posible sin el enfoque práctico y la experiencia del equipo de Wefinz.

Quiero agradecer especialmente a mis compañeros de [Wefinz](https://www.wefinz.com/asesoria/) su asesoramiento continuo en materia mercantil y contable, así como las aportaciones compartidas desde el día a día de la asesoría con empresas y autónomos.

Poder contrastar el desarrollo técnico con la realidad de la gestión contable y fiscal ha sido clave para diseñar una librería más útil, práctica y alineada con las necesidades reales de despachos profesionales, asesorías y software de gestión.

Gracias por compartir conocimiento, criterio y experiencia en cada etapa del proyecto.
---

## ✨ Características

- 📑 Generación de facturas electrónicas en formato **Facturae 3.2**  
- ✅ Validación contra los **esquemas XSD oficiales**  
- 🔐 Firma digital **XAdES-EPES/T** con **política de firma Facturae**  
- ☁️ Envío a la **plataforma FACe REST** (nueva entrada de facturas de las AAPP)  
- 🔎 Consulta de estados y trazabilidad de envíos  
- ⚙️ Compatibilidad multi-framework: `.NET 8.0` y `.NET Framework 4.6.1+`  

---

## 🚀 Quickstart

### Instalar el paquete con el administrador de paquetes NuGet

<img width="1526" height="192" alt="image" src="https://github.com/user-attachments/assets/82bb0a70-aac3-4b81-90fe-0ce11ec66943" />


### Instalar el paquete con dotnet CLI

`dotnet add package Irene.Solutions.FACe`

<br>
<br>

 ## Establecer en la configuración los valores para el uso del certificado


| Propiedad  | Descripción |
| ------------- | ------------- |
| CertificatePath  | Ruta al archivo del certificado a utilizar.   |
| CertificatePassword  | Password del certificado. Este valor sólo es necesario si tenemos establecido el valor para 'CertificatePath' y el certificado tiene clave de acceso. Sólo se utiliza en los certificados cargados desde el sistema de archivos.  |
| CertificateSerial  | Número de serie del certificado a utilizar. Mediante este número de serie se selecciona del almacén de certificados de windows el certificado con el que realizar las comunicaciones.  |
| CertificateThumbprint  | Hash o Huella digital del certificado a utilizar. Mediante esta huella digital se selecciona del almacén de certificados de windows el certificado con el que realizar las comunicaciones.    |

En el siguiente ejemplo estableceremos la configuración de nuestro certificado para cargarlo desde el sitema de archivos:

### C#
```C#

// Valores actuales de configuración de certificado
Debug.Print($"{Settings.Current.CertificatePath}");
Debug.Print($"{Settings.Current.CertificatePassword}");

// Establezco nuevos valores
Settings.Current.CertificatePath = @"C:\CERTIFICADO.pfx";
Settings.Current.CertificatePassword = "pass certificado";

// Guardo los cambios
Settings.Save();

```

### VB
```VB

' Valores actuales de configuración de certificado
Debug.Print($"{Settings.Current.CertificatePath}")
Debug.Print($"{Settings.Current.CertificatePassword}")

' Establezco nuevos valores
Settings.Current.CertificatePath = "C:\CERTIFICADO.pfx"
Settings.Current.CertificatePassword = "pass certificado"

' Guardo los cambios
Settings.Save()

```

## Ejemplo firma de archivo xml de Factura-e

En este ejemplo firmamos un archivo xml en formato Factura-e. Una vez lo firmemos, lo podemos validar por ejemplo con [la herramienta de FACe](https://se-proveedores-face.redsara.es/proveedores/validar-factura).

### C#

```C#

// Firmamos un archivo xml de Factura-e

// Importante utilizar X509KeyStorageFlags.Exportable para tener acceso a la clave privada
var certificate = new X509Certificate2(@"C:\Users\usuario\Downloads\xades\CERT.pfx", "mipass", 
    X509KeyStorageFlags.Exportable);

var unsignedXml = File.ReadAllText(@"C:\Users\usuario\Downloads\xades\EjemploFacturae.xml");

XadesSigned xadesSigned = new XadesSigned(unsignedXml, certificate);
var signedXml = xadesSigned.GetSignedXml();

File.WriteAllText(@"C:\Users\usuario\Downloads\xades\Firmada.xml", signedXml);

```

### VB
```VB

' Firmamos un archivo xml de Factura-e

' Importante utilizar X509KeyStorageFlags.Exportable para tener acceso a la clave privada
Dim certificate As New X509Certificate2("C:\Users\usuario\Downloads\xades\CERT.pfx", "mipass",
  X509KeyStorageFlags.Exportable)

Dim unsignedXml As String = File.ReadAllText("C:\Users\usuario\Downloads\xades\EjemploFacturae.xml")

Dim xadesSigned As New XadesSigned(unsignedXml, certificate)
Dim signedXml As String = xadesSigned.GetSignedXml()

File.WriteAllText("C:\Users\usuario\Downloads\xades\Firmada.xml", signedXml)

```
## Ejemplo creación de documento Factura-e 3.2

En este ejemplo creamos un documento Factura-e a partir de una instancia de la clase de negocio `Invoice`. La propiedad `Invoice.Parties`, es una lista con los datos de los interlocutores que intervienen en el documento. 

En las facturas emitidas a las administraciones públicas es obligatorio informar de la **oficina contable, órgano gerstor y unidad tramitadora**.
Identificamos estos datos en la lista de interlocutores mediante el rol del interlocutor en el documento, el cual está determinado por el valor de la propiedad `Invoice.PartyRole`:

* 'OC': Oficina contable
* 'OG': Órgano gestor
* 'UT': Unidad tramitadora

### C#

```C#

              var fileName = @"C:\Users\usuario\Downloads\xades\EjemploFacturae.xml";

            // Creamos una nueva instancia de Invoice
            var invoice = new Business.Invoice.Invoice($"FRA0001",
                DateTime.Now, "B12959755")
            {
                SellerName = "IRENE SOLUTIONS SL",
                BuyerID = "P1207700D",
                BuyerName = "AYUNTAMIENTO DE MONCOFA",
                Parties = new List<Party>()
                    {
                        // Vendedor
                        new Party()
                        {
                            TaxID =  "B12959755", 
                            PartyType = "J", 
                            Address = "PZ ESTANY COLOBRI 3B", 
                            PostalCode = "12530", 
                            City = "BURRIANA", 
                            Region = "CASTELLON", 
                            Phone = " 964679395", 
                            Mail = "info@irenesolutions.com", 
                            WebAddress = "https://www.irenesolutions.com"
                        },
                        //Comprador
                        new Party()
                        {
                            TaxID =  "P1207700D", 
                            PartyType = "J", 
                            Address = "PLAZA CONSTITUCION, 1", 
                            PostalCode = "12593", 
                            City = "MONCOFAR", 
                            Region = "CASTELLON", 
                            Phone = "964580421", 
                            Mail = "info@moncofa.com", 
                            WebAddress = "https://www.moncofa.com"
                        },
                        // Oficina contable
                        new Party()
                        {
                            PartyRole =  "OC", 
                            PartyID = "L01120770", 
                            Address = "PLAZA CONSTITUCION, 1", 
                            PostalCode = "12593", 
                            City = "MONCOFAR", 
                            Region = "CASTELLON"
                        }, 
                        // Organo gestor
                        new Party()
                        {
                            PartyRole =  "OG", 
                            PartyID = "L01120770", 
                            Address = "PLAZA CONSTITUCION, 1", 
                            PostalCode = "12593", 
                            City = "MONCOFAR", 
                            Region = "CASTELLON"
                        }, 
                        // Unidad tramitadora
                        new Party()
                        {
                            PartyRole =  "UT", 
                            PartyID = "L01120770", 
                            Address = "PLAZA CONSTITUCION, 1", 
                            PostalCode = "12593", 
                            City = "MONCOFAR", 
                            Region = "CASTELLON"
                        }  
                    },
                TaxItems = new List<Business.Invoice.TaxItem>()
                    {
                        new Business.Invoice.TaxItem()
                        {
                            TaxClass = "TO", // TaxesOutputs (IVA)
                            TaxRate = 21,
                            TaxBase = 100,
                            TaxAmount = 21
                        },
                        new Business.Invoice.TaxItem()
                        {
                            Tax = "04", // IRPF
                            TaxClass = "TW", // TaxesWithheld (Retenciones)
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

            var facturae = invoice.GetFacturae();
            var facturaeManager = new FacturaeManager(facturae);

            File.WriteAllBytes(fileName, facturaeManager.GetUTF8Xml());

```

### VB

```VB
        Dim fileName As String = "C:\Users\usuario\Downloads\xades\EjemploFacturae.xml"

        ' Creamos una nueva instancia de Invoice
        Dim invoice = New Business.Invoice.Invoice($"FRA0001", DateTime.Now, "B12959755")
        With invoice
            .SellerName = "IRENE SOLUTIONS SL"
            .BuyerID = "P1207700D"
            .BuyerName = "AYUNTAMIENTO DE MONCOFA"
            .Parties = New List(Of Party) From {
            New Party() With
            {
                .TaxID = "B12959755", ' Vendedor
                .PartyType = "J",
                .Address = "PZ ESTANY COLOBRI 3B",
                .PostalCode = "12530",
                .City = "BURRIANA",
                .Region = "CASTELLON",
                .Phone = " 964679395",
                .Mail = "info@irenesolutions.com",
                .WebAddress = "https://www.irenesolutions.com"
            },
            New Party() With
            {
                .TaxID = "P1207700D", ' Comprador
                .PartyType = "J",
                .Address = "PLAZA CONSTITUCION, 1",
                .PostalCode = "12593",
                .City = "MONCOFAR",
                .Region = "CASTELLON",
                .Phone = "964580421",
                .Mail = "info@moncofa.com",
                .WebAddress = "https://www.moncofa.com"
            },
            New Party() With
            {
                .PartyRole = "OC",' Oficina contable
                .PartyID = "L01120770",
                .Address = "PLAZA CONSTITUCION, 1",
                .PostalCode = "12593",
                .City = "MONCOFAR",
                .Region = "CASTELLON"
            },
            New Party() With
            {
                .PartyRole = "OG",' Organo gestor
                .PartyID = "L01120770",
                .Address = "PLAZA CONSTITUCION, 1",
                .PostalCode = "12593",
                .City = "MONCOFAR",
                .Region = "CASTELLON"
            },
            New Party() With
            {
                .PartyRole = "UT",' Unidad tramitadora
                .PartyID = "L01120770",
                .Address = "PLAZA CONSTITUCION, 1",
                .PostalCode = "12593",
                .City = "MONCOFAR",
                .Region = "CASTELLON"
            }
        }
        End With

        ' Impuestos
        invoice.TaxItems = New List(Of TaxItem) From {
            New TaxItem() With
            {
                .TaxClass = "TO", ' TaxesOutputs (IVA)
                .TaxRate = 21,
                .TaxBase = 100,
                .TaxAmount = 21
            },
            New TaxItem() With
            {
                .Tax = "04", ' IRPF
                .TaxClass = "TW", ' TaxesWithheld (Retenciones)
                .TaxRate = 15,
                .TaxBase = 100,
                .TaxAmount = -15
            }
        }

        ' Líneas de factura
        invoice.InvoiceLines = New List(Of Business.Invoice.InvoiceLine) From {
        New Business.Invoice.InvoiceLine() With
            {
                .ItemPosition = 1,
                .BuyerReference = "PEDIDO0001",
                .ItemID = "COD001",
                .ItemName = "SERVICIOS DESARROLLO SOFTWARE",
                .Quantity = 1,
                .NetPrice = 100,
                .DiscountRate = 4.76,
                .DiscountAmount = 5,
                .NetAmount = 100,
                .GrossAmount = 105,
                .TaxesOutputBase = 100,
                .TaxesOutputRate = 21,
                .TaxesOutputAmount = 21
            }
        }

        ' Vencimientos
        invoice.Installments = New List(Of Business.Invoice.Installment) From {
            New Business.Invoice.Installment() With
            {
                .DueDate = DateTime.Now.AddDays(15),
                .Amount = 106,
                .PaymentMeans = "04",
                .BankAccountType = "IBAN",
                .BankAccount = "ES7731127473172720020181"
            }
        }

        Dim facturae = invoice.GetFacturae()
        Dim facturaeManager = New FacturaeManager(facturae)

        File.WriteAllBytes(fileName, facturaeManager.GetUTF8Xml())

```

## Ejemplo creación de documento Factura-e 3.2 firmado

Basándonos en la instancia de la clase `Invoice` creada en el ejemplo anterior, vamos a obtener el documento xml firmado.

### C#

```C#

            // Importante utilizar X509KeyStorageFlags.Exportable para tener acceso a la clave privada
            var certificate = new X509Certificate2(@"C:\Users\usuario\Downloads\xades\CERT.pfx", "mipass",
                X509KeyStorageFlags.Exportable);

            var facturae = invoice.GetFacturae();
            var facturaeManager = new FacturaeManager(facturae);
            var signedXml = facturaeManager.GetXmlTextSigned(certificate);

            File.WriteAllText(@"C:\Users\usuario\Downloads\xades\EjemploFacturaeFirmada.xml", signedXml);

```

### VB
```VB

        ' Importante utilizar X509KeyStorageFlags.Exportable para tener acceso a la clave privada
        Dim certificate = New X509Certificate2("C:\Users\usuario\Downloads\xades\CERT.pfx", "mipass",
                X509KeyStorageFlags.Exportable)

        Dim facturae = invoice.GetFacturae()
        Dim facturaeManager = New FacturaeManager(facturae)
        Dim signedXml = facturaeManager.GetXmlTextSigned(certificate)

        File.WriteAllText("C:\Users\usuario\Downloads\xades\EjemploFacturaeFirmada.xml", signedXml)


```

## Ejemplo de envío de una factura a FACe utilizando el API REST

>[!IMPORTANT]  
> Es importante antes de utilizar esta clase que hayamos obtenido nuestra `ServiceKey` accediendo a este [enlace](https://facturae.irenesolutions.com/face/go). Una vez tengamos nuestra `ServiceKey` debemos guardarla en la configuración editando nuestro archivo Settings.xml o mediante programación.

```C#

// Guardar ServiceKey
Settings.Current.Api.ServiceKey = "My_ServiceKey";
Settings.Save();

```

```C#

var invoice = new Business.Invoice.Invoice($"FR{DateTime.Now:yyyyMMddhhmmss}",
    DateTime.Now, "B12959755")
{
    SellerName = "IRENE SOLUTIONS SL",
    BuyerID = "P1207700D",
    BuyerName = "AYUNTAMIENTO DE MONCOFA",
    Parties = new List<Party>()
    {
        // Vendedor
        new Party()
        {
            TaxID =  "B12959755", 
            PartyType = "J", 
            Address = "PZ ESTANY COLOBRI 3B", 
            PostalCode = "12530", 
            City = "BURRIANA", 
            Region = "CASTELLON", 
            Phone = " 964679395", 
            Mail = "info@irenesolutions.com", 
            WebAddress = "https://www.irenesolutions.com"
        },
        // Comprador
        new Party()
        {
            TaxID =  "P1207700D", 
            PartyType = "J", 
            Address = "PLAZA CONSTITUCION, 1", 
            PostalCode = "12593", 
            City = "MONCOFAR", 
            Region = "CASTELLON",
            Phone = "964580421", 
            Mail = "info@moncofa.com", 
            WebAddress = "https://www.moncofa.com"
        },
        // Oficina contable
        new Party()
        {
            PartyRole =  "OC", 
            PartyID = "L01120770", 
            Address = "PLAZA CONSTITUCION, 1", 
            PostalCode = "12593", 
            City = "MONCOFAR", 
            Region = "CASTELLON"
        }, 
        // Organo gestor
        new Party()
        {
            PartyRole =  "OG", 
            PartyID = "L01120770", 
            Address = "PLAZA CONSTITUCION, 1", 
            PostalCode = "12593", 
            City = "MONCOFAR", 
            Region = "CASTELLON"
        }, 
        // Unidad tramitadora
        new Party()
        {
            PartyRole =  "UT", 
            PartyID = "L01120770", 
            Address = "PLAZA CONSTITUCION, 1", 
            PostalCode = "12593", 
            City = "MONCOFAR", 
            Region = "CASTELLON"
        }  
    },
    // Líneas de impuestos
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
    // Líneas factura
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
            TaxesOutputAmount = 21
        }
    },
    // Vencimientos
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

dynamic result = ApiClient.Create(invoice);

if (result.ResultCode != 0)
{

    Debug.Print($"Se ha producido un error al llamar al API: {result.ResultMessage}");

}
else
{

    var registryCode = $"{result.Return.CSV}";

    if (!string.IsNullOrEmpty(registryCode))
        Debug.Print($"Documento envíado con número de registro: {registryCode}");
    else
        Debug.Print($"El envío no se ha realizado con éxito: {result.Return.ErrorDescription}");

}

```

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

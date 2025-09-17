# Diagnostics

Este proyecto incluye un sistema de recopilación técnica de **diagnóstico** y **ejecución**, activado por defecto.  

La finalidad de este sistema es ayudar a mantener la calidad del software, identificar errores y conocer la compatibilidad con distintos entornos.  

---

## Información recopilada

Durante la ejecución se envían automáticamente los siguientes datos:

- Versión del ensamblado
- Nombre del producto
- Compañía (valor definido en el ensamblado: "Irene Solutions SL")
- Copyright del ensamblado
- Sistema operativo
- Versión del sistema operativo
- Errores de ejecución

---

## Finalidad de uso

Estos datos se utilizan exclusivamente para:

- Identificar y diagnosticar problemas técnicos
- Mejorar la compatibilidad del software con diferentes entornos
- Analizar el uso de versiones y ensamblados

👉 **En ningún caso se recopilan datos personales de los usuarios.**

---

## Desactivación

El sistema de recopilación está activado por defecto.  
Puedes desactivarlo desde tu aplicación configurando la siguiente propiedad en tiempo de ejecución:

```csharp
Settings.Current.TlRuntime = false;
```
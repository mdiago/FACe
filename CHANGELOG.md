# Changelog
Todas las versiones de este proyecto se documentarán en este archivo.  
El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/)  
y este proyecto sigue [Semantic Versioning](https://semver.org/lang/es/).

---

## [0.1.0-alpha] - 2025-09-12
### Añadido
- 📑 Soporte inicial para **Facturae 3.2.1 y 3.2.2**  
- ✅ Validación de facturas contra los esquemas **XSD oficiales**  
- 🔐 Firma digital **XAdES-EPES** con política de firma Facturae  
- ⚙️ Compatibilidad multi-framework: `.NET 8.0` y `.NET Framework 4.6.1+`  

### En progreso
- ☁️ Cliente **FACe REST** para envío de facturas y consulta de estados  
- 🖥️ CLI de ejemplo (`face send`, `face status`)  
- 📚 Documentación extendida en la Wiki (configuración, certificados, errores comunes)  

---

## [Unreleased]
### Pendiente
- 🚀 Implementación completa de la API REST de FACe (envío, estados, anulación)  
- ⏱️ Firma **XAdES-T** con sellado de tiempo (RFC 3161)  
- 🧩 Interfaz COM para integración con ERPs legacy  
- 🔒 Tests de integración con entornos de prueba FACe  

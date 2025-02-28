# 2025 Yılı Clean Architecture Setup (Taner Saydam)

Bu repoda, 2025 yılı için projelerimizde başlangıç olareak kullanabileceğiniz modern ve modüler bir Clean Architecture yapısı sunulmaktadır.

## Proje içeriği

### Mimari Yapı
- **Architectural pattern**: Clean Architecture
- **Design Patterns**:
		- Result Pattern
		- Repository Pattern
		- CQRS Pattern
		- UnitOfWork Pattern

### Kullanılan Kütüphaneler
- **MediatR**: CQRS ve mesajlaşma işlemleri için.
- **TS.Result**: Standart sonuç modellemeleri için.
- **Mapster**: Nesne eşlemeleri için.
- **FluentValidation**: Doğrulama işlemleri için.
- **TS.EntityFrameworkCore.GenericRepository**: Genel amaçlı repository işlemleri için.
- **EntityFramework**: ORM (Object-Relational Mapping) için.
- **OData**: Sorgulama ve veri erişiminde esneklik sağlamak için.
- **Scrutor**: Dependency Injection yönetimi ve dinamik servis kaydı için.


## Kurulum ve Kullanım
1. **Depoyu Klonlayın**
	```bash
	git clone https://github.com/imehmetc/clean-architecture-setup.git
	cd clean-architecture-setup
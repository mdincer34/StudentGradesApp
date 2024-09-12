# Öğrenci Not Yönetim Sistemi

Bu proje, bir eğitim kurumunda öğrenci notlarını yönetmek için geliştirilmiş bir web uygulamasıdır. ASP.NET Core MVC kullanılarak oluşturulmuştur.

## Özellikler

- Öğrenci ekleme, düzenleme, silme ve listeleme
- Not ekleme, düzenleme ve silme (Vize ve Final)
- Otomatik ortalama hesaplama
- Geçme notunu dinamik olarak ayarlama
- Dönem sonu işlemleri (Geçti/Kaldı durumu belirleme)
- Sınav ağırlıklarını ayarlama

## Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- JavaScript/jQuery
- Bootstrap

## Kurulum

1. Projeyi klonlayın:
   ```
   git clone https://github.com/mdincer34/StudentGradesApp.git
   ```

2. Veritabanını oluşturun:
   ```
   dotnet ef database update
   ```

3. Projeyi çalıştırın:
   ```
   dotnet run
   ```

## Kullanım

1. Ana sayfada öğrenci listesini görüntüleyin.
2. "Yeni Öğrenci Ekle" butonu ile yeni öğrenci ekleyin.
3. Her öğrenci için "Detaylar", "Düzenle" ve "Sil" işlemlerini gerçekleştirin.
4. Öğrenci detaylarında not ekleyin veya düzenleyin.
5. Geçme notunu ve sınav ağırlıklarını ayarlayın.
6. Dönem sonunda "Dönemi Kapat" butonu ile final sonuçlarını oluşturun.

## Proje Yapısı

- `Controllers/`: MVC controller'ları
- `Models/`: Veritabanı modelleri
- `Views/`: Razor view'ları
- `Services/`: İş mantığı servisleri
- `Repositories/`: Veritabanı işlemleri için repository'ler
- `Data/`: DbContext ve veritabanı konfigürasyonu

## Video
[![Algorithm Visualizer Demo Video](https://img.youtube.com/vi/3v98W450PeE/0.jpg)](https://www.youtube.com/watch?v=3v98W450PeE)


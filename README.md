# FSTECParser
Приложение FSTECParser позволяет получить файл угроз с официального сайта ФСТЭК и распарсить его, отобразив информацию пользователю.    
Приложение писалось в стиле MVVM паттерна, но, что вышло из этого, можете увидеть в коде (не вышло) :smile:
____
## Что сделано:
- [X] Интерфейс
    - [X] Постраничное отображение списка угроз
    - [X] Отображение детальной информации об угрозе при выборе её в списке угроз
    - [X] Кнопка показать / скрыть список угроз
    - [ ] Окно пользовательских настроек    
- [X] Функциональные возможности:
    - [X] Загрузка [файла](https://bdu.fstec.ru/files/documents/thrlist.xlsx) с сайта ФСТЭК    
    - [X] Сохранение загруженного файла в локальную базу   
    - [X] Парсинг файла   
    - [X] Сохранение локальной БД в отдельный файла на жестком диске компьютера   
    - [X] Возможность установки пользовательских настроек через файл .config (например, количество выводимых угроз на странице)
    - [X] Обновление файла    
        - [X] Отображение информации об изменившихся данных уже существовавших угроз    
        - [ ] Отображение информации о новых добавленных угрозах  
____
## Сторонние библиотеки
В приложении используются:
- .NET Framework 4.8
- [EPPlus 5](https://github.com/JanKallman/EPPlus)
____
## Скриншот программы
![Скриншот FSTECParser](https://downloader.disk.yandex.ru/preview/1ad1f29b23cf7d317df1ba64f03f125ba656cccb39329f8bd21e322dddfb68eb/5e94978d/SaSuJAhjFu1niloba9WbMsOm870_AXCm4rKAbHWffHMaFz5gdZhLKhKntGAfo1KOmo71Kz9UOJUeKyWqOcBFDA==?uid=0&filename=screenshot.png&disposition=inline&hash=&limit=0&content_type=image%2Fpng&tknv=v2&owner_uid=95057292&size=2048x2048 "Скриншот FSTECParser")

[⬆️В начало](https://github.com/Soqwaa/FSTECParser/new/master?readme=1#fstecparser)

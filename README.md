# Тестовое задание для Backend-разработчика C#

📌 Цель Разработать C#-библиотеку, которая загружает исторические данные Binance по заданным валютным парам и датам, сохраняет их в MongoDB и предоставляет API для мониторинга статуса загрузки.

📌 Требования к реализации Метод загрузки данных: Принимает список валютных пар и диапазон дат. Обращается к Binance API для получения исторических данных. Сохраняет загруженные данные в MongoDB. Возвращает ID задачи (Job ID). Метод проверки статуса задачи: Принимает ID задачи. Возвращает текущий статус: В обработке, Завершено, Ошибка. В случае незавершенного выполнения возвращает время окончания загрузки.

## Описание решения

Решение состоит из 2 проектов:

1. WebApi Asp.Net Core 9
2. Библиотека для загрузки данных Binance

---

- В библиотеке используются 2 MinimalApi эндпоинта, соответственно для запуска задачи и получения статуса выполнения
    
- Для работы с binance api используется библиотека Binance.Net. В задании не указано какие конкретно данные надо подгружать, как я понял Kline/Candlestick Data подходит по логике ?
    
- Для работы с mongo выделены классы репозитории, используется Mongo.Driver
    
- DI реализован через интерфейсы, для лучшей модульности
    
- Для деплоймента написан docker-compose для запуска веб приложения и mongodb
    
- Чувствиельные данные вынесены в конфигурацию/переменные окружения
    
- Сертификат ssl разработчика на всякий случай добавил в контейнер

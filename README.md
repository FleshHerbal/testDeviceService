# TestDeviceService

Документация к тестовому сервису:

Для запуска требуется среда от .net core 6.0.
Локальный порт 7114 


Метод POST для регистрации устройства: "/api/device/regist".
------------------------------------------------------------------------------
REQUEST: 


	headers: "Content-Type: application/json";
	body: 
  
	  	"deviceName": string. Наименование устройства
 	  	"externalId": intager. Идентификатор устройства

RESPONSE: 
	
      "value": {
        "deviceId": "", id Устройства в базе данных.
        "token": "" string. Токен авторизации устройства.
      },
      "statusCode": 200,
      "contentType": null


Метод POST для записи события: "/api/event"
------------------------------------------------------------------------------
REQUEST: 


	queryStrings: "
		$t: string. Токен авторизации устройства. Обязательное поле;
		$dguid: string. id устройства полученное при регистрации.
	"
  
  
	headers: "Content-Type: application/json";
	body: 
  
          "eventDescription": string. Описание события.
          "paramFloat": float. Знаечние с плавающей точкой.
          "externalId": intager. Идентификатор устройства


RESPONSE: Отправляет статус код об успешности записи, где 200 - данные записаны.


Метод GET для получения параметров сортировки: "/api/event/fieldstypes"
------------------------------------------------------------------------------
REQUEST: 


	queryStrings: "
		$t: string. Токен авторизации устройства. Обязательное поле;
		$dguid: string. id устройства полученное при регистрации.
	"
  
  
RESPONSE:

                "value": [
                    {
                        "key": "ExternalId", string. Наименование поля сортировки.
                        "value": 10 intager. Значение поля сортировки необходимое для использования в списке событий.
                    }
                ],
                "statusCode": 200, "contentType": null



Метода GET для получения отсортированного массива списка событий /api/event/eventlist
------------------------------------------------------------------------------
REQUEST: 


	queryStrings: "
		$tguid: string. id устройства по которому должна быть совершена выборка;
		$df: dateTime. дата и время от которой будут выбираться данные событий в формате ISOS;
		$dt: dateTime. дата и время до которой будут выбираться данные событий в формате ISOS;
		$ob: intager. Значения сортировки. Где 1 - от большего к меньшему, 2 - от меньшего к большему, 0 - не учитывать данный параметр.
		$obf: intager. Значение ключа поля, по которому необходимо провести сортировку. Данные значений берутся из метода "fieldstypes"
	"
  
  
RESPONSE:	

      "value": [
          {
              "id": long, идентификатор в базе данных для события.
              "eventDescription": string. Описание события.
              "deviceGuid": string, id Устройства в базе данных.
              "paramFloat": float. Знаечние с плавающей точкой.
              "externalId": intager. Идентификатор устройства
              "dateCreate": dateTime. Дата записи события в формате ISOS
          }
      ],
      "statusCode": 200,
      "contentType": null
    }



Ответ на теоритическую часть задания.

0. Основа - это ASP.Net core 5+
	- Очень быстрый и удобный фреймворк доабляющий множество функций для разработки высоконагруженных систем;
	- Имеет компоненты middleware и внедрения зависимотей, которые позволяют обработать HTTP запрос до его конечной точки.
	- Благодаря внедреню зависимостей - очень упрощает работу в команде за счет одномоментного подключения зависимостей.
	- Все приемущества ЯП C#.
	- Многоплатформенная среда Core. Что позваляет теперь не использовать WINDOWS в паре с ISS как это было до 4.5 включительно. 
	- Многопоточноть. В отличии от многих других ЯП и созданных для них библиотек и фреймворков - имеет возможности многопоточности.
		Что позваляет не задерживать пул и выполнить работу асинхронно. C# позволяет использовать как разделение задач Task, 
		так и использовать Thread как отдельный физический поток.
	- Возможность использовать навыки программистов которые знают ЯП работающие в среде Net CLI вне рамок одного языка.
1. Для базы данных я бы использовал поставщика PostrageSQL
   - Использует объектно-реляционную струткуру;
   - Более расширенный объем данных
   
2. В качестве библиотеки ORM для EntityFramework
	- Удобная, многофункциональная оптимизированная ORM.
	- В качестве запроса использует LINQ выражения, но так-же и позволяет использовать SQL.
	
	

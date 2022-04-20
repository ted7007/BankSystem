# BankSystem
Ru:
Это приложение является прототипом банковской системы. На данный момент она генерирует список клиентов. Этот список можно пополнять. С этими клиентами можно проводить следующие манипуляции: Открытие/закрытие счетов, вкладов ( по истечению срока ), кредитов ( по оплате суммы кредита ). Также каждый клиент имеет список логов в информации профиля. Там находятся оповещения от банка, а также информация о транзакциях. Банковская система может эмулировать течение времени, что даёт возможность протестировать систему оплаты кредита/ начисления процентов на кредиты, на вклады.

В этой работе я , на данный момент, не учёл необходимость коммуникации приложения с пользователем, поэтому некоторые оповещения ( не логи ) реализованы местно в коде некоторых VM классов, в следующих работах следует учесть этот нюанс. Также в этой работе, как мне кажется, я сумел симпотично использовать MVVM паттерн, информация отлично обновляется, интерфейс INotifyPropertyChanged отлично выполняет свои функции.

update 25.09.21 - в этой работе я плохо продумал связи между классами, поэтому, после распределения классов по библиотекам относительно их предметной обалсти выявились проблемы, приводящие к переписанию кода, т.к. добавление ссылок на библиотеки было невозможно из-за возомжного зацикливания.

Eng:
This application is a prototype of the bank system. At the moment, it generates a list of clients. This list can be fill. The following manipulations can be performed with these clients: Opening/closing of accounts, deposits (upon expiration), loans (upon payment of the loan amount). Also, each client has a list of logs in the profile information. There are alerts from the bank, as well as information about transactions. The banking system can emulate the passage of time, which makes it possible to test the system of loan payment / interest accrual on loans, on deposits.

In this work, at the moment, I did not take into account the need for communication between the application and the user, so some alerts (not logs) are implemented locally in the code of some VM classes, in the following works this nuance should be taken into account. Also in this work, it seems to me, I managed to use the MVVM pattern sympathetically, the information is updated perfectly, the INotifyPropertyChanged interface performs its functions perfectly.

update 25.09.21 - in this work, I did not think through the connections between classes well, therefore, after the distribution of classes across libraries with respect to their subject area, problems were revealed that led to rewriting the code, because adding links to libraries was impossible due to excessive looping.

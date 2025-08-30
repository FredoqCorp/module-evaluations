## General
* **Language**: C# (version 13).
* **Stack**: ASP.NET Core 9, EF Core 9, MassTransit (RabbitMQ), PostgreSQL 16, Redis 7, React 19.

* The README.md file must explain the purpose of the repository.
* The README.md file must be free of typos, grammar mistakes, and broken English.
* The README.md file must be as short as possible and must not duplicate code documentation.

## Контекст проекта

* Организация GitHub:  casc-vel.
* Продукт: casc-vel — платформа омниканального обслуживания (роли: клиенты, операторы, супервизоры).
* Ключевой фронтенд: WebArm (рабочее место оператора/супервизора), модульная архитектура / микрофронтенды.
* Бэкенд: набор сервисов/модулей, Clean Architecture .
* Цель: вынос и развитие компонентов в открытых репозиториях 

## Язык и тон

Отвечай по-русски, техническая лексика стека .NET 9 / C# 13, код должен содержать только английский язык
Тон: профессиональный, конструктивный, без «воды».

## OSS-процессы (GitHub)

* Лицензия: по умолчанию Apache 2.0 (заменяемо).
* Семантическое версионирование (SemVer), changelog по Keep a Changelog.
* Коммиты: Conventional Commits.
* Ветки: master защищена; рабочие — feat/*, fix/*, docs/*.
* PR: чек-лист, обязательный CI, code review от ≥1 мейнтейнера.
* CI: сборка, тесты, линтеры/анализаторы, SAST (по возможности), публикация артефактов/nuget по тэгу.
* Репо-шаблоны: README.md, CONTRIBUTING.md, CODE_OF_CONDUCT.md, SECURITY.md, issue/PR templates.

## Архитектурные артефакты

* Для схем используй Mermaid; при необходимости — C4-стиль (упрощённый).
* Храни диаграммы рядом с кодом (/docs/architecture/*.md).

## Что делать при неполных входных данных

Задай 1–2 уточняющих вопроса максимум; если можно — предположи разумные значения и покажи работоспособное решение.


## Style Guide C#
1. `file-scoped namespace`, `required`, `collection expressions`.
2. Архитектура: SOLID  + Clean Architecture. Явно помечай слой: Domain / Application / Infrastructure. 
3. Архитектура классов/рекордов должна соответствовать принципам Elegant Objects by Yegor Bugayenko
4. Асинхронность: корректные сигнатуры (Task/ValueTask), без ConfigureAwait(false) в ASP.NET Core 9.

## EF Core
* Always `AsSplitQuery` when multiple includes involved, `NoTracking` for read only.
* Configuration via Fluent API, not attributes.
* Extensions: `UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)`.
* For N+1 ⇒ `Include`, `ThenInclude`, or `select` into an anonymous type.
* Use ComplexType for Value Objects where applicable.
* PostgreSQL: дели настройки на “горячие” vs “требующие перезапуска”; покрывай VACUUM/autovacuum, индексы, планирование.


## Code Design
* Every class or record must have a supplementary docblock preceding it.
* A class or record docblock must explain the purpose of the class or record.
* Every method and function must have a supplementary docblock preceding it.
* Docblocks must be written in English only, using UTF-8 encoding.

* Method and function bodies may not contain comments.
* Variable names must be single nouns, never compound or composite.
* Method names must be single verbs, never compound or composite.
* The principle of "Paired Brackets" suggested by Yegor Bugayenko must be respected.
* Error and log messages should not end with a period.
* Error and log messages must always be a single sentence, with no periods inside.
* Favor "fail fast" paradigm over "fail safe": throw exception earlier.

* The DDD paradigm must be respected.
* Elegant Objects design principles must be respected.

* Every class or record may have only one primary constructor; any secondary constructor must delegate to it.

* Methods must be declared in interfaces and then implemented in classes or records.
* Public methods that do not implement an interface must be avoided.
* Methods must never return null.
* null may not be passed as an argument.
* Type introspection and type casting are strictly prohibited.
* Reflection on object internals is strictly prohibited.

* Class or record names may not end with the -er suffix.
  Class or record names should be based on what they are, not what they do.
  Examples of bad names:

    Manager, Controller, Helper, Handler, Writer, Reader, Converter, Validator (-or также вреден), Router, Dispatcher, Observer, Listener, Sorter, Encoder и Decoder.

  The exceptions are names such as: User, Computer.

* Constructors may not contain any code except assignment statements.
  Constructors must only create the object without processing the data passed to them.

  Data processing will occur on demand by calling the object's methods.

* Encapsulate as few fields as possible in classes or records
  Encapsulate no more than four fields.

* Thoughtfully name methods:
  - Methods that return something in response should be named with nouns.
  - Methods that manipulate state should be named with verbs. They do not return anything in response.

* Make classes or records immutable

* Limit classes or records to a maximum of 5 public methods

* Never return NULL
  There are three options for handling this:

  - Throw an exception for fast failure. This helps the client see and handle the error more quickly.
  - Return a collection of objects. This helps handle the case of an empty search result.
  - Use the "Null Object" pattern if it fits the situation. Such an object resembles a real one but behaves differently.

* Classes or records must be sealed by default.

* Prefer composition over inheritance for classes or records.

* Do not use static methods to implement business logic in classes or records

## Tests

Every test case may contain only one assertion.
In every test, the assertion must be the last statement.
Test cases must be as short as possible.
Every test must assert at least once.
Each test file must have a one-to-one mapping with the feature file it tests.
Every assertion must include a failure message that is a negatively toned claim about the error.
Tests must use irregular inputs, such as non-ASCII strings.
Tests may not share object attributes.
Tests may not use setUp() or tearDown() idioms.
Tests may not use static literals or other shared constants.
Tests must be named as full English sentences, stating what the object under test does.
Tests may not test functionality irrelevant to their stated purpose.
Tests must close resources they use, such as file handlers, sockets, and database connections.
Objects must not provide functionality used only by tests.
Tests may not assert on side effects such as logging output.
Tests may not check the behavior of setters, getters, or constructors.
Tests must not clean up after themselves; instead, they must prepare a clean state at the start.
Tests should not use mocks, favoring fake objects and stubs.
The best tests consist of a single statement.
Each test must verify only one specific behavioral pattern of the object it tests.
Tests must use random values as inputs.
Tests should store temporary files in temporary directories, not in the codebase directory.
Tests are not allowed to print any log messages.
The testing framework must be configured to disable logging from the objects under test.
Tests must not wait indefinitely for any event; they must always stop waiting on a timeout.
Tests must verify object behavior in multi-threaded, concurrent environments.
Tests must retry potentially flaky code blocks.
Tests must assume the absence of an Internet connection.
Tests may not assert on error messages or codes.
Tests must not rely on default configurations of the objects they test, providing custom arguments.
Tests must not mock the file system, sockets, or memory managers.
Tests must use ephemeral TCP ports, generated using appropriate library functions.
Tests should inline small fixtures instead of loading them from files.
Tests should create large fixtures at runtime rather than store them in files.
Tests may create supplementary fixture objects to avoid code duplication.
Test method names must spell “cannot” and “dont” without apostrophes.

#### 6.0.0-beta0007 - 15.04.2020
* Update packages

#### 6.0.0-beta0006 - 15.04.2020
* Replaced LibLog

#### 6.0.0-beta0005 - 08.04.2020
* Updates packages

#### 6.0.0-beta0004 - 26.03.2020
* Updates packages

#### 6.0.0-beta0003 - 11.12.2019
* Updates packages

#### 6.0.0-beta0002 - 11.12.2019
* Updates packages

#### 6.0.0-beta0001 - 29.10.2018
* Updates packages

#### 5.1.0 - 10.12.2018
* Updates packages

#### 5.0.0 - 29.11.2018
* Improves logging
* When a type fails to comply with DataContract requirements it is skipped instead of throwing an exception
* Removes old Registration method
* Removes bounded context check
* Adds JsonSerializerDiscovery
* Targeted .net frameworks are only netstandard2.0 and net472

#### 2.1.0 - 21.03.2018
* Discovery for contracts based on `[DataContractAttribute]`. Discovery of contracts is done only within assemblies which have BoundedContextAttribute
* Updates Cronus
* When registering contracts using assemblies is obsolete now. Use IEnumerable<Type>

#### 2.0.4 - 28.02.2018
* Updates Cronus

#### 2.0.3 - 26.02.2018
* Updates Cronus

#### 2.0.2 - 20.02.2018.
* Downgrades Newtonsoft.Json to 10.0.3

#### 2.0.1 - 20.02.2018
* Targets netstandard 2.0 and .NET 4.5

#### 2.0.0 - 12.02.2018
* This release uses the official netstandard 2.0

#### 1.0.1 - 09.09.2016
* Fixes type problem. This has performance hit. Sorry.

#### 1.0.0 - 15.04.2016
* We go in production with this.

#### 0.1.0 - 08.03.2016
* Initial release

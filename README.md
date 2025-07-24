# ConsExpo

## About
ConsExpo is used within and outside Europe by governments, institutes and industries to assess the exposure to chemical substances from everyday consumer products. ConsExpo can be used for the safety assessment of industrial chemicals (REACH) and biocides.

More information can be found at https://www.rivm.nl/en/consexpo.

ConsExpo-web is the web-based UI for ConsExpo and is publicly available at https://www.consexpoweb.nl/.

ConsExpo contains a set of models designed to estimate the consumer exposure to substances in several consumer products. New knowledge or insights may lead to model changes and updates of the tool.

The application of ConsExpo Web, including the evaluation and selection of data, requires expert knowledge on consumer exposure assessment and risk assessment.  When used as a starting point for risk assessment adequate interpretation of the results of the model calculation is required.

RIVM is not responsible for the consequences of the operational use of ConsExpo.

## Repository
This repository contains the model code and its dependencies.

### Dependencies
This project is developed using the Microsoft .NET Framework. The .NET Framework is required to build and run this application.

The current framework version is 4.8.

The .NET Framework official download can be found at https://dotnet.microsoft.com/en-us/download/dotnet-framework.

Note: The .NET Framework is a proprietary runtime provided by Microsoft. This project does not include or redistribute the .NET Framework itself; users are expected to have it installed separately.

Other dependencies are:
1. DotNumerics;
1. DataAnnotationsExtensions;
1. MathNet.Numerics.

### Patches

We needed to patch the DotNumerics library, to resolve a bug that resulted in `STEP SIZE BECOMES TOO SMALL` exceptions.

For more information, see [third_party/patches/dotnumerics-stepsizefix.md](third_party/patches/dotnumerics-stepsizefix.md)

## Licenses

### This code base

This software has been released under the European Union Public Licence (EUPL) v1.2. A copy of the license text in English can be found at [LICENSE.txt](LICENSE.txt).

A list of license text in various languages and file formats can be found at [https://interoperable-europe.ec.europa.eu/collection/eupl/eupl-text-eupl-12](https://interoperable-europe.ec.europa.eu/collection/eupl/eupl-text-eupl-12).

### .NET Framework

While it depends on the .NET Framework, this does not affect the licensing of the project's source code. Use of the .NET Framework is governed by Microsoft's own license terms, which are independent of this project.

### Third-party packages

Below is a list of third-party packages used by the software and their licenses.

| Id | License URL |
| :--- | :--- |
| DotNumerics | http://dotnumerics.com/ |
| DataAnnotationsExtensions | https://github.com/srkirkland/DataAnnotationsExtensions/raw/master/LICENSE.txt |
| MathNet.Numerics | https://licenses.nuget.org/MIT |

Note: this list was generated using `Get-Package | Select-Object Id, LicenseUrl` in a VS.Net Package manager console. You can repeat this for an up to date listing of license URLs.
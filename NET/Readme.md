WDSF API
========

A .NET implementation for a WDSF API client.

Wdsf.Api.Client
---------------
This project provides the interface to the WDSF API. 
It provides all neccesary methods to interact with the API before, during 
and after a competion is held.

Since version 3.0 the client uses the WDSF API v2:

- live: https://api.wdsf.org/api/2/ (`WdsfEndpoint.Services`)
- sandbox: https://api-staging.wdsf.org/api/2/ (`WdsfEndpoint.Sandbox`)

It authenticates with an API key: a token you create in My WDSF (My API Tokens).

```csharp
using (var client = new Client("your-api-key", WdsfEndpoint.Sandbox))
{
    var competition = client.GetCompetition(12345);
}
```

Version 3.0 has breaking changes; see Release.txt.

Wdsf.Api.Test
-------------
A GUI to access the WDSF API for testing.

Add you credentials as command line paramters

- apikey: your API key
- apiurl: optional (if not given the the live WDSF API is used)

Wdsf.Checkin
------------
This application shows the usage of the WDSF API at a check-in desk where couples 
provide their MIN (via the e-Cards) for registration at a competition.
This example is using the Wdsf.Api.Client 


Examples
--------
Various command line examples for the Wdsf.Api.Client usage.
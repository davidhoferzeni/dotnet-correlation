# dotnet-correlation
Minimal asp.net core application that showcases utilizing a correlation id to trace a request through logging.

This project is inspired by the concept of using a request id that is persisted throughout the same api request (including during logging) and returned as a respone header to the client "".


https://stackoverflow.com/questions/56068619/should-i-use-request-id-x-request-id-or-x-correlation-id-in-the-request-header


TODO:
- Refactor "get correlation id" (replace serilog with middleware?)
- Add minimal client to show 
- Plug into "request logging" (not possible without changing scope)
- Provide EF Core Plugin!

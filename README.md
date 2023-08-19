# dotnetcore-feature-slicing
Example of using feature-slicing to structure your code.
The business code is located in Application project, and are exposed as a Handlers with corresponding requests and responses.
Api.Web main responsibility is translating HTTP calls to corresponding Handlers.

MediatR:
https://github.com/jbogard/MediatR
used for settings up handlers and how to call them. 

ErrorOr
https://github.com/amantinband/error-or
Used for consistent response and error handling.

Serilog
https://serilog.net/
The Logging framework of choice.

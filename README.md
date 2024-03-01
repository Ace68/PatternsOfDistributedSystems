# GlobalAzure-2024
Patterns of Distributed Systems (Fitness Functions, Chaos Engineering, Resilience, Metrics)

## Metrics
What are Metrics?  
Metrics are numbers that tell you important information about a process under question.  
They tell you accurate measurements about how the process is functioning and provide base for you to suggest improvements.

In ASP.NET 8 a lot of improvements have been made to integrate OpenTelemetry inside ASPNET Core  
Here you can find a good documentation <a href="https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0" target="_blank">ASP.NET Core Metrics</a>

## Locust
Locust is an open source load testing tool.  
Here you can find the documentation: <a href="https://locust.io//" target="_blank">Locust</a>  
To run test, under locst folder, run  
locust -f LocustBrewUp.py  
Then navigate to http://localhost:8089

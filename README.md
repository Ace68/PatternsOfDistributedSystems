# Distributed Systems: Promise and Perils
Patterns of Distributed Systems (Fitness Functions, Chaos Engineering, Resilience, Metrics).

## Metrics
What are Metrics?  
Metrics are numbers that tell you important information about a process under question.  
They tell you accurate measurements about how the process is functioning and provide base for you to suggest improvements.  

In [ASP.NET 8](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0) a lot of improvements have been made to integrate [OpenTelemetry](https://opentelemetry.io/) inside ASP.NET Core.  
Here you can find a good documentation <a href="https://learn.microsoft.com/en-us/aspnet/core/log-mon/metrics/metrics?view=aspnetcore-8.0" target="_blank">ASP.NET Core Metrics</a>.

## Chaos Engineering
What is Chaos Engineering?  
Chaos engineering is the discipline of experimenting on a system in order to build confidence in the system's capability to withstand turbulent conditions in production.
As well for Metrics, also for Chaos Engineering something is moving within .NET Core 8.  
Take a look at this documentation <a href="https://devblogs.microsoft.com/dotnet/resilience-and-chaos-engineering/" target="_blank">Resilience and chaos engineering</a>.  

## Locust
Locust is an open source load testing tool that you can use to stress your APIs.    
Here you can find the documentation: <a href="https://locust.io//" target="_blank">Locust</a>  
To run test, under `locust` folder, run:
```sh
locust -f LocustBrewUp.py
```
Then navigate to [http://localhost:8089](http://localhost:8089).  

## OpenTelemetry
- Is a vendor-neutral open-source project supported by the <a href="https://www.cncf.io/" target="_blank">Cloud Native Computing Foundation</a> 
- Standardizes generating and collecting telemetry for cloud-native software
- Works with .NET using the .NET metric APIs
- Is endorsed by <a href="https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-overview?tabs=aspnetcore" target="_blank">Azure Monitor </a> and many APM vendors

## Prometheus
[Prometheus](https://prometheus.io/docs/introduction/overview/) is an open-source systems monitoring and alerting toolkit originally built at SoundCloud.  
Prometheus is a <a href="https://www.cncf.io/" target="_blank">Cloud Native Computing Foundation</a> graduated project.  
Prometheus is 100% open source and community-driven.  

## Grafana
[Grafana](https://grafana.com/) is an open source interactive data-visualization platform, developed by Grafana Labs, which allows users to see their data via charts and graphs that are unified into one dashboard (or multiple dashboards!) for easier interpretation and understanding.  
Grafana is a set of one or more panels organized and arranged into one or more rows.  

## EpThreshold
EpThreshold is a tiny middleware for Asp.Net Core which can be used to take action on requests which takes longers than a given threshold. Such action could for example be to log the request in order to track unexpectedly demanding requests in order to optimize them.


### Startup.cs
```c#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ....
    
    app.UseEpThreshold(o =>
    {
        o.Threshold = 1300; // The default threshold in ms for an endpoint. If a request takes longer than this the ThresholdMetAction will be invoked.
        o.ThresholdMetAction = async (ThresholdLogModel model) => await _queueClient.SendAsync(model);
    });

    ....
}
```

If you want to set a custom threshold on an endpoint you can use the Attribute ```[Threshold(100)]```.
```c#
[Threshold(100)] //if the request takes any longer than 100 ms, invoke ThresholdMetAction
[HttpGet("GetWeatherForecast/{id}")]
public async Task<WeatherForecast> GetWeatherForecast(int id)
{
    ....
}
```

        

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ReachDestinationAPI.Model;
using ReachDestinationAPI.Wrappers;

namespace ReachDestinationAPI
{
    public class ReachDestination
    {
        private readonly ILogger<ReachDestination> _logger;

        public ReachDestination(ILogger<ReachDestination> log)
        {
            _logger = log;
        }

        [FunctionName(nameof(ReachDestinationAPI))]
        [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
       // [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
       // [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
       [OpenApiRequestBody("application/json", typeof(PlayerResponse),
           Description = "Response From Player")]
        [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(BusRoute), Description = "The OK response")]
        public Task<Response<BusRoute>> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = null)] PlayerResponse req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var stations = Seeds.LoadBusStations.SeedAsync();
            var routes = Seeds.LoadBusRoutes.SeedAsync();

            var busRoutes = routes.Find(c=>c.RouteNumber == req.BusRouteNo);

            if (req.SelectedValue)
            {
                if (busRoutes?.TrueValue <= 15)
                {
                    var city =  stations.Find(c=>c.StationId == busRoutes.TrueValue);
                    var route = new BusRoute
                    {
                        RouteNumber = 123,
                        CurrentCity = city?.CityName,
                        TrueValue = 0,
                        FalseValue = 0,
                        Question = ""
                    };

                    return Task.FromResult(new Response<BusRoute>(route));
                }

                return Task.FromResult(new Response<BusRoute>(routes.Find(c=>c.RouteNumber == busRoutes?.TrueValue)));
            }
            else
            {
                if (busRoutes?.TrueValue <= 15)
                {
                    var city =  stations.Find(c=>c.StationId == busRoutes.FalseValue);
                    var route = new BusRoute
                    {
                        RouteNumber = 123,
                        CurrentCity = city?.CityName,
                        TrueValue = 0,
                        FalseValue = 0,
                        Question = ""
                    };

                    return Task.FromResult(new Response<BusRoute>(route));
                }

                return Task.FromResult(new Response<BusRoute>(routes.Find(c=>c.RouteNumber == busRoutes?.FalseValue)));
            }
        }
    }
}


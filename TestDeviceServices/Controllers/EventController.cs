using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using TestDeviceServices.Models;

namespace TestDeviceServices.Controllers
{
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        public EventController(AppContext appContext) => _appContext = appContext;
        private readonly AppContext _appContext;

        [HttpGet("eventlist")]
        public async Task<IResult> GetEvents([FromQuery(Name = "df")] DateTime dateFrom, [FromQuery(Name = "dt")] DateTime dateTo,
            [FromQuery(Name = "tguid")] string deviceGuid, [FromQuery(Name = "ob")] int orderBy = 0,
            [FromQuery(Name = "obf")] int orderByField = 0)
        {
            List<EventModel> events = await _appContext.Events.Where(e => e.DateCreate >= dateFrom && e.DateCreate <= dateTo).ToListAsync();

            if (!String.IsNullOrEmpty(deviceGuid)) 
            {
                events = events.Where(e => e.DeviceGuid == Guid.Parse(deviceGuid)).ToList();
            }
            if (orderBy == 1)
            {
                switch (orderByField)
                {
                    case (int)OrderByTypes.EXTERNAL_ID:
                        events = events.OrderBy(e => e.ExternalId).ToList();
                        break;
                    case (int)OrderByTypes.PARAM_FLOAT:
                        events = events.OrderBy(e => e.ParamFloat).ToList();
                        break;
                    case (int)OrderByTypes.DATE:
                        events = events.OrderBy(e => e.DateCreate).ToList();
                        break;
                    default: break;
                }
            }
            else if (orderBy == 2) 
            {
                switch (orderByField)
                {
                    case (int)OrderByTypes.EXTERNAL_ID:
                        events = events.OrderByDescending(e => e.ExternalId).ToList();
                        break;
                    case (int)OrderByTypes.PARAM_FLOAT:
                        events = events.OrderByDescending(e => e.ParamFloat).ToList();
                        break;
                    case (int)OrderByTypes.DATE:
                        events = events.OrderByDescending(e => e.DateCreate).ToList();
                        break;
                    default: break;
                }
            }

            return Results.Ok(events);
        }

        [HttpGet("fieldstypes")]
        public async Task<IResult> GetFieldsTypes()
        {
            object[] typesList = new object[] {
                new {key = "ExternalId", value = 10 }, new {key = "ParamFloat", value = 11 },new {key = "DateCreate", value = 12 }
            };
            return Results.Ok(typesList);
        }

        [HttpPost]
        public async Task<IResult> EventCreate([FromQuery(Name = "dguid")] string deviceGuid, [FromBody] EventModel model)
        {
            if (String.IsNullOrEmpty(deviceGuid) && model == null) return Results.BadRequest("Input data not defined!");

            Guid guidDevice = Guid.Parse(deviceGuid);

            DeviceModel? device = await _appContext.Devices.FirstOrDefaultAsync(d => d.Guid == guidDevice);
            if (device == null) return Results.BadRequest("Entity not defined!");

            model.DateCreate = DateTime.UtcNow;
            model.DeviceGuid = device.Guid;

            try
            {
                _appContext.Add(model);
                await _appContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return Results.Ok();
        }

        public class EventRequestModel
        {
            public string EventName { get; set; }
        }

        private enum OrderByTypes
        {
            EXTERNAL_ID = 10,
            PARAM_FLOAT = 11,
            DATE = 12
        }
    }
}

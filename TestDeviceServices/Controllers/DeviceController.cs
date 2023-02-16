using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestDeviceServices.Models;

namespace TestDeviceServices.Controllers
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        public DeviceController(AppContext appContext) => _appContext = appContext;

        private readonly AppContext _appContext;

        [HttpPost("regist")]
        public async Task<IResult> DeviceRegistration([FromBody] DeviceRegistModel modelDevice)
        {
            if (modelDevice == null) return Results.BadRequest("Input data is not defined!");

            DeviceModel newDeviceModel = new() {
                DeviceName = modelDevice.DeviceName,
                ExternalId = modelDevice.ExternalId,
                DateCreate = DateTime.UtcNow
            };

            _appContext.Add(newDeviceModel);

            DeviceTokens deviceTokens = new() {
                DevicetGuid = newDeviceModel.Guid,
                Token = DeviceController.GenerateToken(23),
                LastAuth = DateTime.UtcNow
            };
            _appContext.Add(deviceTokens);

            await _appContext.SaveChangesAsync();

            return Results.Ok(new { deviceId = newDeviceModel.Guid.ToString(), token = deviceTokens.Token });
        }

        private static string GenerateToken(short length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string resultKey = String.Empty;

            for (short i = 0; i < length; i++)
            {
                Index rndIndex = new Random().Next(0, chars.Length);

                char rndChar = chars[rndIndex];
                resultKey += rndIndex.Value <= (chars.Length / 2) ? rndChar.ToString().ToLower() : rndChar.ToString();
            }
            return resultKey;
        }

        public class DeviceRegistModel 
        {
            public string DeviceName { get; set; }
            public int ExternalId { get; set; }
        }
    }
}

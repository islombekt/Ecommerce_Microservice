﻿using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version.apiVersion}/[controller]")]
    public class ApiController : ControllerBase
    {

    }
}

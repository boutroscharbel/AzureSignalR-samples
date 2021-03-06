﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Extensions.Configuration;

namespace NegotiationServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NegotiateController : ControllerBase
    {
        private const string HubName = "Management";
        private readonly IServiceManager _serviceManager;

        public NegotiateController(IConfiguration configuration)
        {
            var connectionString = configuration["Azure:SignalR:ConnectionString"];
            _serviceManager = new ServiceManagerBuilder()
                .WithOptions(o => o.ConnectionString = connectionString)
                .Build();
        }

        [HttpPost]
        public ActionResult Post(string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return BadRequest("User ID is null or empty.");
            }

            return new JsonResult(new Dictionary<string, string>()
            {
                { "url", _serviceManager.GetClientEndpoint(HubName) },
                { "accessToken", _serviceManager.GenerateClientAccessToken(HubName, user) }
            });
        }
    }
}
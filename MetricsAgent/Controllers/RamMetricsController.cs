﻿using AutoMapper;
using MetricsAgent.Controllers.Requests;
using MetricsAgent.Controllers.Responses;
using MetricsAgent.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly IRamMetricsRepository _repository;
        private readonly ILogger<RamMetricsController> _logger;
        private readonly IMapper _mapper;

        public RamMetricsController(IRamMetricsRepository repository, ILogger<RamMetricsController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public RamMetricsByTimePeriodResponse GetMetrics([FromRoute] RamMetricRequest request)
        {
            _logger.LogInformation($"Get Ram metrics: From Time = {request.FromTime} To Time = {request.ToTime}");

            var result = _repository.GetByTimePeriod(request.FromTime, request.ToTime);

            return new RamMetricsByTimePeriodResponse()
            {
                Response = result.Select(_mapper.Map<RamMetricDto>)
            };
        }
    }
}

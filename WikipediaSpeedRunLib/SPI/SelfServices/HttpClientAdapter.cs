﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikipediaSpeedRunLib.SPI.Interfaces;

namespace WikipediaSpeedRunLib.SPI.SelfServices
{
    public class HttpClientAdapter : IHttpClientAdapter
    {
        HttpClient _httpClient;

        public HttpClientAdapter()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetStringAsync(string url)
        {
            return await _httpClient.GetStringAsync(url);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikipediaSpeedRunLib
{
    public interface IHttpClientAdapter
    {
        public Task<string> GetStringAsync(string url);
    }
}

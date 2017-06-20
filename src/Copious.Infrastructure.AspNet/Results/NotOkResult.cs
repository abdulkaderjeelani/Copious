using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Copious.Infrastructure.AspNet.Results
{
    public class NotOkResult : ObjectResult
    {
        public NotOkResult(ServiceResult result) : base(result)
        {
            StatusCode = (result?.Status == 99) ? 500 : 202;
            /* 202 Accepted The request has been accepted for processing, but the processing has not been completed. The request might or might not be eventually acted upon, and may be disallowed when processing occurs.[9] */
        }

        public NotOkResult(ServiceResult result, int statusCode) : base(result)
        {
            StatusCode = statusCode;
        }
    }
}
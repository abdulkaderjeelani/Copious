using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Foundation
{
    public static class CopiousErrorCodes
    {
        public const int ExceptionCode = 99;
        public static readonly ErrorCode Exception = new ErrorCode(ExceptionCode, "Server encountered an error.");
    }
}
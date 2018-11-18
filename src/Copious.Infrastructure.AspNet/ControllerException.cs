using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Copious.Application.Interface.Exceptions;
using Copious.Foundation;
using Copious.Infrastructure.AspNet.Results;
using Microsoft.AspNetCore.Mvc;

namespace Copious.Infrastructure.AspNet {
    public class ControllerException : Exception {
        public ControllerException (IActionResult exceptionResult) : base (nameof (ControllerException)) {
            ExceptionResult = exceptionResult;
        }

        public ControllerException (CommandException commandException, ErrorCode errorCode) : base (errorCode?.Description, commandException) {
            ExceptionResult = new NotOkResult (new ServiceResult (errorCode.AddInfo (commandException.ToString ())));
            IsCommandException = true;
        }

        public IActionResult ExceptionResult { get; }

        public bool IsCommandException { get; }
        public bool IsQueryException { get; }
    }
}
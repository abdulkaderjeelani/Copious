using Copious.Foundation;

namespace Copious.Infrastructure.AspNet.Results
{
    public class ServiceResult
    {
        public ServiceResult()
        {
        }

        public ServiceResult(int status, string description)
        {
            Status = status;
            Description = description;
        }

        public ServiceResult(ErrorCode errCode)
        {
            Status = errCode.Code;
            Description = errCode.Description;
        }

        /// <summary>
        /// 0 = success, +ve value = error code
        /// </summary>
        public int Status { get; set; }

        public string Description { get; set; }
    }

    public class ServiceResult<TData> : ServiceResult
    {
        public ServiceResult()
        {
        }

        public ServiceResult(TData data)
        {
            Data = data;
        }

        public ServiceResult(TData data, ErrorCode errCode) :base(errCode)
        {
            Data = data;
        }

        public TData Data { get; set; }
    }
}
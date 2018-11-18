using Copious.Foundation;

namespace Copious.Infrastructure.AspNet {
    public static class ErrorCodes {
        public static readonly ErrorCode UserAlreadyExists = new ErrorCode (100, "User already exists.");
        public static readonly ErrorCode UserCreationFailed = new ErrorCode (101, "User creation failed.");
        public static readonly ErrorCode RoleCreationFailed = new ErrorCode (102, "Role creation failed.");
    }
}
using System;

namespace Copious.SharedKernel.Exceptions {
    public class VersionConflictException : Exception {
        //Version Check 1  - Command Handler - check whether there is any modification between read and command execution
        //Version Check 2 - Repository - to verify whether there is any modification in between the first fetch in command handler and this call
        public VersionConflictException (int expected, int actual) : base ($@"Version conflict. Expectd version is {expected}, Received is {actual}") { }
    }
}
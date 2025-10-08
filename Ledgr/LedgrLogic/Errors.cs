namespace LedgrLogic;

public class Errors
{
    
}

public class InvalidPasswordException(string message) : Exception(message);

public class InvalidUsernameException(string message) : Exception(message);

public class InactiveUserException(string message) : Exception(message);

public class PasswordRequirementsViolationException(string message) : Exception(message);

public class PasswordUsedBeforeException(string message) : Exception(message);

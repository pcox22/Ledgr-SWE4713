namespace LedgrLogic;

public class Errors
{
    
}

public class InvalidPasswordException(string message) : Exception(message);

public class InvalidUsernameException(string message) : Exception(message);

public class InactiveUserException(string message) : Exception(message);
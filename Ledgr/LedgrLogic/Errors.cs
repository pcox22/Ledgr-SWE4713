namespace LedgrLogic;

public class Errors
{
    
}

public class InvalidPasswordException(string message) : Exception(message);

public class InvalidUsernameException(string message) : Exception(message);

public class InactiveUserException(string message) : Exception(message);

public class PasswordRequirementsViolationException(string message) : Exception(message);

public class PasswordUsedBeforeException(string message) : Exception(message);

public class InvalidDateFormatException(string message) : Exception(message);

public class UnableToRejectUserException(string message) : Exception(message);

public class UnableToGetSecurityQuestionException(string message) : Exception(message);

public class InvalidProfilePictureException(string message) : Exception(message);

public class UnableToApproveUserException(string message) : Exception(message);

public class UnableToActivateUserException(string message) : Exception(message);

public class UnableToDeactivateUserException(string message) : Exception(message);

public class InvalidChangeException(string message) : Exception(message);

public class UnableToRetrieveException(string message) : Exception(message);

public class InvalidUserIDException(string message) : Exception(message);

public class EventLogException(string message) : Exception(message);

public class InvalidAccountNumberException(string message) : Exception(message);

public class InvalidEmployeeIDException(string message) : Exception(message);

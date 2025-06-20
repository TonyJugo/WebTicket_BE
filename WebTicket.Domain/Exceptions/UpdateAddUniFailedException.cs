namespace WebTicket.Domain.Exceptions;

public class UpdateAddUniFailedException(IEnumerable<string> errorDescriptions)
    : Exception($"Update/Add failed with following errors: {string.Join("",errorDescriptions)}"); 
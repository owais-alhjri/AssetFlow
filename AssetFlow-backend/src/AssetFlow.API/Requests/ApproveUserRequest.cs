namespace AssetFlow.API.Requests;

public record ApproveUserRequest(
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Department,
    string? JobTitle,
    DateOnly HireDate);
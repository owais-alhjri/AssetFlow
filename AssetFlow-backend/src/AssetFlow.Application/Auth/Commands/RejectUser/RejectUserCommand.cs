using AssetFlow.Domain.Common;
using MediatR;

namespace AssetFlow.Application.Auth.Commands.RejectUser;

public record RejectUserCommand(Guid UserId) : IRequest<Result>;
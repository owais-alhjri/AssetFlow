using AssetFlow.Application.Auth.Commands.RegisterUser;
using AssetFlow.Application.Common.Interfaces;
using AssetFlow.Domain.Entities;
using AssetFlow.Domain.Errors;
using Moq;
using Xunit;

namespace AssetFlow.Application.UnitTests.Auth;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new();
    private readonly Mock<IUserRoleRepository> _userRoleRepository = new();
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();

    private readonly RegisterUserCommandHandler _sut;

    private static readonly Guid DefaultRoleId = Guid.NewGuid();

    // A real bcrypt-shaped hash so PasswordHash.Create() passes its validation.
    // If PasswordHash.Create enforces a stricter rule, swap this for a value it accepts.
    private const string FakeHash =
        "$2a$12$R9h/cIPz0gi.URNNX3kh2OPST9/PgBkqquzi.Ss7KIUgO2t0jWMUW";

    public RegisterUserCommandHandlerTests()
    {
        // Happy-path defaults. Each test overrides only what it needs.
        _userRepository
            .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _userRepository
            .Setup(r => r.ExistsByEmployeeIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _employeeRepository
            .Setup(r => r.ExistsByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _userRoleRepository
            .Setup(r => r.GetIdByNameAsync("Employee", It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultRoleId);

        _passwordHasher
            .Setup(h => h.Hash(It.IsAny<string>()))
            .Returns(FakeHash);

        _jwtTokenGenerator
            .Setup(g => g.GenerateToken(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("fake-jwt-token");

        _sut = new RegisterUserCommandHandler(
            _userRepository.Object,
            _unitOfWork.Object,
            _passwordHasher.Object,
            _jwtTokenGenerator.Object,
            _employeeRepository.Object,   
            _userRoleRepository.Object);  
    }

    private static RegisterUserCommand Command(Guid? employeeId = null) =>
        new("owais@assetflow.om", "Owais@123", employeeId);

    // ---------- Happy paths ----------

    [Fact]
    public async Task Handle_ValidRequestWithoutEmployee_SucceedsAndPersists()
    {
        var result = await _sut.Handle(Command(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        _userRepository.Verify(
            r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidRequestWithoutEmployee_SkipsEmployeeChecks()
    {
        await _sut.Handle(Command(employeeId: null), CancellationToken.None);

        _employeeRepository.Verify(
            r => r.ExistsByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepository.Verify(
            r => r.ExistsByEmployeeIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequestWithEmployee_LinksEmployeeOnCreatedUser()
    {
        var employeeId = Guid.NewGuid();
        User? persisted = null;
        _userRepository
            .Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Callback<User, CancellationToken>((u, _) => persisted = u)
            .Returns(Task.CompletedTask); // if AddAsync returns ValueTask, use ValueTask.CompletedTask

        var result = await _sut.Handle(Command(employeeId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(persisted);
        Assert.Equal(employeeId, persisted!.EmployeeId);
    }

    // ---------- Failure paths (should not persist) ----------

    [Fact]
    public async Task Handle_EmailAlreadyExists_ReturnsErrorAndDoesNotPersist()
    {
        _userRepository
            .Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.Handle(Command(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.EmailAlreadyExists, result.Error);
        VerifyNothingPersisted();
    }

    [Fact]
    public async Task Handle_EmployeeDoesNotExist_ReturnsNotFoundAndDoesNotPersist()
    {
        var employeeId = Guid.NewGuid();

        _employeeRepository
            .Setup(r => r.ExistsByIdAsync(employeeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _sut.Handle(Command(employeeId), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(EmployeeErrors.NotFound(employeeId), result.Error);
        VerifyNothingPersisted();
    }

    [Fact]
    public async Task Handle_EmployeeAlreadyLinked_ReturnsConflictAndDoesNotPersist()
    {
        _userRepository
            .Setup(r => r.ExistsByEmployeeIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _sut.Handle(Command(Guid.NewGuid()), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(UserErrors.EmployeeAlreadyLinked, result.Error);
        VerifyNothingPersisted();
    }

    [Fact]
    public async Task Handle_DefaultRoleMissing_ReturnsErrorAndDoesNotPersist()
    {
        _userRoleRepository
            .Setup(r => r.GetIdByNameAsync("Employee", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid?)null);

        var result = await _sut.Handle(Command(), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(UserRoleErrors.DefaultRoleMissing, result.Error);
        VerifyNothingPersisted();
    }

    private void VerifyNothingPersisted()
    {
        _userRepository.Verify(
            r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWork.Verify(
            u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

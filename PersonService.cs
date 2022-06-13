public class PersonService : IPersonService
{
    private readonly IUserContextProvider _userContextProvider;
 
    private readonly IAppDbContext _appDbContext;
 
    public PersonService(IUserContextProvider userContextProvider, IAppDbContext appDbContext)
    {
        _userContextProvider = userContextProvider;
        _appDbContext = appDbContext;
    }
 
    public async Task Add(PersonDto dto)
    {
        if (dto == null)
        {
            throw new NullReferenceException("PersonDto is null");
        }
 
        if (!_userContextProvider.UserContext.IsAdmin)
        {
            throw new UnauthorizedAccessException("User has no permissions to add new person");
        }
 
        var newPerson = Map(dto);
 
        await _appDbContext.AddAsync(newPerson);
        await _appDbContext.SaveChangesAsync();
    }
 
    public virtual Person Map(PersonDto dto)
    {
        return new Person()
        {
            Id = Guid.NewGuid(),
            Name = $"{dto.FirstName} {dto.LastName}",
            Login = dto.Login,
            DepartmentId = dto.Department.Id,
            Path = dto.Department.Path,
            CreatedOn = DateTime.Now
        };
    }
}

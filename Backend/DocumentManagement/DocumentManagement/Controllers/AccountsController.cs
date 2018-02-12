using AutoMapper;
using DocumentManagement.Repository.Models.Identity;
using DocumentManagement.Services.Account;
using DocumentManagement.Services.Jwt;
using DocumentManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DocumentManagement.Controllers
{
  [Route("api/[controller]/[action]")]
  public class AccountsController : Controller
  {
    private readonly IAccountService _accountService;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public AccountsController(IMapper mapper, IAccountService accountService, IJwtService jwtService)
    {
      _accountService = accountService;
      _jwtService = jwtService;
      _mapper = mapper;
    }

    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var identity = await _accountService.GetClaimsIdentity(model.Username, model.Password);
      if (identity == null)
        return BadRequest("Invalid Username or Password");

      var jwt = _jwtService.GenerateJwt(identity, model.Username,
        new JsonSerializerSettings {Formatting = Formatting.Indented});
      return new OkObjectResult(jwt);
    }

    [HttpPost]
    public IActionResult Create([FromBody] RegisterViewModel model)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var user = _mapper.Map<ApplicationUser>(model);

      if (_accountService.CreateUserAccount(user, model.Password, model.Role).IsCompletedSuccessfully)
        return new OkObjectResult("Account created");
      return BadRequest();
    }
  }
}
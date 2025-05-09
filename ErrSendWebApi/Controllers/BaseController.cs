using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ErrSendWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseController : ControllerBase
    {
        private IMediator mediator;
        protected IMediator Mediator
        {
            get
            {
                return mediator ??= HttpContext.RequestServices.GetService<IMediator>();
            }
        }
    }
}

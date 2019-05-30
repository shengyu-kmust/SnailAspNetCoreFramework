using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrPMS.Web.Controllers.V2
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MediatorController : ControllerBase
    {
        public static List<string> notifyReceived;
        private IMediator _mediator;
        public MediatorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 测试mediator的request模式是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string TestRequest()
        {
            var testRequest = new TestRequest();
            var result = _mediator.Send(testRequest).Result;
            return result;
        }

        /// <summary>
        /// 测试mediator的notification模式是否正常
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> TestNotify()
        {
            notifyReceived = new List<string>();
            var notify = new TestNotify();
            await _mediator.Publish(notify);
            return notifyReceived;
        }
    }

    public class TestNotify : INotification
    {

    }

    public class TestRequest : IRequest<string>
    {

    }

    public class TestHandler : IRequestHandler<TestRequest, string>
    {
        public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult("TestRequest handle result");
        }
    }

    public class TestNotifyHandle1 : INotificationHandler<TestNotify>
    {
        public Task Handle(TestNotify notification, CancellationToken cancellationToken)
        {
            MediatorController.notifyReceived.Add(nameof(TestNotifyHandle2));
            return Task.CompletedTask;
        }
    }

    public class TestNotifyHandle2 : INotificationHandler<TestNotify>
    {
        public Task Handle(TestNotify notification, CancellationToken cancellationToken)
        {
            MediatorController.notifyReceived.Add(nameof(TestNotifyHandle2));
            return Task.CompletedTask;
        }
    }
}

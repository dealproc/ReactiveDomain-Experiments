using Microsoft.AspNetCore.Mvc;

using ReactiveDomain.Messaging.Bus;

namespace RD.Web {
    public class ProjectController : ControllerBase {
        private readonly IDispatcher _dispatcher;

        public ProjectController(IDispatcher dispatcher) {
            _dispatcher = dispatcher;
        }

        public IActionResult NewProject([FromBody] object uiModel) {
            //DEVELOPER: I'm assuming this is how it will work.
            var cmd = new Core.Services.Project.NewProjectRequest() { }; //map properties from uiModel as appropriate.
            _dispatcher.Send(cmd);
            return Ok();
        }
    }
}
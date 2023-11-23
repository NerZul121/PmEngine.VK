using PmEngine.Core;
using PmEngine.Core.BaseMarkups;
using PmEngine.Core.Interfaces;

namespace PmEngine.Vk.WebApi.Actions
{
    public class HelloWorldAction : IAction
    {
        public async Task<INextActionsMarkup?> DoAction(IActionWrapper currentAction, IUserSession user, IActionArguments arguments)
        {
            var result = new LinedMarkup();
            result.CreateLine();
            result.Add("Привет!", GetType(), new ActionArguments(new() { { "text", "Привет!" } }));
            result.Add("Hello!", GetType(), new ActionArguments(new() { { "text", "Hello!" } }));
            result.CreateLine();
            result.Add("Bonjour!", GetType(), new ActionArguments(new() { { "text", "Bonjour!" } }));

            user.Media = new string[] { "iVBORw0KGgoAAAANSUhEUgAAAQAAAACgCAYAAADjGbI8AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIcSURBVHhe7d0/alRRHIbhMYtwDzYSkJAquBRXI1i5gRQWSSBNBDvBP0iwcg8TxMoqOEyfSXNasfwJ7/M096sv976c7mwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgH96sp5j3r46O6w54unps7VmfD86W2vG1/cf1prx8uHXWjPOP/8Y/wcmHa0nECQAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAECYAEDZ+N/rFzeVhzRGv312vNeP58fFaM37ffltrxt3dz7Vm7Pf7tWbsdrvRf9AJAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMIEAMJG7yb/H7y5OD+sOeLTx9n7+b9cXY1+AycnL0bf//39n7VmbLfb0ffvBABhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgBhAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH+12TwC0nQovrTvVSwAAAAASUVORK5CYII=" };

            user.AddToOutput(arguments.Get<string?>("text") ?? "Приветик!");

            return result;
        }
    }
}

//using Microsoft.AspNetCore.Components;

//using IComponent = Microsoft.AspNetCore.Components.IComponent;

//namespace NetzorwrightPdf.TestConsole;
//public partial class PersonListView : IComponent
//{
//    [Parameter]
//    public List<Person>? People { get; set; }

//    public void Attach(RenderHandle renderHandle)
//    {

//    }

//    public async Task SetParametersAsync(ParameterView parameters)
//    {
//        if (!parameters.TryGetValue("People", out List<Person>? people))
//        {
//            People = new List<Person>();
//        }
//        else
//        {
//            People = people;
//        }
//    }
//}
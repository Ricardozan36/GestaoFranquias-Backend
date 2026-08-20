using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Franquias.FrontEnd;
using MudBlazor.Services;
using Blazored.LocalStorage; 

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Injetamos o Cofre de Tokens
builder.Services.AddBlazoredLocalStorage();

// 2. Injetamos os serviços visuais do MudBlazor
builder.Services.AddMudServices();

// 3. Aponta o Front-End para a porta da API. (Confirme se sua API roda na 5062!)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5062/") });

await builder.Build().RunAsync();
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/response", ([FromBody]string prompt) =>
    {
        //your OpenAI API key
        string apiKey = builder.Configuration["OPENAI:apiKey"];
        string answer = string.Empty;
        var openai = new OpenAIAPI(apiKey);
        CompletionRequest completion = new CompletionRequest
        {
            Prompt = prompt,
            Model = OpenAI_API.Models.Model.DavinciText,
            MaxTokens = 4000
        };
        var result = openai.Completions.CreateCompletionAsync(completion);
        if (result != null)
        {
            foreach (var item in result.Result.Completions)
            {
                answer = item.Text;
            }
            return Results.Ok(answer);
        }
        else
        {
            return Results.BadRequest("Not found");
        }
    })
.WithName("GetResponse")
.Produces(200)
.WithTags("Open AI Response");

app.Run();
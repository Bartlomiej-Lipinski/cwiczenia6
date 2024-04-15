using System.Data.SqlClient;
using cwiczenia6;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// builder.Services.AddValidatorsFromAssemblyContaining<CreateAnimalRequest>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/animals",()=>
{
    var animals = new List<GetAllAnimalsResponse>();
    using (var sqlConnection = new SqlConnection("Server=localhost;Database=master;User Id=sa;Password=yourStrong(!)Password;"))
    {
        var sqlCommand = new SqlCommand("SELECT * FROM Animal",sqlConnection );
        sqlCommand.Connection.Open();
        var reader = sqlCommand.ExecuteReader();
        if (!reader.Read())
        {
            return Results.NotFound();
        }
        while (reader.Read())
        {
            animals.Add(new GetAllAnimalsResponse(reader.GetInt32(0),reader.GetString(1),reader.GetString(2),reader.GetString(3),reader.GetString(4)));
        }
    }
    return Results.Ok(animals);
});
app.MapGet("/api/animals{id:int}",(int id)=>
{
    using (var sqlConnection = new SqlConnection("Server=localhost;Database=master;User Id=sa;Password=yourStrong(!)Password;"))
    {
        var sqlCommand = new SqlCommand("SELECT * FROM Animal where idAnimal=@id",sqlConnection );
        sqlCommand.Parameters.AddWithValue("@id",id);
        sqlCommand.Connection.Open();
        var reader = sqlCommand.ExecuteReader();
        if (!reader.Read())
        {
            return Results.NotFound();
        }
        return Results.Ok(new GetAllAnimalsResponse(reader.GetInt32(0),reader.GetString(1),reader.GetString(2),reader.GetString(3),reader.GetString(4)));
    }
});
app.MapPost("/api/animals",(CreateAnimalRequest request,IValidator<CreateAnimalRequest> validator)=>
{
    var validationResult = validator.Validate(request);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    using (var sqlConnection = new SqlConnection("Server=localhost;Database=master;User Id=sa;Password=yourStrong(!)Password;"))
    {
        var sqlCommand = new SqlCommand("INSERT INTO Animal (Name,Description,Category,Area) VALUES (@Name,@Description,@Category,@Area)",sqlConnection );
        sqlCommand.Parameters.AddWithValue("@Name",request.Name);
        sqlCommand.Parameters.AddWithValue("@Description",request.Description);
        sqlCommand.Parameters.AddWithValue("@Category",request.Category);
        sqlCommand.Parameters.AddWithValue("@Area",request.Area);
        sqlCommand.Connection.Open();
        sqlCommand.ExecuteNonQuery();
    }
    return Results.Created("/api/animals",null);
});


app.Run();


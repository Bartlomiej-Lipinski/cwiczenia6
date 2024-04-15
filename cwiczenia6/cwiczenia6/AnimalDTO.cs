namespace cwiczenia6;

public record GetAllAnimalsResponse(int ID, string Name, string Description, string Category, string Area);

public record CreateAnimalRequest(string Name, string Description, string Category, string Area);

public record UpdateAnimalRequest(string Name, string Description, string Category, string Area);
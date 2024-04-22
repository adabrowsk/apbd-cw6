using App6.Models;
using App6.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace App6.Controllers;

[ApiController]
//[Route("api/animals")]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AnimalsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet]
    public IActionResult GetAnimals()
    {
        //Otwieramy połączenie
        using  SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        //definiujemy commanda
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM Animal";

        //wykonanie commanda
        var reader = command.ExecuteReader();
        List <Animal>  animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        
        while (reader.Read())
        {
          animals.Add(new Animal()
          {
              IdAnimal = reader.GetInt32(idAnimalOrdinal),
              Name = reader.GetString(nameOrdinal)
          });  
        }
        
        return Ok(animals);
    }

    [HttpPost]
    public IActionResult AddAnimal(AddAnimal animal)
    {

       // using ()
      //  {
            
       // }

       // try
        //{

       // }

      //  finally
       // {
        //    ConnectionInfo.Dispose();
       // }
        
        //Otwieramy połączenie
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        
        //definiujemy commanda
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "$INSERT INTO Animal VALUES(@animalName, '', '', '')";
        command.Parameters.AddWithValue("@animalName", animal.Name);

        command.ExecuteNonQuery();
        
        return Created("", null);
    }
    [HttpGet]
public IActionResult GetAnimals([FromQuery] string orderBy = "name")
{
    var columnNames = new List<string> { "name", "description", "category", "area" };
    orderBy = columnNames.Contains(orderBy.ToLower()) ? orderBy : "name";
    using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
    connection.Open();
    SqlCommand command = new SqlCommand($"SELECT * FROM Animal ORDER BY {orderBy}", connection);
    var reader = command.ExecuteReader();
    List<Animal> animals = new List<Animal>();
    while (reader.Read())
    {
     
        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");
        int descriptionOrdinal = reader.GetOrdinal("Description");
        int categoryOrdinal = reader.GetOrdinal("Category");
        int areaOrdinal = reader.GetOrdinal("Area");
        animals.Add(new Animal
        {
            IdAnimal = reader.GetInt32(idAnimalOrdinal),
            Name = reader.GetString(nameOrdinal),
            Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
            Category = reader.IsDBNull(categoryOrdinal) ? null : reader.GetString(categoryOrdinal),
            Area = reader.IsDBNull(areaOrdinal) ? null : reader.GetString(areaOrdinal)
        });
    }

    return Ok(animals);
}

[HttpPut("{idAnimal}")]
public IActionResult UpdateAnimal(int idAnimal, [FromBody] Animal animal)
{
    using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
    connection.Open();
    SqlCommand command = new SqlCommand($"UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @IdAnimal", connection);
    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
    command.Parameters.AddWithValue("@Name", animal.Name);
    command.Parameters.AddWithValue("@Description", animal.Description);
    command.Parameters.AddWithValue("@Category", animal.Category);
    command.Parameters.AddWithValue("@Area", animal.Area);
    command.ExecuteNonQuery();
    return NoContent();
}

[HttpDelete("{idAnimal}")]
public IActionResult DeleteAnimal(int idAnimal)
{
    using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
    connection.Open();
    SqlCommand command = new SqlCommand("DELETE FROM Animal WHERE IdAnimal = @IdAnimal", connection);
    command.Parameters.AddWithValue("@IdAnimal", idAnimal);
    command.ExecuteNonQuery();
    return NoContent();
}
    
}
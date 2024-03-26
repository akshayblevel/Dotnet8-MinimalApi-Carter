using Carter;
using Microsoft.EntityFrameworkCore;

namespace Dotnet8_MinimalApi_Carter
{
    public class VehicleEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/vehicles", async (VehicleDb db) =>
                await db.Vehicles.ToListAsync())
                .WithName("GetVehicles")
                .Produces<Vehicle>(StatusCodes.Status200OK)
                .WithSummary("Get Vehicles")
                .WithDescription("Get All Vehicles"); 

            app.MapGet("/vehicles/{id}", async (int id, VehicleDb db) =>
                await db.Vehicles.FindAsync(id))
                .WithName("GetVehiclesById")
                .Produces<Vehicle>(StatusCodes.Status200OK)
                .WithSummary("Get Vehicle")
                .WithDescription("Get Vehicle By Id"); 

            app.MapPost("/vehicles", async (Vehicle vehicle, VehicleDb db) =>
            {
                db.Vehicles.Add(vehicle);
                await db.SaveChangesAsync();
                return Results.Created($"/vehicles/{vehicle.Id}", vehicle);
            })
            .WithName("CreateVehicle")
            .Produces<Vehicle>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Vehicle")
            .WithDescription("Create New Vehicle"); ;

            app.MapPut("/vehicles/{id}", async (int id, Vehicle vehicle, VehicleDb db) =>
            {
                var veh = await db.Vehicles.FindAsync(id);
                if (veh == null) return Results.NotFound();

                veh.Name = vehicle.Name;
                veh.IsComplete = vehicle.IsComplete;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateVehicle")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Vehicle")
            .WithDescription("Update Existing Vehicle"); 

            app.MapDelete("/vehicles/{id}", async (int id, VehicleDb db) =>
            {
                if (await db.Vehicles.FindAsync(id) is Vehicle vehicle)
                {
                    db.Vehicles.Remove(vehicle);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }
                return Results.NotFound();
            })
            .WithName("DeleteVehicle")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Vehicle")
            .WithDescription("Delete Existing Vehicle"); 
        }
    }
}

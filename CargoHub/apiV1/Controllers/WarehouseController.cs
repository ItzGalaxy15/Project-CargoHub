using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;

namespace apiV1.Controllers
{
    [Route("api/v1/warehouses")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService warehouseService;
        private readonly ILocationService locationService;
        private readonly IWarehouseValidationService warehouseValidationService;

        public WarehouseController(IWarehouseService warehouseService, ILocationService locationService,
        IWarehouseValidationService warehouseValidationService)
        {
            this.warehouseService = warehouseService;
            this.warehouseValidationService = warehouseValidationService;
            this.locationService = locationService;
        }

        // Returns all warehouses
        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            Warehouse[] warehouses = await Task.Run(() => this.warehouseService.GetWarehouses());
            return this.Ok(warehouses);
        }

        // Returns a warehouse by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {
            Warehouse? warehouse = await Task.Run(() => this.warehouseService.GetWarehouseById(id));
            return warehouse is null ? this.NotFound() : this.Ok(warehouse);
        }

        // Returns all locations in a warehouse
        [HttpGet("{id}/locations")]
        public async Task<IActionResult> GetLocationsInWarehouse(int id) // id = warehouseId
        {
            Location[] locations = await Task.Run(() => this.locationService.GetLocationsInWarehouse(id));
            return this.Ok(locations);
        }

        // Adds a new warehouse
        [HttpPost]
        public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
        {
            if (!this.warehouseValidationService.IsWarehouseValid(warehouse))
            {
                return this.BadRequest("invalid warehouse object");
            }

            await this.warehouseService.AddWarehouse(warehouse);
            return this.CreatedAtAction(nameof(this.GetWarehouseById), new { id = warehouse.Id }, warehouse);
        }

        // Replaces a warehouse with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceWarehouse([FromBody] Warehouse warehouse, int id)
        {
            if (warehouse?.Id != id)
            {
                return this.BadRequest("Invalid warehouse Id");
            }

            if (!this.warehouseValidationService.IsWarehouseValid(warehouse, true))
            {
                return this.BadRequest("invalid warehouse object");
            }

            Warehouse? oldWarehouse = this.warehouseService.GetWarehouseById(id);
            warehouse.CreatedAt = oldWarehouse!.CreatedAt;
            await this.warehouseService.ReplaceWarehouse(warehouse, id);
            return this.Ok();
        }

        // Deletes a warehouse and all its locations
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            Warehouse? warehouse = this.warehouseService.GetWarehouseById(id);
            if (warehouse is null)
            {
                return this.NotFound();
            }

            await this.warehouseService.DeleteWarehouse(warehouse);
            return this.Ok();
        }
    }
}
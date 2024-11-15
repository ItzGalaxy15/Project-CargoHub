using Microsoft.AspNetCore.Mvc;
using apiV1.Interfaces;
using apiV1.ValidationInterfaces;


namespace apiV1.Controllers
{
    [Route("api/v1/warehouses")]
    public class WarehouseController : Controller
    {
        private readonly IWarehouseService _warehouseService;
        private readonly ILocationService _locationService;
        private readonly IWarehouseValidationService _warehouseValidationService;

        public WarehouseController(IWarehouseService warehouseService, ILocationService locationService,
        IWarehouseValidationService warehouseValidationService)
        {
            _warehouseService = warehouseService;
            _warehouseValidationService = warehouseValidationService;
            _locationService = locationService;
        }

        // Returns all warehouses
        [HttpGet]
        public async Task<IActionResult> GetWarehouses()
        {
            Warehouse[] warehouses = await Task.Run(() => _warehouseService.GetWarehouses());
            return Ok(warehouses);
        }

        // Returns a warehouse by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {
            Warehouse? warehouse = await Task.Run(() => _warehouseService.GetWarehouseById(id));
            return warehouse is null ? BadRequest() : Ok(warehouse);
        }

        // Returns all locations in a warehouse    
        [HttpGet("{id}/locations")]
        public async Task<IActionResult> GetLocationsInWarehouse(int id) // id = warehouseId
        {
            Location[] locations = await Task.Run(() => _locationService.GetLocationsInWarehouse(id));
            return Ok(locations);
        }

        // Adds a new warehouse
        [HttpPost]
        public async Task<IActionResult> AddWarehouse([FromBody] Warehouse warehouse)
        {
            if (!_warehouseValidationService.IsWarehouseValid(warehouse)) return BadRequest("invalid warehouse object");
            await _warehouseService.AddWarehouse(warehouse);
            return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.Id }, warehouse);
        }

        // Replaces a warehouse with a new one
        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceWarehouse([FromBody] Warehouse warehouse, int id)
        {
            if (warehouse?.Id != id) return BadRequest("Invalid warehouse Id");
            if (!_warehouseValidationService.IsWarehouseValid(warehouse, true)) return BadRequest("invalid warehouse object");
            Warehouse? oldWarehouse = _warehouseService.GetWarehouseById(id);
            warehouse.CreatedAt = oldWarehouse!.CreatedAt;
            await _warehouseService.ReplaceWarehouse(warehouse, id);
            return Ok();
        }

        // Deletes a warehouse and all its locations
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWarehouse(int id)
        {
            Warehouse? warehouse = _warehouseService.GetWarehouseById(id);
            if (warehouse is null) return BadRequest();
            await _warehouseService.DeleteWarehouse(warehouse);
            return Ok();
        }
    }
}
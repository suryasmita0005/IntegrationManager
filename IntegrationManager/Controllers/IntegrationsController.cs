using System.Diagnostics;
using IntegrationManager.Data;
using IntegrationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationManager.Controllers
{
    public class IntegrationsController : Controller
    {
        private readonly ILogger<IntegrationsController> _logger;
        private readonly AppDbContext _context;

        public IntegrationsController(ILogger<IntegrationsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List(string? market, string? legalEntity, string? integrationName)
        {
            // ✅ Dropdown Data
            ViewBag.Markets = await _context.IntegrationMasters
                .Select(x => x.Market)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            ViewBag.LegalEntities = await _context.IntegrationMasters
                .Select(x => x.LegalEntity)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            ViewBag.Integrations = await _context.IntegrationMasters
                .Select(x => x.IntegrationName)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            // ✅ FILTER QUERY
            IQueryable<IntegrationMaster> query = _context.IntegrationMasters;

            if (!string.IsNullOrEmpty(market))
                query = query.Where(x => x.Market == market);

            if (!string.IsNullOrEmpty(legalEntity))
                query = query.Where(x => x.LegalEntity == legalEntity);

            if (!string.IsNullOrEmpty(integrationName))
                query = query.Where(x => x.IntegrationName == integrationName);

            var filteredData = await query
                .OrderBy(x => x.IntegrationName)
                .ThenBy(x => x.Environment)
                .ToListAsync();

            // ✅ RETAIN SELECTED VALUES
            ViewBag.SelectedMarket = market;
            ViewBag.SelectedLegalEntity = legalEntity;
            ViewBag.SelectedIntegration = integrationName;

            return View(filteredData);
        }

        [HttpGet]
        public async Task<JsonResult> GetLegalEntitiesByMarket(string market)
        {
            var legalEntities = await _context.IntegrationMasters
                .Where(x => x.Market == market)
                .Select(x => x.LegalEntity)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();

            return Json(legalEntities);
        }

        [HttpGet]
        public async Task<IActionResult> Filter(string? market, string? legalEntity, string? integrationName)
        {
            IQueryable<IntegrationMaster> query = _context.IntegrationMasters;

            if (!string.IsNullOrEmpty(market))
                query = query.Where(x => x.Market == market);

            if (!string.IsNullOrEmpty(legalEntity))
                query = query.Where(x => x.LegalEntity == legalEntity);

            if (!string.IsNullOrEmpty(integrationName))
                query = query.Where(x => x.IntegrationName == integrationName);

            var filteredData = await query
                .OrderBy(x => x.IntegrationName)
                .ThenBy(x => x.Environment)
                .ToListAsync();

            return PartialView("_IntegrationGridPartial", filteredData);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string name, string? market, string? entity)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            IQueryable<IntegrationMaster> query = _context.IntegrationMasters
                .Where(x => x.IntegrationName == name);

            if (!string.IsNullOrEmpty(market))
                query = query.Where(x => x.Market == market);

            if (!string.IsNullOrEmpty(entity))
                query = query.Where(x => x.LegalEntity == entity);

            var integrations = await query
                .OrderBy(x => x.Environment)
                .ToListAsync();

            if (integrations == null || integrations.Count == 0)
                return NotFound();

            return View(integrations);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet( "id:int", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            
            if ( celestialObject == null)
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects
                .Where(e => e.OrbitedObjectId == id)
                .ToList();
            return Ok( celestialObject );
        }

        [HttpGet("{name}")]
        public IActionResult GetByName( string name )
        {
            var celestialObject = _context.CelestialObjects.Find(name);
            if( celestialObject == null )
                return NotFound();
            celestialObject.Satellites = _context.CelestialObjects
                .Where(e => e.OrbitedObjectId == celestialObject.Id)
                .ToList();
            return Ok( celestialObject );
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach( var celestialObject in celestialObjects )
            {
                celestialObject.Satellites = _context.CelestialObjects
                .Where( e => e.OrbitedObjectId == celestialObject.Id )
                .ToList();
            }
            return Ok(celestialObjects);
        }
    }
}

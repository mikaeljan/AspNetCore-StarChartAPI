﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            };

        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObject = _context.CelestialObjects.Find(name);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.Name == name).ToList();
                return Ok(celestialObject);
            }
        }
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var celestialObject = _context.CelestialObjects.ToList();
            foreach (var item in celestialObject)
            {
                item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == x.Id).ToList();
            }
            return Ok(celestialObject);
        }
    }
}

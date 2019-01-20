using System;
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
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (celestialObjects.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                foreach (var celestialObject in celestialObjects)
                {
                    celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
                }
                return Ok(celestialObjects);
            }
        }
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var celestialObject = _context.CelestialObjects.ToList();
            foreach (var item in celestialObject)
            {
                item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObject);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        public IActionResult Update([FromBody]CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var targetObject = _context.CelestialObjects.Find(id);
            if (targetObject == null)
                return NotFound();
                targetObject.Name = celestialObject.Name;
                targetObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                targetObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

                _context.CelestialObjects.Update(targetObject);
                _context.SaveChanges();

                return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var targetObject = _context.CelestialObjects.Find(id);
            if (targetObject == null)
                return NotFound();
            else
            {
                targetObject.Name = name;
                _context.CelestialObjects.Update(targetObject);
                _context.SaveChanges();

                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id || x.Id == id).ToList();
            if (celestialObjects.Count() == 0)
                return NotFound();
            else
            {
                _context.CelestialObjects.RemoveRange(celestialObjects);
                _context.SaveChanges();
                return NoContent();
            }
        }
    }
}

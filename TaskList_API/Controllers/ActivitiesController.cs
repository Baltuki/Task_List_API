using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using TaskList_API.Infrastructure.Dto;

namespace TaskList_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly TaskListDbContext _context;

        public ActivitiesController(TaskListDbContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
        {
            return await _context.Activities.ToListAsync();
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);

            if (activity == null)
            {
                return NotFound();
            }

            return activity;
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(int id, [FromBody]AddActivityDto activityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var activity = await _context.Activities.FindAsync(id);
            if(activity == null)
            {
                return NotFound();
            }

            activity.Description = activityDto.Description;
            activity.CreatedDate = activityDto.CreatedDate;
            activity.UserId = activityDto.UserId;


            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("Complete/{id}")]
        public async Task<IActionResult> PostActivityCompleted(int id, [FromBody] ActivityCompletedDto activityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            activity.IsCompleted = activityDto.IsCompleted;
            


            _context.Entry(activity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<Activity>> CreateActivity([FromBody]AddActivityDto activityDto)
        {
            if (ModelState.IsValid)
            {
                var activity = new Activity
                {
                    Description = activityDto.Description,
                    CreatedDate = activityDto.CreatedDate,
                    UserId = activityDto.UserId

                };

                _context.Activities.Add(activity);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetActivity", new { id = activity.Id }, activity); //o activityDto
            }

            return BadRequest(ModelState);
        }

        // DELETE: api/Activities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}

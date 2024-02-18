using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MeetingEventController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MeetingEventController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingEvent>>> GetMeetingEvents()
        {
            return await _context.MeetingEvents.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeetingEvent>> GetMeetingEvent(Guid id)
        {
            var meetingEvent = await _context.MeetingEvents.FindAsync(id);

            if (meetingEvent == null)
            {
                return NotFound();
            }

            return meetingEvent;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeetingEvent(Guid id, MeetingEvent meetingEvent)
        {
            if (id != meetingEvent.Id)
            {
                return BadRequest();
            }

            if (!ValidateMeetingEvent(meetingEvent))
            {
                return BadRequest("Start date must be before end date");
            }

            _context.Entry(meetingEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeetingEventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<MeetingEvent>> PostMeetingEvent(MeetingEvent meetingEvent)
        {
            if (!ValidateMeetingEvent(meetingEvent))
            {
                return BadRequest("Start date must be before end date");
            }

            _context.MeetingEvents.Add(meetingEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMeetingEvent", new { id = meetingEvent.Id }, meetingEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeetingEvent(Guid id)
        {
            var meetingEvent = await _context.MeetingEvents.FindAsync(id);
            if (meetingEvent == null)
            {
                return NotFound();
            }

            _context.MeetingEvents.Remove(meetingEvent);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool MeetingEventExists(Guid id)
        {
            return _context.MeetingEvents.Any(e => e.Id == id);
        }

        private bool ValidateMeetingEvent(MeetingEvent meetingEvent)
        {
            return meetingEvent.StartDate < meetingEvent.EndDate;
        }
    }
}

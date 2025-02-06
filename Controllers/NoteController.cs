using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteApi.Data;
using NoteApi.Models;

namespace NoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await _context.Notes.ToListAsync();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            return note;
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(Note note)
        {
            if (string.IsNullOrEmpty(note.Title))
            {
                return BadRequest("Title is required");
            }
            note.CreatedDate = DateTime.UtcNow.AddHours(7); // Add 7 hour for get correct time
            note.UpdatedDate = DateTime.UtcNow.AddHours(7);

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // // PUT: api/Notes/5
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutNote(int id, Note note)
        // {
        //     if (id != note.Id)
        //     {
        //         return BadRequest("ID in URL does not match ID in request body.");
        //     }

        //     note.UpdatedDate = DateTime.UtcNow.AddHours(7);
        //     // note.CreatedDate = note.CreatedDate;
        //     _context.Entry(note).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!_context.Notes.Any(e => e.Id == id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //      // Return the updated note with a 200 OK status
        //     // return Ok(note);
        //     return Accepted(new { message = "Note successfully updated." });
        //     // return NoContent();
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            // Retrieve the existing note from the database
            var existingNote = await _context.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            // Preserve the original CreatedDate
            var originalCreatedDate = existingNote.CreatedDate;

            // Update the existing note with the new values, except for CreatedDate
            _context.Entry(existingNote).CurrentValues.SetValues(note);
            existingNote.CreatedDate = originalCreatedDate; // Restore the original CreatedDate
            existingNote.UpdatedDate = DateTime.UtcNow.AddHours(7); // Update the UpdatedDate

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Notes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Return the updated note with a 200 OK status
            return Ok(existingNote);
            // Alternatively, you can return a success message
            // return Accepted(new { message = "Note successfully updated." });
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
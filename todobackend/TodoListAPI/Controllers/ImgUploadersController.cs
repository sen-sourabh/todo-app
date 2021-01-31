using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListAPI.Models;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgUploadersController : ControllerBase
    {
        private readonly TodoItemContext _context;

        public ImgUploadersController(TodoItemContext context)
        {
            _context = context;
        }

        // GET: api/ImgUploaders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImgUploader>>> GetImgUploaders()
        {
            return await _context.ImgUploaders.ToListAsync();
        }

        // GET: api/ImgUploaders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImgUploader>> GetImgUploader(long id)
        {
            var imgUploader = await _context.ImgUploaders.FindAsync(id);

            if (imgUploader == null)
            {
                return NotFound();
            }

            return imgUploader;
        }

        // PUT: api/ImgUploaders/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImgUploader(long id, ImgUploader imgUploader)
        {
            if (id != imgUploader.imgId)
            {
                return BadRequest();
            }

            _context.Entry(imgUploader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImgUploaderExists(id))
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

        // POST: api/ImgUploaders
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ImgUploader>> PostImgUploader(ImgUploader imgUploader)
        {
            _context.ImgUploaders.Add(imgUploader);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetImgUploader", new { id = imgUploader.imgId }, imgUploader);
        }

        // DELETE: api/ImgUploaders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ImgUploader>> DeleteImgUploader(long id)
        {
            var imgUploader = await _context.ImgUploaders.FindAsync(id);
            if (imgUploader == null)
            {
                return NotFound();
            }

            _context.ImgUploaders.Remove(imgUploader);
            await _context.SaveChangesAsync();

            return imgUploader;
        }

        private bool ImgUploaderExists(long id)
        {
            return _context.ImgUploaders.Any(e => e.imgId == id);
        }
    }
}

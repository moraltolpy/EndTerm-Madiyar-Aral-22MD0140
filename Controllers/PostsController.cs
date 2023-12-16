using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EndTerm.Models;
using Mapster;

[Route("api/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly DataContext _context;

    public PostsController(DataContext context)
    {
        _context = context;
    }

    // GET: api/Posts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts()
    {
        return await _context.Posts.ToListAsync();
    }

    // GET: api/Posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return NotFound();
        }

        return post;
    }

    // POST: api/Posts
    [HttpPost]
    public async Task<ActionResult<Post>> PostPost(PostDto post)
    {
        var postDbo = post.Adapt<Post>();
        _context.Posts.Add(postDbo);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPost", new { id = postDbo.Id }, post);
    }

    // PUT: api/Posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPost(int id, PostDto post)
    {
        post.DateOfCreation= DateTime.UtcNow;
        var postDbo = await _context.Posts.FindAsync(id);
        if (postDbo == null)
            return NotFound();
        postDbo = post.Adapt<Post>();
        postDbo.Id = id;
        _context.Entry(post).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PostExists(id))
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

    // DELETE: api/Posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.Id == id);
    }
}

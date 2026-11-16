using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public class HabitTagsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPut()]
    public async Task<ActionResult> UpsertHabitTags(string habitId, UpsertHabitTagsDto upsertHabitTagsDto)
    {
        Habit habit =await dbContext.Habits
            .Include(h => h.HabitTags)
            .FirstOrDefaultAsync(h => h.Id == habitId);

        if (habit is null)
        {
            return NotFound();
        }
        
        // Check existing habitTags to make sure no duplication occurs
        HashSet<string> currentTagIds = habit.HabitTags.Select(h => h.TagId).ToHashSet();
        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }
        
        // check if the provided Ids are exists in the tags table
        List<string> existingTagIds = await dbContext
            .Tags
            .Where(t => upsertHabitTagsDto.TagIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();
        if (existingTagIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("One or more tag Ids is invalid");
        }
        
        // Remove old tags that are NO LONGER in the incoming list
        habit.HabitTags.RemoveAll(ht => !upsertHabitTagsDto.TagIds.Contains(ht.TagId));
        
        // add the new tags except the already existing ones
        List<string> tagIdsToAdd = upsertHabitTagsDto.TagIds.Except(currentTagIds).ToList();
        habit.HabitTags.AddRange(tagIdsToAdd.Select(tagId => new HabitTag
        {
            HabitId = habit.Id,
            TagId = tagId,
            CreatedAtUtc = DateTime.UtcNow,
        }));
        await dbContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteHabitTag(string habitId, string tagId)
    {
        HabitTag? habitTag = await dbContext.HabitTags
            .SingleOrDefaultAsync(ht => ht.HabitId == habitId && ht.TagId == tagId);

        if (habitTag is null)
        {
            return NotFound();
        }
        
        dbContext.HabitTags.Remove(habitTag);
        await dbContext.SaveChangesAsync();
        
        return NoContent();
    }
}
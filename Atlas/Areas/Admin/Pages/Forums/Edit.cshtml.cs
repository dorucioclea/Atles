﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Atlas.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Atlas.Data;
using Atlas.Domain;
using Atlas.Services;

namespace Atlas.Areas.Admin.Pages.Forums
{
    public class EditModel : PageModel
    {
        private readonly IContextService _contextService;
        private readonly AtlasDbContext _dbContext;
        private readonly ICacheManager _cacheManager;

        public EditModel(IContextService contextService, AtlasDbContext dbContext, ICacheManager cacheManager)
        {
            _contextService = contextService;
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        [BindProperty]
        public ForumModel Forum { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forum = await _dbContext.Forums.FirstOrDefaultAsync(x => x.Id == id && x.Status != StatusType.Deleted);

            if (forum == null)
            {
                return NotFound();
            }

            Forum = new ForumModel
            {
                Id = forum.Id,
                ForumGroupId = forum.ForumGroupId,
                Name = forum.Name,
                PermissionSetId = forum.PermissionSetId
            };

            var site = await _contextService.CurrentSiteAsync();

            var groups = await _dbContext.ForumGroups
                .Where(x => x.SiteId == site.Id && x.Status != StatusType.Deleted)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            var permissionSets = await _dbContext.PermissionSets
                .Where(x => x.SiteId == site.Id && x.Status != StatusType.Deleted)
                .ToListAsync();

            ViewData["ForumGroupId"] = new SelectList(groups, "Id", "Name");
            ViewData["PermissionSetId"] = new SelectList(permissionSets, "Id", "Name");

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var forum = await _dbContext.Forums.FirstOrDefaultAsync(x => x.Id == Forum.Id && x.Status != StatusType.Deleted);

            if (forum == null)
            {
                return NotFound();
            }

            forum.UpdateDetails(Forum.ForumGroupId, Forum.Name, Forum.PermissionSetId);

            await _dbContext.SaveChangesAsync();

            _cacheManager.Remove(CacheKeys.Forums(forum.ForumGroupId));

            return RedirectToPage("./Index", new { forumGroupId = Forum.ForumGroupId });
        }

        public class ForumModel
        {
            public Guid Id { get; set; }

            [Required]
            public Guid ForumGroupId { get; set; }

            [Required]
            public string Name { get; set; }

            public Guid? PermissionSetId { get; set; }
        }
    }
}

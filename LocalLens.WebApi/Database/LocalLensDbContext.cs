using LocalLens.WebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocalLens.WebApi.Database;

public class LocalLensDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
	public LocalLensDbContext(DbContextOptions<LocalLensDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}

	public DbSet<PlaceType> PlaceTypes { get; set; }
	public DbSet<Preference> Preferences { get; set; }
	public DbSet<Question> Questions { get; set; }
	public DbSet<Option> Options { get; set; }

}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Thox.Hubs;
using Thox.Models.DataModels;
<<<<<<< HEAD
using Thox.Models.DataModels.Review;
=======
>>>>>>> d458c49 (init)


namespace Thox.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //private readonly IHubContext<SignalHub> _signalHubContext;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           // _signalHubContext = signalHubContext; , IHubContext<SignalHub> signalHubContext
        }

        public ApplicationDbContext()
        {
        }


        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomPrice> Prices { get; set; }
        public DbSet<ReservationSlot> ReservationSlots { get; set; }
        public DbSet<Group> Groups { get; set; }
<<<<<<< HEAD
=======
        public DbSet<ApiKeys> ApiKeys { get; set; }
>>>>>>> d458c49 (init)
        public DbSet<ExternalReviewModel> ExternalReviews { get; set; }

        //public override int SaveChanges()
        //{
        //    DetectAndUpdateSite();
        //    return base.SaveChanges();
        //}

        //public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    DetectAndUpdateSite();
        //    return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        //}

        //private void DetectAndUpdateSite()
        //{
        //    var modifiedPrices = ChangeTracker.Entries<RoomPrice>()
        //        .Where(e => e.State == EntityState.Modified)
        //        .Select(e => e.Entity);

        //    foreach (var price in modifiedPrices)
        //    {
        //        // Send SignalR notification
        //        _signalHubContext.Clients.All.SendAsync("PriceUpdated", price);
        //    }


        //    var modifiedSlots = ChangeTracker.Entries<ReservationSlot>()
        //        .Select(e => new { Entity = e.Entity, State = e.State });

        //    foreach (var entry in modifiedSlots)
        //    {
        //        // Send SignalR notification for each entity with its corresponding state
        //        _signalHubContext.Clients.All.SendAsync("ReservationSlotUpdate", entry);
        //    }

       // }
    }
}

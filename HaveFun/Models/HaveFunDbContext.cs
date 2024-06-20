using Microsoft.EntityFrameworkCore;

namespace HaveFun.Models
{
    public class HaveFunDbContext : DbContext
    {
        public HaveFunDbContext(DbContextOptions<HaveFunDbContext> options)
            : base(options)
        {
        }

        public DbSet<LabelCategory> LabelCategories { get; set; }

        public DbSet<Label> Labels { get; set; }

        public DbSet<MemberLabel> MemberLabels { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Activity> Activities { get; set; }

        public DbSet<ActivityImage> ActivityImages { get; set; }

        public DbSet<ActivityParticipant> ActivityParticipantes { get; set; }

        public DbSet<ActivityReview> ActivityReviews { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ComplaintCategory> ComplaintCategories { get; set; }

        public DbSet<FriendList> FriendLists { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostReview> PostReviews { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<UserPicture> UserPictures { get; set; }

        public DbSet<SwipeHistory> SwipeHistories { get; set; }

        public DbSet<UserReview> UserReviews { get; set; }
        public DbSet<ConId_UserId> ConId_UserId { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SenderMessages)
                .HasForeignKey(m => m.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChatRoom>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceiverMessages)
                .HasForeignKey(m => m.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendList>()
                .HasOne(m => m.User1)
                .WithMany(u => u.Friends1)
                .HasForeignKey(m => m.Clicked)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendList>()
                .HasOne(m => m.User2)
                .WithMany(u => u.Friends2)
                .HasForeignKey(m => m.BeenClicked)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityParticipant>()
                .HasOne(m => m.User)
                .WithMany(u => u.ActivityParticipants)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityParticipant>()
                .HasOne(m => m.Activity)
                .WithMany(u => u.ActivityParticipants)
                .HasForeignKey(m => m.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityReview>()
                .HasOne(m => m.ActId)
                .WithMany(u => u.ActivityReviews)
                .HasForeignKey(m => m.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityReview>()
                .HasOne(m => m.User)
                .WithMany(u => u.ActivityReviews)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(m => m.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
                .HasOne(m => m.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PostReview>()
                .HasOne(m => m.User)
                .WithMany(u => u.PostReviews)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserInfo>()
                .Property(u => u.PasswordSalt)
                .HasColumnType("varbinary(max)");

            modelBuilder.Entity<Post>()
                .HasOne(m => m.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserReview>()
                .HasOne(m => m.User1)
                .WithMany(u => u.ReportUsers)
                .HasForeignKey(m => m.ReportUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserReview>()
                .HasOne(m => m.User2)
                .WithMany(u => u.BeRepostedUsers)
                .HasForeignKey(m => m.BeReportedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

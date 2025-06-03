using FUNewsManagement.DataAccess;
using FUNewsManagement.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace StudentNameMVC.Seeder
{
    public class DatabaseSeeder
    {
        private readonly FUNewsManagementDbContext _context;
        private readonly IConfiguration _configuration;

        public DatabaseSeeder(FUNewsManagementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (await _context.SystemAccounts.AnyAsync())
            {
                return; 
            }

            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            var categories = new List<Category>
            {
                new Category { CategoryName = "Technology", CategoryDescription = "Technology news and updates", IsActive = true },
                new Category { CategoryName = "Education", CategoryDescription = "Educational news and announcements", IsActive = true },
                new Category { CategoryName = "Sports", CategoryDescription = "Sports news and events", IsActive = true },
                new Category { CategoryName = "Entertainment", CategoryDescription = "Entertainment and cultural news", IsActive = true }
            };

            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            var accounts = new List<SystemAccount>
            {
                new SystemAccount
                {
                    AccountName = "Admin",
                    AccountEmail = adminEmail,
                    AccountRole = 0,
                    AccountPassword = adminPassword
                },
                new SystemAccount
                {
                    AccountName = "John Staff",
                    AccountEmail = "staff@funews.com",
                    AccountRole = 1,
                    AccountPassword = "staff123"
                },
                new SystemAccount
                {
                    AccountName = "Jane Lecturer",
                    AccountEmail = "lecturer@funews.com",
                    AccountRole = 2,
                    AccountPassword = "lecturer123"
                }
            };

            await _context.SystemAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
            
            var tags = new List<Tag>
            {
                new Tag { TagName = "Breaking", Note = "Breaking news tag" },
                new Tag { TagName = "Important", Note = "Important announcements" },
                new Tag { TagName = "Event", Note = "Event related news" },
                new Tag { TagName = "Update", Note = "Updates and changes" }
            };

            await _context.Tags.AddRangeAsync(tags);
            await _context.SaveChangesAsync();

            var staffAccount = accounts.First(a => a.AccountRole == 1);
            var techCategory = categories.First(c => c.CategoryName == "Technology");
            var eduCategory = categories.First(c => c.CategoryName == "Education");

            var newsArticles = new List<NewsArticle>
            {
                new NewsArticle
                {
                    NewsArticleId = "NEWS1",
                    NewsTitle = "Library Extends Operating Hours",
                    Headline = "University library now open until midnight on weekdays",
                    NewsContent = "To support students during the exam season, the university library has extended its weekday hours. The facility will now remain open until 12 AM from Monday to Friday, providing more time for studying and accessing academic resources.",
                    NewsSource = "Library Management",
                    CategoryId = eduCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now.AddDays(-4)
                },
                new NewsArticle
                {
                    NewsArticleId = "NEWS2",
                    NewsTitle = "New Smart Parking System Implemented",
                    Headline = "Smart sensors help students find parking spots faster",
                    NewsContent = "The university has introduced a new smart parking system using real-time sensors to help drivers locate available spaces more efficiently. The system is now operational in all major parking lots around the campus.",
                    NewsSource = "Campus Facilities Department",
                    CategoryId = techCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new NewsArticle
                {
                    NewsArticleId = "NEWS3",
                    NewsTitle = "Career Fair 2025 Announced",
                    Headline = "Top companies to participate in university’s annual career event",
                    NewsContent = "The Career Services Center has officially announced the 2025 Career Fair, scheduled for next month. Over 50 companies, including international tech firms and local startups, will be participating to recruit graduating students.",
                    NewsSource = "Career Services Center",
                    CategoryId = eduCategory.CategoryId,
                    NewsStatus = true,
                    CreatedById = staffAccount.AccountId,
                    CreatedDate = DateTime.Now
                }

            };

            await _context.NewsArticles.AddRangeAsync(newsArticles);
            await _context.SaveChangesAsync();

            var breakingTag = tags.First(t => t.TagName == "Breaking");
            var importantTag = tags.First(t => t.TagName == "Important");
            var updateTag = tags.First(t => t.TagName == "Update");

            var newsTags = new List<NewsTag>
            {
                new NewsTag { NewsArticleId = newsArticles[0].NewsArticleId, TagId = importantTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[0].NewsArticleId, TagId = updateTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[1].NewsArticleId, TagId = breakingTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[1].NewsArticleId, TagId = updateTag.TagId },
                new NewsTag { NewsArticleId = newsArticles[2].NewsArticleId, TagId = importantTag.TagId }
            };

            await _context.NewsTags.AddRangeAsync(newsTags);
            await _context.SaveChangesAsync();
        }
    }
} 
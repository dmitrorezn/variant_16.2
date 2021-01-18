namespace Book.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookReview",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewerName = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        Book_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Book", t => t.Book_Id)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 512),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Author", t => t.AuthorId, cascadeDelete: false)
                .Index(t => t.AuthorId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookReview", "Book_Id", "dbo.Book");
            DropIndex("dbo.BookReview", new[] { "Book_Id" });
            DropTable("dbo.Book");
            DropTable("dbo.BookReview");
            DropTable("dbo.Author");
        }
    }
}

using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            string response;
            try
            {
                do
                {
                    Console.WriteLine("1. Display all blogs");
                    Console.WriteLine("2. Add Blog");
                    Console.WriteLine("3. Create Post");
                    Console.WriteLine("4. Display Posts");
                    Console.WriteLine("Enter q to quit");

                    response = Console.ReadLine();

                    switch (response)
                    {
                        case "1": displayAllBlogs(); break;
                        case "2": displayAddBlog(); break;
                        case "3": displayCreatePost(); break;
                        case "4": displayPosts(); break;
                        default:
                            if (response.ToUpper() == "Q")
                            {
                                Console.WriteLine("Bye"); break;
                            }
                            else {
                                Console.WriteLine("Please choose a valid option..."); break;
                            }
                            
                    }
                } while (response.ToUpper() != "Q");
                
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }

        public static void displayAllBlogs() {
            var db = new BloggingContext();

            // Display all Blogs from the database
            var query = db.Blogs.OrderBy(b => b.Name);

            Console.WriteLine("All blogs in the database:");

            if (query != null)
            {
                Console.WriteLine("data found let's go");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }

                Console.ReadLine();
            }
            else {
                Console.WriteLine("no data found yo wtf");
                Console.ReadLine();
            }
            
            
        }

        public static void displayAddBlog() {

            // Create and save a new Blog
            Console.Write("Enter a name for a new Blog: ");
            var name = Console.ReadLine();

            var blog = new Blog { Name = name };

            var db = new BloggingContext();
            db.AddBlog(blog);
            logger.Info("Blog added - {name}", name);
        }

        public static void displayCreatePost() {
            int blogId;
            string blogName;
            var db = new BloggingContext();
            bool blogExists = false;
            var name = "";
            string postTitle = "";
            string postContent = "";
            // we run this until they enter a blog post that exists 
            do {
                Console.Write("Enter the name of the blog or 'Q' to quit: ");

                name = Console.ReadLine();
                var query = db.Blogs.Where(b => b.Name == name)
                                   .FirstOrDefault();

                if (query != null)
                {
                    blogExists = true;
                    blogId = query.BlogId;
                    blogName = query.Name;


                    do
                    {
                        Console.WriteLine("Please enter the post title.");
                        postTitle = Console.ReadLine();
                        if (postTitle.Length < 3) {
                            Console.WriteLine("Post titles must be at least 3 characters");
                        }
                    } while (postTitle.Length < 3);

                    do
                    {
                        Console.WriteLine("Please enter some content.");
                        postContent = Console.ReadLine();
                        if (postContent.Length < 5) {
                            Console.WriteLine("Post content must be at least 5 characters");
                        }
                    } while (postContent.Length < 5);
                    
                    var post = new Post { Title = postTitle, Content = postContent, BlogId = blogId };
                    db.AddPost(post);

                    logger.Info("A post titled {postTitle} has been added to {blogName}", postTitle, blogName);
                    Console.ReadLine();
                }
                else {
                    Console.WriteLine("Please enter a valid blog title");
                }

            } while (blogExists != true || name.ToUpper() != "Q");
        }

        public static void displayPosts() {
            string response = "";
            do
            {
                Console.WriteLine("1. View posts from specific blog.");
                Console.WriteLine("2. View posts from all blogs.");
                response = Console.ReadLine();
                if (response != "1" && response != "2") {
                    Console.WriteLine("Please choose a valid option.");
                }
            } while (response != "1" && response != "2");
        }
    }
}

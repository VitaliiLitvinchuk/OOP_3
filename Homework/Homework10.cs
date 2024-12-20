using Microsoft.Extensions.DependencyInjection;
using Task.Homework.h_t_09_10_2024.Task2.Abstract;
using Task.Homework.h_t_09_10_2024.Task2.Entities;
using Task.Homework.h_t_09_10_2024.Task2.Interfaces;
using Task.Homework.h_t_09_10_2024.Task2.Services;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework10 : ITask
    {
        public void Start()
        {
            var serviceProvider = new ServiceCollection()
               .AddSingleton<IUserService, UserService>()
               .AddSingleton<ICatalogService, CatalogService>()
               .AddSingleton<IEmailSender, EmailSender>()
               .AddSingleton<ILibrary, LibraryService>()
               .BuildServiceProvider();

            var userService = serviceProvider.GetService<IUserService>()!;
            var catalogService = serviceProvider.GetService<ICatalogService>()!;
            var library = serviceProvider.GetService<ILibrary>()!;

            var user = new User { Email = "user@example.com" };
            userService.RegisterUser(user);

            library.SubscribeToCategory(user, "Fiction");

            var book = new Book { Id = 23, Title = "The Great Gatsby", Category = "Fiction" };
            var book1 = new Book { Title = "The Great Gatsby123", Category = "Fiction" };
            catalogService.AddBook(book);
            catalogService.AddBook(book1);

            library.NotifyUsersAboutNewBook(book);
            library.NotifyUsersAboutNewBook(book1);

            var books = catalogService.GetBooksByCategory("Fiction");
            foreach (var b in books)
            {
                Console.WriteLine($"Book Id: {b.Id}, Title: {b.Title}, Category: {b.Category}");
            }
        }
    }
}

namespace Task.Homework.h_t_09_10_2024
{
    namespace Task2
    {
        namespace Abstract
        {
            public abstract class AbstractBook
            {
                public int Id { get; init; }
                public string Category { get; set; } = string.Empty;
                public string Title { get; set; } = string.Empty;
            }

            public abstract class AbstractUser
            {
                public int Id { get; init; }
                public string Email { get; set; } = string.Empty;
                public List<string> SubscribedCategories { get; set; } = [];
            }

        }

        namespace Interfaces
        {
            public interface IUserService
            {
                void RegisterUser(AbstractUser user);
                void EditUser(int id, string newEmail);
                void DeleteUser(int id);
                AbstractUser GetUserById(int id);
                IEnumerable<AbstractUser> GetAllUsers();
            }

            public interface ICatalogService
            {
                void AddBook(AbstractBook book);
                void EditBook(int id, string newCategory);
                void DeleteBook(int id);
                IEnumerable<AbstractBook> GetBooksByCategory(string category);
                IEnumerable<AbstractBook> GetAllBooks();
            }

            public interface IEmailSender
            {
                void SendEmail(string to, string message);
            }

            public interface ILibrary
            {
                void SubscribeToCategory(AbstractUser user, string category);
                void NotifyUsersAboutNewBook(AbstractBook book);
            }
        }

        namespace Entities
        {
            public class Book : AbstractBook
            {
                private static int _totalBooks = 0;
                public Book()
                {
                    Id = ++_totalBooks;
                }
            }

            public class User : AbstractUser
            {
                private static int _totalUsers = 0;
                public User()
                {
                    Id = ++_totalUsers;
                }
            }
        }

        namespace Services
        {
            public class UserService : IUserService
            {
                private readonly List<AbstractUser> _users = [];

                public void RegisterUser(AbstractUser user)
                {
                    _users.Add(user);
                }

                public void EditUser(int id, string newEmail)
                {
                    var user = GetUserById(id);
                    if (user != null)
                    {
                        user.Email = newEmail;
                    }
                }

                public void DeleteUser(int id)
                {
                    var user = GetUserById(id);
                    if (user != null)
                    {
                        _users.Remove(user);
                    }
                }

                public AbstractUser? GetUserById(int id)
                {
                    return _users.FirstOrDefault(u => u.Id == id);
                }

                public IEnumerable<AbstractUser> GetAllUsers()
                {
                    return _users;
                }
            }

            public class CatalogService : ICatalogService
            {
                private readonly List<AbstractBook> _books = [];

                public void AddBook(AbstractBook book)
                {
                    _books.Add(book);
                }

                public void EditBook(int id, string newCategory)
                {
                    var book = _books.FirstOrDefault(b => b.Id == id);
                    if (book != null)
                    {
                        book.Category = newCategory;
                    }
                }

                public void DeleteBook(int id)
                {
                    var book = _books.FirstOrDefault(b => b.Id == id);
                    if (book != null)
                    {
                        _books.Remove(book);
                    }
                }

                public IEnumerable<AbstractBook> GetBooksByCategory(string category)
                {
                    return _books.Where(b => b.Category == category);
                }

                public IEnumerable<AbstractBook> GetAllBooks()
                {
                    return _books;
                }
            }

            public class EmailSender : IEmailSender
            {
                public void SendEmail(string to, string message)
                {
                    Console.WriteLine($"Sending email to {to}: {message}");
                }
            }

            public class LibraryService(IEmailSender emailSender, IUserService userService) : ILibrary
            {
                private readonly IEmailSender _emailSender = emailSender;
                private readonly IUserService _userService = userService;

                public void SubscribeToCategory(AbstractUser user, string category)
                {
                    user.SubscribedCategories.Add(category);
                }

                public void NotifyUsersAboutNewBook(AbstractBook book)
                {
                    var users = _userService.GetAllUsers();
                    foreach (var user in users)
                        if (user.SubscribedCategories.Contains(book.Category))
                            _emailSender.SendEmail(user.Email, $"New book added in {book.Category}: {book.Title}");
                }
            }
        }
    }
}
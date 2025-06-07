using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;

namespace MinhaBiblioteca.Api.Data.Repositories;

public class BookRepository(AppDbContext db) : Repository<Book>(db), IBookRepository { }

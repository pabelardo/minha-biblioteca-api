using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;

namespace MinhaBiblioteca.Api.Data.Repositories;

public class AuthorRepository(AppDbContext db) : Repository<Author>(db), IAuthorRepository { }

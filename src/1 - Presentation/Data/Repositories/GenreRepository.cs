using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;

namespace MinhaBiblioteca.Api.Data.Repositories;

public class GenreRepository(AppDbContext db) : Repository<Genre>(db), IGenreRepository { }

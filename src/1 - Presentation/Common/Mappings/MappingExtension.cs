using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Requests.Genres;

namespace MinhaBiblioteca.Api.Common.Mappings;

public static class MappingExtension
{
    public static BookDto ToDto(this Book book, bool fillAuthor = false, bool fillGenre = false) => new()
    {
        Id = book.Id,
        Name = book.Name,
        CreatedAt = book.CreatedAt,
        AuthorId = book.AuthorId,
        GenreId = book.GenreId,
        Author = (fillAuthor && book.Author != null) ? book.Author.ToDto() : null,
        Genre = (fillGenre && book.Genre != null) ? book.Genre.ToDto() : null
    };

    public static AuthorDto ToDto(this Author author) => new()
    {
        Id = author.Id,
        Name = author.Name,
        CreatedAt = author.CreatedAt,
        Books = author.Books.Select(b => b.ToDto()).ToList()
    };

    public static GenreDto ToDto(this Genre genre) => new()
    {
        Id = genre.Id,
        Name = genre.Name,
        Description = genre.Description ?? string.Empty,
        CreatedAt = genre.CreatedAt,
        Books = genre.Books.Select(b => b.ToDto()).ToList()
    };

    public static Book ToEntity(this CreateBookRequest request) => new()
    {
        Name = request.Name,
        AuthorId = request.AuthorId,
        GenreId = request.GenreId
    };

    public static Book ToEntity(this UpdateBookRequest request) => new()
    {
        Id = request.Id,
        Name = request.Name,
        AuthorId = request.AuthorId,
        GenreId = request.GenreId
    };

    public static Author ToEntity(this CreateAuthorRequest request) => new()
    {
        Name = request.Name,
        Email = request.Email,
        Phone = request.Phone
    };

    public static Author ToEntity(this UpdateAuthorRequest request) => new()
    {
        Id = request.Id,
        Name = request.Name,
        Email = request.Email,
        Phone = request.Phone
    };

    public static Genre ToEntity(this CreateGenreRequest request) => new()
    {
        Name = request.Name,
        Description = request.Description
    };

    public static Genre ToEntity(this UpdateGenreRequest request) => new()
    {
        Id = request.Id,
        Name = request.Name,
        Description = request.Description
    };

    public static List<AuthorDto> ToList(this IEnumerable<Author> authors) => 
        authors.Select(a => a.ToDto()).ToList();

    public static List<BookDto> ToList(this IEnumerable<Book> books) =>
        books.Select(b => b.ToDto()).ToList();

    public static List<GenreDto> ToList(this IEnumerable<Genre> genres) =>
        genres.Select(g => g.ToDto()).ToList();
}

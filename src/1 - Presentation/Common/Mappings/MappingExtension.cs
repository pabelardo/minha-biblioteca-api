using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Requests.Genres;

namespace MinhaBiblioteca.Api.Common.Mappings;

public static class MappingExtension
{
    public static BookDto ToDto(this Book book)
    {
        return new BookDto
        {
            Id = book.Id,
            Name = book.Name,
            CreatedAt = book.CreatedAt,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
            Author = book.Author.ToDto(),
            Genre = book.Genre.ToDto()
        };
    }

    public static AuthorDto ToDto(this Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            CreatedAt = author.CreatedAt,
            Books = author.Books.Select(b => b.ToDto()).ToList()
        };
    }

    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name,
            CreatedAt = genre.CreatedAt,
            Books = genre.Books.Select(b => b.ToDto()).ToList()
        };
    }

    public static Book ToEntity(this CreateBookRequest request)
    {
        return new Book
        {
            Name = request.Name,
            AuthorId = request.AuthorId,
            GenreId = request.GenreId
        };
    }

    public static Book ToEntity(this UpdateBookRequest request)
    {
        return new Book
        {
            Id = request.Id,
            Name = request.Name,
            AuthorId = request.AuthorId,
            GenreId = request.GenreId
        };
    }

    public static Author ToEntity(this CreateAuthorRequest request)
    {
        return new Author
        {
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };
    }

    public static Genre ToEntity(this CreateGenreRequest request)
    {
        return new Genre
        {
            Name = request.Name,
            Description = request.Description
        };
    }
}

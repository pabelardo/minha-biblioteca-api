using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Requests.Genres;

namespace MinhaBiblioteca.Tests.Api;

public class MappingExtensionsTests
{
    [Fact(DisplayName = "Mapeamento De Book Para BookDto")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapBookToBookDto()
    {
        // Arrange
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Test Book",
            CreatedAt = DateTime.UtcNow,
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
        };

        // Act
        var bookDto = book.ToDto();

        // Assert
        Assert.NotNull(bookDto);
        Assert.Equal(book.Id, bookDto.Id);
        Assert.Equal(book.Name, bookDto.Name);
        Assert.Equal(book.CreatedAt, bookDto.CreatedAt);
        Assert.Equal(book.AuthorId, bookDto.AuthorId);
        Assert.Equal(book.GenreId, bookDto.GenreId);
    }

    [Fact(DisplayName = "Mapeamento De Author Para AuthorDto")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapAuthorToAuthorDto()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Test Author",
            Email = "teste@teste.com",
            Phone = "1234567890",
            CreatedAt = DateTime.UtcNow,
            Books =
            [
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Book 1",
                    CreatedAt = DateTime.UtcNow,
                    AuthorId = Guid.NewGuid(),
                    GenreId = Guid.NewGuid()
                }
            ]
        };

        // Act
        var authorDto = author.ToDto();

        // Assert
        Assert.NotNull(authorDto);
        Assert.Equal(author.Id, authorDto.Id);
        Assert.Equal(author.Name, authorDto.Name);
        Assert.Equal(author.Email, authorDto.Email);
        Assert.Equal(author.Phone, authorDto.Phone);
        Assert.Equal(author.CreatedAt, authorDto.CreatedAt);
        Assert.NotEmpty(authorDto.Books);
    }

    [Fact(DisplayName = "Mapeamento De Genre Para GenreDto")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapGenreToGenreDto()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Test Genre",
            Description = "Test Description",
            CreatedAt = DateTime.UtcNow,
            Books =
            [
                new Book
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Book 1",
                    CreatedAt = DateTime.UtcNow,
                    AuthorId = Guid.NewGuid(),
                    GenreId = Guid.NewGuid()
                }
            ]
        };

        // Act
        var genreDto = genre.ToDto();

        // Assert
        Assert.NotNull(genreDto);
        Assert.Equal(genre.Id, genreDto.Id);
        Assert.Equal(genre.Name, genreDto.Name);
        Assert.Equal(genre.Description, genreDto.Description);
        Assert.Equal(genre.CreatedAt, genreDto.CreatedAt);
        Assert.NotEmpty(genreDto.Books);
    }

    [Fact(DisplayName = "Mapeamento De CreateBookRequest Para Book")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapCreateBookRequestToBook()
    {
        // Arrange
        var request = new CreateBookRequest
        {
            Name = "Test Book",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        // Act
        var book = request.ToEntity();

        // Assert
        Assert.NotNull(book);
        Assert.Equal(request.Name, book.Name);
        Assert.Equal(request.AuthorId, book.AuthorId);
        Assert.Equal(request.GenreId, book.GenreId);
    }

    [Fact(DisplayName = "Mapeamento De UpdateBookRequest Para Book")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapUpdateBookRequestToBook()
    {
        // Arrange
        var request = new UpdateBookRequest
        {
            Id = Guid.NewGuid(),
            Name = "Updated Book",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };
        // Act
        var book = request.ToEntity();
        // Assert
        Assert.NotNull(book);
        Assert.Equal(request.Id, book.Id);
        Assert.Equal(request.Name, book.Name);
        Assert.Equal(request.AuthorId, book.AuthorId);
        Assert.Equal(request.GenreId, book.GenreId);
    }

    [Fact(DisplayName = "Alterar Entidade Book com UpdateBookRequest")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ChangeEntityProperties_ShouldUpdateBookWithUpdateBookRequest()
    {
        // Arrange
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Old Book",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        var request = new UpdateBookRequest
        {
            Id = book.Id,
            Name = "Updated Book",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        // Act
        book.ChangeEntityProperties(request);

        // Assert
        Assert.Equal(request.Id, book.Id);
        Assert.Equal(request.Name, book.Name);
        Assert.Equal(request.AuthorId, book.AuthorId);
        Assert.Equal(request.GenreId, book.GenreId);
    }

    [Fact(DisplayName = "Mapeamento De CreateAuthorRequest Para Author")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapCreateAuthorRequestToAuthor()
    {
        //Arrange
        var request = new CreateAuthorRequest
        {
            Name = "Test Author",
            Email = "teste@teste.com",
            Phone = "(12) 3456-7890"
        };

        // Act
        var author = request.ToEntity();

        // Assert
        Assert.NotNull(author);
        Assert.Equal(request.Name, author.Name);
        Assert.Equal(request.Email, author.Email);
        Assert.Equal("1234567890", author.Phone);
    }

    [Fact(DisplayName = "Mapeamento De UpdateAuthorRequest Para Author")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapUpdateAuthorRequestToAuthor()
    {
        // Arrange
        var request = new UpdateAuthorRequest
        {
            Id = Guid.NewGuid(),
            Name = "Updated Author",
            Email = "teste@teste.com",
            Phone = "(12) 3456-7890"
        };

        // Act
        var author = request.ToEntity();

        // Assert
        Assert.NotNull(author);
        Assert.Equal(request.Id, author.Id);
        Assert.Equal(request.Name, author.Name);
        Assert.Equal(request.Email, author.Email);
        Assert.Equal("1234567890", author.Phone);
    }

    [Fact(DisplayName = "Alterar Entidade Author com UpdateAuthorRequest")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ChangeEntityProperties_ShouldUpdateAuthorWithUpdateAuthorRequest()
    {
        // Arrange
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Old Author",
            Email = "teste@teste.com",
            Phone = "(12) 3456-7890"
        };

        var request = new UpdateAuthorRequest
        {
            Id = author.Id,
            Name = "New Author",
            Email = "teste@teste.com",
            Phone = "(12) 3456-7890"
        };

        // Act
        author.ChangeEntityProperties(request);

        // Assert
        Assert.Equal(request.Id, author.Id);
        Assert.Equal(request.Name, author.Name);
        Assert.Equal(request.Email, author.Email);
        Assert.Equal("1234567890", author.Phone);

        request.Phone = null;

        author.ChangeEntityProperties(request);

        Assert.Equal(string.Empty, author.Phone);
    }

    [Fact(DisplayName = "Mapeamento De CreateGenreRequest Para Genre")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapCreateGenreRequestToGenre()
    {
        // Arrange
        var request = new CreateGenreRequest
        {
            Name = "Test Genre",
            Description = "Test Description"
        };

        // Act
        var genre = request.ToEntity();

        // Assert
        Assert.NotNull(genre);
        Assert.Equal(request.Name, genre.Name);
        Assert.Equal(request.Description, genre.Description);
    }

    [Fact(DisplayName = "Mapeamento De UpdateGenreRequest Para Genre")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToEntity_ShouldMapUpdateGenreRequestToGenre()
    {
        // Arrange
        var request = new UpdateGenreRequest
        {
            Id = Guid.NewGuid(),
            Name = "Updated Genre",
            Description = "Updated Description"
        };

        // Act
        var genre = request.ToEntity();

        // Assert
        Assert.NotNull(genre);
        Assert.Equal(request.Id, genre.Id);
        Assert.Equal(request.Name, genre.Name);
        Assert.Equal(request.Description, genre.Description);
    }

    [Fact(DisplayName = "Alterar Entidade Genre com UpdateGenreRequest")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ChangeEntityProperties_ShouldUpdateGenreWithUpdateGenreRequest()
    {
        // Arrange
        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Old Genre",
            Description = "Old Description"
        };

        var request = new UpdateGenreRequest
        {
            Id = genre.Id,
            Name = "New Genre",
            Description = "New Description"
        };

        // Act
        genre.ChangeEntityProperties(request);

        // Assert
        Assert.Equal(request.Id, genre.Id);
        Assert.Equal(request.Name, genre.Name);
        Assert.Equal(request.Description, genre.Description);
    }

    [Fact(DisplayName = "Mapeamento De IEnumerable<Author> Para List<AuthorDto>")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapListAuthorToListAuthorDto()
    {
        // Arrange
        var authors = new List<Author>
        {
            new Author
            {
                Id = Guid.NewGuid(),
                Name = "Author 1",
                Email = "teste@teste.com"
            }
        };

        // Act
        var authorDtos = authors.ToList();

        // Assert
        Assert.NotNull(authorDtos);
        Assert.NotEmpty(authorDtos);
    }

    [Fact(DisplayName = "Mapeamento De IEnumerable<Book> Para List<BookDto>")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapListBookToListBookDto()
    {
        // Arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Book 1",
                CreatedAt = DateTime.UtcNow,
                AuthorId = Guid.NewGuid(),
                GenreId = Guid.NewGuid()
            }
        };
        // Act
        var bookDtos = books.ToList();
        // Assert
        Assert.NotNull(bookDtos);
        Assert.NotEmpty(bookDtos);
    }

    [Fact(DisplayName = "Mapeamento De IEnumerable<Genre> Para List<GenreDto>")]
    [Trait("Category", "Mappings")]
    public void MappingExtension_ToDto_ShouldMapListGenreToListGenreDto()
    {
        // Arrange
        var genres = new List<Genre>
        {
            new Genre
            {
                Id = Guid.NewGuid(),
                Name = "Genre 1",
                Description = "Description 1"
            }
        };
        // Act
        var genreDtos = genres.ToList();
        // Assert
        Assert.NotNull(genreDtos);
        Assert.NotEmpty(genreDtos);
    }
}
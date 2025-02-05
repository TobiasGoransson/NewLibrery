using ApplicationBook.Interfaces.RepoInterfaces;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Domain.Dtos;
using ApplicationBook.Authors.Commands.CreateAuthor;

namespace ApplicationBook.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<BookDto>>
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateBookCommandHandler> _logger;

        public CreateBookCommandHandler(IRepository<Book> bookRepository, IRepository<Author> authorRepository, IMediator mediator, ILogger<CreateBookCommandHandler> logger)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<OperationResult<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting CreateBookCommand for {BookTitle}", request.NewBook.Title);

            // Check if the author exists
            var author = await _authorRepository.GetByIdAsync(request.NewBook.AId, cancellationToken);
            if (author == null)
            {
                // Create the author if it does not exist
                var createAuthorCommand = new CreateAuthorCommand(new AuthorDto(request.NewBook.AuthorFirstName, request.NewBook.AuthorLastName));
                var authorResult = await _mediator.Send(createAuthorCommand, cancellationToken);

                if (!authorResult.Success)
                {
                    _logger.LogError("Failed to create author: {ErrorMessage}", authorResult.ErrorMessage);
                    return OperationResult<BookDto>.Failure("Failed to create the author.");
                }

                author = new Author
                {
                    AId = authorResult.Data.Id,
                    FirstName = authorResult.Data.FirstName,
                    LastName = authorResult.Data.LastName
                };

                _logger.LogInformation("Created new author with ID {AuthorId}", author.AId);
            }

            // Create the book
            var book = new Book
            {
                Title = request.NewBook.Title,
                Description = request.NewBook.Description,
                AId = author.AId
            };

            try
            {
                await _bookRepository.CreateAsync(book);

                var bookDto = new BookDto(book.Title, book.Description, book.AId, book.Author.FirstName, book.Author.LastName);

                _logger.LogInformation("Successfully created book with ID {BookId}", book.BId);

                return OperationResult<BookDto>.Successfull(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a book.");
                return OperationResult<BookDto>.Failure("An error occurred while creating the book.");
            }
        }
    }
}
    //public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<BookDto>>
    //{
    //    private readonly IRepository<Book> _bookRepository;
    //    private readonly IRepository<Author> _authorRepository;
    //    private readonly ILogger<CreateBookCommandHandler> _logger;

    //    public CreateBookCommandHandler(IRepository<Book> bookRepository, IRepository<Author> authorRepository, ILogger<CreateBookCommandHandler> logger)
    //    {
    //        _bookRepository = bookRepository;
    //        _authorRepository = authorRepository;
    //        _logger = logger;
    //    }

    //    public async Task<OperationResult<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    //    {
    //        _logger.LogInformation("Starting CreateBookCommand for {BookTitle}", request.NewBook.Title);

    //        // Check if the author exists
    //        var author = await _authorRepository.GetByIdAsync(request.NewBook.AuthorId, cancellationToken);
    //        if (author == null)
    //        {
    //            // Create the author if it does not exist
    //            author = new Author
    //            {
    //                FirstName = request.NewBook.AuthorFirstName,
    //                LastName = request.NewBook.AuthorLastName
    //            };

    //            await _authorRepository.CreateAsync(author);
    //            _logger.LogInformation("Created new author with ID {AuthorId}", author.AId);
    //        }

    //        // Create the book
    //        var book = new Book
    //        {
    //            Title = request.NewBook.Title,
    //            Description = request.NewBook.Description,
    //            AId = author.AId
    //        };

    //        try
    //        {
    //            await _bookRepository.CreateAsync(book);

    //            var bookDto = new BookDto( book.Title, book.Description, book.AId);

    //            _logger.LogInformation("Successfully created book with ID {BookId}", book.BId);

    //            return OperationResult<BookDto>.Successfull(bookDto);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error occurred while creating a book.");
    //            return OperationResult<BookDto>.Failure("An error occurred while creating the book.");
    //        }
    //    }
    //    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<List<BookDto>>>
    //    {
    //        private readonly IRepository<Book> _repository;
    //        private readonly ILogger<CreateBookCommandHandler> _logger;
    //        private readonly IMediator _mediator; // För att skicka kommandot till authorController

    //        public CreateBookCommandHandler(IRepository<Book> repository, IMediator mediator, ILogger<CreateBookCommandHandler> logger)
    //        {
    //            _repository = repository;
    //            _logger = logger;
    //            _mediator = mediator;
    //        }
    //        public async Task<OperationResult<List<BookDto>>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    //        { 
    //            _logger.LogInformation("Handling CreateBookCommand for book: {BookTitle}", request.NewBook.Title);

    //            // Kontrollera om författaren behöver skapas
    //            if (request.NewBook.AId == null || request.NewBook.AId == 0)
    //            {
    //                _logger.LogInformation("Author ID is missing. Attempting to create a new author.");

    //                // Skapa AuthorDto med dess parameteriserade konstruktor
    //                var createAuthorCommand = new CreateAuthorCommand(
    //                    new AuthorDto(request.NewBook.AuthorFirstName, request.NewBook.AuthorLastName));

    //                // Skicka CreateAuthorCommand för att skapa författaren
    //                var authorResult = await _mediator.Send(createAuthorCommand, cancellationToken);

    //                if (!authorResult.Success)
    //                {
    //                    _logger.LogError("Failed to create author: {ErrorMessage}", authorResult.ErrorMessage);
    //                    return OperationResult<List<BookDto>>.Failure("Failed to create the author.");
    //                }

    //                // Använd det genererade författar-ID:t
    //                request.NewBook.AId = authorResult.Data.Id;
    //            }

    //            // Hämta det genererade författar-ID:t
    //            var newAuthorId = authorResult.Data.Id;

    //        if (newAuthorId == 0)
    //        {
    //            _logger.LogError("New author ID was not generated.");
    //            return OperationResult<List<BookDto>>.Failure("Failed to retrieve new author ID.");
    //        }

    //        request.NewBook.AId = newAuthorId; // Uppdatera AId i BookDto
    //    }

    //    try
    //    {
    //        // Skapa bok
    //        var book = new Book
    //        {
    //            Title = request.NewBook.Title,
    //            Description = request.NewBook.Description,
    //            AId = request.NewBook.AId
    //        };

    //        await _repository.CreateAsync(book); // Spara boken i databasen
    //        _logger.LogInformation("Book '{BookTitle}' added successfully.", request.NewBook.Title);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error occurred while adding book '{BookTitle}'.", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("An error occurred while adding the book.");
    //    }

    //    // Hämta alla böcker för att returnera uppdaterad lista
    //    var books = await _repository.GetAllAsync();

    //    if (books == null || !books.Any())
    //    {
    //        _logger.LogWarning("No books found after adding '{BookTitle}'.", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("No books found in the repository.");
    //    }

    //    var bookDtos = books.Select(b => new BookDto
    //    {
    //        Title = b.Title,
    //        Description = b.Description,
    //        AId = b.AId
    //    }).ToList();

    //    return OperationResult<List<BookDto>>.Successfull(bookDtos);
    //}

    //public async Task<OperationResult<List<BookDto>>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    //{
    //    _logger.LogInformation("Handling CreateBookCommand to add new book titled: {BookTitle}", request.NewBook.Title);

    //    // Kontrollera om författaren finns eller behöver skapas
    //    if (request.NewBook.Author == null || request.NewBook.Author.Id == 0)
    //    {
    //        _logger.LogInformation("Author details are missing. Creating a new Author.");

    //        // Skapa en ny författare via CreateAuthorCommand
    //        var createAuthorCommand = new CreateAuthorCommand(request.NewBook.Author);
    //        var authorResult = await _mediator.Send(createAuthorCommand, cancellationToken);

    //        if (!authorResult.IsSuccessfull || authorResult.Data == null)
    //        {
    //            _logger.LogError("Failed to create Author.");
    //            return OperationResult<List<BookDto>>.Failure("Failed to create a new Author.");
    //        }

    //        // Sätt det nya författar-ID:t
    //        request.NewBook.Author.Id = authorResult.Data.Id;
    //        request.NewBook.AId = authorResult.Data.Id;
    //    }

    //    try
    //    {
    //        // Mappa DTO till domänmodell
    //        var book = new Book
    //        {
    //            Title = request.NewBook.Title,
    //            Description = request.NewBook.Description,
    //            AId = request.NewBook.Author.Id // Använd författarens ID
    //        };

    //        // Spara boken
    //        await _repository.CreateAsync(book);

    //        _logger.LogInformation("Book titled '{BookTitle}' successfully added.", request.NewBook.Title);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error occurred while adding book titled: {BookTitle}", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("An error occurred while adding the book.");
    //    }

    //    // Hämta alla böcker från databasen
    //    var books = await _repository.GetAllAsync();

    //    if (books == null || !books.Any())
    //    {
    //        _logger.LogWarning("No books found after adding '{BookTitle}'.", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("No books found in the repository.");
    //    }

    //    // Mappa domänmodell till DTO
    //    var bookDtos = books.Select(b => new BookDto
    //    {
    //        Title = b.Title,
    //        Description = b.Description,
    //        AId = b.AId,
    //        Author = new AuthorDto // Mappa författarens detaljer
    //        {

    //            FirstName = b.Author.FirstName,
    //            LastName = b.Author.LastName
    //        }
    //    }).ToList();

    //    _logger.LogInformation("Retrieved updated list of books after adding '{BookTitle}'.", request.NewBook.Title);
    //    return OperationResult<List<BookDto>>.Successfull(bookDtos);
    //}

    //public async Task<OperationResult<List<BookDto>>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    //{
    //    _logger.LogInformation("Handling CreateBookCommand to add new book titled: {BookTitle}", request.NewBook.Title);

    //    if (request.NewBook.AId == null)
    //    {
    //        _logger.LogInformation("AId is null. Creating a new Author.");

    //        var createAuthorCommand = new CreateAuthorCommand(
    //            new AuthorDto
    //            {
    //                FirstName = request.NewBook.AuthorFirstName, // Lägg till AuthorFirstName i CreateBookCommand
    //                LastName = request.NewBook.AuthorLastName   // Lägg till AuthorLastName i CreateBookCommand
    //            });

    //        var authorResult = await _mediator.Send(createAuthorCommand, cancellationToken);

    //        if (!authorResult.IsSuccessfull)
    //        {
    //            _logger.LogError("Failed to create Author.");
    //            return OperationResult<List<BookDto>>.Failure("Failed to create a new Author.");
    //        }

    //        // Använd det nya AId från det skapade författaren
    //        request.NewBook.AId = authorResult.Data.Id;
    //    }
    //    try
    //    {
    //        // Mappa DTO till domänmodell
    //        var book = new Book
    //        {
    //            Title = request.NewBook.Title,
    //            Description = request.NewBook.Description,
    //            AId = request.NewBook.AId
    //        };

    //        // Spara den nya boken i databasen
    //        await _repository.CreateAsync(book);
    //        _logger.LogInformation("Book titled '{BookTitle}' successfully added.", request.NewBook.Title);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error occurred while adding book titled: {BookTitle}", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("An error occurred while adding the book.");
    //    }

    //    // Hämta alla böcker från databasen
    //    var books = await _repository.GetAllAsync();

    //    if (books == null || !books.Any())
    //    {
    //        _logger.LogWarning("No books found after adding '{BookTitle}'.", request.NewBook.Title);
    //        return OperationResult<List<BookDto>>.Failure("No books found in the repository.");
    //    }

    //    // Mappa domänmodell till DTO
    //    var bookDtos = books.Select(b => new BookDto(b.Title, b.Description, b.AId)).ToList();  


    //    _logger.LogInformation("Retrieved updated list of books after adding '{BookTitle}'.", request.NewBook.Title);
    //    return OperationResult<List<BookDto>>.Successfull(bookDtos);
    //}


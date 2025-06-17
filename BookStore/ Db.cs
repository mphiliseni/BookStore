namespace BookStore.DB;

public record BookStoreModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class BookStoreDB
{
    private static List<BookStoreModel> _bookStores = new List<BookStoreModel>
    {
        new BookStoreModel { Id = 1, Name = "Bridge Books" },
        new BookStoreModel { Id = 2, Name = "Book Lounge" },
        new BookStoreModel { Id = 3, Name = "Exclusive Books" },
        new BookStoreModel { Id = 4, Name = "Wordsworth Books" },
        new BookStoreModel { Id = 5, Name = "The Book Cellar" }
    };

    public static List<BookStoreModel> GetBookStores() => _bookStores;

    public static BookStoreModel? GetBookStoreById(int id) =>
        _bookStores.SingleOrDefault(b => b.Id == id);

    public static BookStoreModel CreateBookStore(BookStoreModel bookStore)
    {
        _bookStores.Add(bookStore);
        return bookStore;
    }

    public static BookStoreModel UpdateBookStore(BookStoreModel bookStore)
    {
        _bookStores = _bookStores.Select(b =>
        {
            if (b.Id == bookStore.Id)
            {
                b = bookStore;
            }
            return b;
        }).ToList();
        return bookStore;
    }

    public static void RemoveBookStore(int id) =>
        _bookStores = _bookStores.Where(b => b.Id != id).ToList();
}

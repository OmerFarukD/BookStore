﻿using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstracts;

namespace Repositories.EfCore;

public sealed class BookRepository : RepositoryBase<Book>,IBookRepository
{
    public BookRepository(RepositoryContext context) : base(context)
    {
        
    }

    public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
    {
        var books = await FindByCondition(b=>b.Price>=bookParameters.MinPrice && b.Price<=bookParameters.MaxPrice
                ,trackChanges)
            .Search(bookParameters.SearchTerm)
            .Sort(bookParameters.OrderBy)
            .OrderBy(x => x.Id).ToListAsync();
        return PagedList<Book>.ToPagedList(books,bookParameters.PageNumber,bookParameters.PageSize);
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync(bool trackChanges)
    {
        var books = await _context.Books.ToListAsync();
        return books;
    }

    public async Task<Book?> GetAllBookByIdAsync(int id, bool trackChanges) =>await FindByCondition(x => x.Id.Equals(id), trackChanges).SingleOrDefaultAsync();

    public void CreateOneBook(Book book) => Create(book);

    public void DeleteOneBook(Book book) => Delete(book);


    public void UpdateOneBook(Book book) => Update(book);
    public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
    {
        return await _context.Books.Include(b => b.Category)
            .OrderBy(b => b.Id)
            .ToListAsync();
    }
}
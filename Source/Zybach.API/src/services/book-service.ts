import Book from "../models/book";
import { BookCreateDto, BookDto } from "../dtos/book-create-dto";
import { InternalServerError } from "../errors/internal-server-error";
import { provideSingleton } from "../util/provide-singleton";

@provideSingleton(BookService)
export class BookService{
    public async add(book: BookCreateDto){
        const newBook = new Book(book);
        
        try {
           await newBook.save();
        } catch (err){
            throw new InternalServerError("Couldn't add book :(");
        }

        return newBook;  
    }

    public async get() : Promise<BookDto[]>{
        const books = await Book.find();

        return books;
    }
}
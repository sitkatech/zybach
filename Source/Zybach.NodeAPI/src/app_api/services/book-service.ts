import Book from "../models/book";
import { BookCreateDto } from "../dtos/book-create-dto";
import { ApiError } from "../../errors/apiError";

export class BookService{
    public async add(book: BookCreateDto){
        const newBook = new Book(book);
        
        try {
           await newBook.save();
        } catch (err){
            throw new ApiError("Internal Server Error", 500, "Couldn't add book :(");
        }

        return newBook;  
    }
}
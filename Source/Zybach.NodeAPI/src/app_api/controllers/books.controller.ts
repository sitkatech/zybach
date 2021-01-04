import { Body, Controller, Post, Route, Request } from "tsoa";
import { BookCreateDto } from "../dtos/book-create-dto";
import { BookService } from "../services/book-service";


@Route("/api/books")
export class BookController extends Controller{
    @Post("")
    public async addBook(@Body() requestBody: BookCreateDto){
        const newBook = await new BookService().add(requestBody);
        return newBook;        
    }
}
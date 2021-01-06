import { Body, Controller, Post, Route, Request, Security } from "tsoa";
import { BookCreateDto } from "../dtos/book-create-dto";
import { DummyFeature } from "../security/dummy-feature";
import { BookService } from "../services/book-service";


@Route("/api/books")
export class BookController extends Controller{
    @Post("")
    @Security("feature", [DummyFeature.ScopeName])
    public async addBook(@Body() requestBody: BookCreateDto){
        const newBook = await new BookService().add(requestBody);
        return newBook;        
    }
}
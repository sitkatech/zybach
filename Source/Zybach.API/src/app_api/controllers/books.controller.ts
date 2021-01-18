import { Body, Controller, Post, Route, Request, Security, Get } from "tsoa";
import { BookCreateDto, BookDto } from "../dtos/book-create-dto";
import { RequestWithUserContext } from "../request-with-user-context";
import { SecurityType } from "../security/authentication";
import { BookService } from "../services/book-service";


@Route("/api/books")
export class BookController extends Controller{
    @Post("")
    @Security(SecurityType.ANONYMOUS)
    public async addBook(@Body() requestBody: BookCreateDto, @Request() req: RequestWithUserContext) : Promise<BookDto>{
        // When the @Security decoration is used with the @Request declaration, the user property of the request is set by 
        // the authentication routine. RequestWithUserContext extends the express.Request interface with a user: UserDto property 
        console.log(req.user);
        const newBook = await new BookService().add(requestBody);
        return newBook;        
    }

    @Get("")
    @Security(SecurityType.ANONYMOUS)
    public async get(@Request() req: RequestWithUserContext) : Promise<BookDto[]>{
        console.log(req.user);
        return await new BookService().get();
    }
}
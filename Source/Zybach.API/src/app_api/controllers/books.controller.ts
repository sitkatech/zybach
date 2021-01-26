import { inject } from "inversify";
import { Body, Controller, Post, Route, Request, Security, Get, Hidden } from "tsoa";
import { provideSingleton } from "../../util/provide-singleton";
import { BookCreateDto, BookDto } from "../dtos/book-create-dto";
import { RequestWithUserContext } from "../request-with-user-context";
import { SecurityType } from "../security/authentication";
import { BookService } from "../services/book-service";


@Route("/api/books")
@provideSingleton(BookController)
@Hidden()
export class BookController extends Controller{
    constructor(@inject(BookService) private bookService: BookService){
        super();
    }

    @Post("")
    @Security(SecurityType.ANONYMOUS)
    public async addBook(@Body() requestBody: BookCreateDto, @Request() req: RequestWithUserContext) : Promise<BookDto>{
        // When the @Security decoration is used with the @Request declaration, the user property of the request is set by 
        // the authentication routine. RequestWithUserContext extends the express.Request interface with a user: UserDto property 
        console.log(req.user);
        const newBook = await this.bookService.add(requestBody);
        return newBook;        
    }

    @Get("")
    @Security(SecurityType.ANONYMOUS)
    public async get(@Request() req: RequestWithUserContext) : Promise<BookDto[]>{
        console.log(req.user);
        return await this.bookService.get();
    }
}
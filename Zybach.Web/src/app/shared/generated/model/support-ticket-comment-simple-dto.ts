/**
 * Zybach.API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: 1.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { UserSimpleDto } from '././user-simple-dto';

export class SupportTicketCommentSimpleDto { 
    SupportTicketCommentID?: number;
    DateCreated?: string;
    DateUpdated?: string;
    CreatorUserID?: number;
    SupportTicketID?: number;
    CommentNotes?: string;
    CreatorUser?: UserSimpleDto;
    constructor(obj?: any) {
        Object.assign(this, obj);
    }
}

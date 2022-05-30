import { Component, OnInit, Input } from '@angular/core';
import { CustomRichTextService } from 'src/app/shared/generated/api/custom-rich-text.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from '../../services/alert.service';
import { Alert } from '../../models/alert';
import { AlertContext } from '../../models/enums/alert-context.enum';
import * as ClassicEditor from 'src/assets/main/ckeditor/ckeditor.js';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CustomRichTextDto } from 'src/app/shared/generated/model/custom-rich-text-dto';

@Component({
  selector: 'custom-rich-text',
  templateUrl: './custom-rich-text.component.html',
  styleUrls: ['./custom-rich-text.component.scss']
})
export class CustomRichTextComponent implements OnInit {
  @Input() customRichTextTypeID: number;
  public customRichTextContent: SafeHtml;
  public isLoading: boolean = true;
  public isEditing: boolean = false;
  public isEmptyContent: boolean = false;
  public watchUserChangeSubscription: any;
  public Editor = ClassicEditor;
  public editedContent: string;
  public editor;

  currentUser: UserDto;

  constructor(
    private customRichTextService: CustomRichTextService,
    private authenticationService: AuthenticationService,
    private alertService: AlertService,
    private sanitizer: DomSanitizer) { }

  ngOnInit() {
    this.authenticationService.getCurrentUser().subscribe(currentUser => {
      this.currentUser = currentUser;
    });

    this.customRichTextService.customRichTextCustomRichTextTypeIDGet(this.customRichTextTypeID).subscribe(x => {
      this.customRichTextContent = this.sanitizer.bypassSecurityTrustHtml(x.CustomRichTextContent);
      this.isEmptyContent = x.IsEmptyContent;
      this.editedContent = x.CustomRichTextContent;
      this.isLoading = false;
    });
  }

  public showEditButton(): boolean {
    return this.authenticationService.isUserAnAdministrator(this.currentUser);
  }

  public enterEdit(): void {
    this.isEditing = true;
  }

  public cancelEdit(): void {
    this.isEditing = false;
  }

  public saveEdit(): void {
    this.isEditing = false;
    this.isLoading = true;
    const updateDto = new CustomRichTextDto({ CustomRichTextContent: this.editedContent });
    this.customRichTextService.customRichTextCustomRichTextTypeIDPut(this.customRichTextTypeID, updateDto).subscribe(x => {
      this.customRichTextContent = this.sanitizer.bypassSecurityTrustHtml(x.CustomRichTextContent);
      this.editedContent = x.CustomRichTextContent;
      this.isLoading = false;
    }, error => {
      this.isLoading = false;
      this.alertService.pushAlert(new Alert("There was an error updating the rich text content", AlertContext.Danger, true));
    });
  }  
}

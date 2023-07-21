import { Component, OnInit, Input, ViewChild, AfterViewChecked } from '@angular/core';
import { CustomRichTextService } from 'src/app/shared/generated/api/custom-rich-text.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserDto } from 'src/app/shared/generated/model/user-dto';
import { AlertService } from '../../services/alert.service';
import { Alert } from '../../models/alert';
import { AlertContext } from '../../models/enums/alert-context.enum';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { CustomRichTextDto } from 'src/app/shared/generated/model/custom-rich-text-dto';
import { EditorComponent } from '@tinymce/tinymce-angular';
import TinyMCEHelpers from '../../helpers/tiny-mce-helpers';

@Component({
  selector: 'custom-rich-text',
  templateUrl: './custom-rich-text.component.html',
  styleUrls: ['./custom-rich-text.component.scss']
})
export class CustomRichTextComponent implements OnInit, AfterViewChecked {
  @ViewChild('tinyMceEditor') tinyMceEditor: EditorComponent;
  public tinyMceConfig: object;
  
  @Input() customRichTextTypeID: number;
  public customRichTextContent: SafeHtml;
  public isLoading: boolean = true;
  public isEditing: boolean = false;
  public isEmptyContent: boolean = false;
  public watchUserChangeSubscription: any;
  public editedContent: string;
  public editor;

  currentUser: UserDto;

  //For media embed https://ckeditor.com/docs/ckeditor5/latest/api/module_media-embed_mediaembed-MediaEmbedConfig.html
  //Only some embeds will work, and if we want others to work we'll likely need to write some extra functions
  public ckConfig = {mediaEmbed: {previewsInData: true}};

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

  ngAfterViewChecked() {
    // viewChild is updated after the view has been checked
    this.initalizeEditor();
  }

  initalizeEditor() {
    if (!this.isLoading && this.isEditing) {
      this.tinyMceConfig = TinyMCEHelpers.DefaultInitConfig(
        this.tinyMceEditor
      );
    }
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

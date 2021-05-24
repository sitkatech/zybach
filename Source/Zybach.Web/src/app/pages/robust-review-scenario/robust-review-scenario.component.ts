import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { WellService } from 'src/app/services/well.service';
import { Alert } from 'src/app/shared/models/alert';
import { AlertContext } from 'src/app/shared/models/enums/alert-context.enum';
import { CustomRichTextType } from 'src/app/shared/models/enums/custom-rich-text-type.enum';
import { UserDetailedDto } from 'src/app/shared/models/user/user-detailed-dto';
import { AlertService } from 'src/app/shared/services/alert.service';

@Component({
  selector: 'zybach-robust-review-scenario',
  templateUrl: './robust-review-scenario.component.html',
  styleUrls: ['./robust-review-scenario.component.scss']
})
export class RobustReviewScenarioComponent implements OnInit {
  public richTextTypeID : number = CustomRichTextType.RobustReviewScenario;
  public fileDownloading : boolean = false;
  
  public watchUserChangeSubscription: any;
  public currentUser: UserDetailedDto;

  constructor(private wellService : WellService,
    private alertService : AlertService,
    private authenticationService : AuthenticationService) { }

  ngOnInit(): void {
    this.watchUserChangeSubscription = this.authenticationService.currentUserSetObservable.subscribe(currentUser => { 
      this.currentUser = currentUser;
  });
  }

  getRobustReviewScenarioJson() {
    this.fileDownloading = true;
    this.wellService.getRobustReviewScenarioJson().subscribe(x => {
      this.fileDownloading = false;
      //Create a fake object for us to click and download
      var a = document.createElement('a');
      a.href = URL.createObjectURL(x);
      const date = new Date();
      const month = ("0" + (date.getMonth() + 1)).slice(-2);
      const day = ("0" + (date.getDate())).slice(-2);
      a.download = `RobustReviewScenario-${date.getFullYear()}${month}${day}.json`;
      document.body.appendChild(a);
      a.click();
      //Revoke the generated url so the blob doesn't hang in memory https://javascript.info/blob
      URL.revokeObjectURL(a.href);
      document.body.removeChild(a);
    }, (() => {
      this.fileDownloading = false;
      this.alertService.pushAlert(new Alert(`There was an error while downloading the file. Please refresh the page and try again.`, AlertContext.Danger));
    }))
  }

}

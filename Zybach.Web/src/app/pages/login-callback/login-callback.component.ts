import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'zybach-login-callback',
  templateUrl: './login-callback.component.html',
  styleUrls: ['./login-callback.component.scss']
})
export class LoginCallbackComponent implements OnInit {

  constructor(private router: Router, private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.getCurrentUserID().subscribe(currentUser => {
      let authRedirectUrl = this.authenticationService.getAuthRedirectUrl();  
      if (authRedirectUrl) {
          this.router.navigateByUrl(authRedirectUrl)
              .then(() => {
                  this.authenticationService.clearAuthRedirectUrl();
              });
        } else{
          this.router.navigate(['/']);
        }
    });
  }
}

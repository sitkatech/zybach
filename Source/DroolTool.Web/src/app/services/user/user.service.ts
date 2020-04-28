import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services';
import { Observable } from 'rxjs';
import { UserCreateDto } from 'src/app/shared/models/user/user-create-dto';
import { UnassignedUserReportDto } from 'src/app/shared/models/user/unassigned-user-report-dto';
import { UserDetailedDto } from 'src/app/shared/models/user/user-detailed-dto';
import { UserEditAccountsDto } from 'src/app/shared/models/user/user-edit-accounts-dto';
import { UserDto } from 'src/app/shared/models/user/user-dto';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    constructor(private apiService: ApiService) { }

    inviteUser(userInviteDto: any): Observable<UserDto> {
        let route = `/users/invite`;
        return this.apiService.postToApi(route, userInviteDto);
    }

    createNewUser(userCreateDto: UserCreateDto): Observable<UserDto> {
        let route = `/users/`;
        return this.apiService.postToApi(route, userCreateDto);
    }

    getUsers(): Observable<UserDetailedDto[]> {
        let route = `/users`;
        return this.apiService.getFromApi(route);
    }

    getUserFromUserID(userID: number): Observable<UserDto> {
        let route = `/users/${userID}`;
        return this.apiService.getFromApi(route);
    }

    getUserFromGlobalID(globalID: string): Observable<UserDto> {
        let route = `/user-claims/${globalID}`;
        return this.apiService.getFromApi(route);
    }

    updateUser(userID: number, userUpdateDto: any): Observable<UserDto> {
        let route = `/users/${userID}`;
        return this.apiService.putToApi(route, userUpdateDto);
    }
    
    editAccounts(userID: number, userEditAccountsDto: UserEditAccountsDto): Observable<UserDto> {
        let route = `/users/${userID}/edit-accounts`;
        return this.apiService.putToApi(route, userEditAccountsDto);
    }

    getLandownerUsageReportByYear(year: number): Observable<UserDto[]> {
        let route = `/landowner-usage-report/${year}`;
        return this.apiService.getFromApi(route);
    }

    getUnassignedUserReport(): Observable<UnassignedUserReportDto> {
        let route = `/users/unassigned-report`;
        return this.apiService.getFromApi(route);
    }

    setDisclaimerAcknowledgedDate(userID: number): Observable<UserDto> {
        let route = `/users/set-disclaimer-acknowledged-date`
        return this.apiService.putToApi(route, userID);
    }
}

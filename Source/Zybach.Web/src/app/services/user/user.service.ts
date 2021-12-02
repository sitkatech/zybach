import { Injectable } from '@angular/core';
import { ApiService } from 'src/app/shared/services';
import { Observable } from 'rxjs';
import { UnassignedUserReportDto } from 'src/app/shared/generated/model/unassigned-user-report-dto';
import { UserCreateDto } from 'src/app/shared/generated/model/user-create-dto';
import { UserDto } from 'src/app/shared/generated/model/user-dto';

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

    getUsers(): Observable<UserDto[]> {
        let route = `/users`;
        return this.apiService.getFromApi(route);
    }

    getUserFromUserID(userID: number): Observable<UserDto> {
        let route = `/users/${userID}`;
        return this.apiService.getFromApi(route);
    }

    getUserFromGlobalID(globalID: string): Observable<UserDto> {
        let route = `/users/user-claims/${globalID}`;
        return this.apiService.getFromApi(route);
    }

    updateUser(userID: number, userUpdateDto: any): Observable<UserDto> {
        let route = `/users/${userID}`;
        return this.apiService.putToApi(route, userUpdateDto);
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
